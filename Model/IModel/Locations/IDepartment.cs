namespace Location.IModel.Locations
{
    public interface IDepartment
    {
        //ICollection<IDepartment> Children { get; set; }
        int Id { get; set; }
        string Name { get; set; }
        int? ParentId { get; set; }
        int ShowOrder { get; set; }
    }
}
