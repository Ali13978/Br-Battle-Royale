using UnityEngine;

public class GlobalSound : MonoBehaviour
{
	private AudioSource audioSourceLoop;

	private AudioSource audioSourceOnce;

	public void AudioLoop(string source, float volume)
	{
		if (audioSourceLoop == null)
		{
			audioSourceLoop = base.transform.Find("Loop").GetComponent<AudioSource>();
		}
		AudioClip clip = Resources.Load<AudioClip>(string.Empty + source);
		audioSourceLoop.clip = clip;
		audioSourceLoop.volume = volume;
		audioSourceLoop.loop = true;
		audioSourceLoop.Play();
	}

	public void AudioOnce(string source, float volume)
	{
		if (audioSourceOnce == null)
		{
			audioSourceOnce = base.transform.Find("Once").GetComponent<AudioSource>();
		}
		AudioClip clip = Resources.Load<AudioClip>(string.Empty + source);
		audioSourceOnce.PlayOneShot(clip, volume);
	}

	public void AudioOnce(AudioClip audioClip, float volume)
	{
		if (audioSourceOnce == null)
		{
			audioSourceOnce = base.transform.Find("Once").GetComponent<AudioSource>();
		}
		audioSourceOnce.PlayOneShot(audioClip, volume);
	}

	public AudioSource GetAudioSource()
	{
		return audioSourceLoop;
	}
}
