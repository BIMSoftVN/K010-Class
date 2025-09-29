using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Internal;
using Autodesk.AutoCAD.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Application = Autodesk.AutoCAD.ApplicationServices.Application;

namespace AutoCAD_NET.TKC
{
    public class Commands
    {
        [CommandMethod("TKC")]
        public static void ThongKeCoc()
        {
            try
            {
                Document AcadDoc = Application.DocumentManager.MdiActiveDocument;
                Editor AcadEditor = AcadDoc.Editor;
                Database AcadDb = AcadDoc.Database;

                using (Transaction tr = AcadDb.TransactionManager.StartTransaction())
                {
                    TypedValue[] TypedValueList = new TypedValue[4];
                    TypedValueList.SetValue(new TypedValue((int)DxfCode.Operator, "<OR"), 0);
                    TypedValueList.SetValue(new TypedValue((int)DxfCode.Start, "TEXT"), 1);
                    TypedValueList.SetValue(new TypedValue((int)DxfCode.Start, "CIRCLE"), 2);
                    TypedValueList.SetValue(new TypedValue((int)DxfCode.Operator, "OR>"), 3);

                    var filter = new SelectionFilter(TypedValueList);
                    var result = AcadEditor.GetSelection(filter);

                    if (result.Status == PromptStatus.OK)
                    {
                        var sSet = result.Value;

                        List<clCoc> cList = new List<clCoc>();
                        List<clText> tList = new List<clText>();

                        foreach (SelectedObject c in sSet)
                        {
                            var oCir = tr.GetObject(c.ObjectId, OpenMode.ForRead) as Circle;
                            if (oCir != null)
                            {
                                cList.Add(new clCoc
                                {
                                    Id = oCir.ObjectId,
                                    Diameter = oCir.Diameter,
                                    Orgin = oCir.Center,
                                });
                            }
                            else
                            {
                                var oText = tr.GetObject(c.ObjectId, OpenMode.ForRead) as DBText;
                                if (oText != null)
                                {
                                    tList.Add(new clText
                                    {
                                        Id = oText.ObjectId,
                                        Orgin = oText.Position,
                                        TextString = oText.TextString
                                    });
                                }
                            }    
                        }

                        if (tList!=null && tList.Count>0)
                        {
                            foreach(var text in tList)
                            {
                                var coc_NoName_List = cList.Where(o => string.IsNullOrEmpty(o.Name));
                                if(coc_NoName_List!=null && coc_NoName_List.Count() > 0)
                                {
                                    var Nested_Coc = coc_NoName_List.OrderBy(o=> o.Orgin.DistanceTo(text.Orgin)).FirstOrDefault();
                                    if (Nested_Coc != null) 
                                    {
                                        Nested_Coc.Name = text.TextString;
                                    }
                                }    
                            }    
                        }  
                        
                        

                        if (cList!=null && cList.Count>0)
                        {
                            cList = cList.OrderBy(o => o.Name).ToList();


                            var blockTable = tr.GetObject(AcadDb.BlockTableId, OpenMode.ForRead) as BlockTable;
                            if(blockTable!=null)
                            {
                                var modelSpaceId = blockTable[BlockTableRecord.ModelSpace];
                                var modelSpaceRec = tr.GetObject(modelSpaceId, OpenMode.ForWrite) as BlockTableRecord;

                                ObjectId? TkcBlockId = null;

                                foreach (var blockId in blockTable)
                                {
                                    BlockTableRecord blockRec = tr.GetObject(blockId, OpenMode.ForRead) as BlockTableRecord;
                                    if (blockRec !=null && blockRec.Name == "TKCOC")
                                    {
                                        TkcBlockId = blockRec.ObjectId;
                                        break;
                                    }    

                                }    

                                if (TkcBlockId!=null && TkcBlockId.HasValue)
                                {
                                    var StartPointResult = AcadEditor.GetPoint("\nChọn điểm chèn thống kê");
                                    if (StartPointResult.Status == PromptStatus.OK)
                                    {
                                        var InsPoint = StartPointResult.Value;

                                        int stt = 0;
                                        foreach (var c in cList)
                                        {
                                            stt++;
                                            var blockRef = new BlockReference(InsPoint, TkcBlockId.Value);
                                            modelSpaceRec.AppendEntity(blockRef);
                                            tr.AddNewlyCreatedDBObject(blockRef, true);

                                            InsPoint = new Autodesk.AutoCAD.Geometry.Point3d(InsPoint.X, InsPoint.Y - 100, 0);

                                            var bt = AcadDb.BlockTableId.GetObject(OpenMode.ForRead) as BlockTable;
                                            var btr = bt["TKCOC"].GetObject(OpenMode.ForRead) as BlockTableRecord;

                                            
                                            foreach (var id in btr)
                                            {
                                                var ent = id.GetObject(OpenMode.ForRead);
                                                if (ent != null)
                                                {
                                                    var attDef = ent as AttributeDefinition;
                                                    if (attDef != null)
                                                    {
                                                        switch (attDef.Tag)
                                                        {
                                                            case "STT":
                                                                {
                                                                    using (var attRef = new AttributeReference())
                                                                    {
                                                                        attRef.SetAttributeFromBlock(attDef, blockRef.BlockTransform);
                                                                        attRef.TextString = stt.ToString();

                                                                        blockRef.AttributeCollection.AppendAttribute(attRef);
                                                                        tr.AddNewlyCreatedDBObject(attRef, true);
                                                                    }
                                                                }
                                                                break;

                                                            case "NAME":
                                                                {
                                                                    using (var attRef = new AttributeReference())
                                                                    {
                                                                        attRef.SetAttributeFromBlock(attDef, blockRef.BlockTransform);
                                                                        if (string.IsNullOrEmpty(c.Name))
                                                                        {
                                                                            c.Name = "";
                                                                        }
                                                                        attRef.TextString = c.Name;

                                                                        blockRef.AttributeCollection.AppendAttribute(attRef);
                                                                        tr.AddNewlyCreatedDBObject(attRef, true);
                                                                    }
                                                                }
                                                                break;

                                                            case "DIAMETER":
                                                                {
                                                                    if (c.Diameter.HasValue)
                                                                    {
                                                                        using (var attRef = new AttributeReference())
                                                                        {
                                                                            attRef.SetAttributeFromBlock(attDef, blockRef.BlockTransform);

                                                                            attRef.TextString = Math.Round(c.Diameter.Value,0).ToString();

                                                                            blockRef.AttributeCollection.AppendAttribute(attRef);
                                                                            tr.AddNewlyCreatedDBObject(attRef, true);
                                                                        }
                                                                    }
                                                                    
                                                                }
                                                                break;

                                                            case "X":
                                                                {
                                                                    if (c.Orgin != null)
                                                                    {
                                                                        using (var attRef = new AttributeReference())
                                                                        {
                                                                            attRef.SetAttributeFromBlock(attDef, blockRef.BlockTransform);

                                                                            attRef.TextString = Math.Round(c.Orgin.X,2).ToString();

                                                                            blockRef.AttributeCollection.AppendAttribute(attRef);
                                                                            tr.AddNewlyCreatedDBObject(attRef, true);
                                                                        }
                                                                    }

                                                                }
                                                                break;

                                                            case "Y":
                                                                {
                                                                    if (c.Orgin != null)
                                                                    {
                                                                        using (var attRef = new AttributeReference())
                                                                        {
                                                                            attRef.SetAttributeFromBlock(attDef, blockRef.BlockTransform);

                                                                            attRef.TextString = Math.Round(c.Orgin.Y,2).ToString();

                                                                            blockRef.AttributeCollection.AppendAttribute(attRef);
                                                                            tr.AddNewlyCreatedDBObject(attRef, true);
                                                                        }
                                                                    }

                                                                }
                                                                break;
                                                        }    
          
                                                    }
                                                }
                                            }

                                        }
                                    }
                                    tr.Commit();
                                    AcadEditor.Regen();

                                    MessageBox.Show($"Đã chọn {sSet.Count} đối tượng.");
                                }    
                            }    

                           

                            
                                

                            
                        }    
                    }
                }      
            }
            catch (Autodesk.AutoCAD.Runtime.Exception ex)
            {

            }
        }
    }
}
