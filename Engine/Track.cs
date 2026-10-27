using System.Numerics;

public class Track
{
	public List<Wall> Walls { get; set; } = [];

	public void LoadDefaultTrack()
	{
		Walls.Clear();

		// top
		Walls.Add(new Wall(new Vector2(2.0f, 2.0f), new Vector2(14.0f, 2.0f)));
		// right
		Walls.Add(new Wall(new Vector2(14.0f, 2.0f), new Vector2(14.0f, 10.25f)));
		// bottom
		Walls.Add(new Wall(new Vector2(14.0f, 10.25f), new Vector2(2.0f, 10.25f)));
		// left
		Walls.Add(new Wall(new Vector2(2.0f, 10.25f), new Vector2(2.0f, 2.0f)));

		// inside top
		Walls.Add(new Wall(new Vector2(5.0f, 4.25f), new Vector2(11.0f, 4.25f)));
		// inside right
		Walls.Add(new Wall(new Vector2(11.0f, 4.25f), new Vector2(11.0f, 8.0f)));
		// inside bottom
		Walls.Add(new Wall(new Vector2(11.0f, 8.0f), new Vector2(5.0f, 8.0f)));
		// inside left
		Walls.Add(new Wall(new Vector2(5.0f, 8.0f), new Vector2(5.0f, 4.25f)));
	}

}

