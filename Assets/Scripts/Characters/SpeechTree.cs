using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeechTree
{
	public int InteractionId;

	private SpeechNode root;


	SpeechTree(int interactionId, SpeechNode root)
	{
		this.InteractionId = interactionId;
		this.root = root;
	}

	public string PrintableString ()
	{
		return "--INTERACTION #" + InteractionId + "--" + root.PrintableString(0);
	}

	internal SpeechNode GetRoot ()
	{
		return root;
	}


	public static Dictionary<int, SpeechTree> FromTextAsset (TextAsset asset)
	{
		Dictionary<int, SpeechTree> dict = new Dictionary<int, SpeechTree>();
		
		string[] rawLines = asset.ToString().Split("\n");
		string[][] splitLines = new string[rawLines.Length][];

		List<SpeechNode> roots = new List<SpeechNode>();

		for (int i = 0; i < rawLines.Length; i++)
		{
			splitLines[i] = rawLines[i].Split("\t");
			if (splitLines[i][0] != "")
			{
				int interactionId = int.Parse(splitLines[i][0]);
				SpeechNode rootNode = new SpeechNode(i+1, DataFromSplitLine(splitLines[i]));

				roots.Add(rootNode);
				dict.Add(interactionId, new SpeechTree(interactionId, rootNode));
			}
		}

		foreach (SpeechNode node in roots)
		{
			GetChildrenForNode(node, splitLines, 1);
		}

		return dict;
	}

	private static void GetChildrenForNode (SpeechNode node, string[][] splitLines, int depth)
	{
		if (depth > 120)
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

            SpeechNode child = new SpeechNode(childId, DataFromSplitLine(splitLines[childId-1]));
			node.AddChild(child);
			GetChildrenForNode(child, splitLines, depth + 1);
		}
    }

	private static SpeechNodeData DataFromSplitLine(string[] splitLine)
	{
		return new SpeechNodeData(splitLine[1], splitLine[2], splitLine[3], splitLine[4]);
	}
}

class SpeechNode
{
	private int id;
	public int GetId() { return id; }

	private SpeechNodeData data;
	public SpeechNodeData GetData() { return data; }

	private List<SpeechNode> children;
	public int GetChildCount() { return children.Count; }
	public SpeechNode GetChildAt(int idx) { return children[idx]; }
	public void AddChild(SpeechNode child) { children.Add(child); }

	public SpeechNode (int id, SpeechNodeData data)
	{
		this.id = id;
		this.data = data;
		children = new List<SpeechNode>();
	}

	public string PrintableString (int depth)
	{
		string result = "\n" + new string('\t', depth) + data.PrintableString();
		foreach (SpeechNode child in children)
		{
			result += child.PrintableString(depth + 1);
		}
		return result;
	}
}

struct SpeechNodeData
{
	public string speaker;
	public string text;
    public string color;
    public string action;

	public SpeechNodeData (string speaker, string text, string color, string action)
	{
		this.speaker = speaker.Replace('%', ',');
		this.text = text.Replace('%', ',');
		this.color = color.Replace('%', ',');
        this.action = action.Replace('%', ',');
	}

	public string PrintableString ()
	{
		return speaker + ": " + text;
	}
}