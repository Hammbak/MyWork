using MyWork.Exceptions;
using MyWork.Model;
using NUnit.Framework;
using System.Xml.Linq;

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
                new XElement("connectionString", "connectionStringValue")
            );
            var result = DbConnectionInfoConverter.Convert(ele);
            Assert.AreEqual("connectionStringValue", result.ConnectionString);
            Assert.AreEqual("descriptionValue", result.Description);
            Assert.AreEqual(DataBasePurpose.Dev, result.Purpose);
        }

        [Test()]
        public void ConvertTest_description_없음()
        {
            var ele = new XElement("connection",
                new XAttribute("purpose", "dev"),
                new XElement("description", ""),
                new XElement("connectionString", "connectionStringValue")
            );
            var result = DbConnectionInfoConverter.Convert(ele);
            Assert.AreEqual("connectionStringValue", result.ConnectionString);
            Assert.AreEqual("connectionStringValue", result.Description);
            Assert.AreEqual(DataBasePurpose.Dev, result.Purpose);
        }

        [Test()]
        public void ConvertTest_예외()
        {
            var ele = new XElement("connection",
                new XAttribute("purpose", "dev"),
                new XElement("description", "descriptionValue"),
                new XElement("connectionString", "")
            );

            Assert.Throws<ConfigException>(() => { DbConnectionInfoConverter.Convert(ele); });
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