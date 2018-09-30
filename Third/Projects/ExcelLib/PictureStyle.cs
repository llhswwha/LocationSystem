using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;

namespace ExcelLib
{
    public class PictureStyle
    {
        /// <summary>
        /// dx1,dy1,dx2,dy2都是便宜量，255是一行的高度，一列的默认宽度相当于1000左右
        /// </summary>
        private readonly int _dx1;
        private readonly int _dy1;
        private readonly int _dx2;
        private readonly int _dy2;

        private readonly int _col1;
        private readonly int _row1;
        private readonly int _col2;
        private readonly int _row2;

        public PictureStyle(int dx1, int dy1, int dx2, int dy2, int col1, int row1, int col2, int row2)
        {
            _dx1 = dx1;
            _dy1 = dy1;
            _dx2 = dx2;
            _dy2 = dy2;
            _col1 = col1;
            _row1 = row1;
            _col2 = col2;
            _row2 = row2;
            OriginalSize = false;
        }

        public PictureStyle(int row1, int col1, int row2, int col2)
        {
            _col1 = col1;
            _row1 = row1;
            _col2 = col2;
            _row2 = row2;
            OriginalSize = false;
        }

        public PictureStyle(int row1, int col1)
        {
            _col1 = col1;
            _row1 = row1;
            _col2 = col1+1;
            _row2 = row1+1;
            OriginalSize = true;
        }

        public bool OriginalSize { get; set; }

        public bool Border { get; set; }

        public void SetStyle(bool originalSize, bool hasBorder)
        {
            Border = hasBorder;
            OriginalSize = originalSize;
        }

        public void SetStyle(HSSFPicture picture)
        {
            if (OriginalSize)
            {
                picture.Resize();
            }

            if (Border)
            {
                //picture.LineStyle = HSSFPicture.LINESTYLE_DASHDOTGEL;//虚线，有效果
                picture.LineStyle = LineStyle.Solid;//似乎看不出效果
            }
            else
            {
                picture.LineStyle = LineStyle.None;
            }
        }

        //private int _lineStyle = HSSFPicture.LINESTYLE_NONE;
        //public int LineStyle
        //{
        //    get { return _lineStyle; }
        //    set { _lineStyle = value; }
        //}

        public int Col1
        {
            get { return _col1; }
        }

        public int Row1
        {
            get { return _row1; }
        }

        public int Col2
        {
            get { return _col2; }
        }

        public int Row2
        {
            get { return _row2; }
        }

        public int Dx1
        {
            get { return _dx1; }
        }

        public int Dy1
        {
            get { return _dy1; }
        }

        public int Dx2
        {
            get { return _dx2; }
        }

        public int Dy2
        {
            get { return _dy2; }
        }
    }
}
