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
            public int noExamen { get; set; }
            public int nota { get; set; }

            public Realizados(int i, string c, int e, int n)
            {
                idClase = i;
                clase = c;
                noExamen = e;
                nota = n;
            }
        }

        struct porRealizar
        {
            public int id { get; set; }
            public string clase { get; set; }
            public int noExamen { get; set; }
            public int noPreguntas { get; set; }

            public porRealizar(int i,string c, int e, int p)
            {
                id = i;
                clase = c;
                noExamen = e;
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
            List<String> realizados = new List<String>();
            conn = dba.getConn();
            int i = 1;
            while (conn.KeyExists("Clase:Clase"+i))
            {
                int cont = 1;
                while (conn.KeyExists("Examen:E" + cont + "C" + i))
                {
                    if (conn.KeyExists("Resultado:A" + id + "E" + cont + "C" + i))
                    {
                        r.Add(new Realizados(Convert.ToInt32(conn.HashGet("Resultado:A" + id+"E"+cont + "C" + i, "idClase")), conn.HashGet("Resultado:A" + id+"E"+cont + "C" + i, "nombreClase"), cont,Convert.ToInt32(conn.HashGet("Resultado:A" + id+"E"+cont + "C" + i, "nota"))));
                        realizados.Add("Examen:E" + cont + "C" + i);
                    }
                    if (!realizados.Contains("Examen:E" + cont + "C" + i))
                    {
                        if (conn.KeyExists("Examen:E" + cont + "C" + i))
                            p.Add(new porRealizar(i, conn.HashGet("Clase:Clase" + i, "nombre"), cont,Convert.ToInt32(conn.HashGet("Examen:E" + cont + "C" + i, "noPreguntas"))));
                    }
                    cont++;
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
                HacerExamen he = new HacerExamen(info.id,id, info.noExamen,this);
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
                VerRespuestas vr = new VerRespuestas(id, info.noExamen,info.idClase);
                vr.Show();
            }
            else
            {
                MessageBox.Show("Debe seleccionar un examen para ver los resultados.");
            }
        }

    }
}
