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
    /// Interaction logic for AlumnoWindow.xaml
    /// </summary>
    public partial class AlumnoWindow : Window
    {
        int id = -1;
        DBA dba = new DBA("127.0.0.1:6379,password=1");
        IDatabase conn;

        struct Realizados
        {
            public int idClase { get; set; }
            public string clase { get; set; }
            public int nota { get; set; }

            public Realizados(int i, string c, int n)
            {
                idClase = i;
                clase = c;
                nota = n;
            }
        }

        struct porRealizar
        {
            public int id { get; set; }
            public string clase { get; set; }
            public int noPreguntas { get; set; }

            public porRealizar(int i,string c, int p)
            {
                id = i;
                clase = c;
                noPreguntas = p;
            }
        }

        public AlumnoWindow(int x)
        {
            conn = dba.getConn();
            id = x;
            InitializeComponent();
            lbNombre.Content = "Bienvenido, " + conn.HashGet("Alumno:Alumno"+id, "nombre");
            updateTables();
        }

        public void updateTables()
        {
            List<Realizados> r = new List<Realizados>();
            List<porRealizar> p = new List<porRealizar>();
            List<int> realizados = new List<int>();
            conn = dba.getConn();
            int i = 1;
            while (conn.KeyExists("Clase:Clase"+i))
            {
                if(conn.KeyExists("Resultado:A" + id + "C" + i))
                {
                    r.Add(new Realizados(Convert.ToInt32(conn.HashGet("Resultado:A"+id+"C"+i, "idClase")),conn.HashGet("Resultado:A" + id + "C" + i, "nombreClase"), Convert.ToInt32(conn.HashGet("Resultado:A" + id + "C" + i, "nota"))));
                    realizados.Add(i);
                }
                if (!realizados.Contains(i))
                {
                    if(conn.KeyExists("Examen:ExamenC" + i))
                        p.Add(new porRealizar(i,conn.HashGet("Clase:Clase"+i,"nombre"), Convert.ToInt32(conn.HashGet("Examen:ExamenC"+i, "noPreguntas"))));
                }
                i++;
            }
            dgRealizados.ItemsSource = r;
            dgPorRealizar.ItemsSource = p;
        }

        private void btHacerExamen_Click(object sender, RoutedEventArgs e)
        {
            int index = dgPorRealizar.SelectedIndex;
            if (index > -1)
            {
                DataGridRow row = dgPorRealizar.ItemContainerGenerator.ContainerFromIndex(index) as DataGridRow;
                porRealizar info = (porRealizar)dgPorRealizar.ItemContainerGenerator.ItemFromContainer(row);
                HacerExamen he = new HacerExamen(info.id,id, this);
                he.Show();
            }
            else
            {
                MessageBox.Show("Debe seleccionar una clase para realizar un examen.");
            }
        }

        private void btResultados_Click(object sender, RoutedEventArgs e)
        {
            int index = dgRealizados.SelectedIndex;
            if (index > -1)
            {
                DataGridRow row = dgRealizados.ItemContainerGenerator.ContainerFromIndex(index) as DataGridRow;
                Realizados info = (Realizados)dgRealizados.ItemContainerGenerator.ItemFromContainer(row);
                VerRespuestas vr = new VerRespuestas(id, info.idClase);
                vr.Show();
            }
            else
            {
                MessageBox.Show("Debe seleccionar un examen para ver los resultados.");
            }
        }
    }
}
