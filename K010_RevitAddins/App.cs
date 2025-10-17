using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace K010_RevitAddins
{
    class App : IExternalApplication
    {
        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }

        public Result OnStartup(UIControlledApplication application)
        {

            string TabName = "K010_Addins"; 
            string PanelName = "K010_Panel";

            application.CreateRibbonTab(TabName);

            var panel = application.CreateRibbonPanel(TabName, PanelName);

            string assemblyName = typeof(App).Assembly.Location;
            string classNameOpenWin = "K010_RevitAddins.ImportCAD.OpenWindow";

            var btnOpenWin = new PushButtonData("MoCuaSo", "Mở giao diện", assemblyName, classNameOpenWin);
            btnOpenWin.ToolTip = "Mở giao diện người dùng";

            btnOpenWin.Image = mHelper.ConvertResourceToImgSource("K010_RevitAddins.Photo.SolidWorks.png", 16);
            btnOpenWin.LargeImage = mHelper.ConvertResourceToImgSource("K010_RevitAddins.Photo.SolidWorks.png", 32);


            panel.AddItem(btnOpenWin);


            return Result.Succeeded;    

        }
    }
}
