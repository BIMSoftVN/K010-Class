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
            for (var i=1; i < 10; i++)
            {
                var kq = await AddUser(new efUser
                {
                    UserName = "testuser" + i,
                    Email = "testemail"+ i +"@gmail.com",
                    FullName = "Test User "+ i,
                });
                Console.WriteLine(kq.Message);
            }
            
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
                                isSuccess = true;
                                message = "Đã tải danh sách người dùng";
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


        public static async Task<(bool IsSuccess, string Message)> SendEmailChangePassword(string EmailInput)
        {
            bool isSuccess = false;
            string message = string.Empty;
            efUser userInfo = new efUser();

            try
            {
                if (string.IsNullOrEmpty(EmailInput))
                {
                    message = "Email không được để trống";
                    return (isSuccess, message);
                }


                return (true, "Đã gửi Email đổi mật khẩu");
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return (isSuccess, message);
        }


        public static async Task<(bool IsSuccess, string Message)> EditUserInfo(efUser UserInfo)
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
                                .Where(o => o.UserName == UserInfo.UserName)
                                .FirstOrDefaultAsync();


                            if (uInfo != null)
                            {
                                uInfo.FullName = UserInfo.FullName;
                                uInfo.DateOfBirth = UserInfo.DateOfBirth;
                                uInfo.Email = UserInfo.Email;
                                uInfo.Photo = UserInfo.Photo;

                                var kqChange = await _dbContext.SaveChangesAsync();


                                if (kqChange > 0)
                                {
                                    isSuccess = true;
                                    message = "Đã cập nhật thông tin người dùng";


                                }
                                else
                                {
                                    message = "Không thể cập nhật thông tin người dùng";
                                }
                            }
                            else
                            {
                                _dbContext.Users.Add(UserInfo);
                                var kqChange = await _dbContext.SaveChangesAsync();

                                if (kqChange > 0)
                                {
                                    isSuccess = true;
                                    message = "Đã thêm người dùng";
                                }
                                else
                                {
                                    message = "Không thể thên người dùng";
                                }
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
            return (isSuccess, message);
        }

        public static async Task<(bool IsSuccess, string Message)> AddUser(efUser UserInfo)
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
                                .Where(o => o.UserName == UserInfo.UserName || o.Email == UserInfo.Email)
                                .FirstOrDefaultAsync();


                            if (uInfo == null)
                            {
                                _dbContext.Users.Add(UserInfo);
                                var kqChange = await _dbContext.SaveChangesAsync();

                                if (kqChange > 0)
                                {
                                    isSuccess = true;
                                    message = "Đã thêm người dùng";


                                }
                                else
                                {
                                    message = "Không thể thên người dùng";
                                }
                            }
                            else
                            {
                                message = "Username hoặc Email đã tồn tại";
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
            return (isSuccess, message);
        }

        public static async Task<(bool IsSuccess, string Message)> DeleteUser(efUser UserInfo)
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
                                .Where(o => o.UserName == UserInfo.UserName)
                                .FirstOrDefaultAsync();


                            if (uInfo != null)
                            {
                                _dbContext.Users.Remove(uInfo);
                                var kqChange = await _dbContext.SaveChangesAsync();

                                if (kqChange > 0)
                                {
                                    isSuccess = true;
                                    message = "Đã xóa người dùng";


                                }
                                else
                                {
                                    message = "Không thể xóa người dùng";
                                }
                            }
                            else
                            {
                                message = "Username không tồn tại";
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
            return (isSuccess, message);
        }

        public static async Task<(bool IsSuccess, string Message)> DeleteUserList(ICollection<efUser> UserList)
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
                            var UNameList = UserList.Select(o => o.UserName).ToList();


                            var uInfoList = await _dbContext.Users
                                .Where(o => UNameList.Contains(o.UserName))
                                .ToListAsync();


                            if (uInfoList != null && uInfoList.Count >0)
                            {
                                _dbContext.Users.RemoveRange(uInfoList);
                                var kqChange = await _dbContext.SaveChangesAsync();

                                if (kqChange > 0)
                                {
                                    isSuccess = true;
                                    message = $"Đã xóa {kqChange} người dùng";


                                }
                                else
                                {
                                    message = "Không thể xóa người dùng";
                                }
                            }
                            else
                            {
                                message = "Username không tồn tại";
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
            return (isSuccess, message);
        }

    }
}
