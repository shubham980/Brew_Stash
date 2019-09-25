using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V7.App;
using Android.Widget;
using System;

namespace Brew_Stash
{
    [Activity(Label = "Choose Additional Services", Theme = "@style/AppTheme.NoActionBar", MainLauncher = false)]
    public class ServicesList : AppCompatActivity
    {

        /// <summary>
        /// Screen of additional services
        /// </summary>
        /// <param name="bundle"></param>

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.servicesView);

            /// Populates dropdown lists with data

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

            button.Click += delegate
            {
                var intent = new Intent(this, typeof(DetailsList));
                this.StartActivity(intent);
            };

        }

        /// <summary>
        /// Reports whats selected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void spinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            Spinner spinner = (Spinner)sender;
            UpdateOrder(e.Position, spinner.Id);
        }

        /// <summary>
        /// Updates final order with selected data 
        /// </summary>
        /// <param name="count"></param>
        /// <param name="id"></param>

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