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
    /// Interaction logic for CrearClase.xaml
    /// </summary>
    public partial class CrearClase : Window
    {
        DBA dba = new DBA("127.0.0.1:6379,password=1");
        IDatabase conn;
        int noClases = -1;
        public CrearClase(int x)
        {
            noClases = x;
            tbId.Text = Convert.ToString(noClases + 1);
            InitializeComponent();
        }

        private void btCrear_Click(object sender, RoutedEventArgs e)
        {
            conn = dba.getConn();
            int id = Convert.ToInt32(tbId.Text);
            string nombre = tbNombre.Text;
            conn.HashSet("Clase:Clase"+id, new HashEntry[] { new HashEntry("id",id), new HashEntry("nombre",nombre)});
            this.Close();
        }
    }
}
