namespace ProofOfReserve.MerkleTree
{
    /// <summary>
    /// Represents a Merkle Tree node
    /// </summary>
    public class MerkleNode
    {
        /// <summary>
        /// Gets the hash of this node
        /// </summary>
        public byte[] Hash { get; }

        /// <summary>
        /// Gets the left child node
        /// </summary>
        public MerkleNode? Left { get; }

        /// <summary>
        /// Gets the right child node
        /// </summary>
        public MerkleNode? Right { get; }

        /// <summary>
        /// Gets a value indicating whether this node is a leaf node
        /// </summary>
        public bool IsLeaf => Left == null && Right == null;

        /// <summary>
        /// Initializes a new instance of the <see cref="MerkleNode"/> class
        /// </summary>
        /// <param name="hash">The hash of this node</param>
        /// <param name="left">The left child node</param>
        /// <param name="right">The right child node</param>
        public MerkleNode(byte[] hash, MerkleNode? left = null, MerkleNode? right = null)
        {
            Hash = hash;
            Left = left;
            Right = right;
        }

        /// <summary>
        /// Gets the hash as a hex string
        /// </summary>
        /// <returns>The hash as a hex string</returns>
        public string GetHashAsHex()
        {
            return BIP340HashUtility.ToHexString(Hash);
        }
    }

    /// <summary>
    /// Represents a proof that a leaf is part of a Merkle Tree
    /// </summary>
    public class MerkleProof
    {
        /// <summary>
        /// Gets the leaf data
        /// </summary>
        public string LeafData { get; }

        /// <summary>
        /// Gets the proof elements
        /// </summary>
        public List<(string Hash, bool IsLeftSide)> Elements { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MerkleProof"/> class
        /// </summary>
        /// <param name="leafData">The leaf data</param>
        public MerkleProof(string leafData)
        {
            LeafData = leafData;
            Elements = new List<(string, bool)>();
        }

        /// <summary>
        /// Adds a proof element
        /// </summary>
        /// <param name="hash">The hash of the sibling node</param>
        /// <param name="isLeftSide">Whether the sibling node is on the left side</param>
        public void AddElement(string hash, bool isLeftSide)
        {
            Elements.Add((hash, isLeftSide));
        }
    }

    /// <summary>
    /// Implements a Merkle Tree that follows Bitcoin block transaction patterns
    /// </summary>
    public class MerkleTree
    {
        private readonly MerkleNode? _root;
        private readonly List<MerkleNode> _leaves;
        private readonly Dictionary<string, int> _leafIndices;
        private readonly string _hashTag;
        private readonly string _branchHashTag;

        /// <summary>
        /// Initializes a new instance of the <see cref="MerkleTree"/> class
        /// </summary>
        /// <param name="data">The data to include in the tree</param>
        /// <param name="hashTag">The tag to use for leaf hashing</param>
        /// <param name="branchHashTag">The tag to use for branch hashing</param>
        public MerkleTree(IEnumerable<string> data, string hashTag = "Bitcoin_Transaction", string branchHashTag = "Bitcoin_Transaction")
        {
            _hashTag = hashTag;
            _branchHashTag = branchHashTag;
            _leaves = new List<MerkleNode>();
            _leafIndices = new Dictionary<string, int>();

            // Create leaf nodes
            int index = 0;
            foreach (var item in data)
            {
                byte[] hash;
                if (hashTag == "Bitcoin_Transaction")
                {
                    hash = BIP340HashUtility.HashBitcoinTransaction(item);
                }
                else if (hashTag == "ProofOfReserve_Leaf")
                {
                    hash = BIP340HashUtility.HashProofOfReserveLeaf(item);
                }
                else
                {
                    hash = BIP340HashUtility.TaggedHash(hashTag, item);
                }

                var node = new MerkleNode(hash);
                _leaves.Add(node);
                _leafIndices[item] = index;
                index++;
            }

            // Build the tree
            _root = BuildTree(_leaves);
        }

        /// <summary>
        /// Gets the root of the Merkle Tree
        /// </summary>
        public MerkleNode? Root => _root;

        /// <summary>
        /// Gets the Merkle root as a hex string
        /// </summary>
        /// <returns>The Merkle root as a hex string</returns>
        public string? GetRootHashAsHex()
        {
            return _root?.GetHashAsHex();
        }

        private MerkleNode? BuildTree(List<MerkleNode> nodes)
        {
            if (nodes.Count == 0)
            {
                // Empty tree
                return null;
            }

            if (nodes.Count == 1)
            {
                // Single node tree
                return nodes[0];
            }

            List<MerkleNode> parents = new List<MerkleNode>();

            // Process nodes pairwise
            for (int i = 0; i < nodes.Count; i += 2)
            {
                MerkleNode right = (i + 1 < nodes.Count) ? nodes[i + 1] : nodes[i]; // Duplicate last node if odd
                
                byte[] parentHash;
                if (_branchHashTag == "Bitcoin_Transaction")
                {
                    // Concatenate left and right hashes
                    byte[] combined = BIP340HashUtility.ConcatenateBytes(nodes[i].Hash, right.Hash);
                    parentHash = BIP340HashUtility.HashBitcoinTransaction(combined);
                }
                else if (_branchHashTag == "ProofOfReserve_Branch")
                {
                    // Concatenate left and right hashes
                    byte[] combined = BIP340HashUtility.ConcatenateBytes(nodes[i].Hash, right.Hash);
                    parentHash = BIP340HashUtility.HashProofOfReserveBranch(combined);
                }
                else
                {
                    // Concatenate left and right hashes
                    byte[] combined = BIP340HashUtility.ConcatenateBytes(nodes[i].Hash, right.Hash);
                    parentHash = BIP340HashUtility.TaggedHash(_branchHashTag, combined);
                }

                MerkleNode parent = new MerkleNode(parentHash, nodes[i], right);
                parents.Add(parent);
            }

            // Recurse to build the next level
            return BuildTree(parents);
        }

