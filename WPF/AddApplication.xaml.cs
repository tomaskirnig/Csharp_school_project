using Project.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.DirectoryServices;
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
using System.Xml.Linq;

namespace WPF
{
    /// <summary>
    /// Interakční logika pro AddApplication.xaml
    /// </summary>
    public partial class AddApplication : Window
    {
        private ORM db = new ORM("../../../../My_lib/bin/Debug/net8.0/My_lib.dll", "Data Source=../../../../Project/bin/Debug/net8.0/MyDatabase.db;", "My_lib.Models.Applicationn", "My_lib.Models.Highschool", "My_lib.Models.Specialization", "My_lib.Models.Student");

        public AddApplication()
        {
            InitializeComponent();
            LoadData();
        }

        private async void LoadData()
        {
            var highschoolType = ReflectionLoader.loadedTypes["Highschool"];
            var specializationType = ReflectionLoader.loadedTypes["Specialization"];
            var studentType = ReflectionLoader.loadedTypes["Student"];

            try
            {
                var highschoolsTask = FetchItems(highschoolType);
                var specializationsTask = FetchItems(specializationType);
                var studentsTask = FetchItems(studentType);

                // Wait for all tasks to complete
                await Task.WhenAll(highschoolsTask, specializationsTask, studentsTask);

                // Retrieve results
                var highschools = await highschoolsTask;
                var specializations = await specializationsTask;
                var students = await studentsTask;

                SetupComboBox(cmbSchool, highschools, "Name", "Id_School");
                cmbStudent.ItemsSource = students;
                SetupComboBox(cmbStudent, students, "Last_Name", "Id_Student");

                cmbSchool.SelectionChanged += (sender, e) =>
                {
                    if (cmbSchool.SelectedItem != null)
                    {
                        var selectedSchoolType = cmbSchool.SelectedItem.GetType();
                        var schoolIdProperty = selectedSchoolType.GetProperty("Id_School");
                        if (schoolIdProperty != null)
                        {
                            var schoolId = (int)schoolIdProperty.GetValue(cmbSchool.SelectedItem);
                            var filteredSpecializations = specializations.Where(s => s.GetType().GetProperty("Id_School").GetValue(s) == schoolId).ToList();

                            SetupComboBox(cmbSpecialization1, filteredSpecializations, "Name", "Id_Specialization");
                            SetupComboBox(cmbSpecialization2, filteredSpecializations, "Name", "Id_Specialization");
                            SetupComboBox(cmbSpecialization3, filteredSpecializations, "Name", "Id_Specialization");
                        }
                    }
                };
            }catch(Exception ex)
            {
                MessageBox.Show($"Error loading data: {ex.Message}");
            }
            
        }

        private async Task<IEnumerable<dynamic>> FetchItems(Type itemType)
        {
            var method = typeof(ORM).GetMethod("LoadAll").MakeGenericMethod(itemType);
            return await Task.Run(() => (IEnumerable<dynamic>)method.Invoke(db, null));
        }

        private void SetupComboBox(ComboBox comboBox, IEnumerable<dynamic> items, string displayMember, string valueMember)
        {
            comboBox.ItemsSource = items;
            comboBox.DisplayMemberPath = displayMember;
            comboBox.SelectedValuePath = valueMember;
        }

        private void Add_Button_Click(object sender, RoutedEventArgs e)
        {
            int id;
            int idSchool = (int)cmbSchool.SelectedValue;
            int idSpecialization1 = (int)cmbSpecialization1.SelectedValue;
            int? idSpecialization2 = (int?)cmbSpecialization2.SelectedValue;
            int? idSpecialization3 = (int?)cmbSpecialization3.SelectedValue;
            int idStudent = (int)cmbStudent.SelectedValue;
            string dateOfCreation = DateTime.Now.ToString("yyyy-MM-dd");

            try
            {
                MethodInfo getFreeIdMethod = typeof(ORM).GetMethod("GetFreeId").MakeGenericMethod(ReflectionLoader.loadedTypes["Applicationn"]);
                var result = getFreeIdMethod.Invoke(db, null);
                id = (int)result;

                if (idSpecialization2 == null)
                {
                    idSpecialization2 = -1;
                }
                if (idSpecialization3 == null)
                {
                    idSpecialization3 = -1;
                }

                object[] applicationParams = new object[]
                {
                    id,
                    idSchool,
                    idSpecialization1,
                    idSpecialization2,
                    idSpecialization3,
                    idStudent,
                    dateOfCreation
                };
                var application = Activator.CreateInstance(ReflectionLoader.loadedTypes["Applicationn"], applicationParams);

                MethodInfo saveMethod = typeof(ORM).GetMethod("Save");
                saveMethod.Invoke(db, new object[] { application });
                MessageBox.Show($"Application added with ID: {id}, school ID: {idSchool}, specialization1 ID: {idSpecialization1}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding application: {ex.Message}");
            }
        }


        private void Cancel_Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }
}
