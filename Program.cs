using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projectile
{
    /** Klasa główna uruchamiająca pętlę programu.*/
    class Program
    {   
        static void Main(string[] args)
        {
            Projectile projectile = new Projectile();
            projectile.Run();
        }
    }
}
