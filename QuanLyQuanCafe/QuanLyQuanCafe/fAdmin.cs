using QuanLyQuanCafe.DAO;
using QuanLyQuanCafe.DTO;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace QuanLyQuanCafe
{
    public partial class fAdmin : Form
    {
        BindingSource foodList = new BindingSource();
        BindingSource accountList = new BindingSource();
        public fAdmin()
        {
            InitializeComponent();
            load();
        }
        #region Method
        List<Food> searchFoodByName(string name)
        {
            List<Food> listFood = FoodDAO.Instance.searchFoodByName(name );
            return listFood;
        }
        void load()
        {
            dtgvFood.DataSource = foodList;
            dtgvAccount.DataSource = accountList;
            loadDateTimePickBill();
            loadListBillByDate(dtpkFromDate.Value, dtpkToDay.Value);
            loadListFood();
            loadAccount();
            addFoodBinding();
            loadCategoryIntoCombobox(cbbCategoryFood);
            addAccountBinding();
        }
        void addAccountBinding()
        {
            txtNameAccount.DataBindings.Add("Text", dtgvAccount.DataSource, "Tên tài khoản", true, DataSourceUpdateMode.Never);
            txtDisplayAccount.DataBindings.Add("Text", dtgvAccount.DataSource, "Tên hiển thị", true, DataSourceUpdateMode.Never);
            nmTypeAccount.DataBindings.Add("value", dtgvAccount.DataSource, "Loại tài khoản", true, DataSourceUpdateMode.Never);
        }
        void loadAccount()
        {
            accountList.DataSource = AccountDAO.Instance.getListAccount();
        }
        void loadDateTimePickBill()
        {
            DateTime dt = DateTime.Now;
            dtpkFromDate.Value = new DateTime(dt.Year, dt.Month, 1);
            dtpkToDay.Value = dtpkFromDate.Value.AddMonths(1).AddDays(-1);
        }
        public void loadListBillByDate(DateTime checkIn, DateTime checkOut)
        {
            dtgvBill.DataSource =  BillDAO.Instance.getBillListByDate(checkIn, checkOut);

        }
        /// <summary>
        /// Binidng khi chọn vào 1 item trong dtgvFood
        /// </summary>
        void addFoodBinding()
        {
            txtNameFood.DataBindings.Add(new Binding("Text",dtgvFood.DataSource,"Name", true, DataSourceUpdateMode.Never));
            txtIDFood.DataBindings.Add(new Binding("Text",dtgvFood.DataSource,"ID", true, DataSourceUpdateMode.Never));
            nmPriceFood.DataBindings.Add(new Binding("value", dtgvFood.DataSource, "Price", true, DataSourceUpdateMode.Never));
        }
        /// <summary>
        /// Hiển thị danh danh category thức ăn lên Combobox
        /// </summary>
        void loadListFood()
        {
            foodList.DataSource = FoodDAO.Instance.getListFood();
        }
        /// <summary>
        /// hiển thị danh mục thức ăn lên combobox
        /// </summary>
        /// <param name="cb"></param>
        void loadCategoryIntoCombobox(ComboBox cb)
        {
            cb.DataSource = CategoryDAO.Instance.getListCategory();
            cb.DisplayMember = "Name";
        }
        void addAcount(string username, string displayname, int type)
        {
            if(AccountDAO.Instance.insertAccount(username, displayname, type))
            {
                MessageBox.Show("Thêm tài khoản thành công","Thông báo");
            }
            else
            {
                MessageBox.Show("Có lỗi xảy ra, vui lòng thử lại", "Thông báo thêm tài khoản");
            }
            loadAccount();
        }
        void editAccount(string username, string displayname, int type)
        {
            if (AccountDAO.Instance.insertAccount(username, displayname, type))
            {
                MessageBox.Show("Sửa tài khoản thành công", "Thông báo");
            }
            else
            {
                MessageBox.Show("Có lỗi xảy ra, vui lòng thử lại", "Thông báo sửa tài khoản");
            }
            loadAccount();
        }
        void deleteAccount(string username)
        {
            if (AccountDAO.Instance.deleteAccount(username))
            {
                MessageBox.Show("Xóa tài khoản thành công", "Thông báo");
            }
            else
            {
                MessageBox.Show("Có lỗi xảy ra, vui lòng thử lại", "Thông báo xóa tài khoản");
            }
            loadAccount();
        }
        void ResetPassWord(string name)
        {
            if (AccountDAO.Instance.resetPassWord(name))
            {
                MessageBox.Show("Đặt lại mật khẩu thành công", "Thông báo");
            }
            else
            {
                MessageBox.Show("Có lỗi xảy ra, vui lòng thử lại", "Thông báo");
            }
        }
        #endregion
        #region Event
        private void btnSreachFood_Click(object sender, EventArgs e)
        {
           foodList.DataSource =  searchFoodByName(txtSreachFood.Text.Trim());
        }
        private void btnViewBill_Click(object sender, EventArgs e)
        {
            loadListBillByDate(dtpkFromDate.Value, dtpkToDay.Value);
        }

        private void btnViewFood_Click(object sender, EventArgs e)
        {
            loadListFood();
        }
        private void txtIDFood_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (dtgvFood.SelectedCells.Count > 0)
                {
                    int id = (int)dtgvFood.SelectedCells[0].OwningRow.Cells["IDCategory"].Value;
                    Category category = CategoryDAO.Instance.getCategoryByID(id);
                    cbbCategoryFood.SelectedItem = category;

                    int index = -1;
                    int i = 0;
                    foreach (Category item in cbbCategoryFood.Items)
                    {
                        if (item.Id == category.Id)
                        {
                            index = i;
                            break;
                        }
                        i++;
                    }
                    cbbCategoryFood.SelectedIndex = index;
                }
            }
            catch
            {

            }
        }

        private void btnAddFood_Click(object sender, EventArgs e)
        {
            string name = txtNameFood.Text;
            int category = (cbbCategoryFood.SelectedItem as Category).Id;
            float price = (float)nmPriceFood.Value;
            if (FoodDAO.Instance.insertFood(name, category, price))
            {
                MessageBox.Show("Thêm món thành công", "Thông báo");
                loadListFood();
                if (eventinsertFood != null)
                {
                    eventinsertFood(this, new EventArgs());
                }
            }
            else
            {
                MessageBox.Show("Có lỗi xảy ra vui lòng thử lại", "Thông báo");
            }
        }

        private void btnDeleteFood_Click(object sender, EventArgs e)
        {
            int idFood = Convert.ToInt32(txtIDFood.Text);
            if (FoodDAO.Instance.deleteFood(idFood))
            {
                MessageBox.Show("Xóa món thành công", "Thông báo");
                loadListFood();
                if (eventdeleteFood != null)
                {
                    eventdeleteFood(this, new EventArgs());
                }
            }
            else
            {
                MessageBox.Show("Có lỗi xảy ra vui lòng thử lại", "Thông báo");
            }
        }

        private void btnEditFood_Click(object sender, EventArgs e)
        {
            string name = txtNameFood.Text;
            int category = (cbbCategoryFood.SelectedItem as Category).Id;
            float price = (float)nmPriceFood.Value;
            int idFood = Convert.ToInt32(txtIDFood.Text);
            if (FoodDAO.Instance.updateFood(name, category, price, idFood))
            {
                MessageBox.Show("Sửa món thành công", "Thông báo");
                loadListFood();
                if (eventupdateFood != null)
                {
                    eventupdateFood(this, new EventArgs());
                }
            }
            else
            {
                MessageBox.Show("Có lỗi xảy ra vui lòng thử lại", "Thông báo");
            }
        }
        private event EventHandler eventinsertFood;
        public event EventHandler eventInsertFood
        {
            add { eventinsertFood += value; }
            remove { eventinsertFood -= value; }
        }
        private event EventHandler eventupdateFood;
        public event EventHandler eventUpdateFood
        {
            add { eventupdateFood += value; }
            remove { eventupdateFood -= value; }
        }
        private event EventHandler eventdeleteFood;
        public event EventHandler eventDeleteFood
        {
            add { eventdeleteFood += value; }
            remove { eventdeleteFood -= value; }
        }

        private void btnViewAccount_Click(object sender, EventArgs e)
        {
            loadAccount();
        }

        private void btnAddAccount_Click(object sender, EventArgs e)
        {
            addAcount(txtNameAccount.Text, txtDisplayAccount.Text, (int)nmTypeAccount.Value);
        }
        private void btnDeleteAccount_Click(object sender, EventArgs e)
        {
            deleteAccount(txtNameAccount.Text);
        }
        private void btnEditAccount_Click(object sender, EventArgs e)
        {
            // tên tài khoản không được phép trùng với tên cũ
            editAccount(txtNameAccount.Text, txtDisplayAccount.Text, (int)nmTypeAccount.Value);
        }
        private void btnResetPassword_Click(object sender, EventArgs e)
        {
            ResetPassWord(txtNameAccount.Text);
        }
        #endregion

    }
}
