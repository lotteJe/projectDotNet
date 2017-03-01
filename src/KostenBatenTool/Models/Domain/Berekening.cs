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


        public Dictionary<string, Type> Velden { get; set; } = new Dictionary<string, Type>();
        public IList<Dictionary<string, Object>> Lijnen { get; set; } = new List<Dictionary<string, object>>();

        #endregion


        #region Methods


        public abstract decimal BerekenResultaat();


        public abstract decimal BerekenBedragPerLijn(int index);

       // public abstract boolean ControleerOfResultaatKanBerekendWorden(int index);

        public void VoegLijnToe(int index) //Voegt nieuwe Dictionary toe op index waarvan alle strings ingevuld zijn en elk object null is
        {
            if (index == 0 || !ControleerVorigeLijnLeeg(index))
            {
                Lijnen.Insert(index, new Dictionary<string, object>());
                foreach (KeyValuePair<string, Type> veld in Velden)
                {

                    Lijnen[index].Add(veld.Key, null);
                }
            }
            else
            {
                throw new ArgumentException("Vorige lijn is niet ingevuld");
            }
           
        }


        public decimal VulVeldIn(int index, string key, Object waarde)//geeft telkens totaalresultaat terug
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
                if ((Velden[key] == typeof(decimal) && (decimal) waarde < 0) || //checken of waarde geen negatief getal is
                    (Velden[key] == typeof(double) && (double) waarde < 0))
                {
                    throw new ArgumentException("Waarde mag niet negatief zijn");
                }
                if (key.Contains("%") && (decimal) waarde > 1)
                {
                    throw new ArgumentException("Waarde mag tussen 0 en 1 liggen.");
                }
                Lijnen[index][key] = waarde;
                //if(ControleerOfResultaatKanBerekendWorden(){
                    //BerekenResultaatPerLijn(index);

            //}
                return BerekenResultaat(); // bij voeg lijn toe op 0 zetten!!!!!!!!!!

            }
            else // Object is van verkeerde datatype
            {
                throw new ArgumentException($"Waarde moet {Velden[key].ToString()} zijn!");
            }

        }

        public void ControleerIndex(int index)
        {
            if(index < 0 || index >= Lijnen.Count)
                throw new ArgumentException("Index is ongeldig!");
        }

        private bool ControleerVorigeLijnLeeg(int index)
        {
            
            foreach (KeyValuePair<string, Object> pair in Lijnen[index-1])
            {
                if (pair.Value != null)
                {
                    return false;
                }
            }
            return true;
        }

        #endregion
    }
}

