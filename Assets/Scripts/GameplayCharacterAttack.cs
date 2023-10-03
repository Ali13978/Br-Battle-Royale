using UnityEngine;

public class GameplayCharacterAttack : MonoBehaviour
{
	public GameplayCharacter gc;

	private void OnTriggerEnter(Collider c)
	{
		if (gc.CollideAttack(c.transform.parent))
		{
			base.gameObject.SetActive(value: false);
		}
	}
}
