using Project.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

namespace WPF
{
    /// <summary>
    /// Interakční logika pro EditDeleteSpecialization.xaml
    /// </summary>
    public partial class EditDeleteSpecialization : Window
    {
        private ORM db = new ORM("../../../../My_lib/bin/Debug/net8.0/My_lib.dll", "Data Source=../../../../Project/bin/Debug/net8.0/MyDatabase.db;", "My_lib.Models.Applicationn", "My_lib.Models.Highschool", "My_lib.Models.Specialization", "My_lib.Models.Student");

        public EditDeleteSpecialization()
        {
            InitializeComponent();
            LoadSpecializations();
        }

        private void Home_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void LoadSpecializations()
        {
            Type specializationType = ReflectionLoader.loadedTypes["Specialization"];

            // Create a list<Specialization>
            var SpecializationsType = typeof(List<>).MakeGenericType(specializationType);

            var method = typeof(ORM).GetMethod("LoadAll").MakeGenericMethod(specializationType);
            var result = method.Invoke(db, null);

            IList specializationList = (IList)result;

            // Clear previous content
            mainGrid.Children.Clear();
            mainGrid.RowDefinitions.Clear();

            int rowIndex = 0;
            foreach (var specialization in specializationList)
            {
                var properties = specialization.GetType().GetProperties();
                foreach (var prop in properties)
                {
                    if (prop.Name == properties[0].Name) continue; // Skip ID property

                    mainGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

                    var label = new Label { Content = prop.Name + ":" };
                    var textBlock = new TextBlock { Text = prop.GetValue(specialization)?.ToString(), TextWrapping = TextWrapping.Wrap };

                    Grid.SetRow(label, rowIndex);
                    Grid.SetColumn(label, 0);
                    Grid.SetRow(textBlock, rowIndex);
                    Grid.SetColumn(textBlock, 1);

                    mainGrid.Children.Add(label);
                    mainGrid.Children.Add(textBlock);

                    rowIndex++; // Increment after adding each property pair
                }

                mainGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

                var editButton = new Button { Content = "Edit", Margin = new Thickness(5) };
                var deleteButton = new Button { Content = "Delete", Margin = new Thickness(5) };

                editButton.Click += EditSpecializationButton_Click;
                deleteButton.Click += DeleteSpecializationButton_Click;

                editButton.DataContext = specialization;
                deleteButton.DataContext = specialization;

                Grid.SetRow(editButton, rowIndex);
                Grid.SetColumn(editButton, 0);
                Grid.SetRow(deleteButton, rowIndex);
                Grid.SetColumn(deleteButton, 1);

                mainGrid.Children.Add(editButton);
                mainGrid.Children.Add(deleteButton);

                rowIndex++; // Increment after adding buttons

                mainGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

                var divider = new Rectangle { Fill = Brushes.Gray, Height = 2, Margin = new Thickness(0, 10, 0, 10) };

                Grid.SetRow(divider, rowIndex);
                Grid.SetColumnSpan(divider, 2);

                mainGrid.Children.Add(divider);

                rowIndex++; // Increment after adding divider
            }
        }

        private void EditSpecializationButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var specialization = button.DataContext;
            if (specialization != null)
            {
                var properties = specialization.GetType().GetProperties().ToArray();
                var parametrs = new object[properties.Length];
                foreach (var prop in properties)
                {
                    parametrs[Array.IndexOf(properties, prop)] = prop.GetValue(specialization);
                    //Console.WriteLine($"{prop.Name}: {prop.GetValue(specialization)}, {prop.GetValue(specialization).GetType()}");
                }
                var specializationlInstance = Activator.CreateInstance(ReflectionLoader.loadedTypes["Specialization"], parametrs);
                EditSpecialization editSpecializationWindow = new EditSpecialization(specializationlInstance, LoadSpecializations);
                editSpecializationWindow.Show();
            }
        }

        private void DeleteSpecializationButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var specialization = button.DataContext as dynamic; 

            if (specialization != null)
            {
                int specializationId = specialization.Id_Specialization; 
                MessageBox.Show($"Delete Specialization ID: {specializationId}");

                Type specializationType = ReflectionLoader.loadedTypes["Specialization"];
                MethodInfo deleteMethod = typeof(ORM).GetMethod("Delete").MakeGenericMethod(specializationType);
                deleteMethod.Invoke(db, new object[] { specializationId });

                LoadSpecializations(); // Refresh the list
            }
            else
            {
                MessageBox.Show("Specialization not found.");
            }
        }
    }
}
