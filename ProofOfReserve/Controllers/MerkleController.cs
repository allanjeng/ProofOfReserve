using Microsoft.AspNetCore.Mvc;
using ProofOfReserve.Services;

namespace ProofOfReserve.Controllers;

/// <summary>
/// Controller for Proof of Reserve API endpoints
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class MerkleController : ControllerBase
{
    private readonly ProofOfReserveService _proofOfReserveService;
    private readonly UserService _userService;

    public MerkleController(ProofOfReserveService proofOfReserveService, UserService userService)
    {
        _proofOfReserveService = proofOfReserveService;
        _userService = userService;
    }

    /// <summary>
    /// Gets the Merkle root of all users
    /// </summary>
    /// <returns>The Merkle root</returns>
    [HttpGet("root")]
    public IActionResult GetMerkleRoot()
    {
        try
        {
            var root = _proofOfReserveService.GetMerkleRoot();
            return Ok(new { MerkleRoot = root });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = $"Error getting Merkle root: {ex.Message}" });
        }
    }

    /// <summary>
    /// Gets the Merkle proof for a user
    /// </summary>
    /// <param name="userId">The user ID</param>
    /// <returns>The Merkle proof</returns>
    [HttpGet("proof/{userId}")]
    public IActionResult GetMerkleProof(int userId)
    {
        try
        {
            var proof = _proofOfReserveService.GenerateProofForUser(userId);
            if (proof == null)
            {
                return NotFound(new { Message = $"User with ID {userId} not found" });
            }

            return Ok(proof);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = $"Error getting Merkle proof: {ex.Message}" });
        }
    }

    /// <summary>
    /// Gets all users
    /// </summary>
    /// <returns>All users</returns>
    [HttpGet("users")]
    public IActionResult GetAllUsers()
    {
        try
        {
            var users = _userService.GetAllUsers();
            return Ok(users);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = $"Error getting users: {ex.Message}" });
        }
    }
} 