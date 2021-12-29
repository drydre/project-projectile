using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;
using SFML.Audio;
using SFML.Graphics;
using SFML.Window;

namespace Projectile
{
    /** 
     * Klasa przechowująca metody symulujące fizykę w grze
     * w oparciu o przyjęte wcześniej założenia.
     */
    public class Physics
    {   
        /** zegar inicjowany po wystrzale*/
        public Clock clock;
        /** składowa pozioma prędkosci początkowej*/
        double V0x;
        /** składowa pionowa prędkości początkowej*/
        double V0y;
        /** kąt nachylenia strzału względem poziomu*/
        float alpha;
        /** początkowa energia kinetyczna wystrzelonego pocisku */
        float E;
        /** masa pocisku*/
        float m;
        /** przyspieszenie grawitacyjne*/
        float g;
        /** koordynaty początkowe pocisku*/
        Vector2f startCoords;

        /**
         * metoda inicjalizacji strzału - przekazanie obiektowi wszystkich potrzebnych
         * danych do wykonania obliczeń
         * @param alpha kąt nachylenia strzału względem poziomu
         * @param E początkowa energia kinetyczna wystrzelonego pocisku
         * @param m masa pocisku
         * @param startCoords koordynaty początkowe pocisku
         * @param g przyspieszenie grawitacyjne
         */
        public void InitShot(float alpha, float E, float m, Vector2f startCoords, float g)
        {   
            this.alpha = alpha;
            this.E = E;
            this.m = m;
            this.startCoords = startCoords;
            double V0 = GetV0();
            // rozkład prędkości wylotowej na składowe X i Y
            this.V0x = Math.Cos(alpha) * V0;
            this.V0y = Math.Sin(alpha) * V0;
            this.g = g * C.DISTANCE_SCALE;
            // uruchomienie odliczania czasu od wylotu pocisku z procy
            this.clock = new Clock();
        }

        /**
         * metoda zwracająca koordynaty położenia pocisku
         * w zależności od upłyniętego czasu
         */
        public Vector2f UpdateCoords()
        {   
            float t = clock.ElapsedTime.AsSeconds();
            float x = startCoords.X + (float)V0x * t;
            //znak odwrócony żeby koordynaty były względem góry
            float y = startCoords.Y - (float)V0y * t + g * (float)Math.Pow(t, 2)/2; 
            Vector2f coords = new Vector2f(x, y);
            return coords;
        }

        /**
         * metoda zwracająca prędkość pocisku
         * wyświetlaną w trakcie jego lotu
         */
        public float UpdateVelocity()
        {   
            float t = clock.ElapsedTime.AsSeconds();
            double Vy = V0y - g * t;
            float v = (float)Math.Sqrt(Math.Pow(V0x, 2) + Math.Pow(Vy, 2));
            return v;
        }

        /**
         * metoda zwracająca prędkość początkową
         * pocisku, która jest następnie rozkładana
         * na składowe poziomą i pionową
         */
        public double GetV0()
        {   
            double V0 = Math.Sqrt(2 * E / m)*C.DISTANCE_SCALE;
            return V0;
        }

        /**
         * metoda zwracająca energię kinetyczną, którą
         * będzie miał pocisk opuszczający procę
         * @param stringK pseudo-stała sprężystości cięciwy
         * @param stretch naciąg cięciwy
         */
        public float GetEnergy(float stringK, float stretch)
        {   
            // MIMO ZASTOSOWANIA POJĘCIA STAŁEJ SPRĘŻYSTOŚCI K
            // NIE ODPOWIADA ONA ENERGII POTENCJALNEJ SPRĘŻYSTOŚCI
            // PONIEWAŻ NIE UWZGLĘDNIA STRAT W GUMIE I ENERGII
            // POTRZEBNEJ DO UNIESIENIA POCISKU DO PUNKTU WYLOTU
            float energy = (float)(stringK * Math.Pow(stretch/C.DISTANCE_SCALE, 2) / 2);
            return energy;
        }
    }
}
