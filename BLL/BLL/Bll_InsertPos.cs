using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DbModel.Location.AreaAndDev;
using DbModel.Location.Data;
using DbModel.Location.Person;
using DbModel.Location.Relation;
using DbModel.LocationHistory.Data;
using Location.BLL.Tool;
using System.Threading;
using DbModel;
using SelfBatchImport;
using System.Collections.Concurrent;
using BLL.Tools;
using LocationServer;

namespace BLL
{

    //修改历史数据和实时数据用的代码
    public partial class Bll
    {

        /// <summary>
        /// 根据标签信息初始化实时位置信息表，这样跟定位引擎对接时就不用每次都判断是否是新增还是修改了
        /// </summary>
        public List<LocationCard> InitTagPosition(int mockPowerCount)
        {
            List<LocationCard> tags = LocationCards.ToList();
            if (tags == null) tags = new List<LocationCard>();

             var mockTags = GetMockTags(mockPowerCount, tags); //生成模拟数据，测试大数据量，mockPowerCount = 100的话，2个变成200个
            tags.AddRange(mockTags);

            AddTagPositionsByTags(tags);

            return tags;
        }

        public void AddTagPositionsByTags(List<LocationCard> tags)
        {
            return;//cww_2019_01_18:不插入无位置的标签位置
            var tagPosList = LocationCardPositions.ToList();//事先取出全部到内存中，比每次都到数据库中查询快很多。 100个从6.4s->1.8s,1.8s中主要是第一次查询的一些初始工作
            var newPosList = new List<LocationCardPosition>();

            foreach (var tag in tags)
            {
                //TagPosition tagPos = TagPositions.FindByCode(tag.Code);//100个要2s
                var tagPos = tagPosList.Find(i => i.Id == tag.Code);//判断是否存在实时数据
                if (tagPos == null)
                {
                    var tagPosition = new LocationCardPosition(tag.Code);
                    newPosList.Add(tagPosition);
                }
            }

            //根据标签数量初始化位置信息，插入没有位置信息的标签位置
            foreach (var tp in newPosList)
            {
                LocationCardPositions.Add(tp);
            }
        }

        /// <summary>
        /// 生成模拟数据，测试大数据量，mockPowerCount=100的话，2个变成200个
        /// </summary>
        /// <param name="mockPowerCount"></param>
        /// <param name="tags"></param>
        /// <returns></returns>
        private static List<LocationCard> GetMockTags(int mockPowerCount, List<LocationCard> tags)
        {
            List<LocationCard> mockTags = new List<LocationCard>();
            if (tags == null) return mockTags;
            foreach (var tag in tags)
            {
                for (int i = 0; i < mockPowerCount; i++)
                {
                    var mockTag = new LocationCard();
                    mockTag.Code = tag.Code + i;
                    mockTag.Name = tag.Name + i;
                    mockTag.Describe = "模拟数据" + i;
                    mockTags.Add(mockTag);
                }
            }
            return mockTags;
        }

        public bool AddPositionsBySql(List<Position> positions)
        {
            try
            {
                string sql = GetInsertSql(positions);
                if (!string.IsNullOrEmpty(sql))
                    DbHistory.Database.ExecuteSqlCommand(sql);

                List<LocationCardPosition> tagPosList = EditTagPositionList(positions);
                string sql2 = GetUpdateSql(tagPosList);
                if (!string.IsNullOrEmpty(sql2))
                    Db.Database.ExecuteSqlCommand(sql2);
                return true;
            }
            catch (Exception ex)
            {
                Log.Error("AddPositionsBySql", ex);
                return false;
            }
        }

        /// <summary>
        /// 删除重复的数据
        /// </summary>
        /// <param name="list1"></param>
        /// <returns></returns>
        private List<Position> RemoveRepeatPosition(List<Position> list1)
        {
            //var tagPositions = positionBll.LocationCardPositions.ToList();//实时位置
            ////剔除位置信息不变的部分
            //List<Position> list2 = new List<Position>();
            //foreach (var pos in list1)
            //{
            //    var tagPos = tagPositions.Find(i => i.Code == pos.Code);
            //    if (tagPos != null)
            //    {
            //        double distance = (tagPos.X - pos.X)*(tagPos.X - pos.X) + (tagPos.Z - pos.Z)*(tagPos.Z - pos.Z);
            //        if (distance > 1)
            //        {
            //            list2.Add(pos);
            //        }
            //    }
            //}

            Dictionary<string, Position> dict = new Dictionary<string, Position>();
            foreach (Position pos in list1)
            {
                if (pos == null) continue;
                try
                {
                    dict[pos.Code] = pos;
                }
                catch (Exception ex)
                {
                    Log.Error("RemoveRepeatPosition", ex);
                }
            }
            return dict.Values.ToList();
        }

