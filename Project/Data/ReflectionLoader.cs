using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;


namespace Project.Data
{
    public static class ReflectionLoader
    {
        public static Dictionary<string, Type> loadedTypes = new Dictionary<string, Type>();

        public static void LoadAssemblyAndTypes(string assemblyPath, params string[] typeNames)
        {
            Assembly assembly;
            try
            {
                assembly = Assembly.LoadFile(Path.GetFullPath(assemblyPath));

                foreach (var typeName in typeNames)
                {
                    Type type = assembly.GetType(typeName);
                    if (type != null)
                    {
                        var simpleTypeName = typeName.Split('.').Last();
                        loadedTypes[simpleTypeName] = type;
                        //Console.WriteLine($"Loaded type: {typeName}");
                    }
                    else
                    {
                        Console.WriteLine($"Type not found: {typeName}");
                    }
                }
            }
            catch
            {
                Console.WriteLine("Library not found.");
            }
        }

        public static void PrintLoadedTypes()
        {
            if (loadedTypes.Count > 0)
            {
                foreach (var type in loadedTypes)
                {
                    Console.WriteLine(type.Key);
                }
            }
            else Console.WriteLine("No types loaded.");
            
        }
    }
}