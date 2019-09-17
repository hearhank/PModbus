using Modbus.Device;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PModbus
{
    public enum PModbusStatus
    {
        Normal,
        Running,
        Faild,
        Stopped
    }
    public class StatusEventArgs : EventArgs
    {
        public StatusEventArgs(PModbusStatus status)
        {
            this.PModbusStatus = status;
        }
        public PModbusStatus PModbusStatus { get; set; }
    }
    public delegate void ModbusClienterNotify(PModbusStatus status, string message);
    public class PModbusClient : IPModbusClient
    {
        const string Name = "PModbusClienter";
        public PModbusClient(IPModbusConnection connection, IPModbusStore store)
        {
            this.Connection = connection;
            this.Store = store;
        }
        
        IPModbusStore Store;
        public IPModbusConnection Connection { get; private set; }

        public event ModbusClienterNotify Notify;
        event EventHandler<StatusEventArgs> OnStatusChanged;

        private PModbusStatus _PModbusStatus;
        public PModbusStatus PModbusStatus
        {
            get => _PModbusStatus;
            private set
            {
                if (value == PModbusStatus.Faild || _PModbusStatus != value)
                {
                    _PModbusStatus = value;
                    OnStatusChanged?.Invoke(this, new StatusEventArgs(value));
                }
            }
        }

        public Exception Exception { get; private set; }

        CancellationTokenSource source = new CancellationTokenSource();
        Task currentTask;
        bool startFlag = false;

        private int _interval = 10;

        public int Interval
        {
            get { return _interval; }
            set { _interval = value; }
        }

        public void Start()
        {
            if (startFlag)
                return;
            if (source == null)
                source = new CancellationTokenSource();

            OnStatusChanged += PModbusClienter_OnStatusChanged;
            currentTask = new Task(() =>
              {
                  Notify?.Invoke(PModbusStatus.Normal, "Communication task has started.");
                  byte slaveid = Connection.SlaveID;
                  do
                  {
                      if (source.IsCancellationRequested)
                          break;
                      doSomthing(slaveid, Interval);

                      Thread.Sleep(Interval);
                  } while (true);
              }, source.Token);
            currentTask.ContinueWith(t =>
            {
                Connection.Close();
                Notify?.Invoke(PModbusStatus.Stopped, string.Format("Communication task has been stopped.", Connection.Name));
            });

            currentTask.Start();
            startFlag = true;

        }

        private void PModbusClienter_OnStatusChanged(object sender, StatusEventArgs e)
        {
            Task.Factory.StartNew(() =>
            {
                if (_PModbusStatus == PModbusStatus.Faild && Exception != null)
                {
                    Debug.WriteLine(Exception);
                    Notify?.Invoke(PModbusStatus.Faild, string.Format("{0} communication failure.", Connection.Name));
                }
                else
                {
                    Notify?.Invoke(_PModbusStatus, string.Format("{0} communication OK.", Connection.Name));
                }
            });
        }

        IModbusMaster master;

        void doSomthing(byte slaveID, int interval = 1)
        {
            try
            {
                if (!Connection.Connect(ref master))
                {
                    PModbusStatus = PModbusStatus.Faild;//unable to connect
                    return;
                }

                while (Connection.WritesQueue.TryDequeue(out IPModbusWriteItem item))
                {
                    if (source.IsCancellationRequested)
                        break;
                    master.WriteMultipleRegisters(slaveID, item.StartAddress, item.ToArray());
                    Thread.Sleep(interval);
                }
                var EnableList = Connection.ReadList.ToArray();
                foreach (var item in EnableList)
                {
                    if (source.IsCancellationRequested)
                        break;
                    if (!item.CountTo())
                        continue;

                    ushort[] results = null;
                    if (item.ModbusDataType == Modbus.Data.ModbusDataType.InputRegister)
                    {
                        results = master.ReadInputRegisters(slaveID, item.StartAddress, item.Length);
                    }
                    else if (item.ModbusDataType == Modbus.Data.ModbusDataType.HoldingRegister)
                    {
                        results = master.ReadHoldingRegisters(slaveID, item.StartAddress, item.Length);
                    }
                    else
                    {
                        throw new NotSupportedException(item.ModbusDataType.ToString());
                    }
                    if (results != null && results.Length > 0)
                    {
                        if (item.StoreAction != null)
                        {
                            item.StoreAction(results);
                        }
                        else
                        {
                            Store.Store(item.ModbusDataType, item.StartAddress, results);
                        }
                    }
                    Thread.Sleep(interval);
                }

                if (Exception != null)
                    Exception = null;

                if (PModbusStatus != PModbusStatus.Running)
                    PModbusStatus = PModbusStatus.Running;
            }
            catch (Exception ex)
            {
                Exception = ex;
                PModbusStatus = PModbusStatus.Faild;
                Thread.Sleep(100);
            }
        }
        public void Stop()
        {
            if (startFlag)
            {
                source.Cancel();
                Task.WaitAll(new Task[] { currentTask });
                startFlag = false;
                source = null;
                OnStatusChanged -= PModbusClienter_OnStatusChanged;
            }
        }

        public void Write(ushort offset, IEnumerable<ushort> datas)
        {
            Connection?.AddToWrite(new PModbusWriteItem(offset, datas));
        }

        public void ChangeDevice(IConnectDevice device)
        {
            Stop();
            Connection = new PModbusConnection(device);
            Start();
        }
    }
}
