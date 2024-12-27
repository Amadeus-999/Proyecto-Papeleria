using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace proyecto
{
    public class DatabaseService
    {
        private string connectionString = "Server=localhost\\PAPELERIA;Database=papeleria;User=gaby;Password=183492765;Trusted_Connection=False;TrustServerCertificate=True;";

        public SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }
    }

}
