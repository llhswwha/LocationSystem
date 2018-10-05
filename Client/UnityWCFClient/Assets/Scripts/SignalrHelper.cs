using BestHTTP.SignalR;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SignalrHelper : MonoBehaviour {

    public Text txt;

    private Connection _connection;

    public string Url = "http://localhost:40161/realtime/echo";

	// Use this for initialization
	void Start () {
        Uri uri = new Uri(Url);
        _connection = new Connection(uri);
        _connection.OnStateChanged += _connection_OnStateChanged;
        _connection.OnNonHubMessage += _connection_OnNonHubMessage;
        _connection.Open();
    }

    private void _connection_OnNonHubMessage(Connection connection, object data)
    {
        string msg = BestHTTP.JSON.Json.Encode(data);
        Debug.Log(msg);
    }

    private void _connection_OnStateChanged(Connection connection, ConnectionStates oldState, ConnectionStates newState)
    {
        string msg = string.Format("[State Change] {0} => {1}", oldState.ToString(), newState.ToString());
        Debug.Log(msg);
    }

    // Update is called once per frame
    void Update () {
		
	}
}
