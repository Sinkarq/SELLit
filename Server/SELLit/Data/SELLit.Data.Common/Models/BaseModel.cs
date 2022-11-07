using System.ComponentModel.DataAnnotations;

namespace SELLit.Data.Common.Models;

public abstract class BaseModel<TKey> : IAuditInfo
{
    [Key] public TKey Id { get; set; } = default!;

    public DateTime CreatedOn { get; set; }

    public DateTime? ModifiedOn { get; set; }
}