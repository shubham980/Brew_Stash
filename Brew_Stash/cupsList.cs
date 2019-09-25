using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using static Android.Widget.AdapterView;

namespace Brew_Stash
{
    [Activity(Label = "Choose Cup Size", Theme = "@style/AppTheme.NoActionBar", MainLauncher = false)]
    public class CupsList : AppCompatActivity
    {
        readonly List<Cup> cupItems = new List<Cup>();
        Android.Widget.ListView listView;

        /// <summary>
        /// Creates a list of cups and populates screen with according data
        /// </summary>
        /// <param name="bundle"></param>
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.myListView);
            listView = FindViewById<Android.Widget.ListView>(Resource.Id.listView1);


            cupItems.Add(new Cup()
            {
                Image = Resource.Drawable.smallCup,
                Size = "Small",
                Price = "5$"
            });
            cupItems.Add(new Cup()
            {
                Image = Resource.Drawable.mediumCup,
                Size = "Medium",
                Price = "7$"
            });
            cupItems.Add(new Cup()
            {
                Image = Resource.Drawable.bigCup,
                Size = "Large",
                Price = "9$"
            });

            listView.Adapter = new CupListAdapter(this, cupItems);

            listView.ItemClick += (object sender, ItemClickEventArgs e) =>
            {
                var selectedFromList = listView.GetItemAtPosition(e.Position);
                Console.WriteLine("Item clicked " + cupItems[e.Position].Size);
                MainActivity.finalOrder.CupSize = cupItems[e.Position].Size;
                var intent = new Intent(this, typeof(ServicesList));
                this.StartActivity(intent);
            };

        }

        /// <summary>
        /// Cup class
        /// </summary>

        public class Cup
        {
            public string Size { get; set; }
            public int Image { get; set; }
            public string Price { get; set; }
        }

        /// <summary>
        /// Populates list with data
        /// </summary>

        public class CupListAdapter : BaseAdapter<Cup>
        {
            readonly List<Cup> items;
            readonly Activity context;
            public CupListAdapter(Activity context, List<Cup> items)
                : base()
            {
                this.context = context;
                this.items = items;
            }
            public override long GetItemId(int position)
            {
                return position;
            }
            public override Cup this[int position]
            {
                get { return items[position]; }
            }
            public override int Count
            {
                get { return items.Count; }
            }
            public override Android.Views.View GetView(int position, Android.Views.View convertView, ViewGroup parent)
            {
                var item = items[position];

                Android.Views.View view = convertView;
                if (view == null) // no view to re-use, create new
                    view = context.LayoutInflater.Inflate(Resource.Layout.cupsView, null);
                view.FindViewById<TextView>(Resource.Id.textCup).Text = item.Size;
                view.FindViewById<ImageView>(Resource.Id.imageCup).SetImageResource(item.Image);
                view.FindViewById<TextView>(Resource.Id.textCupPrice).Text = item.Price;

                return view;
            }
        }

    }
}