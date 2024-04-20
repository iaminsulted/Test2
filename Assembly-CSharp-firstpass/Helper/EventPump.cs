using System;
using System.Collections.Generic;
using UnityEngine;
using Windows.Kinect;

namespace Helper
{
	// Token: 0x02000031 RID: 49
	[ExecuteInEditMode]
	internal class EventPump : MonoBehaviour
	{
		// Token: 0x17000009 RID: 9
		// (get) Token: 0x060001D9 RID: 473 RVA: 0x00016488 File Offset: 0x00014688
		// (set) Token: 0x060001DA RID: 474 RVA: 0x0001648F File Offset: 0x0001468F
		public static EventPump Instance { get; private set; }

		// Token: 0x060001DB RID: 475 RVA: 0x00016498 File Offset: 0x00014698
		public static void EnsureInitialized()
		{
			try
			{
				if (EventPump.Instance == null)
				{
					object obj = EventPump.s_Lock;
					lock (obj)
					{
						if (EventPump.Instance == null)
						{
							GameObject gameObject = new GameObject("Kinect Desktop Event Pump");
							EventPump.Instance = gameObject.AddComponent<EventPump>();
							UnityEngine.Object.DontDestroyOnLoad(gameObject);
						}
					}
				}
			}
			catch
			{
				Debug.LogError("Events must be registered on the main thread.");
			}
		}

		// Token: 0x060001DC RID: 476 RVA: 0x00016520 File Offset: 0x00014720
		private void Update()
		{
			Queue<Action> queue = this.m_Queue;
			lock (queue)
			{
				while (this.m_Queue.Count > 0)
				{
					Action action = this.m_Queue.Dequeue();
					try
					{
						action();
					}
					catch
					{
					}
				}
			}
		}

		// Token: 0x060001DD RID: 477 RVA: 0x00016590 File Offset: 0x00014790
		private void OnApplicationQuit()
		{
			KinectSensor @default = KinectSensor.GetDefault();
			if (@default != null && @default.IsOpen)
			{
				@default.Close();
			}
			NativeObjectCache.Flush();
		}

		// Token: 0x060001DE RID: 478 RVA: 0x000165BC File Offset: 0x000147BC
		public void Enqueue(Action action)
		{
			Queue<Action> queue = this.m_Queue;
			lock (queue)
			{
				this.m_Queue.Enqueue(action);
			}
		}

		// Token: 0x040001E7 RID: 487
		private static object s_Lock = new object();

		// Token: 0x040001E8 RID: 488
		private Queue<Action> m_Queue = new Queue<Action>();
	}
}
