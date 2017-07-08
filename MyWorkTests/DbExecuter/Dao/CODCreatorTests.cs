using NUnit.Framework;
using MyWork.DbExecuter.Dao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JangBoGo.Info.Object;

namespace MyWork.DbExecuter.Dao.Tests
{
    [TestFixture()]
    public class CODCreatorTests
    {
        CODCreator CODCreator;

        [Test()]
        public void GetCODTest()
        {
            CODCreator = new CODCreator();

            string conn = "testConnection";
            var result = CODCreator.GetCOD(conn);
            var cod = (CommonObjectDao)result;
            Assert.AreEqual(cod.AdoTemplate.DbProvider.ConnectionString, conn);
        }
    }
}