using Godot;

public class SpeechBubble : Node2D
{
	[Export] public AudioStream PraiseSound;
	[Export] public AudioStream ComplainSound;

	private Vector2 originalPosition;
	private Tween tween = new Tween();
	private Sprite sprite;

	private AudioStreamPlayer audioPlayer = new AudioStreamPlayer();

	public override void _Ready()
	{
		base._Ready();

		sprite = GetNode<Sprite>("Sprite");
		originalPosition = this.Position;
	
		AddChild(tween);
		AddChild(audioPlayer);
	}

	public void Praise()
	{
		tween.InterpolateProperty(sprite, "modulate", new Color(1, 1, 1), new Color(0, 1, 0.3f), 0.3f);
		tween.InterpolateProperty(sprite, "modulate", new Color(0, 1, 0.3f), new Color(1, 1, 1), 0.3f, delay: 0.3f);
		
		tween.InterpolateProperty(this, "scale", Vector2.One, Vector2.One * 1.3f, 0.2f);
		tween.InterpolateProperty(this, "scale", Vector2.One * 1.3f, Vector2.One, 0.2f, delay: 0.2f);

		tween.Start();

		audioPlayer.Stream = PraiseSound;
		audioPlayer.Play();
	}

	public void Complain()
	{
		Vector2[] positions = {
			originalPosition,
			originalPosition + Vector2.Right * 2.0f,
			originalPosition + Vector2.Left * 2.0f,
			originalPosition + Vector2.Right * 3.0f,
			originalPosition + Vector2.Left * 3.0f,
			originalPosition + Vector2.Right * 4.0f,
			originalPosition + Vector2.Left * 4.0f,
			originalPosition + Vector2.Right * 3.0f,
			originalPosition + Vector2.Left * 3.0f,
			originalPosition + Vector2.Right * 2.0f,
			originalPosition + Vector2.Left * 2.0f,
			originalPosition,
		};

		tween.InterpolateProperty(sprite, "modulate", new Color(1, 1, 1),new Color(1, 0.3f, 0.3f) , positions.Length * 0.08f / 2);
		tween.InterpolateProperty(sprite, "modulate", new Color(1, 0.3f, 0.3f), new Color(1, 1, 1), positions.Length * 0.08f / 2, delay: positions.Length * 0.08f / 2);

		for (int i = 0; i < positions.Length - 1; i++)
		{
			Vector2 from = positions[i];
			Vector2 to = positions[i + 1];
			tween.InterpolateProperty(this, "position", from, to, 0.08f, delay: i * 0.08f);
		}

		tween.Start();

		audioPlayer.Stream = ComplainSound;
		audioPlayer.Play();
	}
}
