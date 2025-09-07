using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Excel = Microsoft.Office.Interop.Excel;
using Office = Microsoft.Office.Core;
using Microsoft.Office.Tools.Excel;
using System.Windows.Forms;
using Microsoft.Office.Tools;

namespace ExcelAddIn1
{
    public partial class ThisAddIn
    {
        private TaskPaneControl taskPaneControl1;
        private CustomTaskPane taskPaneValue;

        private void ThisAddIn_Startup(object sender, System.EventArgs e)
        {
            taskPaneControl1 = new TaskPaneControl();
            taskPaneValue = this.CustomTaskPanes.Add(
                taskPaneControl1, "MyCustomTaskPane");
            taskPaneValue.VisibleChanged +=
                new EventHandler(taskPaneValue_VisibleChanged);
        }

        private void taskPaneValue_VisibleChanged(object sender, System.EventArgs e)
        {
            Globals.Ribbons.Ribbon1.toggleButton1.Checked =
                taskPaneValue.Visible;
        }

        public CustomTaskPane TaskPane
        {
            get
            {
                return taskPaneValue;
            }
        }

        private void ThisAddIn_Shutdown(object sender, System.EventArgs e)
        {
        }

        #region VSTO generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InternalStartup()
        {
            this.Startup += new System.EventHandler(ThisAddIn_Startup);
            this.Shutdown += new System.EventHandler(ThisAddIn_Shutdown);
        }
        
        #endregion
    }
}
