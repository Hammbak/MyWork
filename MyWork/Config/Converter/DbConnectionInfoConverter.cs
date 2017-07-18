using MyWork.Exceptions;
using MyWork.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace MyWork.Config.Converter
{
    public interface IDbConnectionInfoConverter
    {
        IEnumerable<DbConnectionInfoItem> Convert(XElement ele);
    }
    public class DbConnectionInfoConverter : IDbConnectionInfoConverter
    {
        public IEnumerable<DbConnectionInfoItem> Convert(XElement ele)
        {
            var description = ele.Element("description")?.Value;
            var stringPurpose = ele.Attribute("purpose")?.Value;
            var connectionIp = ele.Element("connectionIp")?.Value;
            var connectionId = ele.Element("connectionId")?.Value;
            var connectionPassword = ele.Element("connectionPassword")?.Value;
            var connectionDatabases = ele.Element("connectionDatabases")?.Elements("connectionDatabase");

            if (string.IsNullOrEmpty(connectionIp)) throw new ConfigException();
            if (string.IsNullOrEmpty(connectionId)) throw new ConfigException();
            if (string.IsNullOrEmpty(connectionPassword)) throw new ConfigException();
            if (connectionDatabases == null) throw new ConfigException();
            if (string.IsNullOrEmpty(description)) description = connectionIp;


            return connectionDatabases.Select(databaseEle => databaseEle.Value).Select(database => 
                new DbConnectionInfoItem{
                    Description = description,
                    ConnectionIp = connectionIp,
                    ConnectionDatabase = database??"master",
                    ConnectionId = connectionId,
                    ConnectionPassword = connectionPassword,
                    Purpose = GetPurpose(stringPurpose)
                }
            );
        }

        private DataBasePurpose GetPurpose(string value)
        {
            DataBasePurpose enumPurpose;
            if (Enum.TryParse(value, true, out enumPurpose))
            {
                return enumPurpose;
            }
            return DataBasePurpose.Dev;
        }
    }
}
