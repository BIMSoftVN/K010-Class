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
            var kq = await GetUserByEmail("anhdt");
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

        public static async Task<(bool IsSuccess, string Message, efUser UserInfo)> GetUserByEmail(string Email)
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
                            var uInfo = await _dbContext.Users.AsNoTracking()
                                .Where(u=>u.Email == Email || u.UserName == Email).FirstOrDefaultAsync();
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
    }
}
