namespace ItransitionTask5.API.Contracts
{
    public record LoginResponse(
        string Token,
        Guid UserId,
        string Name,
        string Email,
        string Status
    );
}