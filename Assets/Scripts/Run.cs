using System;
using System.Collections;
using UnityEngine;

public class Run
{
	public class CTempWindow
	{
		public Run inst;

		public Rect pos;

		public string title;

		public int winID = GUIHelper.GetFreeWindowID();

		public void Close()
		{
			inst.Abort();
		}
	}

	public bool isDone;

	public bool abort;

	private IEnumerator action;

	public Action onGUIaction;

	public Coroutine WaitFor => MonoBehaviourSingleton<CoroutineHelper>.Instance.StartCoroutine(_WaitFor(null));

	public static Run EachFrame(Action aAction)
	{
		Run run = new Run();
		run.action = _RunEachFrame(run, aAction);
		run.Start();
		return run;
	}

	private static IEnumerator _RunEachFrame(Run aRun, Action aAction)
	{
		aRun.isDone = false;
		while (!aRun.abort && aAction != null)
		{
			aAction();
			yield return null;
		}
		aRun.isDone = true;
	}

	public static Run Every(float aInitialDelay, float aDelay, Action aAction)
	{
		Run run = new Run();
		run.action = _RunEvery(run, aInitialDelay, aDelay, aAction);
		run.Start();
		return run;
	}

	private static IEnumerator _RunEvery(Run aRun, float aInitialDelay, float aSeconds, Action aAction)
	{
		aRun.isDone = false;
		if (aInitialDelay > 0f)
		{
			yield return new WaitForSeconds(aInitialDelay);
		}
		else
		{
			int FrameCount2 = Mathf.RoundToInt(0f - aInitialDelay);
			for (int j = 0; j < FrameCount2; j++)
			{
				yield return null;
			}
		}
		while (!aRun.abort && aAction != null)
		{
			aAction();
			if (aSeconds > 0f)
			{
				yield return new WaitForSeconds(aSeconds);
				continue;
			}
			int FrameCount = Mathf.Max(1, Mathf.RoundToInt(0f - aSeconds));
			for (int i = 0; i < FrameCount; i++)
			{
				yield return null;
			}
		}
		aRun.isDone = true;
	}

	public static Run After(float aDelay, Action aAction)
	{
		Run run = new Run();
		run.action = _RunAfter(run, aDelay, aAction);
		run.Start();
		return run;
	}

	private static IEnumerator _RunAfter(Run aRun, float aDelay, Action aAction)
	{
		aRun.isDone = false;
		yield return new WaitForSeconds(aDelay);
		if (!aRun.abort)
		{
			aAction?.Invoke();
		}
		aRun.isDone = true;
	}

	public static Run Lerp(float aDuration, Action<float> aAction)
	{
		Run run = new Run();
		run.action = _RunLerp(run, aDuration, aAction);
		run.Start();
		return run;
	}

	private static IEnumerator _RunLerp(Run aRun, float aDuration, Action<float> aAction)
	{
		aRun.isDone = false;
		float t = 0f;
		while (t < 1f)
		{
			t = Mathf.Clamp01(t + Time.deltaTime / aDuration);
			if (!aRun.abort)
			{
				aAction?.Invoke(t);
			}
			yield return null;
		}
		aRun.isDone = true;
	}

	public static Run OnDelegate(SimpleEvent aDelegate, Action aAction)
	{
		Run run = new Run();
		run.action = _RunOnDelegate(run, aDelegate, aAction);
		run.Start();
		return run;
	}

	private static IEnumerator _RunOnDelegate(Run aRun, SimpleEvent aDelegate, Action aAction)
	{
		aRun.isDone = false;
		Action action = delegate
		{
			aAction();
		};
		aDelegate.Add(action);
		while (!aRun.abort && aAction != null)
		{
			yield return null;
		}
		aDelegate.Remove(action);
		aRun.isDone = true;
	}

	public static Run Coroutine(IEnumerator aCoroutine)
	{
		Run run = new Run();
		run.action = _Coroutine(run, aCoroutine);
		run.Start();
		return run;
	}

	private static IEnumerator _Coroutine(Run aRun, IEnumerator aCoroutine)
	{
		yield return MonoBehaviourSingleton<CoroutineHelper>.Instance.StartCoroutine(aCoroutine);
		aRun.isDone = true;
	}

	public static Run OnGUI(float aDuration, Action aAction)
	{
		Run run = new Run();
		run.onGUIaction = aAction;
		if (aDuration > 0f)
		{
			run.action = _RunAfter(run, aDuration, null);
		}
		else
		{
			run.action = null;
		}
		run.Start();
		MonoBehaviourSingleton<CoroutineHelper>.Instance.Add(run);
		return run;
	}

	public static CTempWindow CreateGUIWindow(Rect aPos, string aTitle, Action<CTempWindow> aAction)
	{
		CTempWindow tmp = new CTempWindow();
		tmp.pos = aPos;
		tmp.title = aTitle;
		tmp.inst = OnGUI(0f, delegate
		{
			tmp.pos = GUI.Window(tmp.winID, tmp.pos, delegate
			{
				aAction(tmp);
			}, tmp.title);
		});
		return tmp;
	}

	private void Start()
	{
		if (action != null)
		{
			MonoBehaviourSingleton<CoroutineHelper>.Instance.StartCoroutine(action);
		}
	}

	public IEnumerator _WaitFor(Action aOnDone)
	{
		while (!isDone)
		{
			yield return null;
		}
		aOnDone?.Invoke();
	}

	public void Abort()
	{
		abort = true;
	}

	public Run ExecuteWhenDone(Action aAction)
	{
		Run run = new Run();
		run.action = _WaitFor(aAction);
		run.Start();
		return run;
	}
}
