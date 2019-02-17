using System;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;

namespace ExcelLib
{
    public class ExcelFile
    {
        public string FileName { get; set; }

        #region Load
        public DataSet Load(FileInfo file, bool isFirtRowHeader)
        {
            Open(file.FullName, false);
            DataSet ds = ExcelHelper.GetDataSet(isFirtRowHeader, hssfworkbook);
            return ds;
        }

        private void Open(string fileName,bool isCreate)
        {
            if (isCreate)
            {
                if (File.Exists(fileName))
                    File.Delete(fileName);
                InitializeWorkbook();
                FileName = fileName;
            }
            else
            {
                hssfworkbook = ExcelHelper.Open(fileName);
            }
        }

        public DataTable LoadTable(FileInfo file, string tableName, bool isFirtRowHeader)
        {
            Open(file.FullName, false);
            ISheet sheet1 = hssfworkbook.GetSheet(tableName);
            DataTable dt = ExcelHelper.GetDataTable(sheet1, tableName, isFirtRowHeader);
            return dt;
        }
        #endregion

        private IWorkbook hssfworkbook;

        private ISheet ActiveSheet
        {
            get { return hssfworkbook.GetSheetAt(hssfworkbook.ActiveSheetIndex); }
        }

        public ISheet GetSheet(int id)
        {
            if (SheetCount == 0 || SheetCount <= id)
            {
                return null;
            }
            return hssfworkbook.GetSheetAt(id);
        }

        public int SheetCount
        {
            get { return hssfworkbook.NumberOfSheets; }
        }

        /// <summary>
        /// 合并列中的相同的数据，合并单元格
        /// </summary>
        /// <param name="sheetId"></param>
        /// <param name="columnId"></param>
        public void MergeColumn(int sheetId,int columnId)
        {
            ISheet sheet = GetSheet(sheetId);
            sheet.MergeColumn(columnId);
        }


        public void MergeColumnEx(int sheetId, int col, int startRow, int endRow, int columnSpan)
        {
            ISheet sheet = GetSheet(sheetId);
            sheet.MergeColumnEx(col, startRow, endRow, columnSpan);
        }

        /// <summary>
        /// 设置列的对齐方式
        /// </summary>
        /// <param name="sheetId"></param>
        /// <param name="columnId"></param>
        /// <param name="alignment"></param>
        public void SetColumnAlign(int sheetId, int columnId,HorizontalAlignment alignment)
        {
            ISheet sheet = GetSheet(sheetId);
            sheet.SetColumnAlignment(columnId, alignment);
        }



        public ExcelFile()
        {
            InitializeWorkbook();
            ICellStyleManager.Clear();
        }

        public ExcelFile(string fileName,bool isCreate=false)
            :this()
        {
            FileName = fileName;
            Open(fileName, isCreate);
        }

        public ExcelFile(DataSet dataSet, string[] titles)
            :this()
        {
            InitData(dataSet, titles);
        }

        public ExcelFile(DataTable dataTable, string title)
            : this()
        {
            InitData(dataTable, title);
        }

        private void InitData(DataSet dataSet, string[] titles)
        {
            for (int i = 0; i < dataSet.Tables.Count; i++)
            {
                DataTable dataTable = dataSet.Tables[i];

                string title = null;
                if (titles != null && titles.Length > i)
                    title = titles[i];
                string sheetName = string.IsNullOrEmpty(dataTable.TableName) ? "Sheet" + (i + 1) : dataTable.TableName;
                AddSheet(dataTable, sheetName, title);
            }
        }

        private void InitData(DataTable dataTable, string title)
        {
            string sheetName = string.IsNullOrEmpty(dataTable.TableName) ? "Sheet1" : dataTable.TableName;
            AddSheet(dataTable, sheetName, title);
        }

        /// <summary>
        /// 保存文件
        /// </summary>
        public bool Save(FileInfo file,bool openFile=false)
        {
            try
            {
                if (file.Directory.Exists == false)
                {
                    file.Directory.Create();
                }
                WriteToFile(hssfworkbook, file.FullName);
                if (openFile)
                {
                    Process.Start("\"" +file.FullName+ "\"");
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex);
                return false;
            }

            return true;
        }

        public bool Save()
        {
            return Save(new FileInfo(FileName));
        }

