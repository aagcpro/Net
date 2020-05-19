using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Camara_de_Diputados
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

            textboxUri.Text = "http://www.diputados.gob.mx/inicio.htm";
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