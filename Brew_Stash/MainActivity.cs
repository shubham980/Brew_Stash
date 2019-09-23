using System;
using Android.App;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.OS;
using Android.Support.V7.App;
using Android.Gms.Location;
using System.Threading.Tasks;
using System.Collections.Generic;
using Brew_Stash.RestClient;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using Android.Content;
using System.IO;

namespace Brew_Stash
{
    [Activity(Label = "Brew Stash", Theme = "@style/MyTheme.Splash", MainLauncher = true)]
    public class MainActivity : AppCompatActivity, IOnMapReadyCallback
    {
        private static readonly string PlaceAPIkey = "AIzaSyChmYLvTZu0eb6iCj2JZ4gRkqlyNXnuTkw";

        private FusedLocationProviderClient fusedLocationProviderClient;
        double lat;
        double lon;
        public string pagetoken = "";
        private string googleQuery = "https://maps.googleapis.com/maps/api/place/textsearch/json?query={0}+{1}&type={2}&language=it&key=" + PlaceAPIkey;
        private readonly string nearbyQuery = "https://maps.googleapis.com/maps/api/place/nearbysearch/json?location={0},{1}&radius={2}&type={3}&keyword={4}&key=" + PlaceAPIkey;
        public string radius = "10000";
        public string typeSearch = "Cafe";
        public static FinalOrder finalOrder;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Forms.Init(this, savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            finalOrder = new FinalOrder();

            fusedLocationProviderClient = LocationServices.GetFusedLocationProviderClient(this);
            var location = fusedLocationProviderClient.GetLastLocationAsync();

            MapFragment mapFragment = (MapFragment)FragmentManager.FindFragmentById(Resource.Id.map);
            mapFragment.GetMapAsync(this);
            
        }


        public async void OnMapReady(GoogleMap map)
        {
            try
            {
                map.MapType = GoogleMap.MapTypeNormal;
                map.UiSettings.ZoomControlsEnabled = true;
                map.UiSettings.CompassEnabled = true;

                var currentLocation = await fusedLocationProviderClient.GetLastLocationAsync();
                lat = currentLocation.Latitude;
                lon = currentLocation.Longitude;

                LatLng location = new LatLng(lat, lon);

                CameraPosition.Builder builder = CameraPosition.InvokeBuilder();
                builder.Target(location);
                builder.Zoom(16);
                builder.Bearing(155);

                CameraPosition cameraPosition = builder.Build();

                CameraUpdate cameraUpdate = CameraUpdateFactory.NewCameraPosition(cameraPosition);

                map.MoveCamera(cameraUpdate);

                MarkerOptions markerOpt1 = new MarkerOptions();
                markerOpt1.SetPosition(new LatLng(lat, lon));
                markerOpt1.SetTitle("My Position");
                map.AddMarker(markerOpt1);

                var result = await NearByPlaceSearch(nearbyQuery, lat.ToString().Replace(",", "."), lon.ToString().Replace(",", "."), radius, typeSearch, typeSearch, "");

                var listData = new ObservableCollection<SearchData.Result>();
                if (result != null)
                {
                    foreach (var item in result)
                    {
                        listData.Add(item);
                    }
                    System.Diagnostics.Debug.WriteLine("Total result: " + listData.Count);
                }
                map.InfoWindowClick += MapOnInfoWindowClick;

                AddLocationMarkers(map, listData);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            
        }


        public async Task<List<SearchData.Result>> NearByPlaceSearch(string googleQuery, string lat, string lng, string radius, string type, string keyword, string nextPageToken)
        {
            pagetoken = nextPageToken != null ? "&pagetoken=" + nextPageToken : null;
            var requestUri = string.Format(googleQuery, lat, lng, radius, type, keyword) + pagetoken;
            try
            {
                var restClient = new RestClient<SearchData.RootObject>();
                var result = await restClient.GetAsync(requestUri);
                var tempResult = result.results;
                pagetoken = result.next_page_token;
                if (pagetoken != null)
                {
                    while (pagetoken != null)
                    {
                        await System.Threading.Tasks.Task.Delay(2000);
                        restClient = new RestClient<SearchData.RootObject>();
                        result = await restClient.GetAsync(requestUri + pagetoken);
                        foreach (var item in result.results)
                        {
                            tempResult.Add(item);
                        }
                        pagetoken = result.next_page_token;
                    }
                }
                if (tempResult.Count == 0)
                {
                    return null;
                }
                return tempResult;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Near by place search error: " + e.Message);
            }
            return null;
        }

        public void AddLocationMarkers(GoogleMap map, ObservableCollection<SearchData.Result> list)
        {
            foreach(var item in list)
            {
                MarkerOptions markerOpt1 = new MarkerOptions();
                markerOpt1.SetPosition(new LatLng(item.geometry.location.lat, item.geometry.location.lng));
                markerOpt1.SetTitle(item.name);
                map.AddMarker(markerOpt1);
            }
        }

        private async void MapOnInfoWindowClick(object sender, GoogleMap.InfoWindowClickEventArgs e)
        {
            Marker myMarker = e.Marker;
            // Do something with marker.
            Console.WriteLine("Marker Clicked");
            if (myMarker.Title != "My Position")
            {
                finalOrder.Cafe = myMarker.Title;
                var intent = new Intent(this, typeof(CoffeeList));
                this.StartActivity(intent);
            }
        }

        public static Database database;

        public static Database Database
        {
            get
            {
                if (database == null)
                {
                    database = new Database(Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData), "Orders.db3"));
                }
                return database;
            }
        }
    }
}

