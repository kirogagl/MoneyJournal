using System;

namespace finWpf
{
    /// <summary>
    /// class for expense and income records
    /// </summary>
    public class MoneyRecord
    {
        public decimal Money { get; set; }
        public MoneyUnits MonUnit { get; set; }
        public string Desc { get; set; }
        public DateTime? dt { get; set; }
        public Types Type { get; set; }

        public MoneyRecord(int Money, MoneyUnits MonUnit, string Desc, DateTime? dt, Types type)
        {
            this.Money = Money;
            this.MonUnit = MonUnit;
            this.Desc = Desc;
            this.dt = dt;
            this.Type = type;
        }
        public MoneyRecord()
        {
            Money = 0;
            MonUnit = MoneyUnits.USD;
            Desc = "";
            dt = DateTime.Now;
            Type = Types.Income;
        }

        public override string ToString()
        {
            return Money.ToString() + "/" + MonUnit.ToString() + "/" + Desc + "/" + dt.ToString() + "/" + Type.ToString();
        }
    }
}
