using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace K010_RevitAddins
{

    [Transaction(TransactionMode.Manual)]
    public class AssignBIMId : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            try
            {
                UIApplication uiApp = commandData.Application;
                UIDocument uiDoc = uiApp.ActiveUIDocument;
                Document doc = uiDoc.Document;


                FilteredElementCollector viewCollector = new FilteredElementCollector(uiDoc.Document, doc.ActiveView.Id).WhereElementIsNotElementType();

                var ElementList = viewCollector.ToList();

                if(ElementList ==null || ElementList.Count ==0)
                {
                    TaskDialog.Show("Error", "Không có phần tử");
                }

                int sl = 0;

                using (Transaction tr = new Transaction(doc, "Gán ID phần tử"))
                {
                    tr.Start();

                    foreach (var element in ElementList)
                    {
                        Parameter BIMId = element.LookupParameter("K010_BIMId");
                        if (BIMId != null && BIMId.IsReadOnly == false)
                        {
                            BIMId.Set(element.Id.Value.ToString());
                            sl++;
                        }
                    }

                    tr.Commit();
                }    
                    


                TaskDialog.Show("Thành công", $"Đã thực gán Id cho {sl} phần tử");

                return Result.Succeeded;
            }
            catch (Exception ex)
            {
                TaskDialog.Show("Error", ex.Message);
                return Result.Failed;
            }

            
        }
    }
}
