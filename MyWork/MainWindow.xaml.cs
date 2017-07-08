using MyWork.Config;
using MyWork.DbExecuter;
using MyWork.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MyWork
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        private IDbExecuter DbExecuter { get; set; } = new DbExecuter.DbExecuter();
        public IEnumerable<DbConnectionInfoItem> DbConnectionInfoList;

        public MainWindow()
        {
            InitializeComponent();
            ConfigLoad();
        }

        private void ConfigLoad()
        {
            ConfigFileLoader configFileLoader = new ConfigFileLoader();
            var configPath = AppDomain.CurrentDomain.BaseDirectory.CombinePath(@"config.xml");
            DbConnectionInfoList = configFileLoader.GetConfigInfo(configPath);
            dbConnectionListView.ItemsSource = DbConnectionInfoList;
        }

        public void Execute()
        {
            var selectedList = DbConnectionInfoList.Where(t => t.Checked);
            string query = new TextRange(queryText.Document.ContentStart, queryText.Document.ContentEnd).Text;
 
            DbExecuter.Execute(selectedList, query);
            DbConnectionListViewRefresh();
        }

        private void DbConnectionListViewRefresh()
        {
            dbConnectionListView.Dispatcher.Invoke(new Action(() => { dbConnectionListView.Items.Refresh(); }));
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            Execute();
        }
    }
}
