using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class TimeManager : MonoBehaviour
{
	/*public static Action OnMinuteChanged;
	public static Action OnHourChanged;
	public static Action OnDayChanged;
	public static Action OnMonthChanged;
	public static Action OnYearChanged;*/
	public static Action OnGameTick;

	public DateTime CurrentDateTime { get; private set; }
	public static int Minute { get; private set; }

	public GameObject pausedAlert;

	private float inGameTimeRatio = 1f;
	private float timer;
	private float startTime;
	private bool paused;

	void Start()
	{
		CurrentDateTime = new DateTime(1815, 3, 16);
		startTime = Time.time;
		timer = inGameTimeRatio;

		pausedAlert?.SetActive(paused); // false to hide, true to show
	}

	// Update is called once per frame
	void Update()
	{
		if (!paused)
		{
			// Amount of time since timer started in seconds
			/*float t = Time.time - startTime;

			string minutes = ((int)t / 60).ToString();
			string seconds = (t % 60).ToString("f2");



			timerText.text = minutes + ":" + seconds;*/

			timer -= Time.deltaTime;
			if (timer <= 0)
			{
				Minute++;
				OnGameTick?.Invoke(); // ?. checks if not null before invoking action (callback)

				timer = inGameTimeRatio;
			}
		}

	}

	public void PauseGame()
	{
		paused = !paused; // Toggles between paused and not paused.

		pausedAlert?.SetActive(paused); // false to hide, true to show
	}

}
