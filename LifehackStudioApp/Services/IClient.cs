namespace LifehackStudioApp.Services
{
    public interface IClient
    {
        int Id { get; set; }
        string Name { get; set; }
        State State { get; set; }
    }
}
