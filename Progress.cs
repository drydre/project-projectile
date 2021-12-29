using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projectile
{
    class Progress
    {   // klasa przechowująca metody zapisu, odczytu i resetu postępu

        public static void SaveProgress(Int16 level)
        {
            //metoda zapisu do pliku binarnego
            using (BinaryWriter progress = new BinaryWriter(File.Open(C.SAVE_PATH, FileMode.OpenOrCreate)))
            {
                if (DisplayMenu.miniatures[level - 1].completed == false) 
                {
                    progress.BaseStream.Position = progress.BaseStream.Length;
                    progress.Write(level);
                }
            }
            LoadProgress();
        }

        public static void ResetProgress(ref List<Miniature> miniatures)
        {
            //metoda resetowania pliku binarnego
            foreach(Miniature min in miniatures)
            {
                if (min.IsRect())
                    min.completed = false;
            }
            BinaryWriter progress = new BinaryWriter(File.Open(C.SAVE_PATH, FileMode.Create));
        }

        public static void LoadProgress()
        {
            //metoda wczytywania postępu po włączeniu rozgrywki
            if (File.Exists(C.SAVE_PATH))
            {
                using (BinaryReader progress = new BinaryReader(File.Open(C.SAVE_PATH, FileMode.Open)))
                {   // z pliku binarnego ukończone poziomy wczytywane są do listy
                    progress.BaseStream.Position = 0;
                    while (progress.BaseStream.Position != progress.BaseStream.Length)
                    {
                        DisplayMenu.miniatures[progress.ReadInt16() - 1].completed = true;
                    }
                }
            }
        }
    }
}
