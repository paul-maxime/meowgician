using Godot;
using Godot.Collections;
using System;

public class Cauldron : StaticBody2D
{
	public float Instability { get => instability; set => SetInstability(value); }
	private float instability;

	private PackedScene smallPotion = ResourceLoader.Load<PackedScene>("res://Scenes/SmallPotion.tscn");
	private PackedScene mathOperator = ResourceLoader.Load<PackedScene>("res://Scenes/MathOperator.tscn");
	private Array<SmallPotion> neededPotions = new Array<SmallPotion> { };
	private int numberOfPotions = 0;
	private Array<Table> tables = null;
	private float minX;
	private float maxX;

	private CPUParticles2D particles;
	private Sprite sprite;
	private Vector2 initialPosition;

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
			foreach (Potion potion in potions)
			{
				potion.DropIntoCauldron();
			}
			foreach (SmallPotion potion in neededPotions)
			{
				potion.QueueFree();
			}
			GetParent().GetNode<Hero>("Hero").DropPotions();
			Instability = Instability - 0.5f;
			if (numberOfPotions == 4)
			{
				GetNode<EndScreen>("/root/Game/EndScreen").Win();
			}
			else
			{
				GenerateNeededPotions();
			}
		}
	}

	private void GenerateNeededPotions()
	{
		neededPotions = new Array<SmallPotion> { };
		if (numberOfPotions < 4)
		{
			tables[numberOfPotions].GetNode<Sprite>("SimpleTable").Visible = false;
			tables[numberOfPotions].GetNode<Sprite>("Sprite").Visible = true;
			tables[numberOfPotions].AddToGroup("table");
			numberOfPotions++;
		}
		for (int i = 0; i < numberOfPotions; i++)
		{
			if (i > 0)
			{
				MathOperator plusSign = mathOperator.Instance<MathOperator>();
				plusSign.Position = new Vector2(minX + i * 5.5f - 0.8f, -25f);
				AddChild(plusSign);
			}
			var potion = smallPotion.Instance<SmallPotion>();
			potion.init(new Vector2(minX + i * 5.5f + 2f, -26f), tables[i].potionIndex /*Godot.GD.Randi() % 4*/);
			AddChild(potion);
			neededPotions.Add(potion);
		}
	}


	public override void _Ready()
	{
		tables = new Array<Table>{
			GetParent().GetNode<Table>("Furniture/FireTable"),
			GetParent().GetNode<Table>("Furniture/MortarTable"),
			GetParent().GetNode<Table>("Furniture/BookTable"),
			GetParent().GetNode<Table>("Furniture/CoffeeTable")
		};
		particles = GetNode<CPUParticles2D>("CPUParticles2D");
		sprite = GetNode<Sprite>("Sprite");

		var speachBubbleSprite = GetNode<Sprite>("SpeachBubble/Sprite");
		Vector2 speachBubbleSize = speachBubbleSprite.Texture.GetSize();
		minX = -(speachBubbleSize.x / 4f) + 3;
		maxX = speachBubbleSize.x / 4f - 3;

		GenerateNeededPotions();

		initialPosition = this.Position;

		Instability = 0;
	}

	public override void _Process(float delta)
	{
		base._Process(delta);
		Instability += delta / 60f;

		if (instability > 0.5f)
		{
			Position = initialPosition + new Vector2((float)GD.RandRange(-(Instability - 0.5f) * 8, (Instability - 0.5f) * 8), 0);
		}
	}

	private void SetInstability(float value)
	{
		if (value < 0)
		{
			value = 0;
		}
		float previousLevel = instability;
		instability = value;

		particles.Lifetime = Mathf.Lerp(1.5f, 0.1f, instability);
		particles.InitialVelocity = Mathf.Lerp(3.5f, 135f, Math.Max(0f, instability - 0.1f) * 10f / 9f);
		particles.ScaleAmount = Mathf.Lerp(0.2f, 0.5f, instability);

		if (Math.Floor(previousLevel * 100f) != Math.Floor(instability * 100f))
		{
			sprite.Modulate = ModulatedColor;

			Color color = ParticlesColor;
			particles.ColorRamp.SetColor(1, color);
			particles.ColorRamp.SetColor(2, color);
		}

		if (instability >= 1.0f)
		{
			GetNode<EndScreen>("/root/Game/EndScreen").Lose();
		}
	}

	public Color ParticlesColor
	{
		get
		{
			float r = Mathf.Lerp(0x6d / 255f, 0xd0 / 255f, instability);
			float g = Mathf.Lerp(0xaa / 255f, 0x46 / 255f, instability);
			float b = Mathf.Lerp(0x2c / 255f, 0x48 / 255f, instability);
			return new Color(r, g, b);
		}
	}

	public Color ModulatedColor
	{
		get
		{
			float r = Mathf.Lerp(1f, 0xd0 / 255f, instability);
			float g = Mathf.Lerp(1f, 0x46 / 255f, instability);
			float b = Mathf.Lerp(1f, 0x48 / 255f, instability);
			return new Color(r, g, b);
		}
	}
}
