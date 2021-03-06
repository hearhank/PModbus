# PModbus

> 对NModbus4的封装。简单配置后即可进行多线程下的数据读写

### Features

1. 读写数据方便
2. 连接状态通知
3. 可以分组修改读取数据列表
4. 断开自动年重连
5. 可以设置延迟读取数据轮次

~~~C#
PModbusClient client;
~~~

~~~C#
private void ButtonStart_Click(object sender, EventArgs e)
{ 
    if (client == null)
     {
         //添加RS232通信
         var cc = new PModbusConnection(new SerialPortDevice()
         {
             ComPort = "COM1",
             BandRate = 115200,
             Timeout = 1000,
         });                               
         //添加需要读取的数据
     	cc.AddToRead(new PModbusReadItem(0, 100, PModbusType.Input) { GroupID = 1, Enabled = false });
        cc.AddToRead(new PModbusReadItem(0, 100, PModbusType.Hold));

         //添加数据存储类，并实例化PModbusClient
         client = new PModbusClient(cc, new PModbusStore(ref AppData.IDatas, ref AppData.ODatas));
         //注册状态改变的通知事件
         client.Notify += Clienter_Notify;
         //设置数据读写的间隔
         client.Interval=10;
         //启动任务
         client.Start();
		
         Debug.WriteLine(client.Connection.ToString());
     }
}
~~~

~~~c#
 private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
 {//程序关闭时，停止任务并释放资源
     if (client != null)
     	client.Stop();
 }
~~~

~~~C#
//指定的位置写入数据
client.Write(0,new ushort[] { 1});
~~~

