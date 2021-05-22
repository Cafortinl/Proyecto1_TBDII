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
    /// Interaction logic for Registrarse.xaml
    /// </summary>
    public partial class Registrarse : Window
    {
        DBA dba = new DBA("127.0.0.1:6379,password=1");
        public Registrarse()
        {
            InitializeComponent();
        }

        private void btRegistrarse_Click(object sender, RoutedEventArgs e)
        {
            int id;
            string nombre, usuario, password;
            id = Convert.ToInt32(tbId.Text);
            nombre = tbNombre.Text;
            usuario = tbUsuario.Text;
            password = pbPassword.Password;
            IDatabase conn = dba.getConn();
            if(!conn.KeyExists("Alumno:Alumno" + id))
            {
                if (!conn.KeyExists("Login:" + usuario))
                {
                    conn.HashSet("Alumno:Alumno"+id, new HashEntry[] { new HashEntry("id",id), new HashEntry("nombre", nombre)});
                    conn.HashSet("Login:"+usuario, new HashEntry[] { new HashEntry("usuario", usuario), new HashEntry("password", password), new HashEntry("idAlumno", id)});
                    tbId.Text = "";
                    tbNombre.Text = "";
                    tbUsuario.Text = "";
                    pbPassword.Password = "";
                    this.Close();
                }
                else
                {
                    MessageBox.Show("El usuario ingresado no es válido.");
                }
            }
            else
            {
                MessageBox.Show("El id ingresado no es válido.");
            }
        }
    }
}
