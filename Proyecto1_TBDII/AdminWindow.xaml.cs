﻿using System;
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
        int noClases = -1;
        IDatabase conn;
        public AdminWindow()
        {
            InitializeComponent();
            setNoClases();
        }

        public void setNoClases()
        {
            int i = 0;
            conn = dba.getConn();
            while (conn.KeyExists("Clase:Clase"+i))
            {
                i++;
            }
            noClases = i;
        }

        private void btCrearClase_Click(object sender, RoutedEventArgs e)
        {
            setNoClases();
            CrearClase cc = new CrearClase(noClases);
            cc.Show();
        }
    }
}
