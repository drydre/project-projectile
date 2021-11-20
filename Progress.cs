using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projectile
{
    class Progress
    {

        public static void SaveProgress(Int16 level, string name)
        {

            using (BinaryWriter progress = new BinaryWriter(File.Open(name, FileMode.OpenOrCreate)))
            {
                progress.BaseStream.Position = progress.BaseStream.Length;
                progress.Write(level);
            }
        }

        public static void ResetProgress(ref List<Miniature> miniatures, string name)
        {
            foreach(Miniature min in miniatures)
            {
                if (min.IsRect())
                    min.completed = false;
            }
            BinaryWriter progress = new BinaryWriter(File.Open(name, FileMode.Create));
        }

        public static void LoadProgress(ref List<Miniature> miniatures, string name)
        {
            if (File.Exists(name))
            {
                using (BinaryReader progress = new BinaryReader(File.Open(name, FileMode.Open)))
                {
                    List<Int16> completedLevels = new List<Int16>();
                    int i = 0;
                    progress.BaseStream.Position = 0;
                    while (progress.BaseStream.Position != progress.BaseStream.Length)
                    {
                        completedLevels.Add(progress.ReadInt16());
                    }
                }
            }
            
        }
    }
}
