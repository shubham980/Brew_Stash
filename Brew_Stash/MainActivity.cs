﻿using System;
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

namespace Brew_Stash
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
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

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            fusedLocationProviderClient = LocationServices.GetFusedLocationProviderClient(this);
            var location = fusedLocationProviderClient.GetLastLocationAsync();


            MapFragment mapFragment = (MapFragment)FragmentManager.FindFragmentById(Resource.Id.map);
            mapFragment.GetMapAsync(this);
        }


        public async void OnMapReady(GoogleMap map)
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

            var result = await NearByPlaceSearch(nearbyQuery, lat.ToString().Replace(",","."), lon.ToString().Replace(",","."), radius, typeSearch, typeSearch, "");

            var listData = new ObservableCollection<Data.Result>();
            if (result != null)
            {
                foreach (var item in result)
                {
                    listData.Add(item);
                }
                System.Diagnostics.Debug.WriteLine("Total result: " + listData.Count);
            }


            AddLocationMarkers(map, listData);
        }


        public async Task<List<Data.Result>> NearByPlaceSearch(string googleQuery, string lat, string lng, string radius, string type, string keyword, string nextPageToken)
        {
            pagetoken = nextPageToken != null ? "&pagetoken=" + nextPageToken : null;
            var requestUri = string.Format(googleQuery, lat, lng, radius, type, keyword) + pagetoken;
            try
            {
                var restClient = new RestClient<Data.RootObject>();
                var result = await restClient.GetAsync(requestUri);
                var tempResult = result.results;
                pagetoken = result.next_page_token;
                if (pagetoken != null)
                {
                    while (pagetoken != null)
                    {
                        await System.Threading.Tasks.Task.Delay(2000);
                        restClient = new RestClient<Data.RootObject>();
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

        public void AddLocationMarkers(GoogleMap map, ObservableCollection<Data.Result> list)
        {
            foreach(var item in list)
            {
                MarkerOptions markerOpt1 = new MarkerOptions();
                markerOpt1.SetPosition(new LatLng(item.geometry.location.lat, item.geometry.location.lng));
                markerOpt1.SetTitle(item.name);
                map.AddMarker(markerOpt1);
            }
        }

    }
}

