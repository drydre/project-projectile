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
        {
            get { return _speed; }
            set { _speed = value; }
        }

        public float Delta
        {
            get { return _delta * _speed; }
            set { _delta = value; }
        }

        public float DeltaNormalSpeed
        {
            get { return _delta; }
        }

        public float TotalTime
        {
            get;
            private set;
        }

        public Frames()
        {
        }

        public void SetValue(float delta, float totalTime)
        {
            _delta = delta;
            TotalTime = totalTime;
        }
    }
}
