using Microsoft.AspNetCore.Identity;

namespace OneOf.Types;

public readonly struct InvalidLoginCredentials
{
    public InvalidLoginCredentials(IEnumerable<IdentityError> errors) => Errors = errors;

    public InvalidLoginCredentials() => this.Errors = Enumerable.Empty<IdentityError>();

    public IEnumerable<IdentityError> Errors { get; }
}

public readonly struct Unauthorized
{
}

public readonly struct UniqueConstraintError
{
    private readonly string propertyName;
    
    public UniqueConstraintError(string propertyName) => this.propertyName = propertyName;

    public readonly string Message => $"{propertyName} not available.";
}