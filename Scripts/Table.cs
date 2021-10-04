using Godot;

public abstract class Table : KinematicBody2D
{
	private PackedScene potion = ResourceLoader.Load<PackedScene>("res://Scenes/Potion.tscn");

	public bool isWorking = false;

	public uint potionIndex;

	private Ingredient ingredient = null;

	private TableBoard board;

	public override void _Ready()
	{
		base._Ready();
		board = GetNode<TableBoard>("Board");
	}

	public virtual void Interact()
	{
		var hero = GetNode<Hero>("/root/Game/Hero");
		uint heroIngredient = hero.ingredient.IngredientType;
		uint currentIngredient = ingredient?.IngredientType ?? 0xFF;
		if ((heroIngredient != board.IngredientType1 && heroIngredient != board.IngredientType2) || heroIngredient == currentIngredient)
		{
			board.Complain();
			return;
		}

		if (ingredient == null)
		{
			ingredient = hero.ingredient;
			ingredient.Position = Vector2.Zero;
			hero.ingredient = null;
			hero.RemoveChild(ingredient);
			AddChild(ingredient);
		}
		else
		{
			hero.ingredient.QueueFree();
			ingredient.QueueFree();
			hero.ingredient = null;
			ingredient = null;
			GetNode<AnimationPlayer>("AnimationPlayer").Play("Working");
			GetNode<Timer>("Timer").Start();
			GetNodeOrNull<AudioStreamPlayer2D>("AudioPlayer")?.Play();
			isWorking = true;
		}
	}

	public void _on_Timer_timeout()
	{
		isWorking = false;
		var potionInstance = potion.Instance<Potion>();
		potionInstance.init(new Vector2(0, 0), potionIndex);
		potionInstance.AddToGroup("potions");
		potionInstance.AddToGroup("selectable");
		AddChild(potionInstance);
		GetNode<AnimationPlayer>("AnimationPlayer").Play("Idle");
		GetNodeOrNull<AudioStreamPlayer2D>("AudioPlayer")?.Stop();
	}
}