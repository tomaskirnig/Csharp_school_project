using Project.Data;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void AddApplication_Click(object sender, RoutedEventArgs e)
        {
            AddApplication addApplication = new AddApplication();
            addApplication.Show();
        }

        private void AddSpecialization_Click(object sender, RoutedEventArgs e)
        {
            AddSpecialization addSpecialization = new AddSpecialization();
            addSpecialization.Show();
        }

        // Event handler for editing or deleting an application
        private void EditDeleteApplication_Click(object sender, RoutedEventArgs e)
        {
            EditDeleteApplication editDeleteApplication = new EditDeleteApplication();
            editDeleteApplication.Show();
        }

        private void EditDeleteSpecialization_Click(object sender, RoutedEventArgs e)
        {
            EditDeleteSpecialization editDeleteSpecialization = new EditDeleteSpecialization();
            editDeleteSpecialization.Show();
        }

        // Event handler for adding a new school
        private void AddSchool_Click(object sender, RoutedEventArgs e)
        {
            AddHighschool addHighschool = new AddHighschool();
            addHighschool.Show();
        }

        // Event handler for editing or deleting a school
        private void EditDeleteSchool_Click(object sender, RoutedEventArgs e)
        {
            EditDeleteSchool editDeleteSchool = new EditDeleteSchool();
            editDeleteSchool.Show();
        }

        private void MakeReport(object sender, RoutedEventArgs e)
        {
            var db = new ORM("../../../../My_lib/bin/Debug/net8.0/My_lib.dll", "Data Source=../../../../Project/bin/Debug/net8.0/MyDatabase.db;", "My_lib.Models.Applicationn", "My_lib.Models.Highschool", "My_lib.Models.Specialization", "My_lib.Models.Student");

            var applicationType = ReflectionLoader.loadedTypes["Applicationn"];
            var highschoolType = ReflectionLoader.loadedTypes["Highschool"];

            var countApp = (int)typeof(ORM).GetMethod("GetCount").MakeGenericMethod(applicationType).Invoke(db, null);
            var countHig = (int)typeof(ORM).GetMethod("GetCount").MakeGenericMethod(highschoolType).Invoke(db, null);

            MessageBox.Show($"Number of applications: {countApp}\nNumber of highschools: {countHig}");
        }
    }
}