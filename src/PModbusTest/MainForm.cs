using PModbus;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PModbusTest
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }
        PModbusClient client;
        private void buttonStart_Click(object sender, EventArgs e)
        {
            if (client == null)
            {
                //Debug.WriteLine("Start");
                var cc = new PModbusConnection(new SerialPortDevice()
                {
                    ComPort = "COM1",
                    BandRate = 115200,
                    Timeout = 1000,
                });
                cc.AddToRead(new PModbusItem(0, 100, PModbusType.Input));
                cc.AddToRead(new PModbusItem(0, 100, PModbusType.Hold));

                client = new PModbusClient(cc, new PModbusStore(ref AppData.IDatas, ref AppData.ODatas));
                client.Notify += Clienter_Notify;
                client.Start();

                Debug.WriteLine(client.Connection.ToString());
            }
            buttonStart.Enabled = false;
            button3.Enabled = false;
            button2.Enabled = true;
        }
        private void Clienter_Notify(PModbusStatus status, string message)
        {
            if (status != PModbusStatus.Normal)
            {
                Debug.WriteLine("=================Status and Message:");
                Debug.WriteLine(">>" + status);
                Debug.WriteLine(">>" + message);
                this.Invoke(new Action(() =>
                {
                    toolStripStatusLabel1.Text = message;
                    toolStripStatusLabel2.Text = status.ToString();
                }));
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            if (client == null)
            {
                //Debug.WriteLine("Start");
                var cc = new PModbusConnection(new EthernetDevice()
                {
                    IPAddr = "192.168.1.170",
                    Timeout = 1000,
                });
                cc.AddToRead(new PModbusItem(0, 100, PModbusType.Input));
                cc.AddToRead(new PModbusItem(0, 100, PModbusType.Hold));

                client = new PModbusClient(cc, new PModbusStore(ref AppData.IDatas, ref AppData.ODatas));
                client.Notify += Clienter_Notify;
                client.Start();
                
                Debug.WriteLine(client.Connection.ToString());
            }

            buttonStart.Enabled = false;
            button3.Enabled = false;
            button2.Enabled = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (client != null)
            {
                client.Stop();
                //Debug.WriteLine("Stop");
                client = null;
            }
            buttonStart.Enabled = true;
            button3.Enabled = true;
            button2.Enabled = false;
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (client != null)
                client.Stop();
        }
    }

    public class AppData
    {
        public static ushort[] IDatas = new ushort[1000];
        public static ushort[] ODatas = new ushort[1000];
    }
}
