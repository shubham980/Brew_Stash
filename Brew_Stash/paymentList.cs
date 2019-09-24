using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;
using static Android.Resource;
using static Android.Widget.AdapterView;

namespace Brew_Stash
{
    [Activity(Label = "Enter Credit Card Details", Theme = "@style/AppTheme.NoActionBar", MainLauncher = false)]
    public class PaymentList : AppCompatActivity    {

       /// <summary>
       /// Shows card payment screen and writes to database after
       /// </summary>
       /// <param name="bundle"></param>
       /// 
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.cardPaymentView);

            Android.Widget.Button button = FindViewById<Android.Widget.Button>(Resource.Id.button10);

            button.Click += delegate {
                MainActivity.Database.SaveOrderAsync(MainActivity.finalOrder);
                var note = MainActivity.Database.GetOrderAsync(0);
                Console.WriteLine(MainActivity.finalOrder.ToString());
                var intent = new Intent(this, typeof(FinalOrderScreen));
                this.StartActivity(intent);
            };
      
        }
        

    }
}