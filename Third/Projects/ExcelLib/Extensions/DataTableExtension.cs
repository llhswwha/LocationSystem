using System;
using System.Data;
using NPOI.SS.UserModel;

namespace ExcelLib
{
    public static class DataTableExtension
    {
        public static void InsertDataTable(this ISheet  ISheet1, DataTable dataTable, int startRow, int startColumn, bool showHeader,bool autoWidth)
        {
            if (showHeader)
            {
                ISheet1.AddTableHeader(dataTable, ref startRow, startColumn);
            }

            ISheet1.AddTableBody(dataTable, startRow, startColumn);

            if (autoWidth)
            {
                ISheet1.AutoSizeColumn(dataTable.Columns.Count, startColumn);
            }
        }

        /// <summary>
        /// 自动列宽
        /// </summary>
        /// <param name="ISheet1"></param>
        /// <param name="count"></param>
        /// <param name="startColumn"></param>
        public static void AutoSizeColumn(this ISheet  ISheet1, int count, int startColumn)
        {
            for (int j = 0; j < count; j++)
            {
                try
                {
                    int column = j + startColumn;
                    ISheet1.AutoSizeColumn(j + startColumn);//这里会出现空指针异常，可能是越界了。
                    int width = ISheet1.GetColumnWidth(column);
                    ISheet1.SetColumnWidth(column, width + 1 * 256);
                }
                catch (Exception)
                {
                    
                    //throw;
                }
            }
        }

        /// <summary>
        /// 填充表头的内容
        /// </summary>
        /// <param name="ISheet1"></param>
        /// <param name="dataTable"></param>
        /// <param name="startRow"></param>
        /// <param name="startColumn"></param>
        /// <returns></returns>
        public static void AddTableHeader(this ISheet  ISheet1, DataTable dataTable,ref int startRow,int startColumn)
        {
            IRow headerRow = ISheet1.CreateRow(startRow++);
            for (int j = 0; j < dataTable.Columns.Count; j++)
            {
                object value = dataTable.Columns[j].ColumnName;
                ICell cell = headerRow.CreateCell(startColumn+j);
                cell.SetCellValue(value.ToString());
                cell.SetICellStyle(10, true, true);
            }
        }

        public static void AddTableBody(this ISheet  ISheet1, DataTable dataTable, int startRow, int startColumn)
        {
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                AddDataRow(ISheet1, dataTable, i, startRow, startColumn);
            }
        }

        /// <summary>
        /// 填充一行数据
        /// </summary>
        /// <param name="ISheet1"></param>
        /// <param name="dataTable"></param>
        /// <param name="startRow"></param>
        /// <param name="i"></param>
        /// <param name="startColumn"></param>
        private static void AddDataRow(ISheet  ISheet1, DataTable dataTable, int i, int startRow, int startColumn)
        {
            DataRow dataRow = dataTable.Rows[i];
            IRow row = ISheet1.GetRowEx(i + startRow);
            row.AddDataRow(dataRow, startColumn, i);

        }

        /// <summary>
        /// 填充一行数据
        /// </summary>
        /// <param name="row"></param>
        /// <param name="dataRow"></param>
        /// <param name="startColumn"></param>
        public static void AddDataRow(this IRow row, DataRow dataRow, int startColumn,int rowIndex)
        {
            for (int j = 0; j < dataRow.Table.Columns.Count; j++)
            {
                ICell cell = row.CreateCell(j + startColumn);
                cell.SetCellValue(dataRow[j].ToString());
                cell.SetICellStyle(10, false, true);
            }
        }
    }
}
