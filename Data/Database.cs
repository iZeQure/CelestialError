using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Text;
using DevNet.Interfaces;
using MySql.Data.MySqlClient;

namespace DevNet.Data
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="DevNet.Interfaces.IDatabase" />
    public class Database : IDatabase
    {
        #region Instances
        private static Database instance = null;
        #endregion

        #region Attributes
        private static readonly string mySqlConnString = ConfigurationManager.ConnectionStrings["CelestialError"].ConnectionString;
        private MySqlConnection mySqlConn;
        #endregion

        #region Properties
        public MySqlConnection MySqlConn
        {
            get { return mySqlConn; }
            private set { mySqlConn = value; }
        }

        public static string MySqlConnString
        {
            get { return mySqlConnString; }
        }
        #endregion


        private Database()
        {
            try
            {
                MySqlConn = new MySqlConnection(MySqlConnString);
            }
            catch (MySqlException mySqlExeptionMessage)
            {
                Debug.WriteLine($" ################################# Failed to Connect to Host: {mySqlExeptionMessage.Message} [{mySqlExeptionMessage.Number}]");
            }
            catch (Exception exeptionMessage)
            {
                Debug.WriteLine($" ################################# Undefined Exeption: {exeptionMessage.Message} [{exeptionMessage.HelpLink}]");
            }
        }

        public static Database Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Database();
                }
                return instance;
            }
        }

        public void OpenConnection()
        {
            try
            {
                MySqlConn.Open();
            }
            catch (MySqlException mySqlExceptionMessage)
            {
                Debug.WriteLine($" ################################# Could not Open Connection: {mySqlExceptionMessage.Message} [{mySqlExceptionMessage.Number}]");
            }
            catch (Exception exceptionMessage)
            {
                Debug.WriteLine($" ################################# Undefined Exception: {exceptionMessage.Message} [{exceptionMessage.HelpLink}]");
            }
        }

        public void CloseConnection()
        {
            try
            {
                MySqlConn.Close();
            }
            catch (MySqlException mySqlExceptionMessage)
            {
                Debug.WriteLine($" ################################# Could not Dispose Connection: {mySqlExceptionMessage.Message} [{mySqlExceptionMessage.Number}]");
            }
            catch (Exception exceptionMessage)
            {
                Debug.WriteLine($" ################################# Undefined Exception: {exceptionMessage.Message} [{exceptionMessage.HelpLink}]");
            }
            finally
            {
                if (MySqlConn != null)
                    MySqlConn.Dispose();
            }
        }
    }
}
