using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Microsoft.WindowsAzure.MobileServices;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Roundtown.Models;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Roundtown.DatabaseControls
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DisplayPage : Page
    {
        public DisplayPage()
        {
            this.InitializeComponent();
        }

        private void ProcessBegin()
        {
            processRing.IsActive = true;
            ButtonRefresh.IsEnabled = false;
        }

        private void ProcessEnd()
        {
            processRing.IsActive = false;
            ButtonRefresh.IsEnabled = true;
        }

        private MobileServiceCollection<Event, Event> items;
        private IMobileServiceTable<Event> eventTable = App.MobileService.GetTable<Event>();
        

        private async void ButtonRefresh_Click(object sender, RoutedEventArgs e)
        {
            await RefreshEvents();
        }

        private async Task RefreshEvents()
        {
            MobileServiceInvalidOperationException exception = null;
            try
            {
                ProcessBegin();
                items = await eventTable
                    .Where(Event => Event.deleted == false)
                    .ToCollectionAsync();
             
            }
            catch (MobileServiceInvalidOperationException e)
            {
                exception = e;
            }

            if (exception != null)
            {
                await new MessageDialog(exception.Message, "Error loading items").ShowAsync();
            }
            else
            {
                ListItems.ItemsSource = items;
            }
            ProcessEnd();
        }

        private async Task UpdateCheckedTodoItem(Event item)
        {
            try
            {


                //The following code illustrates how to delete an existing instance. The instance is identified by the "Id" field set on the todoItem.
                await eventTable.DeleteAsync(item);

                ListItems.Focus(FocusState.Unfocused);


            }
            catch (Exception e)
            {
                MessageDialog messageDialog = new MessageDialog("An Error Occurred: " + e.Message, "Windows 8");
                await messageDialog.ShowAsync();
            }
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            await RefreshEvents();
        }

        private void eventClick_Click(object sender, RoutedEventArgs e)
        {
            string ID = ((Button)sender).Tag.ToString();
            Frame.Navigate(typeof(EventDetail), ID);
        }
    }
}
