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
    /// Interakční logika pro AddHighschool.xaml
    /// </summary>
    public partial class AddHighschool : Window
    {
        private ORM db = new ORM("../../../../My_lib/bin/Debug/net8.0/My_lib.dll", "Data Source=../../../../Project/bin/Debug/net8.0/MyDatabase.db;", "My_lib.Models.Applicationn", "My_lib.Models.Highschool", "My_lib.Models.Specialization", "My_lib.Models.Student");

        public AddHighschool()
        {
            InitializeComponent();
        }

        private void Add_Button_Click(object sender, RoutedEventArgs e)
        {
            int id;
            string name = txtName.Text;        
            string address = txtAddress.Text;   
            string phoneNumber = txtPhoneNumber.Text; 
            string email = txtEmail.Text;       
            string city = txtCity.Text;

            try
            {
                MethodInfo getFreeIdMethod = typeof(ORM).GetMethod("GetFreeId").MakeGenericMethod(ReflectionLoader.loadedTypes["Highschool"]);
                var result = getFreeIdMethod.Invoke(db, null);
                id = (int)result;

                object[] highschoolParams = new object[]
                {
                    id,
                    name,
                    address,
                    phoneNumber,
                    email,
                    city
                };

                var highschool = Activator.CreateInstance(ReflectionLoader.loadedTypes["Highschool"], highschoolParams);

                MessageBox.Show($"Highschool added with ID: {id}, name: {name}, address: {address}");

                MethodInfo Save = typeof(ORM).GetMethod("Save");
                Save.Invoke(db, new object[] { highschool });
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
