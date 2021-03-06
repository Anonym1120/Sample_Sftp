using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using ClassLibrary_Sftp_RenciSshNet;

namespace ConsoleApp_Sftp_Put
{
    class Program
    {
        static void Main(string[] args)
        {
            string host = ConfigurationManager.AppSettings["host"].ToString();
            int port = Convert.ToInt32(ConfigurationManager.AppSettings["port"].ToString());
            string username = ConfigurationManager.AppSettings["username"].ToString();
            string password = ConfigurationManager.AppSettings["password"].ToString();
            string localPath = ConfigurationManager.AppSettings["localPath"].ToString();
            string remotePath = ConfigurationManager.AppSettings["remotePath"].ToString();
            string fileName = @"FileName" + DateTime.Now.ToString("yyyyMMdd") + @".txt";

            SFTP sftp = new SFTP(host, port, username, password);
            sftp.Connect();
            sftp.Put(localPath, remotePath, fileName);
            sftp.Disconnect();
        }
    }
}
