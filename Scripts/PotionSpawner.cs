using Godot;

public class PotionSpawner : Node2D
{
	private PackedScene potion = ResourceLoader.Load<PackedScene>("res://Scenes/Potion.tscn");

	public override void _Ready()
	{
		for (int i = 0; i < 2; i++)
		{
			Potion potionInstance = potion.Instance<Potion>();
			potionInstance.AddToGroup("potions");
			potionInstance.AddToGroup("selectable");
			AddChild(potionInstance);
		}
	}
}
