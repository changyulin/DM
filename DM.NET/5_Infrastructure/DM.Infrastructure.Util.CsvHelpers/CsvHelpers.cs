using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LumenWorks.Framework.IO.Csv;

namespace DM.Infrastructure.Util.CsvHelpers
{
    public class CsvHelpers : IDisposable
    {
        public CachedCsvReader ExcelReaderObj;
        public int ExcelReaderFieldCount
        {
            get
            {
                try
                {
                    return ExcelReaderObj.FieldCount;
                }
                catch
                {
                    return 0;
                }
            }
        }
        public long CurrentRecordIndex
        {
            get
            {
                return ExcelReaderObj.CurrentRecordIndex;
            }
        }
        public CsvHelpers(StreamReader sr, int columnnum)
        {
            InitReaderObj(sr, columnnum);
        }

        public CsvHelpers(Stream filestream, int columnnum)
        {
            StreamReader sr = new StreamReader(filestream);
            InitReaderObj(sr, columnnum);
        }

        private void InitReaderObj(StreamReader sr, int columnnum)
        {
            StringBuilder content = new StringBuilder();
            string str;
            //trim "," in the header line
            str = sr.ReadLine();
            str = str.TrimEnd(',');
            content.Append(str).Append("\r\n");
            while ((str = sr.ReadLine()) != null)
            {
                while (str.Split(',').Length < columnnum)
                {
                    str += ",";
                }
                content.Append(str).Append("\r\n");
            }
            sr.Close();
            ExcelReaderObj = new CachedCsvReader(new StringReader(content.ToString()), true);
        }

        public bool ExcelReaderReadNext()
        {
            return ExcelReaderObj.ReadNextRecord();
        }
        public void ExcelReaderMoveToStart()
        {
            ExcelReaderObj.MoveToStart();
        }
        public string GetCell(int index)
        {
            return ExcelReaderObj[index];
        }

        public string GetCell(string field)
        {
            return ExcelReaderObj[field];
        }

        public string[] GetFieldHeaders()
        {
            return ExcelReaderObj.GetFieldHeaders();
        }


        public void Dispose()
        {
            ExcelReaderObj.Dispose();
        }
    }
}
