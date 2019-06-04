using Location.TModel.Location.AreaAndDev;
using LocationServices.Converters;
using LocationServices.Locations.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocationServices.Locations
{
    //岗位相关的接口
    public partial class LocationService : ILocationService, IDisposable
    {
        public static int count = 0;

        public IList<Tag> GetTags()
        {
            ShowLogEx(">>>>> GetTags:" + count);
            count++;
            IList<Tag> tagsT = new TagService(db).GetList(true);
            //string xml = XmlSerializeHelper.GetXmlText(tagsT);
            //int length = xml.Length;
            //ShowLog("<<<<< GetTags:"+ length);
            return tagsT;
            //return new TagService(db).GetList(true);
        }

        public Tag GetTag(int id)
        {
            return new TagService(db).GetEntity(id + "");
        }

        /// <summary>
        /// 添加标签
        /// </summary>
        /// <returns></returns>
        public int AddTag(Tag tag)
        {
            var entity= new TagService(db).Post(tag);
            if (entity != null)
            {
                return entity.Id;
            }
            return -1;
        }

        /// <summary>
        /// 添加标签
        /// </summary>
        /// <returns></returns>
        public bool EditTag(Tag tag)
        {
            var service=new TagService(db);
            var entity = service.Put(tag);
            return entity != null;
        }

        /// <summary>
        /// 添加标签
        /// </summary>
        /// <returns></returns>
        public bool AddTags(List<Tag> tags)
        {
            return new TagService(db).AddList(tags);
        }

        /// <summary>
        /// 删除某一个标签
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteTag(int id)
        {
            return new TagService(db).Delete(id + "") != null;
        }

        /// <summary>
        /// 清空标签数据库表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteAllTags()
        {
            return new TagService(db).DeleteAll();
        }

        public bool EditTagById(Tag Tag, int? id)
        {
            bool bReturn = false;
            var lc = db.LocationCards.FirstOrDefault(p => p.Code == Tag.Code);
            if (lc == null)
            {
                lc = Tag.ToDbModel();
                lc.Abutment_Id = id;
                bReturn = db.LocationCards.Add(lc);
            }
            else
            {
                lc.Name = Tag.Name;
                lc.Describe = Tag.Describe;
                lc.Abutment_Id = id;
                lc.IsActive = Tag.IsActive;
                bReturn = db.LocationCards.Edit(lc);
            }

            return bReturn;
        }

        public bool EditBusTag(Tag Tag)
        {
            bool bDeal = false;
            int nFlag = 0;
            var btag = db.bus_tags.FirstOrDefault(p => p.tag_id == Tag.Code);
            if (btag == null)
            {
                btag = new DbModel.Engine.bus_tag();
                nFlag = 1;
            }

            btag.tag_id = Tag.Code;

            if (nFlag == 0)
            {
                bDeal = db.bus_tags.Edit(btag);

            }
            else
            {
                bDeal = db.bus_tags.Add(btag);
            }

            if (!bDeal)
            {
                return bDeal;
            }

            bDeal = EditTagById(Tag, btag.Id);

            return bDeal;
        }
    }
}
