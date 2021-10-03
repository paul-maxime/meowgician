using Godot;
using Godot.Collections;

public class Hero : KinematicBody2D
{
	[Export] public int speed = 200;

	private Array<Potion> potions = new Array<Potion> { };

	public Vector2 velocity = new Vector2();

	public override void _Ready()
	{
		GetNode<AnimationPlayer>("AnimationPlayer").Play("Idle");
	}

	private Node2D GetClosestItemSelectable()
	{
		Array selectableItems = new Array { };
		if (potions.Count < 4)
		{
			selectableItems = GetTree().GetNodesInGroup("potions");
		}
		if (potions.Count > 0)
		{
			selectableItems.Add(GetNode<Node2D>("/root/Root/Cauldron"));
		}
		foreach (Table table in GetTree().GetNodesInGroup("table"))
		{
			if (!table.isWorking)
			{
				selectableItems.Add(table);
			}
		}

		if (selectableItems.Count == 0)
		{
			return null;
		}
		Node2D nearestItem = (Node2D)selectableItems[0];
		foreach (Node2D item in selectableItems)
		{
			if (item.GlobalPosition.DistanceTo(this.GlobalPosition) < nearestItem.GlobalPosition.DistanceTo(this.GlobalPosition))
			{
				nearestItem = item;
			}
		}
		return nearestItem;
	}

	private void HideSelectablesOutline()
	{
		var items = GetTree().GetNodesInGroup("selectable");
		foreach (Node2D item in items)
		{
			item.GetNode<Sprite>("Outline").Visible = false;
		}
	}

	private void PickPotion(Potion potion)
	{
		if (potion.isOnTable)
		{
			var globalPosition = potion.GlobalPosition;
			potion.Position = Position;
			potion.isOnTable = false;
			potion.CollisionLayer = 1;
			potion.CollisionMask = 1;
			potion.GetParent().RemoveChild(potion);
			potion.AddToGroup("Shakable");
			GetParent().AddChild(potion);
		}
		potions.Add(potion);
		potion.RemoveFromGroup("potions");
		potion.GetNode<Sprite>("Outline").Visible = false;
		potion.GetNode<Node2D>("Sprite/Eyes").Visible = true;
		potion.Name = "Potion";
	}

	public void DropPotions()
	{
		foreach (Potion potion in potions)
		{
			potion.QueueFree();
		}
		potions = new Array<Potion> { };
	}

	private void DropPotionOnGround()
	{
		var potion = potions[potions.Count - 1];
		potions.Remove(potion);
		potion.AddToGroup("potions");
		potion.GetNode<Sprite>("Outline").Visible = true;
		potion.GetNode<Node2D>("Sprite/Eyes").Visible = false;
	}

	public override void _Input(InputEvent @event)
	{
		base._Input(@event);
		if (@event.IsActionPressed("ui_accept"))
		{
			var closestItemSelectable = GetClosestItemSelectable();
			if (closestItemSelectable != null && closestItemSelectable.GlobalPosition.DistanceTo(this.GlobalPosition) < 80)
			{
				HideSelectablesOutline();
				if (closestItemSelectable is Potion potion)
				{
					PickPotion(potion);
				}
				else if (closestItemSelectable is Cauldron cauldron)
				{
					cauldron.TryToDrop(potions);
				}
				else if (closestItemSelectable is Table table)
				{
					table.Interact();
				}
			}
			else if (potions.Count > 0)
			{
				DropPotionOnGround();
			}
		}
	}

	public void GetVelocity()
	{
		velocity = new Vector2();

		if (Input.IsActionPressed("ui_right"))
			velocity.x += 1;

		if (Input.IsActionPressed("ui_left"))
			velocity.x -= 1;

		if (Input.IsActionPressed("ui_down"))
			velocity.y += 1;

		if (Input.IsActionPressed("ui_up"))
			velocity.y -= 1;

		var animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		if (velocity != new Vector2())
		{
			HideSelectablesOutline();
			var closestItemSelectable = GetClosestItemSelectable();
			if (closestItemSelectable != null && closestItemSelectable.GlobalPosition.DistanceTo(this.GlobalPosition) < 80)
			{
				closestItemSelectable.GetNode<Sprite>("Outline").Visible = true;
			}
			if (velocity.x > 0)
			{
				animationPlayer.Play("WalkRight");
			}
			else if (velocity.x < 0)
			{
				animationPlayer.Play("WalkLeft");
			}
			else if (velocity.y > 0)
			{
				animationPlayer.Play("WalkRight");
			}
			else if (velocity.y < 0)
			{
				animationPlayer.Play("WalkLeft");
			}
		}
		else
		{
			if (animationPlayer.CurrentAnimation == "WalkRight")
			{
				animationPlayer.Play("IdleRight");
			}
			else if (animationPlayer.CurrentAnimation == "WalkLeft")
			{
				animationPlayer.Play("IdleLeft");
			}
			else if (animationPlayer.CurrentAnimation == "WalkLeft")
			{
				animationPlayer.Play("IdleLeft");
			}
			else if (animationPlayer.CurrentAnimation == "WalkRight")
			{
				animationPlayer.Play("IdleRight");
			}
		}

		velocity = velocity.Normalized() * speed;
	}

	public override void _PhysicsProcess(float delta)
	{
		GetVelocity();
		foreach (var potion in potions)
		{
			potion.CollisionLayer = 2;
			potion.CollisionMask = 2;
		}
		velocity = MoveAndSlide(velocity, infiniteInertia: false);
		foreach (var potion in potions)
		{
			potion.CollisionLayer = 1;
			potion.CollisionMask = 1;
		}
		for (int i = 0; i < potions.Count; i++)
		{
			var potion = potions[i];
			var target = i == 0 ? (Node2D)this : (Node2D)potions[i - 1];
			Vector2 direction = potion.Position.DirectionTo(target.Position);

			potion.MoveAndSlide(direction * speed, infiniteInertia: false);
			potion.UpdateEyes(direction, delta);
		}
	}
}
