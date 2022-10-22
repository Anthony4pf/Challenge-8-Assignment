using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.Audio;


public class Audiomanager : MonoBehaviour
{
	private Scene scene;
	public Sound[] sounds;

	public static Audiomanager instance;
	AudioSource MyAudioSource;
    
    void Awake()
    {
		MyAudioSource = GetComponent<AudioSource>();
		scene = SceneManager.GetActiveScene();

		/*if(instance == null)
			instance = this;
		else 
		{
			Destroy(gameObject);
			return;
		}
		DontDestroyOnLoad(gameObject);*/

		foreach (Sound s in sounds)
		{
			s.source = gameObject.AddComponent<AudioSource>();
			s.source.clip = s.clip;
			s.source.volume = s.volume;
			s.source.pitch = s.pitch;
			s.source.loop = s.loop;
			s.source.loop = s.loop;
		}
    }
	void Start()
	{
		Play("Theme");
	}

	public void Play (string name)
	{
		Sound s = Array.Find(sounds, sound => sound.name == name);
		if(s == null)
			return;
		s.source.Play();
	}
	public void Stop(string name)
	{
		Sound s = Array.Find(sounds, sound => sound.name == name);
		if(s == null)
			return;
		s.source.Stop();
	}
}
