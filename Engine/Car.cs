using System.Numerics;

public class Car
{
	// units: m, rad, m/s, m/s^2, rad/s
	// state variables
	public Vector2 Position { get; set; }
	public float Orientation { get; set; }
	public Vector2 Velocity { get; set; }   // m/s in X and Y directions respectively
	public float CurrentSteeringAngle { get; private set; }

	public float ForwardVelocity { get; private set; }
	public float LateralVelocity { get; private set; }

	// vehicle config
	private const float WheelBase = 2.5f;
	private const float MaxSteeringAngle = 0.6f;    // ~34 degrees
	private const float SteeringRate = 4.0f;
	private const float MaxAcceleration = 6.0f;
	private const float MaxDeceleration = 12.0f;
	private const float RollingResistance = 0.5f;
	private const float AerodynamicDrag = 0.02f;

	// tire and grip
	private const float MaxGrip = 6.5f;                 // m/s^2
	private const float CorneringStiffness = 15.0f;     

	public void UpdatePhysics(float throttleInput, float brakeInput, float steeringInput, float dt)
	{
		throttleInput = Math.Clamp(throttleInput, 0f, 1.0f);
		brakeInput = Math.Clamp(brakeInput, 0f, 1.0f);
		steeringInput = Math.Clamp(steeringInput, -1.0f, 1.0f);

		if (throttleInput > 0.0f)
		{
			ForwardVelocity += throttleInput * MaxAcceleration * dt;
		}
		else if (brakeInput > 0.0f)
		{
			ForwardVelocity -= brakeInput * MaxDeceleration * dt;
			if (ForwardVelocity < 0.0f) 
				ForwardVelocity = 0.0f;
		}
		else
		{
			float resistance = RollingResistance + (AerodynamicDrag * ForwardVelocity * ForwardVelocity);
			if (ForwardVelocity > 0.0f)
			{
				ForwardVelocity -= resistance * dt;
				if (ForwardVelocity < 0.0f) 
					ForwardVelocity = 0.0f;
			}
			else if (ForwardVelocity < 0.0f)
			{
				ForwardVelocity += resistance * dt;
				if (ForwardVelocity > 0.0f) 
					ForwardVelocity = 0.0f;
			}
		}

		float targetSteeringAngle = steeringInput * MaxSteeringAngle;
		float steeringDifference = targetSteeringAngle - CurrentSteeringAngle;
		float maxStep = SteeringRate * dt;
		CurrentSteeringAngle += Math.Clamp(steeringDifference, -maxStep, maxStep);

		float yawRate = 0.0f;
		if (Math.Abs(CurrentSteeringAngle) > 0.001f && Math.Abs(ForwardVelocity) > 0.1f)
		{
			float turningRadius = WheelBase / (float)Math.Tan(CurrentSteeringAngle);
			yawRate = ForwardVelocity / turningRadius;
			Orientation += yawRate * dt;
		}

		Position += Velocity * dt;
	}
}