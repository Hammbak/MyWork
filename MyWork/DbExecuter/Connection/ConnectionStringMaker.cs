using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWork.DbExecuter.Connection
{
    public interface IConnectionStringMaker
    {
        string Make(string ip, string database, string id, string password);
    }
    public class ConnectionStringMaker : IConnectionStringMaker
    {
        public string Make(string ip, string database, string id, string password)
        {
            return $"SERVER={ip}; DATABASE={database}; USER ID={id}; PASSWORD={password}";
        }
    }
}
