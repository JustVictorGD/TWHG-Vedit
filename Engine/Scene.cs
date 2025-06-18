namespace WhgVedit.Engine;

using Objects;

public class Scene : GameObject
{
	// The TurnIdle signal is used for preventing crashes by queueing additions or
	// deletions of objects made inside Ready() or Update() functions of other objects.
	// Add async to any function and use the line "await Scene.TurnIdle;" to delay execution.
	private readonly TaskCompletionSource _idleTCS = new();
	public Task TurnIdle => _idleTCS.Task;

	private readonly Dictionary<string, List<GameObject>> groupLists = [];

	public static Scene? Main { get; set; }

	public override void AddChild(GameObject child)
	{
		base.AddChild(child);
		child.Scene = this;

		foreach (string groupName in @child.Groups)
		{
			groupLists[groupName].Add(@child);
			@child.AddToGroup(groupName);
		}
	}

	public void AddChildren(IEnumerable<GameObject> children, string groupName)
	{
		groupLists.Add(groupName, []);

		foreach (GameObject child in children)
		{
			child.AddToGroup(groupName);
			AddChild(child);
		}
	}

	public override void RemoveChild(GameObject child)
	{
		base.RemoveChild(child);
		child.Scene = null;

		foreach (string groupName in @child.Groups)
			groupLists[groupName].Remove(@child);
	}

	public List<GameObject> GetObjectsInGroup(string groupName)
	{
		return groupLists.TryGetValue(groupName, out List<GameObject>? list) ? list : [];
	}

	public void AddObjectsToGroups(IEnumerable<GameObject> objects, params string[] groupNames)
	{
		foreach (GameObject @object in objects)
		{
			foreach (string groupName in groupNames)
			{
				if (!groupLists.ContainsKey(groupName))
					groupLists.Add(groupName, []);

				List<GameObject> list = groupLists[groupName];

				if (!list.Contains(@object))
					list.Add(@object);

				if (!@object.Groups.Contains(groupName))
					@object.Groups.Add(groupName);
			}
		}
	}

	public void RemoveObjectFromGroup(GameObject @object, string groupName)
	{
		if (groupLists.TryGetValue(groupName, out List<GameObject>? list))
			list.Remove(@object);
	}

	public override void RecursiveReady()
	{
		base.RecursiveReady();
		TriggerIdle();
	}
	public override void RecursiveUpdate()
	{
		base.RecursiveUpdate();
		TriggerIdle();
	}
	public override void RecursiveDraw()
	{
		base.RecursiveDraw();
		TriggerIdle();
	}
	public override void RecursiveDrawUI()
	{
		base.RecursiveDrawUI();
		TriggerIdle();
	}
	private void TriggerIdle()
	{
		if (!_idleTCS.Task.IsCompleted)
			_idleTCS.SetResult();
	}
}