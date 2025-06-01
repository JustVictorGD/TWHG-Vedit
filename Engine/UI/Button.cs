namespace WhgVedit.Engine.UI;

using Objects;
using Raylib_cs;
using Types;
using WhgVedit.Common;

public class Button(int x, int y, int width, int height) : GameObject
{
	// Currently unused.
	public event Action? Pressed;
	public event Action? Released;
	public event Action? Confirmed; // When releasing on top of the button.
	
	public enum State { Up, Focused, Aborted, Down }

	// I can't name this property "State" because of a name clash with the enum.
	private State state;
	public State GetState => state;

	public Rect2i Body = new(x, y, width, height);
	public bool IsDown = false;
	public bool Focused => Collision.PointInRect(Raylib.GetMousePosition(), Body);

	public override void Update()
	{
		bool focused = Focused;

		if (IsDown && Raylib.IsMouseButtonReleased(MouseButton.Left))
		{
			IsDown = false;
			Released?.Invoke();

			if (focused) Confirmed?.Invoke();
		}

		else if (Raylib.IsMouseButtonPressed(MouseButton.Left) && focused)
		{
			IsDown = true;
			Pressed?.Invoke();
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

	
}