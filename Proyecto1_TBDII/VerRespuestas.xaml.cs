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
    /// Interaction logic for VerRespuestas.xaml
    /// </summary>
    public partial class VerRespuestas : Window
    {
        DBA dba = new DBA("127.0.0.1:6379,password=1");
        IDatabase conn;
        int id, idClase;
        string[] respuestas;
        public VerRespuestas(int i, int ic)
        {
            id = i;
            idClase = ic;
            InitializeComponent();
            conn = dba.getConn();
            lbNombreClase.Content = "Examen de: " + conn.HashGet("Clase:Clase"+idClase, "nombre");
            respuestas = Convert.ToString(conn.HashGet("Resultado:A" + id + "C" + idClase, "respOrder")).Split(";");
            updateTable();
        }

        struct Resultado{
            public string titulo { get; set; }
            public string pregunta { get; set; }
            public string respuesta { get; set; }
            public string respuestaDada { get; set; }

            public Resultado(string t, string p, string r, string rd)
            {
                titulo = t;
                pregunta = p;
                respuesta = r;
                respuestaDada = rd;
            }

        }
        public void updateTable()
        {
            conn = dba.getConn();
            int i = 1;
            List<Resultado> res = new List<Resultado>();
            while(i <= Convert.ToInt32(conn.HashGet("Examen:ExamenC" + idClase, "noPreguntas")))
            {
                res.Add(new Resultado(conn.HashGet("Pregunta:C" + idClase + "P" + i, "titulo"), conn.HashGet("Pregunta:C" + idClase + "P" + i, "descripcion"), conn.HashGet("Pregunta:C" + idClase + "P" + i, "respuesta"), Convert.ToString(Convert.ToBoolean(Convert.ToInt32(respuestas[i-1])))));
                i++;
            }
            dgResultados.ItemsSource = res;
        }
    }
}
