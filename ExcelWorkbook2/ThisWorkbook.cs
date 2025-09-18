using DevExpress.Xpf.Core;
using Microsoft.VisualStudio.Tools.Applications.Runtime;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
using Office = Microsoft.Office.Core;

namespace ExcelWorkbook2
{
    public partial class ThisWorkbook
    {
        private HelloControl _helloControl = new HelloControl();
        private void ThisWorkbook_Startup(object sender, System.EventArgs e)
        {
            ApplicationThemeHelper.ApplicationThemeName = DevExpress.Xpf.Core.Theme.Win11LightName;
            this.ActionsPane.Controls.Add(_helloControl);
        }

        private void ThisWorkbook_Shutdown(object sender, System.EventArgs e)
        {
        }

        #region VSTO Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InternalStartup()
        {
            this.Startup += new System.EventHandler(ThisWorkbook_Startup);
            this.Shutdown += new System.EventHandler(ThisWorkbook_Shutdown);
        }

        #endregion

    }
}
