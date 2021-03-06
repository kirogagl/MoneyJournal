using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO;

namespace finWpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<MoneyRecord> listRecMon = new List<MoneyRecord>();
        private decimal allMyMoney = 0;
        private MoneyUnits flag = MoneyUnits.USD;
        private MoneyUnits flagCB = MoneyUnits.USD;
        private AllMyMoneyWindow allMyMoneyWindow;
        private addMoneyWindow addMoneyWindow;
        private removeWindow removeWindow;
        private EditWindow editWindow;
        private ConverterWindow converterWindow;
        private yaDisk ya;
        private bool isRequestsExist = true;
        public ObservableCollection<string[]> listStringRecMon = new ObservableCollection<string[]>();
        public string curAPIKey = "31dec130c60c25e40c913e7536c4d8b1";

        public MainWindow()
        {
            InitializeComponent();
            addMoneyWindow = new addMoneyWindow(this);
            removeWindow = new removeWindow(this);
            dataGrid.ItemsSource = listStringRecMon;
            JSONDeserializer();
        }

        /// <summary>
        /// Adding record
        /// </summary>
        private void addMoney_Click(object sender, EventArgs e)
        {
            addMoneyWindow = new addMoneyWindow(this);
            addMoneyWindow.Show();
        }

        /// <summary>
        /// Delete record
        /// </summary>
        private void removeMoney_Click(object sender, EventArgs e)
        {
            removeWindow = new removeWindow(this);
            removeWindow.Show();
        }

        /// <summary>
        /// Editing an entry
        /// </summary>
        private void editMoney_Click(object sender, RoutedEventArgs e)
        {
            editWindow = new EditWindow(this);
            editWindow.Show();
        }

        private void allMyMoney_Click(object sender, RoutedEventArgs e)
        {
            allMyMoneyWindow = new AllMyMoneyWindow(this);
            allMyMoneyWindow.Show();
        }

        /// <summary>
        /// Filling data in the dataGrid when record added
        /// </summary>
        private void listRecMon_ListAdded(MoneyRecord moneyRecord)
        {
            var variable = moneyRecord.ToString().Split("/").ToList();

            string[] del = variable[3].Split(" ");
            variable[3] = del[0];
            listStringRecMon.Add(variable.ToArray());

        }

        /// <summary>
        /// Removing record
        /// </summary>
        /// <param name="index">The location of the element where the record will be deleted</param>
        /// <returns></returns>
        public async Task listRecMon_ListRemoveAsync(int index)
        {
            listRecMon.RemoveAt(index);
            listStringRecMon.RemoveAt(index);
            await allMyMoneyCounter();
            acc.Dispatcher.Invoke(() => acc.Text = allMyMoney.ToString());
            JSONSerializer();
        }

        /// <summary>
        /// Adding an item to a list
        /// </summary>
        /// <param name="moneyRecord"></param>
        public async Task ListAppendAsync(MoneyRecord moneyRecord)
        {
            listRecMon.Add(moneyRecord);
            listRecMon_ListAdded(moneyRecord);
            await allMyMoneyCounter();
            acc.Dispatcher.Invoke(() => acc.Text = allMyMoney.ToString());
            await Task.Delay(51);
            JSONSerializer();
        }

        /// <summary>
        /// Calculating the total amount
        /// </summary> 
        private async Task allMyMoneyCounter()
        {
            allMyMoney = 0;

            foreach (var i in listRecMon)
            {
                if (isRequestsExist)
                {
                    if (i.MonUnit == flagCB && i.Type == Types.Income)
                        allMyMoney += i.Money;
                    else if (i.MonUnit == flagCB && i.Type == Types.Spending)
                        allMyMoney -= i.Money;
                    else
                    {
                        if (i.Type == Types.Income)
                        {
                            var j = await convertUnit(i);
                            allMyMoney += j;

                        }
                        else if (i.Type == Types.Spending)
                        {
                            var j = await convertUnit(i);
                            allMyMoney -= j;
                        }
                    }
                }
            }
            if (isRequestsExist) {
                acc.Dispatcher.Invoke(() => acc.Text = Decimal.Round(allMyMoney, 2, MidpointRounding.ToEven).ToString());
                await Task.Delay(51);
            }
            else
            {
                acc.Dispatcher.Invoke(() => acc.Text = finWpf.Resources.Local.err);
                await Task.Delay(51);
            }
        }

        /// <summary>
        /// Сurrency converter
        /// </summary>
        /// <param name="mr">MoneyRecord</param>
        private async Task<decimal> convertUnit(MoneyRecord mr)
        {
            string Price = await GetPrice(mr.MonUnit.ToString(), flagCB.ToString());
            if (Price != "1" && isRequestsExist)
            {
                Converter? a = Newtonsoft.Json.JsonConvert.DeserializeObject<Converter>(Price);
                NumberFormatInfo nfi = new NumberFormatInfo() { NumberDecimalSeparator = "." };
                return mr.Money * Decimal.Round(decimal.Parse(a.data.value, nfi), 3, MidpointRounding.ToEven);
            }
            else return 1;
        }

        /// <summary>
        /// Combobox changed
        /// </summary>
        private async void unitFlag_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox? s = sender as ComboBox;
            int selInd = s.SelectedIndex;
            if (selInd == 0) flagCB = MoneyUnits.USD;
            else if (selInd == 1) flagCB = MoneyUnits.RUB;
            else if (selInd == 2) flagCB = MoneyUnits.EUR;
            else if (selInd == 3) flagCB = MoneyUnits.CHF;
            else if (selInd == 4) flagCB = MoneyUnits.GBP;
            else if (selInd == 5) flagCB = MoneyUnits.JPY;
            else if (selInd == 6) flagCB = MoneyUnits.UAH;
            else if (selInd == 7) flagCB = MoneyUnits.KZT;
            else if (selInd == 8) flagCB = MoneyUnits.BYN;
            else if (selInd == 9) flagCB = MoneyUnits.TRY;
            else if (selInd == 10) flagCB = MoneyUnits.CNY;
            string Price = await GetPrice(flag.ToString(), flagCB.ToString());
            if (Price != "1")
            {
                Converter? a = Newtonsoft.Json.JsonConvert.DeserializeObject<Converter>(Price);
                NumberFormatInfo nfi = new NumberFormatInfo() { NumberDecimalSeparator = "." };
                allMyMoney *= decimal.Parse(a.data.value, nfi);
                acc.Dispatcher.Invoke(() => acc.Text = Decimal.Round(allMyMoney, 2, MidpointRounding.ToEven).ToString());
                await Task.Delay(51);
                flag = flagCB;
            }
        }

        /// <summary>
        /// Get from currate.ru сurrency exchange rate
        /// </summary>
        /// <param name="val">from this currency</param>
        /// <param name="uta">to this currency</param>
        /// <returns>JSON</returns>
        public async Task<string> GetPrice(string val, string uta)
        {
            try
            {
                if (val.Equals(uta)) throw new ArgumentException();
                string valuta = val + uta;
                using var client = new HttpClient();
                var result = await client.GetStringAsync($"https://currate.ru/api/?get=rates&pairs={valuta}&key={curAPIKey}");
                var results = result.Split("\"");
                results[9] = "value";
                result = "";
                if (results[3] == "500") throw new Exception();
                if (results[3] == "400")
                {
                    isRequestsExist = false;
                    MessageBox.Show(finWpf.Resources.Local.lowReqLimit, finWpf.Resources.Local.warn, MessageBoxButton.OK, MessageBoxImage.Warning);
                    throw new Exception();
                }
                for (int i = 0; i < 13; i++)
                {
                    if (i != 12)
                        result += results[i] + "\"";
                    else result += results[i];
                }
                return result;
            }
            catch (Exception)
            {
                return "1";
            }
        }

        /// <summary>
        /// Getter
        /// </summary>
        /// <returns>List of MoneyRecords</returns>
        public List<MoneyRecord> getListRecMon()
        {
            return listRecMon;
        }

        /// <summary>
        /// Getter
        /// </summary>
        /// <returns>ObservableCollection of string array</returns>
        public ObservableCollection<string[]> GetListStringRecMon()
        {
            return listStringRecMon;
        }

        /// <summary>
        /// Setter
        /// </summary>
        /// <param name="listStringRecMon">ObservableCollection of string array</param>
        /// <param name="listRecMon">List of MoneyRecords</param>
        public async void setListsAsync(ObservableCollection<string[]> listStringRecMon, List<MoneyRecord> listRecMon)
        {
            this.listStringRecMon = listStringRecMon;
            this.listRecMon = listRecMon;

            dataGrid.ItemsSource = listStringRecMon;
            await allMyMoneyCounter();
            JSONSerializer();
        }
        
        /// <summary>
        /// Serialize
        /// </summary>
        public void JSONSerializer()
        {
            string json = JsonSerializer.Serialize< ObservableCollection<string[]>>(listStringRecMon);
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            File.WriteAllText(path + "\\mj.json", json);
            json = JsonSerializer.Serialize<List<MoneyRecord>>(listRecMon);
            File.WriteAllText(path + "\\mj2.json", json);
            string api = curAPIKey;
            File.WriteAllText(path + "\\Key.txt", api);
        }

        /// <summary>
        /// Deserialize
        /// </summary>
        public async void JSONDeserializer()
        {
            try
            {
                curAPIKey = File.ReadAllText(Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "\\Key.txt");

            }
            catch (Exception) { }
            try
            {
                string json = File.ReadAllText(Environment.GetFolderPath(Environment.SpecialFolder.Personal)+"\\mj.json");
                listStringRecMon = JsonSerializer.Deserialize<ObservableCollection<string[]>>(json);
                json = File.ReadAllText(Environment.GetFolderPath(Environment.SpecialFolder.Personal)+"\\mj2.json");
                listRecMon = JsonSerializer.Deserialize<List<MoneyRecord>>(json);
                dataGrid.ItemsSource = listStringRecMon;
                await allMyMoneyCounter();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Yandex drive
        /// </summary>
        private void saveToCloud_Click(object sender, RoutedEventArgs e)
        {
            ya = new yaDisk(this);
            ya.ClientID = "dab62ffa41da4f34bdbf493060a7702a";
            ya.Show();
        }

        private void converterBut_Click(object sender, RoutedEventArgs e)
        {
            converterWindow = new ConverterWindow(this);
            converterWindow.Show();
        }
    }
}
