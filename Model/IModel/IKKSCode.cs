namespace Location.IModel
{
    public interface IKKSCode:IEntity
    {
        string Serial { get; set; }
        string Code { get; set; }
        string ParentCode { get; set; }
        string DesinCode { get; set; }
        string MainType { get; set; }
        string SubType { get; set; }
        string System { get; set; }
    }
}