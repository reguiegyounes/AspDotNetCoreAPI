﻿using System;
using System.Collections.Generic;

#nullable disable

namespace projectApi.Models
{
    public partial class TblUser
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
