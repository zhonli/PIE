﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PIEM.API.Models
{
    public class ResultModel
    {
        //200: success  300: Fail
        public int Status { get; set; }

        public string Message { get; set; }

        public object Result { get; set; }

    }
}