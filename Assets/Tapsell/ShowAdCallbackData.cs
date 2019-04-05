using System;
using Tapsell.Base;

namespace Tapsell
{
    public class ShowAdCallbackData : ICallbackData
    {
        public readonly Action<long, string> ErrorAction;
        public readonly Action<bool> AdClosedAction;
        public readonly Action<string, string> RewardAction;

        public ShowAdCallbackData(Action<long, string> errorAction, Action<bool> adClosedAction, Action<string, string> rewardAction)
        {
            ErrorAction = errorAction;
            AdClosedAction = adClosedAction;
            RewardAction = rewardAction;
        }
    }
}
