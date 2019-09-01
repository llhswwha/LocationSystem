using System.Collections.Generic;
using System.Linq;
using DbModel.Location.AreaAndDev;
using Location.TModel.Location.AreaAndDev;
using Location.BLL.Tool;

namespace LocationServices.Tools
{
    public class ObjectAddListService
    {
        /// <summary>
        /// 模型大项列表
        /// </summary>
        Dictionary<string, List<ModelTypeItem>> modelItemDic = new Dictionary<string, List<ModelTypeItem>>();
        /// <summary>
        /// 模型大类列表
        /// </summary>
        Dictionary<string, List<ModelTypeItem>> modelClassDic = new Dictionary<string, List<ModelTypeItem>>();

        /// <summary>
        /// 获取模型添加列表
        /// </summary>
        /// <param name="modelList"></param>
        /// <param name="typeList"></param>
        /// <returns></returns>
        public ObjectAddList GetObjectAddList(List<DevModel> modelList, List<DevType> typeList)
        {
            try
            {
                //List<t_SetModel> modelList = db.t_SetModels.ToList();
                //List<t_Template_TypeProperty> typeList = db.t_TypeProperties.ToList();
                var ModelTypeList = from model in modelList
                                    join type in typeList on (model.ModelId + "_前面板_0.png") equals type.FrontElevation into TempTable
                                    from c in TempTable.DefaultIfEmpty()
                                    where model.Items != "" && model.Class != ""
                                    orderby model.Items, model.Class
                                    select new ModelTypeItem
                                    {
                                        Item = model.Items,
                                        Class = model.Class,
                                        Name = model.Name,
                                        nType = c == null ? "0" : c.TypeCode.ToString()
                                    };
                ObjectAddList tempList = new ObjectAddList();



                modelItemDic.Clear();
                modelClassDic.Clear();
                //List<ModelTypeItem> itemListTemp;
                foreach (var item in ModelTypeList)
                {
                    if (!modelItemDic.ContainsKey(item.Item))
                    {
                        List<ModelTypeItem> itemListTemp = new List<ModelTypeItem>();
                        itemListTemp.Add(item);
                        modelItemDic.Add(item.Item, itemListTemp);
                    }
                    else
                    {
                        List<ModelTypeItem> itemListTemp;
                        modelItemDic.TryGetValue(item.Item, out itemListTemp);
                        if (itemListTemp != null)
                            itemListTemp.Add(item);
                    }
                    if (!modelClassDic.ContainsKey(item.Class))
                    {
                        List<ModelTypeItem> itemListTemp = new List<ModelTypeItem>();
                        itemListTemp.Add(item);
                        modelClassDic.Add(item.Class, itemListTemp);
                    }
                    else
                    {
                        List<ModelTypeItem> itemListTemp;
                        modelClassDic.TryGetValue(item.Class, out itemListTemp);
                        if (itemListTemp != null)
                            itemListTemp.Add(item);
                    }
                }
                foreach (KeyValuePair<string, List<ModelTypeItem>> Items in modelItemDic)
                {
                    ObjectAddList_Type t = new ObjectAddList_Type();
                    t.typeName = Items.Key;
                    t.childTypeList = new List<ObjectAddList_ChildType>();
                    foreach (var item in Items.Value)
                    {
                        ObjectAddList_ChildType t2 = new ObjectAddList_ChildType();
                        t2.childTypeName = item.Class;
                        List<ModelTypeItem> ClassTemp;
                        modelClassDic.TryGetValue(item.Class, out ClassTemp);
                        if (ClassTemp != null)
                        {
                            t2.modelList = new List<ObjectAddList_Model>();
                            foreach (var model in ClassTemp)
                            {
                                ObjectAddList_Model modelT = new ObjectAddList_Model();
                                modelT.modelName = model.Name;
                                modelT.typeCode = model.nType;
                                t2.modelList.Add(modelT);
                            }
                            modelClassDic.Remove(item.Class);
                            t.childTypeList.Add(t2);
                        }
                    }
                    tempList.Add(t);
                }
                return tempList;
            }
            catch (System.Exception ex)
            {
                Log.Error(LogTags.DbGet, "Exception:" + ex);
                return null;
            }
        }
        /// <summary>
        /// 获取模型列表
        /// </summary>
        /// <param name="modelList"></param>
        /// <param name="typeList"></param>
        /// <returns></returns>
        public ObjectAddList GetObjectAddListEx(List<DevModel> modelList, List<DevType> typeList)
        {
            try
            {
                var ModelTypeList = from model in modelList
                                    join type in typeList on (model.ModelId + "_前面板_0.png") equals type.FrontElevation into TempTable
                                    from c in TempTable.DefaultIfEmpty()
                                    where !string.IsNullOrEmpty(model.Items) && !string.IsNullOrEmpty(model.Class)
                                    orderby model.Items, model.Class
                                    select new ModelTypeItem
                                    {
                                        Item = model.Items,
                                        Class = model.Class,
                                        Name = model.Name,
                                        nType = c == null ? "0" : c.TypeCode.ToString()
                                    };

                modelItemDic.Clear();
                modelClassDic.Clear();
                foreach (var item in ModelTypeList)
                {
                    InitDic(modelItemDic, item.Item, item);
                    InitDic(modelClassDic, item.Class, item);
                }
                ObjectAddList tempList = new ObjectAddList();
                foreach (KeyValuePair<string, List<ModelTypeItem>> Items in modelItemDic)
                {
                    ObjectAddList_Type t = new ObjectAddList_Type();
                    t.typeName = Items.Key;
                    t.childTypeList = new List<ObjectAddList_ChildType>();
                    foreach (var item in Items.Value)
                    {
                        ObjectAddList_ChildType t2 = new ObjectAddList_ChildType();
                        t2.childTypeName = item.Class;
                        List<ModelTypeItem> ClassTemp;
                        modelClassDic.TryGetValue(item.Class, out ClassTemp);
                        if (ClassTemp != null && ClassTemp.Count != 0)
                        {
                            t2.modelList = new List<ObjectAddList_Model>();
                            List<ModelTypeItem> modelListTemp = new List<ModelTypeItem>();
                            foreach (var model in ClassTemp)
                            {
                                if (model.Item != t.typeName) continue;  //同一个大类的模型，可能不属于同一个大项
                                modelListTemp.Add(model);
                            }
                            foreach (var model in modelListTemp)
                            {
                                ObjectAddList_Model modelT = new ObjectAddList_Model();
                                modelT.modelName = model.Name;
                                modelT.typeCode = model.nType;
                                t2.modelList.Add(modelT);
                                ClassTemp.Remove(model);
                            }
                            t.childTypeList.Add(t2);
                        }
                    }
                    tempList.Add(t);
                }
                tempList = tempList.Count == 0 ? null : tempList;
                return tempList;
            }
            catch (System.Exception ex)
            {
                Log.Error(LogTags.DbGet, "Exception:" + ex);
                return null;
            }
        }
        /// <summary>
        /// 初始化数据字典
        /// </summary>
        /// <param name="ModelDic"></param>
        /// <param name="KeyName"></param>
        /// <param name="item"></param>
        private void InitDic(Dictionary<string, List<ModelTypeItem>> ModelDic, string KeyName, ModelTypeItem item)
        {
            try
            {
                if (string.IsNullOrEmpty(KeyName)) return;
                if (!ModelDic.ContainsKey(KeyName))
                {
                    List<ModelTypeItem> ListTemp = new List<ModelTypeItem>();
                    ListTemp.Add(item);
                    ModelDic.Add(KeyName, ListTemp);
                }
                else
                {
                    List<ModelTypeItem> itemListTemp;
                    ModelDic.TryGetValue(KeyName, out itemListTemp);
                    if (itemListTemp != null)
                        itemListTemp.Add(item);
                }
            }
            catch (System.Exception ex)
            {
                Log.Error(LogTags.DbGet, "Exception:" + ex);

            }
        }
    }
}
