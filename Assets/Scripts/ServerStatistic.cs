using UnityEngine;
using System.Collections;

public class ServerStatistic : MonoBehaviour {

    public static string Session;
    public static GameObject request;
    public GameObject requestTemplate;

    void Awake()
    {
        request = requestTemplate;
        DontDestroyOnLoad(gameObject);
        Session = System.DateTime.UtcNow.ToString();
    }

    void Start()
    {
        string command = "{";
        command += "device_id:"+SystemInfo.deviceUniqueIdentifier;
        command += ",action:OPEN_APP";
        command += ",platform:" + Application.platform.ToString();
        command += ",language:" + Application.systemLanguage.ToString();
        command += ",operating_system:" + SystemInfo.operatingSystem;
        command += ",device_model:" + SystemInfo.deviceModel;
        command += ",country:" + "ID";
        command += ",IP:" + (Network.HavePublicAddress() ? Network.natFacilitatorIP:"-") ;
        command += ",screen_resolution:" + Screen.width + "x" + Screen.height;
        command += "}";
        ServerStatistic.DoRequest(command);
    }


    public static void DoRequest(string command)
    {
        if (request)
        {
            GameObject go = (GameObject)Instantiate(request);
            RequestStatistic req = go.GetComponent<RequestStatistic>();
            req.Command = command;
            req.Session = Session;
        }
    }
}
