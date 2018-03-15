using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DTO
{
    public class Bill
    {
        public Bill(int id, DateTime? checkIn, DateTime? checkOut, int status, int dis)
        {
            this.Id = id;
            this.DateCheckIn = checkIn;
            this.DateCheckOut = checkOut;
            this.Status = status;
            this.DisCount = dis;
        }
        public Bill(DataRow row)
        {
            this.Id = (int)row["id"];
            this.DateCheckIn = (DateTime?)row["dateCheckIn"];
            var dateCheckOutTemp = row["dateCheckOut"];
            if (dateCheckOutTemp.ToString() != "")
            {
                this.DateCheckOut = (DateTime?)dateCheckOutTemp;
            }
            this.Status = (int)row["status"];
            if (row["discount"].ToString() != "")
            {
                this.DisCount = (int)row["discount"];
            }
        }
        private int disCount;
        private int status;
        private DateTime? dateCheckOut;
        private DateTime? dateCheckIn;
        private int id;

        public int Id { get => id; set => id = value; }
        public DateTime? DateCheckIn { get => dateCheckIn; set => dateCheckIn = value; }
        public DateTime? DateCheckOut { get => dateCheckOut; set => dateCheckOut = value; }
        public int Status { get => status; set => status = value; }
        public int DisCount { get => disCount; set => disCount = value; }
    }
}
