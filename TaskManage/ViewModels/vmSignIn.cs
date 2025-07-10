using K010Libs.Mvvm;
using Microsoft.Xaml.Behaviors.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TaskManage.Classes;
using TaskManage.Models;


namespace TaskManage.ViewModels
{
    public class vmSignIn : PropertyChangedBase
    {
        public Window win { get; set; }

        public vmSignIn()
        {
            Email = null;
            Password = null;
        }

        private string _Email;
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

        private string _Password;
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


        private clUser _User;
        public clUser User
        {
            get
            {
                return _User;
            }
            set
            {
                _User = value;
                OnPropertyChanged();
            }
        }


        private ActionCommand dangNhapCommand;

        public ICommand DangNhapCommand
        {
            get
            {
                if (dangNhapCommand == null)
                {
                    dangNhapCommand = new ActionCommand(DangNhap);
                }

                return dangNhapCommand;
            }
        }

        private async void DangNhap()
        {
            try
            {
                var result = await mUser.SignIn(Email, Password);
                if (true == true)
                {
                    User = result.User;
                    win.Hide();
                }
                else
                {
                    MessageBox.Show(result.returnMessage);
                }    
            }
            catch
            {

            }
        }
    }
}
