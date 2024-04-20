using System.Collections.Generic;
using UnityEngine;

public class Sequencer
{
	private LinkedList<SequencedEvent> events;

	public int Count => events.Count;

	public Sequencer()
	{
		events = new LinkedList<SequencedEvent>();
	}

	public void Add(SequencedEvent e)
	{
		if (events.Count == 0)
		{
			events.AddLast(e);
			return;
		}
		for (LinkedListNode<SequencedEvent> linkedListNode = events.First; linkedListNode != events.Last; linkedListNode = linkedListNode.Next)
		{
			SequencedEvent value = linkedListNode.Value;
			if (e.time < value.time)
			{
				events.AddBefore(linkedListNode, e);
				return;
			}
		}
		events.AddLast(e);
	}

	public LinkedList<SequencedEvent> GetCurrentEvents()
	{
		float time = Time.time;
		LinkedList<SequencedEvent> linkedList = new LinkedList<SequencedEvent>();
		while (events.Count > 0 && events.First.Value.time <= time)
		{
			linkedList.AddLast(events.First.Value);
			events.RemoveFirst();
		}
		return linkedList;
	}
}
