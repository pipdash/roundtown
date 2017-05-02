using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

using System.Threading.Tasks;
using Windows.Devices.Geolocation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Roundtown
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class GeoLocation : Page
    {
        public GeoLocation()
        {
            this.InitializeComponent();
        }

        public async Task findLocation()
        {
            Geolocator geolocator = new Geolocator();
            geolocator.DesiredAccuracyInMeters = 50;

            try
            {
                Geoposition geoposition = await geolocator.GetGeopositionAsync(
                    maximumAge: TimeSpan.FromMinutes(5),
                    timeout: TimeSpan.FromSeconds(10)
                    );
                LatitudeTextBlock.Text = "Latitude: " + geoposition.Coordinate.Latitude.ToString("0.00");
                LongitudeTextBlock.Text = "Longitude: " + geoposition.Coordinate.Longitude.ToString("0.00");
            }

            catch (Exception ex)
            {
                if ((uint)ex.HResult == 0x80004004)
                {
                    StatusTextBlock.Text = "location  is disabled in phone settings.";
                }

                else

                {
                    StatusTextBlock.Text = "Unknown error while trying to find location";
                }
            }
        }

        private async void OneShotLocation_Click(object sender, RoutedEventArgs e)

        {
            await findLocation();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            await findLocation();
        }
    }
}
