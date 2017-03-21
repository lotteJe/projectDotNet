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

        public static int Teller = 0;
        public int BerekeningId { get; set; }
        public Dictionary<string, Type> Velden { get; set; } = new Dictionary<string, Type>();
        public List<List<Veld>> Lijnen { get; set; } = new List<List<Veld>>();

        #endregion


        #region Methods

        protected Berekening()
        {
            BerekeningId = System.Threading.Interlocked.Increment(ref Teller);
        }


        public abstract decimal BerekenResultaat();


        public abstract decimal BerekenBedragPerLijn(int index);

        public void VoegLijnToe(int index) //Voegt nieuwe List toe op index waarvan alle keys ingevuld zijn en elke string null is, elke double en decimal zijn 0
        {
                Lijnen.Insert(index, new List<Veld>());
                foreach (KeyValuePair<string, Type> veld in Velden)
                {

                    if (veld.Value == typeof(decimal))
                    {
                        Lijnen[index].Add(new Veld(veld.Key, 0M));
                    }
                    else if (veld.Value == typeof(double))
                    {
                        Lijnen[index].Add(new Veld(veld.Key, 0));
                    }
                    else
                    {

                        Lijnen[index].Add(new Veld(veld.Key, null));
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

            if (!Lijnen[index].Any(v => v.Key.Equals(key)))//als key niet bestaat exception gooien
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
                //Lijnen[index].Where(v => v.Key.Equals(key)).Select(v => { v.Value = waarde; return v; });
                Lijnen[index].First(v => v.Key.Equals(key)).Value = waarde;
                //BerekenBedragPerLijn(index);

            }
            else // Object is van verkeerde datatype
            {
                throw new ArgumentException($"Waarde moet {Velden[key].ToString()} zijn!");
            }
        }

        public void Serialiseer()//oproepen in Repository
        {
            //omzetten van Value naar string
            foreach (List<Veld> lijn in Lijnen)
            {
                foreach (Veld veld in lijn)
                {
                    veld.MapInternalValue();
                }
            }
        }

        public void Deserialiseer()
        {
            //omzetten naar correct type, checken bij Velden
            foreach(List<Veld> lijn in Lijnen)
            {
                foreach (Veld veld in lijn)
                {
                    //type ophalen 
                    if (Velden[veld.Key] == typeof(decimal))
                    {
                        veld.Value = Decimal.Parse(veld.InternalValue);
                    } else if (Velden[veld.Key] == typeof(double))
                    {
                        veld.Value = Double.Parse(veld.InternalValue);
                    } else if (Velden[veld.Key] == typeof(Doelgroep))
                    {
                        veld.Value = Enum.Parse(typeof(Doelgroep), veld.InternalValue);
                    }
                    else
                    {
                        veld.Value = veld.InternalValue;
                    }
                    
                }
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

