using IndustrialAutomationApp.IndustrialAutomation.Communication;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IndustrialAutomationApp
{
    public partial class MainForm : Form
    {
        private ModbusRTUCommunication _modbusRTU;

        public MainForm()
        {
            InitializeComponent();
            InitializeModbusSettings();
            _modbusRTU = new ModbusRTUCommunication();
            // 订阅事件
            _modbusRTU.OnStatusChanged += ModbusRTU_OnStatusChanged;
        }

        // UI 中的状态更新
        private void ModbusRTU_OnStatusChanged(string statusMessage)
        {
            // 更新 UI 状态标签
            lblStatus.Invoke((MethodInvoker)(() =>
            {
                lblStatus.Text = statusMessage;
            }));
        }


        // 初始化界面上的通信参数
        private void InitializeModbusSettings()
        {
            // 填充串口选择
            foreach (var port in System.IO.Ports.SerialPort.GetPortNames())
            {
                comboBoxPort.Items.Add(port);
            }
            comboBoxPort.SelectedIndex = 0;

            // 设置波特率选项
            comboBoxBaudRate.Items.Add(9600);
            comboBoxBaudRate.Items.Add(19200);
            comboBoxBaudRate.Items.Add(38400);
            comboBoxBaudRate.Items.Add(57600);
            comboBoxBaudRate.Items.Add(115200);
            comboBoxBaudRate.SelectedIndex = 0;
        }

        // 连接按钮点击事件
        private void btnConnect_Click(object sender, EventArgs e)
        {
            // 设置通信参数
            _modbusRTU.PortName = comboBoxPort.SelectedItem.ToString();
            _modbusRTU.BaudRate = int.Parse(comboBoxBaudRate.SelectedItem.ToString());
            _modbusRTU.DataBits = 8;
            _modbusRTU.StopBits = System.IO.Ports.StopBits.One;
            _modbusRTU.Parity = System.IO.Ports.Parity.None;

            // 初始化 Modbus RTU 通信
            if (_modbusRTU.Initialize())
            {
                lblStatus.Text = "连接成功！";
                MessageBox.Show("连接成功！");
            }
            else
            {
                lblStatus.Text = "连接失败！";
                MessageBox.Show("连接失败！");
            }
        }

        // 读取保持寄存器按钮点击事件
        private void btnReadRegisters_Click(object sender, EventArgs e)
        {

            if (_modbusRTU != null)
            {
                // 读取寄存器，假设从地址 0 开始读取 10 个寄存器
                var result = _modbusRTU.ReadHoldingRegisters(1, 0, 10);
                if (result != null)
                {
                    // 显示读取的数据
                    dataGridView1.Rows.Clear();
                    foreach (var reg in result)
                    {
                        dataGridView1.Rows.Add(reg);
                    }
                    lblStatus.Text = "读取成功！";
                }
                else
                {
                    MessageBox.Show("读取失败！");
                }
            }
        }

        // 读取输入寄存器按钮点击事件
        private void btnReadInputRegisters_Click(object sender, EventArgs e)
        {
            if (_modbusRTU != null)
            {
                // 假设从地址 0 开始读取 10 个输入寄存器
                var result = _modbusRTU.ReadInputRegisters(1, 0, 10);
                if (result != null)
                {
                    // 显示读取的数据
                    dataGridView1.Rows.Clear();
                    foreach (var reg in result)
                    {
                        dataGridView1.Rows.Add(reg);
                    }
                }
                else
                {
                    MessageBox.Show("读取失败！");
                }
            }
        }

        // 写单个寄存器按钮点击事件
        private void btnWriteSingleRegister_Click(object sender, EventArgs e)
        {
            if (_modbusRTU != null)
            {
                // 假设写入寄存器地址为 1，值为 100
                _modbusRTU.WriteSingleRegister(1, 1, 100);// 示例：寄存器地址1，值100
                MessageBox.Show("写入成功！");
            }
        }


    }
}