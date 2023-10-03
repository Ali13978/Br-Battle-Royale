using UnityEngine;

public class MonoBehaviourSingleton<TSelfType> : MonoBehaviour where TSelfType : MonoBehaviour
{
	private static TSelfType m_Instance;

	public static TSelfType Instance
	{
		get
		{
			if ((Object)m_Instance == (Object)null)
			{
				m_Instance = (TSelfType)UnityEngine.Object.FindObjectOfType(typeof(TSelfType));
				if ((Object)m_Instance == (Object)null)
				{
					m_Instance = new GameObject(typeof(TSelfType).Name).AddComponent<TSelfType>();
				}
				Object.DontDestroyOnLoad(m_Instance.gameObject);
			}
			return m_Instance;
		}
	}
}
