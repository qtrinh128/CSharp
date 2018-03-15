using QuanLyQuanCafe.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DAO
{
    public class AccountDAO
    {
        private static AccountDAO instance;
        public static AccountDAO Instance
        {
            get
            {
                if (instance == null) instance = new AccountDAO(); return instance;
            }
        }
        private AccountDAO() { }
        public bool checkLogin(string userName, string passWord)
        {
            string query = "exec USP_Login @userName , @passWord";
            DataTable result = DataProvider.Instance.ExecuteQuery(query, new object[] { userName, passWord});
            return result.Rows.Count > 0;
        }
        public Account getAccountByUsername(string username)
        {
            DataTable data = DataProvider.Instance.ExecuteQuery("select * from Account where UserName = '"+username+"'");
            foreach (DataRow item in data.Rows)
            {
                return new Account(item);
            }
            return null;
        }
        public bool updateAccount(string username, string displayName, string pass, string newPass)
        {
            int result = DataProvider.Instance.ExecuteNonQuery("exec USP_UpdateAccount @username , @displayName , @password , @newPassword", new object[] { username, displayName, pass, newPass});

            return result > 0;
        }
        public DataTable getListAccount()
        {
            return DataProvider.Instance.ExecuteQuery("select Username as [Tên tài khoản], DisplayName as[Tên hiển thị], type as [Loại tài khoản] from Account");
        }
        /// <summary>
        /// thêm tài khoản
        /// </summary>
        /// <param name="name"></param>
        /// <param name="displayName"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool insertAccount(string name, string displayName, int type)
        {
            string query = String.Format("insert Account(UserName, DisplayName, type)values(N'{0}',N'{1}',N'{2}')", name, displayName, type);
            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }
        /// <summary>
        /// Cập nhật tài khoản
        /// </summary>
        /// <param name="name"></param>
        /// <param name="displayName"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool updateAccount(string name, string displayName, int type)
        {
            string query = String.Format("update Account set DisplayName = N'{1}', type = {2} where UserName = N'{0}'", name, displayName, type);
            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }
        /// <summary>
        /// xóa tài khoản
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool deleteAccount(string name)
        {
            string query = String.Format("delete Account where UserName = N'{0}'", name);
            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }
        /// <summary>
        /// cập nhật mật khẩu cho tài khoản
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool resetPassWord(string name)
        {
            string query = String.Format("update Account  set PassWord = N'0' where UserName = N'{0}'", name);
            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;

        }
    }
}
