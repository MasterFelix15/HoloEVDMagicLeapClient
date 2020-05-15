using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

public class UDPReceiver : MonoBehaviour
{
    Thread receiveThread;
    UdpClient client;
    public int port; // define > init
    public GameObject brain;
    public GameObject tool;
    public GameObject hud;
    private Dictionary<string, string> updates;
    private Dictionary<string, GameObject> objects;
    private string toolStr = "";
    private string brainStr = "";

    private void init()
    {
        updates = new Dictionary<string, string>();
        objects = new Dictionary<string, GameObject>();
        objects.Add("brain", brain);
        objects.Add("tool", tool);
        port = 8051;
        print("Sending to 127.0.0.1 : " + port);
        print("Test-Sending to this Port: nc -u 127.0.0.1  " + port + "");

        receiveThread = new Thread(
            new ThreadStart(ReceiveData));
        receiveThread.IsBackground = true;
        receiveThread.Start();
    }

    private void ReceiveData()
    {
        client = new UdpClient(port);
        while (true)
        {
            try
            {
                // Bytes empfangen.
                IPEndPoint anyIP = new IPEndPoint(IPAddress.Any, 0);
                byte[] data = client.Receive(ref anyIP);
                string text = Encoding.UTF8.GetString(data);
                // print(">> " + text);
                // decoding
                if (text.Length == 0)
                {
                    continue;
                }
                text = text.Substring(0, text.Length - 1);
                toolStr = text.Split('#')[0];
                brainStr = text.Split('#')[1];
            }
            catch (Exception err)
            {
                print(err.ToString());
            }
        }
    }

    void Start()
    {
        init();
    }

    // Update is called once per frame
    void Update()
    {
        if (toolStr != "")
        {
            var pos = toolStr.Split(':')[0].Split(',');
            Vector3 toolPos = new Vector3(float.Parse(pos[0]), float.Parse(pos[1]), float.Parse(pos[2]));
            var rot = toolStr.Split(':')[1].Split(',');
            Quaternion toolRot = new Quaternion(float.Parse(rot[0]), float.Parse(rot[1]), float.Parse(rot[2]), float.Parse(rot[3]));
            tool.transform.localPosition = toolPos;
            tool.transform.localRotation = toolRot;

            pos = brainStr.Split(':')[0].Split(',');
            Vector3 brainPos = new Vector3(float.Parse(pos[0]), float.Parse(pos[1]), float.Parse(pos[2]));
            rot = brainStr.Split(':')[1].Split(',');
            Quaternion brainRot = new Quaternion(float.Parse(rot[0]), float.Parse(rot[1]), float.Parse(rot[2]), float.Parse(rot[3]));
            brain.transform.localPosition = brainPos;
            brain.transform.localRotation = brainRot;
        }
    }

    void OnApplicationQuit()
    {
        receiveThread.Abort();
    }
}
