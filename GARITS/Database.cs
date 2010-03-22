using System;
using System.Collections;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using System.Security.Cryptography;

namespace GARITS
{

    public class Database
    {
        public MySqlConnection sqlConn;
        private string connStr;
        private bool isConnected;

        /// <summary>
        /// Creates a new database wrapper object that wraps around
        /// the users table.
        /// </summary>
        /// <param name="svr">The name of the server</param>
        /// <param name="db">The database catalog to use</param>
        /// <param name="user">The user name</param>
        /// <param name="pass">The user password</param>   CharSet=utf8;
        public Database(string server, string database, string username, string password)
        {
            if (password != "")
                this.connStr = "Server=" + server + ";Database=" + database + ";Uid=" + username + ";Pwd=" + password + ";";
            else
                this.connStr = "Server=" + server + ";Database=" + database + ";Uid=" + username + ";";

            try
            {
                sqlConn = new MySqlConnection(this.connStr);
            }
            catch (Exception excp)
            {
                Exception myExcp = new Exception("Error connecting you to " +
                    "the my sql server. Internal error message: " + excp.Message, excp);
                throw myExcp;
            }

            this.isConnected = false;
        }

        ~Database()
        {
            this.Disconnect();
        }

        /// <summary>
        /// Opens the connection to the SQL database.
        /// </summary>
        public void Connect()
        {
            bool success = true;

            if (this.isConnected == false)
            {
                try
                {
                    this.sqlConn.Open();
                }
                catch (Exception excp)
                {
                    this.isConnected = false;
                    success = false;
                    Exception myException = new Exception("Error opening connection" +
                        " to the sql server. Error: " + excp.Message, excp);

                    throw myException;
                }

                if (success)
                {
                    this.isConnected = true;
                }
            }
        }

        /// <summary>
        /// Closes the connection to the sql connection.
        /// </summary>
        public void Disconnect()
        {
            if (this.isConnected)
            {
                this.sqlConn.Close();
            }
        }

        /// <summary>
        /// Gets the current state (boolean) of the connection.
        /// True for open, false for closed.
        /// </summary>
        public bool IsConnected
        {
            get
            {
                return this.isConnected;
            }
        }


        public bool login(string username, string password)
        {
            DatabaseResult dbresult = runQuery("select password from users where username = '" + username + "' and active = 1");
            if (dbresult.getLength == 0)
                return false;
            string dbpassword = dbresult.getColumnValue(0);
            Console.WriteLine(dbpassword);
            Console.WriteLine(EncodePassword(password));
            if (dbpassword == EncodePassword(password))
                return true;
            return false;
        }

