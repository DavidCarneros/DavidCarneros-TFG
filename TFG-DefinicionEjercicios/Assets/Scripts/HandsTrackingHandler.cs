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
    Vector3 kinectPosition;

    public int port;
    public GameObject kinectObject;

    // Start is called before the first frame update
    void Start()
    {
        receiverThread = new Thread (new ThreadStart(ReceivePointData));
        receiverThread.IsBackground = true;
        receiverThread.Start();
    }

    // Update is called once per frame
    void Update()
    {
        kinectPosition = kinectObject.transform.position;
    }

    void OnApplicationQuit () {
        stopThread ();
    }

    private void stopThread () {
        if (receiverThread.IsAlive) {
            receiverThread.Abort ();
        }
        Server.Close ();
    }


    private void ReceivePointData () {
        Server = new UdpClient (port);
        while (true) {
            try {
                IPEndPoint anyIp = new IPEndPoint (IPAddress.Any, 0);
                byte[] data = Server.Receive (ref anyIp);
                string jsonString = Encoding.UTF8.GetString (data);
                HandsPacket packet = JsonUtility.FromJson<HandsPacket> (jsonString);
                Debug.Log("PACKET");
                // Solo derecha de momento 
                //Vector3 kinectPosition = this.kinectObject.transform.position;
                Vector3 handPoint = packet.right + kinectPosition;
                HandInformation information = new HandInformation(handPoint, packet.q_right, packet.right_level);
                // lanzamos evento
                if(OnPointsRightReceived!=null){
                    OnPointsRightReceived(information);
                    Debug.Log("Lanzado evento");
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