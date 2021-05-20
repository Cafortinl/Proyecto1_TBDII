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
            int id = -1;
            string user, pass;
            user = tbUsuario.Text;
            pass = pbPassword.Password;
            IDatabase conn = dba.getConn();
            var hash = conn.HashGetAll("Login:"+user);
            string data="";
            for(int i = 0;i < 3;i++)
            {
                if (i == 3)
                    id = Convert.ToInt32(hash[i].Value);
            }
            MessageBox.Show(Convert.ToString(id));
        }
    }
}
