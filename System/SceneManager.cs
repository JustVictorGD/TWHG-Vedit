using WhgVedit.Objects;

namespace WhgVedit.System;

class SceneManager
{
	public static void ChangeScene(Scene scene)
	{

	}

	private static SceneManager instance = new();
	private readonly Dictionary<string, List<GameObject>> groups = [];

	public static List<GameObject> GetObjectsInGroup(string groupName)
	{
		return instance.groups.TryGetValue(groupName, out List<GameObject>? list) ? list : [];
	}

	public static void AddObjectToGroup(GameObject @object, string groupName)
	{
		if (!instance.groups.ContainsKey(groupName))
			instance.groups.Add(groupName, []);

		List<GameObject> list = instance.groups[groupName];

		if (!list.Contains(@object))
			list.Add(@object);
	}

	public static void RemoveObjectFromGroup(GameObject @object, string groupName)
	{
		if (!instance.groups.TryGetValue(groupName, out List<GameObject>? list))
			return;

		list.Remove(@object);
	}
}