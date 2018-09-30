using System;
using NPOI.SS.UserModel;

namespace ExcelLib
{
    /// <summary>
    /// 基本扩展，一些Get
    /// </summary>
    public static class BaseExtension
    {
        public static ICell GetCellEx(this IRow row, int columnId,CellType type=CellType.String)
        {
            ICell cell = row.GetCell(columnId);
            if (cell == null)
            {
                cell = row.CreateCell(columnId, type);
                cell.SetCellValue("");
            }
            return cell;
        }

        public static IRow GetRowEx(this ISheet sheet, int rowId)
        {
            IRow row = sheet.GetRow(rowId);
            if (row == null)
            {
                row = sheet.CreateRow(rowId);
            }
            return row;
        }

        public static ICell GetCell(this ISheet sheet1, int rowId, int columnId,CellType type=CellType.String)
        {
            IRow row = sheet1.GetRowEx(rowId);
            ICell cell = row.GetCellEx(columnId, type);
            return cell;
        }

        public static void SetCellValueEx(this ICell cell, object value)
        {
            if (value is double
                ||value is float
                || value is int
                || value is short
                || value is long
                ||value is uint
                ||value is ushort
                ||value is ulong)
            {
                double v = Convert.ToDouble(value);
                cell.SetCellValue(v);
            }
            else if (value is DateTime)
            {
                cell.SetCellValue((DateTime)value);
            }
            else
            {
                cell.SetCellValue(value.ToString());
            }
        }
    }
}
