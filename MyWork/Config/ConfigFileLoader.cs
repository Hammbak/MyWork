using MyWork.Config.Converter;
using MyWork.Exceptions;
using MyWork.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace MyWork.Config
{
    public interface IConfigFileLoader
    {
        IEnumerable<DbConnectionInfoItem> GetConfigInfo(string path);
    }

    public class ConfigFileLoader : IConfigFileLoader
    {
        public IDbConnectionInfoConverter DbConnectionInfoConverter { get; set; } = new DbConnectionInfoConverter();
        public IEnumerable<DbConnectionInfoItem> GetConfigInfo(string path)
        {
            var xDoc = XDocument.Load(path);
            return Parse(xDoc);
        }

        private IEnumerable<DbConnectionInfoItem> Parse(XDocument xDoc)
        {
            return xDoc.Element("config").Element("connections").Elements("connection").SelectMany(t => DbConnectionInfoConverter.Convert(t)).ToList();
        }
    }
}
