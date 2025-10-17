using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace K010_RevitAddins.ImportCAD
{
    [Transaction(TransactionMode.Manual)]
    public class OpenWindow : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
           try
            {
                var uiDoc = commandData.Application.ActiveUIDocument;
                var doc = uiDoc.Document;

                var ssFilter = new linkCADFilter();

                var LinkCAD = uiDoc.Selection.PickObject(ObjectType.Element, ssFilter, "Hãy chọn file Link");

                var win = new vMain();
                ((vmMain)win.DataContext).LinkCAD = doc.GetElement(LinkCAD.ElementId) as ImportInstance;


                win.ShowDialog();


            }
            catch
            {

            }




            
            return Result.Succeeded;
        }

        public class linkCADFilter : ISelectionFilter
        {
            public bool AllowElement(Element elem)
            {
                if (elem is ImportInstance cadLink && cadLink.IsLinked == true)
                {
                    return true;
                }

                return false;
            }
            public bool AllowReference(Reference reference, XYZ position)
            {
                return false;
            }
        }
    }
}
