using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using StackExchange.Redis;

namespace Proyecto1_TBDII
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        DBA dba = new DBA("127.0.0.1:6379,password=1");
        public Login()
        {
            InitializeComponent();
        }

        private void btLogin_Click(object sender, RoutedEventArgs e)
        {
            int id;
            string user, pass;
            user = tbUsuario.Text;
            pass = pbPassword.Password;
            IDatabase conn = dba.getConn();
            if(conn.KeyExists("Login:" + user))//validando que el usuario existe
            {
                if(user != "Admin")
                {
                    if(pass == conn.HashGet("Login:" + user, "password"))
                    {
                        id = Convert.ToInt32(conn.HashGet("Login:" + user, "idAlumno"));
                        MessageBox.Show(Convert.ToString(id));
                    }
                    else
                    {
                        MessageBox.Show("Usuario o contraseña incorrectos.");
                    }
                }
                else
                {
                    MessageBox.Show("No está autorizado a usar este usuario.");
                }
            }
            else
            {
                MessageBox.Show("El usuario ingresado no existe.");
            }
            tbUsuario.Text = "";
            pbPassword.Password = "";
        }
    }
}
