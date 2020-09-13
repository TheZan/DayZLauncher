using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text.Json;
using DayZLauncher.Model;

namespace DayZLauncher.Utility
{
    public class MD5Checker
    {
        public MD5Checker(string folder, string ftp)
        {
            modsFolder = $"{folder}\\Mod";
            ftpUrl = ftp;

            if (!Directory.Exists(modsFolder))
                Directory.CreateDirectory(modsFolder);
        }

        public delegate void CheckerHandler(string message, bool progress);

        public event CheckerHandler Notify;

        public delegate void DownloadHandler(int current, int maximum, bool progress);

        public event DownloadHandler DownloadStatus;

        private readonly string modsFolder;
        private readonly string ftpUrl;
        private const string md5HashFileName = "MD5Hash.json";

        private List<FileHash> clientHashes;
        private List<FileHash> serverHashes;
        private List<FileHash> failedClientHashes;

        public void StartChecking()
        {
            GetServerHashes();
            GetClientHashes();
            Checksum();
        }

        private void Checksum()
        {
            failedClientHashes = new List<FileHash>();

            for (int i = 0; i < serverHashes.Count; i++)
            {
                if (clientHashes[i].FileName != serverHashes[i].FileName || clientHashes[i].Hash != serverHashes[i].Hash)
                {
                    failedClientHashes.Add(clientHashes[i]);
                    Notify?.Invoke($"Файл - {clientHashes[i].FileName} не прошел проверку.", true);
                }
            }

            if (failedClientHashes.Any())
            {
                Notify?.Invoke($"Начало загрузки файлов.", true);

                int fileNumber = 0;

                foreach (var file in failedClientHashes)
                {
                    fileNumber++;

                    Notify?.Invoke($"Загрузка - {file.FileName} | {fileNumber}/{failedClientHashes.Count}", true);
                    DownloadFileFromFtp(file.FileName);
                }

                Notify?.Invoke("Загрузка завершена. Все файлы успешно прошли проверку.", true);
            }
            else
            {
                Notify?.Invoke("Все файлы успешно прошли проверку.", true);
            }
        }

        private void DownloadFileFromFtp(string file)
        {
            WebRequest sizeRequest = WebRequest.Create($"{ftpUrl}/{file}");
            sizeRequest.Method = WebRequestMethods.Ftp.GetFileSize;
            int fileSize = (int) sizeRequest.GetResponse().ContentLength / 1000;

            FtpWebRequest request = (FtpWebRequest)WebRequest.Create($"{ftpUrl}/{file}");
            request.Method = WebRequestMethods.Ftp.DownloadFile;
            using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
            {
                using (Stream responseStream = response.GetResponseStream())
                {

                    string path = $"{modsFolder}\\{file}";
                    string pathToFolder = path.Remove(path.LastIndexOf("\\"), path.Length - path.LastIndexOf("\\"));

                    if (!Directory.Exists(pathToFolder))
                        Directory.CreateDirectory(pathToFolder);

                    using (FileStream fs = new FileStream(path, FileMode.Create))
                    {
                        byte[] buffer = new byte[64];
                        int size = 0;

                        while ((size = responseStream.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            fs.Write(buffer, 0, size);
                            int position = (int) fs.Position / 1000;
                            DownloadStatus?.Invoke(position, fileSize, false);
                        }
                    }
                }
            }
        }

        private void GetServerHashes()
        {
            Notify?.Invoke("Начало проверки.", true);

            FtpWebRequest request = (FtpWebRequest)WebRequest.Create($"{ftpUrl}/{md5HashFileName}");
            request.Method = WebRequestMethods.Ftp.DownloadFile;
            FtpWebResponse response = (FtpWebResponse)request.GetResponse();
            Stream responseStream = response.GetResponseStream();

            var md5Hash = new StreamReader(responseStream).ReadToEnd();

            serverHashes = JsonSerializer.Deserialize<List<FileHash>>(md5Hash);
        }

        private void GetClientHashes()
        {
            List<string> files = new List<string>();

            foreach (var file in serverHashes)
            {
                files.Add(file.FileName);
            }

            if (DirSearch(modsFolder).Any())
            {
                Notify?.Invoke($"Начало проверки файлов.", true);

                int fileNumber = 0;

                clientHashes = new List<FileHash>();

                foreach (var file in files)
                {
                    clientHashes.Add(new FileHash()
                    {
                        FileName = file,
                        Hash = GenerateHash($"{modsFolder}\\{file}")
                    });

                    fileNumber++;

                    Notify?.Invoke($"{fileNumber}/{serverHashes.Count}", false);
                }
            }
            else
            {
                Notify?.Invoke($"Моды не найдены", true);

                Notify?.Invoke($"Начало загрузки.", true);

                int fileNumber = 1;

                foreach (var file in serverHashes)
                {
                    fileNumber++;
                    Notify?.Invoke($"Загрузка - {file.FileName} | {fileNumber}/{serverHashes.Count}", true);
                    DownloadFileFromFtp(file.FileName);
                }

                Notify?.Invoke($"Загрузка завершена.", true);

                GetClientHashes();
            }
        }

        private string GenerateHash(string file)
        {
            try
            {
                using (var md5 = MD5.Create())
                {
                    using (var stream = File.OpenRead(file))
                    {
                        var hash = md5.ComputeHash(stream);
                        return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                    }
                }
            }
            catch
            {
                return null;
            }
        }

        private List<string> DirSearch(string sDir)
        {
            List<string> files = new List<string>();
            try
            {
                foreach (string f in Directory.GetFiles(sDir))
                {
                    files.Add(f);
                }

                foreach (string d in Directory.GetDirectories(sDir))
                {
                    files.AddRange(DirSearch(d));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return files;
        }
    }
}
