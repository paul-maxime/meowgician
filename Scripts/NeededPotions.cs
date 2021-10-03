using Godot;
using System;

public class NeededPotions : Node2D
{
	private PackedScene smallPotion = ResourceLoader.Load<PackedScene>("res://Scenes/SmallPotion.tscn");
	private SmallPotion potion1;
	private SmallPotion potion2;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
		Vector2 cauldronSize = GetNode<Sprite>("/root/Root/Cauldron/Sprite").Texture.GetSize();
		potion1 = (SmallPotion) smallPotion.Instance();
		potion1.init(- (cauldronSize.x / 8) + 4, 0f, Godot.GD.Randi() % 4);
		potion2 = (SmallPotion) smallPotion.Instance();
		potion2.init(cauldronSize.x / 8 - 4, 0f, Godot.GD.Randi() % 4);
		AddChild(potion1);
		AddChild(potion2);
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	// public override void _Process(float delta)
	// {
		
	// }
}
