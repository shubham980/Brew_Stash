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
using Android.Runtime;
using Android.Support.V4.App;
using Android;
using Android.Util;
using Android.Support.Design.Widget;
using Android.Widget;

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

        /// <summary>
        /// Loads all of the needed elements
        /// </summary>
        /// <param name="savedInstanceState"></param>
        protected async override void OnCreate(Bundle savedInstanceState)
        {
            try
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
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            
        }

        /// <summary>
        /// When map is connected, gets current location, searches for nearest cafes and puts markers on the map
        /// </summary>
        /// <param name="map">our map</param>

        public async void OnMapReady(GoogleMap map)
        {
            try
            {
                await TryToGetPermissions();

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

        /// <summary>
        /// Requests neariest cafes data from Google Places API
        /// </summary>
        /// <param name="googleQuery"></param>
        /// <param name="lat"></param>
        /// <param name="lng"></param>
        /// <param name="radius"></param>
        /// <param name="type"></param>
        /// <param name="keyword"></param>
        /// <param name="nextPageToken"></param>
        /// <returns></returns>

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

        /// <summary>
        /// Adds location markers on the map
        /// </summary>
        /// <param name="map">used map</param>
        /// <param name="list">list of places</param>

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

        /// <summary>
        /// When cafe name is clicked, takes to next page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

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

        /// <summary>
        /// Starts a database
        /// </summary>

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


        #region RuntimePermissions

        /// <summary>
        /// Checks SDK version and then looks for permissions
        /// </summary>
        /// <returns></returns>

        async Task TryToGetPermissions()
        {
            if ((int)Build.VERSION.SdkInt >= 23)
            {
                await GetPermissionsAsync();
                return;
            }


        }
        const int RequestLocationId = 0;

        /// <summary>
        /// Needed permissions
        /// </summary>

        readonly string[] PermissionsGroupLocation =
            {
                            //TODO add more permissions
                            Manifest.Permission.AccessCoarseLocation,
                            Manifest.Permission.AccessFineLocation,
             };

        /// <summary>
        /// Checks whether or not permissions are granted. Reports the result
        /// </summary>
        /// <returns></returns>

        async Task GetPermissionsAsync()
        {
            const string permission = Manifest.Permission.AccessFineLocation;

            if (CheckSelfPermission(permission) == (int)Android.Content.PM.Permission.Granted)
            {
                Toast.MakeText(this, "Location permissions granted", ToastLength.Short).Show();
                return;
            }

            if (ShouldShowRequestPermissionRationale(permission))
            {
                await ShowDialog();

                return;
            }

            RequestPermissions(PermissionsGroupLocation, RequestLocationId);

        }

        /// <summary>
        /// Shows popup window which requests permissions to be granted
        /// </summary>
        /// <returns></returns>

        private Task<bool> ShowDialog()
        {
            var tcs = new TaskCompletionSource<bool>();
            //set alert for executing the task
            Android.App.AlertDialog.Builder alert = new Android.App.AlertDialog.Builder(this);
            alert.SetTitle("Permissions Needed");
            alert.SetMessage("The application needs location permissions to continue");
            alert.SetPositiveButton("Request Permissions", (senderAlert, args) =>
            {
                RequestPermissions(PermissionsGroupLocation, RequestLocationId);
            });

            alert.SetNegativeButton("Cancel", (senderAlert, args) =>
            {
                Toast.MakeText(this, "Cancelled!", ToastLength.Short).Show();
                
            });

            Dialog dialog = alert.Create();
            dialog.Show();
            
            return tcs.Task;
        }

        /// <summary>
        /// When permissions result is received, reports what happened
        /// </summary>
        /// <param name="requestCode"></param>
        /// <param name="permissions"></param>
        /// <param name="grantResults"></param>

        public override async void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            switch (requestCode)
            {
                case RequestLocationId:
                    {
                        if (grantResults[0] == (int)Android.Content.PM.Permission.Granted)
                        {
                            Toast.MakeText(this, "Location permissions granted", ToastLength.Short).Show();

                        }
                        else
                        {
                            //Permission Denied :(
                            Toast.MakeText(this, "Location permissions denied", ToastLength.Short).Show();
                        }
                    }
                    break;
            }
            //base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        #endregion

    }
}

