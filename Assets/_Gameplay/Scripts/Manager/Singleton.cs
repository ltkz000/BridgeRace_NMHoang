using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T ins;

    public static T Ins
    {
        get
        {
            if(ins == null)
            {
                ins = FindObjectOfType<T>();
            }

            if(ins == null)
            {   
                GameObject gameObject = new GameObject();
                ins = gameObject.AddComponent<T>();
                gameObject.name = typeof(T).ToString();
            }

            return ins;
        }
    }
}