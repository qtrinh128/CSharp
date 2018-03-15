using QuanLyQuanCafe.DTO;
using System.Collections.Generic;
using System.Data;


namespace QuanLyQuanCafe.DAO
{
    public class BillInfoDAO
    {
        private static BillInfoDAO instance;

        public static BillInfoDAO Instance { get { if (instance == null) instance = new BillInfoDAO(); return instance; } }
        private BillInfoDAO() { }
        public List<BillInfor> getListBillInfor(int id)
        {
            List<BillInfor> listBillInfor = new List<BillInfor>();

            DataTable data = DataProvider.Instance.ExecuteQuery("select * from BillInfo where idBill = "+id);
            foreach (DataRow item in data.Rows)
            {
                BillInfor infor = new BillInfor(item);
                listBillInfor.Add(infor);
            }
            return listBillInfor;
        }
        public void insertBillInfo(int idBill, int idFood, int count)
        {
            //định mệnh nhớ cách tham số kia ra
            DataProvider.Instance.ExecuteNonQuery("exec USP_InsertBillInfo @idBill , @idFood , @count", new object[] { idBill, idFood, count });
        }
        public void deleteBillInfoByFoodId(int id)
        {
            DataProvider.Instance.ExecuteQuery("delete BillInfo where idFood = "+id);
        }
    }
}
