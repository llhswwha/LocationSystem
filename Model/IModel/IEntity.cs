namespace Location.IModel
{
    public interface IEntity: IId,IName, IEntity<int>
    {
    }

    public interface IEntity<TKey> : IId<TKey>,IName
    {
    }

    public interface IId<TKey>
    {
        TKey Id { get; set; }
    }

    public interface IId:IId<int>
    {
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
