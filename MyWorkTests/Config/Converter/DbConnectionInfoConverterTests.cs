using MyWork.Exceptions;
using MyWork.Model;
using NUnit.Framework;
using System.Xml.Linq;
using System.Linq;

namespace MyWork.Config.Converter.Tests
{
    [TestFixture()]
    public class DbConnectionInfoConverterTests
    {
        DbConnectionInfoConverter DbConnectionInfoConverter { get; set; }
        Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject pObj;

        [SetUp]
        public void Setup()
        {
            DbConnectionInfoConverter = new DbConnectionInfoConverter();
            pObj = new Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject(DbConnectionInfoConverter);
        }

        [Test()]
        public void ConvertTest()
        {
            var ele = new XElement("connection",
                new XAttribute("purpose", "dev"),
                new XElement("description", "descriptionValue"),
                new XElement("connectionIp", "ipValue"),
                new XElement("connectionId", "idValue"),
                new XElement("connectionDatabases", 
                    new XElement("connectionDatabase", "databaseValue1"),
                    new XElement("connectionDatabase", "databaseValue2")
                ),
                new XElement("connectionPassword", "passwordValue")
            );

            var result = DbConnectionInfoConverter.Convert(ele).ToList();
            Assert.AreEqual(2, result.Count());
            Assert.AreEqual("ipValue", result[0].ConnectionIp);
            Assert.AreEqual("idValue", result[0].ConnectionId);
            Assert.AreEqual("passwordValue", result[0].ConnectionPassword);
            Assert.AreEqual("descriptionValue", result[0].Description);
            Assert.AreEqual("databaseValue1", result[0].ConnectionDatabase);
            Assert.AreEqual("databaseValue2", result[1].ConnectionDatabase);
            Assert.AreEqual(DataBasePurpose.Dev, result[0].Purpose);
        }

        [Test()]
        public void ConvertTest_아이디없음()
        {
            var ele = new XElement("connection",
                new XAttribute("purpose", "dev"),
                new XElement("description", "descriptionValue"),
                new XElement("connectionIp", "ipValue"),
                new XElement("connectionId", ""),
                new XElement("connectionDatabases",
                    new XElement("connectionDatabase", "databaseValue1"),
                    new XElement("connectionDatabase", "databaseValue2")
                ),
                new XElement("connectionPassword", "passwordValue")
            );

            Assert.Throws<ConfigException>(() => { DbConnectionInfoConverter.Convert(ele); });
        }

        [Test()]
        public void ConvertTest_아이피없음()
        {
            var ele = new XElement("connection",
                new XAttribute("purpose", "dev"),
                new XElement("description", "descriptionValue"),
                new XElement("connectionIp", ""),
                new XElement("connectionId", "idValue"),
                new XElement("connectionDatabases",
                    new XElement("connectionDatabase", "databaseValue1"),
                    new XElement("connectionDatabase", "databaseValue2")
                ),
                new XElement("connectionPassword", "passwordValue")
            );

            Assert.Throws<ConfigException>(() => { DbConnectionInfoConverter.Convert(ele); });

        }

        [Test()]
        public void ConvertTest패스워드없음()
        {
            var ele = new XElement("connection",
                new XAttribute("purpose", "dev"),
                new XElement("description", "descriptionValue"),
                new XElement("connectionIp", "ipValue"),
                new XElement("connectionId", "idValue"),
                new XElement("connectionDatabases",
                    new XElement("connectionDatabase", "databaseValue1"),
                    new XElement("connectionDatabase", "databaseValue2")
                ),
                new XElement("connectionPassword", "")
            );

            Assert.Throws<ConfigException>(() => { DbConnectionInfoConverter.Convert(ele); });
        }

        [Test()]
        public void ConvertTest데이터베이스없음()
        {
            var ele = new XElement("connection",
                new XAttribute("purpose", "dev"),
                new XElement("description", "descriptionValue"),
                new XElement("connectionIp", "ipValue"),
                new XElement("connectionId", "idValue"),
                new XElement("connectionDatabases",
                    new XElement("connectionDatabase", "databaseValue1"),
                    new XElement("connectionDatabase", "databaseValue2")
                ),
                new XElement("connectionPassword", "")
            );

            Assert.Throws<ConfigException>(() => { DbConnectionInfoConverter.Convert(ele); });
        }


        [Test()]
        public void ConvertTest_description_없음()
        {
            var ele = new XElement("connection",
                new XAttribute("purpose", "dev"),
                new XElement("description", ""),
                new XElement("connectionIp", "ipValue"),
                new XElement("connectionId", "idValue"),
                new XElement("connectionDatabases",
                    new XElement("connectionDatabase", "databaseValue1"),
                    new XElement("connectionDatabase", "databaseValue2")
                ),
                new XElement("connectionPassword", "passwordValue")
            );

            var result = DbConnectionInfoConverter.Convert(ele).ToList();
            Assert.AreEqual(2, result.Count());
            Assert.AreEqual("ipValue", result[0].ConnectionIp);
            Assert.AreEqual("idValue", result[0].ConnectionId);
            Assert.AreEqual("passwordValue", result[0].ConnectionPassword);
            Assert.AreEqual("ipValue", result[0].Description);
            Assert.AreEqual("databaseValue1", result[0].ConnectionDatabase);
            Assert.AreEqual("databaseValue2", result[1].ConnectionDatabase);
            Assert.AreEqual(DataBasePurpose.Dev, result[0].Purpose);
        }

        [Test]
        public void GetPurposeTest()
        {
            Assert.AreEqual(DataBasePurpose.Dev, GetPurpose("dev"));
            Assert.AreEqual(DataBasePurpose.Dev, GetPurpose("dswef32e"));
            Assert.AreEqual(DataBasePurpose.Dev, GetPurpose(null));
            Assert.AreEqual(DataBasePurpose.Dev, GetPurpose(""));

            Assert.AreEqual(DataBasePurpose.Operation, GetPurpose("OPERATION"));
            Assert.AreEqual(DataBasePurpose.Operation, GetPurpose("operation"));
            Assert.AreEqual(DataBasePurpose.Operation, GetPurpose("opeRatIon"));
            Assert.AreEqual(DataBasePurpose.Operation, GetPurpose("Operation"));
            Assert.AreEqual(DataBasePurpose.Test, GetPurpose("Test"));
        }
        private DataBasePurpose GetPurpose(string value)
        {
            return (DataBasePurpose)pObj.Invoke("GetPurpose", value);
        }

    }
}