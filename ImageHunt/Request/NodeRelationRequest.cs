namespace ImageHunt.Request
{
    public class NodeRelationRequest
    {
      public int NodeId { get; set; }
      public int[] ChildrenId { get; set; }
    }
}
