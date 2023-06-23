using Dapper;
using LoginApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace LoginApp.Controllers
{
    internal class Customers
    {
        public string? Username { get; internal set; }
        public string? Password { get; internal set; }
    }
}


