namespace ItransitionTask5.API.Contracts
{
    public record RegisterRequest(
        string Name,
        string Email,
        string Position,
        string Password

    );
}