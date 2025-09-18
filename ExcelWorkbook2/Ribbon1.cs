using AutoCAD;
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

                    startPoint[0] = startPoint[0] + b + 300;
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

                
            }
            catch
            {

            }
        }
    }
}
