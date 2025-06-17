namespace WhgVedit.Objects.UI;

using System.Numerics;

using Types;

public class Slider(int x, int y, int width, int height, bool isUI = true) : Button(x, y, width, height, isUI)
{
	private Vector2I mousePosOffset = new();
	public bool CanMoveX { get; set; } = true;
	public bool CanMoveY { get; set; } = true;

	public override void Press()
	{
		mousePosOffset = (Vector2I)Position - (Vector2I)Game.GetMousePosition(IsUI);
	}

	public override void Update()
	{
		Vector2 mousePosition = Game.GetMousePosition(IsUI);

		base.Update();

		if (IsDown)
		{
			if (CanMoveX) SetX(mousePosition.X + mousePosOffset.X);
			if (CanMoveY) SetY(mousePosition.Y + mousePosOffset.Y);
		}
	}

	public override State GetState()
	{
		State value = base.GetState();
		if (value == State.Aborted) value = State.Down;
		return value;
	}
}