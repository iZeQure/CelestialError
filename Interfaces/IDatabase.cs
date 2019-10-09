using System;
using System.Collections.Generic;
using System.Text;

namespace DevNet.Interfaces
{
    public interface IDatabase
    {
        void OpenConnection();
        void CloseConnection();
    }
}