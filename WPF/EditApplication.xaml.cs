using Project.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
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
    /// Interakční logika pro EditSchool.xaml
    /// </summary>
    public partial class EditApplication : Window
    {
        private ORM db = new ORM("../../../../My_lib/bin/Debug/net8.0/My_lib.dll", "Data Source=../../../../Project/bin/Debug/net8.0/MyDatabase.db;", "My_lib.Models.Applicationn", "My_lib.Models.Highschool", "My_lib.Models.Specialization", "My_lib.Models.Student");
        private dynamic Applicationn;
        private readonly Action _onWindowClosed;

        public EditApplication(dynamic applicationn, Action onWindowClosed)
        {
            InitializeComponent();
            Applicationn = applicationn;
            DataContext = Applicationn;
            _onWindowClosed = onWindowClosed;
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
                //cmbStudent.ItemsSource = students;
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
                cmbSchool.SelectedValue = Applicationn.Id_School;
                cmbSpecialization1.SelectedValue = Applicationn.Id_Specialization_1;
                cmbSpecialization2.SelectedValue = Applicationn.Id_Specialization_2;
                cmbSpecialization3.SelectedValue = Applicationn.Id_Specialization_3;
                cmbStudent.SelectedValue = Applicationn.Id_Student;
            }
            catch (Exception ex)
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

        public void Save_Button_Click(object sender, RoutedEventArgs e)
        {
            //Applicationn.Id_School = int.Parse(txtHighschool.Text);
            //Applicationn.Id_Specialization_1 = int.Parse(txtSpecialization1.Text);

            //if (string.IsNullOrWhiteSpace(txtSpecialization2.Text)) Applicationn.Id_Specialization_2 = -1;
            //else Applicationn.Id_Specialization_2 = int.Parse(txtSpecialization2.Text);

            //if (string.IsNullOrWhiteSpace(txtSpecialization3.Text)) Applicationn.Id_Specialization_3 = -1;
            //else Applicationn.Id_Specialization_3 = int.Parse(txtSpecialization3.Text);

            //Applicationn.Id_Student = int.Parse(txtStudent.Text);

            int? sp2 = (int?)cmbSpecialization2.SelectedValue;
            int? sp3 = (int?)cmbSpecialization3.SelectedValue;

            Applicationn.Id_School = (int)cmbSchool.SelectedValue;
            Applicationn.Id_Specialization_1 = (int)cmbSpecialization1.SelectedValue;
            Applicationn.Id_Specialization_2 = sp2 == null ? -1 : (int)sp2;
            Applicationn.Id_Specialization_3 = sp3 == null ? -1 : (int)sp3;
            Applicationn.Id_Student = (int)cmbStudent.SelectedValue;
            Applicationn.DateOfCreation = txtDateOfCreation.Text;

            var method = typeof(ORM).GetMethod("Update");
            method.Invoke(db, new object[] { Applicationn });

            MessageBoxResult res = MessageBox.Show(
                $"School updated: {Applicationn.Id_School}, {Applicationn.Id_Specialization_1}, {Applicationn.Id_Specialization_2}, {Applicationn.Id_Specialization_3}, {Applicationn.Id_Student}",
                "Update Successful",
                MessageBoxButton.OK);
            if (res == MessageBoxResult.OK)
            {
                this.Close();
            }
        }

        public void Cancel_Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            _onWindowClosed?.Invoke();
        }
    }
}
