using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class Tutorial : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] private List<Task> tasks;
    private Task currentTask;

    private void Start()
    {
        foreach (var t in tasks)
        {
            t.tutorial = this;
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
        currentTask.text.DOText(currentTask.taskText, 1f);
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
        currentTask.text.DOText(currentTask.taskText, 1f);
        tasks.RemoveAt(0);
	}
}
