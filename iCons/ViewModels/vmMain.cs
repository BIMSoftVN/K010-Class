using DevExpress.Mvvm;
using DevExpress.Xpf.WindowsUI;
using iCons.Classes;
using iCons.Models;
using iCons.Views;
using K010Libs.Mvvm;
using MaterialDesignThemes.Wpf;
using Microsoft.Xaml.Behaviors.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

namespace iCons.ViewModels
{
    public class vmMain : PropertyChangedBase
    {
        private bool _IsWinActive = true;
        public bool IsWinActive
        {
            get
            {
                return _IsWinActive;
            }
            set
            {
                _IsWinActive = value;
                OnPropertyChanged();
            }
        }


        private SnackbarMessageQueue _SbMessage = new SnackbarMessageQueue();
        public SnackbarMessageQueue SbMessage
        {
            get
            {
                return _SbMessage;
            }
            set
            {
                _SbMessage = value;
                OnPropertyChanged();
            }
        }

        private string _PbMessage = "Đang tải";
        public string PbMessage
        {
            get
            {
                return _PbMessage;
            }
            set
            {
                _PbMessage = value;
                OnPropertyChanged();
            }
        }

        private double _PbMaxValue = 0;
        public double PbMaxValue
        {
            get
            {
                return _PbMaxValue;
            }
            set
            {
                _PbMaxValue = value;
                OnPropertyChanged();
            }
        }

        private double _PbValue = 0;
        public double PbValue
        {
            get
            {
                return _PbValue;
            }
            set
            {
                _PbValue = value;
                OnPropertyChanged();
            }
        }


        private bool _PbIsIndeterminate = true;
        public bool PbIsIndeterminate
        {
            get
            {
                return _PbIsIndeterminate;
            }
            set
            {
                _PbIsIndeterminate = value;
                OnPropertyChanged();
            }
        }





        private ActionCommand openAccInfoWin_Command;

        public ICommand OpenAccInfoWin_Command
        {
            get
            {
                if (openAccInfoWin_Command == null)
                {
                    openAccInfoWin_Command = new ActionCommand(OpenAccInfoWin_);
                }

                return openAccInfoWin_Command;
            }
        }



        private efUser _UserInfo = new efUser();
        public efUser UserInfo
        {
            get
            {
                return _UserInfo;
            }
            set
            {
                _UserInfo = value;
                OnPropertyChanged();
            }
        }


        private async void OpenAccInfoWin_(object par)
        {
            IsWinActive = false;
            PbMessage = "Đang tải thông tin";

            try
            {
                string[] Roles = App.MainUser.Roles.Split(',');
                var IsRole = Roles.Contains("HSE1");

                if (IsRole == false)
                {
                    SbMessage?.Enqueue("Bạn không có quyền truy cập vào chức năng này", null, null, null, false, true, TimeSpan.FromSeconds(2));
                    IsWinActive = true;
                    return;
                }


                var win = par as WinUIDialogService;
                if (win != null)
                {
                    var kqUser = await mUser.GetUserByEmail(App.MainUser.UserName);
                    if (kqUser.IsSuccess == true)
                    {
                        UserInfo = kqUser.UserInfo;
                        var kq = win.ShowDialog(dialogButtons: MessageButton.OKCancel, title: "Thông tin người dùng", viewModel: this);

                        if (kq == MessageResult.OK)
                        {
                            var kqEdit = await mUser.EditUserInfo(UserInfo);
                            SbMessage?.Enqueue(kqEdit.Message, null, null, null, false, true, TimeSpan.FromSeconds(2));
                        }
  
                        
                    }
                    else
                    {
                        SbMessage?.Enqueue(kqUser.Message, null, null, null, false, true, TimeSpan.FromSeconds(2));
                    }
                }
                else
                {
                    SbMessage?.Enqueue("Không thể mở cửa sổ thông tin người dùng", null, null, null, false, true, TimeSpan.FromSeconds(2));
                }
            }
            catch (Exception ex)
            {
                SbMessage?.Enqueue(ex.Message, null, null, null, false, true, TimeSpan.FromSeconds(2));
            }

                

            IsWinActive = true;
        }





        private ActionCommand changeUserPhoto;

        public ICommand ChangeUserPhoto
        {
            get
            {
                if (changeUserPhoto == null)
                {
                    changeUserPhoto = new ActionCommand(PerformChangeUserPhoto);
                }

                return changeUserPhoto;
            }
        }

        private void PerformChangeUserPhoto()
        {
            IsWinActive = false;
            PbMessage = "Đổi ảnh người dùng";

            try
            {
                using (var oFDg = new OpenFileDialog())
                {
                    oFDg.Title = "Chọn ảnh đại diện người dùng";
                    oFDg.Filter = "Các file ảnh|*.jpg;*.jpeg;*.png;*.bmp;*.gif";

                    oFDg.Multiselect = false;

                    if (oFDg.ShowDialog() == DialogResult.OK)
                    {
                        UserInfo.Photo = File.ReadAllBytes(oFDg.FileName);
                    }
                }    
            }
            catch (Exception ex)
            {
                SbMessage?.Enqueue(ex.Message, null, null, null, false, true, TimeSpan.FromSeconds(2));
            }

            IsWinActive = true;
        }
    }
}
