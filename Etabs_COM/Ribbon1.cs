using ETABSv1;
using Microsoft.Office.Interop.Excel;
using Microsoft.Office.Tools.Ribbon;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Windows.Forms;

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
    }
}
