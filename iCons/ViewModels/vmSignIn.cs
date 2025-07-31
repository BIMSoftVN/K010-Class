using iCons.Models;
using K010Libs.Mvvm;
using Microsoft.Xaml.Behaviors.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace iCons.ViewModels
{
    public class vmSignIn : PropertyChangedBase
    {
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
            try
            {
                var kq = await mUser.GetUserByEmail(this.Email);
                if (kq.IsSuccess)
                {

                }
            }
            catch
            {

            }
        }
    }
}
