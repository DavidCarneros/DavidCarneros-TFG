using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class HandInformation {


    public Vector3 position;
    public Quaternion quaternion;
    public int confianceLevel;

    public HandInformation(Vector3 position, Quaternion quaternion, int confianceLevel){
        this.position = position;
        this.quaternion = quaternion;
        this.confianceLevel = confianceLevel;
    }


}