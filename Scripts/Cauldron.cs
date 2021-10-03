using Godot;
using Godot.Collections;

public class Cauldron : StaticBody2D
{
	private PackedScene smallPotion = ResourceLoader.Load<PackedScene>("res://Scenes/SmallPotion.tscn");
	private PackedScene mathOperator = ResourceLoader.Load<PackedScene>("res://Scenes/MathOperator.tscn");
	private Array<SmallPotion> neededPotions = new Array<SmallPotion> { };
	private uint numberOfPotions = 4;
	private float minX;
	private float maxX;

	public void TryToDrop(Array<Potion> potions)
	{
		var copyPotions = neededPotions.Duplicate();
		foreach (var potion in potions)
		{
			foreach (var neededPotion in copyPotions)
			{
				if (potion.color == neededPotion.color)
				{
					copyPotions.Remove(neededPotion);
					break;
				}
			}
		}
		if (copyPotions.Count == 0)
		{
			GetParent().GetNode<Hero>("Hero").DropPotions();
		}
	}

	private void GenerateNeededPotions()
	{
		neededPotions = new Array<SmallPotion> { };
		for (int i = 0; i < numberOfPotions; i++)
		{
			SmallPotion potion = (SmallPotion)smallPotion.Instance();
			potion.init(new Vector2(minX + i * 7 + 3, 0f), Godot.GD.Randi() % 2 == 0 ? (uint)1 : 3);
			AddChild(potion);
			neededPotions.Add(potion);
		}
	}

	public override void _Ready()
	{
		Vector2 cauldronSize = GetNode<Sprite>("Sprite").Texture.GetSize();
		minX = -(cauldronSize.x / 4f) + 4;
		maxX = cauldronSize.x / 4f - 4;

		for (int i = 1; i < numberOfPotions; i++)
		{
			MathOperator plusSign = mathOperator.Instance<MathOperator>();
			plusSign.Position = new Vector2(minX + i * 7, 0);
			AddChild(plusSign);
		}
		GenerateNeededPotions();
	}
}
