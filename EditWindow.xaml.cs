using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Shapes;

namespace finWpf
{
    /// <summary>
    /// Логика взаимодействия для EditWindow.xaml
    /// </summary>
    public partial class EditWindow : Window
    {
        private ObservableCollection<string[]> listStringRecMon = new ObservableCollection<string[]>();
        private List<MoneyRecord> listRecMon = new List<MoneyRecord>();
        private List<string> lst = new List<string>();
        private MainWindow mainWindow;
        int selInd;
        public EditWindow(MainWindow mainWindow)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
            getListFromMain();
        }

        /// <summary>
        /// This method takes lists from the main class
        /// </summary>
        private void getListFromMain()
        {
            listStringRecMon = mainWindow.GetListStringRecMon();
            listRecMon = mainWindow.getListRecMon();  
            foreach(var item in listStringRecMon)
            {

                lst.Add(item[0] + " " + item[1] + " " + item[2] + " " + item[3] + " " + item[4]);
            }
            records.ItemsSource = lst;
        }

        /// <summary>
        /// This method makes changes to the lists and sends it to the main class
        /// </summary>
        private void changeMoney_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ds.Text.Equals("")) throw new Exception();
                if (mc.Text.Equals("")) throw new Exception();

                listRecMon[selInd].Money = Convert.ToDecimal(mc.Text);
                listRecMon[selInd].MonUnit = selectedMonUnit();
                listRecMon[selInd].Desc = ds.Text;
                listRecMon[selInd].dt = calendar.SelectedDate;
                listRecMon[selInd].Type = selectedType();

                string[] recMon = new string[5];
                recMon[0] = listRecMon[selInd].Money.ToString();
                recMon[1] = listRecMon[selInd].MonUnit.ToString();
                recMon[2] = listRecMon[selInd].Desc;
                Func<DateTime?, string>? dtToStr = (DateTime? d) =>
                {
                    string? dateTime = d.ToString();
                    string[] dateAndTime = dateTime.Split(' ');
                    return dateAndTime[0];
                };
                recMon[3] = dtToStr((DateTime?)listRecMon[selInd].dt);
                recMon[4] = listRecMon[selInd].Type.ToString();
                listStringRecMon[selInd] = recMon;

                mainWindow.setListsAsync(listStringRecMon, listRecMon);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Empty or wrong values!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        /// <summary>
        /// This method helps to get one element from types from the combobox
        /// </summary>
        /// <returns>Type</returns>
        private Types selectedType()
        {
            if (ty.SelectedIndex == 0) return Types.Income;
            else return Types.Spending;
        }

        /// <summary>
        /// This method helps to get one element from MoneyUnits from the combobox
        /// </summary>
        /// <returns>MoneyUnit</returns>
        private MoneyUnits selectedMonUnit()
        {
            MoneyUnits unit;
            if (mu.SelectedIndex == 0)  unit = MoneyUnits.USD;
            else if (mu.SelectedIndex == 1) unit = MoneyUnits.RUB;
            else if (mu.SelectedIndex == 2) unit = MoneyUnits.EUR;
            else if (mu.SelectedIndex == 3) unit = MoneyUnits.CHF;
            else if (mu.SelectedIndex == 4) unit = MoneyUnits.GBP;
            else if (mu.SelectedIndex == 5) unit = MoneyUnits.JPY;
            else if (mu.SelectedIndex == 6) unit = MoneyUnits.UAH;
            else if (mu.SelectedIndex == 7) unit = MoneyUnits.KZT;
            else if (mu.SelectedIndex == 8) unit = MoneyUnits.BYN;
            else if (mu.SelectedIndex == 9) unit = MoneyUnits.TRY;
            else unit = MoneyUnits.CNY;
            return unit;
        }

        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// This method fills in the window elements when the combo box field is selected
        /// </summary>
        private void records_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try {
                ComboBox? s = sender as ComboBox;
                selInd = s.SelectedIndex;
                calendar.Text = listRecMon[selInd].dt.ToString();
                mc.Text = listRecMon[selInd].Money.ToString();
                ds.Text = listRecMon[selInd].Desc;
                if (listRecMon[selInd].MonUnit == MoneyUnits.USD) mu.SelectedIndex = 0;
                else if(listRecMon[selInd].MonUnit == MoneyUnits.RUB) mu.SelectedIndex = 1;
                else if (listRecMon[selInd].MonUnit == MoneyUnits.EUR) mu.SelectedIndex = 2;
                else if (listRecMon[selInd].MonUnit == MoneyUnits.CHF) mu.SelectedIndex = 3;
                else if (listRecMon[selInd].MonUnit == MoneyUnits.GBP) mu.SelectedIndex = 4;
                else if (listRecMon[selInd].MonUnit == MoneyUnits.JPY) mu.SelectedIndex = 5;
                else if (listRecMon[selInd].MonUnit == MoneyUnits.UAH) mu.SelectedIndex = 6;
                else if (listRecMon[selInd].MonUnit == MoneyUnits.KZT) mu.SelectedIndex = 7;
                else if (listRecMon[selInd].MonUnit == MoneyUnits.BYN) mu.SelectedIndex = 8;
                else if (listRecMon[selInd].MonUnit == MoneyUnits.TRY) mu.SelectedIndex = 9;
                else mu.SelectedIndex = 10;

                if (listRecMon[selInd].Type == Types.Income) ty.SelectedIndex = 0;
                else if (listRecMon[selInd].Type == Types.Spending) ty.SelectedIndex = 1;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
