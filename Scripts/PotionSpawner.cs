using Godot;

public class PotionSpawner : Node2D
{
	private PackedScene potion = ResourceLoader.Load<PackedScene>("res://Scenes/Potion.tscn");

	public override void _Ready()
	{
		for (int i = 0; i < 20; i++)
		{
			var potionInstance = potion.Instance();
			potionInstance.AddToGroup("selectable");
			AddChild(potionInstance);
		}
	}
}
