using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace AliceAPI
{
    public class DB
    {
        MySqlConnection connection = new MySqlConnection(getcon());

        /// <summary>String for connection with DB. Change port, username and password to connect</summary>
        /// <returns>return striing</returns>
        private static string getcon()
        {
            return "server=84.201.140.246;port=3306;charset=utf8;username=alice;password=Alice_123;";
        }

        /// <summary>Opens the connection.</summary>
        public void openConnection()
        {
            if (connection.State == System.Data.ConnectionState.Closed)
                connection.Open();
        }

        /// <summary>Closes the connection.</summary>
        public void closeConnection()
        {
            if (connection.State == System.Data.ConnectionState.Open)
                connection.Close();
        }

        /// <summary>Gets the connection.</summary>
        /// <returns>return connection to DB</returns>
        public MySqlConnection getConnection()
        {
            return connection;
        }
        


    }
}
