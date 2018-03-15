using QuanLyQuanCafe.DAO;
using QuanLyQuanCafe.DTO;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;

namespace QuanLyQuanCafe
{
    public partial class fTableManager : Form
    {

        private Account loginAccount;

        public Account LoginAccount
        {
            get { return loginAccount; }
            set { loginAccount = value; changeAccount(loginAccount.Type); }
        }

        public fTableManager(Account acc)
        {
            InitializeComponent();
            //nhan tai khoan dang nhap
            this.LoginAccount = acc;
            loadTable();
            loadCategory();
            loadComboboxTable(cbbSwitchTable);
        }
        #region Method
        void changeAccount(int type)
        {
            adminToolStripMenuItem.Enabled = type == 1;
            thôngTinTàiKhoảnToolStripMenuItem.Text += "[ "+LoginAccount.DisplayName+" ]";
        }

        void loadCategory()
        {
            List<Category> listCategory = CategoryDAO.Instance.getListCategory();
            cbbCategory.DataSource = listCategory;
            // chọn trường hiển thị trong cbb
            cbbCategory.DisplayMember = "Name";
        }

        void loadFoodListByCategoryId(int id)
        {
            List<Food> listFood = FoodDAO.Instance.getListFoodByCategoryId(id);
            cbbFood.DataSource = listFood;
            // chọn trường hiển thị trong cbb
            cbbFood.DisplayMember = "Name";
        }
        void loadTable()
        {
            flpnTableFood.Controls.Clear();

            List<Table> tabList = TableDAO.Instance.loadTableList();
            foreach (Table item in tabList)
            {
                Button btn = new Button() { Width = TableDAO.tableWidth, Height = TableDAO.tableHeight };
                btn.Text = item.Name + Environment.NewLine + item.Status;
                btn.Click += btn_Click;
                btn.Tag = item;
                if (item.Status.Contains("Tr"))
                {
                    btn.BackColor = Color.Azure;
                }
                else
                {
                    btn.BackColor = Color.Aqua;
                }
                flpnTableFood.Controls.Add(btn);
            }
        }
        void showBill(int id)
        {
            float totalPrice = 0;
            lsvBill.Items.Clear();
            List<DTO.Menu> listBillInfor = MenuDAO.Instance.getListMenuByIDTable(id);
            foreach (DTO.Menu item in listBillInfor)
            {
                ListViewItem listViewItem = new ListViewItem(item.FoodName.ToString());
                listViewItem.SubItems.Add(item.Count.ToString());
                listViewItem.SubItems.Add(item.Price.ToString());
                listViewItem.SubItems.Add(item.TotalPrice.ToString());
                totalPrice += item.TotalPrice;
                lsvBill.Items.Add(listViewItem);
            }
            // Định dạng kiểu tiền tệ
            CultureInfo culture = new CultureInfo("vn-VN");
            Thread.CurrentThread.CurrentCulture = culture;
            txtTotalPrice.Text = totalPrice.ToString("c");
        }
        /// <summary>
        /// Load table len combobox chuyen ban
        /// </summary>
        /// <param name="box"></param>
        void loadComboboxTable(ComboBox box)
        {
            box.DataSource = TableDAO.Instance.loadTableList();
            box.DisplayMember = "Name";
        }
        #endregion
        #region Event
        private void btn_Click(object sender, EventArgs e)
        {
            int tableID = ((sender as Button).Tag as Table).ID;
            lsvBill.Tag = ((sender) as Button).Tag;
            showBill(tableID);
        }
        private void đăngXuấtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void thôngTinCáNhânToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fAccountProfile f = new fAccountProfile(loginAccount);
            f.UpdateInfoAccount += F_UpdateInfoAccount;
            f.ShowDialog();
        }

        private void F_UpdateInfoAccount(object sender, AccountEvent e)
        {
            thôngTinTàiKhoảnToolStripMenuItem.Text = "Thông tin tài khoản [ " + e.Acc.DisplayName + " ]";
        }

