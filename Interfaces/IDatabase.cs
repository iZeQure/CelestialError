using System;
using System.Collections.Generic;
using System.Text;

namespace DevNet.Interfaces
{
    public interface IDatabase: IDisposable
    {
        void Open();
    }
}