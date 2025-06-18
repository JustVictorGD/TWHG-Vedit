using Newtonsoft.Json.Linq;
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

	public List<GameObject> Children { get; private set; } = [];
	public GameObject? Parent { get; private set; }

	public virtual void Ready() { if (Children.Contains(this)) Console.WriteLine(Children); }
	public virtual void Update() { }

	public virtual void AddChild(GameObject child)
	{
		Children.Add(child);
		child.Parent = this;
		child.GetAdded();

		foreach (string groupName in child.Groups)
			Scene?.AddObjectsToGroups([child], groupName);
	}

	public virtual void RemoveChild(GameObject child)
	{
		Children.Remove(child);
		child.Parent = null;
	}

	// Currently, the only use for this is in CursorListener.cs. Used
	// for running code immediately after getting added as a child.
	public virtual void GetAdded() { }

	public void AddToGroup(string groupName)
	{
		if (!Groups.Contains(groupName)) Groups.Add(groupName);
	}

	public void RemoveFromGroup(string groupName)
	{
		Groups.Remove(groupName);
	}

	public virtual JObject ToJson()
	{
		return new JObject
		{
			["type"] = GetType().Name
		};
	}

	public virtual void RecursiveReady()
	{
		Ready();

		foreach (GameObject child in Children)
			child.RecursiveReady();
	}

	public virtual void RecursiveUpdate()
	{
		Update();

		foreach (GameObject child in Children)
			child.RecursiveUpdate();
	}

	// Draw() and DrawUI() calls are added in SpacialObject,
	// but non-spacial ones can still carry the signal down.
	public virtual void RecursiveDraw()
	{
		foreach (GameObject child in Children)
			child.RecursiveDraw();
	}

	public virtual void RecursiveDrawUI()
	{
		foreach (GameObject child in Children)
			child.RecursiveDrawUI();
	}
}