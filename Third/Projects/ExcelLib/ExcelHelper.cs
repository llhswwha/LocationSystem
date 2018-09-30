using System;
using System.Data;
using System.Data.OleDb;
using System.IO;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;

namespace ExcelLib
{
    public static class ExcelHelper
    {
        #region Load
        public static DataSet Load(FileInfo file, bool isFirtRowHeader)
        {
            IWorkbook hssfworkbook = Open(file.FullName);
            return GetDataSet(isFirtRowHeader, hssfworkbook);
        }

        //private static DataTable GetDataTable(FileInfo file, bool isFirtRowHeader, string sheetName)
        //{
        //    IWorkbook hssfworkbook = Open(file.FullName);
        //    DataTable dt = GetDataTableFromBook(isFirtRowHeader, hssfworkbook, sheetName);
        //    return dt;
        //}

        //public static DataTable LoadTable(FileInfo fileInfo, bool isFirtRowHeader, string sheetName)
        //{
            
        //    if (fileInfo.FullName.EndsWith("xlsx"))
        //    {
        //        DataTable dt = new DataTable();
        //        //using (OleDbConnection con = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0; Extended Properties=Excel 12.0;Data Source=" + file.FullName + @";Extended Properties=Excel 12.0;"))
        //        //{
        //        //    con.Open();
        //        //    string sql = String.Format("SELECT * FROM [{0}$]", sheetName);
        //        //    OleDbDataAdapter da = new OleDbDataAdapter(sql, con);
        //        //    da.Fill(dt);
        //        //}
        //        FileStream file = File.OpenRead(fileInfo.FullName);
        //        IWorkbook workbook = new XSSFWorkbook(file);
        //        ISheet sheet=workbook.GetSheetAt(0);
        //        return dt;
        //    }
        //    else
        //    {
        //        return GetDataTable(fileInfo, isFirtRowHeader, sheetName);
        //    }
        //}

        public static DataSet GetDataSet(bool isFirtRowHeader, IWorkbook hssfworkbook)
        {
            DataSet ds = new DataSet();
            for (int i = 0; i < hssfworkbook.NumberOfSheets; i++)
            {
                var dt = GetDataTableFromBook(isFirtRowHeader, hssfworkbook, i);
                ds.Tables.Add(dt);
            }
            return ds;
        }

        private static DataTable GetDataTableFromBook(bool isFirtRowHeader, IWorkbook hssfworkbook, int i)
        {
            ISheet  sheet = hssfworkbook.GetSheetAt(i);
            string sheetName = hssfworkbook.GetSheetName(i);
            DataTable dt = GetDataTable(sheet, sheetName, isFirtRowHeader);
            return dt;
        }

        private static DataTable GetDataTableFromBook(bool isFirtRowHeader, IWorkbook hssfworkbook,string sheetName)
        {
            ISheet sheet = hssfworkbook.GetSheet(sheetName);
            DataTable dt = GetDataTable(sheet, sheetName, isFirtRowHeader);
            return dt;
        }

        public static IWorkbook Open(string fileName)
        {
            IWorkbook workbook = null;
            try
            {
                FileStream fs = File.OpenRead(fileName);
                if (fileName.ToLower().EndsWith(".xls"))
                {
                    try
                    {
                        workbook = new HSSFWorkbook(fs);
                    }
                    catch (Exception)
                    {
                        workbook = new XSSFWorkbook(fs);//有人会手动修改.xlsx的后缀为.xls
                    }

                }
                else if (fileName.ToLower().EndsWith(".xlsx"))
                {
                    try
                    {
                        workbook = new XSSFWorkbook(fs);
                    }
                    catch (Exception)
                    {
                        workbook = new HSSFWorkbook(fs);
                    }
                }
                fs.Close();
            }
            catch (Exception ex)
            {
                
            }
            return workbook;
        }

        public static DataTable GetDataTable(ISheet  sheet, string sheetName, bool isFirtRowHeader)
        {
            if (sheet == null) return null;
            DataTable dt = new DataTable(sheetName);

            var columnCount = GetColumnCount(sheet);

            for (int j = 0; j <= sheet.LastRowNum; j++)
            {
                IRow row = sheet.GetRow(j);
                if (row == null) continue;
                if (InitColumns(isFirtRowHeader, dt, row, columnCount))
                    continue;//如果第一行作为表头，则跳过
                AddDataRow(row, dt);
            }
            return dt;
        }

