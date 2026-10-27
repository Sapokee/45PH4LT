using System.Numerics;
using Raylib_cs;

public class Game
{
	private Renderer _renderer;
	private Track _track;
	private Car _car;
	private Vector2? _drawingStartPoint = null;

	public void Run()
	{
		_renderer = new Renderer();
		_renderer.Init(1200, 900, "Autonomous Car RL - Modular Sandbox");

		_track = new Track();
		_track.LoadDefaultTrack();
		_car = new Car
		{
			Position = new Vector2(5, 5),
			Orientation = 0.0f,
			Velocity = new Vector2(0, 0)
		};

		const float dt = 0.01666f; // 60Hz physics timestep

		while (!_renderer.ShouldClose())
		{
			ProcessInput();

			_renderer.BeginFrame();

			_renderer.DrawUI();
			_renderer.DrawTrack(_track);

			if (_drawingStartPoint.HasValue)
			{
				_renderer.DrawDrawingPreview(_drawingStartPoint.Value);
			}

			_renderer.DrawCar(_car);

			_renderer.EndFrame();
		}

		_renderer.Close();
	}

	private float _throttleInput = 0.0f;
	private float _steeringInput = 0.0f;
	private float _brakingInput = 0.0f;

	private void ProcessInput()
	{
		_throttleInput = 0.0f;
		_steeringInput = 0.0f;
		_brakingInput = 0.0f;

		if (Raylib.IsKeyDown(KeyboardKey.W))
			_throttleInput = 1.0f;
		if (Raylib.IsKeyDown(KeyboardKey.S))
			_brakingInput = 1.0f;
		if (Raylib.IsKeyDown(KeyboardKey.A))
			_steeringInput = -1.0f;
		if (Raylib.IsKeyDown(KeyboardKey.D))
			_steeringInput = 1.0f;

		if (Raylib.IsKeyPressed(KeyboardKey.R))
		{
			_car.Position = new Vector2(5, 5);
			_car.Orientation = 0.0f;
			_car.Velocity = new Vector2(0, 0);
		}

		// track editor
		if (Renderer.IsMouseButtonPressed(MouseButton.Left))
		{
			Vector2 mousePos = Renderer.GetMousePosition();
			if (_drawingStartPoint == null)
			{
				_drawingStartPoint = mousePos;
			}
			else
			{
				_track.Walls.Add(new Wall(_drawingStartPoint.Value, mousePos));
				_drawingStartPoint = mousePos;
			}
		}
		if (Renderer.IsMouseButtonPressed(MouseButton.Right))
		{
			_drawingStartPoint = null;
		}

		// apply physics tick
		_car.UpdatePhysics(_throttleInput, _brakingInput, _steeringInput, 0.01666f);
	}
}