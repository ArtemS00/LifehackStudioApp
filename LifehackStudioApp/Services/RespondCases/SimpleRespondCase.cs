using System;

namespace LifehackStudioApp.Services.RespondCases
{
    /// <summary>
    /// Class for creation simple responce cases without making a new class
    /// </summary>
    public class SimpleRespondCase : IRespondCase
    {
        public string Key { get; }
        private Func<IClient, string, string> _respondTo;

        public SimpleRespondCase(string key, Func<IClient, string, string> respondToFunc)
        {
            Key = key == null 
                ? throw new ArgumentNullException(nameof(key)) 
                : key;
            _respondTo = respondToFunc == null
                ? throw new ArgumentNullException(nameof(respondToFunc))
                : respondToFunc;
        }

        public string RespondTo(IClient client, string message)
        {
            return _respondTo(client, message);
        }
    }
}
