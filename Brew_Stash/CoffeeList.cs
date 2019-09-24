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
    [Activity(Label = "Choose Coffee Type", Theme = "@style/AppTheme.NoActionBar", MainLauncher = false)]
    public class CoffeeList : AppCompatActivity    {

        List<Coffee> coffeeItems = new List<Coffee>();
        Android.Widget.ListView listView;

        /// <summary>
        /// Creates list of coffees and populates screen with according information
        /// </summary>
        /// <param name="bundle"></param>
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);


            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.myListView);
            listView = FindViewById<Android.Widget.ListView>(Resource.Id.listView1);


            coffeeItems.Add(new Coffee()
            {
                Image = Resource.Drawable.Espresso_Macchiato,
                Description = "A 1-ounce shot of coffee served in a small cup. Espresso coffee is often blended from several roasts and varietals to form a bold - not bitter flavour. The finely ground coffee is tightly packed or tamped into a “portafilter” and then a high-pressure water is then forced through the grounds and extracted in small, concentrated amounts",
                Name = "Single shot Espresso"
            });
            coffeeItems.Add(new Coffee()
            {
                Image = Resource.Drawable.Espresso_Macchiato,
                Description = "A double shot uses 14g of coffee and produces around 60ml of espresso (about 2 liquid ounces).",
                Name = "Double shot Espresso"
            });
            coffeeItems.Add(new Coffee()
            {
                Image = Resource.Drawable.Espresso_Macchiato,
                Description = "Macchiato is a very simple drink devoid of the flavoured caramel and chocolate treatment better suited to an ice-cream parlour. It is simply a shot of espresso with a layer of foamed milk.",
                Name = "Espresso Macchiato"
            });
            coffeeItems.Add(new Coffee()
            {
                Image = Resource.Drawable.Iced_Coffee,
                Description = "Coffee with ice cubes makes for watery, cold coffee. Begin with strong coffee - stronger than you would normally brew hot. Try bolder tasting, dark roasts. Brew it strong. You can double brew by pouring hot coffee back onto fresh grinds - like pouring the coffee back into the coffee maker and brewing again. Add sugar or spices like cardamom before chilling so they dissolve thoroughly. You can add ice then, but it’s best to chill in the refrigerator for a few hours or even overnight so the ice doesn’t melt so fast.",
                Name = "Iced Coffee"
            });
            coffeeItems.Add(new Coffee()
            {
                Image = Resource.Drawable.Frappucinno,
                Description = "It is a foam-covered iced coffee drink made from spray-dried instant coffee. ",
                Name = "Frappé"
            });
            coffeeItems.Add(new Coffee()
            {
                Image = Resource.Drawable.Iced_Coffee,
                Description = "It is the process of steeping coffee grounds in water at cool temperatures for an extended period. Coarse-ground beans are soaked in water for a prolonged period of time, for 12 to 24 hours.",
                Name = "Cold brew"
            });
            coffeeItems.Add(new Coffee()
            {
                Image = Resource.Drawable.Espresso_con_Panna,
                Description = "It is a single or double shot of espresso topped with whipped cream.",
                Name = "Espresso con panna"
            });
            coffeeItems.Add(new Coffee()
            {
                Image = Resource.Drawable.Vienna,
                Description = "It is a cream-based coffee beverage. It is made by preparing two shots of strong black espresso in a standard sized coffee cup and infusing the coffee with whipped cream (as a replacement for milk and sugar) until the cup is full. Then the cream is twirled and optionally topped off with chocolate sprinklings. The coffee is drunk through the creamy top.",
                Name = "Vienna Coffee"
            });
            coffeeItems.Add(new Coffee()
            {
                Image = Resource.Drawable.Caffe_Mocha,
                Description = "It is a chocolate-flavoured variant of a caffè latte. Caffè mocha is based on espresso and hot milk but with added chocolate flavouring and sweetener, typically in the form of cocoa powder and sugar. Many varieties use chocolate syrup instead, and some may contain dark or milk chocolate.",
                Name = "Caffè mocha"
            });

            listView.Adapter = new CoffeeListAdapter(this, coffeeItems);

            listView.ItemClick += (object sender, ItemClickEventArgs e) =>
            {
                var selectedFromList = listView.GetItemAtPosition(e.Position);
                MainActivity.finalOrder.CoffeeType = coffeeItems[e.Position].Name;
                var intent = new Intent(this, typeof(CupsList));
                this.StartActivity(intent);
            };

        }

        /// <summary>
        /// Coffe object
        /// </summary>

        public class Coffee
        {
            public string Description { get; set; }
            public string Name { get; set; }
            public int Image { get; set; }
        }

        /// <summary>
        /// Populates screen with data
        /// </summary>

        public class CoffeeListAdapter : BaseAdapter<Coffee>
        {
            List<Coffee> items;
            Activity context;
            public CoffeeListAdapter(Activity context, List<Coffee> items)
                : base()
            {
                this.context = context;
                this.items = items;
            }
            public override long GetItemId(int position)
            {
                return position;
            }
            public override Coffee this[int position]
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
                    view = context.LayoutInflater.Inflate(Resource.Layout.coffeeView, null);
                view.FindViewById<TextView>(Resource.Id.textView1).Text = item.Name;
                view.FindViewById<TextView>(Resource.Id.textView2).Text = item.Description;
                view.FindViewById<ImageView>(Resource.Id.imageView1).SetImageResource(item.Image);

                return view;
            }
        }

    }
}