        private void adminToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fAdmin admin = new fAdmin();
            admin.eventInsertFood += Admin_eventInsertFood;
            admin.eventDeleteFood += Admin_eventDeleteFood;
            admin.eventUpdateFood += Admin_eventUpdateFood;
            admin.ShowDialog();
        }

        private void Admin_eventUpdateFood(object sender, EventArgs e)
        {
            loadFoodListByCategoryId((cbbCategory.SelectedItem as Category).Id);
            if(lsvBill.Tag != null)
                showBill((lsvBill.Tag as Table).ID);
        }

        private void Admin_eventDeleteFood(object sender, EventArgs e)
        {
            loadFoodListByCategoryId((cbbCategory.SelectedItem as Category).Id);
            if (lsvBill.Tag != null)
                showBill((lsvBill.Tag as Table).ID);
            loadTable();
        }

        private void Admin_eventInsertFood(object sender, EventArgs e)
        {
            loadFoodListByCategoryId((cbbCategory.SelectedItem as Category).Id);
            if (lsvBill.Tag != null)
                showBill((lsvBill.Tag as Table).ID);
        }

        private void cbbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            int id = 0;

            ComboBox cbb = sender as ComboBox;
            if (cbb.SelectedItem == null)
                return;
            Category selected = cbb.SelectedItem as Category;
            id = selected.Id;
            loadFoodListByCategoryId(id);
        }
        private void btnAddFood_Click(object sender, EventArgs e)
        {
            Table table = lsvBill.Tag as Table;
            if (table == null)
            {
                MessageBox.ReferenceEquals("Hãy chọn bàn","Thông báo");
                return;
            }
            int idBill = BillDAO.Instance.getUnCheckBillIDByTableID(table.ID);
            int idFood = (cbbFood.SelectedItem as Food).Id;
            int count = (int)nmFoodCount.Value;
            // trường hợp không có bill nào cả thì thêm mới 1 bill
            if (idBill == -1)
            {
                BillDAO.Instance.insertBill(table.ID);
                BillInfoDAO.Instance.insertBillInfo(BillDAO.Instance.getMaxBill(), idFood, count);
            }
            // trường hợp bill đã tồn tại
            else
            {
                BillInfoDAO.Instance.insertBillInfo(idBill, idFood, count);
            }
            //load lại hóa đơn khi thêm hoặc sửa
            showBill(table.ID);
            loadTable();
        }

        private void btnCheckOut_Click(object sender, EventArgs e)
        {
            Table table = lsvBill.Tag as Table;
            int idBill = BillDAO.Instance.getUnCheckBillIDByTableID(table.ID);
            int discount = (int)nmDisCount.Value;
            double totalPrice = Convert.ToDouble(txtTotalPrice.Text.Split('₫')[1]);
            double finalTotalPrice = totalPrice - (totalPrice / 100) * discount;
            if (idBill != -1)
            {
                if (MessageBox.Show("Bạn có muốn thanh toán hóa đơn cho " + table.Name + "\nTổng tiền - (Tổng tiền / 100) x Giảm giá " + discount + " % - (" + totalPrice + " / 100) x " + totalPrice + " = " + finalTotalPrice, "Thông báo", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    BillDAO.Instance.checkOut(idBill, discount, (float)finalTotalPrice);
                    showBill(table.ID);
                    loadTable();
                }
            }
        }

        private void btnSwitchTable_Click(object sender, EventArgs e)
        {
            string t1 = (lsvBill.Tag as Table).Name;
            string t2 = (cbbSwitchTable.SelectedItem as Table).Name;
            int tb1 = (lsvBill.Tag as Table).ID;
            int tb2 = (cbbSwitchTable.SelectedItem as Table).ID;
            if (MessageBox.Show("Bạn có thật sự muốn chuyển từ bàn " + t1 + " sang " + t2 + "", "Thông báo", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                TableDAO.Instance.swicthTable(tb1, tb2);
                loadTable();
            }

        }
        #endregion

    }
}
