using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
namespace finWpf
{
    /// <summary>
    /// Логика взаимодействия для removeWindow.xaml
    /// </summary>
    public partial class removeWindow : Window
    {
        private MainWindow mainWindow;
        private int indexForRemove;
        public removeWindow(MainWindow mainWindow)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
            fillComboBox();
        }

        private void fillComboBox()
        {
            List<MoneyRecord> listRecMon = mainWindow.getListRecMon();
            foreach (MoneyRecord mon in listRecMon)
            {
                var variable = mon.ToString().Split("/").ToList();
                string[] del = variable[3].Split(" ");
                variable[3] = del[0];
                records.Items.Add(variable[0]+" "+variable[1]+" "+variable[2]+" "+variable[3]+" "+variable[4]);
            }
        }
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox? s = sender as ComboBox;
            indexForRemove = s.SelectedIndex;
        }

        private async void Remove_Click(object sender, RoutedEventArgs e)
        {
            await mainWindow.listRecMon_ListRemoveAsync(indexForRemove);
            this.Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
