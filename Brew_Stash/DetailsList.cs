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
    [Activity(Label = "Add Required Details", Theme = "@style/AppTheme.NoActionBar", MainLauncher = false)]
    public class DetailsList : AppCompatActivity    {

       
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.detailsView);

            Android.Widget.Button button = FindViewById<Android.Widget.Button>(Resource.Id.button2);

            button.Click += delegate {
                UpdateOrder();
                var intent = new Intent(this, typeof(PaymentList));
                this.StartActivity(intent);
            };
        }
        
        private void UpdateOrder()
        {
            MainActivity.finalOrder.Name = FindViewById<EditText>(Resource.Id.detailsEditText1).Text;
            MainActivity.finalOrder.ContactNumber = FindViewById<EditText>(Resource.Id.detailsEditText2).Text;
            MainActivity.finalOrder.PickupTime = FindViewById<EditText>(Resource.Id.detailsEditText3).Text;
        }
    }
}