using MyWork.Exceptions;
using MyWork.Model;
using System;
using System.Xml.Linq;

namespace MyWork.Config.Converter
{
    public interface IDbConnectionInfoConverter
    {
        DbConnectionInfoItem Convert(XElement ele);
    }
    public class DbConnectionInfoConverter : IDbConnectionInfoConverter
    {
        public DbConnectionInfoItem Convert(XElement ele)
        {
            var description = ele.Element("description")?.Value;
            var connectionString = ele.Element("connectionString")?.Value;
            var stringPurpose = ele.Attribute("purpose")?.Value;

            if (string.IsNullOrEmpty(connectionString)) throw new ConfigException();
            if (string.IsNullOrEmpty(description)) description = connectionString;

            return new DbConnectionInfoItem
            {
                Description = description,
                ConnectionString = connectionString,
                Purpose = GetPurpose(stringPurpose)
            };
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
