using System;

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
        switch (code)
        {
            case "MakeQuiet":
                interactor.Talkitiveness = Talkitiveness.Quiet;
                break;
            case "MakeReluctant":
                interactor.Talkitiveness = Talkitiveness.Reluctant;
                break;
            case "MakeTalkitive":
                interactor.Talkitiveness = Talkitiveness.Talkitive;
                break;
            case "Hide":
                interactor.Visible = false;
                break;
            case "Show":
                interactor.Visible = true;
                break;
        }

        interactor.ChangedState ();
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