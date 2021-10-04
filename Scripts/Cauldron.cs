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
	private Array<IngredientGenerator> ingredientGenerators = null;

	private CPUParticles2D bubbleParticles;
	private CPUParticles2D confettiParticles;
	private Sprite sprite;
	private Vector2 initialPosition;
	private Node2D speechBubble;

	private Timer confettiTimer = new Timer { WaitTime = 3.0f };

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
		if (copyPotions.Count == 0 && potions.Count == neededPotions.Count)
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
				WinGame();
			}
			else
			{
				GenerateNeededPotions();
				GetNode<SpeechBubble>("SpeechBubble").Praise();
			}
		}
		else
		{
			GetNode<SpeechBubble>("SpeechBubble").Complain();
		}
	}

	private void WinGame()
	{
		Instability = 0;
		confettiParticles.Emitting = true;
		bubbleParticles.Emitting = false;
		speechBubble.Visible = false;
		GetNode<AudioStreamPlayer2D>("AudioBubbles").Stop();
		GetNode<AudioStreamPlayer>("/root/Game/MusicPlayer").Stop();
		GetNode<AudioStreamPlayer>("ConfettiParticles/AudioStreamPlayer").Play();
		GetNode<AnimationPlayer>("AnimationPlayer").Play("FireOff");
		confettiTimer.Start();
	}

	private void ConfettiTimer_Timeout()
	{
		GetNode<EndScreen>("/root/Game/EndScreen").Win();
	}

	private void GenerateNeededPotions()
	{
		neededPotions = new Array<SmallPotion> { };
		if (numberOfPotions < 4)
		{
			if (numberOfPotions == 0)
			{
				ingredientGenerators[0].Activate();
			}
			if (numberOfPotions < 3)
			{
				ingredientGenerators[numberOfPotions + 1].Activate();
			}
			tables[numberOfPotions].GetNode<Sprite>("SimpleTable").Visible = false;
			tables[numberOfPotions].GetNode<ApparitionAnimation>("ApparitionAnimation").Play();
			tables[numberOfPotions].GetNode<Sprite>("Sprite").Visible = true;
			tables[numberOfPotions].AddToGroup("table");
			numberOfPotions++;
		}
		float bubbleWidth = speechBubble.GetNode<Sprite>("Sprite").GetRect().Size.x;
		float requiredSize = (numberOfPotions * 2 - 2) * 3.7f;
		float delta = -bubbleWidth / 2f + (bubbleWidth - requiredSize) / 2f;
		for (int i = 0; i < numberOfPotions; i++)
		{
			if (i > 0)
			{
				Node2D plusSign = mathOperator.Instance<Node2D>();
				plusSign.Position = new Vector2(delta, -2.0f);
				speechBubble.AddChild(plusSign);
				delta += 3.7f;
			}
			var potion = smallPotion.Instance<SmallPotion>();
			potion.Init(new Vector2(delta, -2.0f), tables[i].potionIndex);
			speechBubble.AddChild(potion);
			neededPotions.Add(potion);
			delta += 3.7f;
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
		bubbleParticles = GetNode<CPUParticles2D>("BubbleParticles");
		confettiParticles = GetNode<CPUParticles2D>("ConfettiParticles");
		ingredientGenerators = new Array<IngredientGenerator>{
			GetParent().GetNode<IngredientGenerator>("Furniture/Window"),
			GetParent().GetNode<IngredientGenerator>("Furniture/Plant"),
			GetParent().GetNode<IngredientGenerator>("Furniture/Aquarium"),
			GetParent().GetNode<IngredientGenerator>("Furniture/Plant3")
		};
		sprite = GetNode<Sprite>("Sprite");

		speechBubble = GetNode<Node2D>("SpeechBubble");

		GenerateNeededPotions();

		initialPosition = this.Position;

		Instability = 0;

		AddChild(confettiTimer);
		confettiTimer.Connect("timeout", this, "ConfettiTimer_Timeout");
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

		bubbleParticles.Lifetime = Mathf.Lerp(1.5f, 0.1f, instability);
		bubbleParticles.InitialVelocity = Mathf.Lerp(3.5f, 135f, Math.Max(0f, instability - 0.1f) * 10f / 9f);
		bubbleParticles.ScaleAmount = Mathf.Lerp(0.2f, 0.5f, instability);

		if (Math.Floor(previousLevel * 100f) != Math.Floor(instability * 100f))
		{
			sprite.Modulate = ModulatedColor;

			Color color = ParticlesColor;
			bubbleParticles.ColorRamp.SetColor(1, color);
			bubbleParticles.ColorRamp.SetColor(2, color);
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
