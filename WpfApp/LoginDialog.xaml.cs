using System;
using System.Collections.Generic;
using System.Linq;
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
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using WpfApp;

namespace Login_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary> 

    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
            tb_Email.Text = Encrypt.DecryptToString(Settings.Default.login);
            tb_password.Password = Encrypt.DecryptToString(Settings.Default.password);
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            if (tb_Email.Text.Length == 0)
            {
                errormessage.Text = "Enter an email.";
                tb_Email.Focus();
            }
            else if (!Regex.IsMatch(tb_Email.Text, @"^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$"))
            {
                errormessage.Text = "Enter a valid email.";
                tb_Email.Select(0, tb_Email.Text.Length);
                tb_Email.Focus();
            }
            else
            {
                string email = tb_Email.Text;
                string password = tb_password.Password;

                Settings.Default.login = Encrypt.EncryptString(tb_Email.Text);
                Settings.Default.password = Encrypt.EncryptString(tb_password.Password);
                Settings.Default.Save();
                Close();
            }
        }

    }
}
