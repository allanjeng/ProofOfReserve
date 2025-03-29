namespace ProofOfReserve.MerkleTree
{
    /// <summary>
    /// Provides examples of using the Merkle Tree library
    /// </summary>
    public static class MerkleTreeExample
    {
        /// <summary>
        /// Demonstrates calculating a Merkle root for the example data
        /// </summary>
        /// <returns>The Merkle root</returns>
        public static string CalculateExampleMerkleRoot()
        {
            // Example data from requirements
            string[] data = ["aaa", "bbb", "ccc", "ddd", "eee"];
            
            // Create a Merkle tree with the example data
            MerkleTree tree = new(data, "Bitcoin_Transaction", "Bitcoin_Transaction");
            
            // Return the Merkle root
            return tree.GetRootHashAsHex() ?? string.Empty;
        }

        /// <summary>
        /// Demonstrates generating a Merkle proof for the example data
        /// </summary>
        /// <param name="data">The data to generate a proof for</param>
        /// <returns>The Merkle proof</returns>
        public static MerkleProof GenerateExampleProof(string data)
        {
            // Example data from requirements
            string[] items = ["aaa", "bbb", "ccc", "ddd", "eee"];
            
            // Create a Merkle tree with the example data
            MerkleTree tree = new(items, "Bitcoin_Transaction", "Bitcoin_Transaction");
            
            // Generate a proof for the specified data
            return tree.GenerateProof(data);
        }

        /// <summary>
        /// Demonstrates verifying a Merkle proof
        /// </summary>
        /// <param name="proof">The Merkle proof</param>
        /// <param name="root">The expected Merkle root</param>
        /// <returns>True if the proof is valid, false otherwise</returns>
        public static bool VerifyExampleProof(MerkleProof proof, string root)
        {
            return MerkleTree.VerifyProof(proof, root, "Bitcoin_Transaction", "Bitcoin_Transaction");
        }

        /// <summary>
        /// Provides a simple demonstration of the Merkle Tree library
        /// </summary>
        public static void RunExample()
        {
            Console.WriteLine("Merkle Tree Example");
            Console.WriteLine("-------------------");
            
            // Example data from requirements
            string[] data = new string[] { "aaa", "bbb", "ccc", "ddd", "eee" };
            Console.WriteLine($"Example data: {string.Join(", ", data)}");
            
            // Calculate the Merkle root
            string root = CalculateExampleMerkleRoot();
            Console.WriteLine($"Merkle root: {root}");
            
            // Generate a proof for "ccc"
            string targetData = "ccc";
            MerkleProof proof = GenerateExampleProof(targetData);
            Console.WriteLine($"Proof for '{targetData}':");
            Console.WriteLine($"  Leaf data: {proof.LeafData}");
            Console.WriteLine("  Proof elements:");
            foreach (var (hash, isLeftSide) in proof.Elements)
            {
                Console.WriteLine($"    Hash: {hash}, Is Left: {isLeftSide}");
            }
            
            // Verify the proof
            bool isValid = VerifyExampleProof(proof, root);
            Console.WriteLine($"Proof is valid: {isValid}");
        }
    }
} 