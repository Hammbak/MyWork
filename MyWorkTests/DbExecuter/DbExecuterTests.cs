using NUnit.Framework;
using MyWork.DbExecuter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using JangBoGo.Info.Object;
using MyWork.Model;
using MyWork.DbExecuter.Dao;

namespace MyWork.DbExecuter.Tests
{
    [TestFixture()]
    public class DbExecuterTests
    {
        DbExecuter DbExecuter;
        Mock<ICODCreator> MockCODCreator;

        [SetUp]
        public void Setup()
        {
            DbExecuter = new DbExecuter();

            MockCODCreator = new Mock<ICODCreator>();
            DbExecuter.CODCreator = MockCODCreator.Object;
        }

        [Test()]
        public void Execute()
        {
            string query = "select * from table";
            bool dao1Called = false;
            bool dao2Called = false;

            Mock<ICommonObjectDao> MockCOD1 = new Mock<ICommonObjectDao>();
            Mock<ICommonObjectDao> MockCOD2 = new Mock<ICommonObjectDao>();

            MockCOD1.Setup(t => t.Query(new ExecuteQuery { Query = query })).Callback(() => { dao1Called = true; });
            MockCOD2.Setup(t => t.Query(new ExecuteQuery { Query = query })).Callback(() => { dao2Called = true; });

            MockCODCreator.Setup(t => t.GetCOD("connectionString1")).Returns(MockCOD1.Object);
            MockCODCreator.Setup(t => t.GetCOD("connectionString2")).Returns(MockCOD2.Object);

            IList<DbConnectionInfoItem> connectionInfoList = new List<DbConnectionInfoItem>();
            connectionInfoList.Add(new DbConnectionInfoItem { ConnectionString = "connectionString1" });
            connectionInfoList.Add(new DbConnectionInfoItem { ConnectionString = "connectionString2" });
            
            DbExecuter.Execute(connectionInfoList, query);
            Assert.AreEqual(true, dao1Called);
            Assert.AreEqual(true, dao2Called);
        }
    }
}