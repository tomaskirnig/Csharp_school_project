using Microsoft.EntityFrameworkCore.Diagnostics;
using Project.Data;
using System;
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
    /// Interakční logika pro AddSpecialization.xaml
    /// </summary>
    public partial class AddSpecialization : Window
    {
        private ORM db = new ORM("../../../../My_lib/bin/Debug/net8.0/My_lib.dll", "Data Source=../../../../Project/bin/Debug/net8.0/MyDatabase.db;", "My_lib.Models.Applicationn", "My_lib.Models.Highschool", "My_lib.Models.Specialization", "My_lib.Models.Student");
        public AddSpecialization()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            var highschoolType = ReflectionLoader.loadedTypes["Highschool"];
            var method = db.GetType().GetMethod("LoadAll").MakeGenericMethod(highschoolType);
            var result = method.Invoke(db, null);
            var highschools = (IEnumerable<object>)result;

            SetupComboBox(this.cbmHighschool, highschools, "Name", "Id_School");
        }

        private void SetupComboBox(ComboBox comboBox, IEnumerable<dynamic> items, string displayMember, string valueMember)
        {
            comboBox.ItemsSource = items;
            comboBox.DisplayMemberPath = displayMember;
            comboBox.SelectedValuePath = valueMember;
        }

        private void Add_Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MethodInfo getFreeIdMethod = typeof(ORM).GetMethod("GetFreeId").MakeGenericMethod(ReflectionLoader.loadedTypes["Applicationn"]);
                var result = getFreeIdMethod.Invoke(db, null);
                var id = (int)result;
                var schoolId = (int)cbmHighschool.SelectedValue;

                object[] specializationParams = new object[]
                {
                    id,
                    schoolId,
                    txtName.Text,
                    txtDescription.Text,
                    int.Parse(txtNumber_Of_Available_Positions.Text),
                    int.Parse(txtDuration_Of_Specialization.Text),
                    txtLanguage.Text,
                    txtForm_Of_Study.Text
                };

                var specializationType = ReflectionLoader.loadedTypes["Specialization"];
                var specialization = Activator.CreateInstance(specializationType, specializationParams);

                MessageBox.Show($"Specialization added with ID: {id}, name: {txtName.Text}");

                MethodInfo Save = typeof(ORM).GetMethod("Save");
                Save.Invoke(db, new object[] { specialization });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error retrieving free ID: {ex.Message}");
            }
        }

        private void Cancel_Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
