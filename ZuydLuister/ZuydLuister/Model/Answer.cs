using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace ZuydLuister.Model
{
    public class Answer
    {
        [PrimaryKey, AutoIncrement]
        public int AnswerId { get; set; }

        public string AnswerContent { get; set; }

        public int AnswerScore { get; set; }
        
        public int ScenarioId { get; set; }

        public int NextScenarioId { get; set; }
    }
}
