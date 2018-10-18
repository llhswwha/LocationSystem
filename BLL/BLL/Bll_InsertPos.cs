using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DbModel.Location.AreaAndDev;
using DbModel.Location.Data;
using DbModel.LocationHistory.Data;

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
                var tagPos = tagPosList.Find(i => i.Code == tag.Code);//判断是否存在实时数据
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

        public bool AddPositions(List<Position> positions)
        {
            bool r = true;
            try
            {
                if (positions == null || positions.Count == 0) return false;              

                //string sql = GetInsertSql(positions);
                //if (!string.IsNullOrEmpty(sql))
                //    DbHistory.Database.ExecuteSqlCommand(sql);

                //List<TagPosition> tagPosList = EditTagPositionList(positions);
                //string sql2 = GetUpdateSql(tagPosList);
                //if (!string.IsNullOrEmpty(sql2))
                //    Db.Database.ExecuteSqlCommand(sql2);

                //todo:获取位置信息参与计算的基站
                foreach (Position position in positions)
                {
                    if (position.Archors != null)
                    {
                        List<Archor> archorList = Archors.FindByCodes(position.Archors);
                        foreach (Archor archor in archorList)
                        {
                            //基站位置和Position位置相等（0.1是为了应对Double类型比较，可能出现的误差）
                            if (Math.Abs(archor.Y - position.Y) < 0.1f)
                            {
                                //找到对应ID,不往后找
                                position.AreaId = archor.DevInfo.ParentId;
                                break;
                            }
                            //if (!position.TopoNodes.Contains(archor.Dev.ParentId))
                            //    position.TopoNodes.Add(archor.Dev.ParentId);
                        }
                    }
                    else
                    {
                        position.AreaId = null;
                    }
                    //Todo:找不到合适的ID,需要处理一下


                    //foreach (string code in position.Archors)
                    //{
                    //    Archor archor=Archors.FindByCode(code);
                    //    if(!position.TopoNodes.Contains(archor.Dev.ParentId))
                    //        position.TopoNodes.Add(archor.Dev.ParentId);
                    //}
                }
                //1.批量插入历史数据数据
                DbHistory.BulkInsert(positions);//插件Z.EntityFramework.Extensions功能
                ////2.获取并修改列表
                //List<LocationCardPosition> tagPosList = EditTagPositionList(positions);
                ////3.更新列表
                //LocationCardPositions.Db.BulkUpdate(tagPosList);//插件Z.EntityFramework.Extensions功能

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
                        p.Code, p.X, p.Y, p.Z, p.DateTimeStamp, p.Power, p.Number, p.Flag);
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
                var tagPos = tagPosList.Find(item => item.Code == position.Code);
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
                var tagPos = tagPosList.Find(item => item.Code == position.Code);
                if (tagPos != null)
                {
                    tagPos.Edit(position);
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

            LocationCardPositions.Db.BulkUpdate(changedTagPosList);//插件Z.EntityFramework.Extensions功能
            LocationCardPositions.Db.BulkInsert(newTagPosList);//插件Z.EntityFramework.Extensions功能
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
