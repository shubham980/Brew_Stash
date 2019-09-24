using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;

namespace Brew_Stash
{
    [Activity(Label = "Thank you!", Theme = "@style/AppTheme.NoActionBar", MainLauncher = false)]
    class FinalOrderScreen : AppCompatActivity
    {
        /// <summary>
        /// Shows final data in final screen
        /// </summary>
        /// <param name="bundle"></param>
        protected override void OnCreate(Bundle bundle)
        {
            try
            {
                base.OnCreate(bundle);

                SetContentView(Resource.Layout.lastView);

                FindViewById<TextView>(Resource.Id.placeFinal).Text += MainActivity.finalOrder.Cafe;
                FindViewById<TextView>(Resource.Id.finalTime).Text += MainActivity.finalOrder.PickupTime;

                Android.Widget.Button button = FindViewById<Android.Widget.Button>(Resource.Id.buttonReturnMain);

                button.Click += delegate {
                    var intent = new Intent(this, typeof(MainActivity));
                    this.StartActivity(intent);
                };
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
            }

        }
    }
}