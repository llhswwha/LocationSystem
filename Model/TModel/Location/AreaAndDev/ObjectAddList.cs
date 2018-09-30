using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Location.TModel.Location.AreaAndDev
{
    /*9.8、获取分类好的模型和对应的模型编号	18020902
 	客户端发送
 #CodeID|18020902|Session$

 	服务器端
 #CodeID|81020902|nRes|strXml$
 nRes: 0 表示成功， -1 表示失败
     strXml 格式：
 <? xml version="1.0"  encoding="gb2312"?>
 <InfoList>
     <Item A = "" >

         < Class  A = "">
             <Model A = "" B = "" />					.
                     .
                     .
                     .
         </Class>
             .
             .
             .
     </Item>
         .
         .
         .
 <InfoList>
 Item：
     大项, 如 机柜、动力设备、装饰
 Class：
 大类, 如 配电柜、办公家具
 Model：
 模型名称和设备类型编号，如 华为_微模块天窗B_3D
 */
    //[DataContract] [Serializable]
    public class ObjectAddList : List<ObjectAddList_Type>
    {

    }

    [DataContract] [Serializable]
    public class ObjectAddList_Type
    {
        /// <summary>
        /// 大项类型
        /// </summary>
        [DataMember]
        public string typeName;

        /// <summary>
        /// 子类型列表
        /// </summary>
        //[XmlArray("Class")]
        [DataMember]
        public List<ObjectAddList_ChildType> childTypeList;
    }

    [DataContract] [Serializable]
    public class ObjectAddList_ChildType
    {
        /// <summary>
        /// 子类型名称
        /// </summary>
        [DataMember]
        public string childTypeName;
        /// <summary>
        /// 模型列表
        /// </summary>
        //[XmlArray("Model")]
        [DataMember]
        public List<ObjectAddList_Model> modelList;//childTypeList
    }

    [DataContract] [Serializable]
    public class ObjectAddList_Model
    {
        /// <summary>
        /// 模型名称
        /// </summary>
        [DataMember]
        public string modelName;

        /// <summary>
        /// 设备类型编号
        /// </summary>
        [DataMember]
        public string typeCode;
    }
}
