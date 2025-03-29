using ProofOfReserve.Models;
using System.Collections.Generic;
using System.Linq;

namespace ProofOfReserve.Services;

/// <summary>
/// Service for managing user data
/// </summary>
public class UserService
{
    private readonly List<User> _users;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserService"/> class
    /// </summary>
    public UserService()
    {
        // Initialize with example data from requirements
        _users = new List<User>
        {
            new User(1, 1111),
            new User(2, 2222),
            new User(3, 3333),
            new User(4, 4444),
            new User(5, 5555),
            new User(6, 6666),
            new User(7, 7777),
            new User(8, 8888)
        };
    }

    /// <summary>
    /// Gets all users
    /// </summary>
    /// <returns>All users</returns>
    public IEnumerable<User> GetAllUsers()
    {
        return _users;
    }

    /// <summary>
    /// Gets a user by ID
    /// </summary>
    /// <param name="id">The user ID</param>
    /// <returns>The user, or null if not found</returns>
    public User? GetUserById(int id)
    {
        return _users.FirstOrDefault(u => u.Id == id);
    }

    /// <summary>
    /// Gets all user data as strings for Merkle tree processing
    /// </summary>
    /// <returns>All user data as strings</returns>
    public IEnumerable<string> GetUserDataAsStrings()
    {
        return _users.Select(u => u.ToString());
    }
} 