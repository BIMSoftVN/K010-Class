using AutoCAD;
using DevExpress.Utils.Gesture;
using DevExpress.XtraSpreadsheet.DocumentFormats.Xlsb;
using ExcelWorkbook2.Classes;
using ExcelWorkbook2.Models;
using ExcelWorkbook2.Views;
using Microsoft.Office.Interop.Excel;
using Microsoft.Office.Tools.Ribbon;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExcelWorkbook2
{
    public partial class Ribbon1
    {
        private void Ribbon1_Load(object sender, RibbonUIEventArgs e)
        {

        }

        private async void button1_Click(object sender, RibbonControlEventArgs e)
        {
            try
            {
                var xlApp = Globals.ThisWorkbook.Application;
                var ws = (Worksheet)Globals.ThisWorkbook.Sheets["Sheet1"];
                string ViDo = (string)ws.Range["I2"].Value;
                string KinhDo = (string)ws.Range["I3"].Value;

                var uList = await mWeather.GetWeatherByLatLon(ViDo, KinhDo);

                long id = -1;
                dynamic[,] arr = new dynamic[uList.Count, 6];
                foreach (var u in uList) 
                {
                    id++;
                    arr[id,0] = id+1;
                    arr[id,1] = u.wDate?.ToString("dd/MM");
                    arr[id, 2] = u.wDate?.ToString("HH:mm");
                    arr[id, 3] = u.wMota;
                    arr[id, 4] = u.wTemp;
                    arr[id, 5] = u.wWind;
                }

                ws.Range["A2:H1048576"].ClearContents();

                ws.Range["A2"].Resize[arr.GetLength(0), arr.GetLength(1)].Value = arr;
            }
            catch
            {

            }
        }

        private void button2_Click(object sender, RibbonControlEventArgs e)
        {
            try
            {
                
            }
            catch
            {

            }
        }

        private void button2_Click_1(object sender, RibbonControlEventArgs e)
        {
            var win = new vMain2();
            win.ShowDialog();
        }

        private void button3_Click(object sender, RibbonControlEventArgs e)
        {
            var xlApp = Globals.ThisWorkbook.Application;
            var wb = Globals.ThisWorkbook;
            Worksheet ws1 = (Worksheet)wb.Sheets["Sheet1"];
            Worksheet ws2 = (Worksheet)wb.Sheets["Sheet2"];

            Range range1 = ws1.UsedRange;
            Range range2 = ws2.UsedRange;

            foreach (Range cell in range1)
            {
                var address = cell.Address;

                var check = (cell.Value == ws2.Range[address].Value);
                if (!check)
                {
                    cell.Interior.Color = Color.Yellow.ToArgb();
                    ws2.Range[address].Interior.Color = Color.Yellow.ToArgb();

                    cell.AddComment("Ở Sheet 2 giá trị là : " + ws2.Range[address].Value);
                    ws2.Range[address].AddComment("Ở Sheet 1 giá trị là : " + cell.Value);
                }    
            }    
        }

        private void button4_Click(object sender, RibbonControlEventArgs e)
        {
            try
            {
                var acadApp = new AcadApplication();
                acadApp.Visible = true;

                var acaDoc = acadApp.ActiveDocument;

                var model = acaDoc.ModelSpace;
                var Sp = new double[3] { 0, 0, 0 };
                var Ep = new double[3] { 10, 10, 0 };
                var oLine = model.AddLine(Sp, Ep);
                oLine.color = ACAD_COLOR.acRed;
                oLine.Update();

                acadApp.ZoomExtents();
            }
            catch
            {

            }
        }

        private void button5_Click(object sender, RibbonControlEventArgs e)
        {
            try
            {
                var xlApp = Globals.ThisWorkbook.Application;
                var sht = (Worksheet)Globals.ThisWorkbook.ActiveSheet;

                var acadApp = Marshal.GetActiveObject("AutoCAD.Application") as AcadApplication;
                var acaDoc = acadApp.ActiveDocument;
                var model = acaDoc.ModelSpace;

                var goc = new double[3] { 0, 0, 0 };
                var startPoint = acaDoc.Utility.GetPoint(goc, "Hãy chọn 1 điểm");

                var lr = ((Range)(sht.Cells[sht.Rows.Count, 1])).End[XlDirection.xlUp].Row;


                for (var i = 3; i <= lr; i++)
                {
                    double b = (double)sht.Range[$"A{i}"].Value;
                    double h = (double)sht.Range[$"B{i}"].Value;
                    double cover = (double)sht.Range[$"C{i}"].Value;

                    int Slt = (int)sht.Range[$"D{i}"].Value;
                    int Dkt = (int)sht.Range[$"E{i}"].Value;
                    int Sld = (int)sht.Range[$"F{i}"].Value;
                    int Dkd = (int)sht.Range[$"G{i}"].Value;


                    VeTietDien(model, startPoint, b, h, cover, Slt, Dkt, Sld, Dkd);

                    startPoint[0] = startPoint[0] + b + 800;
                }


            }
            catch
            {

            }
        }
   
    
        private void VeTietDien(AcadModelSpace model, double[] StartPoint,double b, double h, double cover, int Slt, int Dkt, int Sld, int Dkd)
        {
            try
            {
                double[] VerticesList = new double[]
                {
                    StartPoint[0], StartPoint[1],
                    StartPoint[0] + b, StartPoint[1],
                    StartPoint[0] + b, StartPoint[1] +h,
                    StartPoint[0], StartPoint[1] +h,
                };
                var bolder = model.AddLightWeightPolyline(VerticesList);
                bolder.Closed = true;
                bolder.Layer = "concrete";

                var ThepDai = bolder.Offset(-cover)[0] as AcadLWPolyline;
                ThepDai.Layer = "thepdai";

                var kct = (b - 2 * cover - Dkt) / (Slt - 1);
                for (int i=0; i<Slt; i++)
                {
                    var x = StartPoint[0] + cover + Dkt / 2 + i * kct;
                    var y = StartPoint[1] + h - cover - Dkt / 2;

                    //var rebar = model.AddCircle(new double[] { x, y, 0 }, Dkt / 2);
                    //rebar.Layer = "thepchu";

                    var rebar = model.InsertBlock(new double[] { x, y, 0 }, "rebar", Dkt, Dkt, Dkt, 0);
                    rebar.Layer = "thepchu";

                    var pArr = new double[]
                    {
                        x,y,0,
                        StartPoint[0] + b/2, y+100, 0,
                        StartPoint[0] + b + 350, y + 100 , 0
                    };
                    model.AddLeader(pArr, null, AcLeaderType.acLineWithArrow);
                }

                var gct = model.InsertBlock(new double[] { StartPoint[0] + b + 350, StartPoint[1] + h - cover - Dkt / 2 + 100, 0 }, "ghichuthep", 1, 1, 1, 0);
                if (gct.HasAttributes)
                {
                    foreach (AcadAttributeReference attref in gct.GetAttributes())
                    {
                        if (attref.TagString == "1")
                        {
                            attref.TextString = "1";
                        }
                        else if (attref.TagString == "DONG1")
                        {
                            attref.TextString = $"{Slt}d{Dkt}";
                        }
                        else if (attref.TagString == "DONG2")
                        {
                            attref.TextString = "";
                        }
                    }
                }

                var kcd = (b - 2 * cover - Dkd) / (Sld - 1);
                for (int i = 0; i < Sld; i++)
                {
                    var x = StartPoint[0] + cover + Dkd / 2 + i * kcd;
                    var y = StartPoint[1] + cover + Dkt / 2;

                    //var rebar = model.AddCircle(new double[] { x, y, 0 }, Dkd / 2);
                    //rebar.Layer = "thepchu";
                    var rebar = model.InsertBlock(new double[] { x, y, 0 }, "rebar", Dkd, Dkd, Dkd, 0);
                    rebar.Layer = "thepchu";

                    var pArr = new double[]
                    {
                        x,y,0,
                        StartPoint[0] + b/2, y+100, 0,
                        StartPoint[0] + b + 350, y + 100 , 0
                    };
                    model.AddLeader(pArr, null, AcLeaderType.acLineWithArrow);
                    
                }

                var gcd = model.InsertBlock(new double[] { StartPoint[0] + b + 350, StartPoint[1] + cover + Dkt / 2 + 100, 0 }, "ghichuthep", 1, 1, 1, 0);
                if (gcd.HasAttributes)
                {
                    foreach (AcadAttributeReference attref in gcd.GetAttributes())
                    {
                        if (attref.TagString == "1")
                        {
                            attref.TextString = "2";
                        }
                        else if (attref.TagString == "DONG1")
                        {
                            attref.TextString = $"{Sld}d{Dkd}";
                        }
                        else if (attref.TagString == "DONG2")
                        {
                            attref.TextString = "";
                        }
                    }
                }

                double[] el1 = new double[3] { StartPoint[0], StartPoint[1] - 50, 0 };
                double[] el2 = new double[3] { StartPoint[0] + b, StartPoint[1] - 50, 0 };
                double[] tp = new double[3] { StartPoint[0] + b/2, StartPoint[1] - 200, 0 };

                var dim1 = model.AddDimAligned(el1, el2, tp);
                dim1.StyleName = "XMT 1-1-30";

                el1 = new double[3] { StartPoint[0] -50, StartPoint[1], 0 };
                el2 = new double[3] { StartPoint[0] -50, StartPoint[1]+h, 0 };
                tp = new double[3] { StartPoint[0] - 150, StartPoint[1]+h/2, 0 };

                var dim2 = model.AddDimAligned(el1, el2, tp);
                dim2.StyleName = "XMT 1-1-30";
            }
            catch
            {

            }
        }

        private void button6_Click(object sender, RibbonControlEventArgs e)
        {
            try
            {
                var acadApp = Marshal.GetActiveObject("AutoCAD.Application") as AcadApplication;
                var acaDoc = acadApp.ActiveDocument;
                var model = acaDoc.ModelSpace;

                AcadSelectionSet ss = acaDoc.ActiveSelectionSet;

                if (ss == null)
                {
                    ss = acaDoc.SelectionSets.Add("K010_SS");
                }

                ss.Clear();

                short[] filterType = new short[4];
                object[] filterData = new object[4];

                filterType[0] = (short)-4;
                filterData[0] = "<OR";

                filterType[1] = (short)0;
                filterData[1] = "TEXT";

                filterType[2] = (short)0;
                filterData[2] = "CIRCLE";

                filterType[3] = (short)-4;
                filterData[3] = "OR>";

                ss.SelectOnScreen(filterType, filterData);

                List<clCoc> cList = new List<clCoc>();
                List<clText> tList = new List<clText>();

                foreach (AcadEntity oEntity in ss)
                {
                    if (oEntity is AcadCircle)
                    {
                        var oCircle = (AcadCircle)oEntity;
                        var coc = new clCoc()
                        {
                            Diameter = oCircle.Diameter,
                            Orgin = oCircle.Center
                        };
                        cList.Add(coc);
                    }

                    if (oEntity is AcadText)
                    {
                        var oText = (AcadText)oEntity;
                        var text = new clText()
                        {
                            TextString = oText.TextString,
                            Orgin = oText.InsertionPoint
                        };
                        tList.Add(text);
                    }
                }  
                
                if (tList.Count>0)
                {
                    foreach (var text in tList)
                    {
                        var coc_NoName = cList.Where(o => string.IsNullOrEmpty(o.Name));
                        if (coc_NoName!=null && coc_NoName.Count() >0)
                        {
                            var coc_GanNhat = coc_NoName.OrderBy(c => Math.Sqrt(Math.Pow(c.Orgin[0] - text.Orgin[0], 2) + Math.Pow(c.Orgin[1] - text.Orgin[1], 2))).First();
                            coc_GanNhat.Name = text.TextString;
                        }    
                    }    
                }


                //var xlApp = Globals.ThisWorkbook.Application;
                //var sht = (Worksheet)Globals.ThisWorkbook.ActiveSheet;

                //sht.Range[$"A1"].Value = "STT";
                //sht.Range[$"B1"].Value = "Tên cọc";
                //sht.Range[$"C1"].Value = "Đường kính (mm)";
                //sht.Range[$"D1"].Value = "X (mm)";
                //sht.Range[$"E1"].Value = "Y (mm)";

                long stt = 0;


                cList = cList.OrderBy(o => o.Name).ToList();

                double[] startPoint = acaDoc.Utility.GetPoint(Type.Missing, "Hãy chọn 1 điểm");

                foreach (var coc in cList)
                {
                    stt++;
                    var tkc = model.InsertBlock(new double[] { startPoint[0], startPoint[1], 0 }, "TKCOC", 1, 1, 1, 0);

                    foreach (AcadAttributeReference attref in tkc.GetAttributes())
                    {
                        switch (attref.TagString)
                        {
                            case "STT":
                                {
                                    attref.TextString = stt.ToString();
                                }
                                break;

                            case "NAME":
                                {
                                    attref.TextString = coc.Name;
                                }
                                break;

                            case "DIAMETER":
                                {
                                    attref.TextString = Math.Round(coc.Diameter.Value, 0).ToString();
                                }
                                break;

                            case "X":
                                {
                                    attref.TextString = Math.Round(coc.Orgin[0], 2).ToString();
                                }
                                break;

                            case "Y":
                                {
                                    attref.TextString = Math.Round(coc.Orgin[1], 2).ToString();
                                }
                                break;
                        }    
                    }

                    startPoint[1] = startPoint[1] - 100;
                    //sht.Range[$"A{stt + 1}"].Value = stt;
                    //sht.Range[$"B{stt + 1}"].Value = coc.Name;
                    //sht.Range[$"C{stt + 1}"].Value = Math.Round(coc.Diameter.Value,0);
                    //sht.Range[$"D{stt + 1}"].Value = Math.Round(coc.Orgin[0],2); 
                    //sht.Range[$"E{stt + 1}"].Value = Math.Round(coc.Orgin[1], 2);
                }    
            }
            catch
            {

            }
            
        }
    }
}
