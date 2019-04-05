using System;
using Tapsell.Base.SimpleJSON;

namespace Tapsell.Base
{
    [Serializable]
    public class TapsellMessage
    {
        private const string RequestIdKey = "requestId";
        private const string ActionKey = "action";
        private const string EventTypeKey = "eventType";
        private const string DataKey = "data";
    
        public readonly long RequestId;
        public readonly string Action;
        public readonly string EventType;
        public readonly JSONClass Data;

        public TapsellMessage()
        {
        }

        public TapsellMessage(JSONNode dataNode)
        {
            RequestId = dataNode[RequestIdKey].AsInt;
            Action = dataNode[ActionKey].Value;
            EventType = dataNode[EventTypeKey].Value;
            Data = dataNode[DataKey].AsObject;
        }
    }
}