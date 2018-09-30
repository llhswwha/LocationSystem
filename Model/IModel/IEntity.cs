namespace Location.IModel
{
    public interface IEntity: IName
    {
        int Id { get; set; }
    }

    public interface IEntity<T> : IName
    {
        T Id { get; set; }
    }


    public interface IName
    {
        string Name { get; set; }
    }

    public interface INode:IName
    {
        int? ParentId { get; set; }
    }

    public interface IEntityNode: IEntity,INode
    {
        
    }
}
