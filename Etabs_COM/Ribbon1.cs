using Etabs_COM.Classes;
using ETABSv1;
using Microsoft.Office.Interop.Excel;
using Microsoft.Office.Tools.Ribbon;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Etabs_COM
{
    public partial class Ribbon1
    {
        private void Ribbon1_Load(object sender, RibbonUIEventArgs e)
        {

        }

        private void button1_Click(object sender, RibbonControlEventArgs e)
        {
            try
            {
                cHelper myHelper = new Helper();
                cOAPI myETABSObject = null;
                int ret = 0;

                myETABSObject = myHelper.CreateObjectProgID("CSI.ETABS.API.ETABSObject");

                ret = myETABSObject.ApplicationStart();

                string ModelPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Etabs", "ModelEtabs.edb");

                cSapModel SapModel = myETABSObject.SapModel;

                ret = SapModel.InitializeNewModel();

                ret = SapModel.File.NewSteelDeck(4,12,12,4,4,24,24);

                ret = SapModel.File.Save(ModelPath);

                ret = SapModel.Analyze.RunAnalysis();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button2_Click(object sender, RibbonControlEventArgs e)
        {
            try
            {
                cHelper myHelper = new Helper();
                cOAPI myETABSObject = null;
                int ret = 0;

                myETABSObject = myHelper.GetObject("CSI.ETABS.API.ETABSObject");

                cSapModel SapModel = myETABSObject.SapModel;

                int NumberStories = -1;
                string[] StoryNames = new string[] { };
                double[] StoryHeights = new double[] { };
                double[] StoryElevations = new double[] { };
                bool[] IsMasterStory = new bool[] { };
                string[] SimilarToStory = new string[] { };
                bool[] SpliceAbove = new bool[] { };
                double[] SpliceHeight = new double[] { };


                ret = SapModel.Story.GetStories(ref NumberStories, ref StoryNames, ref StoryHeights, ref StoryElevations,
                    ref IsMasterStory, ref SimilarToStory, ref SpliceAbove, ref SpliceHeight);

                if (ret == 0 && NumberStories > 0)
                {
                    var xlApp = Globals.ThisWorkbook.Application;
                    Range aCell = xlApp.ActiveCell;
                    Worksheet ws = aCell.Worksheet;

                    long id = -1;

                    dynamic[,] arr = new dynamic[NumberStories, 3];

                    for (int i = 0; i < NumberStories; i++)
                    {
                        id++;
                        arr[id,0] = StoryNames[i];
                        arr[id, 1] = StoryHeights[i];
                        arr[id, 2] = StoryElevations[i];
                    }

                    aCell.Resize[arr.GetLength(0), arr.GetLength(1)].Value = arr;
                }
                else 
                { 
                    MessageBox.Show("Không lấy được danh sách tầng");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button3_Click(object sender, RibbonControlEventArgs e)
        {
            try
            {
                cHelper myHelper = new Helper();
                cOAPI myETABSObject = null;
                int ret = 0;

                myETABSObject = myHelper.GetObject("CSI.ETABS.API.ETABSObject");

                cSapModel SapModel = myETABSObject.SapModel;

                int NumberNames = -1;
                string[] MyName = new string[] { };

                ret = SapModel.RespCombo.GetNameList(ref NumberNames, ref MyName);

                if (ret == 0 && NumberNames > 0)
                {
                    var xlApp = Globals.ThisWorkbook.Application;
                    Range aCell = xlApp.ActiveCell;
                    Worksheet ws = aCell.Worksheet;

                    long id = -1;

                    dynamic[,] arr = new dynamic[NumberNames, 3];

                    for (int i = 0; i < NumberNames; i++)
                    {
                        id++;
                        arr[id, 0] = MyName[i];
                    }

                    aCell.Resize[arr.GetLength(0), arr.GetLength(1)].Value = arr;
                }
                else
                {
                    MessageBox.Show("Không lấy được danh sáchtổ hợp");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button4_Click(object sender, RibbonControlEventArgs e)
        {
            try
            {
                List<string> selectedCombo = new List<string>();

                var xlApp = Globals.ThisWorkbook.Application;
                Range cSelected = xlApp.Selection;

                foreach (Range cell in cSelected.Cells)
                {
                    if (!string.IsNullOrEmpty(cell.Value))
                    {
                        selectedCombo.Add(cell.Value);
                    }    
                }

                if (selectedCombo != null && selectedCombo.Count > 0)
                {
                    cHelper myHelper = new Helper();
                    cOAPI myETABSObject = null;
                    int ret = 0;

                    myETABSObject = myHelper.GetObject("CSI.ETABS.API.ETABSObject");

                    cSapModel SapModel = myETABSObject.SapModel;

                    ret = SapModel.SetPresentUnits(eUnits.kN_m_C);

                    ret = SapModel.Results.Setup.DeselectAllCasesAndCombosForOutput();

                    foreach (string comboName in selectedCombo)
                    {
                        ret = SapModel.Results.Setup.SetComboSelectedForOutput(comboName);
                    }

                    //int NumberNames = -1;
                    //string[] MyName = new string[] { };

                    //ret = SapModel.FrameObj.GetNameList(ref NumberNames, ref MyName);

                    int numberItems = -1;
                    int[] objectType = new int[] { };
                    string[] objectName = new string[] { };

                    ret = SapModel.SelectObj.GetSelected(ref numberItems, ref objectType,ref objectName);


                    List<string> fName = new List<string>();

                    List<clFrameForce> fList = new List<clFrameForce>();

                    if (numberItems > 0)
                    {
                        for (var i = 0; i< numberItems; i++)
                        {
                            if (objectType[i] == 2)
                            {
                                fName.Add(objectName[i]);
                            }    
                        }    



                        int NumberResults = -1;
                        string[] Obj = new string[] { };
                        double[] ObjSta = new double[] { };
                        string[] Elm = new string[] { };
                        double[] ElmSta = new double[] { };
                        string[] LoadCase = new string[] { };
                        string[] StepType = new string[] { };
                        double[] StepNum = new double[] { };
                        double[] P = new double[] { };
                        double[] V2 = new double[] { };
                        double[] V3 = new double[] { };
                        double[] T = new double[] { };
                        double[] M2 = new double[] { };
                        double[] M3 = new double[] { };

                        foreach (var name in fName)
                        {
                            var Huong = eFrameDesignOrientation.Null;
                            ret = SapModel.FrameObj.GetDesignOrientation(name, ref Huong);
                            if (ret == 0 && Huong == eFrameDesignOrientation.Column)
                            {
                                ret = SapModel.Results.FrameForce(name, eItemTypeElm.ObjectElm, ref NumberResults, ref Obj,
                                    ref ObjSta, ref Elm, ref ElmSta, ref LoadCase, ref StepType, ref StepNum, ref P, ref V2, ref V3, ref T, ref M2, ref M3);


                                string Label = null;
                                string Story = null;

                                ret = SapModel.FrameObj.GetLabelFromName(name, ref Label, ref Story);

                                for (int i = 0; i < NumberResults; i++)
                                {
                                    fList.Add(new clFrameForce
                                    {
                                        FrameName = name,
                                        Label = Label,
                                        Story = Story,
                                        LoadCase = LoadCase[i],
                                        P = P[i],
                                        M2 = M2[i],
                                        M3 = M3[i]
                                    });
                                }

                            }
                        }
                    }


                    if (fList.Count > 0)
                    {
                        long id = -1;
                        dynamic[,] arr = new dynamic[fList.Count, 7];
                        foreach (var item in fList)
                        {
                            id++;
                            arr[id, 0] = item.FrameName;
                            arr[id, 1] = item.Label;
                            arr[id, 2] = item.Story;
                            arr[id, 3] = item.LoadCase;
                            arr[id, 4] = item.P;
                            arr[id, 5] = item.M2;
                            arr[id, 6] = item.M3;
                        }

                        Range aCell = xlApp.ActiveCell;
                        aCell.Resize[arr.GetLength(0), arr.GetLength(1)].Value = arr;
                    }
                    else
                    {
                        MessageBox.Show("Không có kết quả nào");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void button5_Click(object sender, RibbonControlEventArgs e)
        {
            try
            {
                var xlApp = Globals.ThisWorkbook.Application;
                var ws = xlApp.ActiveSheet as Worksheet;

                int SoNhip = (int)ws.Range["B1"].Value;
                double BuocNhip = ws.Range["B2"].Value;
                double Rong = ws.Range["B3"].Value;
                double Cao = ws.Range["B4"].Value;
                double Mai = ws.Range["B5"].Value;

                var kq =await ReadInfo(SoNhip, BuocNhip, Rong, Cao, Mai);


                cHelper myHelper = new Helper();
                cOAPI myETABSObject = null;
                int ret = 0;

                myETABSObject = myHelper.GetObject("CSI.ETABS.API.ETABSObject");

                cSapModel SapModel = myETABSObject.SapModel;

                ret = SapModel.SetPresentUnits(eUnits.kN_m_C);



                if (kq.lineList != null && kq.lineList.Count>0)
                {
                    
                    foreach (var line in kq.lineList)
                    {
                        string lineName = null;
                        
                        ret = SapModel.FrameObj.AddByCoord(line.StartPoint.X.Value, line.StartPoint.Y.Value, line.StartPoint.Z.Value,
                            line.EndPoint.X.Value, line.EndPoint.Y.Value, line.EndPoint.Z.Value, ref lineName);

                        line.Name = lineName;
                    }
                }

                if (kq.areaList != null && kq.areaList.Count > 0)
                {

                    foreach (var area in kq.areaList)
                    {
                        string Name = null;
                        double[] X = area.X;
                        double[] Y = area.Y;
                        double[] Z = area.Z;
                        ret = SapModel.AreaObj.AddByCoord(area.X.GetLength(0),ref X, ref Y, ref Z, ref Name);

                        area.Name = Name;
                    }
                }


                SapModel.View.RefreshView();
            }
            catch
            {

            }
        }

        private static async Task<(List<clLine> lineList, List<clArea> areaList)> ReadInfo(int SoNhip, double BuocNhip, double Rong, double Cao, double Mai)
        {
            var lineList = new List<clLine>();
            var areaList = new List<clArea>();

            try
            {

                var p1_t = new clPoint ();
                var p2_t = new clPoint ();
                var p3_t = new clPoint ();
                var p4_t = new clPoint ();
                var p5_t = new clPoint ();

                for (var i=0; i< SoNhip; i++)
                {
                    var p1 = new clPoint { X = i* BuocNhip, Y = 0, Z = 0 };
                    var p2 = new clPoint { X = i * BuocNhip, Y = 0, Z = Cao };
                    var p3 = new clPoint { X = i * BuocNhip, Y = Rong / 2, Z = Cao + Mai };
                    var p4 = new clPoint { X = i * BuocNhip, Y = Rong, Z = Cao };
                    var p5 = new clPoint { X = i * BuocNhip, Y = Rong, Z = 0 };

                    lineList.Add(new clLine { StartPoint = p1, EndPoint = p2 });
                    lineList.Add(new clLine { StartPoint = p2, EndPoint = p3 });
                    lineList.Add(new clLine { StartPoint = p3, EndPoint = p4 });
                    lineList.Add(new clLine { StartPoint = p4, EndPoint = p5 });

                    if (i>0)
                    {
                        var area = new clArea();
                        area.X = new double[] { p2.X.Value,p3.X.Value, p3_t.X.Value, p2_t.X.Value };
                        area.Y = new double[] { p2.Y.Value, p3.Y.Value, p3_t.Y.Value, p2_t.Y.Value };
                        area.Z = new double[] { p2.Z.Value, p3.Z.Value, p3_t.Z.Value, p2_t.Z.Value };

                        areaList.Add(area);

                        var area2 = new clArea();
                        area2.X = new double[] { p4.X.Value, p3.X.Value, p3_t.X.Value, p4_t.X.Value };
                        area2.Y = new double[] { p4.Y.Value, p3.Y.Value, p3_t.Y.Value, p4_t.Y.Value };
                        area2.Z = new double[] { p4.Z.Value, p3.Z.Value, p3_t.Z.Value, p4_t.Z.Value };

                        areaList.Add(area2);
                    }

                    p1_t = p1;
                    p2_t = p2;
                    p3_t = p3;
                    p4_t = p4;
                    p5_t = p5;
                }  
            }
            catch
            {

            }

            return (lineList,areaList);
        }
    }
}
