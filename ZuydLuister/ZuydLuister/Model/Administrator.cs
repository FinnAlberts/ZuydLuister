using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace ZuydLuister.Model
{
    public class Administrator
    {
        [PrimaryKey, AutoIncrement]
        public int AdminId { get; set; }

        public string AdminEmail { get; set; }

        public bool IsMasterAdmin { get; set; }
    }
}
