using System.Numerics;
using Raylib_cs;
using WhgVedit.Common;

namespace WhgVedit.Engine.Video.Shapes;

public class TextCall : ShapeCall
{
	public Vector2 Position { get; set; }
	public string Text { get; set; }
	public int FontSize { get; set; }

	private Font _font;

	public TextCall(string text, Vector2 position, int fontSize, Font font, Color color)
	{
		Text = text;
		Position = position;
		FontSize = fontSize;
		_font = font;
		Color = color;
	}
	
	public override void Execute()
	{
		Raylib.DrawTextEx(_font, Text, Position, FontSize, spacing: 1f, Color);
		//Raylib.DrawText(Text, Utils.Round(Position.X), Utils.Round(Position.Y), FontSize, Color.Black);
	}
}