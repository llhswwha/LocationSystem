using DAL;
using DbModel.Location.AreaAndDev;
using Location.BLL.Tool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Blls.Location
{
    public class KKSCodeBll : BaseBll<KKSCode, LocationDb>
    {
        public KKSCodeBll():base()
        {

        }
        public KKSCodeBll(LocationDb db) : base(db)
        {

        }

        protected override void InitDbSet()
        {
            DbSet = Db.KKSCodes;
        }

        private List<KKSCode> _kksCodes;

        private Dictionary<string, KKSCode> _kksDict = new Dictionary<string, KKSCode>();

        List<KKSCode> errorKKSList = new List<KKSCode>();

        public List<KKSCode> ToListEx()
        {
            if (_kksCodes == null)
            {
                _kksCodes = this.ToList();
                foreach (KKSCode kks in _kksCodes)
                {
                    //kks.Children = _kksCodes.Where(p => p.ParentCode == kks.Code).ToList();//获取子节点

                    if (!_kksDict.ContainsKey(kks.Code))
                    {
                        _kksDict.Add(kks.Code, kks);
                    }
                    else
                    {
                        var kks1 = _kksDict[kks.Code];
                        Log.Error("KKS重复:" + kks.Code);
                        errorKKSList.Add(kks);
                    }
                }

                foreach (KKSCode kks in _kksCodes)
                {
                    if (string.IsNullOrEmpty(kks.ParentCode))
                    {
                        continue;
                    }

                    if (_kksDict.ContainsKey(kks.ParentCode))
                    {
                        var parent = _kksDict[kks.ParentCode];
                        parent.AddChild(kks);
                    }
                }
            }

            return _kksCodes;

        }

        public Dictionary<string, KKSCode> GetDictionary()
        {
            List<KKSCode> list = this.ToList();
            Dictionary<string, KKSCode> dic = KKSCode.ToDict(list);
            return dic;
        }

        public Dictionary<string, KKSCode> GetDictionaryEx()
        {
            List<KKSCode> list = this.ToListEx();
            Dictionary<string, KKSCode> dic = KKSCode.ToDict(list);
            return dic;
        }


    }
}