        private TagRelationBuffer tagRelation;

        public void UpdateBuffer()
        {
            if (tagRelation != null)
            {
                tagRelation.ForceLoadData();
            }
        }

        public bool AddPositionsEx(List<Position> positions)
        {
            //1.删除重复的位置信息，只留最新的部分
            var positions2 = RemoveRepeatPosition(positions);
            positions.Clear();
            positions.AddRange(positions2);
            //2.处理定位引擎位置信息，添加关联人员信息
            //var tagRelation = new TagRelationBuffer(this);
            if (tagRelation == null)
            {
                //tagRelation = new TagRelationBuffer();
                tagRelation = TagRelationBuffer.Instance();
            }
            tagRelation.SetPositionInfo(positions2);//设置标签、人员和区域
            //3.写入数据库
            return AddPositions(positions2);
        }

        /// <summary>
        /// 写入位置信息
        /// </summary>
        /// <param name="positions"></param>
        /// <returns></returns>
        public bool AddPositions(List<Position> positions)
        {
            bool r = true;
            try
            {
                if (positions == null || positions.Count == 0) return true;

                //AddPositionsBySql(positions);//在Z.EntityFramework工具不能使用的情况下，自己写的简单的拼接sql语句。Z.EntityFramework能用的话，这个就不需要。

                if (PartitionThread == null)
                {
                    int count2 = DbHistory.U3DPositions.Count();

                    PartitionThread = new Thread(InsertPartitionInfo);
                    PartitionThread.IsBackground = true;
                    PartitionThread.Start();

                    while (bPartitionInitFlag) { }//
                }

                //AddPositionToHistory(positions);
                AddPositionToHistoryAsync(positions);

                //修改实时数据
                EditTagPositionListOP(positions);
            }
            catch (Exception ex)
            {
                r = false;
                ErrorMessage = ex.Message;
            }
            return r;
        }

        private ConcurrentBag<Position> temp = new ConcurrentBag<Position>();

        /// <summary>
        /// 插入历史数据用线程
        /// </summary>
        private static Thread AddPostionThread;

        private void AddPositionToHistoryAsync(List<Position> positions)
        {
            try
            {
                if (AddPostionThread == null)
                {
                    AddPostionThread = new Thread(() =>
                    {
                        while (true)
                        {
                            try
                            {
                                Thread.Sleep(AppSetting.AddHisPositionInterval);//历史数据10s插入一次,其实60s甚至更久也可以的。
                                lock (temp) //怀疑和lock有关
                                {
                                    if (temp.Count > 0)
                                    {
                                        Dictionary<string,Position> posList2 = new Dictionary<string, Position>();//Set
                                        //posList2.AddRange(temp);
                                        foreach (var item in temp)
                                        {
                                            string key = item.Code + item.DateTimeStamp;
                                            if (posList2.ContainsKey(key))//相同卡的相同时间戳
                                            {
                                                Log.Error("AddPositions", "收到重复数据:"+key);
                                            }
                                            else
                                            {
                                                posList2.Add(key, item);
                                            }
                                        }

                                        var posList3 = posList2.Values.ToList();
                                        bool r1 = AddPositionToHistory(posList3);
                                        if (r1)
                                        {
                                            temp = new ConcurrentBag<Position>();
                                            Log.Info("AddPositions", string.Format("插入 count:{0},r:{1}", posList2.Count, r1));
                                        }
                                        else
                                        {
                                            Thread.Sleep(100);
                                            if (ErrorMessage.Contains("Table has no partition for value"))
                                            {
                                                AddPartion();//姑且尝试添加分区
                                            }
                                            temp = new ConcurrentBag<Position>();
                                            //插入失败了，也要清空，不然数据会一直积累，插入重复相同的数据。
                                            //怀疑，数据实际上是插入成功了的。
                                            //怀疑，mysql连接池/线程池满了导致的插入失败。
                                            Log.Error("AddPositions", string.Format("插入历史数据失败 count:{0},r:{1}", posList2.Count, r1));
                                        }
                                    }
                                    else
                                    {
                                        Log.Info("AddPositions", "Wait");
                                    }
                                }
                            }
                            catch (Exception ex)
                            {

                                Log.Error("AddPositions", "Exception1:" + ex);
                            }

                        }
                    });
                    AddPostionThread.IsBackground = true;
                    AddPostionThread.Start();
                }

                foreach (var item in positions)
                {
                    temp.Add(item);
                }
            }
            catch (Exception ex)
            {
                Log.Error("AddPositions", "Exception2:"+ex);
            }
            
        }

