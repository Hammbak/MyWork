using JangBoGo.Info.Object;
using MyWork.DbExecuter.Dao;
using MyWork.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWork.DbExecuter
{
    public interface IDbExecuter
    {
        void Execute(IEnumerable<DbConnectionInfoItem> connectionInfoList, string query);
    }
    public class DbExecuter : IDbExecuter
    {
        public ICODCreator CODCreator { get; set; } = new CODCreator();
        public void Execute(IEnumerable<DbConnectionInfoItem> connectionInfoList, string query)
        {
            connectionInfoList.ForEach(t => Execute(t, query));
        }

        private void Execute(DbConnectionInfoItem connectionInfo, string query)
        {
            var cod = CODCreator.GetCOD(connectionInfo.ConnectionString);
            var executeQuery = new ExecuteQuery {
                Query = query
            };

            try
            {
                cod.Query(executeQuery);
                connectionInfo.Status = "처리완료";
            }
            catch (Exception ex)
            {
                connectionInfo.Status = ex.Message;
            }
        }
    }
}
