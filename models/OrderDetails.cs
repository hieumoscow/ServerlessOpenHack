using System;
using System.Collections.Generic;
using System.Text;

namespace ServerlessOpenHack.models
{
   public  class OrderDetails
    {
        public string ponumber { get; set; }
        public string datetime { get; set; }
        public string locationid { get; set; }
        public string locationname { get; set; }
        public string locationaddress { get; set; }
        public string locationpostcode { get; set; }

        public string totalcost { get; set; }
        public string totaltax { get; set; }


        public List<OrderItems> orderItemList { get; set; }
    }
}
