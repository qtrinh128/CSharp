
using System.Data;

namespace QuanLyQuanCafe.DTO
{
    public class Table
    {
        public Table(int id, string name, string staus)
        {
            this.ID = id;
            this.Name = name;
            this.Status = status;
        }
        public Table(DataRow row)
        {
            this.ID = (int)row["id"];
            this.Name = row["name"].ToString();
            this.Status = row["status"].ToString();
        }

        private string name;
        private int iD;
        private string status;

        public string Name { get => name; set => name = value; }
        public int ID { get => iD; set => iD = value; }
        public string Status { get => status; set => status = value; }
    }
}
