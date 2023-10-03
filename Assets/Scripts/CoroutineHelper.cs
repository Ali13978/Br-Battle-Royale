using System.Collections.Generic;

public class CoroutineHelper : MonoBehaviourSingleton<CoroutineHelper>
{
	private List<Run> m_OnGUIObjects = new List<Run>();

	public int ScheduledOnGUIItems => m_OnGUIObjects.Count;

	public Run Add(Run aRun)
	{
		if (aRun != null)
		{
			m_OnGUIObjects.Add(aRun);
		}
		return aRun;
	}

	private void OnGUI()
	{
		for (int i = 0; i < m_OnGUIObjects.Count; i++)
		{
			Run run = m_OnGUIObjects[i];
			if (!run.abort && !run.isDone && run.onGUIaction != null)
			{
				run.onGUIaction();
			}
			else
			{
				run.isDone = true;
			}
		}
	}

	private void Update()
	{
		for (int num = m_OnGUIObjects.Count - 1; num >= 0; num--)
		{
			if (m_OnGUIObjects[num].isDone)
			{
				m_OnGUIObjects.RemoveAt(num);
			}
		}
	}
}
