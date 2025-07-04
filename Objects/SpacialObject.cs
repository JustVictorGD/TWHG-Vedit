using System.Numerics;

namespace WhgVedit.Objects;

using WhgVedit.Types;

public class SpacialObject : GameObject
{
	public virtual ZIndex ZIndex { get; set; } = new();
	public bool IsVisible { get; set; } = true;
	public bool IsUI { get; set; }

	public Vector2X Position { get; set; } = new();
	public float Rotation { get; set; } = 0;
	public Vector2 Scale { get; set; } = Vector2.One;

	public Transform2D Transform => new(Position, Basis.FromRotationAndScale(Rotation, Scale));


	// There are dedicated functions for X and Y elements of
	// Position because editing them individually can otherwise
	// get annoyingly long due to structs being immutable.
	public void SetX(double value, bool fromSteps = false)
	{
		if (fromSteps) value *= Fixed.StepSize;

		Position = new(value, Position.Y);
	}

	public void SetY(double value, bool fromSteps = false)
	{
		if (fromSteps) value *= Fixed.StepSize;

		Position = new(Position.X, value);
	}

	public ZIndex GetGlobalZ()
	{
		ZIndex z = ZIndex;
		GameObject? parent = Parent;

		while (parent != null)
		{
			if (parent == this) throw new InvalidOperationException("An Object2D is its own ancestor. This will cause an infinite loop.");

			if (parent is SpacialObject spacial)
				z += spacial.ZIndex;
			parent = parent.Parent;
		}

		return z;
	}

	// Not fully finished! This doesn't take rotation, scale and skew into account.
	public Vector2X GetGlobalPosition()
	{
		Vector2X position = Position;
		GameObject? parent = Parent;

		while (parent != null)
		{
			if (parent == this) throw new InvalidOperationException("An Object2D is its own ancestor. This will cause an infinite loop.");

			if (parent is SpacialObject spacial)
				position += spacial.Position;
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

	public override void RecursiveDraw()
	{
		Draw();
		base.RecursiveDraw();
	}

	public override void RecursiveDrawUI()
	{
		DrawUI();
		base.RecursiveDrawUI();
	}
}