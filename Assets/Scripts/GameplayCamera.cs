using System.Collections;
using UnityEngine;

public class GameplayCamera : MonoBehaviour
{
	public Transform cameraObj;

	private Vector3 nowPosition;

	private bool isShake;

	private bool isShakeLoop;

	private bool isShakeLoopEnd;

	private float shakeIntensity;

	private float shakeDecay;

	public void InitShake(float intensity)
	{
		if (!isShake && !isShakeLoop)
		{
			nowPosition = cameraObj.localPosition;
			shakeIntensity = intensity;
			shakeDecay = 0.01f;
			isShake = true;
		}
	}

	public void InitShakeLoop()
	{
		nowPosition = cameraObj.localPosition;
		shakeIntensity = 0.1f;
		shakeDecay = 0f;
		isShakeLoop = true;
	}

	public void EndShakeLoop()
	{
		isShakeLoopEnd = true;
		shakeIntensity = 0.15f;
		shakeDecay = 0.01f;
	}

	private IEnumerator EndShakeLoopCoroutine()
	{
		yield return new WaitForSeconds(0.1f);
		isShakeLoop = false;
	}

	private void Update()
	{
		if (isShakeLoop)
		{
			ShakeLoop();
		}
		else if (isShake)
		{
			ShakeOnce();
		}
	}

	private void ShakeOnce()
	{
		if (shakeIntensity > 0f)
		{
			cameraObj.localPosition = nowPosition + UnityEngine.Random.insideUnitSphere * shakeIntensity;
			shakeIntensity -= shakeDecay;
		}
		else if (isShake)
		{
			isShake = false;
		}
	}

	private void ShakeLoop()
	{
		if (!isShakeLoopEnd)
		{
			cameraObj.localPosition = nowPosition + UnityEngine.Random.insideUnitSphere * shakeIntensity;
		}
		else if (shakeIntensity > 0f)
		{
			cameraObj.localPosition = nowPosition + UnityEngine.Random.insideUnitSphere * shakeIntensity;
			shakeIntensity -= shakeDecay;
		}
		else if (isShakeLoop)
		{
			isShakeLoop = false;
			isShakeLoopEnd = false;
		}
	}
}
