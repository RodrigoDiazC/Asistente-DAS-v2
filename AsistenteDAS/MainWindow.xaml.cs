using GongSolutions.Wpf.DragDrop;
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
    public partial class MainWindow : Window, IDropTarget
    {

        private Point _lastMouseDown;
        private TreeViewItem _target;
        private TreeViewItem draggedItem;
        private TreeViewItem oldParent;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

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

        //---------------------- Metodos para mover childs de tree view

        // This event occurs when any mouse button is down. In this event, we first check the button down, then save mouse position in a variable if left button is down.

        private void Tree_Actividades_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                _lastMouseDown = e.GetPosition(tree_Actividades);
            }
        }

        // This event occurs when mouse is moved. Here first we check whether left mouse button is pressed or not. Then check the distance mouse
        // moved if it moves outside the selected treeview item, then check the drop effect if it's dragged (move) and then dropped over a TreeViewItem 
        // (i.e. target is not null) then copy the selected item in dropped item. In this event, you can put your desired condition for dropping treeviewItem.

        private void Tree_Actividades_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                draggedItem = (TreeViewItem)tree_Actividades.SelectedItem;

                if (draggedItem != null)
                {
                    foreach (TreeViewItem item in this.tree_Actividades.Items) item.IsExpanded = false;
                }

                /*
                try
                {
                    if (e.LeftButton == MouseButtonState.Pressed)
                    {
                        Point currentPosition = e.GetPosition(tree_Actividades);

                        if ((Math.Abs(currentPosition.X - _lastMouseDown.X) > 10.0) ||
                            (Math.Abs(currentPosition.Y - _lastMouseDown.Y) > 10.0))
                        {
                            draggedItem = (TreeViewItem)tree_Actividades.SelectedItem;
                            if (draggedItem != null)
                            {

                                // Minimizamos todos los nodos para que el usuario tenga una mejor vista
                                foreach (TreeViewItem item in this.tree_Actividades.Items) item.IsExpanded = false;

                                DragDropEffects finalDropEffect =
                                System.Windows.DragDrop.DoDragDrop(tree_Actividades, tree_Actividades.SelectedValue, DragDropEffects.Move);
                                //Checking target is not null and item is
                                //dragging(moving)
                                if ((finalDropEffect == DragDropEffects.Move) && (_target != null))
                                {
                                    // A Move drop was accepted
                                    if (!draggedItem.Header.ToString().Equals(_target.Header.ToString()))
                                    {
                                        // Remueve el control del parent anterior
                                        TreeViewItem old = (TreeViewItem)draggedItem.Parent;
                                        old.Items.Remove(draggedItem);

                                        // Lo agrega en este nodo
                                        _target.Items.Add(draggedItem);
                                        //CopyItem(draggedItem, _target);

                                        // Maximizamos parent de origen
                                        old.IsExpanded = true;

                                        _target = null;
                                        draggedItem = null;
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception)
                {
                }
                */
            }
        }

        // This event occurs when an object is dragged (moves) within the drop target's boundary. Here, 
        // we check whether the pointer is near a TreeViewItem or not; if near, then set Drop effect on it.

        private void Tree_Actividades_DragOver(object sender, DragEventArgs e)
        {

            //if (!(e.OriginalSource is System.Windows.Controls.TreeViewItem)) return;

            try
            {
                Point currentPosition = e.GetPosition(tree_Actividades);

                if ((Math.Abs(currentPosition.X - _lastMouseDown.X) > 10.0) || (Math.Abs(currentPosition.Y - _lastMouseDown.Y) > 10.0))
                {
                    // Verify that this is a valid drop and then store the drop target
                    contro_Actividad source = (contro_Actividad)e.Source;

                    TreeViewItem item = (TreeViewItem)source.Parent; // GetNearestContainer(e.OriginalSource as UIElement);

                    // if (CheckDropTarget(draggedItem, item))
                    if (item != null)
                    {
                        e.Effects = DragDropEffects.Move;
                    }
                    else
                    {
                        e.Effects = DragDropEffects.None;
                    }
                }
                e.Handled = true;
            }
            catch (Exception)
            {
            }
        }

        // This event occurs when an object is dropped on the drop target. Here we check whether the dropped item is dropped 
        // on a TreeViewItem or not. If yes, then set drop effect to none and the target item into a variable. And then MouseMove 
        // event completes the drag and drop operation.

        private void Tree_Actividades_Drop(object sender, DragEventArgs e)
        {
            if (!(e.Source is control_Header)) return;

            try
            {
                e.Effects = DragDropEffects.None;
                e.Handled = true;

                // Verify that this is a valid drop and then store the drop target

                // Convierte primero a control y despues adquiere el parent el cual es el treeview item que queremos
                control_Header source = (control_Header)e.Source;
                TreeViewItem TargetItem = (TreeViewItem)source.Parent;// GetNearestContainer(e.OriginalSource as UIElement);

                if (TargetItem != null && draggedItem != null)
                {
                    _target = TargetItem;
                    e.Effects = DragDropEffects.Move;
                }
            }
            catch (Exception)
            {
            }
        }


        //--- Gong Drag Drop Código para controlar el movimeinto de actividades entre nodos del tree view
        void IDropTarget.DragOver(IDropInfo dropInfo)
        {
       
            // Hacer que todos las activididades se hagan pequeñas para facilitar visibilidad

            object sourceItem = (dropInfo.Data as TreeViewItem).Header;
            object targetItem = (dropInfo.TargetItem as TreeViewItem).Header;

            // Solo es posible mover de control actividad a control header
            if (sourceItem != null && targetItem != null && sourceItem is contro_Actividad)
            {
                dropInfo.DropTargetAdorner = DropTargetAdorners.Highlight;
                dropInfo.Effects = DragDropEffects.Copy;
            }
        }

        void IDropTarget.Drop(IDropInfo dropInfo)
        {
            TreeViewItem sourceItem = dropInfo.Data as TreeViewItem;
            TreeViewItem targetItem = dropInfo.TargetItem as TreeViewItem;

            // Mueve a un nodo diferente
            if (targetItem.Header is control_Header && sourceItem.Parent != targetItem)
            {
                // Desasocia el parent anterior
                TreeViewItem parent = (TreeViewItem)sourceItem.Parent;
                parent.Items.Remove(sourceItem);
                targetItem.Items.Add(sourceItem);
                // Expande el nodo de origen de nuevo
                parent.IsExpanded = true;
            }

            // Mueve de lugar dentro del mismo nodo
            else
            {
                // Tomar acciones en la actividad
            }

        }


    }
}
