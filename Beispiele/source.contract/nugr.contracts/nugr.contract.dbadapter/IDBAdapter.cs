using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using nugr.contract.domainmodel;

namespace nugr.contract.dbadapter
{
    public interface IDBAdapter
    {
        void Save(CalcPrj prj, string filename);
        CalcPrj Load(string filename);
    }
}
