# Proof of Reserve Implementation

A simple way to verify user balances using Merkle Trees. Built with .NET 8.

## Overview

This project has two main parts:
1. A library that creates Merkle Trees (like Bitcoin uses)
2. A web API that lets users verify their balances

## Project Structure

### Core Library (ProofOfReserve.MerkleTree)
- Creates Merkle Trees
- Uses BIP340 hashing
- Generates and checks proofs

### Web API (ProofOfReserve)
- Shows total balance root hash
- Lets users verify their balance
- Stores user data in memory
- Has API docs (Swagger)

### Tests (ProofOfReserve.MerkleTree.Tests)
- Tests the core library
- Checks edge cases
- Verifies proofs work

### Example App (ProofOfReserve.MerkleTree.TestConsole)
- Shows how to use the library
- Runs sample data
- Displays results

## Features

### Library Features
- Makes Merkle Trees from strings
- Uses secure hashing (BIP340)
- Creates and checks proofs
- Works with any number of items

### API Features
- Gets root hash of all balances
- Makes proofs for user balances
- Uses simple format: "(1,100)"
- Uses UTF8 encoding

## Requirements

- .NET 8 SDK
- Visual Studio 2022 or VS Code

## Getting Started

1. Clone the repository
```bash
git clone [repository-url]
```

2. Build the solution
```bash
dotnet build
```

3. Run the tests
```bash
dotnet test
```

4. Run the example console application
```bash
cd ProofOfReserve.MerkleTree.TestConsole
dotnet run
```

5. Run the Web API
```bash
cd ProofOfReserve
dotnet run
```

## API Endpoints

### Get Root Hash
```
GET /api/merkle/root
```
Shows the root hash of all balances.

### Get User Proof
```
GET /api/merkle/proof/{userId}
```
Shows proof for one user's balance.

## Example Data

The system includes example user data:
```
(1,1111)
(2,2222)
(3,3333)
(4,4444)
(5,5555)
(6,6666)
(7,7777)
(8,8888)
```

## Testing

Run the test suite:
```bash
dotnet test
```

The test suite includes:
- Even number of nodes
- Odd number of nodes
- Single node trees
- Empty trees
- Proof verification
- Edge cases

## Future Improvements

See [IMPROVEMENTS.md](IMPROVEMENTS.md) for a comprehensive list of suggested improvements for production deployment.