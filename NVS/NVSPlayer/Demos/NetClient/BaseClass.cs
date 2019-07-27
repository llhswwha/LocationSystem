using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetClient
{
    public class BaseView
    {
        private string m_sGUID = "";
        private BaseView m_MyParent = null;
        private List<BaseView> m_lstSubViews = new List<BaseView>();

        public virtual void OnMessagePro(int iMsg)
        {

        }

        public void OnNotify(int iMsg)
        {
            try
            {
                OnMessagePro(iMsg);
            }
            catch (Exception ex)
            {
                // 保证其他模块可以正常处理消息
            }


            for (int i = 0; i < m_lstSubViews.Count; i++)
            {
                try
                {
                    m_lstSubViews[i].OnNotify(iMsg);
                }
                catch (Exception ex)
                {
                    continue;
                }

            }
        }

        public BaseView(BaseView _Parent)
        {
            m_MyParent = _Parent;
            m_sGUID = System.Guid.NewGuid().ToString();
        }
        public void AddSubView(BaseView _OneView)
        {
            m_lstSubViews.Add(_OneView);
        }
        public void DeleteSubView(BaseView _OneView)
        {
            for (int i = 0; i < m_lstSubViews.Count; i++)
            {
                if (_OneView.m_sGUID == m_lstSubViews[i].m_sGUID)
                {
                    m_lstSubViews.RemoveAt(i);
                    break;
                }
            }
        }



    }
}
