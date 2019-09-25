using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V7.App;
using System;

namespace Brew_Stash
{
    [Activity(Label = "Enter Credit Card Details", Theme = "@style/AppTheme.NoActionBar", MainLauncher = false)]
    public class PaymentList : AppCompatActivity
    {

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

            button.Click += delegate
            {
                var intent = new Intent(this, typeof(FinalOrderScreen));
                this.StartActivity(intent);
            };

        }


    }
}