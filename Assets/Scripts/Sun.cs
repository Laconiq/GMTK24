public class Sun : Celestial
{
    public float gravitationalConstant = 10f;
    public float gravitationalCorrectionForce = 1f;
    public float orbitalVelocityThreshold = 5f;
    
    public override float GetTotalDistanceTraveled()
    {
        return 0;
    }
}