using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace The_Coursach.Classes
{
    public class Orders
    {
        public int Id { get; set; }
        public string OrderName { get; set; }
        public int PersonId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public double Total { get; set; }
        public double Paid { get; set; }
        public double Change { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
    }
}
