using BestHTTP.SignalR;
using BestHTTP.SignalR.Hubs;
using BestHTTP.SignalR.Messages;
//using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EchoHubClient : MonoBehaviour {

    public Text txt;

    public InputField input;

    public Button btn;

    private Connection _connection;

    public string Url = "http://localhost:51337/realtime";

    private Hub _proxy;

	// Use this for initialization
	void Start () {
        if (txt)
        {
            txt.text = "";
        }
        btn.onClick.AddListener(Send);
        Uri uri = new Uri(Url);
        _proxy = new Hub("echoHub");
        _connection = new Connection(uri, _proxy);
        _proxy.On("Message", Message);
        _connection.Open();
	}

    public void Send()
    {
        string msg = input.text;
        _proxy.Call("Broadcast", msg);
    }

    private void Message(Hub hub,MethodCallMessage methodCall)
    {
        string arg0 = methodCall.Arguments[0].ToString();
        //ItemValueModel item
        Debug.Log("Message:" + arg0);
        if (txt)
        {
            txt.text += arg0+"\n";
        }
        //JsonConvert.DeserializeObject<>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnDestroy()
    {
        if (_connection != null)
        {
            _connection.Close();
        }
    }
}
