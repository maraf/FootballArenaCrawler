﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballArenaCrawler
{
    public class Configuration
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public int TeamId { get; set; }
        public string ExportPath { get; set; }
    }
}
