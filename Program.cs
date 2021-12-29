using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projectile
{
    class Program
    {   // klasa główna uruchamiająca pętlę programu
        static void Main(string[] args)
        {
            Projectile projectile = new Projectile();
            projectile.Run();
        }
    }
}
