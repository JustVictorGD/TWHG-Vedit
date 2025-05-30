namespace WhgVedit.Engine;

using Objects;

class Scene(List<GameObject> objects)
{
	private readonly List<GameObject> gameObjects = objects;
	private readonly Dictionary<string, List<GameObject>> groups = [];

	public static Scene? Main { get; set; }

	// Per instance
	public bool Frozen { get; set; } = false;
	public bool Visible { get; set; } = true;

	public void AddObject(GameObject @object)
	{
		gameObjects.Add(@object);

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
		gameObjects.Remove(@object);
		@object.Groups = [];
		foreach (string item in @object.Groups)
		{
			groups[item].Remove(@object);
		}
	}

	public List<GameObject> GetObjectsInGroup(string groupName)
	{
		return groups.TryGetValue(groupName, out List<GameObject>? list) ? list : [];
	}

	public void AddObjectToGroup(GameObject @object, string groupName)
	{
		if (!groups.ContainsKey(groupName))
			groups.Add(groupName, []);

		List<GameObject> list = groups[groupName];

		if (!list.Contains(@object))
			list.Add(@object);
	}

	public void RemoveObjectFromGroup(GameObject @object, string groupName)
	{
		if (!groups.TryGetValue(groupName, out List<GameObject>? list))
			return;

		list.Remove(@object);
	}

	public void Ready()
	{
		foreach (GameObject gameObject in gameObjects)
			gameObject.Ready();
	}

	public void Update()
	{
		if (Frozen) return;

		foreach (GameObject gameObject in gameObjects)
			gameObject.Update();
	}

	public void Draw()
	{
		if (!Visible) return;

		foreach (GameObject gameObject in gameObjects)
			gameObject.Draw();
	}
}