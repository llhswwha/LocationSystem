using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;

namespace ExcelLib
{
    public static class PictureHelper
    {
        public static void AddPicture(this ISheet  ISheet1, string imagePath, PictureStyle style)
        {
            var patriarch = GetPatriarch(ISheet1);
            var anchor = CreateClientAnchor(style.Dx1, style.Dy1, style.Dx2, style.Dy2, style.Col1, style.Row1, style.Col2, style.Row2, AnchorType.MoveDontResize);
            //load the picture and get the picture index in the workbook
            HSSFPicture picture = (HSSFPicture)patriarch.CreatePicture(anchor, LoadImage(imagePath, ISheet1.Workbook));
            style.SetStyle(picture);
            //Reset the image to the original size.
            //picture.Resize();
            //picture.LineStyle = HSSFPicture.LINESTYLE_NONE;
        }

        private static HSSFPatriarch GetPatriarch(ISheet  ISheet1)
        {
            HSSFPatriarch patriarch = (HSSFPatriarch)ISheet1.DrawingPatriarch;
            if (patriarch == null)
                patriarch = (HSSFPatriarch)ISheet1.CreateDrawingPatriarch();
            return patriarch;
        }

        public static void AddPicture(this ISheet  ISheet1, Stream imageStream, PictureStyle style)
        {
            var patriarch = GetPatriarch(ISheet1);
            var anchor = CreateClientAnchor(style.Dx1, style.Dy1, style.Dx2, style.Dy2, style.Col1, style.Row1, style.Col2, style.Row2, AnchorType.MoveDontResize);
            //load the picture and get the picture index in the workbook
            HSSFPicture picture = (HSSFPicture)patriarch.CreatePicture(anchor, LoadImage(imageStream, ISheet1.Workbook));
            style.SetStyle(picture);
            //Reset the image to the original size.
            //picture.Resize();
            //picture.LineStyle = HSSFPicture.LINESTYLE_NONE;
        }

        /// <summary>
        /// create the anchor
        /// </summary>
        /// <param name="dx1"></param>
        /// <param name="dy1"></param>
        /// <param name="dx2"></param>
        /// <param name="dy2"></param>
        /// <param name="col1"></param>
        /// <param name="row1"></param>
        /// <param name="col2"></param>
        /// <param name="row2"></param>
        /// <param name="anchorType"></param>
        /// <returns></returns>
        private static HSSFClientAnchor CreateClientAnchor(int dx1, int dy1, int dx2, int dy2, int col1, int row1, int col2, int row2, AnchorType anchorType)
        {
            HSSFClientAnchor anchor;
            anchor = new HSSFClientAnchor(dx1, dy1, dx2, dy2, col1, row1, col2, row2);
            anchor.AnchorType = anchorType;
            return anchor;
        }

        public static int LoadImage(string path, IWorkbook wb)
        {
            FileStream file = new FileStream(path, FileMode.Open, FileAccess.Read);
            return LoadImage(file,wb);
        }

        private static int LoadImage(Stream stream,IWorkbook wb)
        {
            byte[] buffer = new byte[stream.Length];
            stream.Read(buffer, 0, (int)stream.Length);
            return wb.AddPicture(buffer, PictureType.JPEG);
        }

        public static void SetBackColor(this ICell cell, HSSFColor color)
        {
            var style1 = cell.Sheet.Workbook.CreateFillColor(color);
            cell.CellStyle = style1;
        }

        //NPOI.HSSF.Util.HSSFColor.BLUE.index
        public static void SetBackColor(this ISheet  ISheet1, int rowId, int columnId, HSSFColor color)
        {
            var style1 = ISheet1.Workbook.CreateFillColor(color);
            ISheet1.CreateRow(rowId).CreateCell(columnId).CellStyle = style1;
        }
    }
}
