namespace WhgVedit.Engine.UI;

using Raylib_cs;

using Engine.Video;
using Objects;

public class Button : RectObject
{
	// Currently unused.
	public event Action? Pressed;
	public event Action? Released;
	public event Action? Confirmed; // When releasing on top of the button.

	public enum State { Up, Focused, Aborted, Down }

	// I can't name this property "State" because of a name clash with the enum.
	private State state;
	public virtual State GetState() => state;

	public bool IsDown { get; set; } = false;
	public bool IsFocused { get; set; } = false;

	public Button(int x, int y, int width, int height, bool isUI = true)
	{
		Position = new(x, y);
		Size = new(width, height);
		IsUI = isUI;
	}

	public override void Update()
	{
		if (Raylib.IsMouseButtonPressed(MouseButton.Left) && IsFocused)
		{
			IsDown = true;
			Pressed?.Invoke();
			Press();
		}

		else if (IsDown && Raylib.IsMouseButtonReleased(MouseButton.Left))
		{
			IsDown = false;
			Released?.Invoke();
			Release();

			if (IsFocused) { Confirmed?.Invoke(); Confirm(); }
		}

		int score = 0;

		if (IsFocused) score++;
		if (IsDown) score += 2;

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
				VideoEngine.QueueOutlinedRect(ZIndex, 0, Body, new(128, 128, 128), new(224, 224, 224));
				break;

			case State.Focused:
				VideoEngine.QueueOutlinedRect(ZIndex, 0, Body, new(96, 192, 255), new(192, 255, 255));
				break;

			case State.Aborted:
				VideoEngine.QueueOutlinedRect(ZIndex, 0, Body, new(255, 128, 128), new(255, 204, 204));
				break;

			case State.Down:
				VideoEngine.QueueOutlinedRect(ZIndex, 0, Body, new(0, 64, 192), new(160, 192, 255));
				
				break;
		}
	}

	public bool IsUnderCursor() => CursorArea.IsCursorInRect(Body, IsUI);

	public virtual void Press() { }
	public virtual void Release() { }
	public virtual void Confirm() { }
}