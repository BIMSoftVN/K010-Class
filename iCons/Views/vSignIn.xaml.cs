﻿using iCons.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace iCons.Views
{
    /// <summary>
    /// Interaction logic for vSignIn.xaml
    /// </summary>
    public partial class vSignIn : Window
    {
        public vSignIn()
        {
            InitializeComponent();
            (this.DataContext as vmSignIn).Win = this;
        }
    }
}
