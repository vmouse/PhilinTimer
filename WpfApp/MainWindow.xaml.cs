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

                string      login = LoginDialog.tb_Email.Text; ;
                SecureString pass = LoginDialog.tb_password.SecurePassword;

                //                SecureString pass = new SecureString();
                //                foreach (char c in "password".ToCharArray()) pass.AppendChar(c);

                cont.Credentials = new SharePointOnlineCredentials(login, pass);
                Web oWeb = cont.Web;
                cont.Load(oWeb);
                cont.Load(cont.Web.CurrentUser);
                List tasks = oWeb.Lists.GetByTitle("Tasks");
                cont.ExecuteQuery();

                CamlQuery caml = new Microsoft.SharePoint.Client.CamlQuery();
                caml.ViewXml = string.Format(@"<View Scope='RecursiveAll'>
                    <Query><Where><And>
                        <Eq><FieldRef Name = 'AssignedTo' LookupId = 'TRUE' /><Value Type = 'Lookup' >{0}</Value></Eq>
                        <Neq><FieldRef Name = 'PercentComplete' /><Value Type='Number' >1.00</Value></Neq>
                    </And></Where></Query></View>", cont.Web.CurrentUser.Id);
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
