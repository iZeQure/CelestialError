using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Text;
using DevNet.Interfaces;
using DevNet.Services;
using MySql.Data.MySqlClient;

namespace DevNet.Data
{
    /// <summary>
    /// Creates a Database Connection,
    /// of <see cref="MySqlConnection"/>.
    /// </summary>
    /// <remarks>
    /// This Class is using Singleton Pattern.
    /// </remarks>
    /// <seealso cref="DevNet.Interfaces.IDatabase" />
    public class Database : IDatabase
    {
        #region Instances
        //private static ConfigurationBuilders confBuilder = null;
        private static Database instance = null;
        #endregion

        #region Attributes
        private static readonly string mySqlConnString = ConfigurationManager.ConnectionStrings["CelestialError"]?.ConnectionString;
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

        public void Dispose()
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

        public void Open()
        {
            try
            {
                MySqlConn.Open();
            }
            catch (NullReferenceException ex)
            {
                Debug.WriteLine($"Object Reference : {ex.Message}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Unexpected Unhandles Exception Occured: {ex.Message}");
            }
        }
    }
}
