using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Renci.SshNet;

namespace ClassLibrary_Sftp_RenciSshNet
{
    public class SFTP
    {
        private SftpClient sftp;

        /// <summary>
        /// 連線資料
        /// </summary>
        /// <param name="host">伺服器位址(IP)</param>
        /// <param name="port">伺服器連接埠號</param>
        /// <param name="username">伺服器使用者名稱</param>
        /// <param name="password">伺服器使用者密碼</param>
        public SFTP(string host, int port, string username, string password)
        {
            this.sftp = new SftpClient(host, port, username, password);
        }

        /// <summary>
        /// 連線狀態
        /// </summary>
        public bool Connected
        {
            get { return this.sftp.IsConnected; }
        }

        /// <summary>
        /// 連線
        /// </summary>
        /// <returns></returns>
        public bool Connect()
        {
            bool result = false;
            try
            {
                bool flag = !this.Connected;
                if (flag == true)
                {
                    this.sftp.Connect();
                }
                result = true;
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("連接SFTP失敗，原因:{0}", ex.Message));
            }
            return result;
        }

        /// <summary>
        /// 中斷連線
        /// </summary>
        public void Disconnect()
        {
            try
            {
                bool flag = this.sftp != null && this.Connected;
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("中斷SFTP連線，失敗，原因:{0}", ex.Message));
            }
        }

        /// <summary>
        /// 上傳文件
        /// </summary>
        /// <param name="localPath">本地路徑</param>
        /// <param name="remotePath">伺服器路徑</param>
        /// <param name="fileName">檔案名稱</param>
        public void Put(string localPath, string remotePath, string fileName)
        {
            try
            {
                using (FileStream fileStream = File.OpenRead(localPath + fileName))
                {
                    this.Connect();
                    this.sftp.UploadFile(fileStream, remotePath + fileName, null);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("文件上傳至SFTP，失敗，原因: {0}", ex.Message));
            }
        }

        /// <summary>
        /// 取得文件
        /// </summary>
        /// <param name="remotePath">伺服器文件完整路徑</param>
        /// <param name="localPath">本地文件完整路徑</param>
        public void Get(string remotePath, string localPath, string fileName)
        {
            try
            {
                Connect();
                var byt = sftp.ReadAllBytes(remotePath + fileName);
                Disconnect();
                File.WriteAllBytes(localPath + fileName, byt);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("無法取得SFTP文件，原因: {0}", ex.Message));
            }
        }

        /// <summary>
        /// 刪除文件
        /// </summary>
        /// <param name="remotePath">伺服器文件完整路徑</param>
        public void Delete(string remotePath, string fileName)
        {
            try
            {
                this.Connect();
                this.sftp.Delete(remotePath + fileName);
                this.Disconnect();
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("SFTP文件删除失败，原因：{0}", ex.Message));
            }
        }

        /// <summary>
        /// 移動文件
        /// </summary>
        /// <param name="oldRemotePath">舊的伺服器文件完整路徑</param>
        /// <param name="newRemotePath">新的伺服器文件完整路徑</param>
        public void Move(string oldRemotePath, string newRemotePath, string fileName)
        {
            try
            {
                this.Connect();
                this.sftp.RenameFile(oldRemotePath + fileName, newRemotePath + fileName);
                this.Disconnect();
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("SFTP文件移动失败，原因：{0}", ex.Message));
            }
        }

    }
}
