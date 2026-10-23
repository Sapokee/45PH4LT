using System.Numerics;
using Raylib_cs;

public class Renderer
{
	public void Init(int width, int height, string title)
	{
		Raylib.InitWindow(width, height, title);
		Raylib.SetTargetFPS(60);
	}

	public void Close()
	{
		Raylib.CloseWindow();
	}

	public bool ShouldClose()
	{
		Raylib.WindowShouldClose();
		return Raylib.WindowShouldClose();
	}

	public void BeginFrame()
	{
		Raylib.BeginDrawing();
		Raylib.ClearBackground(Color.RayWhite);
	}

	public void EndFrame()
	{
		Raylib.EndDrawing();
	}

	public void DrawUI()
	{
		Raylib.DrawText("Controls: W/S (Throttle), A/D (Steering), R (Reset Car)", 20, 20, 20, Color.DarkGray);
		Raylib.DrawText("Track Editor: Left Click (Add Wall), Right Click (Cancel Chain)", 20, 50, 20, Color.DarkGray);
	}

	public void DrawTrack(Track track)
	{
		foreach (var wall in track.Walls)
		{
			Raylib.DrawLineV(wall.Start, wall.End, Color.Black);
		}
	}

	public void DrawDrawingPreview(Vector2 startPoint)
	{
		Raylib.DrawLineV(startPoint, Raylib.GetMousePosition(), Color.Green);
	}

	public void DrawCar(Car car)
	{
		// Draw car body
		Raylib.DrawCircleV(car.Position, 10.0f, Color.Red);

		// Draw heading direction indicator
		Vector2 headingVector = new Vector2(
			(float)Math.Cos(car.Heading) * 20.0f,
			(float)Math.Sin(car.Heading) * 20.0f
		);
		Raylib.DrawLineV(car.Position, car.Position + headingVector, Color.Blue);
	}

	// Input wrappers to keep input handling clean
	public Vector2 GetMousePosition() => Raylib.GetMousePosition();
	public bool IsMouseButtonPressed(MouseButton button) => Raylib.IsMouseButtonPressed(button);
}