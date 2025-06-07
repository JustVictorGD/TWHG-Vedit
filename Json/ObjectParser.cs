using System.Reflection;
using Newtonsoft.Json.Linq;
using Raylib_cs;
using WhgVedit.Objects;
using WhgVedit.Types;

namespace WhgVedit.Json;

using Newtonsoft.Json;

public class ObjectParser
{
	public string Path { get; set; }
	private List<GameObject> _gameObjects = [];
	private bool _isParsed = false;

	public ObjectParser(string path)
	{
		Path = path;
	}

	public List<GameObject> GetObjects()
	{
		if (_isParsed) return _gameObjects;
		Parse();
		return _gameObjects;
	}

	public List<T> GetObjectsOfType<T>()
	{
		List<T> list = [];
		foreach (var gameObject in GetObjects())
		{
			if (gameObject is T typedObject)
				list.Add(typedObject);
		}

		return list;
	}
	
	public void Parse()
	{
		if (!File.Exists(Path)) return;
		_isParsed = true;

		string data = File.ReadAllText(Path);

		var jsonDictionary = JsonConvert.DeserializeObject<Dictionary<string, JArray>>(data);
		
		if (jsonDictionary is null || !jsonDictionary.ContainsKey("objects")) return;

		var objectsJArray = jsonDictionary["objects"];
		foreach (var jToken in objectsJArray)
		{
			var jObject = (JObject)jToken;
			GameObject? gameObject = GetObjectFromJObject(jObject);
			if (gameObject is not null) _gameObjects.Add(gameObject);
		}
	}

	// Path is in the form of "[0].rect" or "[0].type", so doing this is necessary
	private string GetJPropertyKey(JProperty property)
	{
		return property.Path.Split('.').Last();
	}
	private GameObject? GetObjectFromJObject(JObject jObject)
	{
		Dictionary<string, string> properties = new();
		foreach (var property in jObject.Properties())
		{
			properties[GetJPropertyKey(property)] = property.Value.ToString();
		}
		
		string? typeName = properties["type"];
		if (typeName is null) return null;
		
		//Type? type = Assembly.GetExecutingAssembly().GetTypes().FirstOrDefault(type => type.Name == typeName);
		
		//GameObject obj = (GameObject)Activator.CreateInstance(type)!;
		GameObject? obj;
		if (typeName == "Wall")
		{
			obj = new Wall();
			SetWallProperties((Wall)obj, properties);
		}
		else if (typeName == "Enemy")
		{
			obj = new Enemy();
			SetEnemyProperties((Enemy)obj, properties);
		}
		else if (typeName == "Checkpoint")
		{
			obj = new Checkpoint();
			SetCheckpointProperties((Checkpoint)obj, properties);
		}
		else obj = null;
		return obj;
	}

	private void SetCheckpointProperties(Checkpoint checkpoint, Dictionary<string, string> properties)
	{
		if (properties.ContainsKey("rect"))
		{
			string value = properties["rect"];
			var rectData = ParseRect(value);
			if (rectData is not null)
			{
				checkpoint.Position = rectData.Value.position;
				checkpoint.Size = rectData.Value.size;
			}
		}
	}

	private void SetEnemyProperties(Enemy enemy, Dictionary<string,string> properties)
	{
		if (properties.ContainsKey("position"))
		{
			string value = properties["position"];
			int[] array = ParseToIntArray(value); // [252, 252]
			if (array.Length == 2)
			{
				enemy.Position = new Subpixel2(array[0], array[1]);
			}
		}
	}

	private void SetWallProperties(Wall wall, Dictionary<string, string> properties)
	{
		if (properties.ContainsKey("rect"))
		{
			string value = properties["rect"]; // [525, 525, 246, 54]
			var rectData = ParseRect(value);

			if (rectData is not null)
			{
				wall.Position = rectData.Value.position;
				wall.Size = rectData.Value.size;
			}
		}
		if (properties.ContainsKey("zIndex"))
		{
			string value = properties["zIndex"];
			wall.ZIndex = int.Parse(value);
		}
		if (properties.ContainsKey("outlineColor"))
		{
			string value = properties["outlineColor"]; // [0, 102, 0]
			int[] array = ParseToIntArray(value);

			if (array.Length == 3)
			{
				wall.OutlineColor = new Color(array[0], array[1], array[2]);
			}
		}
		if (properties.ContainsKey("fillColor"))
		{
			string value = properties["fillColor"]; // [0, 255, 0]
			int[] array = ParseToIntArray(value);

			if (array.Length == 3)
			{
				wall.FillColor = new Color(array[0], array[1], array[2]);
			}
		}
	}

	private (Subpixel2 position, Vector2i size)? ParseRect(string text)
	{
		int[] array = ParseToIntArray(text);

		if (array.Length == 4)
		{
			var position = new Subpixel2(array[0], array[1]);
			var size = new Vector2i(array[2], array[3]);
			return (position, size);
		}

		return null;
	}
	private int[] ParseToIntArray(string text)
		=> text.Replace("\r\n", string.Empty).Trim('[', ']').Split(", ").Select(int.Parse).ToArray();
}