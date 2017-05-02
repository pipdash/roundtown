using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Microsoft.WindowsAzure.MobileServices;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Roundtown.Models;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Roundtown
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class EventDetail : Page
    {
        public EventDetail()
        {
            this.InitializeComponent();
        }

        private void ProcessBegin()
        {
            processRing.IsActive = true;
        }

        private void ProcessEnd()
        {
            processRing.IsActive = false;
        }

        private MobileServiceCollection<Event, Event> details;
        private IMobileServiceTable<Event> eventTable = App.MobileService.GetTable<Event>();

        private async Task LoadEvent(string eventID)
        {
            MobileServiceInvalidOperationException exception = null;

            try
            {
                details = await eventTable
                    .Where(Event => Event.ID == eventID)
                    .ToCollectionAsync();

                eventDetailList.ItemsSource = details;
            }
            catch
            {
                var e = exception;
            }
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            ProcessBegin();
            string eventID = e.Parameter.ToString();
            await LoadEvent(eventID);
            ProcessEnd();
        }
    }
}


