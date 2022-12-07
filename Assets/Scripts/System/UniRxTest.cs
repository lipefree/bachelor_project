using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using UniRx;
using System.Threading.Tasks;
using System;
using Unity.Jobs;

public class UniRxTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    MyJob j = new MyJob();

    JobHandle handle = j.Schedule();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void test() { 
        Debug.Log("Pls print");
    }
}

public struct MyJob : IJob
{
    public void Execute()
    {
        Debug.Log("test para");
    }
}
