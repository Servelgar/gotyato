using Godot;
using System;

public partial class character : CharacterBody3D
{
	
	[Export] float mouseSensitivity = 0.01f;
	[Export] public const float Speed = 5.0f;
	[Export] public const float JumpVelocity = 4.5f;
	[Export] float maxAngleView = 90f;
	[Export] float minAngleView = -90f;
	float  cameraAngle = 0f;
	float headRelativeAngle = 0f;
	
	public Camera3D camera;
	public Node3D head;
	public Control dialogue;

	[Export] bool canMove = true;
	
	public override void _Ready(){
		GetNodes();
		Input.MouseMode = Input.MouseModeEnum.Captured;
	}
	
	public override void _Input(InputEvent @event){
		if(canMove){
			if (@event is InputEventMouseMotion){
			InputEventMouseMotion mouseMotion = @event as InputEventMouseMotion;
			head.RotateY(-mouseMotion.Relative.X * mouseSensitivity);
			camera.RotateX(-mouseMotion.Relative.Y * mouseSensitivity);
			
			Vector3 cameraRot = camera.Rotation;
			cameraRot.X = Mathf.Clamp(cameraRot.X, Mathf.DegToRad(-80f), Mathf.DegToRad(80f));
			camera.Rotation = cameraRot;
			}
		}
	}

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();

	public override void _PhysicsProcess(double delta)
	{
		if(canMove){
			Vector3 velocity = Velocity;

			// Add the gravity.
			if (!IsOnFloor())
				velocity.Y -= gravity * (float)delta;

			// Handle Jump.
			if (Input.IsActionJustPressed("ui_accept") && IsOnFloor())
				velocity.Y = JumpVelocity;

			// Get the input direction and handle the movement/deceleration.
			// As good practice, you should replace UI actions with custom gameplay actions.
			Vector2 inputDir = Input.GetVector("move_left", "move_right", "move_foward", "move_back");
			Vector3 direction = (head.GlobalTransform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();
			if (direction != Vector3.Zero)
			{
				
				
				//direction = direction.Normalized();
				//GetNode<Node3D>("Head").LookAt(Position + direction, Vector3.Up);
				
				velocity.X = direction.X * Speed;
				velocity.Z = direction.Z * Speed;
			}
			else
			{
				velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
				velocity.Z = Mathf.MoveToward(Velocity.Z, 0, Speed);
			}

			Velocity = velocity;
			MoveAndSlide();
		}
	}
	
	private void _on_area_3d_area_entered(Area3D area)
	{
		Console.WriteLine("llegamos aqui");
		dialogue.Visible = true;
		
		
	//	GetTree().ChangeSceneToFile("res://nodos//dialogue_system.tscn");
	}
	
	void GetNodes(){
		head = GetNode<Node3D>("Head");
		camera = GetNode<Camera3D>("Head/Camera3D");
		dialogue = GetNode<Control>("../Okhalam/CanvasLayer/DialogueSystem");
	}

	private void _on_dialogue_system_visibility_changed()
	{
		if(dialogue.Visible) {
			Input.MouseMode = Input.MouseModeEnum.Visible;
			canMove = false;
		} else {
			Input.MouseMode = Input.MouseModeEnum.Captured;
			canMove = true;
		}
	}

}



