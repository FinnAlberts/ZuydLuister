using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace ZuydLuister.Model
{
    public class ScoreCategory
    {
        [PrimaryKey, AutoIncrement]
        public int ScoreCategoryId { get; set; }
        
        public string ScoreCategoryName { get; set; }

        public string ScoreCategoryDescription { get; set; }
    }
}
