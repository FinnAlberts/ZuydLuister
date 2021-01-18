using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace ZuydLuister
{
    public class ScoreCategory
    {
        [PrimaryKey, AutoIncrement]
        public int ScoreCategoryId { get; set; }
        
        public string ScoreCategoryName { get; set; }
    }
}