        /// <summary>
        /// 添加表格
        /// </summary>
        /// <param name="dataTable"></param>
        /// <param name="sheetName"></param>
        /// <param name="title"></param>
        private void AddSheet(DataTable dataTable, string sheetName, string title)
        {
            ISheet sheet1 = AddSheet(sheetName);
            //AddTitle(sheet1, title);
            int id = GetTitleLineCount(title);
            FillSheet(sheet1, dataTable, id);//这里有宽度自适应，但是标题会被计算进去-->
            //放到最后

            for (int i = 0; i < id; i++)
            {
                sheet1.AddMergedRegion(new CellRangeAddress(i, i, 0, dataTable.Columns.Count - 1));
            }

            AddTitle(sheet1, title);//-->所以，移动到这里。先加内容，再加标题。
        }

        private int GetTitleLineCount(string title)
        {
            if (string.IsNullOrEmpty(title)) return 0;
            return title.Split('\n').Length;
        }

        public ISheet AddSheet(string sheetName)
        {
            //return hssfworkbook.CreateSheet(sheetName);
            try
            {
                sheetName = sheetName.Replace("*", "");
                return hssfworkbook.CreateSheet(sheetName);
            }
            catch (Exception ex)
            {
                if (ex.Message == "The workbook already contains a ISheet of this name")
                {
                    sheetName = sheetName + "_2";
                    return AddSheet(sheetName);
                }
                else
                {                    
                    Console.WriteLine(ex);
                    throw ex;
                    return hssfworkbook.GetSheetAt(hssfworkbook.NumberOfSheets - 1);
                }
            }
        }

        /// <summary>
        /// 添加标题 返回添加后的行数
        /// </summary>
        /// <param name="sheet1"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        private void AddTitle(ISheet sheet1, string title, int startRowId=0)
        {
            if (string.IsNullOrEmpty(title)) return ;

            string[] lines = title.Split('\n');
            for (int i = 0; i < lines.Length; i++)
            {
                AddTitleCell(sheet1, lines, i, startRowId + i);
            }
        }

        private static void AddTitleCell(ISheet sheet1, string[] lines, int i, int id)
        {
            ICell cell = sheet1.SetCellValue(id, 0, lines[i]);
            SetTitleStyle(i, cell);
            if (i == 0)
            {
                cell.Row.Height = 2*256;
            }
        }

        /// <summary>
        /// 设置标题行的样式
        /// </summary>
        /// <param name="i"></param>
        /// <param name="cell"></param>
        private static void SetTitleStyle(int i, ICell cell)
        {
            if (i == 0)
            {
                SetMainTitleStyle(cell);
            }
            else
            {
                SetSubTitleStyle(cell);
            }
        }

        private static void SetSubTitleStyle(ICell cell)
        {
            cell.SetICellStyle(12, false, false);
        }

        private static void SetMainTitleStyle(ICell cell)
        {
            cell.SetICellStyle(15, false, false, true);
        }

        private void AddSheet(HSSFWorkbook hssfworkbook, DataTable dataTable, string sheetName)
        {
            ISheet sheet1 = hssfworkbook.CreateSheet(sheetName);
            FillSheet(sheet1, dataTable, 2);
        }

        /// <summary>
        /// 填充数据
        /// </summary>
        /// <param name="sheet1"></param>
        /// <param name="dataTable"></param>
        /// <param name="startRow"></param>
        private void FillSheet(ISheet sheet1, DataTable dataTable, int startRow)
        {
            //sheet1.AddTableHeader(dataTable,ref startRow,0);
            //sheet1.AddTableBody(dataTable, startRow + 1, 0);
            //sheet1.AutoSizeColumn(dataTable.Columns.Count, 0);
            sheet1.InsertDataTable(dataTable, startRow, 0, true, true);
        }

        void WriteToFile(IWorkbook hssfworkbook, string fileName)
        {
            FileStream file = new FileStream(fileName, FileMode.Create);
            hssfworkbook.Write(file);
            file.Close();
        }

        void InitializeWorkbook()
        {
            hssfworkbook = new HSSFWorkbook();

            //////create a entry of DocumentSummaryInformation
            //DocumentSummaryInformation dsi = PropertySetFactory.CreateDocumentSummaryInformation();
            //dsi.Company = "NPOI Team";
            //hssfworkbook.DocumentSummaryInformation = dsi;

            //////create a entry of SummaryInformation
            //SummaryInformation si = PropertySetFactory.CreateSummaryInformation();
            //si.Subject = "NPOI SDK Example";
            //hssfworkbook.SummaryInformation = si;
        }

    }
}
