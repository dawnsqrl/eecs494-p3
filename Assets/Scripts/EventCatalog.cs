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

public class GameStartEvent
{
}

public class GameEndEvent
{
    public readonly bool status; // builder = true, enemy = false

    public GameEndEvent(bool _status)
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
    public readonly Vector2 size;
    public readonly Dictionary<string, UnityAction> buttons;

    public DisplayDialogEvent(string _title, string _content, Vector2 _size,
        Dictionary<string, UnityAction> _buttons)
    {
        title = _title;
        content = _content;
        size = _size;
        buttons = _buttons;
    }
}

public class DismissDialogEvent
{
}

public class DialogBlockingEvent
{
    public readonly bool status;

    public DialogBlockingEvent(bool _status)
    {
        status = _status;
    }
}

public class DisplayPopupEvent
{
    public readonly bool isWorldPosition;
    public readonly Vector2 position;
    public readonly float offset;
    public readonly Color color;
    public readonly Dictionary<string, UnityAction> buttons;

    public DisplayPopupEvent(bool _isWorldPosition, Vector2 _position, float _offset,
        Color _color, Dictionary<string, UnityAction> _buttons)
    {
        isWorldPosition = _isWorldPosition;
        position = _position;
        offset = _offset;
        color = _color;
        buttons = _buttons;
    }
}

public class DisplayBannerEvent
{
    public readonly Transform parent;
    public readonly float offset;
    public readonly Color color;
    public readonly string text;

    public DisplayBannerEvent(Transform _parent, float _offset, Color _color, string _text)
    {
        parent = _parent;
        offset = _offset;
        color = _color;
        text = _text;
    }
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

public class SpawnEnemyEvent
{
}

public class ModifyVitalityEvent
{
    public readonly int vitality;

    public ModifyVitalityEvent(int _vitality)
    {
        vitality = _vitality;
    }
}