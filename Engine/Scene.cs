namespace WhgVedit.Engine;

using Objects;

public class Scene(List<GameObject> objects)
{
	// The TurnIdle signal is used for preventing crashes by queueing additions or
	// deletions of objects made inside Ready() or Update() functions of other objects.
	// Add async to any function and use the line "await Scene.TurnIdle;" to delay execution.
	private readonly TaskCompletionSource _idleTCS = new();
	public Task TurnIdle => _idleTCS.Task;

	public readonly List<GameObject> GameObjects = objects;
	private readonly Dictionary<string, List<GameObject>> groups = [];

	public static Scene? Main { get; set; }

	// Per instance
	public bool Frozen { get; set; } = false;
	public bool Visible { get; set; } = true;

	public void AddObject(GameObject @object)
	{
		GameObjects.Add(@object);
		@object.Scene = this;

		foreach (string groupName in @object.Groups)
		{
			groups[groupName].Add(@object);
			@object.AddToGroup(groupName);
		}
	}

	public void AddObjects(List<GameObject> objects)
	{
		foreach (GameObject gameObject in objects)
		{
			AddObject(gameObject);
		}
	}

	public void RemoveObject(GameObject @object)
	{
		GameObjects.Remove(@object);

		foreach (string item in @object.Groups)
			groups[item].Remove(@object);
	}

	public List<GameObject> GetObjectsInGroup(string groupName)
	{
		return groups.TryGetValue(groupName, out List<GameObject>? list) ? list : [];
	}

	public void AddObjectsToGroups(IEnumerable<GameObject> objects, params string[] groupNames)
	{
		foreach (GameObject @object in objects)
		{
			if (@object.Scene != this)
				AddObject(@object);

			foreach (string groupName in groupNames)
			{
				if (!groups.ContainsKey(groupName))
					groups.Add(groupName, []);

				List<GameObject> list = groups[groupName];

				if (!list.Contains(@object))
					list.Add(@object);

				if (!@object.Groups.Contains(groupName))
					@object.Groups.Add(groupName);
			}
		}
	}

	public void RemoveObjectFromGroup(GameObject @object, string groupName)
	{
		if (!groups.TryGetValue(groupName, out List<GameObject>? list))
			return;

		list.Remove(@object);
	}

	public void Ready()
	{
		foreach (GameObject gameObject in GameObjects)
			gameObject.Ready();

		TriggerIdle();
	}

	public void Update()
	{
		if (Frozen) return;

		foreach (GameObject gameObject in GameObjects)
			gameObject.Update();

		TriggerIdle();
	}

	public void Draw()
	{
		if (!Visible) return;

		foreach (GameObject gameObject in GameObjects)
			if (gameObject is SpacialObject spacial) spacial.Draw();

		TriggerIdle();
	}

	public void DrawUI()
	{
		if (!Visible) return;

		foreach (GameObject gameObject in GameObjects)
			if (gameObject is SpacialObject spacial) spacial.DrawUI();

		TriggerIdle();
	}

	private void TriggerIdle()
	{
		if (!_idleTCS.Task.IsCompleted)
			_idleTCS.SetResult();
	}
}