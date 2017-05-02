using System;
using System.IO;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Popups;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Web.Http;
using System.Threading;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Windows.Foundation;
using Roundtown.Models;
using Windows.UI.Xaml.Media.Imaging;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Roundtown
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AddPage : Page
    {
        public AddPage()
        {
            this.InitializeComponent();
            insertBtn.IsEnabled = false;
        }


        public string PhotoURI = null;

        private void ProcessBegin()
        {
            processRing.IsActive = true;
            uploadBtn.IsEnabled = false;
            insertBtn.IsEnabled = false;
            eventName.IsEnabled = false;
            eventLocation.IsEnabled = false;
            eventDate.IsEnabled = false;
            eventDescription.IsEnabled = false;
        }

        private void ProcessEnd()
        {
            processRing.IsActive = false;
            uploadBtn.IsEnabled = true;
            eventName.IsEnabled = true;
            eventLocation.IsEnabled = true;
            eventDate.IsEnabled = true;
            eventDescription.IsEnabled = true;
        }

        async private void insertBtn_Click(object sender, RoutedEventArgs e)
        {
            if (eventName.Text != "" && eventLocation.Text != "" && eventDate.Text != "" && eventDescription.Text != "")
            {

                try
                {

                    Event itemReg = new Event
                    {
                        photoID = PhotoURI,
                        ID = Guid.NewGuid().ToString("N"),
                        name = eventName.Text,
                        location = eventLocation.Text,
                        date = eventDate.Text,
                        description = eventDescription.Text

                    };

                    await App.MobileService.GetTable<Event>().InsertAsync(itemReg);
                    var dialog = new MessageDialog("Successful! Your event: " + itemReg.name + " has been created.\nNew event ID: " + itemReg.ID + "\nPlease take note of this ID, as it will be required to update or delete your listing");
                    await dialog.ShowAsync();


                }
                catch (Exception em)
                {
                    var dialog = new MessageDialog("An Error Occured: " + em.Message);
                    await dialog.ShowAsync();

                }
            }
            else
            {
                var dialog = new MessageDialog("You must complete all above fields before creating your event.");
                await dialog.ShowAsync();
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private async void refreshListview()
        {
            try
            {
                await App.container.CreateIfNotExistsAsync();
                BlobContinuationToken token = null;
                var tast = App.container.ListBlobsSegmentedAsync(token);
                var blobsSegmented = await tast;
                ProcessEnd();

            }
            catch (Exception)
            {
                // Failed to refresh
            }
        }

        private async void uploadBtn_Click(object sender, RoutedEventArgs e)
        {
            FileOpenPicker fileOpenPicker = new FileOpenPicker();
            fileOpenPicker.ViewMode = PickerViewMode.Thumbnail;
            fileOpenPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            fileOpenPicker.FileTypeFilter.Add(".jpg");
            fileOpenPicker.FileTypeFilter.Add(".jpeg");
            fileOpenPicker.FileTypeFilter.Add(".png");
            ProcessBegin();

            StorageFile inputFile = await fileOpenPicker.PickSingleFileAsync();


            if (inputFile != null)
            {
                using (var fileStreamer = await inputFile.OpenSequentialReadAsync())
                {
                    try
                    {
                        await App.container.CreateIfNotExistsAsync();
                        var blob = App.container.GetBlockBlobReference(inputFile.Name);
                        await blob.DeleteIfExistsAsync();
                        await blob.UploadFromStreamAsync(fileStreamer.AsStreamForRead());
                        var dialog = new MessageDialog("Your photo has been uploaded!");
                        await dialog.ShowAsync();
                        PhotoURI = blob.Uri.ToString();
                        blobImg.Source = new BitmapImage(blob.Uri);
                        refreshListview();
                        insertBtn.IsEnabled = true;

                    }
                    catch (Exception em)
                    {
                        ProcessEnd();
                        var dialog = new MessageDialog("Error uploading photo. Please try again. -- " + em);
                        await dialog.ShowAsync();
                    }
                }
            }

            else
            {
                ProcessEnd();
            }      
        }
    }
}
