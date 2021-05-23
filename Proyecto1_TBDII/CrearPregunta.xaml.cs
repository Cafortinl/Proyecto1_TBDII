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
    /// Interaction logic for CrearPregunta.xaml
    /// </summary>
    public partial class CrearPregunta : Window
    {
        DBA dba = new DBA("127.0.0.1:6379,password=1");
        IDatabase conn;
        int idClase = -1, noPregunta = -1, idPregunta = -1;
        AdminWindow aw;

        private void btCrearPregunta_Click(object sender, RoutedEventArgs e)
        {
            conn = dba.getConn();
            string titulo = tbTitulo.Text;
            string descripcion = tbDesc.Text;
            string resp = Convert.ToString(cbRespuesta.IsEnabled);
            conn.HashSet("Pregunta:C"+idClase+"P"+noPregunta, new HashEntry[] { new HashEntry("id", idPregunta), new HashEntry("titulo", titulo), new HashEntry("descripcion", descripcion), new HashEntry("idClase", idClase), new HashEntry("respuesta", resp)});
            aw.updateTable();
            this.Close();
        }

        public CrearPregunta(int c, int p, int t, AdminWindow a)
        {
            idClase = c;
            noPregunta = p+1;
            idPregunta = t+1;
            aw = a;
            InitializeComponent();
            tbId.Text = Convert.ToString(idPregunta);
            tbClase.Text = Convert.ToString(idClase);
        }
    }
}
