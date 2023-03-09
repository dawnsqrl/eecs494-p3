using System.Collections.Generic;
using UnityEngine.Events;

public class TogglePauseEvent
{
}

public class DisplayDialogEvent
{
    public readonly string title;
    public readonly string content;
    public readonly Dictionary<string, UnityAction> buttons;

    public DisplayDialogEvent(string _title, string _content, Dictionary<string, UnityAction> _buttons)
    {
        title = _title;
        content = _content;
        buttons = _buttons;
    }
}

public class DismissDialogEvent
{
}