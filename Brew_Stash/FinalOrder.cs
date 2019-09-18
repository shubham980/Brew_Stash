using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SQLite;

namespace Brew_Stash
{
    public class FinalOrder
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string Cafe { get; set; }
        public string CoffeeType { get; set; }
        public string CupSize { get; set; }
        public int SugarCount { get; set; }
        public int FullCreamMilkCount { get; set; }
        public int SkimMilkCount { get; set; }
        public int WhipCreamCount { get; set; }
        public int IceCreamCount { get; set; }
        public int MarshmallowCount { get; set; }
        public string Name { get; set; }
        public string ContactNumber { get; set; }
        public string PickupTime { get; set; }
        public string PaymentType { get; set; }
        public FinalOrder()
        {
            Cafe = CoffeeType = CupSize = Name = ContactNumber = PickupTime = PaymentType = "";
            SugarCount = FullCreamMilkCount = SkimMilkCount = WhipCreamCount = IceCreamCount = MarshmallowCount = 0;
        }

        public override string ToString()
        {
            return String.Format("Cafe: {0}, Coffee Type: {1}, Cup Size: {2}, Sugar: {3}, Full Cream Milk: {4}, Skim Milk: {5}, Whip Cream: {6} " +
                "Ice Cream: {7}, Marshmallow: {8}, Name{9}, Number: {10}, Pickup Time: {11}", Cafe, CoffeeType, CupSize, SugarCount, FullCreamMilkCount
                , SkimMilkCount, WhipCreamCount, IceCreamCount, MarshmallowCount, Name, ContactNumber, PickupTime);
        }

    }
}