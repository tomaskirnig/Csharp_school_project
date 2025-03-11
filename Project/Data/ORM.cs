using System;
using System.Data;
using System.Reflection;
using System.Linq;
using Dapper;
using Microsoft.Data.Sqlite;
using System.Data.SqlClient;
using System.Xml.Linq;
//using Microsoft.EntityFrameworkCore.Query;

namespace Project.Data
{
    public class ORM
    {
        private readonly string connectionString;

        public ORM(string assemblyPath, string connectionString, params string[] typeNames)
        {
            this.connectionString = connectionString;
            ReflectionLoader.LoadAssemblyAndTypes(assemblyPath, typeNames);

            //ReflectionLoader.PrintLoadedTypes();
        }

        public void EnsureTableExists(object entity)
        {
            Type entityType = entity.GetType();
            var tableName = entityType.Name;
            var properties = entityType.GetProperties();
            var columns = properties.Select(p => $"{p.Name} {SqlType(p.PropertyType)}").ToArray();
            string columnDefinitions = string.Join(", ", columns);

            string createTableQuery = $@"
            CREATE TABLE IF NOT EXISTS {tableName} (
                {columnDefinitions}
            );";


            using (IDbConnection db = new SqliteConnection(connectionString))
            {
                db.Execute(createTableQuery);
            }
        }

        public void Save(object entity)
        {
            using (IDbConnection db = new SqliteConnection(connectionString))
            {
                Type entityType = entity.GetType();
                var tableName = entityType.Name;

                // check if type is loaded
                if (!ReflectionLoader.loadedTypes.ContainsKey(entityType.Name))
                {
                    throw new InvalidOperationException("Type not loaded or not found.");
                }

                PropertyInfo[] properties = entityType.GetProperties();
                var columnNames = string.Join(", ", properties.Select(p => p.Name));
                var values = string.Join(", ", properties.Select(p => $"'{p.GetValue(entity)}'"));

                string query = $"INSERT INTO {tableName} ({columnNames}) VALUES ({values})";
                db.Execute(query);
            }
        }

        public void Update(object entity)
        {
            using (IDbConnection db = new SqliteConnection(connectionString))
            {
                Type entityType = entity.GetType();
                var tableName = entityType.Name;

                if (!ReflectionLoader.loadedTypes.ContainsKey(entityType.Name))
                {
                    throw new InvalidOperationException("Type not loaded or not found.");
                }

                PropertyInfo[] properties = entityType.GetProperties();

                // Constructing the SET clause
                var setClause = string.Join(", ", properties.Select(p => $"{p.Name} = @{p.Name}"));

                // Assuming the first property is the primary key
                var primaryKey = properties[0];
                var primaryKeyValue = primaryKey.GetValue(entity);

                string query = $"UPDATE {tableName} SET {setClause} WHERE {primaryKey.Name} = @PrimaryKeyValue";

                // Creating the parameters
                var parameters = new DynamicParameters();
                foreach (var property in properties)
                {
                    parameters.Add($"@{property.Name}", property.GetValue(entity));
                }
                parameters.Add("@PrimaryKeyValue", primaryKeyValue);

                db.Execute(query, parameters);
            }
        }


        public void Delete<T>(int id) where T : class
        {
            using (IDbConnection db = new SqliteConnection(connectionString))
            {
                Type type = typeof(T);
                if (!ReflectionLoader.loadedTypes.ContainsKey(type.Name))
                {
                    throw new InvalidOperationException("Type not loaded or not found." + type.Name);
                }

                PropertyInfo[] properties = type.GetProperties();

                string tableName = type.Name;
                string query = $"DELETE FROM {tableName} WHERE {properties[0].Name} = @Id";
                db.Execute(query, new { Id = id });
            }
        }

        public T Load<T>(int id) where T : class
        {
            using (IDbConnection db = new SqliteConnection(connectionString))
            {
                Type type = typeof(T);
                if (!ReflectionLoader.loadedTypes.ContainsKey(type.Name))
                {
                    throw new InvalidOperationException("Type not loaded or not found.");
                }

                PropertyInfo[] properties = type.GetProperties();

                string tableName = type.Name;
                string query = $"SELECT * FROM {tableName} WHERE {properties[0].Name} = @Id";
                return db.QueryFirstOrDefault<T>(query, new { Id = id });
            }
        }

