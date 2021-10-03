using Godot;
using System;
using System.Collections.Generic;

public class Earthquake : Node
{
	public bool IsShaking { get; private set; }

	public float Intensity { get; private set; }

	public float RemainingDuration { get; private set; }

	public float TimeUntilNextShake { get; private set; }
	public Vector2 Direction { get; private set; }

	private Camera2D camera;
	private int currentStep;
	private float rotationTarget;

	private List<(KinematicBody2D, Vector2)> movedKinematics = new List<(KinematicBody2D, Vector2)>();

	public override void _Ready()
	{
		Intensity = 0.5f;
		TimeUntilNextShake = 10.0f;

		camera = GetNode<Camera2D>("/root/Root/Camera2D");
	}

	public override void _Process(float delta)
	{
		if (!IsShaking)
		{
			PrepareNextShake(delta);
		}
		else
		{
			ProcessShake(delta);
		}
	}

	private void PrepareNextShake(float delta)
	{
		TimeUntilNextShake -= delta;
		if (TimeUntilNextShake <= 0)
		{
			TimeUntilNextShake = 10.0f;
			Intensity += 0.2f;
			StartShaking();
		}
	}

	private void ProcessShake(float delta)
	{
		RemainingDuration -= delta;

		if (currentStep == 0)
		{
			float remainingUp = Math.Max(0, RemainingDuration - 4.0f);
			camera.Rotation = Mathf.Lerp(0, rotationTarget, 1.0f - remainingUp);
			if (RemainingDuration <= 4.0f)
			{
				StartSlide();
				currentStep++;
			}
		}

		if (currentStep == 1 && RemainingDuration <= 1f)
		{
			currentStep++;
		}

		if (currentStep == 2)
		{
			camera.Rotation = Mathf.Lerp(rotationTarget, 0, 1.0f - RemainingDuration);
			if (RemainingDuration <= 0f)
			{
				IsShaking = false;
				EndMovement();
				camera.Rotation = 0;
			}
		}

		float intensity = currentStep == 0 ? ((1.0f - RemainingDuration + 4.0f) * 2) : (RemainingDuration * 2);
		camera.Offset = new Vector2((float)GD.RandRange(-intensity, intensity), 0);
	}

	public override void _PhysicsProcess(float delta)
	{
		base._PhysicsProcess(delta);

		foreach (var (kinematicBody, randomVector) in movedKinematics)
		{
			if (!Godot.Object.IsInstanceValid(kinematicBody)) continue;
			Vector2 movement;
			if (Direction == Vector2.Zero)
			{
				movement = randomVector * delta * 25 * Intensity;
			}
			else
			{
				movement = Direction * delta * 50 * Intensity;
			}
			kinematicBody.MoveAndCollide(movement);
		}
	}

	private void StartShaking()
	{
		IsShaking = true;
		RemainingDuration = 5.0f;
		currentStep = 0;
		
		float quakeType = GD.Randf();

		if (quakeType < 0.6f) // Quake everywhere
		{
			Direction = Vector2.Zero;
		}
		else if (quakeType < 0.8f) // Slide left
		{
			float angle = (float)GD.RandRange(-Math.PI / 4, Math.PI / 4);
			Direction = Vector2.Left.Rotated(angle);
		}
		else // Slide right
		{
			float angle = (float)GD.RandRange(-Math.PI / 4, Math.PI / 4);
			Direction = Vector2.Right.Rotated(angle);
		}

		if (Direction == Vector2.Zero)
		{
			rotationTarget = 0;
		}
		else if (Direction.x < 0)
		{
			rotationTarget = (float)Math.PI / 48 * Intensity;
		}
		else
		{
			rotationTarget = -(float)Math.PI / 48 * Intensity;
		}
	}

	private IEnumerable<PhysicsBody2D> GetShakableBodies()
	{
		var entities = GetTree().GetNodesInGroup("shakable");
		foreach (var entity in entities)
		{
			yield return (PhysicsBody2D)entity;
		}
	}

	private void StartSlide()
	{
		foreach (var entity in GetShakableBodies())
		{
			if (entity is RigidBody2D rigidBody)
			{
				rigidBody.Mode = RigidBody2D.ModeEnum.Character;
				Vector2 impulse;
				if (Direction == Vector2.Zero)
				{
					impulse = RandomVector() * 20 * Intensity;
				}
				else
				{
					impulse = Direction * 100 * Intensity;
				}
				rigidBody.ApplyCentralImpulse(impulse);
			}
			else if (entity is KinematicBody2D kinematicBody)
			{
				movedKinematics.Add((kinematicBody, RandomVector()));
			}
		}
	}

	private Vector2 RandomVector()
	{
		return Vector2.Up.Rotated((float)GD.RandRange(-Math.PI, Math.PI));
	}

	private void EndMovement()
	{
		foreach (var entity in GetShakableBodies())
		{
			if (entity is RigidBody2D rigidBody)
			{
				rigidBody.Mode = RigidBody2D.ModeEnum.Static;
			}
		}
		movedKinematics.Clear();
	}
}
