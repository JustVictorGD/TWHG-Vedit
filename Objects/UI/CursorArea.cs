namespace WhgVedit.Objects.UI;

// This is a component used to detect if the mouse clicks an area or hovers
// over it. Only one area can be hovered or clicked at once.

using Raylib_cs;

using Objects;
using Types;

public class CursorArea : RectObject
{
	// The C# equivalent of a signal from Godot.
	public event Action? Pressed;

	public string DebugName { get; set; } = "Unnamed";

	// You should manually read, but not set these values.
	// Setting them is a job for a manager script.
	public bool IsFocused { get; set; }
	public bool IsHeld { get; set; }
	public bool IsSelected { get; set; }

	public CursorArea(Rect2i localBody, bool isUI, string name = "")
	{
		Position = localBody.Position;
		Size = localBody.Size;
		IsUI = isUI;
		DebugName = name;

		// How you connect methods and events.
		Pressed += ReactToPress;
	}

	public override void Update()
	{
		// Currently, IsFocused is being set to true inside Game.cs.
		
		if (IsFocused && Raylib.IsMouseButtonPressed(MouseButton.Left))
			Pressed?.Invoke();

		// This part is important, it makes sure only one area is focused at a time.
		IsFocused = false;
	}

	private void ReactToPress()
	{
		Console.WriteLine($"Omg I have been pressed, my name is {DebugName}");
	}

	// Important: This uses asymmetrical edge handling.
	// The case "point = rect.Position" returns true.
	// The case "point = rect.End" returns false.
	// rect.Size.X * rect.Size.Y equals the amount of valid spots.
	public static bool IsCursorInRect(Rect2i rect, bool isUI)
	{
		Vector2i point = (Vector2i)Game.GetMousePosition(isUI);

		return point >= rect.Start && point < rect.End;
	}

	public bool IsUnderCursor() => IsCursorInRect(Body, IsUI);
}