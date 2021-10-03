using Godot;

public class SmallPotion : StaticBody2D
{
	public uint color;

	public void init(Vector2 position, uint color)
	{
		this.color = color;
		this.Position = position;
		this.GetNode<Sprite>("Sprite").RegionRect = new Rect2(color * 16f, 0, 16f, 16f);
	}
}
