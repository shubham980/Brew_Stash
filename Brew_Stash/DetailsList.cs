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

        private bool cashSelected = true;
        private bool cardSelected = false;
       
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.detailsView);

            Android.Widget.Button button = FindViewById<Android.Widget.Button>(Resource.Id.button2);

            Spinner spinner = FindViewById<Spinner>(Resource.Id.spinnerPayment);

            ///Populates dropdown list with data

            spinner.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(spinner_ItemSelected);
            var adapter = ArrayAdapter.CreateFromResource(
                    this, Resource.Array.payment_methods, Android.Resource.Layout.SimpleSpinnerItem);

            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spinner.Adapter = adapter;

            /// Checks if all fields are valid and proceeds according to that

            button.Click += delegate {
                if (ValidateFieldsNotEmpty() && ValidatePhoneNumber())
                {
                    UpdateOrder();
                    if (cardSelected)
                    {
                        var intent = new Intent(this, typeof(PaymentList));
                        this.StartActivity(intent);
                    }
                    else
                    {
                        var intent = new Intent(this, typeof(FinalOrderScreen));
                        this.StartActivity(intent);
                    }
                }
            };
        }
        
        /// <summary>
        /// Updates final order with the user input
        /// </summary>

        private void UpdateOrder()
        {
            MainActivity.finalOrder.Name = FindViewById<EditText>(Resource.Id.detailsEditText1).Text;
            MainActivity.finalOrder.ContactNumber = FindViewById<EditText>(Resource.Id.detailsEditText2).Text;
            MainActivity.finalOrder.PickupTime = FindViewById<EditText>(Resource.Id.detailsEditText3).Text;
        }

        /// <summary>
        /// Checks what payment option is selected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void spinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            Spinner spinner = (Spinner)sender;
            switch (e.Position)
            {
                case 0:
                    cashSelected = true;
                    cardSelected = false;
                    break;
                case 1:
                    cashSelected = false;
                    cardSelected = true;
                    break;
            }
        }

        /// <summary>
        /// Checks if fields arent empty
        /// </summary>
        /// <returns></returns>

        private bool ValidateFieldsNotEmpty()
        {
            string Name = FindViewById<EditText>(Resource.Id.detailsEditText1).Text;
            string ContactNumber = FindViewById<EditText>(Resource.Id.detailsEditText2).Text;
            string PickupTime = FindViewById<EditText>(Resource.Id.detailsEditText3).Text;
            if (Name != "" && ContactNumber != "" && PickupTime != "")
            {
                return true;
            }
            else
            {
                Toast.MakeText(this, "You need to complete all fields", ToastLength.Short).Show();
                return false;
            }
        }

        /// <summary>
        /// Checks if phone number is made of less than 11 numbers
        /// </summary>
        /// <returns></returns>

        private bool ValidatePhoneNumber()
        {
            string ContactNumber = FindViewById<EditText>(Resource.Id.detailsEditText2).Text;
            var k = ContactNumber.ToCharArray();
            if (k.Length > 11)
            {
                Toast.MakeText(this, "Contact Number not valid", ToastLength.Short).Show();
                return false;
            }
            else
                return true;
        }
    }
}