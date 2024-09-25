using Corvus.Nest.Backend.Extensions;
using Corvus.Nest.Backend.Interfaces.IHelpers;
using System.Globalization;
using System.Text;
using System.Xml;

namespace Corvus.Nest.Backend.Helpers
{
    public class FileHelper : IFileHelper
    {
        private readonly CultureInfo CI = new("en-US", true);

        public FileHelper() { }

        public virtual void WriteLog(string logCont, string logPath, string logFileName)
        {
            try
            {
                logCont = JsonConvert.Serialize(new { LogTime = DateTime.Now.ToString("O"), Message = logCont });

                ChkFolderPath(logPath, true);

                OutPutTxt(logPath, logFileName, logCont);
            }
            catch
            {
                OutPutTxt(logPath.Replace($".txt", $"-{DateTime.Now:HHmmsss}.txt"), logFileName, logCont);
            }
        }

        public virtual void OutPutTxt(string folderPath, string fileName, string outStr)
        {
            string filePath = Path.Combine(folderPath, $@"{fileName}");

            ChkFolderPath(folderPath, true);

            using (FileStream fs = new FileStream(filePath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
            {
                using (StreamWriter outputFile = new StreamWriter(fs))
                {
                    outputFile.WriteLine(outStr);
                }
            }
        }

        public virtual void OutPutTxt(string folderPath, string fileName, List<string> outStr)
        {
            string filePath = Path.Combine(folderPath, fileName);

            ChkFolderPath(folderPath, true);

            using (FileStream fs = new FileStream(filePath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
            {
                using (StreamWriter outputFile = new StreamWriter(fs))
                {
                    foreach (var o in outStr)
                    {
                        outputFile.WriteLine(o);
                    }
                }
            }
        }

        public byte[] OutPutTxtToBytes(List<string> outStrs)
        {
            using MemoryStream stream = new();
            using (StreamWriter writer = new(stream))
            {
                outStrs.ForEach(writer.WriteLine);
            }

            return stream.ToArray();
        }

        public virtual IEnumerable<string> ReadTxt(string filePath)
        {
            IList<string> strs = new List<string>();
            using StreamReader read = new(filePath);

            if (read == null)
                return strs;

            string? line;
            while ((line = read.ReadLine()) != null)
            {
                strs.Add(line);
            }

            return strs;
        }

        public virtual IEnumerable<string> ReadTxt(byte[] bytes, Encoding? encoding = null)
        {
            encoding ??= Encoding.UTF8;

            IList<string> strs = new List<string>();
            using StreamReader reader = new(new MemoryStream(bytes), encoding);

            if (reader is null)
                return strs;

            string? line;
            while ((line = reader.ReadLine()) != null)
            {
                strs.Add(line);
            }

            return strs;
        }

        public virtual T? ReadJson<T>(string jsonPath) where T : new()
        {
            string json = string.Empty;

            using (StreamReader read = new StreamReader(jsonPath))
            {
                json = read.ReadToEnd();
            }

            return JsonConvert.Deserialize<T>(json);
        }

        public virtual XmlDocument ReadXML(string path)
        {
            XmlDocument objXmlDoc = new XmlDocument();
            objXmlDoc.Load(path);

            return objXmlDoc;
        }

        public virtual bool ChkFolderPath(string path, bool createTag)
        {
            var result = Directory.Exists(path);

            if (!result && createTag)
                Directory.CreateDirectory(path);

            return result;
        }

        public virtual bool ChkFilePath(string path)
        {
            return File.Exists(path);
        }

        public virtual void FileCopy(string sourcePath, string afterPath)
        {
            if (File.Exists(sourcePath))
            {
                if (File.Exists(afterPath))
                    FileDelete(afterPath);

                File.Copy(sourcePath, afterPath);
            }
        }

        public virtual void FileMove(string sourcePath, string afterPath)
        {
            if (File.Exists(sourcePath))
            {
                if (File.Exists(afterPath))
                    FileDelete(afterPath);

                File.Move(sourcePath, afterPath);
            }
        }

        /// 刪除檔案(Single)
        public virtual void FileDelete(string path)
        {
            if (File.Exists(path))
                File.Delete(path);
        }

        /// 刪除檔案(資料夾中的所有檔案)
        public virtual void DelectPathFile(string path)
        {
            if (Directory.Exists(path))
            {
                foreach (string f in Directory.GetFileSystemEntries(path))
                {
                    if (File.Exists(f))
                        File.Delete(f); // 如果有子檔案刪除檔案
                }
            }
        }

        public virtual void FileMainTain(string filePath, int retainLimit)
        {
            string[] arrFilePaths;
            DateTime dtTmp;
            var logFileName = $"Log{DateTime.Now.ToString("yyyyMMdd", CI)}.txt";

            ChkFolderPath(filePath, true);

            if (filePath != string.Empty)
            {
                arrFilePaths = Directory.GetFiles(filePath);
                for (int i = 0; i < arrFilePaths.Length; i++)
                {
                    dtTmp = Directory.GetLastWriteTime(arrFilePaths[i]);
                    if ((dtTmp - DateTime.Now).Days > retainLimit)
                    {
                        File.Delete(arrFilePaths[i]);
                    }
                }
            }
        }
    }
}
