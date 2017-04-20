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
        public List<Veld> Velden { get; set; } = new List<Veld>();
        public List<Lijn> Lijnen { get; set; } = new List<Lijn>();
        public decimal Resultaat { get; set; } = 0M;
        #endregion


        #region Methods

        protected Berekening()
        {

        }
        public abstract decimal BerekenResultaat();


        public abstract decimal BerekenBedragPerLijn(int index);

        public void VoegLijnToe(int index) //Voegt nieuwe List toe op index waarvan alle keys ingevuld zijn en elke string null is, elke double en decimal zijn 0
        {
            Lijnen.Insert(index, new Lijn(Velden));
            Lijnen[index].VoegLijnToe();
                
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

            Lijnen[index].VulVeldIn(key, waarde);
            
        }

        public void Serialiseer()//oproepen in Repository
        {
            //omzetten van Value naar string in Lijnen
            Lijnen.ForEach(l => l.Serialiseer());

        }

        public void Deserialiseer()
        {
            //omzetten naar correct type, checken bij Velden
            Lijnen.ForEach(l => l.Deserialiseer(Velden));
        }

        public void ControleerIndex(int index)
        {
            if (index < 0 || index >= Lijnen.Count)
                throw new ArgumentException("Index is ongeldig!");
        }
        
        #endregion
    }
}

