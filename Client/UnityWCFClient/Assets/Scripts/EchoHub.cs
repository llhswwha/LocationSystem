using BestHTTP.SignalR.Hubs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EchoHub : Hub {

	public EchoHub():base("echoHub")
    {

    }
}
