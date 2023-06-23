using Dapper;
using LoginApp.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace LoginApp
{
    public class CustomerRepository
    {
        private readonly string connectionString;

        public CustomerRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public List<Customer> GetAllCustomers()
        {
            using IDbConnection db = new SqlConnection(connectionString);
            return db.Query<Customer>("SELECT * FROM Customer").ToList();
        }

        public Customer GetCustomerById(int? id)
        {
            using IDbConnection db = new SqlConnection(connectionString);
            return db.QueryFirstOrDefault<Customer>("SELECT * FROM Customer WHERE Id = @Id", new { Id = id });
        }

        public bool CreateCustomer(Customer customer)
        {
            using IDbConnection db = new SqlConnection(connectionString);
            string query = "INSERT INTO Customer (Name, Address, Email) VALUES (@Name, @Address, @Email)";
            int rowsAffected = db.Execute(query, customer);
            return rowsAffected > 0;
        }

        public bool UpdateCustomer(Customer customer)
        {
            using IDbConnection db = new SqlConnection(connectionString);
            string query = "UPDATE Customer SET Name = @Name, Address = @Address, Email = @Email WHERE Id = @Id";
            int rowsAffected = db.Execute(query, customer);
            return rowsAffected > 0;
        }

        public bool DeleteCustomer(int id)
        {
            using IDbConnection db = new SqlConnection(connectionString);
            string query = "DELETE FROM Customer WHERE Id = @Id";
            int rowsAffected = db.Execute(query, new { Id = id });
            return rowsAffected > 0;
        }

        public bool CustomerExists(int id)
        {
            using IDbConnection db = new SqlConnection(connectionString);
            string query = "SELECT COUNT(*) FROM Customer WHERE Id = @Id";
            int count = db.ExecuteScalar<int>(query, new { Id = id });
            return count > 0;
        }
    }
}


