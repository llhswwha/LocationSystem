using Base.Common.Tools;
using Location.IModel;
using Location.TModel.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbModel.LocationHistory.Data
{
    public class PosInfo:IComparable<PosInfo>, IPosInfo,IId
    {
        public int Id { get; set; }
        public long DateTimeStamp { get; set; }
        public int? PersonnelID { get; set; }
        public string Code { get; set; }
        public int? AreaId { get; set; }

        public string PersonnelName { get; set; }
        public string AreaPath { get; set; }

        public DateTime DateTime { get; set; }

        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }

        //public string DateTime_Date;
        //public string DateTime_Hour;

        //public int Year;
        //public int Month;
        //public int Day;
        //public int Hour;

        public void ParseTimeStamp()
        {
            DateTime = DateTimeStamp.ToDateTime();
            ////DateTime_Date = DateTime.ToString("yyyy-MM-dd");
            ////DateTime_Hour = DateTime.ToString("yyyy-MM-dd HH");

            //Year = DateTime.Year;
            //Month = DateTime.Year;
            //Day = DateTime.Year;
            //Hour = DateTime.Year;
        }

        public string GetDateTime_Date()
        {
            return DateTime.ToString("yyyy-MM-dd");
            //return "";
        }

        public string GetDateTime_Hour()
        {
            return DateTime.ToString("yyyy-MM-dd HH");
            //return "";
        }

        public string GetPersonnelName()
        {
            return PersonnelName;
        }

        public string GetAreaPath()
        {
            return AreaPath;
        }

        public override string ToString()
        {
            return string.Format("{0},{1},{2},{3},{4}", Id, Code, PersonnelID, AreaId, AreaPath);
        }

        public int CompareTo(PosInfo other)
        {
            return this.DateTimeStamp.CompareTo(other.DateTimeStamp);
        }
    }
}
