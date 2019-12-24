using JavaNet.Jvm.Converter;
using JavaNet.Jvm.Parser;
using System;
using System.IO;
using System.IO.Compression;
using System.Reflection;
using System.Text;

namespace JavaNet.Cli
{
    class Program
    {
        public static void Main(string[] args)
        {
            string dir = "C:/Users/Wesley/Desktop/New folder (3)/";
            AssemblyConverter converter = new AssemblyConverter("Rt");
            foreach (string file in Directory.GetFiles(dir))
            {
                using ZipArchive archive = ZipFile.OpenRead(file);
                foreach (ZipArchiveEntry entry in archive.Entries)
                {
                    if (Path.GetExtension(entry.Name) != ".class")
                    {
                        continue;
                    }
                    Console.WriteLine($"Started parsing '{entry.FullName}'");
                    using Stream stream = entry.Open();
                    JavaClass jc = JavaClass.Create(stream);
                    Console.WriteLine($"Finished parsing '{jc.GetName()}'");
                    converter.Include(jc);
                }
            }

            byte[] bytes = converter.Convert();
            File.WriteAllBytes("rt.dll", bytes);
            Assembly assembly = Assembly.Load(bytes);
            foreach (Type t in assembly.GetTypes())
            {
                Console.WriteLine($"Type: {t.FullName}");
            }
        }
    }
}
