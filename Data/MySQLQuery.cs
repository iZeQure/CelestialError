using System;
using System.Data;
using System.Text;
using System.Collections.Generic;
using System.Diagnostics;
using MySql.Data.MySqlClient;
using DevNet.Services;
using DevNet.Data;
using DevNet.Interfaces;

namespace DevNet.Data
{
    public class MySQLQuery
    {
        private readonly Database instance = Database.Instance;
        private readonly MySQLCommandHandler mySQLCommandHandler = new MySQLCommandHandler();
        private readonly JsonHandler jsonHandler = new JsonHandler();

        public string[] GetUserInformationByNickName(string uniUser)
        {
            using (instance.MySqlConn)
            {
                instance.OpenConnection();

                try
                {
                    jsonHandler.TokenName = "GetUserInfo";
                    jsonHandler.FilePath = "query.json";

                    string rtn = jsonHandler.GetJsonToken();

                    using (MySqlCommand getUserInformationCommand = mySQLCommandHandler.InitMySqlCommand(rtn))
                    {
                        getUserInformationCommand.CommandType = CommandType.StoredProcedure;
                        getUserInformationCommand.Parameters.AddWithValue($"@uniUserName", uniUser);

                        using (MySqlDataReader reader = getUserInformationCommand.ExecuteReader())
                        {
                            try
                            {
                                while (reader.Read())
                                {
                                    string fName = reader.GetString(0);
                                    string lName = reader.GetString(1);

                                    string[] getUserInformation = new string[] { fName, lName };

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
                }
                catch (MySqlException mySqlExceptionMsg)
                {
                    Debug.WriteLine($" ################################# Unsuccessfully Executed GetUserInformationCommand:  {mySqlExceptionMsg.Message} [{mySqlExceptionMsg.Number}]");
                }
                finally
                {
                    if (instance.MySqlConn != null)
                    {
                        instance.CloseConnection();
                        instance.MySqlConn.Dispose();
                    }
                }
            }
            return null;
        }
    }
}