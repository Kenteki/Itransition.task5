namespace ItransitionTask5.API.Contracts
{
    public record UserRequest(
        string name,
        string email,
        string position,
        string passwordHash
        );
}
