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
    /// Interaction logic for AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        DBA dba = new DBA("127.0.0.1:6379,password=1");
        int noClases = 0, noPreguntas = 0;
        IDatabase conn;
        public AdminWindow()
        {
            InitializeComponent();
            setNoClases();
            updateTable();
        }

        struct InfoAdmin
        {
            public int idClase { get; set; }
            public string nombreClase { get; set; }
            public bool tieneExamen { get; set; }
            public int noPreguntas { get; set; }

            public InfoAdmin(int id, string n, bool e, int p)
            {
                idClase = id;
                nombreClase = n;
                tieneExamen = e;
                noPreguntas = p;
            }
        }

        public void setNoClases()
        {
            int i = 1;
            conn = dba.getConn();
            while (conn.KeyExists("Clase:Clase"+i))
            {
                noClases = i;
                i++;
            }
        }

        public void setNoPreguntas(int x)
        {
            int i = 1;
            noPreguntas = 0;
            conn = dba.getConn();
            while (conn.KeyExists("Pregunta:C" + x + "P" + i))
            {
                noPreguntas = i;
                i++;
            }
        }

        private void btCrearClase_Click(object sender, RoutedEventArgs e)
        {
            setNoClases();
            CrearClase cc = new CrearClase(noClases, this);
            cc.Show();
        }

        private void btCrearExamen_Click(object sender, RoutedEventArgs e)
        {
            int index = dgAdmin.SelectedIndex;
            if (index > -1)
            {
                DataGridRow row = dgAdmin.ItemContainerGenerator.ContainerFromIndex(index) as DataGridRow;
                var info = dgAdmin.ItemContainerGenerator.ItemFromContainer(row);
            }
            else
            {
                MessageBox.Show("Debe seleccionar una clase para crear un exámen.");
            }
        }

        public void updateTable()
        {
            setNoClases();
            if(noClases > 0)
            {
                conn = dba.getConn();
                List<InfoAdmin> ia = new List<InfoAdmin>();
                for (int i = 1;i <= noClases;i++)
                {
                    setNoPreguntas(i);
                    ia.Add(new InfoAdmin(i,conn.HashGet("Clase:Clase"+i,"nombre"),conn.KeyExists("Examen:ExamenC"+i),noPreguntas));
                }
                dgAdmin.ItemsSource = ia;
                dgAdmin.IsReadOnly = true;
            }
        }
    }
}
