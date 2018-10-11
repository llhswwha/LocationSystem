using BestHTTP.SignalR;
using BestHTTP.SignalR.Hubs;
using BestHTTP.SignalR.JsonEncoders;
using BestHTTP.SignalR.Messages;
using LitJson;
using Location.WCFServiceReferences.LocationServices;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class CommunicationCallbackClient : MonoBehaviour {

    readonly Uri URI = new Uri("http://localhost:8735/realtime");

    /// <summary>
    /// The SignalR connection instance
    /// </summary>
    Connection signalRConnection;

    /// <summary>
    /// DemoHub client side implementation
    /// </summary>
    AlarmHub alarmHub;

    /// <summary>
    /// TypedDemoHub client side implementation
    /// </summary>
    EchoHubT echoHub;
    // Use this for initialization
    void Start () {
        RegisterImporter();
        alarmHub = new AlarmHub();
        echoHub = new EchoHubT();

        // Create the SignalR connection, passing all the three hubs to it
        signalRConnection = new Connection(URI, alarmHub, echoHub);

        signalRConnection.JsonEncoder = new LitJsonEncoder();        

        signalRConnection.OnStateChanged += signalRConnection_OnStateChanged;
        signalRConnection.OnError += signalRConnection_OnError;
        signalRConnection.OnNonHubMessage += signaleRConnection_OnNoHubMsg;
        signalRConnection.OnConnected += (connection) =>
        {
            // Call the demo functions
            echoHub.Send();
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
    public event Action<DeviceAlarm[]> DeviceAlarmEvent;

    public AlarmHub()
        : base("alarmHub")
    {
        // Setup server-called functions     
        base.On("GetDeviceAlarms", GetDeviceAlarms);        
    }
    private void GetDeviceAlarms(Hub hub, MethodCallMessage methodCall)
    {      
        string arg0 = JsonMapper.ToJson(methodCall.Arguments[0]);
        DeviceAlarm[] alarm = JsonMapper.ToObject<DeviceAlarm[]>(arg0);
        Debug.Log(alarm[0].Abutment_Id);
        Debug.Log(alarm[0].DevId);
        //string arg0 = methodCall.Arguments[0].ToString();
        //Debug.Log("AlarmMessage:" + arg0);
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
    public void Send()
    {
        string msg = DateTime.Now.ToShortTimeString();
        base.Call("Broadcast", msg);
    }
}
