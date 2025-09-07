using ClosedXML.Excel;
using DevExpress.Xpf.Editors.Helpers;
using DevExpress.Xpf.Grid;
using iCons.Classes;
using iCons.Models;
using K010Libs.Mvvm;
using Microsoft.Xaml.Behaviors.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using Microsoft.Office.Interop.Excel;
using Excel = Microsoft.Office.Interop.Excel;
using OpenFileDialog = System.Windows.Forms.OpenFileDialog;
using System.Runtime.InteropServices;

namespace iCons.ViewModels.Main
{
    public class vmAdmin : PropertyChangedBase
    {
        private ObservableRangeCollection<efUser> _UserList = new ObservableRangeCollection<efUser>();
        public ObservableRangeCollection<efUser> UserList
        {
            get
            {
                return _UserList;
            }
            set
            {
                _UserList = value;
                OnPropertyChanged();
            }
        }

        private ObservableRangeCollection<efUser> _UserSelect = new ObservableRangeCollection<efUser>();
        public ObservableRangeCollection<efUser> UserSelect
        {
            get
            {
                return _UserSelect;
            }
            set
            {
                _UserSelect = value;
                OnPropertyChanged();
            }
        }






        private ActionCommand loadAll;

        public ICommand LoadAll
        {
            get
            {
                if (loadAll == null)
                {
                    loadAll = new ActionCommand(PerformLoadAll);
                }

                return loadAll;
            }
        }

        private async void PerformLoadAll()
        {
            var vmMainDc = App.Current.MainWindow.DataContext as vmMain;

            string Message = string.Empty;

            try
            {
                UserList.Clear();

                vmMainDc.IsWinActive = false;
                vmMainDc.PbIsIndeterminate = true;
                vmMainDc.PbMessage = "Đang tải thông tin...";

                var kq = await Task.Run(async () =>
                {
                    return await mUser.GetAllUsers();
                });

                Message = kq.Message;

                if (kq.IsSuccess)
                {
                    UserList.AddRange(kq.UserList);
                }
            }
            catch (Exception ex)
            {
                Message = ex.Message;
            }
            finally
            {
                vmMainDc.IsWinActive = true;
                vmMainDc.SbMessage?.Enqueue(Message, null, null, null, false, true, TimeSpan.FromSeconds(2));
            }
        }

        
        
        private ActionCommand deleteUser_Command;

        public ICommand DeleteUser_Command
        {
            get
            {
                if (deleteUser_Command == null)
                {
                    deleteUser_Command = new ActionCommand(DeleteUser_);
                }

                return deleteUser_Command;
            }
        }

