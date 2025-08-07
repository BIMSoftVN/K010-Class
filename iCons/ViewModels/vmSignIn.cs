using iCons.Models;
using iCons.Views;
using K010Libs.Mvvm;
using MaterialDesignThemes.Wpf;
using Microsoft.Xaml.Behaviors.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace iCons.ViewModels
{
    public class vmSignIn : PropertyChangedBase
    {
        public vSignIn Win;


        private string _Email = Properties.Settings.Default.Email;
        public string Email
        {
            get
            {
                return _Email;
            }
            set
            {
                _Email = value;
                OnPropertyChanged();
            }
        }

        private string _Password = Properties.Settings.Default.Password;
        public string Password
        {
            get
            {
                return _Password;
            }
            set
            {
                _Password = value;
                OnPropertyChanged();
            }
        }


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



        private ActionCommand signInCommand;

        public ICommand SignInCommand
        {
            get
            {
                if (signInCommand == null)
                {
                    signInCommand = new ActionCommand(SignIn);
                }

                return signInCommand;
            }
        }

        private async void SignIn()
        {
            string Message = string.Empty;

            IsWinActive = false;
            PbIsIndeterminate = true;
            PbMessage = "Đang đăng nhập...";

            try
            {
                var kq =  await Task.Run(async () =>
                {
                    return await mUser.SignIn(this.Email, this.Password);
                });


                Message = kq.Message;

                if (kq.IsSuccess)
                {
                    Properties.Settings.Default.Email = this.Email;
                    Properties.Settings.Default.Password = this.Password;
                    Properties.Settings.Default.Save();

                    Win.Hide();
                }  
            } 
            catch (Exception ex)
            {
                Message = ex.Message;
            }

            IsWinActive = true;

            SbMessage?.Enqueue(Message, null, null, null, false, true, TimeSpan.FromSeconds(2));
        }

        private ActionCommand changePasswordCommand;

        public ICommand ChangePasswordCommand
        {
            get
            {
                if (changePasswordCommand == null)
                {
                    changePasswordCommand = new ActionCommand(ChangePassword);
                }

                return changePasswordCommand;
            }
        }

        private async void ChangePassword()
        {
            string Message = string.Empty;

            IsWinActive = false;
            PbIsIndeterminate = true;
            PbMessage = "Đang gửi thông tin...";

            try
            {
                var kq = await Task.Run(async () =>
                {
                    return await mUser.SendEmailChangePassword(this.Email);
                });

                Message = kq.Message;
            }
            catch (Exception ex)
            {
                Message = ex.Message;
            }

            IsWinActive = true;

            SbMessage?.Enqueue(Message, null, null, null, false, true, TimeSpan.FromSeconds(2));
        }
    }
}
