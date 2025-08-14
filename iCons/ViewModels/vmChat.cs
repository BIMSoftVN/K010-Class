using iCons.Classes;
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
    public class vmChat : PropertyChangedBase
    {
        private ObservableRangeCollection<clChatInfo> _ChatList = new ObservableRangeCollection<clChatInfo>();
        public ObservableRangeCollection<clChatInfo> ChatList
        {
            get
            {
                return _ChatList;
            }
            set
            {
                _ChatList = value;
                OnPropertyChanged();
            }
        }

        private string _InputText = null;
        public string InputText
        {
            get
            {
                return _InputText;
            }
            set
            {
                _InputText = value;
                OnPropertyChanged();
            }
        }




        private ActionCommand send_Command;

        public ICommand Send_Command
        {
            get
            {
                if (send_Command == null)
                {
                    send_Command = new ActionCommand(Send_);
                }

                return send_Command;
            }
        }

        private async void Send_()
        {
            if (string.IsNullOrEmpty(InputText))
            {
                return;
            }

            var uChat = new clChatInfo
            {
                ChatUser = App.MainUser.FullName,
                Message = InputText
            };

            ChatList.Add(uChat);


            var kq = await mGoogle.Dich(InputText);

            if (kq.IsSuccess == true)
            {
                var gChat = new clChatInfo
                {
                    ChatUser = "Google",
                    Message = kq.Message
                };

                ChatList.Add(gChat);
            }    

            InputText = string.Empty;
        }
    }
}
