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
using Microsoft.WindowsAzure.MobileServices;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Windows.Storage;
using System.Net.Http;
using Newtonsoft.Json;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Roundtown
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class EditPage : Page
    {
        public EditPage()
        {
            this.InitializeComponent();
        }

        public int isValid { get; set; }

        public class Event
        {
            public string ID { get; set; }
            public string name { get; set; }
            public string location { get; set; }
            public string date { get; set; }
            public string description { get; set; }
        }

        async private void updateBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                Event itemReg = new Event
                {
                    ID = eventID.Text,
                    name = eventName.Text,
                    location = eventLocation.Text,
                    date = eventDate.Text,
                    description = eventDescription.Text

                };
                await App.MobileService.GetTable<Event>().UpdateAsync(itemReg);
                var Update_Dialog = new MessageDialog("Successful! Your event: " + itemReg.name + " has been updated.");
                await Update_Dialog.ShowAsync();

            }
            catch (Exception em)
            {
                var dialog = new MessageDialog("An Error Occured: " + em.Message);
                await dialog.ShowAsync();

            }
        }
    }
}
