using System.Collections.Generic;
using Tapsell.Base.SimpleJSON;
using UnityEngine;

namespace Tapsell.Base
{
    public class TapsellMessageHandler : MonoBehaviour
    {
        private int _requestNum;
        private readonly Dictionary<string, IMessageHandler> _messageHandlers = new Dictionary<string, IMessageHandler>();
        private readonly Dictionary<long, ICallbackData> _callbackDataDict = new Dictionary<long, ICallbackData>();

        public const string MethodName = "HandleMessage";

        public void AddHandler(string action, IMessageHandler messageHandler)
        {
            if (action != null && messageHandler != null)
            {
                _messageHandlers.Add(action, messageHandler);
            }
        }
    
        public void HandleMessage(string messageStr)
        {
            Debug.Log("Handling message: " + messageStr);

            if (messageStr == null)
            {
                Debug.Log("Received message is null!");
                return;
            }

            TapsellMessage message = _getMessageFromJson(messageStr);
            if (message == null)
            {
                return;
            }
        
            var action = message.Action;
            var requestId = message.RequestId;
            _messageHandlers[action].HandleMessage(message, _callbackDataDict[requestId]);
        }

        public int NewItem(ICallbackData callbackData)
        {
            _requestNum++;
            _callbackDataDict.Add(_requestNum, callbackData);

            return _requestNum;
        }

        private TapsellMessage _getMessageFromJson(string messageStr)
        {
            var dataNode = JSON.Parse(messageStr);

            if (dataNode == null)
            {
                Debug.LogError("Message json is null!");
                return null;
            }
            return new TapsellMessage(dataNode);
        }
    }
}