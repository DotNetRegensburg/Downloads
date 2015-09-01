using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.IO;
using nugr.contract.dbadapter;
using nugr.contract.domainmodel;

namespace Nug.DataAdapter
{
    public class DataAdapter : IDBAdapter
    {
        #region IDBAdapter Member

        public CalcPrj Load(string filename)
        {
            CalcPrj deserializeObject = null;
            using (FileStream stream = new FileStream(filename, FileMode.Open))
            {
                BinaryFormatter binForm = new BinaryFormatter();

                deserializeObject = (CalcPrj) binForm.Deserialize(stream);
                
            }
            
            return deserializeObject;
            
        }

        public void Save(CalcPrj prj, string filename)
        {

            using (FileStream stream = new FileStream(filename, FileMode.OpenOrCreate))
            {
                BinaryFormatter binForm = new BinaryFormatter();

                binForm.Serialize(stream, prj);
            }
            
        }

        #endregion
    }
}