        private bool AddPositionToHistory(List<Position> positions)
        {
            ////1.批量插入历史数据数据
            //DbHistory.BulkInsert(positions);//插件Z.EntityFramework.Extensions功能
            //DbHistory.Positions.AddRange(positions);
            bool r1 = this.Positions.AddRange(positions,3);
            //if (r1)
            //{
            //    bool r2 = BatchImport.Insert(Positions.Db.Database, Positions.DbSet, positions);//自己写的创建sql语句
            //}
            //不用BatchImport.Insert，因为它现在没有事务，部分插入失败，不会回滚
            //bool r2=BatchImport.Insert(Positions.Db.Database, Positions.DbSet, positions);//自己写的创建sql语句
            //if (r2 == false)
            //{
            //    Log.Error("Bll_InsertPos.AddPositions", "BatchImport.Insert Error:" + BatchImport.Error);
            //}
            return r1;
        }

        private string GetInsertSql(List<Position> positions)
        {
            string sql = "";
            foreach (Position p in positions)
            {
                if (p == null) continue;
                sql +=
                    string.Format(
                        "insert into Positions (Code,X,Y,Z,DateTimeStamp,Power,Number,Flag) values ( '{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}') ",
                        p.Code, p.X, p.Y, p.Z, p.DateTimeStamp, p.Power, p.Number, p.Flag);
            }
            return sql;
        }

        private string GetUpdateSql(List<LocationCardPosition> positions)
        {
            string sql = "";
            foreach (LocationCardPosition p in positions)
            {
                if (p == null) continue;
                sql +=
                    string.Format(
                        "UPDATE TagPositions SET X = '{1}',Y ='{2}',Z='{3}',DateTimeStamp='{4}',Power='{5}',Number='{6}',Flag='{7}' where Code='{0}' ",
                        p.Id, p.X, p.Y, p.Z, p.DateTimeStamp, p.Power, p.Number, p.Flag);
            }
            return sql;
        }

        public async Task AddPositionsAsyc(List<Position> positions)
        {

            string sql = GetInsertSql(positions);
            if (!string.IsNullOrEmpty(sql))
                await DbHistory.Database.ExecuteSqlCommandAsync(sql);
            List<LocationCardPosition> tagPosList = EditTagPositionList(positions);
            string sql2 = GetUpdateSql(tagPosList);
            if(!string.IsNullOrEmpty(sql2))
                await Db.Database.ExecuteSqlCommandAsync(sql2);

            //批量插入历史数据数据
            //await DbHistory.BulkInsertAsync(positions); //插件Z.EntityFramework.Extensions功能

            //获取并修改列表
            //List<TagPosition> tagPosList = EditTagPositionList(positions);

            ////更新列表
            //await TagPositions.Db.BulkUpdateAsync(tagPosList);//插件Z.EntityFramework.Extensions功能
        }

        private List<LocationCardPosition> EditTagPositionList(List<Position> positions)
        {
            //1.获取列表
            List<LocationCardPosition> tagPosList = LocationCardPositions.ToList();
            List<LocationCardPosition> changedTagPosList = new List<LocationCardPosition>();
            //2.修改数据
            for (int i = 0; i < positions.Count; i++)
            {
                Position position = positions[i];
                if (position == null) continue;//位置信息可能有null
                var tagPos = tagPosList.Find(item => item.Id == position.Code);
                if (tagPos == null) continue;
                tagPos.Edit(position);
                if (!changedTagPosList.Contains(tagPos))
                {
                    changedTagPosList.Add(tagPos);
                }
            }
            return changedTagPosList;
        }

        private LocationCard GetChangedCard(Dictionary<string, LocationCard> dict,Position position)
        {
            LocationCard lc = null;
           
            if (dict != null && dict.ContainsKey(position.Code))
            {
                lc = dict[position.Code];
            }
            if (lc == null) return null;

            if (lc.Flag != position.Flag || lc.Power != position.Power)//标志和电压发生变化
            {
                lc.Flag = position.Flag;
                lc.Power = position.Power;
                if (lc.Power >= AppSetting.LowPowerFlag)
                {
                    lc.PowerState = 0;
                }
                else
                {
                    lc.PowerState = 1;//低电告警状态
                }
                //editCardList.Add(lc);
                //LocationCards.Edit(lc);
                return lc;
            }
            else
            {
                return null; 
            }
        }

