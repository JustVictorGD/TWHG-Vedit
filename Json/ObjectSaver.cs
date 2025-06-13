using System.Text;
using WhgVedit.Objects;
using WhgVedit.Objects.Animation;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Raylib_cs;

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
	}

	private Dictionary<string, JArray> GetJsonObjects()
	{
		string data = File.ReadAllText(_path);
		
		var jsonDictionary = JsonConvert.DeserializeObject<Dictionary<string, JArray>>(data);
		if (jsonDictionary is null)
			throw new NullReferenceException(
				$"Error while reading the JSON's objects, DeserializeObject returned null. File path: {_path}");
		
		if (!jsonDictionary.ContainsKey("objects")) 
			throw new InvalidOperationException(
				$"Attempted to add objects to a JSON without an objects array. File path: {_path}");
		
		return jsonDictionary;
	}
	public string AddNewObjectsToJson(List<GameObject> newObjects)
	{
		if (newObjects.Count == 0) return File.ReadAllText(_path); //
		
		Dictionary<string, JArray> jsonDictionary = GetJsonObjects();

		JArray objects = jsonDictionary["objects"];

		foreach (GameObject gameObject in newObjects)
		{
			JObject objectData = new();

			string? typeName = GetTypeName(gameObject.GetType());
			if (typeName is null) continue;
			
			objectData.Add("type", typeName);

			if (gameObject is Wall wall)
			{
				objectData.Add("rect", new JArray { wall.Position.X.Rounded, wall.Position.Y.Rounded, wall.Size.X, wall.Size.Y });
				
				if (wall.ZIndex != 16)
				{
					objectData.Add("zIndex", wall.ZIndex);
				}

				if (AreColorsEqual(wall.OutlineColor, new(72, 72, 102)))
				{
					objectData.Add("outlineColor",
					new JArray { wall.OutlineColor.R, wall.OutlineColor.G, wall.OutlineColor.B });
				}

				if (AreColorsEqual(wall.FillColor, new(179, 179, 255)))
				{
					objectData.Add("fillColor", 
						new JArray { wall.FillColor.R, wall.FillColor.G, wall.FillColor.B });
				}
			}
			else if (gameObject is Enemy enemy)
			{
				objectData.Add("position", new JArray { enemy.Position.X.Rounded, enemy.Position.Y.Rounded });
			}
			else if (gameObject is Checkpoint cp)
			{
				objectData.Add("rect", new JArray { cp.Position.X.Rounded, cp.Position.Y.Rounded, cp.Size.X, cp.Size.Y });
			}
			else if (gameObject is AnimationPlayer ap)
			{
				objectData.Add("animationName", ap.Animation is null ? string.Empty : ap.Animation.Name);
			}
			
			objects.Add(objectData);
		}

		jsonDictionary["objects"] = objects;

		return JsonConvert.SerializeObject(jsonDictionary);
	}

	/*public string DeleteObjectsFromJson(List<GameObject> objectsToDelete)
	{
		Dictionary<string, JArray> jsonDictionary = GetJsonObjects();
		
		JArray objects = jsonDictionary["objects"];
		ObjectParser parser = new(_path);

		foreach (var VARIABLE in COLLECTION)
		{
			
		}
		parser.GetObjectFromJObject()
	}*/

	public void Save(string data)
	{
		File.WriteAllText(_path, data);
	}

	private static string? GetTypeName(Type type)
	{
		return type.Name;
	}

	private static bool AreColorsEqual(Color color1, Color color2)
	{
		return color1.R == color2.R && color1.G == color2.G && color1.B == color2.B && color1.A == color2.A;
	}

	private static string WriteIntArray(params int[] ints)
	{
		StringBuilder sb = new();
		sb.Append('[');
		sb.Append(string.Join(", ", ints));
		sb.Append(']');

		return sb.ToString();
	}
}