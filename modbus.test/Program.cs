using bx.modbus;
using System;
using System.Collections;

namespace modbus.test
{
    class Program
    {
        static ModbusServer ms;
        static void Main(string[] args)
        {
            client();
           
        }

        private static void client()
        {
            ModbusClient modbusClient = new ModbusClient();
            modbusClient.Connect("192.168.1.88", 502);
            int[] UPSStatus_BF = modbusClient.ReadHoldingRegisters(40001, 1);
            

            modbusClient.Disconnect();



        }

        private static void StartServer(string comport, byte unit)
        {
            ms = new ModbusServer();
            ms.SerialFlag = true;
            ms.Baudrate = 9600;
            ms.Parity = System.IO.Ports.Parity.None;
            ms.SerialPort = "COM" + comport;
            ms.UnitIdentifier = unit;
            ms.numberOfConnectedClientsChanged += Ms_numberOfConnectedClientsChanged;
            ms.logDataChanged += Ms_logDataChanged;
            ms.holdingRegistersChanged += Ms_holdingRegistersChanged;
            ms.Listen();
        }

        private static void Ms_holdingRegistersChanged(int register, int numberOfRegisters)
        {
            try
            {
                Console.WriteLine("HR: " + register.ToString());
            }
            catch (Exception)
            {
                Console.Write("HR Error");
            }

        }

        private static void Ms_logDataChanged()
        {
            try
            {
                Console.WriteLine("Modbus Function: " + ms.ModbusLogData[0].functionCode.ToString());
                Console.WriteLine("QT: " + ms.ModbusLogData[0].quantity.ToString());
                Console.WriteLine("QT Write: " + ms.ModbusLogData[0].quantityWrite.ToString());
                Console.WriteLine("QT Read: " + ms.ModbusLogData[0].quantityRead.ToString());
                Console.WriteLine("Addr: " + ms.ModbusLogData[0].startingAdress.ToString());
                Console.WriteLine("WR Address: " + ms.ModbusLogData[0].startingAddressWrite.ToString());
                Console.WriteLine("RD Address: " + ms.ModbusLogData[0].startingAddressRead.ToString());
                if (ms.ModbusLogData[0].receiveRegisterValues != null)
                    Console.WriteLine("WR Values: " + string.Join(",", ms.ModbusLogData[0].receiveRegisterValues));
                if (ms.ModbusLogData[0].sendRegisterValues != null)
                    Console.WriteLine("RD VAlues: " + string.Join(",", ms.ModbusLogData[0].sendRegisterValues));
                Console.WriteLine("Error: " + ms.ModbusLogData[0].errorCode.ToString());
                Console.WriteLine("Exception: " + ms.ModbusLogData[0].exceptionCode.ToString());

            }
            catch (Exception)
            {
                Console.Write("Log Error");
            }


        }

        private static void Ms_numberOfConnectedClientsChanged()
        {
            try
            {
                Console.WriteLine("Oh Look... someone connected");
            }
            catch (Exception)
            {
                Console.Write("Connection Error");
            }

        }
    }
}
