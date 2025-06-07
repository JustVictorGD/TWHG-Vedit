using WhgVedit.Engine;

namespace WhgVedit.Objects;

public class GameObject
{
	public Scene? Scene { get; set; } = null;

	// Lower number = higher priority! Mimics Z ordering and aligns with list sorting.
	public int UpdatePriority { get; set; } = 0;

	// There may or may not be problems with synchronization.
	// Keep an eye on whether or not an object's groups and
	// the stored references in Scene.instance.groups match.
	public List<string> Groups { get; set; } = [];

	public List<GameObject> Children { get; set; } = [];
	public GameObject? Parent { get; private set; }

	public virtual void Ready() { }
	public virtual void Update() { }

	public void SetParent(GameObject parentObject)
	{
		Parent = parentObject;
		parentObject.Children.Add(parentObject);
	}
	public void AddToScene(Scene scene)
	{
		scene.AddObject(this);
		this.Scene = scene;
	}

	public void RemoveFromScene()
	{
		if (Scene is null) return;
		Scene.RemoveObject(this);
		Scene = null;
	}
	
	public void AddToGroup(string groupName)
	{
		Groups.Add(groupName);
	}

	public void RemoveFromGroup(string groupName)
	{
		Groups.Remove(groupName);
	}
}