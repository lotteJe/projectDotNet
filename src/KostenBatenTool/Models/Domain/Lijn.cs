using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KostenBatenTool.Models.Domain
{
    public class Lijn
    {
        #region Properties
        public int LijnId { get; set; }
        public List<Veld> VeldenDefinitie { get; set; }
        public List<Veld> VeldenWaarden { get; set; } = new List<Veld>();
        #endregion

        #region Constructors

        protected Lijn()
        {
            
        }
        public Lijn(List<Veld> velden)
        {
            VeldenDefinitie = velden;
        }
        #endregion

        #region Methods

        public void VoegLijnToe()
        {
            foreach (Veld veld in VeldenDefinitie)
            {

                if (veld.Value == typeof(decimal))
                {
                    VeldenWaarden.Add(new Veld(veld.VeldKey, 0M));
                }
                else if (veld.Value == typeof(double))
                {
                    VeldenWaarden.Add(new Veld(veld.VeldKey, 0));
                }
                else if (veld.Value == typeof(Doelgroep))
                {

                    VeldenWaarden.Add(new Veld(veld.VeldKey, Doelgroep.Ander));
                }
                else
                {
                    VeldenWaarden.Add(new Veld(veld.VeldKey, "test"));
                }
            }
        }

        public void VulVeldIn(string key, Object waarde)
        {
            if (!VeldenWaarden.Any(v => v.VeldKey.Equals(key)))//als key niet bestaat exception gooien
            {
                throw new ArgumentException("Sleutel bestaat niet!");
            }
            if (waarde.GetType() == VeldenDefinitie.Find(v => v.VeldKey == key).Value) // Checken of Object van juiste dataype is
            {
                if ((VeldenDefinitie.Find(v => v.VeldKey == key).Value == typeof(decimal) && (decimal)waarde < 0) || //checken of waarde geen negatief getal is
                    (VeldenDefinitie.Find(v => v.VeldKey == key).Value == typeof(double) && (double)waarde < 0))
                {
                    throw new ArgumentException("Waarde mag niet negatief zijn");
                }
                if (key.Contains("%") && (decimal)waarde > 1)
                {
                    throw new ArgumentException("Waarde mag tussen 0 en 1 liggen.");
                }
                //Lijnen[index].Where(v => v.Key.Equals(key)).Select(v => { v.Value = waarde; return v; });
                VeldenWaarden.First(v => v.VeldKey.Equals(key)).Value = waarde;
                //BerekenBedragPerLijn(index);

            }
            else // Object is van verkeerde datatype
            {
                throw new ArgumentException($"Waarde moet {VeldenDefinitie.Find(v => v.VeldKey == key).Value.ToString()} zijn!");
            }
        }

        public void Serialiseer()
        {
            foreach (Veld veld in VeldenWaarden)
            {
                if (VeldenDefinitie.Find(v => v.VeldKey == veld.VeldKey).Value == typeof(Doelgroep) && veld.Value != null)
                {
                    veld.InternalValue = Enum.GetName(typeof(Doelgroep), veld.Value);
                }
                else
                {
                    veld.InternalValue = "" + veld.Value;
                }
            }
            foreach (Veld veld in VeldenDefinitie)
            {
                veld.InternalValue = veld.Value.ToString();
            }
        }

        public void Deserialiseer(List<Veld> velden)
        {
            VeldenDefinitie = velden;
            foreach (Veld veld in VeldenDefinitie)
            {
                veld.Value = Type.GetType(veld.InternalValue);
            }
            foreach (Veld veld in VeldenWaarden)
            {
                //type ophalen 
                if (VeldenDefinitie.Find(v => v.VeldKey == veld.VeldKey).Value == typeof(decimal))
                {
                    veld.Value = Decimal.Parse(veld.InternalValue);
                }
                else if (VeldenDefinitie.Find(v => v.VeldKey == veld.VeldKey).Value == typeof(double))
                {
                    veld.Value = Double.Parse(veld.InternalValue);
                }
                else if (VeldenDefinitie.Find(v => v.VeldKey == veld.VeldKey).Value == typeof(Doelgroep))
                {
                    if (veld.InternalValue != "")
                    {
                        veld.Value = Enum.Parse(typeof(Doelgroep), veld.InternalValue);
                    }
                    else
                    {
                        veld.Value = null;
                    }

                }
                else
                {
                    veld.Value = veld.InternalValue;
                }
            }
            
        }
        #endregion

    }
}
