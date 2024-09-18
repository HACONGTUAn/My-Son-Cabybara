using System.Collections.Generic;

namespace Merge
{
    public static class Observer
    {
        public delegate void CallBackObserver(object data);

        static Dictionary<string, HashSet<CallBackObserver>> dictObserver = new Dictionary<string, HashSet<CallBackObserver>>();
        // Use this for initialization
        public static void AddObserver(string topicName, CallBackObserver callbackObserver)
        {
            HashSet<CallBackObserver> listObserver = CreateListObserverForTopic(topicName);
            listObserver.Add(callbackObserver);
        }

        public static void RemoveObserver(string topicName, CallBackObserver callbackObserver)
        {
            HashSet<CallBackObserver> listObserver = CreateListObserverForTopic(topicName);
            if (listObserver.Contains(callbackObserver))
            {
                listObserver.Remove(callbackObserver);
            }
        }

        public static void Notify(string topicName, object Data)
        {
            HashSet<CallBackObserver> listObserver = CreateListObserverForTopic(topicName);
            foreach (CallBackObserver observer in new HashSet<CallBackObserver>(listObserver))
            {
                observer(Data);
            }
        }

        public static void Notify(string topicName)
        {
            HashSet<CallBackObserver> listObserver = CreateListObserverForTopic(topicName);
            foreach (CallBackObserver observer in listObserver)
            {
                observer(null);
            }
        }

        private static HashSet<CallBackObserver> CreateListObserverForTopic(string topicName)
        {
            if (!dictObserver.ContainsKey(topicName))
                dictObserver.Add(topicName, new HashSet<CallBackObserver>());
            return dictObserver[topicName];
        }
    }
}