namespace ProofOfReserve.Models
{
    /// <summary>
    /// Represents a user with an ID and balance
    /// </summary>
    public class User
    {
        /// <summary>
        /// Gets or sets the user ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the user balance in JPY
        /// </summary>
        public int Balance { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="User"/> class
        /// </summary>
        public User()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="User"/> class
        /// </summary>
        /// <param name="id">The user ID</param>
        /// <param name="balance">The user balance in JPY</param>
        public User(int id, int balance)
        {
            Id = id;
            Balance = balance;
        }

        /// <summary>
        /// Returns a serialized representation of the user as per the requirements
        /// Format: "(id,balance)" without spaces
        /// </summary>
        /// <returns>The serialized user</returns>
        public override string ToString()
        {
            return $"({Id},{Balance})";
        }
    }
} 