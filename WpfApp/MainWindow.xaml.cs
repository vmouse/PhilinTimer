using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
//using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Security;
using Microsoft.SharePoint.Client;

namespace WpfApp
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

        private void button_Click(object sender, RoutedEventArgs e)
        {
            Login_WPF.Login LoginDialog = new Login_WPF.Login();

            LoginDialog.ShowDialog();


            using (ClientContext cont = new ClientContext("https://inblago.sharepoint.com"))
            {

//                SecureString pass = new SecureString();
//                foreach (char c in "password".ToCharArray()) pass.AppendChar(c);

                cont.Credentials = new SharePointOnlineCredentials(LoginDialog.tb_Email.Text, LoginDialog.tb_password.SecurePassword);
                Web oWeb = cont.Web;
                cont.Load(oWeb);
                List tasks = oWeb.Lists.GetByTitle("Tasks");
                oWeb.Lists.RetrieveItems().Retrieve();

                CamlQuery caml = new Microsoft.SharePoint.Client.CamlQuery();
                caml.ViewXml = "<View Scope='RecursiveAll' />";
                ListItemCollection items = tasks.GetItems(caml);
                items.RetrieveItems().Retrieve();
                cont.ExecuteQuery();
                // foreach (List lst in oWeb.Lists)
                foreach (ListItem item in items)
                {
                    tb_Log.AppendText(item.FieldValues["Title"] + "\n");
                }
            }
        }
    }
}
