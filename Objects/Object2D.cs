using System.Numerics;

namespace WhgVedit.Objects;

using WhgVedit.Types;

public class Object2D : GameObject
{
	public int ZIndex { get; set; } = 0;
	public Subpixel2 Position { get; set; } = new();
	public float Rotation { get; set; } = 0;
	public Vector2 Scale { get; set; } = Vector2.One;

	public Transform2D Transform => new(Position, Basis.FromRotationAndScale(Rotation, Scale));
	
	
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
	
	// Not fully finished! This doesn't take rotation, scale and skew into account.
	public Subpixel2 GetGlobalPosition()
	{
		Subpixel2 position = Position;
		GameObject? parent = Parent;

		while (parent != null)
		{
			if (parent is Object2D object2D)
				position += object2D.Position;
			parent = parent.Parent;
		}

		return position;
	}

	public void GetGlobalRotation()
	{
		throw new NotImplementedException($"Global properties only extend to adding positions.");
	}

	public void GetGlobalScale()
	{
		throw new NotImplementedException($"Global properties only extend to adding positions.");
	}

	public virtual void Draw() { }
	public virtual void DrawUI() { }
}