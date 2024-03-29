﻿namespace WhatsStatusApp.Services;
internal class LocalDatabaseService : ILocalDatabase
{
    #region Instance
    private static LocalDatabaseService _loaclDatabaseService;
    public static LocalDatabaseService LocalDB { get { _loaclDatabaseService ??= new(); return _loaclDatabaseService; } }
    #endregion

    private SQLiteAsyncConnection database;

    private async Task InitializeDatabase()
    {
        if (database is not null) 
            return;

        var databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "whatsStatus.db3");
        database = new SQLiteAsyncConnection(databasePath);
        await database.CreateTableAsync<Status>();
    }

    public async Task AddNewStatusAsync(Status status)
    {
        await InitializeDatabase();
        await database.InsertAsync(status);
    }

    public async Task DeleteStatusAsync(Status status)
    {
        await InitializeDatabase();
        await database.DeleteAsync<Status>(status.Id);
    }

    /// <summary>
    /// To be implemented... [F.F]
    /// </summary>
    /// <param name="status"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public Task FindStatusAsync(Status status) => throw new NotImplementedException();

    public async Task<List<StatusGroup>> GetGroupedStatusListAsync()
    {
        List<StatusGroup> statusGroupCollection = new();

        var list = await GetStatusListAsync();

        var groups = list?.GroupBy(s => s.DateCreated.Date).OrderByDescending(s => s.Key);

        foreach (var l2 in groups)
            statusGroupCollection.Add(new StatusGroup(l2.Key, l2.OrderByDescending(s => s.DateCreated).ToList()));

        return statusGroupCollection;
    }

    public async Task<List<Status>> GetStatusListAsync()
    {
        await InitializeDatabase();
        return await database.Table<Status>().ToListAsync();
    }

    /// <summary>
    /// Future Feature [F.F]
    /// </summary>
    /// <param name="status"></param>
    /// <returns></returns>
    public async Task UpdateStatusAsync(Status status)
    {
        await InitializeDatabase();
        await database.UpdateAsync(status);
    }

    /// <summary>
    /// Deletes all data from the (Status) class existing in the database
    /// </summary>
    public async Task ClearDatabaseAsync()
    {
        await InitializeDatabase();
        await database.DeleteAllAsync<Status>();
    }

}
