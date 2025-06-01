namespace WhgVedit.Engine.UI;

// TODO: Automate cursor detection. Maybe.

// Buttons should absolutely have signals like "Pressed" and
// "Released," but I don't remember how to do that in C#.

using Types;

public class Button(int x, int y, int width, int height)
{
      public Rect2i Body = new(x, y, width, height);
}