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
    /// Interaction logic for HacerExamen.xaml
    /// </summary>
    public partial class HacerExamen : Window
    {
        DBA dba = new DBA("127.0.0.1:6379,password=1");
        IDatabase conn;
        int id = -1, cont = 1, noPreguntas = -1, correctas = 0, idAlumno = -1;
        string respOrder = "";
        AlumnoWindow aw;

        private void btEnviar_Click(object sender, RoutedEventArgs e)
        {
            conn = dba.getConn();
            bool resp;
            resp = Convert.ToBoolean(rbVerdadero.IsChecked);
            respOrder += ";" + Convert.ToInt32(resp);
            if (resp == Convert.ToBoolean(conn.HashGet("Pregunta:C" + id + "P" + cont, "respuesta")))
                correctas++;
            if(cont == noPreguntas)
            {
                respOrder = respOrder.Substring(1);
                conn.HashSet("Resultado:A"+idAlumno+"C"+id, new HashEntry[] { new HashEntry("idClase",id), new HashEntry("nombreClase", conn.HashGet("Clase:Clase"+id, "nombre")), new HashEntry("nota",correctas), new HashEntry("respOrder", respOrder)});
                this.Close();
                aw.updateTables();
            }
            cont++;
            setPregunta();
        }

        public HacerExamen(int i, int a, AlumnoWindow ad)
        {
            id = i;
            idAlumno = a;
            aw = ad;
            InitializeComponent();
            setInfo();
            setPregunta();
        }

        public void setPregunta()
        {
            conn = dba.getConn();
            if(cont <= noPreguntas)
            {
                tbTitulo.Text = conn.HashGet("Pregunta:C"+id+"P"+cont, "titulo");
                tbkDesc.Text = conn.HashGet("Pregunta:C" + id + "P" + cont, "descripcion");
                if (cont < noPreguntas)
                    btEnviar.Content = "Siguiente";
                else
                    btEnviar.Content = "Enviar";
            }
        }

        public void setInfo()
        {
            conn = dba.getConn();
            noPreguntas = Convert.ToInt32(conn.HashGet("Examen:ExamenC"+id, "noPreguntas"));
        }

    }
}
