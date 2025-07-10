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
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Title = "Chọn file Text";
                openFileDialog.Filter = "Text file (*.txt)|*.txt";
                openFileDialog.Multiselect = false;
                
                if (openFileDialog.ShowDialog() == true)
                {
                    string filePath = openFileDialog.FileName;
                    FileContent = File.ReadAllText(filePath);
                }    
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
            if (St1!=null && St2!=null)
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
                    writeTextFileCommand = new ActionCommand(WriteTextFile);
                }

                return writeTextFileCommand;
            }
        }

        private void WriteTextFile()
        {
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Title = "Lưu file Text";
                saveFileDialog.Filter = "Text file (*.txt)|*.txt";

                if (saveFileDialog.ShowDialog() == true)
                {
                    string filePath = saveFileDialog.FileName;
                    File.WriteAllText(filePath, FileContent);
                    MessageBox.Show("Đã ghi chữ liệu");
                }
            }
            catch
            {

            }
        }
    }
}
