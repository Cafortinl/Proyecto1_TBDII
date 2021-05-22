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

namespace Proyecto1_TBDII
{
    /// <summary>
    /// Interaction logic for AlumnoWindow.xaml
    /// </summary>
    public partial class AlumnoWindow : Window
    {
        int id = -1;
        public AlumnoWindow(int x)
        {
            id = x;
            InitializeComponent();
        }
    }
}
