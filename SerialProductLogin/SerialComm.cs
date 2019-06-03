using System;
using System.Configuration;
using System.IO.Ports;
using System.Threading;

namespace SerialProductLogin
{
    public class SerialComm
    {
        static string comPortIn = ConfigurationManager.AppSettings.Get("ComPortIn");
        static string comPortOut = ConfigurationManager.AppSettings.Get("ComPortOut");

        public string terminatorIn1 = ConfigurationManager.AppSettings.Get("TerminatorIn1");
        public string terminatorIn2 = ConfigurationManager.AppSettings.Get("TerminatorIn2");
        public string terminatorOut1 = ConfigurationManager.AppSettings.Get("TerminatorOut1");
        public string terminatorOut2 = ConfigurationManager.AppSettings.Get("TerminatorOut2");

        SerialPort serialIn = new SerialPort(comPortIn);
        SerialPort serialOut = new SerialPort(comPortOut);

        public SerialComm()
        {


            serialIn.BaudRate = 9600;
            serialIn.Parity = Parity.None;
            serialIn.StopBits = StopBits.One;
            serialIn.DataBits = 8;
            serialIn.Handshake = Handshake.None;
            serialIn.RtsEnable = true;



            serialOut.BaudRate = 9600;
            serialOut.Parity = Parity.None;
            serialOut.StopBits = StopBits.One;
            serialOut.DataBits = 8;
            serialOut.Handshake = Handshake.None;
            serialOut.RtsEnable = true;
            serialOut.WriteTimeout = 5000;


        }

        public string Read()
        {

            string indata;
            //Console.WriteLine(terminatorIn1 + terminatorIn2);
            try
            {
                serialIn.Open();
                Console.WriteLine("Waiting for barcode scan...");
                indata = serialIn.ReadTo(terminatorIn1 + terminatorIn2);
                Console.WriteLine($"Received from {serialIn.PortName}: {indata}");
                return indata;

            }
            catch (Exception ex)
            {
                string returnError = "error";
                Console.WriteLine(ex.Message);
                return returnError;
            }
            finally
            {
                serialIn.Close();
            }
        }

        public void Send(string data)
        {
            serialOut.Open();


            string outdata = data + terminatorOut1 + terminatorOut2;
            try
            {
                serialOut.Write(outdata);
                Console.WriteLine($"Sent data on {serialOut.PortName}: {outdata}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Could not send data on {serialOut.PortName}. {ex.Message} \n");
            }
            finally
            {
                serialOut.Close();
            }



        }

    }

}
