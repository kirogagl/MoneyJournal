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
using System.Windows.Shapes;

namespace finWpf
{
    /// <summary>
    /// Логика взаимодействия для AllMyMoneyWindow.xaml
    /// </summary>
    public partial class AllMyMoneyWindow : Window
    {
        private MainWindow mainWindow;
        private List<MoneyRecord> listRecMon = new List<MoneyRecord>();
        public AllMyMoneyWindow(MainWindow mainWindow)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
            fillStackPanel();
        }

        private void fillStackPanel()
        {
            listRecMon = mainWindow.getListRecMon();

            decimal[] moneys = new decimal[11];

            foreach (var mon in listRecMon)
            {
                if (mon.MonUnit == MoneyUnits.RUB && mon.Type == Types.Income) moneys[0] += mon.Money;
                else if (mon.MonUnit == MoneyUnits.RUB) moneys[0] -= mon.Money;
                else if (mon.MonUnit == MoneyUnits.USD && mon.Type == Types.Income) moneys[1] += mon.Money;
                else if (mon.MonUnit == MoneyUnits.USD) moneys[1] -= mon.Money;
                else if (mon.MonUnit == MoneyUnits.EUR && mon.Type == Types.Income) moneys[2] += mon.Money;
                else if (mon.MonUnit == MoneyUnits.EUR) moneys[2] -= mon.Money;
                else if (mon.MonUnit == MoneyUnits.CHF && mon.Type == Types.Income) moneys[3] += mon.Money;
                else if (mon.MonUnit == MoneyUnits.CHF) moneys[3] -= mon.Money;
                else if (mon.MonUnit == MoneyUnits.GBP && mon.Type == Types.Income) moneys[4] += mon.Money;
                else if (mon.MonUnit == MoneyUnits.GBP) moneys[4] -= mon.Money;
                else if (mon.MonUnit == MoneyUnits.JPY && mon.Type == Types.Income) moneys[5] += mon.Money;
                else if (mon.MonUnit == MoneyUnits.JPY) moneys[5] -= mon.Money;
                else if (mon.MonUnit == MoneyUnits.UAH && mon.Type == Types.Income) moneys[6] += mon.Money;
                else if (mon.MonUnit == MoneyUnits.UAH) moneys[6] -= mon.Money;
                else if (mon.MonUnit == MoneyUnits.KZT && mon.Type == Types.Income) moneys[7] += mon.Money;
                else if (mon.MonUnit == MoneyUnits.KZT) moneys[7] -= mon.Money;
                else if (mon.MonUnit == MoneyUnits.BYN && mon.Type == Types.Income) moneys[8] += mon.Money;
                else if (mon.MonUnit == MoneyUnits.BYN) moneys[8] -= mon.Money;
                else if (mon.MonUnit == MoneyUnits.TRY && mon.Type == Types.Income) moneys[9] += mon.Money;
                else if (mon.MonUnit == MoneyUnits.TRY) moneys[9] -= mon.Money;
                else if (mon.MonUnit == MoneyUnits.CNY && mon.Type == Types.Income) moneys[10] += mon.Money;
                else if (mon.MonUnit == MoneyUnits.CNY) moneys[10] -= mon.Money;
                else throw new Exception("How its possible?");
            }

            for (int i = 0; i < 11; i++)
            {
                TextBlock tb = new TextBlock();
                tb.Text = $"{typeof(MoneyUnits).GetFields().Where(f => f.IsLiteral).Select(f => f.Name).ToArray()[i]} = {moneys[i]}";
                stPanel.Children.Add(tb);
            }
        }
    }
}
