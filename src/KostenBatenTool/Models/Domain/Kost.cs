using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using KostenBatenTool.Models.Domain;

namespace KostenBatenTool.Models.Domain
{
   
    public abstract class Kost
    {
        #region Properties

       
        public Dictionary<string, Type> Velden { get; set; } = new Dictionary<string, Type>();
        public IList<Dictionary<string,Object>> Lijnen { get; set; } = new List<Dictionary<string, object>>();
        
        #endregion

        
        #region Methods


        public abstract decimal BerekenResultaat();


        public abstract decimal BerekenKostPerLijn(int index);

       
        public void VulVeldIn(int index, string key, Object waarde)
        {
            if (waarde.GetType() == Velden[key]) // Checken of Object van juiste dataype is
            {
                if (Lijnen.ElementAt(index) != null) //Checken of lijn al bestaat
                {
                    if (Lijnen[index][key] != null) // Checken of Veld al bestaat
                    {
                        Lijnen[index][key] = waarde;
                    }
                    else // veld bestaat nog niet
                    {
                        Lijnen[index].Add(key, waarde);
                    }
                }
                else // lijn bestaat nog niet
                {
                    Dictionary<string, Object> lijn = new Dictionary<string, object>();
                    lijn.Add(key, waarde);
                    Lijnen.Insert(index, lijn);
                }

            }
            else // Object is van verkeerde datatype
            {
                throw new System.ArgumentException("Waarde moet {0} zijn", Velden[key].ToString());
            }

        }

        #endregion
    }
}

