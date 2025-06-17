namespace WhgVedit.Objects.UI;

using Engine.Video;
using Objects;
using Raylib_cs;

public class Button : RectObject, IPressable
{
	// The component responsible for detecting the cursor.
	public CursorListener Listener { get; set; }

	public enum State { Up, Focused, Aborted, Down }

	// I can't name this property "State" because of a name clash with the enum.
	private State state;
	public virtual State GetState() => state;


	public Button(int x, int y, int width, int height, bool isUI = true)
	{
		Position = new(x, y);
		Size = new(width, height);
		IsUI = isUI;

		Listener = new(new(width, height), isUI);
	}

	public override async void Ready()
	{
		Listener.SetParent(this);

		if (Scene == null) return;

		await Scene.TurnIdle;

		Scene.AddObjectsToGroups([Listener], "CursorListeners");
	}

	public override void Update()
	{
		int score = 0;

		if (Listener.IsFocused) score++;
		if (Listener.IsHeld) score += 2;

		state = score switch
		{
			0 => State.Up,
			1 => State.Focused,
			2 => State.Aborted,
			_ => State.Down,
		};
	}

	// Not recommended to override these 2. Override PrivateDraw() instead.
	public override void Draw()
	{
		if (!IsUI)
			PrivateDraw();
	}

	public override void DrawUI()
	{
		if (IsUI)
			PrivateDraw();
	}

	public virtual void PrivateDraw()
	{
		switch (GetState())
		{
			case State.Up:
				VideoEngine.QueueOutlinedRect(ZIndex, new(), Body, new(128, 128, 128), new(224, 224, 224));
				break;

			case State.Focused:
				VideoEngine.QueueOutlinedRect(ZIndex, new(), Body, new(96, 192, 255), new(192, 255, 255));
				break;

			case State.Aborted:
				VideoEngine.QueueOutlinedRect(ZIndex, new(), Body, new(255, 128, 128), new(255, 204, 204));
				break;

			case State.Down:
				VideoEngine.QueueOutlinedRect(ZIndex, new(), Body, new(0, 64, 192), new(160, 192, 255));
				
				break;
		}
	}

	public bool IsUnderCursor() => CursorListener.IsCursorInRect(Body, IsUI);

	public virtual void Press(MouseButton mouseButton) { }
	public virtual void Release(MouseButton mouseButton) { }
	public virtual void Confirm(MouseButton mouseButton) { }
}