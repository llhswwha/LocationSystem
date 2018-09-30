namespace Location.IModel
{
    public interface IKKSCode
    {
        int Id { get; set; }
        string Serial { get; set; }
        string Name { get; set; }
        string Code { get; set; }
        string ParentCode { get; set; }
        string DesinCode { get; set; }
        string MainType { get; set; }
        string SubType { get; set; }
        string System { get; set; }
    }
}