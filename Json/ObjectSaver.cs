using System.Text;
using WhgVedit.Objects;
using WhgVedit.Objects.Animation;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Raylib_cs;
using WhgVedit.Common;

namespace WhgVedit.Json;

public class ObjectSaver
{
	private readonly string _path;
	private readonly List<GameObject> _gameObjects;
	private List<Animation> _animations;

	private string _data;

	public ObjectSaver(string path, List<GameObject> gameObjects, List<Animation> animations)
	{
		_path = path;
		_gameObjects = gameObjects;
		_animations = animations;
		_data = File.ReadAllText(_path);
	}

	private Dictionary<string, JArray> GetJsonObjects()
	{
		var jsonDictionary = JsonConvert.DeserializeObject<Dictionary<string, JArray>>(_data);
		if (jsonDictionary is null)
			throw new NullReferenceException(
				$"Error while reading the JSON's objects, DeserializeObject returned null. File path: {_path}");

		if (!jsonDictionary.ContainsKey("objects"))
			throw new InvalidOperationException(
				$"Attempted to add objects to a JSON without an objects array. File path: {_path}");

		return jsonDictionary;
	}

	public void AddNewObjectsToJson(List<GameObject> newObjects)
	{
		if (newObjects.Count == 0) return;

		Dictionary<string, JArray> jsonDictionary = GetJsonObjects();

		JArray objects = jsonDictionary["objects"];

		foreach (GameObject gameObject in newObjects)
		{

			string? typeName = GetTypeName(gameObject.GetType());
			if (typeName is null) continue;

			JObject objectData = new();
			objectData.Add("type", typeName);

			objectData = gameObject.ToJson();

			objects.Add(objectData);
		}

		jsonDictionary["objects"] = objects;

		_data = JsonConvert.SerializeObject(jsonDictionary);
	}

	public void DeleteObjectsFromJson(List<GameObject> objectsToRemove)
	{
		if (objectsToRemove.Count == 0) return;
		
		Dictionary<string, JArray> jsonDictionary = GetJsonObjects();

		JArray objects = jsonDictionary["objects"];
		List<JObject> markedForRemoval = [];
		
		foreach (JToken jToken in objects)
		{
			if (jToken is not JObject jObject) continue;
			foreach (GameObject gameObject in objectsToRemove)
			{
				JObject jsonData = gameObject.ToJson();
				if (JToken.DeepEquals(jsonData, jObject)) markedForRemoval.Add(jObject);
			}
		}

		foreach (JObject jObject in markedForRemoval)
		{
			objects.Remove(jObject);
		}

		jsonDictionary["objects"] = objects;
		_data = JsonConvert.SerializeObject(jsonDictionary);
	}

	public void Save()
	{
		File.WriteAllText(_path, _data);
	}

	private static string? GetTypeName(Type type)
	{
		return type.Name;
	}
}