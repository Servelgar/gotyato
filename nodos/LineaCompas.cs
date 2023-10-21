using Godot;
using System;

public partial class LineaCompas : Line2D
{
	private Line2D line;
	private Vector2 initialPosition;
	private Vector2 targetPosition;
	private float moveSpeed = 50.0f;
	
	public override void _Ready()
	{
		line = GetNode<Line2D>("LineaCompas");
		initialPosition = new Vector2(0,0);
		targetPosition = new Vector2(100,100);
		DrawLine();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		/*initialPosition = initialPosition.LinearInterpolate(targetPosition, moveSpeed * delta);
		DrawLine(); */
	}
	
	private void DrawLine(){
		/*line.Points = new Vector2[]{initialPosition, targetPosition};
		line.Update();*/
	}
}
