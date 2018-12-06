using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using DbEntity= DbModel.Location.Alarm.LocationAlarm;
//using TEntity= Location.TModel.Location.Alarm.LocationAlarm;
using TEntity = DbModel.Location.Alarm.LocationAlarm;

namespace LocationServices.Locations.Services
{
    public interface ILocationAlarmService:IEntityService<TEntity>
    {
        void Clear();
    }

    public class LocationAlarmService : EntityService<TEntity>,ILocationAlarmService
    {
        protected override void SetDbSet()
        {
            dbSet = db.LocationAlarms;
        }

        public void Clear()
        {
            dbSet.Clear();
        }
    }
}