        private async void DeleteUser_()
        {
            var vmMainDc = App.Current.MainWindow.DataContext as vmMain;

            string Message = string.Empty;

            try
            {
                if (UserSelect!=null && UserSelect.Count>0)
                {
                    var kqAskDel = MessageBox.Show($"Bạn có chắc chắn muốn xóa {UserSelect.Count} người dùng đã chọn không?", 
                            "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (kqAskDel == DialogResult.Yes)
                    {
                        vmMainDc.IsWinActive = false;
                        vmMainDc.PbIsIndeterminate = true;
                        vmMainDc.PbMessage = "Đang tải thông tin...";

                        var kq = await Task.Run(async () =>
                        {
                            return await mUser.DeleteUserList(UserSelect);
                        });

                        Message = kq.Message;

                        if (kq.IsSuccess)
                        {
                            PerformLoadAll();
                        }
                    } 
                    else
                    {
                        return;
                    }    
                }    
            }
            catch (Exception ex)
            {
                Message = ex.Message;
            }

            vmMainDc.IsWinActive = true;
            vmMainDc.SbMessage?.Enqueue(Message, null, null, null, false, true, TimeSpan.FromSeconds(2));
        }

        private ActionCommand userRowUpdate_Command;

        public ICommand UserRowUpdate_Command
        {
            get
            {
                if (userRowUpdate_Command == null)
                {
                    userRowUpdate_Command = new ActionCommand(UserRowUpdate_);
                }

                return userRowUpdate_Command;
            }
        }

        private async void UserRowUpdate_(object parameter)
        {
            var vmMainDc = App.Current.MainWindow.DataContext as vmMain;
            string Message = string.Empty;

            try
            {
                var row = parameter as RowEventArgs;
                var editUser = (efUser)(row.Row);

                if (editUser !=null)
                {
                    var kq = await Task.Run(async () =>
                    {
                        return await mUser.EditUserInfo(editUser);
                    });

                    Message = kq.Message;

                    if (!kq.IsSuccess)
                    {
                        PerformLoadAll();
                    }
                }    

            }
            catch (Exception ex)
            {
                Message = ex.Message;
            }

            vmMainDc.IsWinActive = true;
            vmMainDc.SbMessage?.Enqueue(Message, null, null, null, false, true, TimeSpan.FromSeconds(2));
        }


        private ActionCommand addUser_Command;

        public ICommand AddUser_Command
        {
            get
            {
                if (addUser_Command == null)
                {
                    addUser_Command = new ActionCommand(AddUser_);
                }

                return addUser_Command;
            }
        }

        private void AddUser_()
        {
            try
            {
                var newUser = new efUser
                {
                    FullName = "Người dùng mới",
                };
                UserList.Insert(0, newUser);
            }
            catch
            {

            }
        }

        private ActionCommand exportExcel_Command;

        public ICommand ExportExcel_Command
        {
            get
            {
                if (exportExcel_Command == null)
                {
                    exportExcel_Command = new ActionCommand(ExportExcel_);
                }

                return exportExcel_Command;
            }
        }

        private void ExportExcel_()
        {
            var vmMainDc = App.Current.MainWindow.DataContext as vmMain;
            string Message = string.Empty;

            try
            {
                if (UserSelect !=null && UserSelect.Count>0)
                {
                    var saveFileDialog = new SaveFileDialog
                    {
                        Filter = "Excel Workbook|*.xlsx",
                        Title = "Lưu danh sách nhân viên",
                        FileName = "DSNhanVien_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".xlsx"
                    };

                    if (saveFileDialog.ShowDialog() != DialogResult.OK)
                    {
                        return;
                    }

                    using (var workbook = new XLWorkbook())
                    {
                        var worksheet = workbook.Worksheets.Add("Danh sách User");

                        worksheet.Cell("A1").Value = "UserName";
                        worksheet.Cell("B1").Value = "Email";
                        worksheet.Cell("C1").Value = "Tên đầy đủ";
                        worksheet.Cell("D1").Value = "Ngày sinh";
                        worksheet.Cell("E1").Value = "Quyền";

                        var lr = 1;
                        foreach (var user in UserSelect)
                        {
                            lr++;
                            worksheet.Cell($"A{lr}").Value = user.UserName;
                            worksheet.Cell($"B{lr}").Value = user.Email;
                            worksheet.Cell($"C{lr}").Value = user.FullName;
                            worksheet.Cell($"D{lr}").Value = user.DateOfBirth?.ToString("yyyy-MM-dd");
                            worksheet.Cell($"E{lr}").Value = user.Roles;
                        }

                        workbook.SaveAs(saveFileDialog.FileName);
                    }    
                }
                else
                {
                    Message = "Vui lòng chọn người dùng";
                }   
                
            }
            catch (Exception ex)
            {
                Message = ex.Message;
            }

            vmMainDc.IsWinActive = true;
            vmMainDc.SbMessage?.Enqueue(Message, null, null, null, false, true, TimeSpan.FromSeconds(2));
        }

        private ActionCommand importExcel_Command;

        public ICommand ImportExcel_Command
        {
            get
            {
                if (importExcel_Command == null)
                {
                    importExcel_Command = new ActionCommand(ImportExcel_);
                }

                return importExcel_Command;
            }
        }

        private async void ImportExcel_()
        {
            var vmMainDc = App.Current.MainWindow.DataContext as vmMain;
            string Message = string.Empty;

            try
            {
                var openFileDialog = new OpenFileDialog
                {
                    Filter = "Excel Workbook|*.xlsx",
                    Title = "Mở danh sách nhân viên",
                    Multiselect = false,
                };

                if (openFileDialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                using (var workbook = new XLWorkbook(openFileDialog.FileName))
                {
                    var sht = workbook.Worksheets.FirstOrDefault();
                    var lr = sht.LastRowUsed().RowNumber();

                    var uList = new List<efUser>();
                    for(long id=2;id<=lr; id++)
                    {
                        var user = new efUser
                        {
                            UserName = sht.Cell($"A{id}").Value.ToString(),
                            Email = sht.Cell($"B{id}").Value.ToString(),
                            FullName = sht.Cell($"C{id}").Value.ToString(),
                            DateOfBirth = sht.Cell($"D{id}").Value.TryConvertToDateTime(),
                            Roles = sht.Cell($"E{id}").Value.ToString(),
                        };
                        uList.Add(user);
                    }

                    foreach (var user in uList)
                    {
                        var kq = await Task.Run(async () =>
                        {
                            return await mUser.EditUserInfo(user);
                        });
                    }

                    PerformLoadAll();
                }

                 
            }
            catch (Exception ex)
            {
                Message = ex.Message;
            }

            vmMainDc.IsWinActive = true;
            vmMainDc.SbMessage?.Enqueue(Message, null, null, null, false, true, TimeSpan.FromSeconds(2));
        }

        private ActionCommand excelCOM_Start_Command;

        public ICommand ExcelCOM_Start_Command
        {
            get
            {
                if (excelCOM_Start_Command == null)
                {
                    excelCOM_Start_Command = new ActionCommand(ExcelCOM_Start_);
                }

                return excelCOM_Start_Command;
            }
        }

        private void ExcelCOM_Start_()
        {
            var vmMainDc = App.Current.MainWindow.DataContext as vmMain;
            string Message = string.Empty;

            try
            {
                var ExcelApp = new Excel.Application();
                ExcelApp.Visible = true;

                var wb = ExcelApp.Workbooks.Add();
                var ws = (Worksheet)wb.Worksheets.Add();
                ws.Name = "DanhSach";

                ws.Cells[1, "A"].Value = "UserName";
                ws.Cells[1, "B"].Value = "Email";
                ws.Cells[1, "C"].Value = "Tên đầy đủ";
                ws.Cells[1, "D"].Value = "Ngày sinh";
                ws.Cells[1, "E"].Value = "Quyền";

                var lr = 1;
                foreach (var user in UserList)
                {
                    lr++;
                    ws.Cells[lr, "A"].Value = user.UserName;
                    ws.Cells[lr, "B"].Value = user.Email;
                    ws.Cells[lr, "C"].Value = user.FullName;
                    ws.Cells[lr, "D"].Value = user.DateOfBirth?.ToString("yyyy-MM-dd");
                    ws.Cells[lr, "E"].Value = user.Roles;
                }



            }
            catch (Exception ex)
            {
                Message = ex.Message;
            }

            vmMainDc.IsWinActive = true;
            vmMainDc.SbMessage?.Enqueue(Message, null, null, null, false, true, TimeSpan.FromSeconds(2));
        }

        private ActionCommand excelCOM_ActiveExcel_Command;

        public ICommand ExcelCOM_ActiveExcel_Command
        {
            get
            {
                if (excelCOM_ActiveExcel_Command == null)
                {
                    excelCOM_ActiveExcel_Command = new ActionCommand(ExcelCOM_ActiveExcel_);
                }

                return excelCOM_ActiveExcel_Command;
            }
        }

        private void ExcelCOM_ActiveExcel_()
        {
            var vmMainDc = App.Current.MainWindow.DataContext as vmMain;
            string Message = string.Empty;

            try
            {
                var ExcelApp = Marshal.GetActiveObject("Excel.Application") as Excel.Application;

                if (ExcelApp!=null)
                {
                    var wb = ExcelApp.ActiveWorkbook;
                    var ws = (Worksheet)wb.ActiveSheet;

                    ws.Cells[1, "A"].Value = "UserName";
                    ws.Cells[1, "B"].Value = "Email";
                    ws.Cells[1, "C"].Value = "Tên đầy đủ";
                    ws.Cells[1, "D"].Value = "Ngày sinh";
                    ws.Cells[1, "E"].Value = "Quyền";

                    var lr = 1;
                    foreach (var user in UserList)
                    {
                        lr++;
                        ws.Cells[lr, "A"].Value = user.UserName;
                        ws.Cells[lr, "B"].Value = user.Email;
                        ws.Cells[lr, "C"].Value = user.FullName;
                        ws.Cells[lr, "D"].Value = user.DateOfBirth?.ToString("yyyy-MM-dd");
                        ws.Cells[lr, "E"].Value = user.Roles;
                    }
                    Message = "Đa ghi dữ liệu";
                }
                else
                {
                    Message = "Excel chưa chạy";
                }    
            }
            catch (Exception ex)
            {
                Message = ex.Message;
            }

            vmMainDc.IsWinActive = true;
            vmMainDc.SbMessage?.Enqueue(Message, null, null, null, false, true, TimeSpan.FromSeconds(2));
        }

        private ActionCommand excelCOM_ImportActiveExcel_Command;

        public ICommand ExcelCOM_ImportActiveExcel_Command
        {
            get
            {
                if (excelCOM_ImportActiveExcel_Command == null)
                {
                    excelCOM_ImportActiveExcel_Command = new ActionCommand(ExcelCOM_ImportActiveExcel_);
                }

                return excelCOM_ImportActiveExcel_Command;
            }
        }

        private async void ExcelCOM_ImportActiveExcel_()
        {
            var vmMainDc = App.Current.MainWindow.DataContext as vmMain;
            string Message = string.Empty;

            try
            {
                var ExcelApp = Marshal.GetActiveObject("Excel.Application") as Excel.Application;

                if (ExcelApp != null)
                {
                    var wb = ExcelApp.ActiveWorkbook;
                    var ws = (Worksheet)wb.ActiveSheet;

                    long lr = ws.Cells[ws.Rows.Count, 1].End[XlDirection.xlUp].Row();

                    var uList = new List<efUser>();

                    for (long id = 2; id <= lr; id++)
                    {
                        var user = new efUser
                        {
                            UserName = ws.Range[$"A{id}"].Value,
                            Email = ws.Range[$"B{id}"].Value,
                            FullName = ws.Range[$"C{id}"].Value,
                            Roles = ws.Range[$"E{id}"].Value,
                        };

                        try
                        {
                            user.DateOfBirth = DateTime.Parse(ws.Range[$"D{id}"].Value.ToString());
                        }
                        catch
                        {

                        }
                         

                        uList.Add(user);
                    }

                    foreach (var user in uList)
                    {
                        var kq = await Task.Run(async () =>
                        {
                            return await mUser.EditUserInfo(user);
                        });
                    }

                    PerformLoadAll();

                    Message = "Đa ghi dữ liệu";
                }
                else
                {
                    Message = "Excel chưa chạy";
                }
            }
            catch (Exception ex)
            {
                Message = ex.Message;
            }

            vmMainDc.IsWinActive = true;
            vmMainDc.SbMessage?.Enqueue(Message, null, null, null, false, true, TimeSpan.FromSeconds(2));
        }
    }
}