        private void EditTagPositionListOP(List<Position> positions)
        {
            //1.获取列表
            var tagPosList = LocationCardPositions.ToDictionary();
            List<LocationCardPosition> changedTagPosList = new List<LocationCardPosition>();
            //Dictionary<string, LocationCard> dict = LocationCards.ToDictionaryByCode();//放在TagRelationBuffer中
            List<LocationCardPosition> newTagPosList = new List<LocationCardPosition>();

            List<LocationCard> editCardList = new List<LocationCard>();
            Dictionary<string, LocationCard> dict = TagRelationBuffer.Instance().GetLocationCardDic();

            var maxSpeed = AppContext.MoveMaxSpeed;
            

            //2.修改数据
            for (int i = 0; i < positions.Count; i++)
            {
                Position position = positions[i];
                if (position == null) continue;//位置信息可能有null
                //LocationCard lc = LocationCards.Where(p=>p.Code == position.Code).FirstOrDefault();
                LocationCard lc = GetChangedCard(dict, position);
                if (lc != null)
                {
                    editCardList.Add(lc);
                }

                if (tagPosList.ContainsKey(position.Code))
                {
                    var tagPos = tagPosList[position.Code];

                    if (maxSpeed >0)
                    {
                        var speed = PosDistanceUtil.GetSpeed(tagPos, position);
                        
                        if (speed > maxSpeed)  //判断错误点
                        {
                            PosDistance dis = new PosDistance(tagPos, position);
                            //这个点就不用来修改实时位置了
                            Log.Info("RealPos",string.Format("发现错误点:{0}", dis));

                            positions.RemoveAt(i);
                            i--;
                        }
                        else
                        {
                            tagPos.Edit(position);//修改实时位置数据
                            if (!changedTagPosList.Contains(tagPos))//修改部分
                            {
                                changedTagPosList.Add(tagPos);
                            }
                        }
                    }
                    else
                    {
                        tagPos.Edit(position);//修改实时位置数据
                        if (!changedTagPosList.Contains(tagPos))//修改部分
                        {
                            changedTagPosList.Add(tagPos);
                        }
                    }
                    


                }
                else
                {
                    LocationCardPosition newTagPos = new LocationCardPosition(position);
                    newTagPosList.Add(newTagPos);
                }
            }

            //List<LocationCardPosition> noChangedTagPosList = new List<LocationCardPosition>();//没有移动的位置信息
            //foreach (var tag1 in tagPosList)
            //{
            //    if (!changedTagPosList.Contains(tag1))
            //    {
            //        noChangedTagPosList.Add(tag1);
            //    }
            //}

            ////设置实时位置的移动状态
            //foreach (var tag1 in noChangedTagPosList)
            //{
            //    TimeSpan time = DateTime.Now - tag1.DateTime;
            //    if (time.TotalSeconds > 30)
            //    {
            //        if (tag1.Flag == "0:0:0:0:1")
            //        {
            //            tag1.MoveState = 1;
            //        }
            //        else
            //        {
            //            tag1.MoveState = 2;
            //        }
            //    }
            //}

            LocationCards.EditRange(editCardList);//修改定位卡信息

            LocationCardPositions.EditRange(changedTagPosList);//修改位置信息

            LocationCardPositions.AddRange(newTagPosList);//增加位置信息
        }

        public bool EditTagPositionEx(Position position)
        {
            var tagPos = LocationCardPositions.FindByCode(position.Code);//判断是否存在实时数据
            if (tagPos == null)
            {
                var tagPosition = new LocationCardPosition(position);
                if (LocationCardPositions.Add(tagPosition))//添加新的实时数据
                {
                    return true;
                }
                else
                {
                    ErrorMessage = Positions.ErrorMessage;
                    return false;
                }
            }
            else
            {
                tagPos.Edit(position);
                if (LocationCardPositions.Edit(tagPos, false))//修改实时数据
                {
                    return true;
                }
                else
                {
                    ErrorMessage = Positions.ErrorMessage;
                    return false;
                }
            }
        }

        public bool AddPosition(Position position)
        {
            bool r = false;
            if (position == null)
            {
                r = false;
            }
            if (Positions.Add(position))//添加历史数据
            {
                r = EditTagPosition(position);//修改实时数据
                //return true;
            }
            else
            {
                ErrorMessage = Positions.ErrorMessage;
                r = false;
            }
            //return EditTagPosition(position);//修改实时数据
            return r;
        }

        public bool EditTagPosition(Position position)
        {
            LocationCardPosition tagPos = LocationCardPositions.FindByCode(position.Code);//判断是否存在实时数据
            if (tagPos == null)
            {
                var tagPosition = new LocationCardPosition(position);
                if (LocationCardPositions.Add(tagPosition))//添加新的实时数据
                {
                    return true;
                }
                else
                {
                    ErrorMessage = Positions.ErrorMessage;
                    return false;
                }
            }
            else
            {
                tagPos.Edit(position);
                if (LocationCardPositions.Edit(tagPos))//修改实时数据
                {
                    return true;
                }
                else
                {
                    ErrorMessage = Positions.ErrorMessage;
                    return false;
                }
            }
        }

        public string ErrorMessage { get; set; }
    }
}
