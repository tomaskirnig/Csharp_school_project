using System;
using Dapper;
using Microsoft.Data.Sqlite;
using System.Data.SqlTypes;
using System.Security.AccessControl;
using System.Transactions;
using Project.Data;
using static System.Net.Mime.MediaTypeNames;
using System.Reflection;
namespace Project
{
    public class Program
    {
        static void Main(string[] args)
        {
            ORM db = new ORM("../../../../My_lib/bin/Debug/net8.0/My_lib.dll", "Data Source=MyDatabase.db;", "My_lib.Models.Applicationn", "My_lib.Models.Highschool", "My_lib.Models.Specialization", "My_lib.Models.Student");

            //creation and filling of the database

            foreach (var type in ReflectionLoader.loadedTypes.Values)
            {
                Console.WriteLine(type);
                var instance = Activator.CreateInstance(type);
                db.EnsureTableExists(instance);
            }

            db.FillDatabase();
        }
    }
}
