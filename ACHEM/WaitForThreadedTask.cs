using System;
using System.Threading;
using UnityEngine;

public class WaitForThreadedTask : CustomYieldInstruction
{
	private Thread thread;

	public override bool keepWaiting => thread.IsAlive;

	public WaitForThreadedTask(Action task)
	{
		thread = new Thread((ThreadStart)delegate
		{
			task();
		});
		try
		{
			thread.Start();
		}
		catch (Exception message)
		{
			Debug.LogError(message);
		}
	}
}
