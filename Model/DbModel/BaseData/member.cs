using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DbModel.BaseData
{
    /*
     {
          "id": 43,
          "teamId": 3,
          "personId": 156,
          "role": 2,
          "state": 0,
          "parentId": 3,
          "seq": ""
        }
    */
    public class member
    {
        public int id { get; set; }
        public int teamId { get; set; }
        public int personId { get; set; }
        public int role { get; set; }
        public int state { get; set; }
        public int parentId { get; set; }

        public string seq { get; set; }

        [XmlIgnore]
        [NotMapped]
        public user user { get; set; }

        [XmlIgnore]
        [NotMapped]
        public org parent { get; set; }

        public override string ToString()
        {
            if(user!= null)
            {
                return user.name;
            }
            return id + "," + personId;
        }
    }
}
