using System.Numerics;

public class Car
{
	public Vector2 Position { get; set; }
	public float Orientation { get; set; } // radians
	public float Speed { get; set; } // meters per second

	private const float WheelBase = 2.5f; // meters
	private const float MaxSteeringAngle = 0.5f; // radians (~28 degrees)
	private const float MaxAcceleration = 5.0f; // m/s^2

	public void UpdatePhysics(float accelerationInput, float steeringInput, float dt)
	{
		accelerationInput = Math.Clamp(accelerationInput, -1.0f, 1.0f);
		steeringInput = Math.Clamp(steeringInput, -1.0f, 1.0f);

		Speed += (accelerationInput * MaxAcceleration) * dt;

		Speed *= 0.98f;

		float actualSteeringAngle = steeringInput * MaxSteeringAngle;

		if (Math.Abs(actualSteeringAngle) > 0.001f)
		{
			float turningRadius = WheelBase / (float)Math.Tan(actualSteeringAngle);
			float angularVelocity = Speed / turningRadius;
			Orientation += angularVelocity * dt;
		}

		Position += new Vector2(
			(float)Math.Cos(Orientation) * Speed * dt,
			(float)Math.Sin(Orientation) * Speed * dt
		);
	}
}