using System.Numerics;
using Newtonsoft.Json.Linq;
using Raylib_cs;
using WhgVedit.Common;
using WhgVedit.Objects;
using WhgVedit.Objects.Animation;
using WhgVedit.Types;

namespace WhgVedit.Json;

using Newtonsoft.Json;

public class ObjectParser
{
	private readonly string _path;
	private readonly List<GameObject> _gameObjects = [];
	private readonly List<Animation> _animations = [];
	private bool _isParsed;

	public ObjectParser(string path)
	{
		_path = path;
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
		if (!File.Exists(_path)) return;
		_isParsed = true;

		string data = File.ReadAllText(_path);

		var jsonDictionary = JsonConvert.DeserializeObject<Dictionary<string, JArray>>(data);
		
		if (jsonDictionary is null || !jsonDictionary.ContainsKey("objects")) return;

		var animationsJArray = jsonDictionary["animations"];
		foreach (var jToken in animationsJArray)
		{
			var jObject = (JObject)jToken;
			Animation animation = GetAnimationFromJObject(jObject);
			_animations.Add(animation);
		}
		
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
	public GameObject? GetObjectFromJObject(JObject jObject)
	{
		Dictionary<string, JToken> properties = new();
		foreach (var property in jObject.Properties())
		{
			properties[GetJPropertyKey(property)] = property.Value;
		}
		
		string? typeName = properties["type"].ToString();
		if (typeName is null) return null;
		
		//Type? type = Assembly.GetExecutingAssembly().GetTypes().FirstOrDefault(type => type.Name == typeName);
		
		//GameObject obj = (GameObject)Activator.CreateInstance(type)!;
		GameObject? obj;
		if (typeName == "Wall")
		{
			obj = new Wall();
			SetWallProperties((Wall)obj, ToStrings(properties));
		}
		else if (typeName == "Enemy")
		{
			obj = new Enemy();
			SetEnemyProperties((Enemy)obj, ToStrings(properties));
		}
		else if (typeName == "Checkpoint")
		{
			obj = new Checkpoint();
			SetCheckpointProperties((Checkpoint)obj, ToStrings(properties));
		}
		else if (typeName == "AnimationPlayer")
		{
			obj = new AnimationPlayer();
			SetAnimationPlayerProperties((AnimationPlayer)obj, ToStrings(properties));
		}
		else return null;

		if (properties.ContainsKey("children"))
		{
			AddChildren(obj, properties["children"]);
		}
		
		return obj;
	}

	private void AddChildren(GameObject gameObject, JToken children)
	{
		if (children is not JArray array) return;
		foreach (JToken jToken in array)
		{
			if (jToken is not JObject jObject) continue;
			AddChildJObject(gameObject, child: jObject);
		}
	}

	private void AddChildJObject(GameObject gameObject, JObject child)
	{
		GameObject? childObject = GetObjectFromJObject(child);
		if (childObject is null) return;

		childObject.SetParent(gameObject);
		_gameObjects.Add(childObject);
	}

	private void SetAnimationPlayerProperties(AnimationPlayer animPlayer, Dictionary<string, string> properties)
	{
		if (properties.ContainsKey("animationName"))
		{
			// Find animation with matching AnimationName in the animations array in the json.
			string animationName = properties["animationName"];
			Animation? animation = GetAnimationByName(animationName);
			if (animation is not null) animPlayer.SetAnimation(animation);
		}
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
			wall.ZIndex = (ZIndex)int.Parse(value);
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

	private Dictionary<string, string> ToStrings(Dictionary<string, JToken> dictionary)
	{
		Dictionary<string, string> newDictionary = new();
		foreach (var kvp in dictionary)
		{
			newDictionary.Add(kvp.Key, kvp.Value.ToString());
		}

		return newDictionary;
	}

	private Animation GetAnimationFromJObject(JObject jObject)
	{
		Animation animation = new();
		if (jObject.ContainsKey("name"))
		{
			animation.Name = jObject["name"]!.ToString();
		}

		if (!jObject.ContainsKey("keyframes")) return animation;
		
		var keyframesJToken = jObject["keyframes"];
		if (keyframesJToken is not JArray keyframesJArray) return animation;

		List<Keyframe> keyframesList = [];
		foreach (var keyframeToken in keyframesJArray)
		{
			if (keyframeToken is not JObject keyframeObject) continue;
			
			Keyframe keyframe = new();
			if (keyframeObject.ContainsKey("duration"))
			{
				keyframe.Duration = float.Parse(keyframeObject["duration"]!.ToString());
			}

			if (keyframeObject.ContainsKey("position"))
			{
				string value = keyframeObject["position"]!.ToString(); // [336, 336]
				int[] array = ParseToIntArray(value);
				keyframe.Position = new Vector2i(array[0], array[1]);
			}
			else if (keyframesList.Count >= 1)
				keyframe.Position = keyframesList.Last().Position;
			
			if (keyframeObject.ContainsKey("rotation"))
			{
				keyframe.Rotation = float.Parse(keyframeObject["rotation"]!.ToString());
			}
			else if (keyframesList.Count >= 1)
				keyframe.Rotation = keyframesList.Last().Rotation;
			
			if (keyframeObject.ContainsKey("scale"))
			{
				string value = keyframeObject["scale"]!.ToString(); // [336, 336]
				int[] array = ParseToIntArray(value);
				keyframe.Scale = new Vector2(array[0], array[1]);
			}
			else if (keyframesList.Count >= 1)
				keyframe.Scale = keyframesList.Last().Scale;

			if (keyframeObject.ContainsKey("easing"))
			{
				// TODO: Get easing by name automatically
				
				string value = keyframeObject["easing"]!.ToString();
				if (value == "sine-in-out") keyframe.EasingFunc = Easings.SineInOut;
				else if (value == "constant") keyframe.EasingFunc = Easings.Constant;
				else keyframe.EasingFunc = Easings.Linear;
			}
			else keyframe.EasingFunc = Easings.Linear;
			keyframesList.Add(keyframe);
		}

		animation.Keyframes = keyframesList;
		return animation;
	}
	private Animation? GetAnimationByName(string name)
	{
		return _animations.Find(anim => anim.Name == name);
	}
}