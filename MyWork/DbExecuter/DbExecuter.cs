using JangBoGo.Info.Object;
using MyWork.DbExecuter.Connection;
using MyWork.DbExecuter.Dao;
using MyWork.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yusurun.Info.NameValue.Model;

namespace MyWork.DbExecuter
{
    public interface IDbExecuter
    {
        void Execute(IEnumerable<DbConnectionInfoItem> connectionInfoList, string query);
        void Find(IEnumerable<DbConnectionInfoItem> connectionInfoList, string objectName);
    }
    public class DbExecuter : IDbExecuter
    {
        public ICODCreator CODCreator { get; set; } = new CODCreator();
        public IConnectionStringMaker ConnectionStringMaker { get; set; } = new ConnectionStringMaker();

        public void Execute(IEnumerable<DbConnectionInfoItem> connectionInfoList, string query)
        {
            connectionInfoList.ForEach(t => Execute(t, query));
        }

        public void Find(IEnumerable<DbConnectionInfoItem> connectionInfoList, string objectName)
        {
            string query = $@"
                SELECT type COLLATE Latin1_General_CI_AS_KS_WS AS [type] FROM dbo.sysobjects WHERE id = OBJECT_ID(N'{objectName}')
                UNION ALL
                SELECT type_desc COLLATE Latin1_General_CI_AS_KS_WS AS [type] FROM sys.indexes WHERE name = '{objectName}'
            ";
            Execute(connectionInfoList, query);
        }

        private void Execute(DbConnectionInfoItem connectionInfo, string query)
        {
            var connectionString = ConnectionStringMaker.Make(connectionInfo.ConnectionIp, connectionInfo.ConnectionDatabase, connectionInfo.ConnectionId, connectionInfo.ConnectionPassword);
            var cod = CODCreator.GetCOD(connectionString);
            var executeQuery = new ListQuery<NameValueItem> {
                Query = query
            };

            try
            {
                var result = cod.Query(executeQuery);
                connectionInfo.Status = "완료";
                var sb = new StringBuilder();

                if (result.Count() > 0)
                {
                    var tdic = result[0].GetDic();
                    var keys = tdic.Keys;

                    sb.AppendLine(string.Join("|", tdic.Keys));

                    result.ForEach(t =>
                    {
                        sb.AppendLine(string.Join("|", keys.Select(k => t.GetString(k))));
                    });
                }
                connectionInfo.Message = sb.ToString();
            }
            catch (Exception ex)
            {
                connectionInfo.Status = "오류";
                connectionInfo.Message = ex.Message;
            }
        }
    }
}
