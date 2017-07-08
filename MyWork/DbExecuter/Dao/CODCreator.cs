using JangBoGo.Info.Object;
using Spring.Data.Common;
using Spring.Data.Generic;
using Spring.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWork.DbExecuter.Dao
{
    public interface ICODCreator
    {
        ICommonObjectDao GetCOD(string connectionString);
    }
    public class CODCreator : ICODCreator
    {
        public ICommonObjectDao GetCOD(string connectionString)
        {
            return new CommonObjectDao
            {
                AdoTemplate = GetAdoTemplate(connectionString)
            };
        }

        private AdoTemplate GetAdoTemplate(string connectionString)
        {
            return new AdoTemplate
            {
                DataReaderWrapperType = (Type)ExpressionEvaluator.GetValue(null, "T(Spring.Data.Support.NullMappingDataReader, Spring.Data)"),
                DbProvider = GetDbProvider(connectionString)
            };
        }

        private IDbProvider GetDbProvider(string connectionString)
        {
            IDbProvider provider = DbProviderFactory.GetDbProvider("System.Data.SqlClient");
            provider.ConnectionString = connectionString;
            return provider;
        }

    }
}
