public interface IHeathPoint
{
    float HeathPoint { get; }
    void OnHeathChange(float damage);
}