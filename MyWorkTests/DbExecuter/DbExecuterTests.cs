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
using MyWork.DbExecuter.Connection;
using Yusurun.Info.NameValue.Model;

namespace MyWork.DbExecuter.Tests
{
    [TestFixture()]
    public class DbExecuterTests
    {
        
        DbExecuter DbExecuter;
        
        Mock<ICODCreator> MockCODCreator;
        Mock<IConnectionStringMaker> MockConnectionStringMaker;

        [SetUp]
        public void Setup()
        {
            DbExecuter = new DbExecuter();

            MockCODCreator = new Mock<ICODCreator>();
            DbExecuter.CODCreator = MockCODCreator.Object;

            MockConnectionStringMaker = new Mock<IConnectionStringMaker>();
            DbExecuter.ConnectionStringMaker = MockConnectionStringMaker.Object;
        }

        [Test()]
        public void Execute()
        {
            string query = "select * from table";
            bool dao1Called = false;
            bool dao2Called = false;

            Mock<ICommonObjectDao> MockCOD1 = new Mock<ICommonObjectDao>();
            Mock<ICommonObjectDao> MockCOD2 = new Mock<ICommonObjectDao>();

            MockCOD1.Setup(t => t.Query(new ListQuery<NameValueItem> { Query = query })).Callback(() => { dao1Called = true; });
            MockCOD2.Setup(t => t.Query(new ListQuery<NameValueItem> { Query = query })).Callback(() => { dao2Called = true; });

            MockCODCreator.Setup(t => t.GetCOD("connectionString1")).Returns(MockCOD1.Object);
            MockCODCreator.Setup(t => t.GetCOD("connectionString2")).Returns(MockCOD2.Object);

            MockConnectionStringMaker.Setup(t => t.Make("ConnectionIp1", "ConnectionDatabase1", "ConnectionId1", "ConnectionPassword1")).Returns("connectionString1");
            MockConnectionStringMaker.Setup(t => t.Make("ConnectionIp2", "ConnectionDatabase2", "ConnectionId2", "ConnectionPassword2")).Returns("connectionString2");

            IList<DbConnectionInfoItem> connectionInfoList = new List<DbConnectionInfoItem>();

            connectionInfoList.Add(new DbConnectionInfoItem { ConnectionIp = "ConnectionIp1", ConnectionDatabase = "ConnectionDatabase1", ConnectionId = "ConnectionId1", ConnectionPassword = "ConnectionPassword1" });
            connectionInfoList.Add(new DbConnectionInfoItem { ConnectionIp = "ConnectionIp2", ConnectionDatabase = "ConnectionDatabase2", ConnectionId = "ConnectionId2", ConnectionPassword = "ConnectionPassword2" });
            
            DbExecuter.Execute(connectionInfoList, query);
            Assert.AreEqual(true, dao1Called);
            Assert.AreEqual(true, dao2Called);
        }
    }
}
