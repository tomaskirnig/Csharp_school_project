using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Dapper;
using Microsoft.Data.Sqlite;
using System.Data.SqlClient;
using Project.Data;
using System.Collections;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace WPF
{
    /// <summary>
    /// Interakční logika pro EditDeleteSchool.xaml
    /// </summary>
    public partial class EditDeleteSchool : Window
    {
        private ORM db = new ORM("../../../../My_lib/bin/Debug/net8.0/My_lib.dll", "Data Source=../../../../Project/bin/Debug/net8.0/MyDatabase.db;", "My_lib.Models.Applicationn", "My_lib.Models.Highschool", "My_lib.Models.Specialization", "My_lib.Models.Student");
        public EditDeleteSchool()
        {
            InitializeComponent();
            LoadSchools();
        }

        private void Home_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void LoadSchools()
        {
            Type highschoolType = ReflectionLoader.loadedTypes["Highschool"];

            // Create a list<Highschool>
            var schoolsType = typeof(List<>).MakeGenericType(highschoolType);

            var method = typeof(ORM).GetMethod("LoadAll").MakeGenericMethod(highschoolType);
            var result = method.Invoke(db, null);

            IList schoolList = (IList)result;

            // Clear previous content
            mainGrid.Children.Clear();
            mainGrid.RowDefinitions.Clear();

            int rowIndex = 0;
            foreach (var school in schoolList)
            {
                foreach (var prop in highschoolType.GetProperties())
                {
                    // Skip properties containing 'ID'
                    //if (prop.Name.ToLower().Contains("id")) continue;

                    mainGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

                    var label = new Label { Content = prop.Name + ":" };
                    var textBlock = new TextBlock { Text = prop.GetValue(school)?.ToString(), TextWrapping = TextWrapping.Wrap };

                    Grid.SetRow(label, rowIndex);
                    Grid.SetColumn(label, 0);
                    Grid.SetRow(textBlock, rowIndex);
                    Grid.SetColumn(textBlock, 1);

                    mainGrid.Children.Add(label);
                    mainGrid.Children.Add(textBlock);

                    rowIndex++;
                }

                // Buttons and divider

                mainGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

                var editButton = new Button { Content = "Edit", Margin = new Thickness(5) };
                editButton.Click += EditSchoolButton_Click;
                editButton.DataContext = school;

                var deleteButton = new Button { Content = "Delete", Margin = new Thickness(5) };
                deleteButton.Click += DeleteSchoolButton_Click;
                deleteButton.DataContext = school;

                Grid.SetRow(editButton, rowIndex);
                Grid.SetColumn(editButton, 0);
                Grid.SetRow(deleteButton, rowIndex);
                Grid.SetColumn(deleteButton, 1);

                mainGrid.Children.Add(editButton);
                mainGrid.Children.Add(deleteButton);

                rowIndex++; // Increment rowIndex after adding buttons

                // Divider row
                mainGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

                var divider = new Rectangle { Fill = Brushes.Gray, Height = 2, Margin = new Thickness(0, 10, 0, 10) };
                Grid.SetRow(divider, rowIndex);
                Grid.SetColumnSpan(divider, 2);

                mainGrid.Children.Add(divider);

                rowIndex++; // Increment rowIndex after adding divider
            }            
        }

        private void EditSchoolButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var school = button.DataContext;
            if (school != null) {
                var properties = school.GetType().GetProperties().ToArray();
                var parametrs = new object[properties.Length];
                foreach (var prop in properties)
                {
                    parametrs[Array.IndexOf(properties, prop)] = prop.GetValue(school);
                    //Console.WriteLine($"{prop.Name}: {prop.GetValue(school)}, {prop.GetValue(school).GetType()}");
                }
                var highschoolInstance = Activator.CreateInstance(ReflectionLoader.loadedTypes["Highschool"], parametrs);
                EditSchool editSchool = new EditSchool(highschoolInstance, LoadSchools);
                editSchool.Show();
            }
            else Console.WriteLine("school = null");
        }

        private void DeleteSchoolButton_Click(object sender, RoutedEventArgs e)
        {
            Type HighschoolType = ReflectionLoader.loadedTypes["Highschool"];
            var button = sender as Button;
            var school = Convert.ChangeType(button.DataContext, HighschoolType) as dynamic;

            if (school != null)
            {
                int schoolId = school.Id_School;
                MessageBox.Show($"Deleted School - ID: {schoolId}");

                MethodInfo deleteMethod = typeof(ORM).GetMethod("Delete").MakeGenericMethod(HighschoolType);
                deleteMethod.Invoke(db, new object[] { schoolId });

                LoadSchools();
            }
            else
            {
                MessageBox.Show("School not found.");
            }
            
        }
    }
}
