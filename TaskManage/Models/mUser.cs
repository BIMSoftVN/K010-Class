using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using TaskManage.Classes;

namespace TaskManage.Models
{
    //[TestFixture]
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
        
        
        //[Test] 
        //public async Task TestSQlIte()
        //{
        //    string filePath = @"C:\Users\kysudo\Desktop\K010.db";
        //    var kq = await GetAllUsers(filePath);
        //    Console.WriteLine(kq.returnMessage);
        //}



        public static async Task<(bool returnCode, string returnMessage, List<clUser> User)> GetAllUsers()
        {
            try
            {
                string connectionString = $"Data Source={App.SqliteFilePath};Version=3;";

                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    var uList = new List<clUser>();

                    using (var cmd = connection.CreateCommand())
                    {
                        cmd.CommandText = "SELECT * FROM [Users]";
                        var reader = await cmd.ExecuteReaderAsync();
                        using (var dt = new DataTable())
                        {
                            dt.Load(reader);
                            uList = JToken.FromObject(dt).ToObject<List<clUser>>();
                        }    
                    }    

                    connection.Close();

                    if (uList !=null && uList.Count >0)
                    {
                        return (true, $"Đã lấy {uList.Count} người dùng", uList);
                    }
                    else
                    {
                        return (false, "Không có người dùng nào", null);
                    }    
                }    
            }
            catch
            {
                return (false, "Lỗi khi lấy danh sách người dùng", null);
            }
        }

        public static async Task<(bool returnCode, string returnMessage)> AddNewUser(clUser user)
        {
            try
            {
                string connectionString = $"Data Source={App.SqliteFilePath};Version=3;";

                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    using (var transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            using (var cmd = connection.CreateCommand())
                            {
                                cmd.Parameters.AddWithValue("@Name", user.Name);
                                cmd.Parameters.AddWithValue("@Email", user.Email);
                                cmd.Parameters.AddWithValue("@Age", user.Age);
                                cmd.Parameters.AddWithValue("@Phone", user.Phone);
                                cmd.Parameters.AddWithValue("@Address", user.Address);
                                cmd.Parameters.AddWithValue("@DateOfBirth", user.DateOfBirth);

                                cmd.CommandText = "INSERT INTO [Users]([Name],[Email],[Age],[Phone],[Address],[DateOfBirth]) " +
                                    "VALUES (@Name,@Email,@Age,@Phone,@Address,@DateOfBirth)";

                                var result = await cmd.ExecuteNonQueryAsync();
                                transaction.Commit();

                                if (result == 0)
                                {
                                    return (true, $"Chưa thêm {user.Name} người dùng");
                                }
                                return (true, $"Đã thêm {user.Name} người dùng");

                            }
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            return (false, ex.Message);
                        }
                        
                    }

                    
                }
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public static async Task<(bool returnCode, string returnMessage)> EditUser(clUser user)
        {
            try
            {
                string connectionString = $"Data Source={App.SqliteFilePath};Version=3;";

                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    using (var transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            using (var cmd = connection.CreateCommand())
                            {
                                cmd.Parameters.AddWithValue("@Id", user.Id);
                                cmd.Parameters.AddWithValue("@Name", user.Name);
                                cmd.Parameters.AddWithValue("@Email", user.Email);
                                cmd.Parameters.AddWithValue("@Age", user.Age);
                                cmd.Parameters.AddWithValue("@Phone", user.Phone);
                                cmd.Parameters.AddWithValue("@Address", user.Address);
                                cmd.Parameters.AddWithValue("@DateOfBirth", user.DateOfBirth);

                                cmd.CommandText = "UPDATE [Users] SET " +
                                        "[Name]=@Name,[Email]=@Email,[Age]=@Age,[Phone]=@Phone,[Address]=@Address,[DateOfBirth]=@DateOfBirth " +
                                        "where [Id]=@Id";

                                var result = await cmd.ExecuteNonQueryAsync();
                                transaction.Commit();

                                if (result == 0)
                                {
                                    return (true, $"Chưa sửa {user.Name} người dùng");
                                }
                                return (true, $"Đã sửa {user.Name} người dùng");

                            }
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            return (false, ex.Message);
                        }

                    }


                }
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public static async Task<(bool returnCode, string returnMessage)> DeleteUser(clUser user)
        {
            try
            {
                string connectionString = $"Data Source={App.SqliteFilePath};Version=3;";

                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    using (var transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            using (var cmd = connection.CreateCommand())
                            {
                                cmd.Parameters.AddWithValue("@Id", user.Id);

                                cmd.CommandText = "DELETE FROM [Users] where [Id]=@Id";

                                var result = await cmd.ExecuteNonQueryAsync();
                                transaction.Commit();

                                if (result == 0)
                                {
                                    return (true, $"Chưa xóa {user.Name} người dùng");
                                }
                                return (true, $"Đã xóa {user.Name} người dùng");

                            }
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            return (false, ex.Message);
                        }

                    }


                }
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

    }
}