        public List<T> LoadAll<T>() where T : class
        {
            using (IDbConnection db = new SqliteConnection(connectionString))
            {
                Type type = typeof(T);
                if (!ReflectionLoader.loadedTypes.ContainsKey(type.Name))
                {
                    throw new InvalidOperationException("Type not loaded or not found.");
                }

                string tableName = type.Name;
                string query = $"SELECT * FROM {tableName}";
                var tmp = db.Query<T>(query).ToList();
                //Console.WriteLine(tmp);
                return tmp;
            }
        }

        public int GetFreeId<T>() where T : class
        {
            using (IDbConnection db = new SqliteConnection(connectionString))
            {
                Type type = typeof(T);
                if (!ReflectionLoader.loadedTypes.ContainsKey(type.Name))
                {
                    throw new InvalidOperationException("Type not loaded or not found.");
                }

                PropertyInfo[] properties = type.GetProperties();

                string tableName = type.Name;
                string query = $"SELECT MAX({properties[0].Name}) FROM {tableName}";
                int? freeId = db.QueryFirstOrDefault<int>(query) + 1;
                return freeId ?? -1;
            }
        }

        public int GetCount<T>() where T : class
        {
            using (IDbConnection db = new SqliteConnection(connectionString))
            {
                Type type = typeof(T);
                if (!ReflectionLoader.loadedTypes.ContainsKey(type.Name))
                {
                    throw new InvalidOperationException("Type not loaded or not found.");
                }

                string tableName = type.Name;
                string query = $"SELECT COUNT(*) FROM {tableName}";
                return db.QueryFirstOrDefault<int>(query);
            }
        }

        private static string SqlType(Type dotNetType)
        {
            if (dotNetType == typeof(int)) return "INTEGER";
            else if (dotNetType == typeof(long)) return "BIGINT";
            else if (dotNetType == typeof(double)) return "REAL";
            else if (dotNetType == typeof(decimal)) return "DECIMAL";
            else if (dotNetType == typeof(bool)) return "BOOLEAN";
            else if (dotNetType == typeof(string)) return "TEXT";
            else if (dotNetType == typeof(DateOnly)) return "DATE";
            return "TEXT";
        }

        public void FillDatabase()
        {
            using (IDbConnection db = new SqliteConnection(connectionString))
            {
                for (int i = 0; i < 5; i++)
                {
                    object[] constructorParameters = new object[]
                    {
                        i + 1,                        // Id
                        "John",                   // First_Name
                        "Doe" + i,                // Last_Name, to vary between instances
                        "022110/123" + i,        // Personal_Identification_Number, made unique
                        "New York",               // Birth_Place
                        "2002-" + (i + 1 + 3) + "-" + (i * 2 + 3), // Birth_Date
                        "123 Main St"             // Address
                    };

                    var student = Activator.CreateInstance(ReflectionLoader.loadedTypes["Student"], constructorParameters);
                    Save(student);
                }

                // Create and save Highschool entries
                for (int i = 0; i < 5; i++)
                {
                    object[] highschoolParams = new object[]
                    {
                        i + 1,                       // Id
                        "Highschool " + i,       // Name
                        "100 Main St, City " + i,// Address
                        "123-456-789" + i,       // Phone_Number
                        "email" + i + "@school.com", // Email
                        "City " + i             // City
                    };
                    var highschool = Activator.CreateInstance(ReflectionLoader.loadedTypes["Highschool"], highschoolParams);
                    Save(highschool);
                }

                // Create and save Specialization entries
                for (int i = 0; i < 5; i++)
                {
                    object[] specializationParams = new object[]
                    {
                        i + 1,                       // Id
                        i + 1,                   // Id_School (assuming a simple integer for demo)
                        "Specialization " + i,   // Name
                        "Description " + i,      // Description
                        50 + i,                  // Number_Of_Available_Positions
                        4,                       // Duration_Of_Specialization
                        "English",               // Language
                        "Full-time"              // Form_Of_Study
                    };
                    var specialization = Activator.CreateInstance(ReflectionLoader.loadedTypes["Specialization"], specializationParams);
                    Save(specialization);
                }

                // Create and save Applicationn entries
                for (int i = 0; i < 5; i++)
                {
                    object[] applicationParams = new object[]
                    {
                        i + 1,                     // Id
                        i + 1,                     // Id_School
                        i + 1,                     // Id_Specialization_1
                        -1,                         // Id_Specialization_2
                        -1,                         // Id_Specialization_3
                        i + 1,                     // Id_Student
                        "2022-" + (i + 1) + "-" + (i * 2) // Date
                    };
                    var applicationn = Activator.CreateInstance(ReflectionLoader.loadedTypes["Applicationn"], applicationParams);
                    Save(applicationn);
                }
            }
        }
    }
}
