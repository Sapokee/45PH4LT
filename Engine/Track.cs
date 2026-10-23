using System.Numerics;

public class Track
{
	public List<Wall> Walls { get; set; } = [];

	public void LoadDefaultTrack()
	{
		Walls.Clear();

		// top
		Walls.Add(new Wall(new Vector2(100, 100), new Vector2(1100, 100)));
		// right
		Walls.Add(new Wall(new Vector2(1100, 100), new Vector2(1100, 800)));
		// bottom
		Walls.Add(new Wall(new Vector2(1100, 800), new Vector2(100, 800)));
		// left
		Walls.Add(new Wall(new Vector2(100, 800), new Vector2(100, 100)));

		// inside top
		Walls.Add(new Wall(new Vector2(350, 300), new Vector2(850, 300)));
		// inside right
		Walls.Add(new Wall(new Vector2(850, 300), new Vector2(850, 600)));
		// inside bottom
		Walls.Add(new Wall(new Vector2(850, 600), new Vector2(350, 600)));
		// inside left
		Walls.Add(new Wall(new Vector2(350, 600), new Vector2(350, 300)));
	}

}

