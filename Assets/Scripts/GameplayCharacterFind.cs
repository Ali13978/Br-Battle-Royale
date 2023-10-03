using UnityEngine;

public class GameplayCharacterFind : MonoBehaviour
{
	public GameplayCharacter gc;

	private void OnTriggerEnter(Collider c)
	{
		if (gc.CollideFind(c.transform.parent))
		{
			base.gameObject.SetActive(value: false);
		}
	}
}
