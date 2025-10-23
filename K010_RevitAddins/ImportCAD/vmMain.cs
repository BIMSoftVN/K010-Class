using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using DevExpress.Mvvm;
using DevExpress.Xpf.Spreadsheet;
using K010_RevitAddins.Classes;
using K010Libs.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace K010_RevitAddins.ImportCAD
{
    public class vmMain : PropertyChangedBase
    {
        private ImportInstance _LinkCAD = null;
        public ImportInstance LinkCAD
        {
            get
            {
                return _LinkCAD;
            }
            set
            {
                _LinkCAD = value;
                OnPropertyChanged();
            }
        }


        private ObservableRangeCollection<clCadCircleInfo> _CadList = new ObservableRangeCollection<clCadCircleInfo>();
        public ObservableRangeCollection<clCadCircleInfo> CadList
        {
            get
            {
                return _CadList;
            }
            set
            {
                _CadList = value;
                OnPropertyChanged();
            }
        }


        private ObservableRangeCollection<clFamilyCoc> _FamilyList = new ObservableRangeCollection<clFamilyCoc>();
        public ObservableRangeCollection<clFamilyCoc> FamilyList
        {
            get
            {
                return _FamilyList;
            }
            set
            {
                _FamilyList = value;
                OnPropertyChanged();
            }
        }

        private clFamilyCoc _FamilySelect = new clFamilyCoc();
        public clFamilyCoc FamilySelect
        {
            get
            {
                return _FamilySelect;
            }
            set
            {
                _FamilySelect = value;
                OnPropertyChanged();
            }
        }



        private DelegateCommand loadAllCmd;
        public ICommand LoadAllCmd
        {
            get
            {
                if (loadAllCmd == null)
                {
                    loadAllCmd = new DelegateCommand(PerformLoadAllCmd);
                }

                return loadAllCmd;
            }
        }

        private void PerformLoadAllCmd()
        {
            try
            {
                CadList.Clear();
                FamilyList.Clear();

                var cl = GetCadCircleFromLinkRevit(LinkCAD);
                if (cl != null)
                {
                    CadList.AddRange(cl.Result);
                }

                var cl2 = GetFamilyCoc(LinkCAD);
                if (cl2 != null)
                {
                    FamilyList.AddRange(cl2.Result);
                }


            }
            catch
            {

            }
        }


        private static async Task<List<clCadCircleInfo>> GetCadCircleFromLinkRevit(ImportInstance LinkCad)
        {
            List<clCadCircleInfo> ret = new List<clCadCircleInfo>();
                    
            try
            {
                var doc = LinkCad.Document;

                var DisplayLengthUnitTypeId = doc.GetUnits().GetFormatOptions(SpecTypeId.Length).GetUnitTypeId();



                var Opts = doc.Application.Create.NewGeometryOptions();
                var Geo1 = LinkCad.get_Geometry(Opts);
                if (Geo1!=null)
                {
                    foreach (var geoObj  in Geo1)
                    {
                        var GeoIntance1 = geoObj as GeometryInstance;

                        if (GeoIntance1 != null)
                        {
                            var Geo2 = GeoIntance1.GetInstanceGeometry();
                            if(Geo2 != null)
                            {
                                foreach (var geoObj2 in Geo2)
                                {
                                    if (geoObj2 is Arc arc)
                                    {
                                        if (arc.IsClosed)
                                        {
                                            var cir = new clCadCircleInfo();

                                            cir.X = UnitUtils.ConvertFromInternalUnits(arc.Center.X, DisplayLengthUnitTypeId);
                                            cir.Y = UnitUtils.ConvertFromInternalUnits(arc.Center.Y, DisplayLengthUnitTypeId);
                                            cir.Diameter = Math.Round(UnitUtils.ConvertFromInternalUnits(arc.Radius*2, DisplayLengthUnitTypeId),0); 

                                            cir.Layer = (doc.GetElement(geoObj2.GraphicsStyleId) as GraphicsStyle).GraphicsStyleCategory.Name;

                                            ret.Add(cir);
                                        }    
                                    }
                                }
                            }

                            
                        }
                    }
                }    

            }
            catch
            {

            }

            return ret;
        }

        private static async Task<List<clFamilyCoc>> GetFamilyCoc(ImportInstance LinkCad)
        {
            List<clFamilyCoc> ret = new List<clFamilyCoc>();

            try
            {
                var doc = LinkCad.Document;
                var DisplayLengthUnitTypeId = doc.GetUnits().GetFormatOptions(SpecTypeId.Length).GetUnitTypeId();

                var Collector = new FilteredElementCollector(doc)
                    .OfCategory(BuiltInCategory.OST_StructuralFoundation)
                    .OfClass(typeof(FamilySymbol));

                var cocTypes = Collector.ToElements();

                foreach (FamilySymbol fs in cocTypes)
                {
                    var family = new clFamilyCoc(); 

                    family.FamilyName = fs.FamilyName;
                    family.TypeName = fs.Name;
                    family.Id = fs.Id;
                    var par = fs.LookupParameter("Width");
                    if (par != null) 
                    { 
                        var dia = par.AsDouble();
                        family.Diameter = Math.Round(UnitUtils.ConvertFromInternalUnits(dia, DisplayLengthUnitTypeId),0);

                        ret.Add(family);    
                    }
                }

            }
            catch
            {

            }

            return ret;
        }

        private DelegateCommand familyChangeCmd;
        public ICommand FamilyChangeCmd
        {
            get
            {
                if (familyChangeCmd == null)
                {
                    familyChangeCmd = new DelegateCommand(PerformFamilyChangeCmd);
                }

                return familyChangeCmd;
            }
        }

        private void PerformFamilyChangeCmd()
        {
            try
            {
                var familyList = FamilyList.Where(x => x.FamilyName == FamilySelect.FamilyName).ToList();

                foreach (var cad in CadList)
                {
                    var family = familyList.Where(x => x.Diameter == cad.Diameter).FirstOrDefault();    
                    if (family!=null)
                    {
                        cad.TypeName = family.TypeName;
                        cad.TypeId = family.Id;
                    }
                    else
                    {
                        cad.TypeName = null;
                        cad.TypeId = null;
                    }
                }
            }
            catch
            {

            }
        }

        private DelegateCommand veCocCmd;
        public ICommand VeCocCmd
        {
            get
            {
                if (veCocCmd == null)
                {
                    veCocCmd = new DelegateCommand(PerformVeCocCmd);
                }

                return veCocCmd;
            }
        }

        private void PerformVeCocCmd()
        {
            try
            {
                var doc = LinkCAD.Document;
                var DisplayLengthUnitTypeId = doc.GetUnits().GetFormatOptions(SpecTypeId.Length).GetUnitTypeId();

                var LinkTrans = LinkCAD.GetTotalTransform();


                using (Transaction tx = new Transaction(doc, "Import CAD Circles to COC"))
                {
                    tx.Start();


                    foreach (var coc in CadList)
                    {
                        if (coc.TypeId!=null)
                        {
                            var familySymbol = doc.GetElement(coc.TypeId) as FamilySymbol;
                            if (!familySymbol.IsActive)
                            {
                                familySymbol.Activate();
                                doc.Regenerate();
                            }

                            XYZ CadLocation = new XYZ(
                                UnitUtils.ConvertToInternalUnits(coc.X.Value, DisplayLengthUnitTypeId),
                                UnitUtils.ConvertToInternalUnits(coc.Y.Value, DisplayLengthUnitTypeId),
                                0);

                            var RevitLoc = LinkTrans.OfPoint(CadLocation);

                            var famiCoc = doc.Create.NewFamilyInstance(RevitLoc, familySymbol, Autodesk.Revit.DB.Structure.StructuralType.Footing);
                        }    
                        
                    }    


                    tx.Commit();

                    TaskDialog.Show("Thành công", "Đã vẽ xong");
                }
            }
            catch(Exception ex)
            {
                TaskDialog.Show("Lỗi", ex.Message);
            }
        }
    }
}