        public DatabaseResult runQuery(string query)
        {
            if (!isConnected)
                throw new Exception("Database is disconnected - unable to run query");
            MySqlCommand command = new MySqlCommand(query, this.sqlConn);
            DatabaseResult dbresult = new DatabaseResult();
            try
            {
                //command.ExecuteNonQuery();
                MySqlDataReader reader = command.ExecuteReader();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    dbresult.addColumn(reader.GetName(i));
                }
                while (reader.Read() != false)
                {
                    string[] values = new string[reader.FieldCount];
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        if (reader.IsDBNull(i))
                            values[i] = null;
                        else
                            values[i] = reader.GetString(i);
                    }
                    dbresult.insertRow(values);
                }
                reader.Close();
            }
            catch (Exception excp)
            {
                Exception myExcp = new Exception("Could not verify user. Error: " +
                    excp.Message, excp);
                throw (myExcp);
            }
            return dbresult;
        }
        /*
        /// <summary>
        /// Verifies whether a user with the supplied user
        /// credentials exists in the database or not. User
        /// credentials are case-sensitive.
        /// </summary>
        /// <param name="username">The user login</param>
        /// <param name="password">The user password</param>
        /// <returns>A boolean value. True if the user exists
        /// in the database, false if the user does not exist
        /// in the database.</returns>
        public bool VerifyUser(string username, string password)
        {
            int returnValue = 0;

            string Query = "SELECT COUNT(*) FROM users where (username=" +
                "'" + username + "' and password='" + EncodePassword(password) + "') LIMIT 1";
            Console.WriteLine(EncodePassword(password));
            MySqlCommand verifyUser = new MySqlCommand(Query, this.sqlConn);
            
            try
            {
                verifyUser.ExecuteNonQuery();

                MySqlDataReader myReader = verifyUser.ExecuteReader();

                while (myReader.Read() != false)
                {
                    returnValue = myReader.GetInt32(0);
                }

                myReader.Close();
            }
            catch (Exception excp)
            {
                Exception myExcp = new Exception("Could not verify user. Error: " +
                    excp.Message, excp);
                throw (myExcp);
            }

            if (returnValue == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Checks whether a supplied user name exists or not
        /// </summary>
        /// <param name="username">The user name</param>
        /// <returns>True if the username is already in the table,
        /// false if the username is not in the table</returns>
        public bool UserExists(string username)
        {
            int returnValue = 0;

            string Query = "SELECT COUNT(*) FROM users where (usr_Name=" +
                "'" + username + "') LIMIT 1";

            MySqlCommand verifyUser = new MySqlCommand(Query, this.sqlConn);

            try
            {
                verifyUser.ExecuteNonQuery();

                MySqlDataReader myReader = verifyUser.ExecuteReader();

                while (myReader.Read() != false)
                {
                    returnValue = myReader.GetInt32(0);
                }

                myReader.Close();
            }
            catch (Exception excp)
            {
                Exception myExcp = new Exception("Could not verify user. Error: " +
                    excp.Message, excp);
                throw (myExcp);
            }

            if (returnValue == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }*/

        public string EncodePassword(string originalPassword)
        {
            //Declarations
            Byte[] originalBytes;
            Byte[] encodedBytes;
            MD5 md5;

            //Instantiate MD5CryptoServiceProvider, get bytes for original password and compute hash (encoded password)
            md5 = new MD5CryptoServiceProvider();
            originalBytes = ASCIIEncoding.Default.GetBytes(originalPassword);
            encodedBytes = md5.ComputeHash(originalBytes);
            
            //Console.WriteLine(encodedBytes);
            //Convert encoded bytes back to a 'readable' string

            return BitConverter.ToString(encodedBytes).Replace("-","").ToLower();
        }

        /*
        public int[] getCurrentRoleID(string username)
        {
            return IntQuery("SELECT roleID from users where username = '" + username + "'");
        }

        public string[] getCurrentRole(string username)
        {
            return StringQuery("select roles.roleName from users, roles where users.username = '" + username + "' and users.roleID = roles.roleID");
        }

        private int[] IntQuery(string query)
        {
            int [] returnValue = new int[2];

            MySqlCommand getRoleID = new MySqlCommand(query + " LIMIT 1", this.sqlConn);

            try
            {
                getRoleID.ExecuteNonQuery();

                MySqlDataReader myReader = getRoleID.ExecuteReader();

                if (myReader.Read() != false)
                {
                    returnValue[1] = myReader.GetInt32(0);
                    returnValue[0] = 1;
                }
                else
                {
                    returnValue[0] = 0;
                }

                myReader.Close();
            }
            catch (Exception excp)
            {
                Exception myExcp = new Exception("Could not fetch int from database. Error: " +
                    excp.Message, excp);
                throw (myExcp);
            }
            return returnValue;
        }

        private string[] StringQuery(string query)
        {
            string[] returnValue = new string[2];

            MySqlCommand command = new MySqlCommand(query + " LIMIT 1", this.sqlConn);

            try
            {
                command.ExecuteNonQuery();

                MySqlDataReader myReader = command.ExecuteReader();

                if (myReader.Read() != false)
                {
                    returnValue[1] = myReader.GetString(0);
                    returnValue[0] = "Found";
                }
                else
                {
                    returnValue[0] = "";
                }

                myReader.Close();
            }
            catch (Exception excp)
            {
                Exception myExcp = new Exception("Could not fetch string from database. Error: " +
                    excp.Message, excp);
                throw (myExcp);
            }
            return returnValue;
        }

        public ArrayList getPermissions()
        {
            ArrayList permissions = new ArrayList();

            MySqlCommand command = new MySqlCommand("SELECT RoleID, Permission, Access from Permissions", this.sqlConn);

            try
            {
                command.ExecuteNonQuery();
                MySqlDataReader myReader = command.ExecuteReader();
                while (myReader.Read() != false)
                {
                    permissions.Add(new Permission(myReader.GetInt32(0), myReader.GetString(1), myReader.GetBoolean(2)));
                }
                myReader.Close();
            }
            catch (Exception excp)
            {
                Exception myExcp = new Exception("Could not fetch permissions from database. Error: " +
                    excp.Message, excp);
                throw (myExcp);
            }
            return permissions;
        }
         */
    }

}
