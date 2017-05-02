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
using Windows.UI.Core;


// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Roundtown
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            btnSpecials.IsEnabled = false;
        }

        void GoBack()
        {
            if (MainFrame != null && MainFrame.CanGoBack) MainFrame.GoBack();
        }


        private void hamburger_Click(object sender, RoutedEventArgs e)
        {
            MySplitView.IsPaneOpen = MySplitView.IsPaneOpen ? false : true;
        }

        private void nav_Click(object sender, RoutedEventArgs e)
        {
            var radioButton = sender as RadioButton;

            if (radioButton != null)
            {
                switch (radioButton.Tag.ToString())
                {
                    case "Back":
                        GoBack();
                        break;
                    case "Home":
                        Frame.Navigate(typeof(MainPage));
                        break;
                    case "Login":
                        MainFrame.Navigate(typeof(Login));
                        break;
                    case "View":
                        MainFrame.Navigate(typeof(DatabaseControls.DisplayPage));
                        break;
                    case "Add":
                        MainFrame.Navigate(typeof(AddPage));
                        break;
                    case "About":
                        MainFrame.Navigate(typeof(About));
                        break;
                }
                MySplitView.IsPaneOpen = false;
            }
        }


        private void btnSpecials_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnEvents_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(typeof(DatabaseControls.DisplayPage));
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(typeof(Login));
        }

        private void btn_About_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(typeof(About));
        }
    }
}
