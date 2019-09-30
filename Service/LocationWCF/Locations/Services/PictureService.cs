using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TModel.Location.AreaAndDev;
using BLL;
using BLL.Blls.Location;
using Location.BLL.Tool;

namespace LocationServices.Locations.Services
{
    public interface IPictureService
    {
        /// <summary>
        /// 添加图片信息
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        bool EditPictureInfo(Picture pc);

        /// <summary>
        /// 获取图片信息
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        Picture GetPictureInfo(string strPictureName);

        /// <summary>
        /// 获取首页图片名称
        /// </summary>
        /// <returns></returns>
        List<string> GetHomePageNameList();

        /// <summary>
        /// 根据图片名称获取首页图片信息
        /// </summary>
        /// <param name="strPictureName"></param>
        /// <returns></returns>
        byte[] GetHomePageByName(string strPictureName);
    }

    public class PictureService : IPictureService
    {
        private Bll db;
        private PictureBll dbSet;
        public PictureService()
        {
            db = Bll.NewBllNoRelation();
            dbSet = db.Pictures;
        }
        public bool EditPictureInfo(Picture pc)
        {
            return db.Pictures.Update(pc.Name, pc.Info);
        }

        public byte[] GetHomePageByName(string strPictureName)
        {
            string strPath = AppDomain.CurrentDomain.BaseDirectory + "\\Data\\HomePages\\" + strPictureName;
            byte[] byteArray = LocationServices.Tools.ImageHelper.LoadImageFile(strPath);
            return byteArray;
        }

        public List<string> GetHomePageNameList()
        {
            try
            {
                Bll bll = Bll.NewBllNoRelation();
                List<string> lst = bll.HomePagePictures.DbSet.Select(p => p.Name).ToList();
                //if (lst == null || lst.Count == 0)
                //{
                //    lst = new List<string>();
                //}

                if (lst.Count == 0)
                {
                    lst = null;
                }

                return lst;
            }
            catch (Exception ex)
            {
                Log.Error("LocationService.GetHomePageNameList", ex);
                return null;
            }
        }

        public Picture GetPictureInfo(string strPictureName)
        {
            DbModel.Location.AreaAndDev.Picture pc2 = db.Pictures.DbSet.FirstOrDefault(p => p.Name == strPictureName);
            if (pc2 == null)
            {
                pc2 = new DbModel.Location.AreaAndDev.Picture();
            }
            var item2 = new Picture();
            item2.Name = pc2.Name;
            item2.Info = pc2.Info;
            return item2;
        }
    }
}
