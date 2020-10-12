namespace LifehackStudioApp.Services
{
    public class BaseClient : IClient
    {
        public int Id { get; set ; }
        public string Name { get; set; }
        public State State { get; set; }
    }
}
