using Mapsui;
using Mapsui.Extensions;
using Mapsui.Layers;
using Mapsui.Nts;
using Mapsui.Nts.Extensions;
using Mapsui.Projections;
using Mapsui.Styles;
using Mapsui.Tiling;
using Mapsui.UI.Maui;
using Mapsui.Utilities;
using NetTopologySuite.Geometries;

namespace schoolbus_mobile
{
    public partial class MainPage : ContentPage
    {
        Pin r_pin;
        Boolean r_lod = false;

        public MainPage()
        {
            InitializeComponent();

            mapView.Map.Layers.Add(OpenStreetMap.CreateTileLayer());
            mapView.Map.CRS = "EPSG:3857";
            mapView!.RotationLock = true;
            mapView.IsMyLocationButtonVisible = false;
            mapView.IsNorthingButtonVisible = false;
            mapView.IsZoomButtonVisible = false;
            mapView.Map.Home = (p_nav) =>
            {
                p_nav.CenterOnAndZoomTo(SphericalMercator.FromLonLat(31.23666412240008, 30.04759540087698).ToMPoint(), p_nav.Resolutions[9]);
            };
        }

        async Task v_track_location()
        {
            await Application.Current.Dispatcher.DispatchAsync(async () =>
            {
                try
                {
                    var l_req = new GeolocationListeningRequest(GeolocationAccuracy.Best);
                    var l_scs = await Geolocation.StartListeningForegroundAsync(l_req);
                    if (!l_scs)
                    {
                        await DisplayAlert("في الطريق", "من فضلك قم بتفعيل الخدمة الموقع", "حسنا");
                        return;
                    }
                    Geolocation.LocationChanged += v_location_changed;
                }
                catch (Exception p_exp)
                {
                    await DisplayAlert("في الطريق", "من فضلك قم بتفعيل الخدمة الموقع", "حسنا");
                }
            });
        }

        async void v_location_changed(object? p_snd, GeolocationLocationChangedEventArgs p_arg)
        {
            if (!r_lod)
            {
                r_lod = true;
                await v_center(p_arg.Location.Latitude, p_arg.Location.Longitude);
            }

            Application.Current.Dispatcher.DispatchAsync(async () =>
            {
                mapView.MyLocationLayer.UpdateMyLocation(new Mapsui.UI.Maui.Position(p_arg.Location.Latitude, p_arg.Location.Longitude));
            });
        }

        async Task v_center(double p_lat, double p_lng)
        {
            Application.Current.Dispatcher.DispatchAsync(async () =>
            {
                var p_sph = SphericalMercator.FromLonLat(p_lng, p_lat).ToMPoint();
                mapView.Map.Navigator.CenterOnAndZoomTo(p_sph, 3, 1);
            });
        }

        IStyle f_LineStringStyle()
        {
            return new VectorStyle
            {
                Fill = null,
                Outline = null,
                Line =
                {
                    Color = Mapsui.Styles.Color.DodgerBlue,
                    Width = 10,
                    PenStrokeCap = PenStrokeCap.Round,
                    StrokeJoin = StrokeJoin.Round
                }
            };
        }

        List<(double x, double y)> l_lst = new List<(double x, double y)>
        {
            (31.290791, 30.090732),
            (31.1, 30.1),
            (31.2, 30.0)
        };

        ILayer f_LineStringLayer(IStyle? style = null)
        {
            var lineString = new LineString(new Coordinate[]
            {
                SphericalMercator.FromLonLat(l_lst[0].x, l_lst[0].y).ToCoordinate(),
                SphericalMercator.FromLonLat(l_lst[1].x, l_lst[1].y).ToCoordinate(),
                SphericalMercator.FromLonLat(l_lst[2].x, l_lst[2].y).ToCoordinate()
            });

            return new MemoryLayer
            {
                Features = new[] { new GeometryFeature { Geometry = lineString } },
                Name = "LineStringLayer",
                Style = style
            };
        }

        IEnumerable<IFeature> CreateFeatures()
        {
            var pinId = typeof(MainPage).LoadSvgId("bus.svg");

            return new List<IFeature>
            {
                new PointFeature(SphericalMercator.FromLonLat(31.290791, 30.090732).ToMPoint())
                {
                    Styles = new[]
                    {
                        new SymbolStyle
                        {
                            BitmapId = pinId,
                            SymbolScale = 0.1
                        }
                    }
                }
            };
        }

        async void v_add_route(object p_snd, EventArgs p_arg)
        {
            mapView.Map.Layers.Add(f_LineStringLayer(f_LineStringStyle()));
        }

        async void v_add_pin(object p_snd, EventArgs p_arg)
        {
            await v_track_location();

            r_pin = new Pin
            {
                Label = "Label",
                Position = new Mapsui.UI.Maui.Position(31.141589272680907, 29.933224729659024),
                Type = PinType.Svg,
                Svg = await LoadSVG(),
                Scale = 0.1f,
            };

            ObservableRangeCollection<Pin> l_pns = (ObservableRangeCollection<Pin>)mapView.Pins;
            l_pns.Add(r_pin);

            await v_move_pin(30.012779148318284, 32.44461128874855);
        }

        async Task v_move_pin(double p_lat, double p_lng)
        {
            (double g_lat, double g_lng) l_old = (r_pin.Position.Latitude, r_pin.Position.Longitude);

            var l_anm = new Animation();
            l_anm.Add(0, 1, new Animation((p_val => r_pin.Position = new Mapsui.UI.Maui.Position(p_val, r_pin.Position.Longitude)), l_old.g_lat, p_lat));
            l_anm.Add(0, 1, new Animation((p_val => r_pin.Position = new Mapsui.UI.Maui.Position(r_pin.Position.Latitude, p_val)), l_old.g_lng, p_lng));
            l_anm.Commit(this, "MovePin", rate: 1, length: 3000, easing: Easing.Linear, repeat: () => false);
        }

        async Task<string> LoadSVG()
        {
            using var l_stm = await FileSystem.OpenAppPackageFileAsync("bus.svg");
            using var l_rdr = new StreamReader(l_stm);

            var l_svg = l_rdr.ReadToEnd();
            return l_svg;
        }
    }
}