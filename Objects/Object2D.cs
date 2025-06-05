using System.Numerics;

namespace WhgVedit.Objects;

using WhgVedit.Types;

public class Object2D : GameObject
{
	public int ZIndex { get; set; } = 0;
	public Subpixel2 Position { get; set; } = new();
	public float Rotation { get; set; } = 0;
	public Vector2 Scale { get; set; } = Vector2.One;
	
	
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
	
	public void GetGlobalPosition()
	{
		throw new NotImplementedException($"Global properties are not supported yet.");

	}

	public void GetGlobalRotation()
	{
		throw new NotImplementedException($"Global properties are not supported yet.");
	}

	public void GetGlobalScale()
	{
		throw new NotImplementedException($"Global properties are not supported yet.");

	}

	public virtual void Draw() { }
	public virtual void DrawUI() { }
}