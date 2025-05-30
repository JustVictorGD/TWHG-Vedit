using WhgVedit.Engine;

namespace WhgVedit.Objects;

class GameObject
{
	public Scene? Scene { get; set; } = null;

	// Lower number = higher priority! Mimics Z ordering and aligns with list sorting.
	public int UpdatePriority { get; set; } = 0;

	// There may or may not be problems with synchronization.
	// Keep an eye on whether or not an object's groups and
	// the stored references in Scene.instance.groups match.
	public List<string> Groups { get; set; } = [];


	public virtual void Ready() { }
	public virtual void Update() { }
	public virtual void Draw() { }

	public void AddToGroup(string groupName)
	{
		throw new NotImplementedException();
	}

	public void RemoveFromGroup(string groupName)
	{
		throw new NotImplementedException();
	}
}