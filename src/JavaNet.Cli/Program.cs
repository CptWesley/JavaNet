﻿using JavaNet.Jvm.Converter;
using JavaNet.Jvm.Parser;
using Mono.Cecil;
using System;
using System.IO;
using System.IO.Compression;
using System.Reflection;

namespace JavaNet.Cli
{
    class Program
    {
        public static void Main()
        {
            
            string dir = "C:/Users/Wesley/Desktop/New folder (4)/";
            AssemblyConverter converter = new AssemblyConverter("Rt");
            foreach (string file in Directory.GetFiles(dir))
            {
                if (Path.GetExtension(file) == ".jar")
                {
                    using ZipArchive archive = ZipFile.OpenRead(file);
                    foreach (ZipArchiveEntry entry in archive.Entries)
                    {
                        if (Path.GetExtension(entry.Name) == ".class")
                        {
                            using Stream stream = entry.Open();
                            AddFile(converter, stream);
                        }
                    }
                }
                else if (Path.GetExtension(file) == ".class")
                {
                    using Stream stream = File.OpenRead(file);
                    AddFile(converter, stream);
                }
            }

            byte[] bytes = converter.Convert();
            File.WriteAllBytes("rt.dll", bytes);

            foreach (Type type in Assembly.Load(bytes).GetTypes())
            {
                Console.WriteLine($"Type: {type.FullName}");
            }
        }

        private static void AddFile(AssemblyConverter converter, Stream stream)
        {
            JavaClass jc = JavaClass.Create(stream);
            Console.WriteLine($"Finished parsing '{jc.GetName()}'");
            converter.Include(jc);
        }

        private static TypeReference DuplicateDefinition(AssemblyDefinition assembly, TypeDefinition type, TypeReference baseType)
        {
            if (type == baseType || type == null)
            {
                return assembly.MainModule.TypeSystem.Object;
            }

            TypeDefinition dup = new TypeDefinition(type.Namespace, type.Name, type.Attributes, DuplicateDefinition(assembly, type.BaseType?.Resolve(), baseType));
            assembly.MainModule.Types.Add(dup);
            return dup;
        }
    }
}
