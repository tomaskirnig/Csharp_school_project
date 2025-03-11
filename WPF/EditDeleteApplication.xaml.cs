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

    class ThreadData
    {
        public string Input { get; set; }
        public dynamic Output { get; set; }
    }
    /// <summary>
    /// Interakční logika pro EditDeleteApplication.xaml
    /// </summary>
    public partial class EditDeleteApplication : Window
    {
        private ORM db = new ORM("../../../../My_lib/bin/Debug/net8.0/My_lib.dll", "Data Source=../../../../Project/bin/Debug/net8.0/MyDatabase.db;", "My_lib.Models.Applicationn", "My_lib.Models.Highschool", "My_lib.Models.Specialization", "My_lib.Models.Student");
        public EditDeleteApplication()
        {
            InitializeComponent();
            LoadApplications();
        }

        private void Home_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void LoadApplications()
        {
            ThreadData thDataApplication = new ThreadData { Input = "Applicationn" };
            ThreadData thDataSpecialization = new ThreadData { Input = "Specialization" };
            ThreadData thDataHighschool = new ThreadData { Input = "Highschool" };
            ThreadData thDataStudent= new ThreadData { Input = "Student" };

            Thread threadApp = new Thread(new ParameterizedThreadStart(LoadAllEntities));
            Thread threadSpe = new Thread(new ParameterizedThreadStart(LoadAllEntities));
            Thread threadHig = new Thread(new ParameterizedThreadStart(LoadAllEntities));
            Thread threadStu = new Thread(new ParameterizedThreadStart(LoadAllEntities));

            threadApp.Start(thDataApplication);
            threadSpe.Start(thDataSpecialization);
            threadHig.Start(thDataHighschool);
            threadStu.Start(thDataStudent);

            threadApp.Join();
            threadSpe.Join();
            threadHig.Join();
            threadStu.Join();

            var applicationList = (IList)thDataApplication.Output;
            var specializationList = (IList)thDataSpecialization.Output;
            var highschoolList = (IList)thDataHighschool.Output;
            var studentList = (IList)thDataStudent.Output;


            // Clear previous content
            mainGrid.Children.Clear();
            mainGrid.RowDefinitions.Clear();

            int rowIndex = 0;
            foreach (var application in applicationList)
            {
                var properties = application.GetType().GetProperties();
                foreach (var prop in properties)
                {
                    // if (prop.Name == properties[0].Name) continue; // Skip ID property

                    string displayValue = prop.GetValue(application).ToString();

                    if (prop.Name.Contains("Id_Specialization_"))
                    {
                        if (Convert.ToInt32(displayValue) == -1)
                        {
                            displayValue = "None";
                        }
                        else
                        {
                            var specialization = specializationList.Cast<object>().FirstOrDefault(s => s.GetType().GetProperty("Id_Specialization").GetValue(s).Equals(Convert.ToInt32(displayValue)));
                            if (specialization != null)
                            {
                                displayValue += " " + specialization.GetType().GetProperty("Name").GetValue(specialization).ToString();
                            
                            }
                        }
                        
                    }
                    else if (prop.Name == "Id_Student")
                    {
                        var student = studentList.Cast<object>()
                            .FirstOrDefault(s => s.GetType().GetProperty("Id_Student").GetValue(s).Equals(Convert.ToInt32(displayValue)));
                        if (student != null)
                        {
                            var firstName = student.GetType().GetProperty("First_Name").GetValue(student);
                            var lastName = student.GetType().GetProperty("Last_Name").GetValue(student);
                            displayValue += $" {firstName} {lastName}";
                        }
                    }
                    else if (prop.Name == "Id_School")
                    {
                        var highschool = highschoolList.Cast<object>()
                            .FirstOrDefault(h => h.GetType().GetProperty("Id_School").GetValue(h).Equals(Convert.ToInt32(displayValue)));
                        if (highschool != null)
                        {
                            displayValue += " " + highschool.GetType().GetProperty("Name").GetValue(highschool).ToString();
                        }
                    }

                    mainGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

                    var label = new Label { Content = prop.Name + ":" };
                    var textBlock = new TextBlock { Text = displayValue?.ToString(), TextWrapping = TextWrapping.Wrap };

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

                editButton.Click += EditApplicationButton_Click;
                deleteButton.Click += DeleteApplicationButton_Click;

                editButton.DataContext = application;
                deleteButton.DataContext = application;

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

        public void LoadAllEntities(object obj)
        {
            ThreadData data = obj as ThreadData;

            Type entityType = ReflectionLoader.loadedTypes[data.Input];
            MethodInfo method = typeof(ORM).GetMethod("LoadAll").MakeGenericMethod(entityType);

            data.Output = method.Invoke(db, null);
        }

        private void EditApplicationButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var application = button.DataContext;
            if (application != null)
            {
                var properties = application.GetType().GetProperties().ToArray();
                var parametrs = new object[properties.Length];
                foreach (var prop in properties)
                {
                    parametrs[Array.IndexOf(properties, prop)] = prop.GetValue(application);
                    //Console.WriteLine($"{prop.Name}: {prop.GetValue(application)}, {prop.GetValue(application).GetType()}");
                }
                var applicationlInstance = Activator.CreateInstance(ReflectionLoader.loadedTypes["Applicationn"], parametrs);
                EditApplication editApplicationWindow = new EditApplication(applicationlInstance, LoadApplications);
                editApplicationWindow.Show();
            }
        }

        private void DeleteApplicationButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var application = button.DataContext as dynamic; 

            if (application != null)
            {
                int applicationId = application.Id_Applications;
                MessageBox.Show($"Delete Application ID: {applicationId}");

                Type applicationType = ReflectionLoader.loadedTypes["Applicationn"];
                MethodInfo deleteMethod = typeof(ORM).GetMethod("Delete").MakeGenericMethod(applicationType);
                deleteMethod.Invoke(db, new object[] { applicationId });

                LoadApplications(); // Refresh the list
            }
            else
            {
                MessageBox.Show("Application not found.");
            }
        }
    }
}
