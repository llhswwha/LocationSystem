using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using NPOI.SS.Util;

namespace ExcelLib
{
    public static class ISheetExtension
    {
        public static ICellStyle SetRegionColor(this ISheet  ISheet1, int startRow, int startColumn, int endRow,
            int endColumn, HSSFColor color, ICellStyle style)
        {
            if (style == null)
            {
                style = ICellStyleManager.CreateStyle(ISheet1.Workbook);
            }
            style.SetFillColor(color);
            return ISheet1.SetRegionStyle(startRow, startColumn, endRow, endColumn, style);
        }

        public static ICellStyle SetRegionBorder(this ISheet  ISheet1, int startRow, int startColumn, int endRow,
            int endColumn)
        {
            var blackBorder = ISheet1.Workbook.CreateBlackBorder();
            return ISheet1.SetRegionStyle(startRow, startColumn, endRow, endColumn, blackBorder);
        }

        public static ICellStyle SetRegionBorder(this ISheet  ISheet1, int startRow, int startColumn, int endRow,
            int endColumn, HSSFColor color)
        {
            var blackBorder = ISheet1.Workbook.CreateBorder(color);
            return ISheet1.SetRegionStyle(startRow, startColumn, endRow, endColumn, blackBorder);
        }

        public static ICellStyle SetRegionStyle(this ISheet  ISheet1, int startRow, int startColumn, int endRow,
            int endColumn, ICellStyle style)
        {
            for (int iRow = startRow; iRow <= endRow; iRow++)
            {
                IRow row = ISheet1.GetRowEx(iRow);
                for (int iCol = startColumn; iCol <= endColumn; iCol++)
                {
                    //Cell ICell = row.GetCell(iCol);
                    //if (cell != null)
                    //{
                    //    cell.ICellStyle = style;
                    //}
                    ICell cell = row.GetCellEx(iCol);
                    cell.CellStyle = style;
                }
            }
            return style;
        }

        public static void SetColumnCenter(this ISheet  ISheet1, int col)
        {
            ISheet1.SetColumnAlignment(col, HorizontalAlignment.Center, VerticalAlignment.Center);
        }

        public static void SetRowCenter(this ISheet  ISheet1, int rowId, int maxColId)
        {
            for (int i = 0; i < maxColId; i++)
            {
                SetCellAlignment(ISheet1, rowId, i, HorizontalAlignment.Center, VerticalAlignment.Center);
            }
        }

        public static void SetRowAlignment(this ISheet  ISheet1, int rowId,int maxColId, HorizontalAlignment alignment, VerticalAlignment verticalAlignment = VerticalAlignment.Center)
        {
            for (int i = 0; i < maxColId; i++)
            {
                SetCellAlignment(ISheet1, rowId, i, alignment, verticalAlignment);
            }
        }

        public static void SetColumnAlignment(this ISheet  ISheet1, int col,HorizontalAlignment alignment, VerticalAlignment verticalAlignment= VerticalAlignment.Center)
        {
            int j = 0;
            for (; j <= ISheet1.LastRowNum; j++)
            {
                SetCellAlignment(ISheet1, j, col, alignment, verticalAlignment);
            }
        }

        public static void SetCellAlignment(this ISheet  ISheet1,int rowId, int col, HorizontalAlignment alignment, VerticalAlignment verticalAlignment)
        {
            IRow row = ISheet1.GetRow(rowId);
            if (row == null) return;
            ICell cell = row.GetCell(col);
            if (cell == null) return;
            cell.CellStyle.Alignment = alignment;
            cell.CellStyle.VerticalAlignment = verticalAlignment;
        }

        public static void MergeColumnEx(this ISheet  ISheet1, int col, List<Range2> ranges)
        {
            foreach (Range2 range in ranges)
            {
                ISheet1.MergeColumn(col, range.Start, range.End, 0);
            }
        }

        public static List<Range2> MergeColumnEx(this ISheet  ISheet1, int col, int startRow, int endRow, int columnSpan,int maxCount=int.MaxValue)
        {
            List < Range2 > result=new List<Range2>();
            object lastValue = null;
            int firstRow = -1;
            int j = startRow;
            int count = 0;
            for (; j <= endRow && j<=ISheet1.LastRowNum; j++)
            {
                count++;
                IRow row = ISheet1.GetRow(j);
                if (row == null) continue;
                ICell cell = row.GetCell(col);
                if (cell == null) continue;
                
                object value = cell.ToString().Trim();
                //if (value != null && value.ToString() == "") continue;

                if (lastValue == null || value.ToString() == "")
                {
                    lastValue = value;
                    firstRow = j;
                }
                else
                {
                    
                    if (lastValue != value || count>= maxCount)
                    {
                        Range2 range2=ISheet1.MergeColumn(col, firstRow, j - 1, columnSpan);
                        result.Add(range2);
                        lastValue = value;
                        firstRow = j;
                        count = 0;

                        
                    }
                }
            }

            Range2 range = ISheet1.MergeColumn(col, firstRow, j - 1, columnSpan);
            result.Add(range);
            return result;
        }

        public static List<Range2> MergeColumn(this ISheet  ISheet1, int col)
        {
            return MergeColumnEx(ISheet1, col, 0, ISheet1.LastRowNum,0);
        }

        public static List<Range2> MergeColumn(this ISheet  ISheet1, int col,int startRow)
        {
            return MergeColumnEx(ISheet1, col, startRow, ISheet1.LastRowNum, 0,-1);
        }

        public static List<Range2> MergeColumn(this ISheet  ISheet1, int col, int startRow,int maxCount)
        {
            int endRow = ISheet1.LastRowNum;
            return MergeColumnEx(ISheet1, col, startRow, endRow, 0, maxCount);
        }

        private static Range2 MergeColumn(this ISheet  ISheet1, int col, int firstRow, int lastRow,int columnSpan)
        {
            int height = 1;
            if (columnSpan > 0)
            {
                height = 0;
            }
            if (lastRow - firstRow >= height)
            {
                ICell cell = ISheet1.GetRow(firstRow).GetCell(col);
                cell.CellStyle.VerticalAlignment = VerticalAlignment.Center;
                ISheet1.AddMergedRegion(new CellRangeAddress(firstRow, lastRow, col, col + columnSpan));
            }
            return new Range2(firstRow, lastRow);
        }

        public static ICell SetCellValue(this ISheet  ISheet1, int rowId, int columnId, object value)
        {
            CellType type=CellType.String;
            Type valueType = value.GetType();
            if (valueType == typeof(double) 
                || valueType == typeof(float) 
                || valueType == typeof(int)
                || valueType == typeof(short)
                || valueType == typeof(long)
                || valueType == typeof(ushort)
                || valueType == typeof(uint)
                || valueType == typeof(ulong)
                )
            {
                type=CellType.Numeric;
            }
            else if (valueType == typeof(bool))
            {
                type=CellType.Boolean;   
            }
            var cell = ISheet1.GetCell(rowId, columnId, type);
            if (cell != null)
            {
                cell.SetCellValueEx(value);
            }
            return cell;
        }

        public static void SetRowHeight(this ISheet  ISheet1,int rowId, int height)
        {
            IRow row=ISheet1.GetRow(rowId);
            if (row == null)
            {
                row=ISheet1.CreateRow(rowId);
            }
            if (row != null)
            {
                row.Height = (short)(height * 256);
            }
        }
    }
}
