using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using nugr.contract.domainmodel;

namespace nugr.contract.application
{
    public interface IApplication
    {
        CalcPrj Calculate(long from, long to);

        void Save(CalcPrj prj, string filename);
        CalcPrj Load(string filename);
    }
}
