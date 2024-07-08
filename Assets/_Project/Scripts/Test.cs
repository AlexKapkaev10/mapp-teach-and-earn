using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class Test : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void Hello();
    

    public void ClickHello()
    {
        Hello();
    }
}
