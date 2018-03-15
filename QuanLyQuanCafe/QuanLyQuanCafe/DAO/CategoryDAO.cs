using QuanLyQuanCafe.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DAO
{
    class CategoryDAO
    {
        private static CategoryDAO instance;
        private CategoryDAO() { }

        internal static CategoryDAO Instance
        {
            get { if (instance == null) instance = new CategoryDAO(); return instance; }
        }
        public List<Category> getListCategory()
        {
            List<Category> list = new List<Category>();
            string query = "select * from FoodCategory";
            DataTable data = DataProvider.Instance.ExecuteQuery(query);
            foreach (DataRow item in data.Rows)
            {
                Category category = new Category(item);
                list.Add(category);
            }
            return list;
        }
        public Category getCategoryByID(int id)
        {
            Category category = null;
            string query = "select * from FoodCategory where id = "+id;
            DataTable data = DataProvider.Instance.ExecuteQuery(query);
            foreach (DataRow item in data.Rows)
            {
                category = new Category(item);
                return category;
            }
            return category;
        }
    }
}
