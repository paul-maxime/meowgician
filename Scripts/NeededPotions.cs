using Godot;
using System;

public class NeededPotions : Node2D
{
	private PackedScene smallPotion = ResourceLoader.Load<PackedScene>("res://Scenes/SmallPotion.tscn");
	private PackedScene mathOperator = ResourceLoader.Load<PackedScene>("res://Scenes/MathOperator.tscn");

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
		uint numberOfPotions = 4;
		Vector2 cauldronSize = GetNode<Sprite>("/root/Root/Cauldron/Sprite").Texture.GetSize();
		float minX = - (cauldronSize.x / 4f) + 4;
		float maxX = cauldronSize.x / 4f - 4;

		for (int i = 0; i < numberOfPotions; i++) 
		{
			if (i != 0)
			{
				MathOperator plusSign = mathOperator.Instance<MathOperator>();
				plusSign.Position = new Vector2(minX + i * 7, 0);
				AddChild(plusSign);
			}

			SmallPotion potion = (SmallPotion) smallPotion.Instance();
			potion.init(minX + i * 7 + 3, 0f, Godot.GD.Randi() % 4);
			AddChild(potion);
		}
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	// public override void _Process(float delta)
	// {
		
	// }
}
