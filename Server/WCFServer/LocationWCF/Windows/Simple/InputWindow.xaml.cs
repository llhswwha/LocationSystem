﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace LocationServer.Windows.Simple
{
    /// <summary>
    /// InputWindow.xaml 的交互逻辑
    /// </summary>
    public partial class InputWindow : Window
    {
        public InputWindow()
        {
            InitializeComponent();
        }

        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        public string InputText
        {
            get
            {
                if (Input != null)
                {
                    return Input.Text;
                }
                return null;
            }
            set
            {
                if (Input != null)
                {
                    Input.Text = value;
                }
            }
        }
    }
}
