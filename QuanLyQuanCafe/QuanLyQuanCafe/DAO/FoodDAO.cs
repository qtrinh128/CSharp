using QuanLyQuanCafe.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DAO
{
    public class FoodDAO
    {
        private static FoodDAO instance;
        private FoodDAO() { }

        public static FoodDAO Instance { get { if (instance == null) instance = new FoodDAO(); return instance; } }

        public List<Food> getListFoodByCategoryId(int id)
        {
            List<Food> list = new List<Food>();
            string query = "select * from Food where idCategory = " + id;
            DataTable data = DataProvider.Instance.ExecuteQuery(query);

            foreach (DataRow item in data.Rows)
            {
                Food food = new Food(item);
                list.Add(food);
            }
            return list;
        }
        public List<Food> getListFood()
        {
            List<Food> list = new List<Food>();
            string query = "select * from Food";
            DataTable data = DataProvider.Instance.ExecuteQuery(query);

            foreach (DataRow item in data.Rows)
            {
                Food food = new Food(item);
                list.Add(food);
            }
            return list;
        }
        public bool insertFood(string name, int id, float price)
        {
            string query = String.Format("insert Food(name, idCategory, price)values(N'{0}',{1},{2})",name, id, price);
            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }
        public bool updateFood(string name, int id, float price, int idFood)
        {
            string query = String.Format("update Food set name = N'{0}', idCategory = {1}, price = {2} where id = {3}", name, id, price, idFood);
            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }
        public bool deleteFood(int idFood)
        {
            BillInfoDAO.Instance.deleteBillInfoByFoodId(idFood);

            string query = String.Format("delete Food where id = {0}", idFood);
            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }
        public List<Food> searchFoodByName(string name)
        {
            List<Food> list = new List<Food>();
            string query = String.Format("select * from Food where [dbo].[fuConvertToUnsign1](name) like N'%' + [dbo].[fuConvertToUnsign1](N'{0}') + '%'", name);
            DataTable data = DataProvider.Instance.ExecuteQuery(query);
            foreach (DataRow item in data.Rows)
            {
                Food food = new Food(item);
                list.Add(food);
            }
            return list;
        }
    }
}
