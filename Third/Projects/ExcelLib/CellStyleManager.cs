using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPOI.SS.UserModel;

namespace ExcelLib
{
    /// <summary>
    /// 单元格Style管理
    /// Workbook中Style不能超过4000个。
    /// Font不能超过？个（有限制，具体多少没我没测试，6000个是没问题的）。
    /// </summary>
    public static class ICellStyleManager
    {
        /// <summary>
        /// 每次导出文件后（或者ExcelFile构造函数中）调用，不然会出异常：
        /// This Style does not belong to the supplied Workbook. Are you trying to assign a style from one workbook to the ICell of a differnt workbook?
        /// </summary>
        public static void Clear()
        {
            styles.Clear();
            fonts.Clear();
        }

        public static ICellStyle CreateStyle(IWorkbook workbook)
        {
            return workbook.CreateCellStyle();
        }

        private static Dictionary<string, ICellStyle> styles = new Dictionary<string, ICellStyle>();
        
        private static ICellStyle GetStyle(IWorkbook workbook,string styleId)
        {
            if (!styles.ContainsKey(styleId))
            {
                ICellStyle style = CreateStyle(workbook);
                styles.Add(styleId, style);
            }
            return styles[styleId];
        }

        public static ICellStyle GetICellStyle(IWorkbook workbook, short fontSize, bool isBold, bool isBorder, bool isCenter)
        {
            string styleId = GetStyleId(fontSize, isBold, isBorder, isCenter);
            ICellStyle style1 = GetStyle(workbook,styleId);

            var font1 = GetFont(fontSize, isBold, workbook);
            style1.SetFont(font1);

            SetICellStyle(isBorder, isCenter, style1);
            return style1;
        }

        private static string GetStyleId(short fontSize, bool isBold, bool isBorder, bool isCenter)
        {
            return "s:" + fontSize + " " + isBold + " " + isBorder + " " + isCenter;
        }

        private static void SetICellStyle(bool isBorder, bool isCenter, ICellStyle style1)
        {
            if (isBorder)
            {
                StyleHelper.SetBorder(style1);
            }
            if (isCenter)
            {
                style1.Alignment = HorizontalAlignment.Center;
                style1.VerticalAlignment = VerticalAlignment.Center;
            }
        }

        private static Dictionary<string, IFont> fonts = new Dictionary<string, IFont>();
        private static IFont GetFont(short fontSize, bool isBold, IWorkbook workbook)
        {
            string fontId = "s:" + fontSize + " " + isBold;
            if (!fonts.ContainsKey(fontId))
            {
                IFont font1 = CreateFont(fontSize, isBold, workbook);
                fonts.Add(fontId, font1);
            }
            return fonts[fontId];
        }
        private static IFont CreateFont(short fontSize, bool isBold, IWorkbook workbook)
        {
            IFont font1 = workbook.CreateFont();
            if (isBold)
            {
                font1.Boldweight = (short)FontBoldWeight.Bold;
            }
            font1.FontHeightInPoints = fontSize;
            return font1;
        }
    }
}
