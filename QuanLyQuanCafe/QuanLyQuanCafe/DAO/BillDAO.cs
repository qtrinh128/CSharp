using QuanLyQuanCafe.DTO;
using System;
using System.Data;

namespace QuanLyQuanCafe.DAO
{
    public class BillDAO
    {
        private static BillDAO instance;

        public static BillDAO Instance { get { if (instance == null) instance = new BillDAO(); return instance; } }
        private BillDAO() { }
        /// <summary>
        /// thanh cong: Bill Id
        /// that bai: -1
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int getUnCheckBillIDByTableID(int id)
        {
            DataTable data = DataProvider.Instance.ExecuteQuery("select * from Bill where idTable = " + id + " and status = 0");
            if (data.Rows.Count > 0)
            {
                Bill bill = new Bill(data.Rows[0]);
                return bill.Id;
            }
            return -1;
        }

        public void checkOut(int id, int discount, float totalPrice)
        {
            string query = "update Bill set DateCheckOut = GETDATE(), status = 1, " + "discount = " + discount + ", TotalPrice = " + totalPrice + " where id = " + id;
            DataProvider.Instance.ExecuteNonQuery(query);
        }
        public void insertBill(int id)
        {
            DataProvider.Instance.ExecuteNonQuery("exec USP_InsertBill @idTable", new object[] { id });
        }

        public DataTable getBillListByDate(DateTime checkIn, DateTime checkOut)
        {
            return DataProvider.Instance.ExecuteQuery("exec USP_GetListBillByDate @checkIn , @checkOut", new object[] { checkIn, checkOut});
        }

        public int getMaxBill()
        {
            try
            {
                return (int)DataProvider.Instance.ExecuteScalarQuery("select max(id) from Bill");
            }
            catch
            {
                return 1;
            }
        }

    }
}
