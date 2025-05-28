using WhgVedit.System;

namespace WhgVedit.Objects;

class GameObject
{
	// Lower number = higher priority! Mimics Z ordering and aligns with list sorting.
	public int UpdatePriority { get; set; } = 0;

	public virtual void Ready() { }
	public virtual void Update() { }
	public virtual void Draw() { }

	public void AddToGroup(string groupName)
	{
		SceneManager.AddObjectToGroup(this, groupName);
	}

	public void RemoveFromGroup(string groupName)
	{
		SceneManager.RemoveObjectFromGroup(this, groupName);
	}
}