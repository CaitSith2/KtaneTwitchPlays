﻿using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class InterruptMusic : MonoBehaviour
{
	public static InterruptMusic Instance { get; private set; }

	private static readonly FieldInfo VolumeLevelGameplayField;
	private readonly Dictionary<int, float> _oldVolumesGameplay = new Dictionary<int, float>();
	private static readonly FieldInfo VolumeLevelOtherField;
	private readonly Dictionary<int, float> _oldVolumesOther = new Dictionary<int, float>();

	static InterruptMusic()
	{
		VolumeLevelGameplayField = typeof(GameplayMusicController).GetField("volumeLevel", BindingFlags.NonPublic | BindingFlags.Instance);
		VolumeLevelOtherField = typeof(MusicController).GetField("volumeLevel", BindingFlags.NonPublic | BindingFlags.Instance);
	}

	private void Awake() => Instance = this;

	public void SetMusicInterrupt(bool enableInterrupt)
	{
		GameplayMusicController[] gameplayMusicControllers = FindObjectsOfType<GameplayMusicController>();
		foreach (GameplayMusicController musicController in gameplayMusicControllers)
		{
			int musicControllerInstanceID = musicController.GetInstanceID();
			if (enableInterrupt)
			{
				if (!_oldVolumesGameplay.ContainsKey(musicControllerInstanceID))
					_oldVolumesGameplay[musicControllerInstanceID] = (float) VolumeLevelGameplayField.GetValue(musicController);
				musicController.SetVolume(0.0f, true);
			}
			else
			{
				if (!_oldVolumesGameplay.ContainsKey(musicControllerInstanceID)) continue;
				musicController.SetVolume(_oldVolumesGameplay[musicControllerInstanceID], true);
				_oldVolumesGameplay.Remove(musicControllerInstanceID);
			}
		}

		MusicController[] musicControllers = FindObjectsOfType<MusicController>();
		{
			foreach (MusicController musicController in musicControllers)
			{
				int musicControllerInstanceID = musicController.GetInstanceID();
				if (enableInterrupt)
				{
					if (!_oldVolumesOther.ContainsKey(musicControllerInstanceID))
						_oldVolumesOther[musicControllerInstanceID] = (float) VolumeLevelOtherField.GetValue(musicController);
					musicController.SetVolume(0.0f, true);
				}
				else
				{
					if (_oldVolumesOther.ContainsKey(musicControllerInstanceID))
					{
						musicController.SetVolume(_oldVolumesOther[musicControllerInstanceID], true);
						_oldVolumesOther.Remove(musicControllerInstanceID);
					}
				}
			}
		}
	}
}
