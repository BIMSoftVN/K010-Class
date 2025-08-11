using DevExpress.Xpf.Core;
using iCons.Classes;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace iCons
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static string ConnectionString = $"Server=./;Database=iCons;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=True;";
        public static efUser MainUser { get; set; } = new efUser();

        protected override void OnStartup(StartupEventArgs e)
        {
            ApplicationThemeHelper.ApplicationThemeName = Theme.Win11LightName;
            base.OnStartup(e);
        }
    }
}
