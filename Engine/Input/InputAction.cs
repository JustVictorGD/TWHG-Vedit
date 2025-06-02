using Raylib_cs;

namespace WhgVedit.Engine;

public class InputAction
{

	
	public string Name { get; set; }
	public List<KeyboardKey> Keys { get; set; }
	public bool IsActive { get; private set; }

	public InputAction(string name, List<KeyboardKey> keys)
	{
		Name = name;
		Keys = keys;
	}
	
	public void CheckInputs()
	{
		if (Keys.Any(key => Raylib.IsKeyDown(key)))
		{
			IsActive = true;
			return;
		}

		IsActive = false;
	}
}