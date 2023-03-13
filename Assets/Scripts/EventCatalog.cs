using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerPauseEvent
{
}

public class ModifyPauseEvent
{
    public readonly bool status;

    public ModifyPauseEvent(bool _status)
    {
        status = _status;
    }
}

public class ScrollSimulationSpeedEvent
{
    public readonly bool direction;

    public ScrollSimulationSpeedEvent(bool _direction)
    {
        direction = _direction;
    }
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

public class ToggleDemoEvent
{
}

public class AssignGameControlEvent
{
    public readonly GameProgressControl gameProgressControl;

    public AssignGameControlEvent(GameProgressControl _gameProgressControl)
    {
        gameProgressControl = _gameProgressControl;
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

public class SpawnCitizenEvent
{
    
}