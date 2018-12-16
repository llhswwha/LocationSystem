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

            //TagPositions.Db.BulkInsert(newPosList);//插件Z.EntityFramework.Extensions功能
            //TagPositions.Db.BulkSaveChanges();

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
                tagRelation = new TagRelationBuffer();
            }
            tagRelation.SetPositionInfo(positions2);
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
                if (positions == null || positions.Count == 0) return false;

                //AddPositionsBySql(positions);

                //1.批量插入历史数据数据
                DbHistory.BulkInsert(positions);//插件Z.EntityFramework.Extensions功能

                //修改实时数据
                EditTagPositionListOP(positions);
            }
            catch (Exception ex)
            {
                r = false;
            }
            return r;
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

        private void EditTagPositionListOP(List<Position> positions)
        {
            //1.获取列表
            List<LocationCardPosition> tagPosList = LocationCardPositions.ToList();
            List<LocationCardPosition> changedTagPosList = new List<LocationCardPosition>();
            List<LocationCardPosition> newTagPosList = new List<LocationCardPosition>();
            //2.修改数据
            for (int i = 0; i < positions.Count; i++)
            {
                Position position = positions[i];
                if (position == null) continue;//位置信息可能有null
                LocationCard lc = LocationCards.Where(p=>p.Code == position.Code).FirstOrDefault();
                if (lc == null) continue;
                if (lc.Flag != position.Flag || lc.Power != position.Power)
                {
                    lc.Flag = position.Flag;
                    lc.Power = position.Power;
                    if (lc.Power >= 400)
                    {
                        lc.PowerState = 0;
                    }
                    else
                    {
                        lc.PowerState = 1;
                    }

                    LocationCards.Edit(lc);
                }

                var tagPos = tagPosList.Find(item => item.Id == position.Code);
                if (tagPos != null)
                {
                    tagPos.Edit(position);//修改实时位置数据
                    if (!changedTagPosList.Contains(tagPos))
                    {
                        changedTagPosList.Add(tagPos);
                    }
                }
                else
                {
                    LocationCardPosition newTagPos = new LocationCardPosition(position);
                    newTagPosList.Add(newTagPos);
                }
            }

            List<LocationCardPosition> noChangedTagPosList = new List<LocationCardPosition>();//没有移动的位置信息
            foreach (var tag1 in tagPosList)
            {
                if (!changedTagPosList.Contains(tag1))
                {
                    noChangedTagPosList.Add(tag1);
                }
            }

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

            try
            {

                LocationCardPositions.Db.BulkUpdate(changedTagPosList);//插件Z.EntityFramework.Extensions功能
            }
            catch (Exception ex)
            {
                Log.Error(string.Format("EditTagPositionListOP1,Type:{0},Count:{1},Error:{2}", typeof(LocationCardPosition), changedTagPosList.Count(), ex.Message));
            }
            
            try
            {

                LocationCardPositions.Db.BulkInsert(newTagPosList);//插件Z.EntityFramework.Extensions功能
            }
            catch (Exception ex)
            {
                Log.Error(string.Format("EditTagPositionListOP2,Type:{0},Count:{1},Error:{2}", typeof(LocationCardPosition), changedTagPosList.Count(), ex.Message));
            }

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
