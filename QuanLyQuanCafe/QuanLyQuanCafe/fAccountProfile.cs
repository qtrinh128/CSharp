using QuanLyQuanCafe.DAO;
using QuanLyQuanCafe.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyQuanCafe
{
    public partial class fAccountProfile : Form
    {
        private Account loginAccount;

        public Account LoginAccount
        {
            get { return loginAccount; }
            set { loginAccount = value; changeAccount( loginAccount); }
        }
        public fAccountProfile(Account acc)
        {
            InitializeComponent();
            this.loginAccount = acc;
            changeAccount(loginAccount);
        }
        void changeAccount(Account acc)
        {
            txtNameLogin.Text = acc.Username.ToString();
            txtDisplayName.Text = acc.DisplayName.ToString();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            updateAccount();
        }


        private void updateAccount()
        {
            string displayName = txtDisplayName.Text.Trim();
            string password = txtPassword.Text.Trim();
            string newPassword = txtNewPassword.Text.Trim();
            string reEnterPassword = txtReEnterPassword.Text.Trim();
            string username = txtNameLogin.Text.Trim();
            if (!newPassword.Equals(reEnterPassword))
            {
                MessageBox.Show("Vui lòng nhập lại mật khẩu","Thông báo");
            }else if (AccountDAO.Instance.updateAccount(username, displayName, password, newPassword))
            {
                MessageBox.Show("Cập nhật thành công","Thông báo");
                txtPassword.Text = String.Empty;
                txtNewPassword.Text = String.Empty;
                txtReEnterPassword.Text = String.Empty;
                if (updateInfoAccount != null)
                {
                    updateInfoAccount(this, new AccountEvent(AccountDAO.Instance.getAccountByUsername(username)));
                }
            }
            else
            {
                MessageBox.Show("Vui lòng điển đúng mật khẩu", "Thông báo");
            }
        }
        // truyen du lieu tu form con ve form cha sau khi dong form con lai

        private event EventHandler<AccountEvent> updateInfoAccount;
        public event EventHandler<AccountEvent> UpdateInfoAccount
        {
            add { updateInfoAccount += value; }
            remove { updateInfoAccount -= value; }
        }

        private void bntCancel_Click(object sender, EventArgs e)
        {

        }
    }
    public class AccountEvent : EventArgs
    {
        private Account acc;

        public Account Acc { get => acc; set => acc = value; }
        public AccountEvent(Account acc)
        {
            this.Acc = acc;
        }
    }
}
