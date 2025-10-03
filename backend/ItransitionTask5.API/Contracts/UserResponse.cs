namespace ItransitionTask5.API.Contracts
{
    public record UserResponse(
        Guid Id,
        string Name,
        string Email,
        string Position,
        DateTime RegistrationTime,
        DateTime? LastLogin,
        string Status
        );
}
