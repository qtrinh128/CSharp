using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DTO
{
    public class Account
    {

        public Account(string username, string displayname, int type, string password = null)
        {
            this.Username = username;
            this.DisplayName = displayname;
            this.type = type;
            this.Password = password;
        }
        public Account(DataRow row)
        {
            this.Username = row["UserName"].ToString();
            this.DisplayName = row["DisplayName"].ToString();
            this.type = (int)row["type"];
            this.Password = row["PassWord"].ToString();
        }
        private string username;
        private string displayName;
        private string password;
        private int type;

        public string Username { get => username; set => username = value; }
        public string DisplayName { get => displayName; set => displayName = value; }
        public string Password { get => password; set => password = value; }
        public int Type { get => type; set => type = value; }
    }
}
