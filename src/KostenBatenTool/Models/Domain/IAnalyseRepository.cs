﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KostenBatenTool.Models.Domain
{
    public interface IAnalyseRepository
    {
        void Add(Analyse analyse);
        Analyse GetAnalyse(int analyseId);
        void SaveChanges();
    }
}
