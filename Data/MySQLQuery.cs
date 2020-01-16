using System.Data;
using System.Diagnostics;
using MySql.Data.MySqlClient;
using DevNet.Services;
using System;

namespace DevNet.Data
{
    /// <summary>
    /// MySql Query Data Layer Class.
    /// </summary>
    /// <remarks>
    /// Creates queries to write, read, update data, inside the database.
    /// </remarks>
    public class MySQLQuery
    {
        private readonly Database db = Database.Instance; // Get Database Instance.
        private readonly MySQLCommandHandler mySQLCommandHandler = new MySQLCommandHandler(); // Get Command Handler Object.
        private readonly JsonHandler jsonHandler = new JsonHandler(); // Get Json Configuration.

        /// <summary>
        /// Gets the name of the user information by nick.
        /// </summary>
        /// <param name="uniUser">The uni user.</param>
        /// <returns>
        /// String Array of user information.
        /// </returns>
        public string[] GetUserInformationByNickName(string uniUser)
        {
            using (db.MySqlConn)
            {
                db.Open();

                try
                {
                    jsonHandler.TokenName = "GetUserInfo"; // Define Stored Procedure Token.
                    jsonHandler.FilePath = "query.json"; // Define Path for file.

                    using MySqlCommand getUserInformationCommand = mySQLCommandHandler.InitMySqlCommand(jsonHandler.TokenName);
                    Debug.WriteLine($"User Information Command Information : {getUserInformationCommand}");

                    getUserInformationCommand.CommandType = CommandType.StoredProcedure;
                    getUserInformationCommand.Parameters.AddWithValue($"@uniUserName", uniUser);

                    // Read data, from executed command.
                    using (MySqlDataReader reader = getUserInformationCommand.ExecuteReader())
                    {
                        try
                        {
                            while (reader.Read())
                            {
                                string fName = reader.GetString(0);
                                string lName = reader.GetString(1);

                                string[] getUserInformation = new string[] { fName, lName };

                                // Return User Infromation.
                                return getUserInformation;
                            }
                        }
                        finally
                        {
                            if (reader != null)
                            {
                                reader.Close();
                                reader.Dispose();
                            }
                        }
                    }
                    mySQLCommandHandler.Dispose();
                }
                catch (MySqlException mySqlExceptionMsg)
                {
                    Debug.WriteLine($" ################################# Unsuccessfully Executed GetUserInformationCommand:  {mySqlExceptionMsg.Message} [{mySqlExceptionMsg.Number}]");
                }
                finally
                {
                    db.Dispose();
                }
            }
            return null;
        }
    }
}