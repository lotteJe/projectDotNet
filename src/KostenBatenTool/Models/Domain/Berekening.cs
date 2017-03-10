using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using KostenBatenTool.Models.Domain;

namespace KostenBatenTool.Models.Domain
{

    public abstract class Berekening
    {
        #region Properties

        public int BerekeningId { get; set; }
        public Dictionary<string, Type> Velden { get; set; } = new Dictionary<string, Type>();
        public IList<Dictionary<string, Object>> Lijnen { get; set; } = new List<Dictionary<string, object>>();

        #endregion


        #region Methods


        public abstract decimal BerekenResultaat();


        public abstract decimal BerekenBedragPerLijn(int index);

        public void VoegLijnToe(int index) //Voegt nieuwe Dictionary toe op index waarvan alle keys ingevuld zijn en elke string null is, elke double en decimal zijn 0
        {
                Lijnen.Insert(index, new Dictionary<string, object>());
                foreach (KeyValuePair<string, Type> veld in Velden)
                {

                    if (veld.Value == typeof(decimal))
                    {
                        Lijnen[index].Add(veld.Key, 0M);
                    }
                    else if (veld.Value == typeof(double))
                    {
                        Lijnen[index].Add(veld.Key, 0);
                    }
                    else
                    {

                        Lijnen[index].Add(veld.Key, null);
                    }
                }
           
        }


        public void VulVeldIn(int index, string key, Object waarde)
        {

            if (index == Lijnen.Count) //Als Lijn nog niet bestaat, ze toevoegen
            {
                VoegLijnToe(index);

            }
            if (index < 0 || index > Lijnen.Count)
            {
                throw new ArgumentException("Index is ongeldig!");
            }

            if (!Lijnen[index].ContainsKey(key))//als key niet bestaat exception gooien
            {
                throw new ArgumentException("Sleutel bestaat niet!");
            }
            if (waarde.GetType() == Velden[key]) // Checken of Object van juiste dataype is
            {
                if ((Velden[key] == typeof(decimal) && (decimal)waarde < 0) || //checken of waarde geen negatief getal is
                    (Velden[key] == typeof(double) && (double)waarde < 0))
                {
                    throw new ArgumentException("Waarde mag niet negatief zijn");
                }
                if (key.Contains("%") && (decimal)waarde > 1)
                {
                    throw new ArgumentException("Waarde mag tussen 0 en 1 liggen.");
                }
                Lijnen[index][key] = waarde;
                //BerekenBedragPerLijn(index);


            }
            else // Object is van verkeerde datatype
            {
                throw new ArgumentException($"Waarde moet {Velden[key].ToString()} zijn!");
            }

        }

        public void ControleerIndex(int index)
        {
            if (index < 0 || index >= Lijnen.Count)
                throw new ArgumentException("Index is ongeldig!");
        }
        
        #endregion
    }
}

