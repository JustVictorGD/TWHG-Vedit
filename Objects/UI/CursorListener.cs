namespace WhgVedit.Objects.UI;

// This is a component used to detect if the mouse clicks an area or hovers
// over it. It ensures only one area can be hovered or clicked at a time.

using Objects;
using Types;

public class CursorListener : RectObject
{
	public const string GroupName = "CursorListeners";

	// The C# equivalent of a signal from Godot.
	public event Action? Pressed;
	public event Action? Released;
	public event Action? Confirmed;

	public string DebugName { get; set; } = "Unnamed";

	// You should manually read, but not set these values.
	// Setting them is a job for a manager script.
	public bool IsFocused { get; set; }
	public bool IsHeld { get; set; }

	// Not currently used.
	public bool IsSelected { get; set; }

	public CursorListener(Vector2I size, bool isUI, string name = "")
	{
		Size = size;
		IsUI = isUI;
		DebugName = name;
	}

	public override void Ready()
	{
		base.Ready();
	}

	public void Press() => Pressed?.Invoke();
	public void Release() => Released?.Invoke();
	public void Confirm() => Confirmed?.Invoke();

	// Important: This uses asymmetrical edge handling.
	// The case "point = rect.Position" returns true.
	// The case "point = rect.End" returns false.
	// rect.Size.X * rect.Size.Y equals the amount of valid spots.
	public static bool IsCursorInRect(Rect2I rect, bool isUI)
	{
		Vector2I point = (Vector2I)Game.GetMousePosition(isUI);

		return point >= rect.Start && point < rect.End;
	}

	public bool IsUnderCursor() => IsCursorInRect(Body, IsUI);
}