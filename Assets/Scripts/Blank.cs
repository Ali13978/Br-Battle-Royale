using UnityEngine;
using UnityEngine.SceneManagement;

public class Blank : MonoBehaviour
{
	public static string targetLoadLevel;

	private void Start()
	{
		SceneManager.LoadScene(string.Empty + targetLoadLevel);
	}
}
