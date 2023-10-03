using UnityEngine;

public class GameplayPrepare : MonoBehaviour
{
	public void End()
	{
		base.transform.parent.Find("Gameplay").GetComponent<Gameplay>().StartGameplay();
		base.gameObject.SetActive(value: false);
	}

	public void Sound(string sound)
	{
		float volume = 0.81f;
		if (sound == "Slash3")
		{
			volume = 0.71f;
		}
		base.transform.root.Find("Sound").GetComponent<GlobalSound>().AudioOnce("Sound/" + sound, volume);
	}
}
