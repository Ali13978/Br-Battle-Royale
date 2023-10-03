using System;

public class SimpleEvent
{
	private Action m_Delegate = delegate
	{
	};

	public void Add(Action aDelegate)
	{
		m_Delegate = (Action)Delegate.Combine(m_Delegate, aDelegate);
	}

	public void Remove(Action aDelegate)
	{
		m_Delegate = (Action)Delegate.Remove(m_Delegate, aDelegate);
	}

	public void Run()
	{
		m_Delegate();
	}
}
