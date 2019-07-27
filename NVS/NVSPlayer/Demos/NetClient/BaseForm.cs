using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NetClient
{
    public partial class BaseForm : Form
    {
        private string m_sGUID = "";
        private BaseForm m_Parent = null;
        private List<BaseForm> m_lNotifyList = new List<BaseForm>();

        public BaseForm()
        {
            InitializeComponent();

        }
        public BaseForm(BaseForm _ParentForm)
        {
            InitializeComponent();
            m_Parent = _ParentForm;
            m_sGUID = System.Guid.NewGuid().ToString();
            if (_ParentForm != null)
            {
                m_Parent.AddSubForm(this);
            }
            
        }

        public virtual void OnMessagePro(IntPtr wParam, IntPtr lParam)
        {

        }

        public void Notify(IntPtr wParam, IntPtr lParam)
        {
            try
            {
                OnMessagePro(wParam, lParam);
            }
            catch (Exception ex)
            {
                // 保证其他模块可以正常处理消息
            }


            for (int i = 0; i < m_lNotifyList.Count; i++)
            {
                try
                {
                    m_lNotifyList[i].Notify(wParam, lParam);
                }
                catch (Exception ex)
                {
                    continue;
                }

            }
        }


        public void AddSubForm(BaseForm _OneForm)
        {
            m_lNotifyList.Add(_OneForm);
        }
        public void DeleteSubForm(BaseForm _OneForm)
        {
            for (int i = 0; i < m_lNotifyList.Count; i++)
            {
                if (_OneForm.m_sGUID == m_lNotifyList[i].m_sGUID)
                {
                    m_lNotifyList.RemoveAt(i);
                    break;
                }
            }
        }

        private void BaseForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (m_Parent != null)
            {
                m_Parent.DeleteSubForm(this);
            }
        }
    }
}
