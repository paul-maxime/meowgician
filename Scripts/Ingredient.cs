using Godot;
using Godot.Collections;

public class Ingredient : Node2D
{
	public uint IngredientType { get; private set; }

	public void Init(Vector2 position, uint ingredient)
	{
		this.Position = position;
		this.GetNode<Sprite>("Sprite").RegionRect = new Rect2(ingredient * 16f, 0, 16f, 16f);
		IngredientType = ingredient;
	}
}