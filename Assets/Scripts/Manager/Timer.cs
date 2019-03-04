using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer
{
    private static MonoBehaviour behaviour;
    public delegate void Task();

    public static void Schedule(MonoBehaviour _behaviour, float delay, Task task)//زمانبندی
    {
        behaviour = _behaviour;
        behaviour.StartCoroutine(DoTask(task, delay));
        //Debug.Log(behaviour.name + "> " + delay + ">" + task.Method.Name);
    }

    private static IEnumerator DoTask(Task task, float delay)
    {
        yield return new WaitForSeconds(delay);
        task();
    }
}
