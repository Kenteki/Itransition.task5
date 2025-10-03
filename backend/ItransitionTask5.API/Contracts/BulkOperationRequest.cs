namespace ItransitionTask5.API.Contracts
{
    public record BulkOperationRequest(
    List<Guid> UserIds
    );
}
