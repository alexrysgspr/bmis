namespace Bmis.EntityFramework.Entities;
public interface ISoftDeletableEntity
{
    public bool IsDeleted { get; set; }
}
