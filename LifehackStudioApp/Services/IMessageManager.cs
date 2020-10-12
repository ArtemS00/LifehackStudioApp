using LifehackStudioApp.Services.RespondCases;
using System.Collections.Generic;

namespace LifehackStudioApp.Services
{
    public interface IMessageManager
    {
        void AddCase(IRespondCase respondCase);
        void RemoveCase(IRespondCase respondCase);

        string RespondTo(IClient client, string message);
        string Connected(IClient client);
        void Disconnected(IClient client);

        IReadOnlyCollection<IClient> Clients { get; }
    }
}
