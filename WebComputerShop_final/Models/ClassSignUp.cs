using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebComputerShop_final.Models
{
    public class ClassSignUp
    {
        public string userName { get; set; }
        public string passWord { get; set; }
        public string rePassWord { get; set; }
        public string address { get; set; }
        public Nullable<int> Phone { get; set; }


    }
}