namespace ItransitionTask5.API.Contracts
{
    public record LoginRequest(
        string Email,
        string Password
    );
}