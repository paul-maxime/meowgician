using Godot;

public class Hero : KinematicBody2D
{
	[Export] public int speed = 200;

	private Godot.Collections.Array<Potion> potions = new Godot.Collections.Array<Potion> { };

	public Vector2 velocity = new Vector2();

	public override void _Ready()
	{
		GetNode<AnimationPlayer>("AnimationPlayer").Play("Idle");
	}

	private Node2D GetClosestItemSelectable()
	{
		Godot.Collections.Array items = new Godot.Collections.Array { };
		if (potions.Count < 4)
		{
			items = GetTree().GetNodesInGroup("potions");
			FireTable fireTable = GetNode<FireTable>("/root/Root/Furniture/FireTable");
			if (!fireTable.isWorking)
			{
				items.Add(fireTable);
			}
			MortarTable mortarTable = GetNode<MortarTable>("/root/Root/Furniture/MortarTable");
			if (!mortarTable.isWorking)
			{
				items.Add(mortarTable);
			}
		}
		if (potions.Count > 0)
		{
			items.Add(GetNode<Node2D>("/root/Root/Cauldron"));
		}

		if (items.Count == 0)
		{
			return null;
		}
		Node2D nearestItem = (Node2D)items[0];
		foreach (Node2D item in items)
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
			potion.Position = globalPosition;
			potion.isOnTable = false;
			potion.GetParent().RemoveChild(potion);
			potion.AddToGroup("Shakable");
			GetParent().AddChild(potion);
		}
		potions.Add(potion);
		potion.RemoveFromGroup("potions");
		potion.GetNode<Sprite>("Outline").Visible = false;
		potion.Name = "Potion";
	}

	private void DropPotion()
	{
		foreach (Potion potion in potions)
		{
			potion.QueueFree();
		}
		potions = new Godot.Collections.Array<Potion> { };
	}

	private void DropPotionOnGround()
	{
		var potion = potions[potions.Count - 1];
		potions.Remove(potion);
		potion.AddToGroup("potions");
		potion.GetNode<Sprite>("Outline").Visible = true;
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
				else if (closestItemSelectable is Cauldron)
				{
					DropPotion();
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
			potion.MoveAndSlide(potion.Position.DirectionTo(target.Position) * speed, infiniteInertia: false);
		}
	}
}
