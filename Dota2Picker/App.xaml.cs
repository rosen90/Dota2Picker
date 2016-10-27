using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Dota2Picker.Objects;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.Storage;

namespace Dota2Picker
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {

        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;

            checkIfFileExist();
        }

        private async void checkIfFileExist()
        {
            if (!await CheckDbAsync("Settings"))
            {
                await CopyFile("Settings");
            }

            if (!await CheckDbAsync("SelectedLang"))
            {
                await CopyFile("Selectedlang");
            }
        }

        public async Task<bool> CheckDbAsync(string fileName)
        {
            bool fileExist = true;

            try
            {
                StorageFile sf = await ApplicationData.Current.LocalFolder.GetFileAsync(fileName + ".txt");
            }
            catch (Exception)
            {
                fileExist = false;
            }

            return fileExist;
        }


        public async Task CopyFile(string fileName)
        {
            bool isFileExisting = false;

            try
            {
                StorageFile storageFile = await ApplicationData.Current.LocalFolder.GetFileAsync(fileName + ".txt");
                isFileExisting = true;
            }
            catch
            {
                isFileExisting = false;
            }

            if (!isFileExisting)
            {
                var file = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Resources/Multilanguage/Settings/" + fileName + ".txt"));
                await file.CopyAsync(ApplicationData.Current.LocalFolder);
            }
        }

        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            BaseViewObject.bvoInstance.lastHeroView = 0;
            Frame rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;
                rootFrame.Navigated += OnNavigated;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    // TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
                
            }
            if ( e.PrelaunchActivated == false)
            { 
                if (rootFrame.Content == null)
                {
                    // When the navigation stack isn't restored navigate to the first page,
                    // configuring the new page by passing required information as a navigation
                    // parameter
                    rootFrame.Navigate(typeof(MainPage), e.Arguments);
                }

                // Ensure the current window is active
                Window.Current.Activate();
            }
        }

        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }
        private void OnNavigated(object sender, NavigationEventArgs e)
        {
            SystemNavigationManager.GetForCurrentView().BackRequested += OnBackRequested;
            // Each time a navigation event occurs, update the Back button's visibility
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                ((Frame)sender).CanGoBack ?
                AppViewBackButtonVisibility.Visible :
                AppViewBackButtonVisibility.Collapsed;
        }

        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            // TODO: Save application state and stop any background activity
            deferral.Complete();
        }

        private void OnBackRequested(object sender, BackRequestedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;

            if (rootFrame.CanGoBack)
            {
                e.Handled = true;
                rootFrame.GoBack();
            }
        }
    }
}
