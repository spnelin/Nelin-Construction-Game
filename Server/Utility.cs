using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuildingWebsite.Server
{
    public static class Utility
    {
        public static string MoneyString(int money)
        {
            string moneyStrFrom = money.ToString();
            string moneyStrTo = ",000";
            while (moneyStrFrom.Length > 3)
            {
                moneyStrTo = "," + moneyStrFrom.Substring(moneyStrFrom.Length - 4, 3) + moneyStrTo;
            }
            moneyStrTo = moneyStrFrom + moneyStrTo;
            return "$" + moneyStrTo;
        }
    }
}
