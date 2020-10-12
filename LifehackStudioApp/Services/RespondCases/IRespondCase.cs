namespace LifehackStudioApp.Services.RespondCases
{
    public interface IRespondCase
    {
        /// <summary>
        /// Key of a respond case. Must be unique.
        /// </summary>
        string Key { get; }

        string RespondTo(IClient client, string message);
    }
}
