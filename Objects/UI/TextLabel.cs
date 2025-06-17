using System.Numerics;
using Newtonsoft.Json.Linq;
using Raylib_cs;
using WhgVedit.Engine.Video;
using WhgVedit.Engine.Video.Shapes;

namespace WhgVedit.Objects.UI;

public class TextLabel : SpacialObject
{
	public Vector2 Position { get; set; }
	public string Text { get; set; }
	public int FontSize { get; set; }
	public string FontFilePath { get; set; }

	private Font _font;

	public TextLabel(Vector2 position, string text, int fontSize, string fontFilePath = "Assets/universcondensed_medium.ttf")
	{
		Position = position;
		Text = text;
		FontSize = fontSize;
		FontFilePath = fontFilePath;
		
		if (!File.Exists(FontFilePath))
			throw new InvalidOperationException($"Font file with path {FontFilePath} does not exist.");
		//_font = Raylib.LoadFont(FontFilePath);
		_font = Raylib.LoadFontEx(FontFilePath, FontSize, null, 0);
	}

	public override void DrawUI()
	{
		TextCall textCall = new TextCall(Text, Position, FontSize, _font, Color.Gray);
		VideoEngine.QueueDraw(textCall);
	}
}