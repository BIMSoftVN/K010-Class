using K010Libs.Mvvm;
using Microsoft.Xaml.Behaviors.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TaskManage.Classes;
using TaskManage.Models;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;

namespace TaskManage.ViewModels
{
    public class vmUserInfo : PropertyChangedBase
    {
        public Window _win;


        private clUser _User = new clUser();
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


        private ActionCommand addNewUser;

        public ICommand AddNewUser
        {
            get
            {
                if (addNewUser == null)
                {
                    addNewUser = new ActionCommand(PerformAddNewUser);
                }

                return addNewUser;
            }
        }

        private async void PerformAddNewUser()
        {
            try
            {
                if (User.Id==null)
                {
                    var kq = await mUser.AddNewUser(User);
                    if (kq.returnCode == true)
                    {
                        _win.Close();
                    }
                    else
                    {
                        MessageBox.Show(kq.returnMessage, "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }    
                else
                {
                    var kq = await mUser.EditUser(User);
                    if (kq.returnCode == true)
                    {
                        _win.Close();
                    }
                    else
                    {
                        MessageBox.Show(kq.returnMessage, "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }    
                
            }
            catch
            {

            }
        }
    }
}
