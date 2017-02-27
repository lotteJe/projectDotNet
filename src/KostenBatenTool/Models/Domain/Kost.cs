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
        public IList<Dictionary<string, Object>> Lijnen { get; set; } = new List<Dictionary<string, object>>();

        #endregion


        #region Methods


        public abstract decimal BerekenResultaat();


        public abstract decimal BerekenKostPerLijn(int index);

        public void VoegLijnToe(int index) //Voegt nieuwe Dictionary toe op index waarvan alle strings ingevuld zijn en elk object null is
        {
            Lijnen.Insert(index, new Dictionary<string, object>());
            foreach (KeyValuePair<string, Type> veld in Velden)
            {

                Lijnen[index].Add(veld.Key, null);
            }
        }


        public void VulVeldIn(int index, string key, Object waarde)
        {
            if (index == Lijnen.Count) //Als Lijn nog niet bestaat, ze toevoegen
            {
                VoegLijnToe(index);

            }
            else if (index > Lijnen.Count || index < 0)//als index ongeldig is exception gooien
            {
                throw new ArgumentException("Index is ongeldig!");

            }
            if (!Lijnen[index].ContainsKey(key))//als key niet bestaat exception gooien
            {
                throw new ArgumentException("Sleutel bestaat niet!");
            }
            if (waarde.GetType() == Velden[key]) // Checken of Object van juiste dataype is
            {
                if ((Velden[key] == typeof(decimal) && (decimal) waarde < 0) ||
                    (Velden[key] == typeof(double) && (double) waarde < 0))
                {
                    throw new ArgumentException("Waarde mag niet negatief zijn");
                }
                else
                {
                    Lijnen[index][key] = waarde;
                }
                
            }
            else // Object is van verkeerde datatype
            {
                throw new ArgumentException("Waarde moet {0} zijn!", Velden[key].ToString());
            }

        }

        #endregion
    }
}

