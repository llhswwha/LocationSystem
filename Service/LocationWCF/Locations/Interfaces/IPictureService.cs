using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using TModel.Location.AreaAndDev;

namespace LocationServices.Locations.Interfaces
{
    [ServiceContract]
    public interface IPictureService
    {
        /// <summary>
        /// 添加图片信息
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        [OperationContract]
        bool EditPictureInfo(Picture pc);

        /// <summary>
        /// 获取图片信息
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        [OperationContract]
        Picture GetPictureInfo(string strPictureName);

        /// <summary>
        /// 获取首页图片名称
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<string> GetHomePageNameList();

        /// <summary>
        /// 根据图片名称获取首页图片信息
        /// </summary>
        /// <param name="strPictureName"></param>
        /// <returns></returns>
        [OperationContract]
        byte[] GetHomePageByName(string strPictureName);
    }
}
