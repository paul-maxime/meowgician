using Godot;
using Godot.Collections;

public class Ingredient : Node2D
{
	public void Init(Vector2 position, uint ingredient)
	{
		this.Position = position;
		this.GetNode<Sprite>("Sprite").RegionRect = new Rect2(ingredient * 8f, 0, 8f, 8f);
	}
}