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
    /// Interaction logic for CrearExamen.xaml
    /// </summary>
    public partial class CrearExamen : Window
    {
        DBA dba = new DBA("127.0.0.1:6379,password=1");
        int id = -1, idClase = -1, noPreguntas = -1, noExamen = -1;
        AdminWindow aw;
        IDatabase conn;

        private void btCrearExamen_Click(object sender, RoutedEventArgs e)
        {
            conn = dba.getConn();
            int preg = Convert.ToInt32(tbPreguntas.Text);
            string fecha = ((DateTime)dp_fecha.SelectedDate).ToString("d");
            if(preg > noPreguntas)
            {
                MessageBox.Show("El examen no puede tener más preguntas que las que tiene la clase.");
                tbPreguntas.Text = "";
            }
            else
            {
                conn.HashSet("Examen:E"+noExamen+"C"+idClase, new HashEntry[] { new HashEntry("id", id), new HashEntry("idClase", idClase), new HashEntry("noPreguntas", preg), new HashEntry("fecha", fecha),new HashEntry("noExamen", noExamen)});
                aw.updateTable();
                this.Close();
            }
        }

        public CrearExamen(int i, int c, int p, AdminWindow a)
        {
            id = i + 1;
            idClase = c;
            noPreguntas = p;
            aw = a;
            updateNoExamen();
            InitializeComponent();
            tbId.Text = Convert.ToString(id);
            tbClase.Text = Convert.ToString(idClase);
        }

        public void updateNoExamen()
        {
            conn = dba.getConn();
            int cont = 1;
            while(conn.KeyExists("Examen:E" + cont + "C" + idClase))
            {
                cont++;
            }
            noExamen = cont;
        }

    }
}
