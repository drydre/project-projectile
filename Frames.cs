using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projectile
{
    public class Frames
    {
        private float _delta = 0f;
        private float _speed = 1f;

        public float Speed
        {   // pole skalowania upływu czasu
            get { return _speed; }
            set { _speed = value; }
        }

        public float Delta
        {   // pole upływu czasu pomiędzy klatkami
            get { return _delta * _speed; }
            set { _delta = value; }
        }

        public float DeltaUnscaled
        {   // pole nieskalowanego upływu czasu pomiędzy klatkami
            get { return _delta; }
        }

        public float TotalTime
        {   // pole czasu od uruchomienia programu
            get;
            private set;
        }

        public Frames()
        {
        }

        public void SetValue(float delta, float totalTime)
        {   // metoda pozwalająca odświeżyć deltę i całkowity upływ czasu
            _delta = delta;
            TotalTime = totalTime;
        }
    }
}
