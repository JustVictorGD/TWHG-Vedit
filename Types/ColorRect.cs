using Raylib_cs;

namespace WhgVedit.Types;

// This class is used for JSON to Runtime conversion.

public class ColorRect
{
	public int[] Body { get; set; }
	public int[] Color { get; set; }

	public ColorRect()
	{
		Body = [0, 0, 0, 0];
		Color = [0, 0, 0, 0];
	}

	public override string ToString()
	{
		return $"{{ Body: [{Body[0]}, {Body[1]}, {Body[2]}, {Body[3]}], Color: [{Color[0]}, {Color[1]}, {Color[2]}, {Color[3]}] }}";
	}

	public Rect2I GetRect() => new(Body[0], Body[1], Body[2], Body[3]);
	public Color GetColor() => new(Color[0], Color[1], Color[2], Color[3]);
}