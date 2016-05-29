using System.Linq;
using System.Windows.Controls.Primitives;
using MahApps.Metro.Controls;
using System.Windows.Controls;

using System.Windows;
using System.Windows.Input;

using SharpBrain.UI;
using SharpBrain.UI.Controls;

namespace SharpBrain
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        MainViewModel CurrentDataContext;

        public MainWindow()
        {
            InitializeComponent();
            
            CurrentDataContext = new MainViewModel(delegate(double aError)
            {
                errorChart.ErrorStat.Add(ErrorSample.Generate(aError));
            });
            DataContext = CurrentDataContext;
        }

        private void Thumb_Drag(object sender, DragDeltaEventArgs e)
        {
            var thumb = sender as Thumb;
            if (thumb == null)
                return;

            var neuron = thumb.DataContext as Neuron;
            if (neuron == null)
                return;

            neuron.X += e.HorizontalChange;
            neuron.Y += e.VerticalChange;
        }

        private void ListBox_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            var listbox = sender as ListBox;

            if (listbox == null)
                return;

            var vm = CurrentDataContext;

            if (vm.SelectedObject != null && vm.SelectedObject.IsFloating)
            {
                if (vm.SelectedObject is Neuron)
                {
                    vm.SelectedObject.X = e.GetPosition(listbox).X;
                    vm.SelectedObject.Y = e.GetPosition(listbox).Y;
                }
                else
                {
                    var node = GetNodeUnderMouse();
                    if (node == null)
                        return;

                    var connector = vm.SelectedObject as Connection;

                    if (connector.Start != null && node != connector.Start && connector.End != node)
                    {
                        if (connector.End != null)
                            connector.End.IsHighlighted = false;
                        node.IsHighlighted = true;
                        connector.End = node;
                    }
                }
            }
        }

        private void ListBox_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            var vm = CurrentDataContext;

            if (CurrentDataContext.CreatingConnection)
            {
                var neuron = GetNodeUnderMouse();
                var connector = vm.SelectedObject as Connection;
                if (neuron != null && connector != null)
                {
                    e.Handled = true;
                    if (connector.Start == null)
                    {
                        connector.Start = neuron;
                        neuron.IsHighlighted = true;
                        return;
                    }
                    else if (connector.End != null)
                    {
                        connector.Start.IsHighlighted = false;
                        connector.End.IsHighlighted = false;
                        connector.Glue();
                        CurrentDataContext.CreatingConnection = false;
                    }
                }
            }

            if (vm.SelectedObject != null)
                vm.SelectedObject.IsFloating = false;
        }

        private void CreateNeuron(object sender, RoutedEventArgs e)
        {
            doEscape();

            var newNeuron = new Neuron();
            CurrentDataContext.Neurons.Add(newNeuron);
            CurrentDataContext.SelectedObject = newNeuron;
        }

        private void CreateLink(object sender, RoutedEventArgs e)
        {
            var obj = CurrentDataContext.SelectedObject;
            var startNode = obj != null ? (obj as Neuron) : null;

            doEscape();

            var newLink = new Connection()
            {
                Start = startNode
            };

            if (startNode != null)
                startNode.IsHighlighted = true;

            CurrentDataContext.CreatingConnection = true;
            CurrentDataContext.Connections.Add(newLink);
            CurrentDataContext.SelectedObject = newLink;
        }

        private void doEscape()
        {
            var obj = CurrentDataContext.SelectedObject;
            if (obj != null && obj.IsFloating)
            {
                if (obj is Neuron)
                    CurrentDataContext.Neurons.Remove((Neuron)obj);
                else if (obj is Connection)
                {
                    var connectionObj = (Connection)obj;
                    CurrentDataContext.Connections.Remove(connectionObj);
                    if (connectionObj.Start != null)
                        connectionObj.Start.IsHighlighted = false;
                    if (connectionObj.End != null)
                        connectionObj.End.IsHighlighted = false;
                }
                CurrentDataContext.CreatingConnection = false;
            }

            if (obj is Neuron)
                ((Neuron)obj).IsHighlighted = false;
            CurrentDataContext.SelectedObject = null;
        }
        
        private void ListBox_KeyDown(object sender, KeyEventArgs e)
        {
            switch(e.Key)
            {
                case Key.Escape:
                    doEscape();
                    break;
                case Key.N:
                    CreateNeuron(sender, null);
                    break;
                case Key.L:
                    CreateLink(sender, null);
                    break;
                case Key.Delete:
                    {
                        var datacontext = CurrentDataContext;
                        var vm = datacontext.SelectedObject;
                        if (vm is Neuron)
                        {
                            var obj = (Neuron)vm;
                            datacontext.Neurons.Remove(obj);
                            for (int i = 0; i < datacontext.Connections.Count; i++)   
                            {
                                var connection = datacontext.Connections[i];
                                if (connection.Start == obj)
                                {
                                    datacontext.Connections.RemoveAt(i);
                                    obj.NeuronModel.Disconnect(connection.End.NeuronModel);
                                    i--;
                                }
                                else if (connection.End == obj)
                                {
                                    datacontext.Connections.RemoveAt(i);
                                    i--;
                                }
                            }
                        }
                        else if (vm is Connection)
                        {
                            var obj = (Connection)vm;
                            datacontext.Connections.Remove(obj);
                            obj.Start.NeuronModel.Disconnect(obj.End.NeuronModel);
                        }
                        datacontext.SelectedObject = null;
                    }
                    break;
            }
        }

        private Neuron GetNodeUnderMouse()
        {
            var item = Mouse.DirectlyOver as ContentPresenter;
            if (item == null)
                return null;

            return item.DataContext as Neuron;
        }
    }
}
