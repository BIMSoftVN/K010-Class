using K010Libs.Mvvm;
using Microsoft.Xaml.Behaviors.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;
using System.Windows.Input;
using System.Windows;
using TaskManage.Models;
using TaskManage.Views;
using Microsoft.Win32;
using System.IO;
using TaskManage.Classes;
using System.Xml.Linq;
using System.Collections.ObjectModel;
using Newtonsoft.Json;

namespace TaskManage.ViewModels
{
    public class vmMain : PropertyChangedBase
    {
        private string _FileContent = null;
        public string FileContent
        {
            get
            {
                return _FileContent;
            }
            set
            {
                _FileContent = value;
                OnPropertyChanged();
            }
        }




        private ActionCommand _OpenTextFileCommand;

        public ICommand OpenTextFileCommand
        {
            get
            {
                if (_OpenTextFileCommand == null)
                {
                    _OpenTextFileCommand = new ActionCommand(VoidOpenTextFileCommand);
                }

                return _OpenTextFileCommand;
            }
        }

        private async void VoidOpenTextFileCommand()
        {
            try
            {
                FileContent = ReadTextFile();
            }
            catch
            {

            }
        }





        private double? _St1;
        public double? St1
        {
            get
            {
                return _St1;
            }
            set
            {
                _St1 = value;
                OnPropertyChanged();
            }
        }

        private double? _St2;
        public double? St2
        {
            get
            {
                return _St2;
            }
            set
            {
                _St2 = value;
                OnPropertyChanged();
                TinhKetQua();
            }
        }

        private double? _KetQua;
        public double? KetQua
        {
            get
            {
                return _KetQua;
            }
            set
            {
                _KetQua = value;
                OnPropertyChanged();
            }
        }


        private void TinhKetQua()
        {
            if (St1 != null && St2 != null)
            {
                KetQua = St1 + St2;
            }
        }





        private ActionCommand writeTextFileCommand;

        public ICommand WriteTextFileCommand
        {
            get
            {
                if (writeTextFileCommand == null)
                {
                    writeTextFileCommand = new ActionCommand(WriteTextFileVoidCommand);
                }

                return writeTextFileCommand;
            }
        }

        private void WriteTextFileVoidCommand()
        {
            try
            {
                WriteTextFile(FileContent);
            }
            catch
            {

            }
        }

        private void WriteTextFile(string content)
        {
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Title = "Lưu file Text";
                saveFileDialog.Filter = "Text file (*.txt)|*.txt";

                if (saveFileDialog.ShowDialog() == true)
                {
                    string filePath = saveFileDialog.FileName;
                    File.WriteAllText(filePath, content);
                    MessageBox.Show("Đã ghi chữ liệu");
                }
            }
            catch
            {

            }
        }

        private string ReadTextFile()
        {
            string returnText = string.Empty;
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Title = "Chọn file Text";
                openFileDialog.Filter = "Text file (*.txt)|*.txt";
                openFileDialog.Multiselect = false;

                if (openFileDialog.ShowDialog() == true)
                {
                    string filePath = openFileDialog.FileName;
                    returnText = File.ReadAllText(filePath);
                }
            }
            catch
            {

            }

