namespace WhgVedit.Objects.UI;

// This is a component used to detect if the mouse clicks an area or hovers
// over it. It ensures only one area can be hovered or clicked at a time.

using Raylib_cs;

using Objects;
using Types;

public class CursorListener : RectObject
{
	public const string GroupName = "CursorListeners";

	public static readonly List<MouseButton> RecognizedButtons = [
		MouseButton.Left,
		MouseButton.Middle,
		MouseButton.Right
	];

	// The C# equivalent of a signal from Godot.
	public event Action<MouseButton>? Pressed;
	public event Action<MouseButton>? Released;
	public event Action<MouseButton>? Confirmed;

	public string DebugName { get; set; } = "Unnamed";

	// You should manually read, but not set these values.
	// Setting them is a job for a manager script.

	// The reason for IsHeld not just being a toggle is
	// because both left and right buttons can be held.
	public bool IsFocused { get; set; }
	public int TimesHeld { get; set; } = 0;
	public bool IsHeld => TimesHeld > 0;

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

	public void Press(MouseButton mouseButton = MouseButton.Left) => Pressed?.Invoke(mouseButton);
	public void Release(MouseButton mouseButton = MouseButton.Left) => Released?.Invoke(mouseButton);
	public void Confirm(MouseButton mouseButton = MouseButton.Left) => Confirmed?.Invoke(mouseButton);

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

	public void ProcessButtonsReleasing()
	{
		if (!IsHeld) return;

		foreach (MouseButton mouseButton in RecognizedButtons)
		{
			if (!Raylib.IsMouseButtonReleased(mouseButton)) continue;

			TimesHeld--;
			Release(mouseButton);
			if (IsUnderCursor()) Confirm(mouseButton);
		}
	}

	public void ProcessButtonsPressing()
	{
		foreach (MouseButton mouseButton in RecognizedButtons)
		{
			if (!Raylib.IsMouseButtonPressed(mouseButton)) continue;

			TimesHeld++;
			Press(mouseButton);
		}
	}

	public override void SetParent(GameObject parent)
	{
		base.SetParent(parent);

		if (parent is IPressable pressable)
		{
			Pressed += pressable.Press;
			Released += pressable.Release;
			Confirmed += pressable.Confirm;
		}
	}
}