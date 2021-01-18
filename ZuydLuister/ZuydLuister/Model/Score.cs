using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace ZuydLuister
{
    public class Score
    {
        [PrimaryKey, AutoIncrement]
        public int ScoreId { get; set; }
        
        public int ScoreCategoryId { get; set; }
        
        public int SavegameId { get; set; }

        public int AchievedScore { get; set; }

        public int MaxScore { get; set; }
    }
}
