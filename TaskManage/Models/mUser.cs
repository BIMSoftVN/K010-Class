using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using TaskManage.Classes;

namespace TaskManage.Models
{
    public class mUser
    {
        public static async Task<(bool returnCode, string returnMessage, clUser User)> SignIn(string email, string password)
        {
            return (true, "Thành công", new clUser());

            bool returnCode = false;
            string returnMessage = string.Empty;


            clUser returnUser = new clUser();

            if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(password))
            {
                if (email == "anhdt@xuanmaicorp.vn")
                {
                    if (password == "123456")
                    {
                        returnUser.Name = "Đỗ Thế Anh";
                        returnUser.Age = 36;
                        returnUser.Email = email;
                        returnUser.Phone = "0987654321";
                        returnUser.DateOfBirth = new DateTime(1987, 1, 1);
                        returnCode = true;
                        returnMessage = "Đăng nhập thành công!";
                    }    
                    else
                    {
                        returnMessage = "Mật khẩu không đúng!";
                    }    
                }
                else if (email == "hungxedap")
                {
                    if (password == "123")
                    {
                        returnUser.Name = "Hùng xe đạp";
                        returnUser.Age = 18;
                        returnUser.Email = email;
                        returnUser.Phone = "123";
                        returnUser.DateOfBirth = new DateTime(1998, 1, 1);
                        returnCode = true;
                        returnMessage = "Đăng nhập thành công!";
                    }
                    else
                    {
                        returnMessage = "Mật khẩu không đúng!";
                    }
                } 
                else
                {
                    returnMessage = "Không có người dùng";
                }    
            }   
            else
            {
                returnMessage = "Vui lòng nhập thông tin";
            }    

            return (returnCode, returnMessage, returnUser);
        }
    }
}
