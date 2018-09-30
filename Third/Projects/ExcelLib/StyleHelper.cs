using NPOI.HSSF.Util;
using NPOI.SS.UserModel;

namespace ExcelLib
{
    public static class StyleHelper
    {
        /// <summary>
        /// 获取黑边框
        /// </summary>
        /// <param name="hssfworkbook"></param>
        /// <returns></returns>
        public static ICellStyle CreateBlackBorder(this IWorkbook hssfworkbook)
        {
            return hssfworkbook.CreateBorder(new HSSFColor.Black());
        }

        public static ICellStyle CreateBorder(this IWorkbook hssfworkbook, HSSFColor color)
        {
            ICellStyle blackBorder = ICellStyleManager.CreateStyle(hssfworkbook);
            return blackBorder.SetBorder(color);
        }

        public static ICellStyle SetBorder(this ICellStyle blackBorder, HSSFColor color)
        {
            blackBorder.SetBorder();
            blackBorder.BottomBorderColor = color.Indexed;
            blackBorder.LeftBorderColor = color.Indexed;
            blackBorder.RightBorderColor = color.Indexed;
            blackBorder.TopBorderColor = color.Indexed;
            return blackBorder;
        }

        public static ICellStyle SetBorder(this ICellStyle blackBorder)
        {
            blackBorder.BorderBottom = BorderStyle.Thin;
            blackBorder.BorderLeft = BorderStyle.Thin;
            blackBorder.BorderRight = BorderStyle.Thin;
            blackBorder.BorderTop = BorderStyle.Thin;
            return blackBorder;
        }

        public static ICellStyle CreateBackgroundColor(this IWorkbook workbook, HSSFColor foreColor, HSSFColor backColor, FillPattern pattern)
        {
            ICellStyle style1 = ICellStyleManager.CreateStyle(workbook);
            return style1.SetBackgroundColor(foreColor, backColor, pattern);
        }

        public static ICellStyle SetBackgroundColor(this ICellStyle style1, HSSFColor foreColor, HSSFColor backColor, FillPattern pattern)
        {
            style1.FillForegroundColor = foreColor.Indexed;
            style1.FillPattern = pattern;
            style1.FillBackgroundColor = backColor.Indexed;
            return style1;
        }

        public static ICellStyle SetFillColor(this ICellStyle style1, HSSFColor color)
        {
            style1.FillForegroundColor = color.Indexed; 
            style1.FillPattern = FillPattern.SolidForeground;
            return style1;
        }

        public static ICellStyle CreateFillColor(this IWorkbook workbook, HSSFColor color)
        {
            ICellStyle style1 = ICellStyleManager.CreateStyle(workbook);
            return style1.SetFillColor(color);
        }

        public static void SetICellStyle(this ICell cell, short fontSize, bool isBold, bool isBorder, bool isCenter = false)
        {
            var style = ICellStyleManager.GetICellStyle(cell.Sheet.Workbook, fontSize, isBold, isBorder, isCenter);
            cell.CellStyle = style;
        }
    }
}
