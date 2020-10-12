using System;

namespace LifehackStudioApp.Services.RespondCases
{
    public class RandomRespondCase : IRespondCase
    {
        public string Key => "Рандом";

        public string RespondTo(IClient client, string message)
        {
            var random = new Random();
            return (random.NextDouble() * 2 - 1).ToString();
        }
    }
}
