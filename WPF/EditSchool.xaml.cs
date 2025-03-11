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
    public partial class EditSchool : Window
    {
        private ORM db = new ORM("../../../../My_lib/bin/Debug/net8.0/My_lib.dll", "Data Source=../../../../Project/bin/Debug/net8.0/MyDatabase.db;", "My_lib.Models.Applicationn", "My_lib.Models.Highschool", "My_lib.Models.Specialization", "My_lib.Models.Student");
        private dynamic Highschool;
        private readonly Action _onWindowClosed;

        public EditSchool(dynamic highschool, Action onWindowClosed)
        {
            InitializeComponent();
            Highschool = highschool;
            DataContext = Highschool;
            _onWindowClosed = onWindowClosed;
        }

        public void Save_Button_Click(object sender, RoutedEventArgs e)
        {
            Highschool.Name = txtName.Text;
            Highschool.Address = txtAddress.Text;
            Highschool.Phone_Number = txtPhoneNumber.Text;
            Highschool.Email = txtEmail.Text;
            Highschool.City = txtCity.Text;

            var method = typeof(ORM).GetMethod("Update");
            method.Invoke(db, new object[] { Highschool });

            MessageBoxResult res = MessageBox.Show(
                $"School updated: {Highschool.Name}, {Highschool.Address}, {Highschool.Phone_Number}, {Highschool.Email}, {Highschool.City}",
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
