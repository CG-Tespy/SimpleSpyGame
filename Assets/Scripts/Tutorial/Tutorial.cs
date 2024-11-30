using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using CGT.CharacterControls;
using SimpleSpyGame;

public class Tutorial : MonoBehaviour
{
	[SerializeField] private List<Task> tasks;
	[SerializeField] private Text textShower;
	public StealthPlayerController player; 
	public AltInputReader inputReader;

	private Task currentTask;

	[Header("For Debug")]
	public bool isThirdEyeToggled;
	public bool isTeleportToggled;

	public bool isHiding;

	private void Awake()
	{
		player = FindAnyObjectByType<StealthPlayerController>();
		inputReader = FindObjectOfType<AltInputReader>();       
	}

	private void OnEnable()
	{
		inputReader.ThirdEyeToggleStart += OnThridEyeToggle;
		player.TeleportStart += OnTeleportStart;
	}

	private void OnDisable()
	{
		inputReader.ThirdEyeToggleStart -= OnThridEyeToggle;
		player.TeleportStart -= OnTeleportStart;
	}

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
		isHiding = player.IsHiding;
	}

	private void GetFirstTask()
	{ 
		currentTask = tasks[0];
		currentTask.gameObject.SetActive(true);
		textShower.DOText(currentTask.taskText, 1f);
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
		textShower.text = ""; // clear the text before showing
		textShower.DOText(currentTask.taskText, 1f);
		tasks.RemoveAt(0);
	}

	public void OnThridEyeToggle()
	{
		isThirdEyeToggled = true;
	}

	public void OnTeleportStart()
	{
		isTeleportToggled = true;
	}
}
