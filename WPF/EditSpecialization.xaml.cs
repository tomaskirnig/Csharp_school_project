using Project.Data;
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

namespace WPF
{
    /// <summary>
    /// Interakční logika pro EditSpecialization.xaml
    /// </summary>
    public partial class EditSpecialization : Window
    {
        private ORM db = new ORM("../../../../My_lib/bin/Debug/net8.0/My_lib.dll", "Data Source=../../../../Project/bin/Debug/net8.0/MyDatabase.db;", "My_lib.Models.Applicationn", "My_lib.Models.Highschool", "My_lib.Models.Specialization", "My_lib.Models.Student");
        private dynamic Specialization;
        private readonly Action _onWindowClosed;

        public EditSpecialization(dynamic specialization, Action onWindowClosed)
        {
            InitializeComponent();
            Specialization = specialization;
            DataContext = Specialization;
            _onWindowClosed = onWindowClosed;
            LoadData();
        }

        public void Save_Button_Click(object sender, RoutedEventArgs e)
        {
            Specialization.Id_School = (int)cbmHighschool.SelectedValue;
            Specialization.Name = txtName.Text;
            Specialization.Description = txtDescription.Text;
            Specialization.Number_Of_Available_Positions = int.Parse(txtNumber_Of_Available_Positions.Text);
            Specialization.Duration_Of_Specialization = int.Parse(txtDuration_Of_Specialization.Text);
            Specialization.Language = txtLanguage.Text;
            Specialization.Form_Of_Study = txtForm_Of_Study.Text;

            var method = typeof(ORM).GetMethod("Update");
            method.Invoke(db, new object[] { Specialization });

            MessageBoxResult res = MessageBox.Show(
                $"Specialization updated: {Specialization.Name}, {Specialization.Number_Of_Available_Positions}, {Specialization.Duration_Of_Specialization}, {Specialization.Language}, {Specialization.Form_Of_Study}",
                "Update Successful",
                MessageBoxButton.OK);
            if (res == MessageBoxResult.OK)
            {
                this.Close();
            }
        }

        private void LoadData()
        {
            Type highschoolType = ReflectionLoader.loadedTypes["Highschool"];

            var res = typeof(ORM).GetMethod("LoadAll").MakeGenericMethod(highschoolType).Invoke(db, null);
            var highschools = (IEnumerable<dynamic>)res;

            SetupComboBox(cbmHighschool, highschools, "Name", "Id_School");

            cbmHighschool.SelectedValue = Specialization.Id_School;
        }

        private void SetupComboBox(ComboBox comboBox, IEnumerable<dynamic> items, string displayMember, string valueMember)
        {
            comboBox.DisplayMemberPath = displayMember;
            comboBox.SelectedValuePath = valueMember;
            comboBox.ItemsSource = items;
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
