using System;
using System.Text;
using System.Collections.Generic;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.IO;
using System.IO.Pipes;
using System.Threading;
using System.Windows.Threading;

namespace SharpBrain.UI
{
    public delegate void RegisterErrorRateHandler(double aError);

    public class MainViewModel : INotifyPropertyChanged
    {
        double mAreaWidth = 500;
        double mAreaHeight = 500;

        bool mErrorUpdate;
        double mLastRecordedError;

        const string mNamedPipe = "SharpBrain";

        PanelObject mSelectedObject;

        ObservableCollection<Neuron> mNeurons;
        ObservableCollection<Connection> mConnections;

        RegisterErrorRateHandler mRegisterErrorRate;
        public event PropertyChangedEventHandler PropertyChanged;

        #region Property
        public bool CreatingConnection { get; set; }
        public ObservableCollection<Neuron> Neurons
        {
            get { return mNeurons ?? (mNeurons = new ObservableCollection<Neuron>()); }
        }
        public ObservableCollection<Connection> Connections
        {
            get { return mConnections ?? (mConnections = new ObservableCollection<Connection>()); }
        }
        public double AreaHeight
        {
            get { return mAreaHeight; }
            set
            {
                mAreaHeight = value;
                OnPropertyChanged("AreaHeight");
            }
        }
        
        public double AreaWidth
        {
            get { return mAreaWidth; }
            set
            {
                mAreaWidth = value;
                OnPropertyChanged("AreaWidth");
            }
        }

        public PanelObject SelectedObject
        {
            get { return mSelectedObject; }
            set
            {
                mSelectedObject = value;
                OnPropertyChanged("SelectedObject");
            }
        }

        public string NamedPipe
        {
            get { return mNamedPipe; }
        }
        #endregion

        public MainViewModel(RegisterErrorRateHandler aEventHandler)
        {
            mRegisterErrorRate = aEventHandler;
            mNeurons = new ObservableCollection<Neuron>();
            mConnections = new ObservableCollection<Connection>();
            StartServer();

            var graphUpdater = new DispatcherTimer();
            graphUpdater.Tick += DispatcherTimerTick;
            graphUpdater.Interval = new TimeSpan(1000);
            graphUpdater.Start();
        }
        
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        private void DispatcherTimerTick(object sender, EventArgs e)
        {
            //this is the best hack that i can think of right now
            //it's 5.30 am here and brain is not working
            //life sucks so this hack
            //i hope you will understand my situation @reader @compiler @code
            if (mRegisterErrorRate != null && mErrorUpdate)
            {
                mErrorUpdate = false;
                mRegisterErrorRate(mLastRecordedError);
            }
        }

        private void StartServer()
        {
            new Thread(() =>
            {
                try
                {
                    var server = new NamedPipeServerStream(mNamedPipe);
                    server.WaitForConnection();
                    var reader = new StreamReader(server);
                    var writer = new StreamWriter(server);

                    var request = new Dictionary<string, double>();
                    var outputNeuron = new List<Neuron>();
                    var stringBuilder = new StringBuilder();

                    while (true)
                    {
                        var line = reader.ReadLine();

                        if (line == null)
                        {
                            //you have to wait if you hurt me ^_^ 3:)
                            Thread.Sleep(500);
                            if (!server.IsConnected)
                            {
                                server.Close();
                                GC.Collect();
                                StartServer();
                                break;
                            }
                            continue;
                        }

                        var args = line.Split(' ');
                        foreach (var arg in args)
                        {
                            int separator = arg.IndexOf('=');
                            if (separator != -1)
                                request.Add(arg.Substring(0, separator), double.Parse(arg.Substring(separator + 1)));
                            else
                                request.Add(arg, 0);
                        }

                        //feeding first
                        foreach (var neuron in mNeurons)
                        {
                            var model = neuron.NeuronModel;

                            if (model.ChildrenCount == 0)
                            {
                                outputNeuron.Add(neuron);
                                continue;
                            }

                            if (request.ContainsKey(neuron.Key))
                                model.Feed(null, request[neuron.Key]);
                        }

                        double TotalError = 0;
                        bool IsThisTraining = false;
                        if (outputNeuron.Count > 0)
                        {
                            foreach (var neuron in outputNeuron)
                            {
                                var model = neuron.NeuronModel;

                                if (request.ContainsKey(neuron.Key))
                                {
                                    double error = request[neuron.Key] - model.Output;
                                    TotalError += Math.Abs(error);
                                    model.Propagate(null, error);

                                    IsThisTraining = true;
                                }
                                else
                                    stringBuilder.AppendFormat("{0}={1} ", neuron.Key, model.Output);
                            }
                        }

                        if (IsThisTraining)
                        {
                            mErrorUpdate = true;
                            mLastRecordedError = TotalError;
                        }

                        if (request.ContainsKey("-e"))
                            stringBuilder.Append(TotalError);

                        if (stringBuilder.Length != 0)
                        {
                            writer.WriteLine(stringBuilder.ToString().TrimEnd());
                            writer.Flush();

                            stringBuilder.Clear();
                        }

                        request.Clear();
                        outputNeuron.Clear();
                    }
                }
                catch { }//I really don't care what you throw me because I can handle anything :D
            })
            { IsBackground = true }.Start();
        }
    }
}
