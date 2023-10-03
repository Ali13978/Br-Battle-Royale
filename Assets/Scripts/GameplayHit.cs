using UnityEngine;

public class GameplayHit : MonoBehaviour
{
	public void EndAnimation()
	{
		Gameplay.gameplay.global.gameplayGraveyard.AddHit(base.transform);
	}
}
