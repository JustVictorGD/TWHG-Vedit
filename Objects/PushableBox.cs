namespace WhgVedit.Objects;

using Common;
using Types;

public class PushableBox : RectObject
{
	public bool IsActive { get; set; } = true;
	public Vector2i LastMovement { get; set; } = new();

	public PushableBox(int width, int height)
	{
		Size = new(width, height);
	}

	public PushableBox(Vector2i size)
	{
		Size = size;
	}

	public Vector2i SuggestSinglePush(Wall wall)
	{
		if (!IsActive) return Vector2i.Zero;

		int top_overlap = wall.Body.Start.Y - Body.End.Y;
		int left_overlap = wall.Body.Start.X - Body.End.X;
		int bottom_overlap = wall.Body.End.Y - Body.Position.Y;
		int right_overlap = wall.Body.End.X - Body.Position.X;

		if (left_overlap > 0 || right_overlap < 0 || top_overlap > 0 || bottom_overlap < 0)
			return new();

		Vector2i push = new(
			(int)Utils.GetClosest(0, left_overlap, right_overlap),
			(int)Utils.GetClosest(0, top_overlap, bottom_overlap)
		);

		if (Math.Abs(push.X) > Math.Abs(push.Y))
			push = new(0, push.Y);
		else
			push = new(push.X, 0);

		return push;
	}

	public Vector2i SuggestWallPushes(List<Wall> walls)
	{
		if (!IsActive) return Vector2i.Zero;

		Vector2i original_position = Body.Position;
		Rect2i tempBody = Body;

		foreach (Wall wall in walls)
			tempBody = new(
				tempBody.Position + SuggestSinglePush(wall),
				tempBody.Size
			);

		return tempBody.Position - original_position;
	}
}