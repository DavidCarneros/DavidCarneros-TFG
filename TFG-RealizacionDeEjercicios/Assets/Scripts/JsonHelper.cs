﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class JsonHelper {
    public static T[] FromJson<T> (string json) {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>> (json);
        return wrapper.Items;
    }

    public static string ToJson<T> (T[] array) {
        Wrapper<T> wrapper = new Wrapper<T> ();
        wrapper.Items = array;
        return JsonUtility.ToJson (wrapper);
    }

    public static string FixJson (string json) {
        string value = "{\"Items\":" + json + "}";
        return value;
    }

    [Serializable]
    private class Wrapper<T> {
        public T[] Items;
    }

}