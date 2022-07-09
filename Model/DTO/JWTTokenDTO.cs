﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DTO
{
    public class JWTTokenDTO
    {
        public string UserName { get; set; }
        public string MobileNo { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string CIF { get; set; }
        public string Token { get; set; }
        public Nullable<DateTime> TokenExpire { get; set; }
    }
}
