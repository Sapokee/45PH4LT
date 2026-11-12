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
			Vector2 startPx = wall.Start * GameConfig.PixelsPerMeter;
			Vector2 endPx = wall.End * GameConfig.PixelsPerMeter;

			Raylib.DrawLineV(startPx, endPx, Color.Black);
		}
	}

	public void DrawDrawingPreview(Vector2 startPoint)
	{
		Raylib.DrawLineV(startPoint, Raylib.GetMousePosition(), Color.Green);
	}

	public void DrawCar(Car car)
	{
		Vector2 screenPosition = car.Position * GameConfig.PixelsPerMeter;

		Raylib.DrawRectanglePro(
			new Rectangle(screenPosition.X, screenPosition.Y, 2.5f * GameConfig.PixelsPerMeter, 1f * GameConfig.PixelsPerMeter),
			new Vector2(20.0f, 10.0f),
			car.Orientation * (180.0f / (float)Math.PI),
			(car.IsDrifting ? Color.Red : Color.Lime)
		);

		Vector2 orientationVector = new Vector2(
			(float)Math.Cos(car.Orientation) * 20.0f,
			(float)Math.Sin(car.Orientation) * 20.0f
		);
		Raylib.DrawLineV(screenPosition, screenPosition+ orientationVector, Color.Blue);
	}

	public static Vector2 GetMousePosition() => Raylib.GetMousePosition();
	public static bool IsMouseButtonPressed(MouseButton button) => Raylib.IsMouseButtonPressed(button);
}