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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace IndividueelProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    public partial class Login : Window
    {

        public Login()
        {
            InitializeComponent();
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //-- Easy log in (Debugging)
            TxtUser.Text = "admin";
            TxtPass.Text = "admin";
        }
        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            //--Controle op username en wachtwoord
            using (MagazijnEntities ctx = new MagazijnEntities())
            {
                Personeelslid user = ctx.Personeelslids.Where(u => u.Login == TxtUser.Text).FirstOrDefault();

                if (user != null)
                {
                    if (user.Wachtwoord == TxtPass.Text)
                    {
                        MagazijnWindow magazijnWindow = new MagazijnWindow();

                        magazijnWindow.User = user;

                        magazijnWindow.Owner = Application.Current.MainWindow;
                        magazijnWindow.Show();
                        magazijnWindow.Owner = null;
                        Close();
                    }
                    else
                    {
                        MessageBox.Show("Ongeldig wachtwoord");
                    }
                }
                else
                {
                    MessageBox.Show("Ongeldige gebruikersnaam");
                }
            }
        }
    }
}
