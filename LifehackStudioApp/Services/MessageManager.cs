using LifehackStudioApp.Services.RespondCases;
using System.Collections.Generic;
using System.Linq;

namespace LifehackStudioApp.Services
{
    public class MessageManager : IMessageManager
    {
        private List<IRespondCase> _cases = new List<IRespondCase>();
        private Dictionary<int, IClient> _clients = new Dictionary<int, IClient>();

        public IReadOnlyCollection<IClient> Clients => _clients.Values;

        public string Connected(IClient client)
        {
            _clients.Add(client.Id, client);
            return "Как тебя зовут?";
        }

        public void Disconnected(IClient client)
        {
            _clients.Remove(client.Id);
        }

        public void AddCase(IRespondCase respondCase)
        {
            _cases.Add(respondCase);
        }

        public void RemoveCase(IRespondCase respondCase)
        {
            var caseToRemove = _cases.Find(c => c.Key == respondCase.Key);
            if (caseToRemove != null)
                _cases.Remove(caseToRemove);
        }

        public string RespondTo(IClient client, string message)
        {
            switch (client.State)
            {
                case State.New:
                    {
                        if (message.Length < 2 || message.Length > 100)
                            return "Некорректное имя! Попробуй еще раз.";

                        client.Name = message;
                        client.State = State.Named;
                        return "Имя сохранено!";
                    }
                case State.Named:
                    {
                        string messageToLower = message.ToLower();
                        foreach (var respondCase in _cases)
                        {
                            if (messageToLower.Contains(respondCase.Key.ToLower()))
                                return respondCase.RespondTo(client, message);
                        }
                        return "Неизвестная команда! \n" +
                            $"Список доступных команд: \n{string.Join('\n', _cases.Select(c => c.Key))}";
                    }
            }
            return "";
        }
    }
}
