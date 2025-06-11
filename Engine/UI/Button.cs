namespace WhgVedit.Engine.UI;

using Raylib_cs;

using Engine.Video;
using Objects;
using Types;

public class Button : Object2D
{
	// Currently unused.
	public event Action? Pressed;
	public event Action? Released;
	public event Action? Confirmed; // When releasing on top of the button.

	public enum State { Up, Focused, Aborted, Down }

	// I can't name this property "State" because of a name clash with the enum.
	private State state;
	public State GetState => state;

	public Vector2i Size { get; set; }

	public Rect2i Body => new((Vector2i)Position, Size);

	public bool IsDown = false;
	public bool Focused => IsCursorInRect(Body, IsUI);

	public Button(int x, int y, int width, int height, bool isUI = true)
	{
		Position = new(x, y);
		Size = new(width, height);
		IsUI = isUI;
	}

	public override void Update()
	{
		bool focused = Focused;

		if (Raylib.IsMouseButtonPressed(MouseButton.Left) && focused)
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

			if (focused) { Confirmed?.Invoke(); Confirm(); }
		}

		int score = 0;

		if (focused) score++;
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
		switch (GetState)
		{
			case State.Up:
				VideoEngine.DrawOutlinedRect(Body, new(192, 192, 192), new(255, 255, 255, 128));
				break;

			case State.Focused:
				VideoEngine.DrawOutlinedRect(Body, new(144, 144, 192), new(192, 192, 255, 128));
				break;

			case State.Aborted:
				VideoEngine.DrawOutlinedRect(Body, new(192, 144, 144), new(255, 192, 192, 128));
				break;

			case State.Down:
				VideoEngine.DrawOutlinedRect(Body, new(192, 0, 192), new(255, 0, 255));
				break;
		}
	}

	// Important: This uses asymmetrical edge handling.
	// The case "point = rect.Position" returns true.
	// The case "point = rect.End" returns false.
	// rect.Size.X * rect.Size.Y equals the amount of valid spots.
	public static bool IsCursorInRect(Rect2i rect, bool isUI)
	{
		Vector2i point = (Vector2i)(isUI ?
			Game.GetMouseUIPosition() :
			Game.GetMouseWorldPosition());

		return point >= rect.Start && point < rect.End;
	}

	public virtual void Press() { }
	public virtual void Release() { }
	public virtual void Confirm() { }
}