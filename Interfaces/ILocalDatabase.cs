namespace WhatsStatusApp.Interfaces;
internal interface ILocalDatabase
{
    public Task AddNewStatusAsync(Status status);
    public Task DeleteStatusAsync(Status status);
    public Task UpdateStatusAsync(Status status);
    public Task FindStatusAsync(Status status);
    public Task<List<Status>> GetAllStatusesAsync();
    public Task ClearDatabaseAsync();
}
