using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerPauseEvent
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

public class AssignGameControlEvent
{
    public readonly GameControl gameControl;

    public AssignGameControlEvent(GameControl _gameControl)
    {
        gameControl = _gameControl;
    }
}

public class AssignInitGrowthPositionEvent
{
    public readonly Vector2 initPos;

    public AssignInitGrowthPositionEvent(Vector2 _initPos)
    {
        initPos = _initPos;
    }
}