            return returnText;
        }



        private ObservableRangeCollection<clUser> _UserList = new ObservableRangeCollection<clUser>();
        public ObservableRangeCollection<clUser> UserList
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


        private clUser _UserSelect = new clUser();
        public clUser UserSelect
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


        private ActionCommand createUsersCommand;

        public ICommand CreateUsersCommand
        {
            get
            {
                if (createUsersCommand == null)
                {
                    createUsersCommand = new ActionCommand(CreateUsers);
                }

                return createUsersCommand;
            }
        }

        private void CreateUsers()
        {
            var names = new[] { "An", "Bình", "Chi", "Dũng", "Hà", "Hải", "Hương", "Khánh", "Lan", "Long", "Minh", "Nam", "Nga", "Phúc", "Quân", "Sơn", "Trang", "Tú", "Việt", "Yến" };
            var addresses = new[] { "Hà Nội", "Hồ Chí Minh", "Đà Nẵng", "Hải Phòng", "Cần Thơ", "Huế", "Biên Hòa", "Nha Trang", "Buôn Ma Thuột", "Vũng Tàu" };

            var radom = new Random();

            UserList.Clear();

            List<clUser> uList = new List<clUser>();
            for (int i = 0; i < 50; i++)
            {
                var age = radom.Next(18, 60);
                var dob = DateTime.Now.AddYears(-age);
                var name = names[radom.Next(0, 19)];

                var user = new clUser
                {
                    Name = name,
                    Email = $"{name}@gmail.com",
                    Age = age,
                    Phone = $"098{radom.Next(1000000, 9999999)}",
                    Address = addresses[radom.Next(0, 9)],
                    DateOfBirth = dob
                };
                uList.Add(user);
            }

            UserList.AddRange(uList);
            uList = null;
        }




        private ActionCommand saveJsonCommand;

        public ICommand SaveJsonCommand
        {
            get
            {
                if (saveJsonCommand == null)
                {
                    saveJsonCommand = new ActionCommand(SaveJson);
                }

                return saveJsonCommand;
            }
        }

        private void SaveJson()
        {
            try
            {
                if (UserList != null && UserList.Count > 0)
                {
                    string jSonUsers = JsonConvert.SerializeObject(UserList);
                    WriteTextFile(jSonUsers);
                }
                else
                {
                    MessageBox.Show("Không có thông tin");
                }
            }
            catch
            {

            }
        }


        private ActionCommand openJsonCommand;

        public ICommand OpenJsonCommand
        {
            get
            {
                if (openJsonCommand == null)
                {
                    openJsonCommand = new ActionCommand(OpenJson);
                }

                return openJsonCommand;
            }
        }
        private void OpenJson()
        {
            try
            {
                UserList.Clear();

                string jSonText = ReadTextFile();

                if (string.IsNullOrEmpty(jSonText))
                {
                    MessageBox.Show("Không có dữ liệu");
                    return;
                }

                var uList = JsonConvert.DeserializeObject<List<clUser>>(jSonText);

                UserList.AddRange(uList);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
        }






        private ActionCommand openFileSqlCommand;

        public ICommand OpenFileSqlCommand
        {
            get
            {
                if (openFileSqlCommand == null)
                {
                    openFileSqlCommand = new ActionCommand(OpenFileSql);
                }

                return openFileSqlCommand;
            }
        }

        private async void OpenFileSql()
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Title = "Chọn file dữ liệu";
                openFileDialog.Filter = "Sqlite file (*.db)|*.db";
                openFileDialog.Multiselect = false;

                if (openFileDialog.ShowDialog() == true)
                {
                    App.SqliteFilePath = openFileDialog.FileName;
                    await LoadDataFromSqlite();
                }
            }
            catch
            {

            }
        }



        private async Task LoadDataFromSqlite()
        {
            try
            {
                UserList.Clear();

                if (!string.IsNullOrEmpty(App.SqliteFilePath))
                {
                    var kq = mUser.GetAllUsers().Result;
                    if (kq.returnCode)
                    {
                        UserList.AddRange(kq.User);
                    }
                    else
                    {
                        MessageBox.Show(kq.returnMessage);
                    }
                }
                else
                {
                    MessageBox.Show("Vui lòng chọn file Sqlite");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private ActionCommand reloadSqliteCommand;

        public ICommand ReloadSqliteCommand
        {
            get
            {
                if (reloadSqliteCommand == null)
                {
                    reloadSqliteCommand = new ActionCommand(ReloadSqlite);
                }

                return reloadSqliteCommand;
            }
        }

        private async void ReloadSqlite()
        {
            await LoadDataFromSqlite();
        }

        private ActionCommand createUserSqliteCommand;

        public ICommand CreateUserSqliteCommand
        {
            get
            {
                if (createUserSqliteCommand == null)
                {
                    createUserSqliteCommand = new ActionCommand(CreateUserSqlite);
                }

                return createUserSqliteCommand;
            }
        }

        private async void CreateUserSqlite()
        {
            var win = new vUserInfo();
            var vm = win.DataContext as vmUserInfo;
            win.ShowDialog();

            await LoadDataFromSqlite();
        }





        private ActionCommand editUserCommand;

        public ICommand EditUserCommand
        {
            get
            {
                if (editUserCommand == null)
                {
                    editUserCommand = new ActionCommand(EditUser);
                }

                return editUserCommand;
            }
        }

        private async void EditUser()
        {
            var win = new vUserInfo();
            var vm = win.DataContext as vmUserInfo;
            vm.User = UserSelect;
            win.ShowDialog();

            await LoadDataFromSqlite();
        }
    }
}
