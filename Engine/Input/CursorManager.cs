using Raylib_cs;
using WhgVedit.Objects.UI;

namespace WhgVedit.Engine.Input;

public static class CursorManager
{
	public static void Update()
	{
		if (Scene.Main == null) return;

		List<CursorListener> listeners = [.. Scene.Main.GetObjectsInGroup("CursorListeners").Cast<CursorListener>()];
		List<CursorListener> focusedListeners = [];

		foreach (CursorListener listener in listeners)
		{
			listener.IsFocused = false;

			if (Raylib.IsMouseButtonReleased(MouseButton.Left) && listener.IsHeld)
			{
				listener.IsHeld = false;
				listener.Release();
				if (listener.IsUnderCursor()) listener.Confirm();
			}

			if (listener.IsUnderCursor())
				focusedListeners.Add(listener);
		}

		if (focusedListeners.Count == 0) return;

		// Ordering.
		focusedListeners = [.. focusedListeners
			.OrderBy(x => x.IsUI)
			.ThenBy(x => x.ZIndex)
		];

		CursorListener highestlistener = focusedListeners[^1];

		highestlistener.IsFocused = true;

		if (Raylib.IsMouseButtonPressed(MouseButton.Left))
		{
			highestlistener.IsHeld = true;
			highestlistener.Press();
		}
	}
}