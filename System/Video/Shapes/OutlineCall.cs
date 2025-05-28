using Raylib_cs;
using WhgVedit.Types;

namespace WhgVedit.Video;

class OutlineCall : ShapeCall
{
    public Rect2i Body { get; set; }
    public Color Color { get; set; }

    public OutlineCall(int zIndex, Rect2i body, Color color)
    {
        ZIndex = zIndex;
        Body = body;
        Color = color;
    }

    public override void Execute()
    {
        VideoEngine.DrawOutline(Body, Color);
    }
}