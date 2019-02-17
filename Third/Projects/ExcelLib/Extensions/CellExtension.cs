using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPOI.SS.UserModel;
using NPOI.SS.Util;

namespace ExcelLib
{
    public static class CellExtension
    {


        public static object GetCellValue(this ICell cell)
        {
            try
            {
                if (cell.CellType == CellType.Numeric)
                {
                    return cell.NumericCellValue;
                }
                else if (cell.CellType == CellType.String)
                {
                    return cell.StringCellValue;
                }
                else if (cell.CellType == CellType.Boolean)
                {
                    return cell.BooleanCellValue;
                }
                else
                {
                    return cell.StringCellValue;
                }
            }
            catch (Exception ex)
            {                
                return null;
            }
        }

        public static ICell SetRowSpan(this ICell cell, int i)
        {
            if (i == 0) return cell;
            if (i > 0)
            {
                int r=cell.Sheet.AddMergedRegion(new CellRangeAddress(
                    cell.RowIndex,
                    cell.RowIndex + i,
                    cell.ColumnIndex,
                    cell.ColumnIndex));
            }
            else
            {
                //合并单元格时取的是左边的数据
                //不过这个操作的意图是把当前单元格扩展到左边，要把左边的数据修改一下。
                //cell.Row.GetCellEx(cell.ColumnIndex + i).SetCellValue(cell.StringCellValue);
                cell.Sheet.SetCellValue(cell.RowIndex + i, cell.ColumnIndex, cell.StringCellValue);
                int r = cell.Sheet.AddMergedRegion(new CellRangeAddress(
                    cell.RowIndex + i,
                    cell.RowIndex,
                    cell.ColumnIndex,
                    cell.ColumnIndex));
            }
            return cell;
        }

        public static ICell SetColumnSpan(this ICell cell, int i)
        {
            if (i == 0) return cell;
            if (i > 0)
            {
                cell.Sheet.AddMergedRegion(new CellRangeAddress(
                    cell.RowIndex,
                    cell.RowIndex,
                    cell.ColumnIndex,
                    cell.ColumnIndex + i));
            }
            else
            {
                //合并单元格时取的是左边的数据
                //不过这个操作的意图是把当前单元格扩展到左边，要把左边的数据修改一下。
                cell.Row.GetCellEx(cell.ColumnIndex + i).SetCellValue(cell.StringCellValue);
                cell.Sheet.AddMergedRegion(new CellRangeAddress(
                    cell.RowIndex,
                    cell.RowIndex,
                    cell.ColumnIndex + i,
                    cell.ColumnIndex));
            }
            return cell;
        }

        public static ICell SetColumnWidth(this ICell cell, int width)
        {
            cell.Sheet.SetColumnWidth(cell.ColumnIndex, width * 256);
            return cell;
        }
    }
}
