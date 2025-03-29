using System.Security.Cryptography;
using System.Text;

namespace ProofOfReserve.MerkleTree
{
    /// <summary>
    /// Implements BIP340 compatible tagged hash functions
    /// </summary>
    public static class BIP340HashUtility
    {
        /// <summary>
        /// Computes a tagged hash using SHA256 according to BIP340.
        /// Tagged hash is defined as SHA256(SHA256(tag) || SHA256(tag) || message)
        /// </summary>
        /// <param name="tag">The tag to use for the hash</param>
        /// <param name="message">The message to hash</param>
        /// <returns>The tagged hash as a byte array</returns>
        public static byte[] TaggedHash(string tag, byte[] message)
        {
            // Calculate SHA256(tag)
            byte[] tagBytes = Encoding.UTF8.GetBytes(tag);
            byte[] tagHash = SHA256.HashData(tagBytes);

            // Concatenate SHA256(tag) || SHA256(tag) || message
            byte[] buffer = new byte[tagHash.Length + tagHash.Length + message.Length];
            Buffer.BlockCopy(tagHash, 0, buffer, 0, tagHash.Length);
            Buffer.BlockCopy(tagHash, 0, buffer, tagHash.Length, tagHash.Length);
            Buffer.BlockCopy(message, 0, buffer, tagHash.Length * 2, message.Length);

            // Calculate SHA256(SHA256(tag) || SHA256(tag) || message)
            return SHA256.HashData(buffer);
        }

        /// <summary>
        /// Computes a tagged hash using SHA256 according to BIP340.
        /// Tagged hash is defined as SHA256(SHA256(tag) || SHA256(tag) || message)
        /// </summary>
        /// <param name="tag">The tag to use for the hash</param>
        /// <param name="message">The message to hash as a string</param>
        /// <returns>The tagged hash as a byte array</returns>
        public static byte[] TaggedHash(string tag, string message)
        {
            return TaggedHash(tag, Encoding.UTF8.GetBytes(message));
        }

        /// <summary>
        /// Computes a tagged hash for Bitcoin transactions
        /// </summary>
        /// <param name="message">The message to hash</param>
        /// <returns>The tagged hash as a byte array</returns>
        public static byte[] HashBitcoinTransaction(string message)
        {
            return TaggedHash("Bitcoin_Transaction", message);
        }

        /// <summary>
        /// Computes a tagged hash for Bitcoin transactions
        /// </summary>
        /// <param name="message">The message to hash as bytes</param>
        /// <returns>The tagged hash as a byte array</returns>
        public static byte[] HashBitcoinTransaction(byte[] message)
        {
            return TaggedHash("Bitcoin_Transaction", message);
        }

        /// <summary>
        /// Computes a tagged hash for Proof of Reserve leaf nodes
        /// </summary>
        /// <param name="message">The message to hash</param>
        /// <returns>The tagged hash as a byte array</returns>
        public static byte[] HashProofOfReserveLeaf(string message)
        {
            return TaggedHash("ProofOfReserve_Leaf", message);
        }

        /// <summary>
        /// Computes a tagged hash for Proof of Reserve leaf nodes
        /// </summary>
        /// <param name="message">The message to hash as bytes</param>
        /// <returns>The tagged hash as a byte array</returns>
        public static byte[] HashProofOfReserveLeaf(byte[] message)
        {
            return TaggedHash("ProofOfReserve_Leaf", message);
        }

        /// <summary>
        /// Computes a tagged hash for Proof of Reserve branch nodes
        /// </summary>
        /// <param name="message">The message to hash</param>
        /// <returns>The tagged hash as a byte array</returns>
        public static byte[] HashProofOfReserveBranch(string message)
        {
            return TaggedHash("ProofOfReserve_Branch", message);
        }

        /// <summary>
        /// Computes a tagged hash for Proof of Reserve branch nodes
        /// </summary>
        /// <param name="message">The message to hash as bytes</param>
        /// <returns>The tagged hash as a byte array</returns>
        public static byte[] HashProofOfReserveBranch(byte[] message)
        {
            return TaggedHash("ProofOfReserve_Branch", message);
        }

        /// <summary>
        /// Converts a byte array to a hex string
        /// </summary>
        /// <param name="bytes">The byte array to convert</param>
        /// <returns>The hex string representation</returns>
        public static string ToHexString(byte[] bytes)
        {
            return BitConverter.ToString(bytes).Replace("-", "").ToLowerInvariant();
        }

        /// <summary>
        /// Concatenates two byte arrays
        /// </summary>
        /// <param name="first">First byte array</param>
        /// <param name="second">Second byte array</param>
        /// <returns>Concatenated byte array</returns>
        public static byte[] ConcatenateBytes(byte[] first, byte[] second)
        {
            byte[] result = new byte[first.Length + second.Length];
            Buffer.BlockCopy(first, 0, result, 0, first.Length);
            Buffer.BlockCopy(second, 0, result, first.Length, second.Length);
            return result;
        }
    }
} 