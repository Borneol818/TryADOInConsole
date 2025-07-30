using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp5
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var repo = new UserInfoRepository("Server=.;Database=WormHole;Trusted_Connection=True;");
            int input;
            do
            {
                Console.WriteLine("請輸入數字1-7，選擇操作：\n" +
                              "1. 新增使用者\n" +
                              "2. 列出所有使用者\n" +
                              "3. 列出最近新增的五筆使用者\n" +
                              "4. 修改使用者資料\n" +
                              "5. 刪除使用者\n" +
                              "6. 列出指定使用者\n" +
                              "7. 離開程式"
                );
                input = Int32.Parse(Console.ReadLine());
                switch (input)
                {
                    case 1:
                        string name, nickname, email, phone;
                        int? wallet; bool sex; DateTime birthday;
                        Console.WriteLine("請輸入使用者姓名：");
                        name = Console.ReadLine();
                        Console.WriteLine("請輸入使用者暱稱：");
                        nickname = Console.ReadLine();
                        Console.WriteLine("請輸入使用者電子郵件：");
                        email = Console.ReadLine();
                        Console.WriteLine("請輸入使用者性別（true/false）：");
                        sex = bool.Parse(Console.ReadLine());
                        Console.WriteLine("請輸入使用者生日（yyyy-MM-dd）：");
                        birthday = DateTime.Parse(Console.ReadLine());
                        Console.WriteLine("請輸入使用者電話：");
                        phone = Console.ReadLine();
                        Console.WriteLine("請輸入使用者錢包餘額（可選，若無則留空）：");
                        wallet = null;
                        var walletInput = Console.ReadLine();
                        if (!string.IsNullOrEmpty(walletInput) && int.TryParse(walletInput, out int walletValue))
                        {
                            wallet = walletValue;
                        }

                        var userInfo = new UserInfo
                        {
                            Name = name,
                            Nickname = nickname,
                            Email = email,
                            Phone = phone,
                            Sex = sex,
                            Birthday = birthday,
                            Wallet = wallet
                        };
                        var userAddDone = repo.AddUser(userInfo);
                        Console.WriteLine("使用者新增成功！目前所有使用者資料如下：");
                        foreach (var user in userAddDone)
                        {
                            Console.WriteLine(
                                $"UserID: {user.UserID},\n" +
                                $"Name: {user.Name},\n" +
                                $"Nickname: {user.Nickname}\n" +
                                $"Sex: {user.Sex},\n" +
                                $"Email: {user.Email},\n" +
                                $"Phone: {user.Phone},\n" +
                                $"Birthday: {user.Birthday.ToShortDateString()},\n" +
                                $"Wallet: {user.Wallet?.ToString() ?? "No Wallet"}\n"
                            );
                        }
                        break;
                    case 2:
                        var users = repo.GetUsers();
                        foreach (var user in users)
                        {
                            Console.WriteLine(
                                $"UserID: {user.UserID},\n" +
                                $"Name: {user.Name},\n" +
                                $"Nickname: {user.Nickname}\n" +
                                $"Sex: {user.Sex},\n" +
                                $"Email: {user.Email},\n" +
                                $"Phone: {user.Phone},\n" +
                                $"Birthday: {user.Birthday.ToShortDateString()},\n" +
                                $"Wallet: {user.Wallet?.ToString() ?? "No Wallet"}\n"
                            );
                        }
                        break;
                    case 3:
                        var usersTop5 = repo.GetTop5Users();
                        foreach (var user in usersTop5)
                        {
                            Console.WriteLine(
                                $"UserID: {user.UserID},\n" +
                                $"Name: {user.Name},\n" +
                                $"Nickname: {user.Nickname}\n" +
                                $"Sex: {user.Sex},\n" +
                                $"Email: {user.Email},\n" +
                                $"Phone: {user.Phone},\n" +
                                $"Birthday: {user.Birthday.ToShortDateString()},\n" +
                                $"Wallet: {user.Wallet?.ToString() ?? "No Wallet"}\n"
                            );
                        }
                        break;
                    case 4:
                        Console.WriteLine("請輸入要修改的使用者名稱(Name):");
                        string searchName = Console.ReadLine();
                        var userToUpdate = repo.GetOneUser(searchName);
                        if(userToUpdate.Name != null)
                        {
                            int inputSelect;
                            do
                            {
                                Console.WriteLine("請選擇要修改的欄位：\n" +
                              "1. 名字\n" +
                              "2. 暱稱\n" +
                              "3. 性別\n" +
                              "4. Email\n" +
                              "5. 電話\n" +
                              "6. 生日\n" +
                              "7. 離開");
                                inputSelect = Int32.Parse(Console.ReadLine());
                                switch (inputSelect)
                                {
                                    case 1:
                                        Console.WriteLine("請輸入新的名字：");
                                        userToUpdate.Name = Console.ReadLine();
                                        break;
                                    case 2:
                                        Console.WriteLine("請輸入新的暱稱：");
                                        userToUpdate.Nickname = Console.ReadLine();
                                        break;
                                    case 3:
                                        Console.WriteLine("請輸入新的性別（true/false）：");
                                        userToUpdate.Sex = bool.Parse(Console.ReadLine());
                                        break;
                                    case 4:
                                        Console.WriteLine("請輸入新的Email：");
                                        userToUpdate.Email = Console.ReadLine();
                                        break;
                                    case 5:
                                        Console.WriteLine("請輸入新的電話：");
                                        userToUpdate.Phone = Console.ReadLine();
                                        break;
                                    case 6:
                                        Console.WriteLine("請輸入新的生日（yyyy-MM-dd）：");
                                        userToUpdate.Birthday = DateTime.Parse(Console.ReadLine());
                                        break;
                                    case 7:
                                        Console.WriteLine("離開修改。");
                                        break;
                                    default:
                                        Console.WriteLine("無效的選擇，請重新輸入。");
                                        break;
                                }
                            } while (inputSelect != 7);
                            Console.WriteLine("確定修改嗎? y/n");
                            var confirm = Console.ReadLine();
                            if (confirm?.ToLower() != "y")
                            {
                                Console.WriteLine("修改已取消。");
                                break;
                            }
                            repo.UpdateUserInfo(userToUpdate);
                            Console.WriteLine("修改完成，請確認修改後的使用者資料：");
                        }
                        else
                        {
                            Console.WriteLine("未找到使用者，請確認名稱是否正確。");
                            break;
                        }
                        
                        break;
                    case 5:
                        Console.WriteLine("請輸入要刪除的使用者名稱(Name)：");
                        string deleteUserName = Console.ReadLine();
                        repo.DeleteUser(deleteUserName);
                        Console.WriteLine("使用者刪除成功！請查詢確認");
                        break;
                    case 6:
                        Console.WriteLine("請輸入要查詢的使用者名稱(Name)：");
                        string searchUserName = Console.ReadLine();
                        var foundUser = repo.GetOneUser(searchUserName);
                        Console.WriteLine(foundUser.Name != null
                            ? $"UserID: {foundUser.UserID},\n" +
                              $"Name: {foundUser.Name},\n" +
                              $"Nickname: {foundUser.Nickname},\n" +
                              $"Sex: {foundUser.Sex},\n" +
                              $"Email: {foundUser.Email},\n" +
                              $"Phone: {foundUser.Phone},\n" +
                              $"Birthday: {foundUser.Birthday.ToShortDateString()},\n" +
                              $"Wallet: {foundUser.Wallet?.ToString() ?? "No Wallet"}\n"
                             : "未找到使用者。\n"
                        );

                        break;
                    case 7:
                        Console.WriteLine("離開程式。");
                        return;
                    default:
                        Console.WriteLine("無效的選擇，請重新輸入。");
                        break;
                }
            } while (input != 7);
        }

        public class UserInfo
        {
            public int UserID { get; set; }
            public string Email { get; set; }
            public string Name { get; set; }
            public string Nickname { get; set; }
            public bool Sex { get; set; }
            public string Phone { get; set; }
            public DateTime Birthday { get; set; }
            public int? Wallet { get; set; }
        }
        public class UserInfoRepository
        {
            //string connectionString = "Server=.;Database=WormHole;Trusted_Connection=True;";
            private readonly string _connStr;
            public UserInfoRepository(string connectionString)
            {
                _connStr = connectionString;
            }

            public List<UserInfo> GetUsers()
            {
                var users = new List<UserInfo>();
                string sql = "SELECT * FROM UserInfo;";

                SqlConnection conn = new SqlConnection(_connStr);
                SqlCommand cmd = new SqlCommand(sql, conn);
                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    users.Add(new UserInfo
                    {
                        UserID = reader.GetInt32(0),
                        Name = reader.GetString(2),
                        Nickname = reader.GetString(3),
                        Email = reader.GetString(1),
                        Sex = reader.GetBoolean(4),
                        Phone = reader.GetString(5),
                        Birthday = reader.GetDateTime(6),
                        Wallet = reader.IsDBNull(10) ? (int?)null : reader.GetInt32(10)
                    });
                }
                conn.Close();
                return users;
            }

            public List<UserInfo> GetTop5Users()
            {
                string sql = "SELECT TOP 5 * FROM UserInfo ORDER BY UserID DESC;";
                SqlConnection conn = new SqlConnection(_connStr);
                SqlCommand cmd = new SqlCommand(sql, conn);
                conn.Open();
                var users = new List<UserInfo>();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    users.Add(new UserInfo
                    {
                        UserID = reader.GetInt32(0),
                        Name = reader.GetString(2),
                        Nickname = reader.GetString(3),
                        Email = reader.GetString(1),
                        Sex = reader.GetBoolean(4),
                        Phone = reader.GetString(5),
                        Birthday = reader.GetDateTime(6),
                        Wallet = reader.IsDBNull(10) ? (int?)null : reader.GetInt32(10)
                    });
                }
                conn.Close();
                return users;
            }

            public UserInfo GetOneUser(string name)
            {
                string sql = "SELECT * FROM UserInfo WHERE Name=@Name;";
                SqlConnection conn = new SqlConnection(_connStr);
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Name", name);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                var user = new UserInfo();
                if (reader.Read())
                {
                    user.UserID = reader.GetInt32(0);
                    user.Name = reader.GetString(2);
                    user.Nickname = reader.GetString(3);
                    user.Email = reader.GetString(1);
                    user.Sex = reader.GetBoolean(4);
                    user.Phone = reader.GetString(5);
                    user.Birthday = reader.GetDateTime(6);
                    user.Wallet = reader.IsDBNull(10) ? (int?)null : reader.GetInt32(10);
                }
                conn.Close();
                return user;
            }

            public List<UserInfo> AddUser(UserInfo user)
            {
                string sql = @"INSERT INTO UserInfo(Name,Nickname,Email,Sex,Phone,Birthday,Wallet)
                               VALUES(@Name,@Nickname,@Email,@Sex,@Phone,@Birthday,@Wallet)";
                SqlConnection conn = new SqlConnection(_connStr);
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Name", user.Name);
                cmd.Parameters.AddWithValue("@Nickname", user.Nickname);
                cmd.Parameters.AddWithValue("@Email", user.Email);
                cmd.Parameters.AddWithValue("@Sex", user.Sex);
                cmd.Parameters.AddWithValue("@Phone", user.Phone);
                cmd.Parameters.AddWithValue("@Birthday", user.Birthday);
                cmd.Parameters.AddWithValue("@Wallet", (object)user.Wallet ?? DBNull.Value);

                conn.Open();
                cmd.ExecuteNonQuery();
                return GetTop5Users();
            }

            public void DeleteUser(string name)
            {
                string sql = "DELETE FROM UserInfo WHERE Name=@Name;";
                SqlConnection conn = new SqlConnection(_connStr);
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Name", name);
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }

            public void UpdateUserInfo(UserInfo user)
            {
                string sql = @"UPDATE UserInfo 
                               SET Name=@Name, 
                                   Nickname=@Nickname,
                                   Email=@Email,
                                   Sex=@Sex,
                                   Phone=@Phone,
                                   Birthday=@Birthday
                               WHERE UserID=@UserID";
                SqlConnection conn = new SqlConnection(_connStr);
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@UserID", user.UserID);
                cmd.Parameters.AddWithValue("@Name", user.Name);
                cmd.Parameters.AddWithValue("@Nickname", user.Nickname);
                cmd.Parameters.AddWithValue("@Email", user.Email);
                cmd.Parameters.AddWithValue("@Sex", user.Sex);
                cmd.Parameters.AddWithValue("@Phone", user.Phone);
                cmd.Parameters.AddWithValue("@Birthday", user.Birthday);
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }
    }
}
