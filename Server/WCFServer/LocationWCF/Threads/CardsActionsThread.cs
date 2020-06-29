using Base.Common.Threads;
using BLL;
using CommunicationClass.SihuiThermalPowerPlant.Models;
using DbModel.Location.AreaAndDev;
using DbModel.LocationHistory.AreaAndDev;
using Location.BLL.Tool;
using Location.TModel.Tools;
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
    /// 门禁卡操作历史保存
    /// </summary>
    public class CardsActionsThread : IntervalTimerThread
    {
        private string Ip = "";
        private string Port = "";
        public CardsActionsThread(string ip, string port) :
            base(TimeSpan.FromHours(12)      //TimeSpan.FromDays(1)//一天检查一次
               , TimeSpan.FromSeconds(5))
        {
            Ip = ip;
            Port = port;
        }

        public override bool TickFunction()
        {
            try
            {
                DateTime now = DateTime.Now;
                JsonSerializerSettings setting = new JsonSerializerSettings();
                setting.NullValueHandling = NullValueHandling.Ignore;
                Bll db = Bll.NewBllNoRelation();
                List<EntranceGuardCard> cardPersons = db.EntranceGuardCards.ToList();
                string strsql = string.Format(@"select Abutment_Id from deventranceguardcardactions where  OperateTime>'{0}' ",now.AddDays(-3));
                List<int> cardHisListInt = db.EntranceGuardCardHistorys.GetListIntBySql(strsql);
                foreach (EntranceGuardCard card in cardPersons)
                {
                    string cardId = card.Abutment_Id.ToString();
                    
                    DateTime beginTime = now.AddDays(-20);
                    string begin_date = beginTime.Year.ToString() + beginTime.Month.ToString() + beginTime.Day.ToString();
                    string end_date = now.Year.ToString() + now.Month.ToString() + now.Day.ToString();
                    string url = "http://" + Ip + ":" + Port + "/api/cards/" + cardId + "/actions";
                    string result = WebApiHelper.GetString("http://120.25.195.214:18000/api/cards/" + cardId + "/actions?begin_date="+begin_date+"&end_date="+end_date);
                    Message<cards_actions>  message= JsonConvert.DeserializeObject<Message<cards_actions>>(result, setting);
                    List<cards_actions> cards_actionsList = message.data;
                    if (cards_actionsList != null && cards_actionsList.Count > 0)
                    {
                        //保存到门禁历史记录里
                        List<DevEntranceGuardCardAction> AddcardHisList = new List<DevEntranceGuardCardAction>();
                        List<DevEntranceGuardCardAction> EditcardHisList = new List<DevEntranceGuardCardAction>();
                        foreach (cards_actions cardAction in cards_actionsList)
                        {
                            DevEntranceGuardCardAction cardHis = new DevEntranceGuardCardAction();
                            cardHis.Abutment_Id = cardAction.id;
                            cardHis.OperateTimeStamp = cardAction.t;
                            cardHis.OperateTime = TimeConvert.ToDateTime((long)cardAction.t*1000);
                            cardHis.code = cardAction.code;
                            cardHis.description = cardAction.description;
                            cardHis.device_id = cardAction.device_id;
                            cardHis.card_code = cardAction.card_code;
                            cardHis.EntranceGuardCardId = card.Id;
                            cardHis.PersonnelAbutment_Id = card.PersonnelAbutment_Id;
                            if (cardHisListInt.Contains(cardAction.id))
                            {
                              //  EditcardHisList.Add(cardHis);
                              // bool result1=  db.DevEntranceGuardCardActions.Edit(cardHis);
                            }
                            else
                            {
                                AddcardHisList.Add(cardHis);
                               // bool result2 = db.DevEntranceGuardCardActions.Add(cardHis);
                            }
                        }

                        bool addResult = db.DevEntranceGuardCardActions.AddRange(AddcardHisList) ;
                      //  bool editResult = db.DevEntranceGuardCardActions.EditRange(EditcardHisList);
                         Log.Info(string.Format("CardsActionsThread:保存门禁历史记录结果，门禁卡号：{0}，添加：{1}条，结果：{2}",cardId,AddcardHisList.Count,addResult));
                    }
                }
               

            }
            catch (Exception ex)
            {
                Log.Error("CardsActionsThread:"+ex.ToString());
            }
            return true;
        }

        protected override void DoBeforeWhile()
        {
            throw new NotImplementedException();
        }
    }
}