        private static int GetColumnCount(ISheet sheet)
        {
            if (sheet == null) return -1;
            int columnCount = 0;
            for (int j = 0; j <= sheet.LastRowNum; j++)
            {
                IRow row = sheet.GetRow(j);
                if (row == null) continue;
                int count = GetColumnCount(row);
                if (count > columnCount)
                {
                    columnCount = count;
                }
            }
            return columnCount;
        }

        private static int GetColumnCount(IRow row)
        {
            return row.LastCellNum - row.FirstCellNum;
        }


        /// <summary>
        /// 添加行数据
        /// </summary>
        /// <param name="row"></param>
        /// <param name="dt"></param>
        private static void AddDataRow(IRow row, DataTable dt)
        {
            var values = GetRowValues(row);
            dt.Rows.Add(values);
        }

        /// <summary>
        /// 初始化列名
        /// </summary>
        /// <param name="isFirtRowHeader"></param>
        /// <param name="dt"></param>
        /// <param name="row"></param>
        /// <param name="columnCount"></param>
        /// <returns>该行是否作为表头</returns>
        private static bool InitColumns(bool isFirtRowHeader, DataTable dt, IRow row,int columnCount)
        {
            if (dt.Columns.Count == 0)
            {
                if (isFirtRowHeader)
                {
                    AddFirstRowHeader(dt, row, columnCount);
                    return true;//如果第一行作为表头，则跳过
                }
                AddDefaultHeader(dt, columnCount);
            }
            return false;
        }

        /// <summary>
        /// 添加默认标题 Column1 Column2的样式
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="columnCount"></param>
        private static void AddDefaultHeader(DataTable dt, int columnCount)
        {
            for (int k = 0; k < columnCount; k++)
            {
                dt.Columns.Add("Column" + (k + 1));
            }
        }

        /// <summary>
        /// 第一行作为列名
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="row"></param>
        /// <param name="columnCount"></param>
        private static void AddFirstRowHeader(DataTable dt, IRow row,int columnCount)
        {
            for (int k = 0; k < columnCount; k++)
            {
                if (row.FirstCellNum == -1) continue;
                ICell cell = row.GetCell(row.FirstCellNum + k);
                if (cell != null)
                {
                    string value = cell.GetCellValue()+"";
                    dt.Columns.Add(value);
                }
                else
                {
                    dt.Columns.Add("");
                }
            }
        }

        /// <summary>
        /// 获取一行中的值
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        private static object[] GetRowValues(IRow row)
        {
            int columnCount = GetColumnCount(row);
            object[] values = new object[columnCount];
            for (int k = 0; k < columnCount; k++)
            {
                ICell cell = row.GetCell(row.FirstCellNum + k);
                if (cell == null)
                {
                    values[k] = "";
                }
                else
                {
                    values[k] = cell.ToString();
                }
            }
            return values;
        }

        public static DataTable LoadTable(FileInfo file, string tableName, bool isFirtRowHeader)
        {
            IWorkbook hssfworkbook = Open(file.FullName);
            ISheet  ISheet1 = hssfworkbook.GetSheet(tableName);
            DataTable dt = GetDataTable(ISheet1, tableName, isFirtRowHeader);
            return dt;
        }
        #endregion

        public static bool Save(DataSet dataSet, FileInfo file,string[] titles)
        {
            ExcelFile excelFile = new ExcelFile(dataSet, titles);
            return excelFile.Save(file);
        }

        public static bool Save(DataTable dataTable, FileInfo file, string title)
        {
            ExcelFile excelFile = new ExcelFile(dataTable, title);
            return excelFile.Save(file);
        }

        private static void CreateTempFile(Stream excelFileStream, string tempFile)
        {
            FileStream fileStream = new FileStream(tempFile, FileMode.Create, FileAccess.Write);

            const int length = 256;
            Byte[] buffer = new Byte[length];
            int bytesRead = excelFileStream.Read(buffer, 0, length);

            while (bytesRead > 0)
            {
                fileStream.Write(buffer, 0, bytesRead);
                bytesRead = excelFileStream.Read(buffer, 0, length);
            }

            excelFileStream.Close();
            fileStream.Close();
        }
    }
}
