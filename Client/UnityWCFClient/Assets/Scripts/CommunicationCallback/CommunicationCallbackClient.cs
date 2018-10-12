using BestHTTP.SignalR;
using BestHTTP.SignalR.Hubs;
using BestHTTP.SignalR.JsonEncoders;
using BestHTTP.SignalR.Messages;
using LitJson;
using Location.WCFServiceReferences.LocationServices;
//using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class CommunicationCallbackClient : MonoBehaviour {

    /// <summary>
    /// IP地址
    /// </summary>
    public string Ip;
    /// <summary>
    /// 端口
    /// </summary>
    public int Port;
    /// <summary>
    /// SignalR服务地址
    /// </summary>
    Uri URI;

    /// <summary>
    /// The SignalR connection instance
    /// </summary>
    Connection signalRConnection;

    /// <summary>
    /// 告警Hub
    /// </summary>
    AlarmHub alarmHub;

    /// <summary>
    /// EchoHub
    /// </summary>
    EchoHubT echoHub;
    // Use this for initialization
    void Start () {
        RegisterImporter();
        if(alarmHub==null)alarmHub = new AlarmHub();
        if(echoHub==null) echoHub = new EchoHubT();
        URI = new Uri(string.Format("http://{0}:{1}/realtime",Ip,Port));
        // Create the SignalR connection, passing all the three hubs to it
        signalRConnection = new Connection(URI, alarmHub, echoHub);

        signalRConnection.JsonEncoder = new LitJsonEncoder();        

        signalRConnection.OnStateChanged += signalRConnection_OnStateChanged;
        signalRConnection.OnError += signalRConnection_OnError;
        signalRConnection.OnNonHubMessage += signaleRConnection_OnNoHubMsg;
        signalRConnection.OnConnected += (connection) =>
        {
            // Call the demo functions
            echoHub.Send("Start Connect...");
        };

        // Start opening the signalR connection
        signalRConnection.Open();
    }
	
	// Update is called once per frame
	void Update () {

	}
    /// <summary>
    /// Display state changes
    /// </summary>
    void signalRConnection_OnStateChanged(Connection connection, ConnectionStates oldState, ConnectionStates newState)
    {
        Debug.Log(string.Format("[State Change] {0} => {1}", oldState, newState));
    }

    /// <summary>
    /// Display errors.
    /// </summary>
    void signalRConnection_OnError(Connection connection, string error)
    {
        Debug.Log("[Error] " + error);
    }
    void signaleRConnection_OnNoHubMsg(Connection connection, object data)
    {
        string arg0 = JsonMapper.ToJson(data);
        Debug.Log("NoHub Msg:"+arg0);
    }
    void OnDestroy()
    {
        // Close the connection when we are closing this sample
        signalRConnection.Close();
    }
    /// <summary>
    /// 注册转换器
    /// </summary>
    private void RegisterImporter()
    {
        JsonMapper.RegisterImporter<int, long>((int value) =>
        {
            return (long)value;
        });
        JsonMapper.RegisterImporter<double, float>((double value) =>
        {
            return (float)value;
        });
    }
}
public class AlarmHub:Hub
{
    /// <summary>
    /// 设备告警
    /// </summary>
    public event Action<List<DeviceAlarm>> OnDeviceAlarmRecieved;
    /// <summary>
    /// 定位告警
    /// </summary>
    public event Action<List<LocationAlarm>> OnLocationAlarmRecieved;
    public AlarmHub()
        : base("alarmHub")
    {
        // Setup server-called functions     
        base.On("GetDeviceAlarms", GetDeviceAlarms);
        base.On("GetLocationAlarms",GetLocationAlarms);     
    }
    /// <summary>
    /// 设备告警回调
    /// </summary>
    /// <param name="hub"></param>
    /// <param name="methodCall"></param>
    private void GetDeviceAlarms(Hub hub, MethodCallMessage methodCall)
    {      
        string arg0 = JsonMapper.ToJson(methodCall.Arguments[0]);
        List<DeviceAlarm> alarm = JsonMapper.ToObject<List<DeviceAlarm>>(arg0);
        //Debug.Log("OnAlarmRecieved:"+methodCall.Arguments.Length);
        if (OnDeviceAlarmRecieved != null) OnDeviceAlarmRecieved(alarm);
    }
    /// <summary>
    /// 定位告警回调
    /// </summary>
    /// <param name="hub"></param>
    /// <param name="methodCall"></param>
    private void GetLocationAlarms(Hub hub,MethodCallMessage methodCall)
    {
        string arg0 = JsonMapper.ToJson(methodCall.Arguments[0]);
        List<LocationAlarm> alarm = JsonMapper.ToObject<List<LocationAlarm>>(arg0);
        //Debug.Log("OnAlarmRecieved:"+methodCall.Arguments.Length);
        if (OnLocationAlarmRecieved != null) OnLocationAlarmRecieved(alarm);
    }
}
public class EchoHubT:Hub
{
    public EchoHubT()
        : base("echoHub")
    {
        // Setup server-called functions     
        base.On("Message", Message);
    }
    private void Message(Hub hub, MethodCallMessage methodCall)
    {
        string arg0 = methodCall.Arguments[0].ToString();
        //ItemValueModel item
        Debug.Log("EchoMessage:" + arg0);
    }
    /// <summary>
    /// 发送消息
    /// </summary>
    /// <param name="msg"></param>
    public void Send(string msg)
    {      
        base.Call("Broadcast", msg);
    }
}
