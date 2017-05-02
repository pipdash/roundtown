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
using winsdkfb;
using Windows.UI.Popups;
using Microsoft.WindowsAzure.MobileServices;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Roundtown
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class FacebookAuth : Page
    {
        public FacebookAuth()
        {
            this.InitializeComponent();
            Initialization();
        }

        public class FacebookUsers
        {
            public string ID { get; set; }
            public string fname { get; set; }
            public string lname { get; set; }
            public string email { get; set; }
        }

        static void Initialization()
        {
            FBSession sess = FBSession.ActiveSession;
            sess.FBAppId = "289402944726162";
            sess.WinAppId = "s-1-15-2-1759067298-3589478606-2794354144-623115376-3120008208-2121077537-2149791539";
        }

        static async void Logout()
        {
            FBSession sess = FBSession.ActiveSession;
            await sess.LogoutAsync();
        }

        private async void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            Logout();
            var dialog = new MessageDialog("You are no longer logged in with Facebook");
            await dialog.ShowAsync();
        }

        private async void MainLogin_Click(object sender, RoutedEventArgs e)
        {
           
            // Get active session
            FBSession sess = FBSession.ActiveSession;

            // Add permissions required by the app
            List<String> permissionList = new List<String>();
            permissionList.Add("public_profile");
            permissionList.Add("user_friends");
            permissionList.Add("user_likes");
            FBPermissions permissions = new FBPermissions(permissionList);

            // Login to Facebook
            FBResult result = await sess.LoginAsync(permissions);

            if (result.Succeeded)
            {
                ProfilePic.UserId = sess.User.Id;
                var dialog = new MessageDialog("You have successfully logged in with Facebook!");
                await dialog.ShowAsync();

                try
                {
                    FacebookUsers authValid = new FacebookUsers
                    {
                        ID = sess.User.Id,
                        fname = sess.User.FirstName,
                        lname = sess.User.LastName,
                        email = sess.User.Email,

                    };

                    await App.MobileService.GetTable<FacebookUsers>().InsertAsync(authValid);
                    var added = new MessageDialog("Information saved to our database.");
                    await added.ShowAsync();
                }
                catch (Exception em)
                {
                    var added = new MessageDialog("An error occured attempting to add your email to our database. " + em.Message);
                    await added.ShowAsync();
                }

            }
            else
            {
                var dialog = new MessageDialog("An Error Occured when trying to login");
                await dialog.ShowAsync();
            }

        }
        
    }
}
