using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DbModel.Location.Authorizations;

namespace BLL.Initializers
{
    public class CardRoleInitializer
    {
        private Bll _bll;
        public CardRoleInitializer(Bll bll)
        {
            _bll = bll;
        }

        private CardRole AddCardRole(string name, string description = "")
        {
            if (string.IsNullOrEmpty(description))
            {
                description = name;
            }
            var role = new CardRole() { Name = name, Description = description };
            _bll.CardRoles.Add(role);
            roles.Add(role);
            return role;
        }

        public CardRole role1;
        public CardRole role2;
        public CardRole role3;
        public CardRole role4;
        public CardRole role5;
        public CardRole role6;
        public CardRole role7;
        public CardRole role8;

        public List<CardRole> roles = new List<CardRole>();

        public void InitData()
        {
            role1 = AddCardRole("超级管理员", "特殊角色，可以进入全部区域。");
            role2 = AddCardRole("管理人员");
            role3 = AddCardRole("巡检人员", "能够进入生产区域");
            role4 = AddCardRole("操作人员", "能够进入生产区域");
            role5 = AddCardRole("维修人员", "能够进入生产区域");
            role6 = AddCardRole("外维人员", "能够进入生活区域和指定生产区域");
            role7 = AddCardRole("参观人员(高级)", "能够进入生活区域和大部分生产区域");
            role8 = AddCardRole("参观人员(一般)", "能够进入生活区域和少部分生产区域");
        }
    }
}
