using Xunit;

namespace ProofOfReserve.MerkleTree.Tests
{
    public class MerkleTreeTests
    {
        [Fact]
        public void TestEvenNumberOfNodes()
        {
            // Arrange
            string[] data = new string[] { "aaa", "bbb", "ccc", "ddd" };
            
            // Act
            var tree = new ProofOfReserve.MerkleTree.MerkleTree(data);
            var root = tree.GetRootHashAsHex();
            
            // Assert
            Assert.NotNull(root);
            Assert.NotEmpty(root);

            // Verify each node has a valid proof
            foreach (var item in data)
            {
                var proof = tree.GenerateProof(item);
                Assert.True(ProofOfReserve.MerkleTree.MerkleTree.VerifyProof(proof, root));
            }
        }

        [Fact]
        public void TestOddNumberOfNodes()
        {
            // Arrange
            string[] data = new string[] { "aaa", "bbb", "ccc", "ddd", "eee" };
            
            // Act
            var tree = new ProofOfReserve.MerkleTree.MerkleTree(data);
            var root = tree.GetRootHashAsHex();
            
            // Assert
            Assert.NotNull(root);
            Assert.NotEmpty(root);

            // Verify each node has a valid proof
            foreach (var item in data)
            {
                var proof = tree.GenerateProof(item);
                Assert.True(ProofOfReserve.MerkleTree.MerkleTree.VerifyProof(proof, root));
            }
        }

        [Fact]
        public void TestSingleNode()
        {
            // Arrange
            string[] data = new string[] { "aaa" };
            
            // Act
            var tree = new ProofOfReserve.MerkleTree.MerkleTree(data);
            var root = tree.GetRootHashAsHex();
            
            // Assert
            Assert.NotNull(root);
            Assert.NotEmpty(root);

            // Verify the single node has a valid proof
            var proof = tree.GenerateProof(data[0]);
            Assert.True(ProofOfReserve.MerkleTree.MerkleTree.VerifyProof(proof, root));
            Assert.Empty(proof.Elements); // Single node should have no proof elements
        }

        [Fact]
        public void TestEmptyTree()
        {
            // Arrange
            string[] data = Array.Empty<string>();
            
            // Act
            var tree = new ProofOfReserve.MerkleTree.MerkleTree(data);
            var root = tree.GetRootHashAsHex();
            
            // Assert
            Assert.Null(root); // Empty tree should return null root

            // Verify attempting to generate a proof throws an exception
            var ex = Assert.Throws<ArgumentException>(() => tree.GenerateProof("any"));
            Assert.Contains("Data not found", ex.Message);
        }

        [Fact]
        public void TestProofVerificationWithDifferentRoot()
        {
            // Arrange
            string[] data = new string[] { "aaa", "bbb", "ccc", "ddd" };
            var tree = new ProofOfReserve.MerkleTree.MerkleTree(data);
            var proof = tree.GenerateProof("aaa");
            
            // Act & Assert
            Assert.False(ProofOfReserve.MerkleTree.MerkleTree.VerifyProof(proof, "invalid_root"));
        }

        [Fact]
        public void TestDuplicateLastNodeForOddNumberOfNodes()
        {
            // Arrange
            string[] data = new string[] { "aaa", "bbb", "ccc" };
            var tree = new ProofOfReserve.MerkleTree.MerkleTree(data);
            var root = tree.GetRootHashAsHex();

            // Create another tree with the last node duplicated explicitly
            string[] dataWithDuplicate = new string[] { "aaa", "bbb", "ccc", "ccc" };
            var treeWithDuplicate = new ProofOfReserve.MerkleTree.MerkleTree(dataWithDuplicate);
            var rootWithDuplicate = treeWithDuplicate.GetRootHashAsHex();

            // Assert
            Assert.Equal(root, rootWithDuplicate); // Both trees should produce the same root
        }
    }
} 