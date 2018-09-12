using System;
using System.Collections.Generic;
using System.Text;

namespace ServerlessOpenHack.models
{
    public class OrderItems
    {
        public string ponumber { get; set; }
        public string productid { get; set; }
        public string quantity { get; set; }
        public string unitcost { get; set; }
        public string totalcost { get; set; }
        public string totaltax { get; set; }
        public string productname { get; set; }
        public string productdescription { get; set; }
    }
}
