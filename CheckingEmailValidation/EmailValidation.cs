using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Net.Sockets;
using ARSoft.Tools.Net.Dns;

namespace CheckingEmailValidation
{
    class EmailValidation
    {
        private byte[] BytesFromString(string str)
        {
            return Encoding.ASCII.GetBytes(str);
        }

        private int GetResponseCode(string ResponseString)
        {
            return int.Parse(ResponseString.Substring(0, 3));
        }

        string provider = " ";

        public bool CheckRecord(string domain)
        {
            var dnsResolver = new DnsStubResolver();
            var mxRecords = dnsResolver.Resolve<MxRecord>(domain, RecordType.Mx);
            var priority = mxRecords.ToArray().Min().Preference;

            //check priority if there is any MXrecords
     
            if (mxRecords.Count == 0)
            {
                return false;
            }
            else
            {
                foreach (var record in mxRecords)
                {
                    Console.WriteLine(record.Preference + " " + record.ExchangeDomainName?.ToString());
                }

                for (int i = 0; i < mxRecords.Count; i++)
                {
                    if (mxRecords[i].Preference == priority)
                    {
                        provider = mxRecords[i].ExchangeDomainName?.ToString();
                    }
                }
                return true;
            }
        }        

        public void ValidateMail(string email)
        {
            string crlf = "\r\n";
            byte[] dataBuffer;
            string ResponseString;

            TcpClient tclient = new TcpClient(provider, 25);
            NetworkStream netStream = tclient.GetStream();
            StreamReader reader = new StreamReader(netStream);
            ResponseString = reader.ReadLine();

            dataBuffer = BytesFromString("HELO KirtanHere" + crlf);
            netStream.Write(dataBuffer, 0, dataBuffer.Length);
            ResponseString = reader.ReadLine();
            Console.WriteLine("\n" + ResponseString);

            dataBuffer = BytesFromString("MAIL FROM:<khaledanika303@gmail.com>" + crlf);
            netStream.Write(dataBuffer, 0, dataBuffer.Length);
            ResponseString = reader.ReadLine();
            Console.WriteLine("\nMAIL FROM:<khaledanika303@gmail.com>\n");
            Console.WriteLine(ResponseString);

            dataBuffer = BytesFromString("RCPT TO:<" + email + ">" + crlf);
            netStream.Write(dataBuffer, 0, dataBuffer.Length);
            Console.WriteLine("\nRCPT TO:<" + email + ">");
            netStream.Write(dataBuffer, 0, dataBuffer.Length);
            ResponseString = reader.ReadLine();

            if (GetResponseCode(ResponseString) == 250)
            {
                Console.WriteLine("\nThe Email is correct!");
            }

            else if (GetResponseCode(ResponseString) == 550)
            {
                Console.WriteLine("Email does not exist !!!");
            }

            dataBuffer = BytesFromString("QUITE" + crlf);
            netStream.Write(dataBuffer, 0, dataBuffer.Length);
            tclient.Close();
        }   
    }
}
