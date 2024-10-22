﻿using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace ZuydLuister.Model
{
    public class Scenario
    {
        [PrimaryKey, AutoIncrement]
        public int ScenarioId { get; set; }

        public string ScenarioName { get; set; }

        public string ScenarioContent { get; set; }
        
        public int ScoreCategoryId { get; set; }

        public string ScenarioImage { get; set; }

        public bool IsStartScenario { get; set; }
    }
}
