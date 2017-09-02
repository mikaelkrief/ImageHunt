namespace ImageHunt.Model
{
    public interface INode
    {
        Geography Coordinate { get; }
        string Name { get; }
    }
}