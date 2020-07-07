using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;

public class HandsTrackingHandler : MonoBehaviour
{
    // Evento derecha
    public delegate void _OnPointsRightReceived (HandInformation information);
    public static event _OnPointsRightReceived OnPointsRightReceived; 

    // Evento izquierda
    public delegate void _OnPointsLeftReceived (HandInformation information);
    public static event _OnPointsLeftReceived OnPointsLeftReceived; 

    // Thread recive information 
    Thread receiverThread;
    UdpClient Server;

    public int port;
    public GameObject kinectObject;
    public GameObject HandsTrackingObject;
    public String ActiveHand;
    public GameObject RightPointer;
    public GameObject LeftPointer;

    // Start is called before the first frame update
    void Start()
    {
        receiverThread = new Thread (new ThreadStart(ReceivePointData));
        receiverThread.IsBackground = true;
        receiverThread.Start();
        this.ActiveHand = "Right";
        this.LeftPointer.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        HandsTrackingObject.transform.position = kinectObject.transform.position;
        HandsTrackingObject.transform.eulerAngles = kinectObject.transform.eulerAngles + new Vector3(180, 0, 0);
    }

    void OnApplicationQuit () {
        this.stopThread ();
    }

    private void stopThread () {
        if (receiverThread.IsAlive) {
            receiverThread.Abort ();
        }
        Server.Close ();
    }

    public void SetHand(string Hand)
    {
        this.ActiveHand = Hand;
        if (Hand == "Left")
        {
            this.LeftPointer.SetActive(true);
            this.RightPointer.SetActive(false);
        }
        else
        {
            this.LeftPointer.SetActive(false);
            this.RightPointer.SetActive(true);
        }
    }

    public GameObject GetActiveHand()
    {
        if (this.ActiveHand == "Left")
        {
            return this.LeftPointer;
        }
        else
        {
            return this.RightPointer;
        }
    }


    private void ReceivePointData () {
        Server = new UdpClient (port);
        while (true) {
            try {
                IPEndPoint anyIp = new IPEndPoint (IPAddress.Any, 0);
                byte[] data = Server.Receive (ref anyIp);
                string jsonString = Encoding.UTF8.GetString (data);
                HandsPacket packet = JsonUtility.FromJson<HandsPacket> (jsonString);


                Debug.Log("Packete recivido");

                Vector3 handPointRight = packet.right;
                HandInformation informationRight = new HandInformation(handPointRight, packet.q_right, packet.right_level);

                Vector3 handPointLeft = packet.left;
                HandInformation informationLeft = new HandInformation(handPointLeft, packet.q_left, packet.left_level);

                if (OnPointsRightReceived != null)
                {
                    OnPointsRightReceived(informationRight);
                }
                if (OnPointsLeftReceived != null)
                {
                    OnPointsLeftReceived(informationLeft);
                }

            } catch (Exception err) {
                Debug.Log ("Exception --> " + err.ToString ());
            }
        }
    }


}

public class HandsPacket {

    public Vector3 right;
    public Vector3 left;
    public Quaternion q_right;
    public Quaternion q_left;
    public int right_level;
    public int left_level;

}