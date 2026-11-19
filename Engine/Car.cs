using System.Numerics;

public class Car
{
	// units: m, rad, m/s, m/s^2, rad/s, kg, N

	public const float GravitationalPull = 9.81f;

	// state variables
	public Vector2 Position { get; set; }
	public float Orientation { get; set; }
	public float ForwardVelocity { get; set; }
	public float LateralVelocity { get; set; }
	public float AngularVelocity { get; set; }
	public float CurrentSteeringAngle { get; set; }
	public bool IsDrifting { get; set; } = false;
	public string DebugData { get; set; } = "";


	// vehicle config
	private const float CenterOfMassHeight = 0.5f;
	private const float DistanceCOMToFrontAxle = 1.25f;
	private const float DistanceCOMToRearAxle = 1.25f;
	private const float Width = 1f;
	private const float Length = 2.5f;
	private const float MaxSteeringAngle = 1.5f;    // ~34 degrees
	private const float SteeringRate = 4.0f;
	private const float MaxAcceleration = 6.0f;
	private const float MaxDeceleration = 12.0f;
	private const float RollingResistance = 0.5f;
	private const float AerodynamicDrag = 0.02f;
	private const float Mass = 0.5f;	
	private const float MomentOfInertia = 1500f;

	// tire and grip
	private const float BaseMaxFrontGrip = 600f;
	private const float BaseMaxRearGrip = 500f;

	public void UpdatePhysics(float throttleInput, float brakeInput, float steeringInput, float dt)
	{

		// POSITION

		throttleInput = Math.Clamp(throttleInput, 0f, 1.0f);
		brakeInput = Math.Clamp(brakeInput, 0f, 1.0f);

		float acceleration = throttleInput * MaxAcceleration;

		if (brakeInput > 0f)
		{
			acceleration -= Math.Sign(ForwardVelocity) * brakeInput * MaxDeceleration;
		}
		else
		{
			acceleration -= Math.Sign(ForwardVelocity) * RollingResistance;
		}

		float drag = AerodynamicDrag * ForwardVelocity * Math.Abs(ForwardVelocity);
		acceleration -= drag;

		float longitudinalAcceleration = acceleration;

		ForwardVelocity += acceleration * dt;

		if (throttleInput == 0f && brakeInput == 0f && Math.Abs(ForwardVelocity) < 0.05f)
			ForwardVelocity = 0f;




		// ORIENTATION

		steeringInput = Math.Clamp(steeringInput, -1.0f, 1.0f);

		float targetSteeringAngle = steeringInput * MaxSteeringAngle;

		float steeringDifference = targetSteeringAngle - CurrentSteeringAngle;

		float maxSteeringChange = SteeringRate * dt;

		CurrentSteeringAngle += Math.Clamp(steeringDifference, -maxSteeringChange, maxSteeringChange);

		float wheelBase = DistanceCOMToFrontAxle + DistanceCOMToRearAxle;

		// WEIGHT TRANSFER
		
		float staticFrontLoad = Mass * GravitationalPull * (DistanceCOMToRearAxle / wheelBase);
		float staticRearLoad = Mass * GravitationalPull * (DistanceCOMToFrontAxle / wheelBase);

		float weightTransfer = (longitudinalAcceleration * Mass * CenterOfMassHeight) / wheelBase;

		float frontLoad = Math.Clamp(staticFrontLoad - weightTransfer, 500f, float.MaxValue);
		float rearLoad = Math.Clamp(staticRearLoad + weightTransfer, 500f, float.MaxValue);

		float maxFrontGrip = BaseMaxFrontGrip * (frontLoad / staticFrontLoad);
		float maxRearGrip = BaseMaxRearGrip * (rearLoad / staticRearLoad);

		// AXLE VELOCITIES

		float frontSlipVelocity = LateralVelocity + (AngularVelocity * DistanceCOMToFrontAxle) - (ForwardVelocity * CurrentSteeringAngle);
		float rearSlipVelocity = LateralVelocity - (AngularVelocity * DistanceCOMToRearAxle);

		float frontMassShare = Mass * (DistanceCOMToRearAxle / wheelBase);
		float rearMassShare = Mass * (DistanceCOMToFrontAxle / wheelBase);

		float frontForceDemand = -frontSlipVelocity * frontMassShare / dt;
		float rearForceDemand = -rearSlipVelocity * rearMassShare / dt;

		float frontLateralForce = Math.Clamp(frontForceDemand, -maxFrontGrip, maxFrontGrip);
		float rearLateralForce = Math.Clamp(rearForceDemand, -maxRearGrip, maxRearGrip);

		float torque = (frontLateralForce * DistanceCOMToFrontAxle) - (rearLateralForce * DistanceCOMToRearAxle);
		float angularAcceleration = torque / MomentOfInertia;
		AngularVelocity += angularAcceleration * dt;

		float totalLateralForce = frontLateralForce + rearLateralForce;
		float lateralAcceleration = totalLateralForce / Mass;
		LateralVelocity += lateralAcceleration * dt;

		LateralVelocity *= 0.98f;

		Vector2 forwardDirection = new Vector2(MathF.Cos(Orientation), MathF.Sin(Orientation));
		Vector2 rightDirection = new Vector2(-MathF.Sin(Orientation), MathF.Cos(Orientation));

		Vector2 totalVelocity = (forwardDirection * ForwardVelocity) + (rightDirection * LateralVelocity);

		Orientation += AngularVelocity * dt;

		Position += totalVelocity * dt;

		DebugData = @"
			Position: " + Position.ToString("n1") + @"
			Orientation: " + Orientation.ToString("n1") + @"
			LateralVelocity: " + LateralVelocity.ToString("n1") + @"
			ForwardVelocity: " + ForwardVelocity.ToString("n1") + @"
			AngularVelocity: " + AngularVelocity.ToString("n1");
	}
}