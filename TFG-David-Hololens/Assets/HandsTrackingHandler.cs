﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;


public class HandsTrackingHandler : MonoBehaviour
{

    Thread receiverThread;
    public int port;
    public String Hand;

    public GameObject HandPointer;

    UdpClient Server;
    int ConfianceLevel;
    Vector3 handVector;

    Vector3 handPoint;
    Vector3 oldHandPoint;

    // Start is called before the first frame update
    void Start()
    {
        this.HandPointer.GetComponent<MeshRenderer>().material.color = Color.red;
        oldHandPoint = Vector3.zero;
        handPoint = Vector3.zero;
        receiverThread = new Thread(new ThreadStart(ReceivePointData));
        receiverThread.IsBackground = true;
        receiverThread.Start();

    }

    // Update is called once per frame
    void Update()
    {
        handPoint = Camera.main.transform.position + handVector;

        if(Vector3.Distance(handPoint,oldHandPoint) >= 0.01){
            HandPointer.transform.position = handPoint;
            switch (ConfianceLevel)
            {
                case 0 :
                    HandPointer.GetComponent<MeshRenderer>().material.color = Color.red;
                    break;
                case 1 :
                    HandPointer.GetComponent<MeshRenderer>().material.color = Color.yellow;
                    break;
                case 2: 
                    HandPointer.GetComponent<MeshRenderer>().material.color = Color.green ;
                    break;
                case 3: 
                    HandPointer.GetComponent<MeshRenderer>().material.color = Color.blue;
                    break;
                default:
                    break;
            }
            oldHandPoint = handPoint;
        }
    }

    void OnApplicationQuit(){
        stopThread();
    }
    private void stopThread(){
        if(receiverThread.IsAlive){
            receiverThread.Abort();
        }
        Server.Close();
    }

    private void ReceivePointData(){
        Server = new UdpClient(port);
        while (true)
        {
            try {
                IPEndPoint anyIp = new IPEndPoint(IPAddress.Any, 0);
                byte[] data = Server.Receive(ref anyIp);
                string jsonString = Encoding.UTF8.GetString(data);
                HandsPacket packet = JsonUtility.FromJson<HandsPacket>(jsonString);

                if(Hand == "Right"){
                    handVector = packet.right;
                    ConfianceLevel = packet.right_level;
                }
            } catch (Exception err){
                Debug.Log ("Exception --> " + err.ToString ());
            }  
        }
    }
}

public class HandsPacket {

    public Vector3 right;
    public int right_level;

}