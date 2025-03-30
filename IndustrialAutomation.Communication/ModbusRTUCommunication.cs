using Modbus.Device;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Modbus;
using System.Windows.Forms;
namespace IndustrialAutomationApp.IndustrialAutomation.Communication
{
    public class ModbusRTUCommunication
    {
        private IModbusSerialMaster _modbusMaster; // IModbusSerialMaster 是 NModbus4 中用于 Modbus RTU 通信的接口
        private SerialPort _serialPort;

        // 配置参数
        public string PortName { get; set; } = "COM1";
        public int BaudRate { get; set; } = 9600;
        public int DataBits { get; set; } = 8;
        public StopBits StopBits { get; set; } = StopBits.One;
        public Parity Parity { get; set; } = Parity.None;

        // 构造函数
        public ModbusRTUCommunication()
        {
            _serialPort = new SerialPort
            {
                PortName = PortName,
                BaudRate = BaudRate,
                DataBits = DataBits,
                StopBits = StopBits,
                Parity = Parity
            };
        }    
        public event Action<string> OnStatusChanged;
        // 初始化通信
        public bool Initialize()
        {
            try
            {
                _serialPort.Open();
                _modbusMaster = ModbusSerialMaster.CreateRtu(_serialPort);
                OnStatusChanged?.Invoke("连接成功！");
                return true;
            }
            catch (Exception ex)
            {
                OnStatusChanged?.Invoke($"初始化失败: {ex.Message}");
                return false;
            }
        }

        // 关闭通信
        public void Close()
        {
            if (_serialPort.IsOpen)
            {
                _serialPort.Close();
                OnStatusChanged?.Invoke("连接已关闭");
            }
        }

        // 读取保持寄存器
        public ushort[] ReadHoldingRegisters(byte slaveAddress, ushort startAddress, ushort numOfPoints)
        {
            try
            {
                return _modbusMaster.ReadHoldingRegisters(slaveAddress, startAddress, numOfPoints);
            }
            catch (Exception ex)
            {
                OnStatusChanged?.Invoke($"读取保持寄存器失败: {ex.Message}");
                return null;
            }
        }

        // 读取离散输入
        public bool[] ReadDiscreteInputs(byte slaveAddress, ushort startAddress, ushort numOfPoints)
        {
            try
            {
                return _modbusMaster.ReadInputs(slaveAddress, startAddress, numOfPoints);
            }
            catch (Exception ex)
            {
                OnStatusChanged?.Invoke($"读取输入寄存器失败: {ex.Message}");
                return null;
            }
        }

        // 写单个线圈
        public void WriteSingleCoil(byte slaveAddress, ushort coilAddress, bool value)
        {
            try
            {
                _modbusMaster.WriteSingleCoil(slaveAddress, coilAddress, value);
                
            }
            catch (Exception ex)
            {
                OnStatusChanged?.Invoke($"写寄存器失败: {ex.Message}");
            }
        }

        // 读取输入寄存器
        public ushort[] ReadInputRegisters(byte slaveAddress, ushort startAddress, ushort numOfPoints)
        {
            try
            {
                return _modbusMaster.ReadInputRegisters(slaveAddress, startAddress, numOfPoints);
            }
            catch (Exception ex)
            {
                OnStatusChanged?.Invoke($"读取输入寄存器失败: {ex.Message}");
                return null;
            }
        }

        // 写单个寄存器
        public void WriteSingleRegister(byte slaveAddress, ushort registerAddress, ushort value)
        {
            try
            {
                _modbusMaster.WriteSingleRegister(slaveAddress, registerAddress, value);
                OnStatusChanged?.Invoke($"成功写入寄存器 {registerAddress} 值 {value}");
            }
            catch (Exception ex)
            {
                OnStatusChanged?.Invoke($"写寄存器失败: {ex.Message}");
            }
        }

       
        
    }
}
