using ProofOfReserve.MerkleTree;
using ProofOfReserve.Models;
using System.Collections.Generic;
using System.Linq;

namespace ProofOfReserve.Services;

/// <summary>
/// Service for generating Merkle root and proofs for Proof of Reserve
/// </summary>
public class ProofOfReserveService
{
    private readonly UserService _userService;
    private MerkleTree.MerkleTree? _merkleTree;

    /// <summary>
    /// Initializes a new instance of the <see cref="ProofOfReserveService"/> class
    /// </summary>
    /// <param name="userService">The user service</param>
    public ProofOfReserveService(UserService userService)
    {
        _userService = userService;
        InitializeMerkleTree();
    }

    /// <summary>
    /// Initializes the Merkle tree with user data
    /// </summary>
    private void InitializeMerkleTree()
    {
        var userData = _userService.GetUserDataAsStrings();
        _merkleTree = new MerkleTree.MerkleTree(userData, "ProofOfReserve_Leaf", "ProofOfReserve_Branch");
    }

    /// <summary>
    /// Gets the Merkle root of all users
    /// </summary>
    /// <returns>The Merkle root</returns>
    public string? GetMerkleRoot()
    {
        return _merkleTree?.GetRootHashAsHex();
    }

    /// <summary>
    /// Generates a Merkle proof for a user
    /// </summary>
    /// <param name="userId">The user ID</param>
    /// <returns>The Merkle proof data</returns>
    public MerkleProofDto? GenerateProofForUser(int userId)
    {
        if (_merkleTree == null)
        {
            return null;
        }
        
        var user = _userService.GetUserById(userId);
        if (user == null)
        {
            return null;
        }

        var userData = user.ToString();
        var proof = _merkleTree.GenerateProof(userData);

        return new MerkleProofDto
        {
            UserBalance = user.Balance,
            ProofElements = proof.Elements.Select(e => new ProofElementDto
            {
                Hash = e.Hash,
                Direction = e.IsLeftSide ? 0 : 1
            }).ToList()
        };
    }
}

/// <summary>
/// Data transfer object for Merkle proof
/// </summary>
public class MerkleProofDto
{
    /// <summary>
    /// Gets or sets the user balance
    /// </summary>
    public int UserBalance { get; set; }

    /// <summary>
    /// Gets or sets the proof elements
    /// </summary>
    public List<ProofElementDto> ProofElements { get; set; } = new List<ProofElementDto>();
}

/// <summary>
/// Data transfer object for a Merkle proof element
/// </summary>
public class ProofElementDto
{
    /// <summary>
    /// Gets or sets the hash
    /// </summary>
    public string Hash { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the direction (0 for left, 1 for right)
    /// </summary>
    public int Direction { get; set; }
} 