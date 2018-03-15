using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DTO
{
    public class Menu
    {
        public Menu(string name, int count, float pr, float ttpr = 0)
        {
            this.foodName = name;
            this.Count = count;
            this.Price = pr;
            this.TotalPrice = ttpr;
        }
        public Menu(DataRow row)
        {
            this.foodName = row["name"].ToString();
            this.Count = (int)row["count"];
            this.Price = (float)Convert.ToDouble( row["price"]);
            this.TotalPrice = (float)Convert.ToDouble(row["totalPrice"]);
        }
        private float totalPrice;
        private float price;
        private int count;
        private string foodName;

        public string FoodName { get => foodName; set => foodName = value; }
        public int Count { get => count; set => count = value; }
        public float Price { get => price; set => price = value; }
        public float TotalPrice { get => totalPrice; set => totalPrice = value; }
    }
}
