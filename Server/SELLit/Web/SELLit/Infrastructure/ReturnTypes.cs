using Microsoft.AspNetCore.Identity;

namespace OneOf.Types;

public readonly struct InvalidLoginCredentials
{
    public InvalidLoginCredentials(IEnumerable<IdentityError> errors) => Errors = errors;

    public InvalidLoginCredentials() => this.Errors = Array.Empty<IdentityError>();

    public IEnumerable<IdentityError> Errors { get; }
}

public readonly struct Unauthorized
{
}