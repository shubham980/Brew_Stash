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
    [Activity(Label = "Choose Additional Services", Theme = "@style/AppTheme.NoActionBar", MainLauncher = false)]
    public class ServicesList : AppCompatActivity    {

       
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.servicesView);

            for (int i = 1; i < 7; i++)
            {
                var res = Resource.Id.spinner1;

                switch (i)
                {
                    case 1:
                        res = Resource.Id.spinner1;
                        break;
                    case 2:
                        res = Resource.Id.spinner2;
                        break;
                    case 3:
                        res = Resource.Id.spinner3;
                        break;
                    case 4:
                        res = Resource.Id.spinner4;
                        break;
                    case 5:
                        res = Resource.Id.spinner5;
                        break;
                    case 6:
                        res = Resource.Id.spinner6;
                        break;

                }
                

                Spinner spinner = FindViewById<Spinner>(res);

                spinner.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(spinner_ItemSelected);
                var adapter = ArrayAdapter.CreateFromResource(
                        this, Resource.Array.numbers_array, Android.Resource.Layout.SimpleSpinnerItem);

                adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
                spinner.Adapter = adapter;
            }

            Android.Widget.Button button = FindViewById<Android.Widget.Button>(Resource.Id.servicesButton);

            button.Click += delegate {
                var intent = new Intent(this, typeof(DetailsList));
                this.StartActivity(intent);
            };

        }
        

        private void spinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            Spinner spinner = (Spinner)sender;
            UpdateOrder(e.Position, spinner.Id);
        }

        private void UpdateOrder(int count, int id)
        {
            switch (id)
            {
                case (Resource.Id.spinner1):
                    MainActivity.finalOrder.SugarCount = count;
                    break;
                case (Resource.Id.spinner2):
                    MainActivity.finalOrder.FullCreamMilkCount = count;
                    break;
                case (Resource.Id.spinner3):
                    MainActivity.finalOrder.SkimMilkCount = count;
                    break;
                case (Resource.Id.spinner4):
                    MainActivity.finalOrder.WhipCreamCount = count;
                    break;
                case (Resource.Id.spinner5):
                    MainActivity.finalOrder.IceCreamCount = count;
                    break;
                case (Resource.Id.spinner6):
                    MainActivity.finalOrder.MarshmallowCount = count;
                    break;
            }
        }


    }
}