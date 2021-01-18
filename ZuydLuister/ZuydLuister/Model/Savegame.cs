using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace ZuydLuister
{
    public class Savegame
    {
        [PrimaryKey, AutoIncrement]
        public int SavegameId { get; set; }

        public string SavegameName { get; set; }

        public string SavegamePassword { get; set; } 
        
        public int ScenarioId { get; set; } 
    }
}
