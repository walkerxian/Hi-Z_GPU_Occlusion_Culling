using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

    /// <summary>
    /// 计算一个数组里面所有数值的累加器
    /// </summary>
public class CounterTest : MonoBehaviour
{
    public struct CounterJob : IJob
    {
        public NativeArray<int> numbers;
        //public int result;
        public NativeArray<int> result;

        public void Execute()
        {
            //throw new System.NotImplementedException();

            int temp = 0;
            for (int i = 0; i < numbers.Length; i++)
            {
                temp += numbers[i];
            }
            result[0] = temp;
        }
    };
    
    static int numCount = 10;
    //声明NativeContainer数据
    NativeArray<int> numbers = new NativeArray<int>(numCount, Allocator.TempJob);
    NativeArray<int> result = new NativeArray<int>(1, Allocator.TempJob);
    private JobHandle handle;

    void Start()
    {
        //初始化数据
        for (int i = 0; i < numCount; i++)
        {
            numbers[i] = i + 1;
        }

        //装载数据
        var jobData = new CounterJob()
        {
            numbers = numbers,
            result = result
        };

        handle = jobData.Schedule();
        
    }

    void Update()
    {
        if (handle.IsCompleted)
        {
            handle.Complete();
            Debug.Log($"The result = {result[0]} ");
        }
    }
    
    public void OnDestroy()
    {
        result.Dispose();
        numbers.Dispose();
    }
}
