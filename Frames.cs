using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projectile
{
    /** Klasa obsługująca parametry czasowe ramki.*/
    public class Frames
    {
        private float _delta = 0f;
        private float _speed = 1f;

        /** pole skalowania upływu czasu*/
        public float Speed
        {   
            get { return _speed; }
            set { _speed = value; }
        }

        /** pole upływu czasu pomiędzy klatkami*/
        public float Delta
        {   
            get { return _delta * _speed; }
            set { _delta = value; }
        }

        /** pole nieskalowanego upływu czasu pomiędzy klatkami*/
        public float DeltaUnscaled
        {   
            get { return _delta; }
        }

        /** pole czasu od uruchomienia programu*/
        public float TotalTime
        {   
            get;
            private set;
        }
        
        /** Konstruktor domyślny*/
        public Frames()
        {
        }

        /**
         * metoda pozwalająca odświeżyć deltę i całkowity upływ czasu
         * @param delta czas między klatkami
         * @param całkowity czas
         */
        public void SetValue(float delta, float totalTime)
        {   
            _delta = delta;
            TotalTime = totalTime;
        }
    }
}
