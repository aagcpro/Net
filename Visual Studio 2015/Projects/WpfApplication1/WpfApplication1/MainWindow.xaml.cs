﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApplication1
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
                    
        }
        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Employee c1 = new Employee();
            c1 = new Employee();
             c1.Name = "Linda";

                 

            Output.Text = c1.Name;  

        }
    }
}
