using System;

public enum Talkitiveness
{
    Quiet,
    Reluctant,
    Talkitive
}

/**
Speech Asset Format
- Must be a txt file but formatted as a TSV.
- Each row represents one node. The ID of a node is its row number.
- Nodes can have multiple parents. 
- If a node has no children, it ends the conversation
- You cannot have loops
- Each row should be of the same form, with the columns as follows:
    1) The interaction tree ID, empty for non-root nodes
    2) The speaker in the interaction
    3) The actual speech text
    4) Action code, happens after the text and before the choices
    5) Show choices flag (True to show choices, False to auto-continue)
  >=6) ID numbers for next dialog options
 */