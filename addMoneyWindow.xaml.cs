using System;
using System.Windows;

namespace finWpf
{
    /// <summary>
    /// Логика взаимодействия для addMoneyWindow.xaml
    /// </summary>
    public partial class addMoneyWindow : Window
    {
        private MoneyRecord moneyRecord;
        private MainWindow mainWindow;

        public addMoneyWindow(MainWindow mainWindow)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
            moneyRecord = new MoneyRecord();
            fillComboBox();
            mu.Text = "USD";
            ty.Text = "Income";
        }

        /// <summary>
        /// Filling in the combobox for currencies
        /// </summary>
        private void fillComboBox()
        {
            Array monUnits = Enum.GetValues(typeof(MoneyUnits));
            foreach (var i in monUnits)
            {
                mu.Items.Add(Enum.ToObject(typeof(MoneyUnits), i).ToString());
            }
            Array types = Enum.GetValues(typeof(Types));
            foreach (var i in types)
            {
                ty.Items.Add(Enum.ToObject(typeof(Types), i).ToString());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void addBut_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MoneyUnits moneyUnits = MoneyUnits.USD;
                if (mu.Text == "USD") moneyUnits = MoneyUnits.USD;
                else if (mu.Text == "RUB") moneyUnits = MoneyUnits.RUB;
                else if (mu.Text == "USD") moneyUnits = MoneyUnits.USD;
                else if (mu.Text == "EUR") moneyUnits = MoneyUnits.EUR;
                else if (mu.Text == "CHF") moneyUnits = MoneyUnits.CHF;
                else if (mu.Text == "GBP") moneyUnits = MoneyUnits.GBP;
                else if (mu.Text == "JPY") moneyUnits = MoneyUnits.JPY;
                else if (mu.Text == "UAH") moneyUnits = MoneyUnits.UAH;
                else if (mu.Text == "KZT") moneyUnits = MoneyUnits.KZT;
                else if (mu.Text == "BYN") moneyUnits = MoneyUnits.BYN;
                else if (mu.Text == "TRY") moneyUnits = MoneyUnits.TRY;
                else if (mu.Text == "CNY") moneyUnits = MoneyUnits.CNY;
                else throw new Exception();

                if (calendar.SelectedDate == null) throw new Exception();
                moneyRecord = new MoneyRecord(Convert.ToInt32(mc.Text), moneyUnits, ds.Text, calendar.SelectedDate, Enum.Parse<Types>(ty.Text));
                await mainWindow.ListAppendAsync(moneyRecord);
                this.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Empty or wrong values!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void cancBut_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }
}
