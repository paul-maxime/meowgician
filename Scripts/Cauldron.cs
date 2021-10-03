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
	private uint numberOfPotions = 4;
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
			GetParent().GetNode<Hero>("Hero").DropPotions();
			Instability = Instability - 0.5f;
			GenerateNeededPotions();
		}
	}

	private void GenerateNeededPotions()
	{
		neededPotions = new Array<SmallPotion> { };
		for (int i = 0; i < numberOfPotions; i++)
		{
			SmallPotion potion = (SmallPotion)smallPotion.Instance();
			potion.init(new Vector2(minX + i * 7 + 3, 1f), Godot.GD.Randi() % 4);
			AddChild(potion);
			neededPotions.Add(potion);
		}
	}


	public override void _Ready()
	{
		particles = GetNode<CPUParticles2D>("CPUParticles2D");
		sprite = GetNode<Sprite>("Sprite");

		Vector2 cauldronSize = GetNode<Sprite>("Sprite").Texture.GetSize();
		minX = -(cauldronSize.x / 4f) + 4;
		maxX = cauldronSize.x / 4f - 4;

		for (int i = 1; i < numberOfPotions; i++)
		{
			MathOperator plusSign = mathOperator.Instance<MathOperator>();
			plusSign.Position = new Vector2(minX + i * 7 - 0.5f, 2f);
			AddChild(plusSign);
		}
		GenerateNeededPotions();

		initialPosition = this.Position;
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

		particles.Lifetime = Mathf.Lerp(1.5f, 0.2f, instability);
		particles.InitialVelocity = Mathf.Lerp(3.5f, 135f, Math.Max(0f, instability - 0.1f) * 10f / 9f);
		particles.ScaleAmount = Mathf.Lerp(0.2f, 0.5f, instability);

		if (Math.Floor(previousLevel * 100f) != Math.Floor(instability * 100f))
		{
			sprite.Modulate = ModulatedColor;

			Color color = ParticlesColor;
			particles.ColorRamp.SetColor(1, color);
			particles.ColorRamp.SetColor(2, color);
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
