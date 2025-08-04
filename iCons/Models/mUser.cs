using iCons.Classes;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iCons.Models
{
    [TestFixture]
    public class mUser
    {
        [Test]
        public static async Task Test()
        {
            var kq = await GetUserByEmail("anhdt@xuanmaicorp.vn");
            Console.WriteLine(kq.Message);
        }


        public static async Task<(bool IsSuccess, string Message, List<efUser> UserList)> GetAllUsers()
        {
            bool isSuccess = false;
            string message = string.Empty;
            List<efUser> userList = new List<efUser>();

            try
            {
                if (!string.IsNullOrEmpty(App.ConnectionString)) 
                {
                    using (var _dbContext = new AppDbContext(App.ConnectionString))
                    {
                        if(_dbContext!=null)
                        {
                            var uList = await _dbContext.Users.AsNoTracking().ToListAsync();
                            if (uList!=null)
                            {
                                userList = uList;
                            }    
                        }
                        else
                        {
                            message = "Không thể kết nối đến cơ sở dữ liệu";
                        }
                    };
                }
                else

                {
                    message = "Không có chuỗi kết nối";
                }    
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return (isSuccess, message, userList);
        }

        public static async Task<(bool IsSuccess, string Message, efUser UserInfo)> GetUserByEmail(string EmailInput)
        {
            bool isSuccess = false;
            string message = string.Empty;
            efUser userInfo = new efUser();

            try
            {
                if (!string.IsNullOrEmpty(App.ConnectionString))
                {
                    using (var _dbContext = new AppDbContext(App.ConnectionString))
                    {
                        if (_dbContext != null)
                        {
                            var uInfo = await _dbContext.Users
                                .AsNoTracking()
                                .Where(o=>o.Email == EmailInput || o.UserName == EmailInput)
                                .FirstOrDefaultAsync();


                            if (uInfo != null)
                            {
                                userInfo = uInfo;
                                isSuccess = true;
                                message = "Đã tải thông tin";
                            }
                            else
                            {
                                message = "Không tìm thấy người dùng với email hoặc tên đăng nhập này";
                            }    
                        }
                        else
                        {
                            message = "Không thể kết nối đến cơ sở dữ liệu";
                        }
                    };
                }
                else

                {
                    message = "Không có chuỗi kết nối";
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return (isSuccess, message, userInfo);
        }

        public static async Task<(bool IsSuccess, string Message, efUser UserInfo)> SignIn(string EmailInput, string Password)
        {
            bool isSuccess = false;
            string message = string.Empty;
            efUser userInfo = new efUser();

            try
            {
                if(string.IsNullOrEmpty(EmailInput))
                {
                    message = "Email không được để trống";
                    return (isSuccess, message, userInfo);
                }


                if (string.IsNullOrEmpty(Password))
                {
                    message = "Password không được để trống";
                    return (isSuccess, message, userInfo);
                }


                if (!string.IsNullOrEmpty(App.ConnectionString))
                {
                    using (var _dbContext = new AppDbContext(App.ConnectionString))
                    {
                        if (_dbContext != null)
                        {
                            var uInfo = await _dbContext.Users
                                .AsNoTracking()
                                .Where(o => o.Email == EmailInput || o.UserName == EmailInput)
                                .FirstOrDefaultAsync();



                            if (uInfo == null)
                            {
                                message = "Không tìm thấy người dùng";
                                return (isSuccess, message, userInfo);
                            }

                            if (uInfo.IsLocked == true)
                            {
                                message = "Tài khoản đang bị khóa";
                                return (isSuccess, message, userInfo);
                            }


                            if (uInfo.Password != Password)
                            {
                                message = "Mật khẩu không hợp lệ";
                                return (isSuccess, message, userInfo);
                            }

                            userInfo = uInfo;
                            isSuccess = true;
                            message = "Đăng nhập thành công";
                        }
                        else
                        {
                            message = "Không thể kết nối đến cơ sở dữ liệu";
                        }
                    };
                }
                else

                {
                    message = "Không có chuỗi kết nối";
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return (isSuccess, message, userInfo);
        }
    }
}
