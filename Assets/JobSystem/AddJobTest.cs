using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

/// <summary>
/// How to get a result of add compute using a job.
/// </summary>
public class AddJobTest : MonoBehaviour
{
    
    /// <summary>
    /// define a Job
    /// </summary>
    public struct AddJob : IJob
    {
        public float a;
        public float b;

        public NativeArray<float> result;

        public void Execute()
        {
            result[0] = a + b;
        }
    };

    private NativeArray<float> result;

    private JobHandle handle;
    // Start is called before the first frame update
    void Start()
    {
        //
        result = new NativeArray<float>(1, Allocator.Persistent);

        //
        var job = new AddJob()
        {
            a = 1,
            b = 2,
            result =  result
        };

        handle = job.Schedule();

    }

    // Update is called once per frame
    void Update()
    {
        //子线程执行完成之后，将数据从子线程读回到主线程
        if (handle.IsCompleted)
        {
            handle.Complete();
            Debug.Log($"result = {result[0]}");
        }
    }

    private void OnDestroy()
    {
        if (result.IsCreated)
        {
            result.Dispose();
        }
    }
}
