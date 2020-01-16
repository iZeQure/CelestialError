using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;
using DevNet.Data;

namespace DevNet.Services
{
    public class MySQLCommandHandler
    {
        private readonly Database db = Database.Instance;
        private MySqlCommand mySqlCommand;

        public MySqlCommand MySqlCommand
        {
            get { return mySqlCommand; }
            private set { mySqlCommand = value; }
        }

        public void Dispose()
        {
            if (MySqlCommand != null)
            {
                MySqlCommand.Dispose();
            }
        }

        public MySqlCommand InitMySqlCommand(string query)
        {
            MySqlCommand = new MySqlCommand(query, db.MySqlConn);

            return mySqlCommand;
        }
    }
}
