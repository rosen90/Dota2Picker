using System;
using System.IO;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using System.Threading.Tasks;
using System.Text;
using Dota2Picker.Objects;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Dota2Picker
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SettingsPage : Page
    {
        private StreamReader sr;
        public SettingsPage()
        {
            this.InitializeComponent();
            //ReadSettingsTexts();
        }

        public async Task<StreamReader> getStreamReader()
        {
            var file = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Resources/Multilanguage/Settings/Settings.txt"));
            var inputStream = await file.OpenReadAsync();
            var classicStream = inputStream.AsStreamForRead();
            var streamReader = new StreamReader(classicStream);
            return streamReader;
        }

        public string getTextByIdentifier( StreamReader sr, string id)
        {
            StringBuilder temp = new StringBuilder();
            bool start = false;

            using (sr)
            {
                while ( sr.Peek() >= 0)
                {
                    string text = sr.ReadLine();
                    if ( start && (text == id))
                    {
                        break;
                    }

                    if ( !start && (text == id))
                    {
                        start = true;
                    }
                    else if ( start && (text != id))
                    {
                        temp.Append(text);
                    }
                }
            }
                return temp.ToString();
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            //sr = await getStreamReader();
            //AboutDescr.Text = await Task.Run(() => getTextByIdentifier (sr, Constants.settingsIdentifiers[pivotIndex]));
        }

        private void ReadSettingsTexts(object sender, Windows.UI.Core.WindowSizeChangedEventArgs e)
        {
            int test = settingsPivot.SelectedIndex;
        }

        private void HamburgerButton_Click(object sender, RoutedEventArgs e)
        {
            MySplitView.IsPaneOpen = !MySplitView.IsPaneOpen;
        }

        private void IconsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage), IconsListBox.SelectedIndex);
        }

        private void SettingsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Temporary
            if (SettingsListBox.SelectedIndex == 1)
            {
                this.Frame.Navigate(typeof(SettingsPage));
            }
        }

        private async void settingsPivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            sr = await getStreamReader();
            int pivotIndex = settingsPivot.SelectedIndex;

            switch (pivotIndex)
            {
                case 0:
                    SettingsDescr.Text = await Task.Run(() => getTextByIdentifier(sr, Constants.settingsIdentifiers[pivotIndex]));
                    break;
                case 1:
                    AboutDescr.Text = await Task.Run(() => getTextByIdentifier(sr, Constants.settingsIdentifiers[pivotIndex]));
                    break;
                case 2:
                    PrivacyDescr.Text = await Task.Run(() => getTextByIdentifier(sr, Constants.settingsIdentifiers[pivotIndex]));
                    break;
            }
        }
    }
}
