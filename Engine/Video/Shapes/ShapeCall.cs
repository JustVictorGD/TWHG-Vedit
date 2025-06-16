namespace WhgVedit.Engine.Video.Shapes;

using Raylib_cs;
using WhgVedit.Types;

public abstract class ShapeCall
{
	public ZIndex ZIndex { get; set; }
	public Color Color { get; set; }
	public abstract void Execute();
}