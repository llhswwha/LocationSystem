using System.Collections;
using System.Collections.Generic;
using System.ServiceModel;
using System.Text;
using Location.WCFServiceReferences.LocationServices;
using UnityEngine;

public class WCFClientTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
        //Hello();
        //GetTags();
        //GetDepList();
        //GetDepTree();
	    //GetTopoList();
	    GetTopoTree();
	    //GetRealPositons();
	    //GetHistoryPositons();
	    //GetConfigs();
	    //GetPosts();
	    //GetLocationAlarms();
	}

    void GetLocationAlarms()
    {
        Debug.Log("->GetLocationAlarms");
        LocationServiceClient client = GetLocationServiceClient();
        AlarmSearchArg arg=new AlarmSearchArg();
        LocationAlarm[] list = client.GetLocationAlarms(arg);
        foreach (LocationAlarm item in list)
        {
            Debug.Log("item:" + item.Content);
        }
    }

    void GetPosts()
    {
        Debug.Log("->GetPosts");
        LocationServiceClient client = GetLocationServiceClient();
        Post[] list=client.GetPostList();
        foreach (Post post in list)
        {
            Debug.Log("Post:" + post);
        }
    }

    void GetConfigs()
    {
        Debug.Log("->GetConfigs");
        LocationServiceClient client = GetLocationServiceClient();
        ConfigArg[] args=client.GetConfigArgList();
        foreach (ConfigArg arg in args)
        {
            Debug.Log(string.Format("Key:{0},Value:{1}", arg.Key, arg.Value));
        }
        TransferOfAxesConfig axesConfig=client.GetTransferOfAxesConfig();
        Debug.Log("axesConfig");

        Debug.Log(string.Format("Zero Key:{0},Value:{1}", axesConfig.Zero.Key, axesConfig.Zero.Value));
        Debug.Log(string.Format("Scale Key:{0},Value:{1}", axesConfig.Scale.Key, axesConfig.Scale.Value));
        Debug.Log(string.Format("Direction Key:{0},Value:{1}", axesConfig.Direction.Key, axesConfig.Direction.Value));

        string[] pars = axesConfig.Zero.Value.Split(',');
        Vector3 vector31=new Vector3(float.Parse(pars[0]), float.Parse(pars[1]), float.Parse(pars[1]));
        Debug.Log("vector31:"+ vector31);
    }

    //private 
	
	// Update is called once per frame
	void Update () {
		
	}

    public LocationServiceClient GetLocationServiceClient()
    {
        

        if (client == null)
        {
            string hostName = "localhost";
            string port = "8733";
            System.ServiceModel.Channels.Binding wsBinding = new BasicHttpBinding();
            string url =
                string.Format("http://{0}:{1}/LocationServices/Locations/LocationService",
                    hostName, port);
            EndpointAddress endpointAddress = new EndpointAddress(url);

            if (client != null)
            {
                if (client.State == CommunicationState.Opened)
                {
                    client.Close();
                }
            }

            client = new LocationServiceClient(wsBinding, endpointAddress);
        }
        return client;
    }

    private LocationServiceClient client;

    public void Hello()
    {
        Debug.Log("->Hello");
        LocationServiceClient client= GetLocationServiceClient();
        string hello=client.Hello("abc");
        Debug.Log("Hello:" + hello);
    }

    //public void GetUser()
    //{
    //    Debug.Log("->GetUser");
    //    LocationServiceClient client = GetLocationServiceClient();
    //    User user = client.GetUser();
    //    Debug.Log("Id:" + user.Id);
    //    Debug.Log("Name:" + user.Name);
    //}

    //public void GetUsers()
    //{
    //    Debug.Log("->GetUsers");
    //    LocationServiceClient client = GetLocationServiceClient();
    //    User[] list = client.GetUsers();
    //    for (int i = 0; i < list.Length; i++)
    //    {
    //        Debug.Log("i:" + i);
    //        Debug.Log("Id:" + list[i].Id);
    //        Debug.Log("Name:" + list[i].Name);
    //    }
    //}


    public void GetTags()
    {
        Debug.Log("->GetTags");
        LocationServiceClient client = GetLocationServiceClient();
        Tag[] list = client.GetTags();
        for (int i = 0; i < list.Length; i++)
        {
            Debug.Log("i:" + i);
            Debug.Log("Id:" + list[i].Id);
            Debug.Log("Name:" + list[i].Name);
        }
    }

    public void GetRealPositons()
    {
        Debug.Log("->GetRealPositons");
        LocationServiceClient client = GetLocationServiceClient();
        TagPosition[] list = client.GetRealPositons();
        for (int i = 0; i < list.Length; i++)
        {
            Debug.Log("i:" + i);
            Debug.Log("Tag:" + list[i].Tag);
            Debug.Log("X:" + list[i].X);
            Debug.Log("Y:" + list[i].Y);
            Debug.Log("Z:" + list[i].Z);
        }
    }

    public void GetHistoryPositons()
    {
        //Debug.Log("->GetHistoryPositons");
        //LocationServiceClient client = GetLocationServiceClient();
        //Position[] list = client.GetHistoryPositons();
        //for (int i = 0; i < list.Length; i++)
        //{
        //    Debug.Log("i:" + i);
        //    Debug.Log("Tag:" + list[i].Tag);
        //    Debug.Log("X:" + list[i].X);
        //    Debug.Log("Y:" + list[i].Y);
        //    Debug.Log("Z:" + list[i].Z);
        //}
    }

    public void GetMaps()
    {
        Debug.Log("->GetMaps");
        //LocationServiceClient client = GetLocationServiceClient();

        //Map map=client.GetMap(0);

        //Debug.Log("Id:" + map.Id);
        //Debug.Log("Name:" + map.Name);

        //Map[] list = client.GetMaps(0);
        //if(list!=null)
        //for (int i = 0; i < list.Length; i++)
        //{
        //    Debug.Log("i:" + i);
        //    Debug.Log("Id:" + list[i].Id);
        //    Debug.Log("Name:" + list[i].Name);
        //}
    }

    public void GetDepList()
    {
        Debug.Log("->GetDepList");
        client = GetLocationServiceClient();
        Department[] list = client.GetDepartmentList();
        StringBuilder stringBuilder=new StringBuilder();
        foreach (Department item in list)
        {
            stringBuilder.AppendLine(string.Format("Id:{0},Name:{1},Pid:{2}", item.Id, item.Name,item.ParentId));
            //stringBuilder.AppendLine(string.Format("Id:{0},Name:{1},Pid:{2}", item.Id, item.Name, item.ParentId));
        }
        Debug.Log(stringBuilder.ToString());
    }

    public void GetDepTree()
    {
        Debug.Log("->GetDepTree");
        client = GetLocationServiceClient();
        Department dep = client.GetDepartmentTree();
        if (dep == null)
        {
            Debug.LogError("Dep == null");
        }
        else
        {
            string txt = ShowDep(dep, 0);
            Debug.Log(txt);
        }
    }

    public void GetTopoList()
    {
        client = GetLocationServiceClient();
        PhysicalTopology[] list = client.GetPhysicalTopologyList();
        foreach (PhysicalTopology item in list)
        {
            Debug.Log(item.Name);
        }
    }

    public void GetTopoTree()
    {
        Debug.Log("->GetTopoTree");
        client = GetLocationServiceClient();
        PhysicalTopology topoRoot = client.GetPhysicalTopologyTree();
        string txt = ShowTopo(topoRoot, 0);
        Debug.Log("length:" + txt.Length);
        Debug.Log(txt);
    }

    private static string ShowTopo(PhysicalTopology dep, int layer)
    {
        //Debug.Log("ShowTopo:" + dep.Name);
        string whitespace = GetWhiteSpace(layer);
        string txt = whitespace + layer + ":" + dep.Name + "\n";
        if (dep.Children != null)
        {
            //txt+=whitespace + "length:" + dep.Children.Length+"\n";
            foreach (PhysicalTopology child in dep.Children)
            {
                txt += ShowTopo(child, layer + 1);
            }
        }
        else
        {
            //txt += whitespace + "children==null\n";
        }

        if (dep.LeafNodes != null)
            foreach (DevInfo dev in dep.LeafNodes)
            {
                txt += whitespace + "" + layer + ":" + dev.Name + "[Dev]\n";
            }

        return txt;
    }

    private static string GetWhiteSpace(int count)
    {
        string space = "";
        for (int i = 0; i < count; i++)
        {
            space += "  ";
        }
        return space;
    }

    private string ShowDep(Department dep,int layer)
    {
        string whitespace = GetWhiteSpace(layer);
        string txt = whitespace + layer+":" + dep.Name+"\n";
        if (dep.Children != null)
        {
            //txt+=whitespace + "length:" + dep.Children.Length+"\n";
            foreach (Department child in dep.Children)
            {
                txt += ShowDep(child, layer + 1);
            }
        }
        else
        {
            //txt += whitespace + "children==null\n";
        }

        if (dep.LeafNodes != null)
            foreach (var person in dep.LeafNodes)
            {
                txt += whitespace + "" + layer + ":" + person.Name + "[Person][" + person.Pst + "]\n";
            }

        return txt;
    }
}
