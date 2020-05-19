using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// La plantilla de elemento Página en blanco está documentada en http://go.microsoft.com/fwlink/?LinkId=234238

namespace Tesoreria_DF
{
   /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        
        List<string> sitesList = new List<string>();

        public MainPage()
        {
            this.InitializeComponent();

            textboxUri.Text = "http://www.finanzas.df.gob.mx/";
            WebView1.Navigate(new Uri(textboxUri.Text));
            sitesList.Add(textboxUri.Text);
        }


        private void Download_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                WebView1.Navigate(new Uri(textboxUri.Text));
            }
            catch (Exception exc)
            {
                textboxUri.Text = "Invalid adress !";
            }
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                WebView1.Navigate(new Uri(textboxUri.Text));
                sitesList.Add(textboxUri.Text);
            }
            catch (Exception exc)
            {
                textboxUri.Text = "Invalid adress !";
            }    
        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            //remove from list
            try
            {
                sitesList.RemoveAt(sitesList.IndexOf(textboxUri.Text));
            }
            catch (Exception exc){}
            
            //show previous page
            try
            {
                textboxUri.Text = sitesList[sitesList.Count - 1];
                WebView1.Navigate(new Uri(sitesList[sitesList.Count - 1]));
            }
            catch (Exception exc) { }
        }

        private void PrevWebPage_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int i = sitesList.IndexOf(textboxUri.Text) - 1;
                if (i < 0)
                {
                    textboxUri.Text = sitesList[sitesList.Count - 1];
                }
                else
                {
                    textboxUri.Text = sitesList[i];
                }

                WebView1.Navigate(new Uri(textboxUri.Text));
            }
            catch (Exception exc) { }
        }

        private void NextWebPage_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int i = sitesList.IndexOf(textboxUri.Text) + 1;
                if (i == sitesList.Count)
                {
                    textboxUri.Text = sitesList[0];
                }
                else
                {
                    textboxUri.Text = sitesList[i];
                }

                WebView1.Navigate(new Uri(textboxUri.Text));
            }
            catch (Exception exc) { }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void AppBarOpened(object sender, object e)
        {
            WebView1.Height -= 85;
        }

        private void AppBarClosed(object sender, object e)
        {
            WebView1.Height += 85;
        }
    }
}
