using Raylib_cs;

namespace WhgVedit.Engine;

public class InputAction
{

	
	public string Name { get; set; }
	public List<KeyboardKey> Keys { get; set; }
	//public bool IsActive { get; private set; }

	private bool _wasActive = false;

	public InputAction(string name, List<KeyboardKey> keys)
	{
		Name = name;
		Keys = keys;
	}
	
	public bool IsActive()
	{
		return Keys.Any(key => Raylib.IsKeyDown(key));
	}

	public bool IsJustActivated()
	{
		return IsActive() && !_wasActive;
	}

	public void Update()
	{
		_wasActive = IsActive();
	}
}