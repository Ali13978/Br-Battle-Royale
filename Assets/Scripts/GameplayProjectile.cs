using System.Collections;
using UnityEngine;

public class GameplayProjectile : MonoBehaviour
{
	private Transform cacheT;

	private float time = 1.5f;

	private float v0x;

	private float v0y;

	private float timeNow;

	private GameplayCharacter from;

	private GameplayCharacter target;

	private Vector3 nowPosition;

	private Vector3 targetLocalPosition;

	private Vector3 xyzTarget;

	private bool isStart;

	public void Init(GameplayCharacter from, GameplayCharacter target, Vector3 targetLocalPosition)
	{
		timeNow = 0f;
		cacheT = base.transform;
		nowPosition = cacheT.localPosition;
		this.from = from;
		this.target = target;
		this.targetLocalPosition = targetLocalPosition + new Vector3(UnityEngine.Random.Range(-0.1f, 0.1f), UnityEngine.Random.Range(-0.1f, 0.1f), 0f);
		xyzTarget = new Vector3(this.targetLocalPosition.x - nowPosition.x, this.targetLocalPosition.y - nowPosition.y, nowPosition.z);
		v0x = xyzTarget.x / time;
		v0y = xyzTarget.y / time - -4.9f * time;
		base.gameObject.SetActive(value: true);
		isStart = true;
	}

	private void Update()
	{
		if (isStart)
		{
			if (timeNow >= time / 2f && Vector2.Distance(cacheT.localPosition, targetLocalPosition) <= 0.2f)
			{
				ProjectileStopMove();
			}
			else
			{
				ProjectileMove();
			}
		}
	}

	private void ProjectileStopMove()
	{
		isStart = false;
		Transform transform = cacheT;
		float x = targetLocalPosition.x;
		float y = targetLocalPosition.y;
		Vector3 localPosition = cacheT.localPosition;
		transform.localPosition = new Vector3(x, y, localPosition.z);
		if (from != null)
		{
			GameplayStatus status = from.GetStatus();
			if (status.attackTarget == GameplayStatus.ATTACKTARGET.Single)
			{
				if (target != null)
				{
					target.ApplyDamage(from);
				}
			}
			else
			{
				from.AreaDamage(cacheT.position - new Vector3(0f, 2f, 0f), status.attackRadius);
			}
		}
		StartCoroutine(DestroyProjectile());
	}

	private void ProjectileMove()
	{
		float num = v0x * timeNow;
		float num2 = v0y * timeNow + -4.9f * timeNow * timeNow;
		cacheT.localPosition = new Vector3(nowPosition.x + num, nowPosition.y + num2, xyzTarget.z);
		float z = Mathf.Atan2(v0y + -9.8f * timeNow, v0x) * 57.29578f;
		base.transform.localRotation = Quaternion.Euler(0f, 0f, z);
		timeNow += Time.deltaTime;
	}

	private IEnumerator DestroyProjectile()
	{
		yield return new WaitForSeconds(UnityEngine.Random.Range(0.5f, 1.5f));
		Gameplay.gameplay.global.gameplayGraveyard.AddArrow(base.transform);
	}
}
