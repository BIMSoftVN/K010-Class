using ExcelWorkbook2.Models;
using ExcelWorkbook2.Views;
using Microsoft.Office.Interop.Excel;
using Microsoft.Office.Tools.Ribbon;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}
