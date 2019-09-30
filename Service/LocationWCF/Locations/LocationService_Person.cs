using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using BLL;
using DbModel.Location.AreaAndDev;
using DbModel.Tools;
using Location.BLL.ServiceHelpers;
using Location.Model.DataObjects.ObjectAddList;
using Location.TModel.FuncArgs;
using Location.TModel.Location;
using Location.TModel.Location.AreaAndDev;
using Location.TModel.Location.Data;
using Location.TModel.Location.Obsolete;
using Location.TModel.Location.Alarm;
using Location.TModel.Location.Person;
using Location.TModel.LocationHistory.Data;
using LocationServices.Converters;
using LocationServices.Tools;
using LocationWCFService;
using LocationWCFService.ServiceHelper;
using ConfigArg = Location.TModel.Location.AreaAndDev.ConfigArg;
using DevInfo = Location.TModel.Location.AreaAndDev.DevInfo;
using KKSCode = Location.TModel.Location.AreaAndDev.KKSCode;
using Post = Location.TModel.Location.AreaAndDev.Post;
using Dev_DoorAccess = Location.TModel.Location.AreaAndDev.Dev_DoorAccess;
using TModel.BaseData;
using LocationServices.Locations.Services;


namespace LocationServices.Locations
{
    //人员相关的接口
    public partial class LocationService : ILocationService, IDisposable
    {
        #region 人员列表

        /// <summary>
        /// 获取人员信息
        /// </summary>
        /// <returns></returns>
        //[OperationContract]
        public List<Personnel> GetPersonList(bool isFilterByTag)
        {
            ShowLogEx(">>>>> GetPersonList");
            return new PersonService().GetListEx(isFilterByTag);
        }

        public List<Personnel> FindPersonList(string name)
        {
            try
            {
                return db.Personnels.GetListByName(name).ToWcfModelList();
            }
            catch (Exception ex)
            {
                LogEvent.Error(ex);
                return null;
            }
            
        }

        public Personnel GetPerson(int id)
        {
            try
            {
                ShowLogEx(">>>>> GetPerson id=" + id);
                var person = db.Personnels.Find(id);
                //if (person.Parent == null && person.ParentId!=null)
                //{
                //    person.Parent = db.Departments.Find(person.ParentId);
                //}

                var tPerson = person.ToTModel();

                DbModel.Location.Relation.LocationCardToPersonnel locationCardToPersonnel = db.LocationCardToPersonnels.Find((i) => i.PersonnelId == id);
                if (locationCardToPersonnel != null)
                {
                    tPerson.TagId = locationCardToPersonnel.LocationCardId;
                }
                var tagT = db.LocationCards.Find(tPerson.TagId);
                tPerson.Tag = tagT.ToTModel();

                if (person.Parent == null && person.ParentId != null)
                {
                    var parent = db.Departments.Find(person.ParentId);
                    tPerson.Parent = parent.ToTModel(true);//这里必须加上true，不然会导致死循环崩溃的，这种崩溃连日志都没有
                }
                return tPerson;
            }
            catch (Exception ex)
            {
                LogEvent.Error(ex);
                return null;
            }

        }

        public int AddPerson(Personnel p)
        {
            try
            {
                var dbP = p.ToDbModel();
                bool r = db.Personnels.Add(dbP);
                if (r == false)
                {
                    return -1;
                }
                else
                {
                    if (p.TagId != null)//如果新增的人，设置了定位卡ID。得把关系添加到cardToPersonnel
                    {
                        var s = new PersonService(db);
                        var value = s.BindWithTag(dbP.Id, (int)p.TagId);
                    }
                }
                return dbP.Id;
            }
            catch (Exception ex)
            {
                LogEvent.Error(ex);
                return -1;
            }
           
        }

        public bool EditPerson(Personnel p)
        {
            try
            {
                var s = new PersonService(db);
                var r = s.BindWithTag(p.Id, (p.TagId == null ? 0 : (int)p.TagId));//修改人员和卡的关联关系
                return db.Personnels.Edit(p.ToDbModel());
            }
            catch (Exception ex)
            {
                LogEvent.Error(ex);
                return false;
            }
            
        }

        public bool DeletePerson(int id)
        {
            try
            {
                return db.Personnels.DeleteById(id) != null;
            }
            catch (Exception ex)
            {
                LogEvent.Error(ex);
                return false;
            }
            
        }

        #endregion
    }
}
