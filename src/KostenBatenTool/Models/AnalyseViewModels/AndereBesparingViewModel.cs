﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KostenBatenTool.Models.Domain;

namespace KostenBatenTool.Models.AnalyseViewModels
{
    public class AndereBesparingViewModel
    {
        public AndereBesparingViewModel()
        {
            
        }
        public string Type { get; set; }
        public decimal Bedrag { get; set; }

    }
}