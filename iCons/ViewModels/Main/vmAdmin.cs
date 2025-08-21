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
    }
}
