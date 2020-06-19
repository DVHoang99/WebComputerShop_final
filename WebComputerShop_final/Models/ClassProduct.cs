using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebComputerShop_final.Models
{
    public class ClassProduct
    {
        public int id { get; set; }
        public string Name { get; set; }
        public Nullable<int> price { get; set; }
        public string description { get; set; }
        public string tilte { get; set; }

        public int idProductType { get; set; }


        public int idProduct { get; set; }
        public string ImgPath { get; set; }

        public HttpPostedFileBase ImageFile { get; set; }
    }
}