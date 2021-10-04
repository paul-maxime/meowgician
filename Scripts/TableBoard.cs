using Godot;
using System;

public class TableBoard : Sprite
{
	private PackedScene smallPotion = ResourceLoader.Load<PackedScene>("res://Scenes/SmallPotion.tscn");
	private PackedScene smallIngredient = ResourceLoader.Load<PackedScene>("res://Scenes/SmallIngredient.tscn");
	private PackedScene mathOperator = ResourceLoader.Load<PackedScene>("res://Scenes/MathOperator.tscn");

	[Export] public uint IngredientType1;
	[Export] public uint IngredientType2;
	[Export] public uint OutputPotionType;

	public override void _Ready()
	{
		Sprite ingredient1 = smallIngredient.Instance<Sprite>();
		ingredient1.Position = new Vector2(-8.0f, -3.0f);
		ingredient1.RegionRect = new Rect2(IngredientType1 * 16, 0, 16, 16);
		AddChild(ingredient1);

		Sprite plusOperator = mathOperator.Instance<Sprite>();
		plusOperator.Position = new Vector2(-4.0f, -3.0f);
		plusOperator.RegionRect = new Rect2(16, 0, 16, 16);
		AddChild(plusOperator);

		Sprite ingredient2 = smallIngredient.Instance<Sprite>();
		ingredient2.Position = new Vector2(0.0f, -3.0f);
		ingredient2.RegionRect = new Rect2(IngredientType2 * 16, 0, 16, 16);
		AddChild(ingredient2);

		Sprite eqOperator = mathOperator.Instance<Sprite>();
		eqOperator.Position = new Vector2(4.0f, -3.0f);
		eqOperator.RegionRect = new Rect2(0, 0, 16, 16);
		AddChild(eqOperator);

		SmallPotion potion = smallPotion.Instance<SmallPotion>();
		potion.Init(new Vector2(8.0f, -3.0f), OutputPotionType);
		AddChild(potion);
	}
}
