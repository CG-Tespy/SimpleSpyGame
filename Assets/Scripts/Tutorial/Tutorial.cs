using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Tutorial : MonoBehaviour
{
    [SerializeField] private List<Task> tasks;
    private Task currentTask;

    private void Start()
    {
        foreach (var t in tasks)
        {
            t.gameObject.SetActive(false);
		}
        GetFirstTask();
    }

    private void Update()
    {
        if (currentTask.Done())
        {
            ChangeToNextTask();
		}
    }

    private void GetFirstTask()
    { 
        currentTask = tasks[0];
        currentTask.gameObject.SetActive(true);
        tasks.RemoveAt(0);
	}

    private void ChangeToNextTask()
    {
        if (tasks.Count == 0)
        {
            Debug.Log("No more tasks");
            currentTask.gameObject.SetActive(false);
            this.gameObject.SetActive(false);
            return;
		}
        currentTask.gameObject.SetActive(false);
        currentTask = tasks[0];
        currentTask.gameObject.SetActive(true);
        tasks.RemoveAt(0);
	}
}
