using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;

namespace SeenDAL.Infrastructure
{
    public sealed class DatabaseHelper : IDatabaseHelper
    {
        private readonly string _connectionString;
        private static DatabaseHelper? _instance;
        private static readonly object _lock = new object();

        // Constructor for DI
        public DatabaseHelper(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") 
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        }

        // Senior approach: In modern .NET, Singleton is usually handled by DI.
        // However, the user specifically asked for a Singleton pattern implementation.
        public static void Initialize(IConfiguration configuration)
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new DatabaseHelper(configuration);
                    }
                }
            }
        }

        public static DatabaseHelper Instance
        {
            get
            {
                if (_instance == null)
                {
                    throw new InvalidOperationException("DatabaseHelper must be initialized with IConfiguration before use.");
                }
                return _instance;
            }
        }

        public SqlConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}
