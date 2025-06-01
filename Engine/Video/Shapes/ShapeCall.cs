namespace WhgVedit.Engine.Video.Shapes;

using Raylib_cs;

public abstract class ShapeCall
{
	public int ZIndex { get; set; }
	public Color Color { get; set; }
	public abstract void Execute();
}