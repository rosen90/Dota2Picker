using System;
using System.IO;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using System.Threading.Tasks;
using System.Text;
using Dota2Picker.Objects;
using Windows.UI.ViewManagement;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Dota2Picker
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SettingsPage : Page
    {
        private StreamReader sr;
        private static int langIdx;
        public SettingsPage()
        {
            this.InitializeComponent();

            configLanguages();
            Window.Current.SizeChanged += configLanguages;
            readSelectedLang();

        }

        public async Task<StreamReader> getStreamReader(string fileName)
        {
            var file = await ApplicationData.Current.LocalFolder.GetFileAsync(fileName + ".txt");
            var inputStream = await file.OpenReadAsync();
            var classicStream = inputStream.AsStreamForRead();
            var streamReader = new StreamReader(classicStream);
            return streamReader;
        }

        public async Task<StreamWriter> getStreamWriter(string fileName)
        {
            var file = await ApplicationData.Current.LocalFolder.GetFileAsync(fileName + ".txt");
            var inputStream = await file.OpenAsync(FileAccessMode.ReadWrite);
            var classicStream = inputStream.AsStreamForWrite();
            var streamWriter = new StreamWriter(classicStream);
            return streamWriter;
        }

        private async void readSelectedLang()
        {
            Languages.ItemsSource = Constants.tipsLenguages;
            StreamReader strRead;
            strRead = await getStreamReader("SelectedLang");
            langIdx = 0;
            Int32.TryParse(strRead.ReadLine(), out langIdx);
            Languages.SelectedIndex = langIdx;
            strRead.Dispose();
            
        }

        private string getTextByIdentifier( StreamReader sr, string id)
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

        protected override void OnNavigatedTo(NavigationEventArgs e)
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

        private void configLanguages(object sender, Windows.UI.Core.WindowSizeChangedEventArgs e)
        {
            var bounds = ApplicationView.GetForCurrentView().VisibleBounds;
            Languages.Width = bounds.Width / 2;

            //sr = await getStreamReader("SelectedLang");

            //Languages.SelectedIndex = readSelectedLang(sr);
            
        }


        private void configLanguages()
        {
            var bounds = ApplicationView.GetForCurrentView().VisibleBounds;
            Languages.Width = bounds.Width / 2;


            //sr = await getStreamReader("SelectedLang");
            
            //Languages.SelectedIndex = readSelectedLang(sr);
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
            sr = await getStreamReader("Settings");
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

        private async void Languages_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Pass the filepath and filename to the StreamWriter Constructor
            StreamWriter sw = await getStreamWriter("SelectedLang");

            //Write a line of text
            sw.WriteLine(Languages.SelectedIndex.ToString());
            await sw.FlushAsync();

            sw.Dispose();
        }
    }
}
