namespace WhgVedit.Common;

using WhgVedit.Objects;
using WhgVedit.Types;

// These functions are usable for basic "push out of wall" logic. When a Vector2i
// is "suggested," you should offset the player's position by that value.

static class Collision
{
	public static Vector2i SuggestSinglePush(Rect2i player, Wall wall)
	{
		int top_overlap = wall.Position.Y - player.End.Y;
		int left_overlap = wall.Position.X - player.End.X;
		int bottom_overlap = wall.End.Y - player.Position.Y;
		int right_overlap = wall.End.X - player.Position.X;

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

	public static Vector2i SuggestWallPushes(Rect2i player, List<Wall> walls)
	{
		Vector2i original_position = player.Position;

		foreach (Wall wall in walls)
			player = new(
				player.Position.Move(SuggestSinglePush(player, wall)),
				player.Size
			);

		return player.Position - original_position;
	}
}
