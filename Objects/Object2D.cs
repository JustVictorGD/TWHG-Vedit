namespace WhgVedit.Objects;

using WhgVedit.Types;

public class Object2D : GameObject
{
	public int ZIndex { get; set; } = 0;
	public Subpixel2 Position { get; set; } = new();

	// There are dedicated functions for X and Y elements of
	// Position because editing them individually can otherwise
	// get annoyingly long due to structs being immutable.
	public void SetX(double value, bool fromSteps = false)
	{
		if (fromSteps) value *= Subpixel.StepSize;

		Position = new(value, Position.Y);
	}

	public void SetY(double value, bool fromSteps = false)
	{
		if (fromSteps) value *= Subpixel.StepSize;

		Position = new(Position.X, value);
	}

	public virtual void Draw() { }
	public virtual void DrawUI() { }
}