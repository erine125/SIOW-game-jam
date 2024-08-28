using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeechTree
{
	public int InteractionId;

	private Node root;


	SpeechTree(int interactionId, Node root)
	{
		this.InteractionId = interactionId;
		this.root = root;
	}

	public string PrintableString ()
	{
		return "--INTERACTION #" + InteractionId + "--" + root.PrintableString(0);
	}


	public static Dictionary<int, SpeechTree> FromTextAsset (TextAsset asset)
	{
		Dictionary<int, SpeechTree> dict = new Dictionary<int, SpeechTree>();
		
		string[] rawLines = asset.ToString().Split("\n");
		string[][] splitLines = new string[rawLines.Length][];

		List<Node> roots = new List<Node>();

		for (int i = 0; i < rawLines.Length; i++)
		{
			splitLines[i] = rawLines[i].Split("\t");
			if (splitLines[i][0] != "")
			{
				int interactionId = int.Parse(splitLines[i][0]);
				Node rootNode = new Node(i+1, DataFromSplitLine(splitLines[i]));

				roots.Add(rootNode);
				dict.Add(interactionId, new SpeechTree(interactionId, rootNode));
			}
		}

		foreach (Node node in roots)
		{
			GetChildrenForNode(node, splitLines, 1);
		}

		return dict;
	}

	private static void GetChildrenForNode (Node node, string[][] splitLines, int depth)
	{
		if (depth > 16)
		{
			Debug.LogWarning("ERROR - Too deep while creating speech tree");
			return;
		}

        string[] splitLine = splitLines[node.GetId() - 1];

        for (int i = 5; i < splitLine.Length; i++)
		{
			if (splitLine[i].Trim () == "")
			{
				break;
			}
			int childId = int.Parse(splitLine[i]);

            Node child = new Node(childId, DataFromSplitLine(splitLines[childId-1]));
			node.AddChild(child);
			GetChildrenForNode(child, splitLines, depth + 1);
		}
    }

	private static NodeData DataFromSplitLine(string[] splitLine)
	{
		return new NodeData(splitLine[1], splitLine[2], splitLine[3], splitLine[4]);
	}
}

class Node
{
	private int id;
	public int GetId() { return id; }

	private NodeData data;
	public NodeData GetData() { return data; }

	private List<Node> children;
	public int GetChildCount() { return children.Count; }
	public Node GetChildAt(int idx) { return children[idx]; }
	public void AddChild(Node child) { children.Add(child); }

	public Node (int id, NodeData data)
	{
		this.id = id;
		this.data = data;
		children = new List<Node>();
	}

	public string PrintableString (int depth)
	{
		string result = "\n" + new string('\t', depth) + data.PrintableString();
		foreach (Node child in children)
		{
			result += child.PrintableString(depth + 1);
		}
		return result;
	}
}

struct NodeData
{
	public string speaker;
	public string text;
	public string action;
	public string showChoices;

	public NodeData (string speaker, string text, string action, string showChoices)
	{
		this.speaker = speaker.Replace('%', ',');
		this.text = text.Replace('%', ',');
		this.action = action.Replace('%', ',');
		this.showChoices = showChoices.Replace('%', ',');
	}

	public string PrintableString ()
	{
		return speaker + ": " + text;
	}
}