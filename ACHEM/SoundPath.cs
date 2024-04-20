using System.Collections.Generic;
using UnityEngine;

public class SoundPath : MonoBehaviour
{
	public class Edge
	{
		public int id;

		public int nodeA;

		public int nodeB;
	}

	public class Node
	{
		public int id;

		public Vector3 position;

		public List<int> neighbors;

		public float audioDistance;

		public float pathWidth;
	}

	public List<Edge> Edges;

	public List<Node> Vertices;
}
