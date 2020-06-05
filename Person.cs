using System;
using System.Collections.Generic;
using System.Text;

namespace MyCoronas
{
    class Person
    {

        public static Random Rnd = new Random();

        public  DateTime? DtInfiziert;

        public DateTime? DtGenesung;


        private bool infiziert;


        public bool Infiziert
        {
            get
            {
                return infiziert; 
            }
      
        }



        public void Infizieren(DateTime when)
        {
            DtInfiziert = when;
            DtGenesung= when + TimeSpan.FromDays(Rnd.Next(7, 13));
            infiziert = true;
        }



        public bool Genesen(DateTime when)
        {
            if (!infiziert)
            {
                return false;
            }
            return ( when > DtGenesung);
              

        }


    }
}
