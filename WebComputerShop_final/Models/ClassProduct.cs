﻿using System;
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
    }
}