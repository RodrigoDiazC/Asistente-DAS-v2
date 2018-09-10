using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AsistenteDAS
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            for (int j = 0; j < 10; j++)
            {
                TreeViewItem act = new TreeViewItem();
                act.Header = new control_Header("Dia" + j.ToString() + " de abril");

                for (int i = 0; i < 3; i++)
                {
                    TreeViewItem act2 = new TreeViewItem();
                    act2.Header = new contro_Actividad();
                    act.Items.Add(act2);
                }

                this.tree_Actividades.Items.Add(act);
            }

        }

        //---- Habilita poder mover la ventana
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        //--- Guarda la lista cuando se cierra la aplicación
        private void button_Cerrar_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
