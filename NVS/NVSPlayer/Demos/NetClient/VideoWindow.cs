using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NetClient
{
    public partial class VideoWindow : UserControl
    {
        public VideoWindow()
        {
            InitializeComponent();
        }

        private void VideoWindow_Resize(object sender, EventArgs e)
        {
            pnlVideo.Left = picVideo.Left+1;
            pnlVideo.Top = picVideo.Top+1;
            pnlVideo.Width = picVideo.Width-2;
            pnlVideo.Height = picVideo.Height-2;
        }
    }
}
