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
        
        public IEnumerable<CheckboxSelectItem> DatabaseSelectedList;
        public IEnumerable<CheckboxSelectItem> PurposeSelectedList;
        public IEnumerable<CheckboxSelectItem> DescriptionSelectedList;

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
            DatabaseSelectedList = DbConnectionInfoList.Select(t => t.ConnectionDatabase).Distinct().Select(t => new CheckboxSelectItem { Name = t, Checked = true }).ToList();
            PurposeSelectedList = GetPurposeSelectedList();
            DescriptionSelectedList = GetDescriptionSelectedList();


            dbConnectionListView.ItemsSource = DbConnectionInfoList;
            databaseListBox.ItemsSource = DatabaseSelectedList;
            purposeListBox.ItemsSource = PurposeSelectedList;
            descriptionListBox.ItemsSource = DescriptionSelectedList;

            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(dbConnectionListView.ItemsSource);
            view.Filter = UserFilter;
        }

        private IEnumerable<CheckboxSelectItem> GetPurposeSelectedList()
        {
            var purposeList = Enum.GetValues(typeof(DataBasePurpose));
            IList<CheckboxSelectItem> result = new List<CheckboxSelectItem>();
            foreach (var item in purposeList)
            {
                result.Add(new CheckboxSelectItem { Name = item.ToString(), Checked = true });
            }
            return result;
        }
        public IEnumerable<CheckboxSelectItem> GetDescriptionSelectedList()
        {
            string[] descriptionList = { "일본향", "싱가폴향", "한국향" };
            return descriptionList.Select(t => new CheckboxSelectItem { Name = t, Checked = true }).ToList();
        }

        private bool UserFilter(object item)
        {
            DbConnectionInfoItem dbItem = item as DbConnectionInfoItem;
            bool isShow = DatabaseSelectedList.Where(t => t.Checked).Select(t => t.Name).Contains(dbItem.ConnectionDatabase, StringComparer.OrdinalIgnoreCase)
                && PurposeSelectedList.Where(t => t.Checked).Select(t => t.Name).Contains(dbItem.Purpose.ToString(), StringComparer.OrdinalIgnoreCase)
                && DescriptionSelectedList.Where(t => t.Checked).Select(t => t.Name).Contains(dbItem.Description.Split(" - ")[1], StringComparer.OrdinalIgnoreCase);
            if (!isShow) dbItem.Checked = false;
            return isShow;
        }

        public void Execute()
        {
            var selectedList = DbConnectionInfoList.Where(t => t.Checked);
            string query = new TextRange(queryText.Document.ContentStart, queryText.Document.ContentEnd).Text;
 
            DbExecuter.Execute(selectedList, query);
        }

        private void ViewRefresh()
        {
            CollectionViewSource.GetDefaultView(dbConnectionListView.ItemsSource).Refresh();
            CollectionViewSource.GetDefaultView(databaseListBox.ItemsSource).Refresh();
            CollectionViewSource.GetDefaultView(purposeListBox.ItemsSource).Refresh();
            CollectionViewSource.GetDefaultView(descriptionListBox.ItemsSource).Refresh();
            //dbConnectionListView.Dispatcher.Invoke(new Action(() => { dbConnectionListView.Items.Refresh(); }));
        }

        private void MessageReset()
        {
            DbConnectionInfoList.ForEach(t => { t.Status = ""; t.Message = ""; });
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            MessageReset();
            ViewRefresh();
            Execute();
            ViewRefresh();
        }


        private void DbConnectionAllCheck(object sender, RoutedEventArgs e)
        {
            DbConnectionInfoList.ForEach(t => t.Checked = true);
            ViewRefresh();
        }

        private void DbConnectionAllUncheck(object sender, RoutedEventArgs e)
        {
            DbConnectionInfoList.ForEach(t => t.Checked = false);
            ViewRefresh();
        }


        private void DatabaseAllCheck(object sender, RoutedEventArgs e)
        {
            DatabaseSelectedList.ForEach(t => t.Checked = true);
            ViewRefresh();
        }

        private void DatabaseAllUncheck(object sender, RoutedEventArgs e)
        {
            DatabaseSelectedList.ForEach(t => t.Checked = false);
            ViewRefresh();
        }

        private void PurposeAllCheck(object sender, RoutedEventArgs e)
        {
            PurposeSelectedList.ForEach(t => t.Checked = true);
            ViewRefresh();
        }

        private void PurposeAllUncheck(object sender, RoutedEventArgs e)
        {
            PurposeSelectedList.ForEach(t => t.Checked = false);
            ViewRefresh();
        }

        private void DescriptionAllCheck(object sender, RoutedEventArgs e)
        {
            DescriptionSelectedList.ForEach(t => t.Checked = true);
            ViewRefresh();
        }

        private void DescriptionAllUncheck(object sender, RoutedEventArgs e)
        {
            DescriptionSelectedList.ForEach(t => t.Checked = false);
            ViewRefresh();
        }

        private void FilterCheckChange(object sender, RoutedEventArgs e)
        {
            ViewRefresh();
        }
    }
}
