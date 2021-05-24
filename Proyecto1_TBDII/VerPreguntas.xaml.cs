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
    /// Interaction logic for VerPreguntas.xaml
    /// </summary>
    public partial class VerPreguntas : Window
    {
        int idClase = -1;
        DBA dba = new DBA("127.0.0.1:6379,password=1");
        IDatabase conn;

        struct Pregunta
        {
            public int id { get; set; }
            public string titulo { get; set; }
            public string descripcion { get; set; }
            public string respuesta { get; set; }

            public Pregunta(int i, string t, string d, string r)
            {
                id = i;
                titulo = t;
                descripcion = d;
                respuesta = r;
            }
        }

        public VerPreguntas(int id)
        {
            conn = dba.getConn();
            idClase = id;
            InitializeComponent();
            lbClase.Content = "Clase: " + conn.HashGet("Clase:Clase"+idClase, "nombre");
            updateTable();
        }

        public void updateTable()
        {
            conn = dba.getConn();
            int i = 1;
            List<Pregunta> preg = new List<Pregunta>();
            while (conn.KeyExists("Pregunta:C" + idClase + "P" + i))
            {
                preg.Add(new Pregunta(Convert.ToInt32(conn.HashGet("Pregunta:C" + idClase + "P" + i, "id")), conn.HashGet("Pregunta:C" + idClase + "P" + i,"titulo"), conn.HashGet("Pregunta:C" + idClase + "P" + i, "descripcion"), conn.HashGet("Pregunta:C" + idClase + "P" + i, "respuesta")));
                i++;
            }
            dgPreguntas.ItemsSource = preg;
        }

    }
}
