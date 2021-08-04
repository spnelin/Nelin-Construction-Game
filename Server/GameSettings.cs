using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuildingWebsite.Server
{
    public class GameSettings
    {
        public int ConcretePrice { get; private set; }

        public int ConcretePriceDiscount => ConcretePrice * 8 / 10;

        public int ConcretePriceDiscountPlus => ConcretePrice / 2;

        public int SteelPrice { get; private set; }

        public int SteelPriceDiscount => SteelPrice * 8 / 10;

        public int SteelPriceDiscountPlus => SteelPrice / 2;

        public int GlassPrice { get; private set; }

        public int GlassPriceDiscount => GlassPrice * 8 / 10;

        public int GlassPriceDiscountPlus => GlassPrice / 2;

        public int WorkerSalary { get; private set; }

        public GameSettings()
        {
            ConcretePrice = 10;
            SteelPrice = 12;
            GlassPrice = 15;
            WorkerSalary = 10;
        }
    }
}
