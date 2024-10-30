using System;
using UnityEngine;

public enum Talkitiveness
{
    Quiet,
    Reluctant,
    Talkitive
}

public static class SpeechUtil
{
    public static void Action(string code, Interactor interactor)
    {
        if (code.Length > 5 && code.Substring (0, 5) == "State")
        {
            int number = int.Parse(code.Substring(5));
            MasterState.Get().UpdateState(number);
        }
        else if (code.Length > 11 && code.Substring (0, 11) == "UpdateState")
        {
            int number = int.Parse(code.Substring(11));
            MasterState.Get().UpdateState(number);
        }
        else
        {
            switch (code)
            {
                case "MakeTalkitive":
                    interactor.Talkitiveness = Talkitiveness.Talkitive;
                    break;
                case "MakeReluctant":
                    interactor.Talkitiveness = Talkitiveness.Reluctant;
                    break;
                case "MakeQuiet":
                    interactor.Talkitiveness = Talkitiveness.Quiet;
                    break;
            }

            MasterState.Get ().StoreUpdatedInteractor (interactor);
        }
    }
}

/**
Speech Asset Format
- Must be a txt file but formatted as a TSV.
    - The name before the extension needs to uniquely identify the interactor,
      in other words, don't repeat names
- Each row represents one node. The ID of a node is its row number.
- Nodes can have multiple parents. 
- If a node has no children, it ends the conversation
- You cannot have loops
- Each row should be of the same form, with the columns as follows:
    1) The interaction tree ID, empty for non-root nodes
    2) The speaker
    3) The text
    4) The color of the text, hex code (e.g. #FF00FF for purple)
    5) Action code, can be any string, happens after the user clicks to go next
  >=6) ID numbers for next dialog options (up to 4 choice). (Line numbers are
       the same as is shown in google sheets for a given row.)
*/