        /// <summary>
        /// Generates a Merkle proof for the given data
        /// </summary>
        /// <param name="data">The data to generate a proof for</param>
        /// <returns>The Merkle proof</returns>
        public MerkleProof GenerateProof(string data)
        {
            if (!_leafIndices.TryGetValue(data, out int leafIndex))
            {
                throw new ArgumentException("Data not found in the Merkle tree", nameof(data));
            }

            MerkleProof proof = new MerkleProof(data);
            GenerateProofHelper(_leaves, leafIndex, proof);
            return proof;
        }

        private void GenerateProofHelper(List<MerkleNode> nodes, int nodeIndex, MerkleProof proof)
        {
            if (nodes.Count <= 1)
            {
                return;
            }

            List<MerkleNode> parents = new List<MerkleNode>();

            // Process nodes pairwise
            for (int i = 0; i < nodes.Count; i += 2)
            {
                MerkleNode right = (i + 1 < nodes.Count) ? nodes[i + 1] : nodes[i]; // Duplicate last node if odd
                
                // Hash the parent node for the next level
                byte[] parentHash;
                if (_branchHashTag == "Bitcoin_Transaction")
                {
                    byte[] combined = BIP340HashUtility.ConcatenateBytes(nodes[i].Hash, right.Hash);
                    parentHash = BIP340HashUtility.HashBitcoinTransaction(combined);
                }
                else if (_branchHashTag == "ProofOfReserve_Branch")
                {
                    byte[] combined = BIP340HashUtility.ConcatenateBytes(nodes[i].Hash, right.Hash);
                    parentHash = BIP340HashUtility.HashProofOfReserveBranch(combined);
                }
                else
                {
                    byte[] combined = BIP340HashUtility.ConcatenateBytes(nodes[i].Hash, right.Hash);
                    parentHash = BIP340HashUtility.TaggedHash(_branchHashTag, combined);
                }

                parents.Add(new MerkleNode(parentHash));

                // If this is the pair that includes our target node, add the sibling to the proof
                if (i == nodeIndex || (i + 1 == nodeIndex && i + 1 < nodes.Count))
                {
                    if (i == nodeIndex)
                    {
                        // Target is left node, sibling is right
                        proof.AddElement(BIP340HashUtility.ToHexString(right.Hash), false);
                    }
                    else
                    {
                        // Target is right node, sibling is left
                        proof.AddElement(BIP340HashUtility.ToHexString(nodes[i].Hash), true);
                    }

                    // Update the nodeIndex for the next level
                    nodeIndex = i / 2;
                }
            }

            // Recurse to the next level
            GenerateProofHelper(parents, nodeIndex, proof);
        }

        /// <summary>
        /// Verifies a Merkle proof against the Merkle root
        /// </summary>
        /// <param name="proof">The Merkle proof</param>
        /// <param name="root">The expected Merkle root</param>
        /// <returns>True if the proof is valid, false otherwise</returns>
        public static bool VerifyProof(MerkleProof proof, string root, string hashTag = "Bitcoin_Transaction", string branchHashTag = "Bitcoin_Transaction")
        {
            // Hash the leaf data
            byte[] currentHash;
            if (hashTag == "Bitcoin_Transaction")
            {
                currentHash = BIP340HashUtility.HashBitcoinTransaction(proof.LeafData);
            }
            else if (hashTag == "ProofOfReserve_Leaf")
            {
                currentHash = BIP340HashUtility.HashProofOfReserveLeaf(proof.LeafData);
            }
            else
            {
                currentHash = BIP340HashUtility.TaggedHash(hashTag, proof.LeafData);
            }

            // Apply each proof element
            foreach (var (siblingHashHex, isLeftSide) in proof.Elements)
            {
                byte[] siblingHash = StringToByteArray(siblingHashHex);
                
                byte[] combinedHash;
                if (isLeftSide)
                {
                    // Sibling is on the left
                    combinedHash = BIP340HashUtility.ConcatenateBytes(siblingHash, currentHash);
                }
                else
                {
                    // Sibling is on the right
                    combinedHash = BIP340HashUtility.ConcatenateBytes(currentHash, siblingHash);
                }

                // Hash the combined value
                if (branchHashTag == "Bitcoin_Transaction")
                {
                    currentHash = BIP340HashUtility.HashBitcoinTransaction(combinedHash);
                }
                else if (branchHashTag == "ProofOfReserve_Branch")
                {
                    currentHash = BIP340HashUtility.HashProofOfReserveBranch(combinedHash);
                }
                else
                {
                    currentHash = BIP340HashUtility.TaggedHash(branchHashTag, combinedHash);
                }
            }

            // Compare with the expected root
            return BIP340HashUtility.ToHexString(currentHash) == root;
        }

        private static byte[] StringToByteArray(string hex)
        {
            int length = hex.Length;
            byte[] bytes = new byte[length / 2];
            for (int i = 0; i < length; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            }
            return bytes;
        }
    }
} 