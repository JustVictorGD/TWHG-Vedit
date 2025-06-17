namespace WhgVedit.Engine.Input;

using Objects.UI;

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

			// Decreases 'TimesHeld' and sends 'Released' and 'Confirmed' signals.
			listener.ProcessButtonsReleasing();

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

		// Increases 'TimesHeld' and sends 'Pressed' signals.
		highestlistener.ProcessButtonsPressing();
	}
}