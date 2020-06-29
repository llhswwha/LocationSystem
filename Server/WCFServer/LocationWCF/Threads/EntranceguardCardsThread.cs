using Base.Common.Threads;
using BLL;
using CommunicationClass.SihuiThermalPowerPlant.Models;
using DbModel.Location.AreaAndDev;
using Location.BLL.Tool;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiLib;

namespace LocationServer.Threads
{
    /// <summary>
    /// 获取门禁卡信息
    /// </summary>
    public class EntranceguardCardsThread : IntervalTimerThread
    {
        private string Ip = "";
        private string Port = "";
        public EntranceguardCardsThread(string ip, string port) :
            base(TimeSpan.FromHours(1)      //TimeSpan.FromDays(1)//一天检查一次
               , TimeSpan.FromSeconds(5))
        {
            Ip = ip;
            Port = port;
        }

        public override bool TickFunction()
        {
            try
            {
                JsonSerializerSettings setting = new JsonSerializerSettings();
                setting.NullValueHandling = NullValueHandling.Ignore;
                Bll db = Bll.NewBllNoRelation();
                string strsql = string.Format(@"select Abutment_Id from entranceguardcards");
                List<int> cardIdList = db.EntranceGuardCards.GetListIntBySql(strsql) ;
                string url = "http://" + Ip + ":" + Port + "/api/cards";
                //string result = WebApiHelper.GetString("http://120.25.195.214:18000/api/cards");
                string result = WebApiHelper.GetString(url);
                Message<EntranceGuardCardsApiSH> message = JsonConvert.DeserializeObject<Message<EntranceGuardCardsApiSH>>(result,setting);
                List<EntranceGuardCardsApiSH> setList = message.data;
                List<EntranceGuardCard> addList = new List<EntranceGuardCard>();
                List<EntranceGuardCard> editList = new List<EntranceGuardCard>();
                foreach (EntranceGuardCardsApiSH cardApi in setList)
                {
                    EntranceGuardCard card = new EntranceGuardCard();
                    card.Abutment_Id = cardApi.cardId;
                    card.Code = cardApi.cardCode;
                    int empid = cardApi.emp_id;
                    if (empid != null)
                    {
                        card.PersonnelAbutment_Id = cardApi.emp_id.ToString();
                    }
                    card.State = cardApi.state;
                    if (cardIdList.Contains(cardApi.cardId))
                    {
                        editList.Add(card);
                    }
                    else
                    {
                        addList.Add(card);
                    }
                }
                bool addResult = db.EntranceGuardCards.AddRange(addList);
                bool editResult = db.EntranceGuardCards.EditRange(editList);
                Log.Info("EntranceguardCardsThread: addResult:"+addList.Count+","+addResult+",editResult:"+editList.Count+","+editResult);
            }
            catch (Exception ex)
            {
                Log.Error("EntranceguardCardsThread:" + ex.ToString());
            }
            return true;
        }

        protected override void DoBeforeWhile()
        {
            throw new NotImplementedException();
        }
    }
}
