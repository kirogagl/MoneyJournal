using System;
using System.Collections.Generic;
using System.Globalization;
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
    /// Логика взаимодействия для ConverterWindow.xaml
    /// </summary>
    public partial class ConverterWindow : Window
    {
        private MainWindow mainWindow;
        public ConverterWindow(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
            InitializeComponent();
            statusAPI.Text = mainWindow.curAPIKey;
        }

        private void ApplyAPIKey_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.curAPIKey = API.Text;
            statusAPI.Text = mainWindow.curAPIKey;
            mainWindow.JSONSerializer();
        }

        private void RestoreAPIKey_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.curAPIKey = "31dec130c60c25e40c913e7536c4d8b1";
            statusAPI.Text = mainWindow.curAPIKey;
            mainWindow.JSONSerializer();
        }


        private async Task simpleConverter()
        {
            try
            {
                if (toCombo != null && fromCombo != null)
                {
                    string fromUnit = GetMonUnit(fromCombo.SelectedIndex);
                    string toUnit = GetMonUnit(toCombo.SelectedIndex);

                    string Price = await mainWindow.GetPrice(fromUnit, toUnit);
                    if (Price != "1")
                    {
                        Converter? a = Newtonsoft.Json.JsonConvert.DeserializeObject<Converter>(Price);
                        NumberFormatInfo nfi = new NumberFormatInfo() { NumberDecimalSeparator = "." };
                        decimal val = decimal.Parse(a.data.value, nfi);
                        toTx.Text = (Convert.ToDecimal(fromTx.Text) * val).ToString();
                    }
                }
            }
            catch(Exception)
            {
            }
        }

        private string? GetMonUnit(int selInd)
        {
            if (selInd == 0) return MoneyUnits.USD.ToString();
            else if (selInd == 1) return MoneyUnits.RUB.ToString();
            else if (selInd == 2) return MoneyUnits.EUR.ToString();
            else if (selInd == 3) return MoneyUnits.CHF.ToString();
            else if (selInd == 4) return MoneyUnits.GBP.ToString();
            else if (selInd == 5) return MoneyUnits.JPY.ToString();
            else if (selInd == 6) return MoneyUnits.UAH.ToString();
            else if (selInd == 7) return MoneyUnits.KZT.ToString();
            else if (selInd == 8) return MoneyUnits.BYN.ToString();
            else if (selInd == 9) return MoneyUnits.TRY.ToString();
            else if (selInd == 10) return MoneyUnits.CNY.ToString();
            else return null;
        }

        private async void fromCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            await simpleConverter();
        }

        private async void fromTx_TextChanged(object sender, TextChangedEventArgs e)
        {
            await simpleConverter();
        }

        private async void toCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            await simpleConverter();
        }
    }
}
