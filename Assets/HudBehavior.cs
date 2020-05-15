using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
using System.Net;
using System;

public class HudBehavior : MonoBehaviour
{
    public int Interval = 5;
    public GameObject toolrep;
    public GameObject mcamera;
    public GameObject tool;
    private float SecondsElapsed = 0.0f;
    private string IPstr = "";
    private bool DisplayTool = false;
    // Start is called before the first frame update
    void Start()
    {
        String strHostName = System.Net.Dns.GetHostName();
        IPHostEntry ipEntry = System.Net.Dns.GetHostEntry(strHostName);
        var addr = ipEntry.AddressList;
        IPstr = "IP: " + addr[addr.Length - 1].ToString();
    }

    // Update is called once per frame
    void Update()
    {
        SecondsElapsed += Time.deltaTime;
        if (SecondsElapsed <= Interval * 1)
        {
            GetComponent<TextMeshPro>().SetText(IPstr + "\nProceed to open space" + (Interval * 1 - SecondsElapsed).ToString());
        }
        else if (SecondsElapsed <= Interval * 2)
        {
            GetComponent<TextMeshPro>().SetText("Align tool with displayed object" + (Interval * 2 - SecondsElapsed).ToString());
            if (!DisplayTool) {
                toolrep.transform.position = mcamera.transform.position + mcamera.transform.forward * 0.4f + mcamera.transform.up * -0.2f;
                toolrep.transform.rotation = toolrep.transform.rotation * mcamera.transform.rotation;
                DisplayTool = true;
            }
        }
        else 
        {
            toolrep.transform.SetParent(tool.transform);
            Destroy(gameObject);
        }
    }
}
