using System.Text;
using System.Xml;

namespace Corvus.Nest.Backend.Interfaces.IHelpers
{
    public interface IFileHelper
    {
        /// <summary>
        /// 寫入文字Log
        /// </summary>
        /// <param name="logCont"> Log內容 </param>
        /// <param name="logPath"> Log資料夾位置 </param>
        /// <param name="logFileName"> Log檔名 </param>
        void WriteLog(string logCont, string logPath, string logFileName);

        /// <summary>
        /// 文字輸出為Txt - 單行
        /// </summary>
        /// <param name="folderPath"> 檔案位置 </param>
        /// <param name="fileName"> 檔案名稱 </param>
        /// <param name="outStr"> 輸出文字 </param>
        void OutPutTxt(string folderPath, string fileName, string outStr);

        /// <summary>
        /// 文字輸出為Txt - 多行
        /// </summary>
        /// <param name="folderPath"> 檔案位置 </param>
        /// <param name="fileName"> 檔案名稱 </param>
        /// <param name="outStr"> 輸出文字 </param>
        void OutPutTxt(string folderPath, string fileName, List<string> outStr);

        /// <summary>
        /// 文字輸出為bytes檔案
        /// </summary>
        /// <param name="outStrs"> 輸出文字 </param>
        byte[] OutPutTxtToBytes(List<string> outStrs);

        /// <summary>
        /// 讀取Txt
        /// </summary>
        /// <param name="filePath"> Txt位置(含檔名及副檔名) </param>
        /// <returns> 檔案內容 </returns>
        IEnumerable<string> ReadTxt(string filePath);

        /// <summary>
        /// 讀取Txt
        /// </summary>
        /// <param name="bytes"> 檔案bytes </param>
        /// <param name="encoding"> 檔案編碼 </param>
        /// <returns> 檔案內容 </returns>
        IEnumerable<string> ReadTxt(byte[] bytes, Encoding? encoding = null);

        /// <summary>
        /// 讀取Json檔
        /// </summary>
        /// <param name="jsonPath"> Json檔案位置 </param>
        /// <returns> T物件 </returns>
        T? ReadJson<T>(string jsonPath) where T : new();

        /// <summary>
        /// 讀取XML檔
        /// </summary>
        /// <param name="path"> XML檔案路徑 </param>
        /// <returns> XmlDocument物件 </returns>
        XmlDocument ReadXML(string path);

        /// <summary>
        /// 檢查資料夾路徑
        /// </summary>
        /// <remarks> 若檔案路徑不存在則建立路徑 </remarks>
        /// <param name="path"> 檔案路徑 </param>
        /// <param name="createTag"> 是否建立資料夾 </param>
        bool ChkFolderPath(string path, bool createTag);

        /// <summary>
        /// 檢查檔案路徑
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        bool ChkFilePath(string path);

        /// <summary>
        /// 複製檔案
        /// </summary>
        /// <remarks> 若複製路徑已存在，則刪除檔案後複製。 </remarks>
        /// <param name="sourcePath"> 原始路徑(含檔名及附檔名) </param>
        /// <param name="afterPath"> 複製路徑(含檔名及副檔名) </param>
        void FileCopy(string sourcePath, string afterPath);

        /// <summary>
        /// 搬移檔案
        /// </summary>
        /// <remarks> 若搬移路徑已存在，則刪除檔案後搬移。 </remarks>
        /// <param name="sourcePath"> 原始路徑(含檔名及附檔名) </param>
        /// <param name="afterPath"> 搬移路徑(含檔名及副檔名) </param>
        void FileMove(string sourcePath, string afterPath);

        /// <summary>
        /// 刪除檔案 - Single
        /// </summary>
        /// <param name="path">檔案路徑</param>
        void FileDelete(string path);

        /// <summary>
        /// 刪除檔案 - 清空資料夾
        /// </summary>
        /// <param name="path">資料夾路徑</param>
        void DelectPathFile(string path);

        /// <summary>
        /// 維護過期檔案
        /// </summary>
        /// <remarks> 刪除資料夾內超出保存時間(retainLimit) </remarks>
        /// <param name="filePath"> 資料夾位置 </param>
        /// <param name="retainLimit"> 保存時間(日) </param>
        void FileMainTain(string filePath, int retainLimit);
    }
}
