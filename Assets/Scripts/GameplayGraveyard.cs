using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayGraveyard : MonoBehaviour
{
	public delegate void OnFinishedEvent();

	public Transform[] resCharacter = new Transform[22];

	public AudioClip[] resSound;

	public Transform resArrow;

	public Transform resHit;

	public Transform resPoof;

	public List<List<Transform>> graveCharacter = new List<List<Transform>>();

	public List<Transform> graveArrow = new List<Transform>();

	public List<Transform> graveHit = new List<Transform>();

	public List<Transform> gravePoof = new List<Transform>();

	private Gameplay gameplay;

	public IEnumerator Init(OnFinishedEvent evt)
	{
		gameplay = Gameplay.gameplay;
		for (int k = 0; k < gameplay.global.cardPlayer.Length; k++)
		{
			int id = gameplay.global.cardPlayer[k].id;
			if (id != 0 && resCharacter[id] == null)
			{
				resCharacter[id] = Resources.Load<Transform>("Character/" + id);
				yield return new WaitForEndOfFrame();
			}
		}
		for (int j = 0; j < gameplay.global.cardEnemy.Length; j++)
		{
			int id2 = gameplay.global.cardEnemy[j].id;
			if (id2 != 0 && resCharacter[id2] == null)
			{
				resCharacter[id2] = Resources.Load<Transform>("Character/" + id2);
				yield return new WaitForEndOfFrame();
			}
		}
		resArrow = Resources.Load<Transform>("Projectile/Arrow");
		resHit = Resources.Load<Transform>("Other/Hit");
		resPoof = Resources.Load<Transform>("Other/Poof");
		for (int i = 0; i < resCharacter.Length; i++)
		{
			graveCharacter.Add(new List<Transform>());
			graveCharacter[i] = new List<Transform>();
		}
		yield return new WaitForEndOfFrame();
		evt();
	}

	public void AddCharacter(int id, Transform character)
	{
		if (id != 0 && !graveCharacter[id].Contains(character))
		{
			character.SetParent(gameplay.global.graveyard);
			graveCharacter[id].Add(character);
			character.gameObject.SetActive(value: false);
		}
	}

	public void AddArrow(Transform arrow)
	{
		arrow.SetParent(gameplay.global.graveyard);
		graveArrow.Add(arrow);
		arrow.gameObject.SetActive(value: false);
	}

	public void AddHit(Transform hit)
	{
		hit.SetParent(gameplay.global.graveyard);
		graveHit.Add(hit);
		hit.gameObject.SetActive(value: false);
	}

	public void AddPoof(Transform poof)
	{
		poof.SetParent(gameplay.global.graveyard);
		gravePoof.Add(poof);
	}

	public Transform GetCharacter(int id)
	{
		if (graveCharacter[id].Count != 0)
		{
			Transform result = graveCharacter[id][0];
			graveCharacter[id].RemoveAt(0);
			return result;
		}
		Transform transform = UnityEngine.Object.Instantiate(resCharacter[id], new Vector3(0f, 0f, 0f), Quaternion.identity) as Transform;
		transform.gameObject.SetActive(value: false);
		return transform;
	}

	public Transform GetArrow()
	{
		if (graveArrow.Count != 0)
		{
			Transform result = graveArrow[0];
			graveArrow.RemoveAt(0);
			return result;
		}
		Transform transform = UnityEngine.Object.Instantiate(resArrow, new Vector3(0f, 0f, 0f), Quaternion.identity) as Transform;
		transform.gameObject.SetActive(value: false);
		return transform;
	}

	public Transform GetHit()
	{
		if (graveHit.Count != 0)
		{
			Transform result = graveHit[0];
			graveHit.RemoveAt(0);
			return result;
		}
		Transform transform = UnityEngine.Object.Instantiate(resHit, new Vector3(0f, 0f, 0f), Quaternion.identity) as Transform;
		transform.gameObject.SetActive(value: false);
		return transform;
	}

	public Transform GetPoof()
	{
		if (gravePoof.Count != 0)
		{
			Transform result = gravePoof[0];
			gravePoof.RemoveAt(0);
			return result;
		}
		return UnityEngine.Object.Instantiate(resPoof, new Vector3(0f, 0f, 0f), Quaternion.identity) as Transform;
	}
}
