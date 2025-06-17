namespace WhgVedit.Objects.UI;

// Use for any object that has CursorListener as a component. This
// interface results in actions being connected to methods automatically.

using Raylib_cs;

public interface IPressable
{
	public void Press(MouseButton mouseButton);
	public void Release(MouseButton mouseButton);
	public void Confirm(MouseButton mouseButton);
}