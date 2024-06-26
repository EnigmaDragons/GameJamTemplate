﻿using System.Collections.Generic;
using System;
using System.Linq;

public static class Message 
{
    private static readonly object Lock = new object();
    private static readonly List<MessageSubscription> EventSubs = new List<MessageSubscription>();
    private static MessageQueue Msgs = new MessageQueue();

    public static int SubscriptionCount => Msgs.SubscriptionCount;
    public static void Publish(object payload) => Msgs.Enqueue(payload);
    public static void Subscribe<T>(Action<T> onEvent, object owner) => Subscribe(MessageSubscription.Create(onEvent, owner));

    public static MessageQueue SwapMessageQueues(MessageQueue newQueue)
    {
        var msgs = Msgs;
        Msgs = newQueue;
        return msgs;
    }

    public static void Subscribe(MessageSubscription subscription)
    {
        Msgs.Subscribe(subscription);
        lock(Lock)
            EventSubs.Add(subscription);
    }

    public static void Unsubscribe(object owner)
    {
        Msgs.Unsubscribe(owner);
        lock(Lock)
            EventSubs.RemoveAll(x => x.Owner.Equals(owner));
    }
    
    public sealed class MessageQueue
    {
        private static readonly Dictionary<Type, string> TypeNamesCache = new Dictionary<Type, string>();
        
        private readonly Dictionary<string, List<object>> _eventActions = new Dictionary<string, List<object>>();
        private readonly Dictionary<object, List<UnsubscriptionAction>> _ownerUnsubscriptionActions = new Dictionary<object, List<UnsubscriptionAction>>();

        private readonly Queue<object> _eventQueue = new Queue<object>();
        private bool _isPublishing;

        public int SubscriptionCount => _eventActions.Sum(e => e.Value.Count);

        public void Enqueue(object payload)
        {
            _eventQueue.Enqueue(payload);
            ProcessQueuedMessages();
        }

        public void Subscribe(MessageSubscription subscription)
        {
            var eventType = GetTypeName(subscription.EventType);
            if (!_eventActions.ContainsKey(eventType))
                _eventActions[eventType] = new List<object>();
            if (!_ownerUnsubscriptionActions.ContainsKey(subscription.Owner))
                _ownerUnsubscriptionActions[subscription.Owner] = new List<UnsubscriptionAction>();
            _eventActions[eventType].Add(subscription.OnEvent);
            _ownerUnsubscriptionActions[subscription.Owner].Add(new UnsubscriptionAction(subscription.OnEvent, _eventActions[eventType]));
        }

        public void Unsubscribe(object owner)
        {
            if (!_ownerUnsubscriptionActions.ContainsKey(owner))
                return;
            foreach (var unsubscription in _ownerUnsubscriptionActions[owner])
                unsubscription.Execute();
            _ownerUnsubscriptionActions.Remove(owner);
        }

        private const string ProcessMessageString = "Processed Message Queue in {0}s";
        private void ProcessQueuedMessages()
        {
            if (_isPublishing) return;

            _isPublishing = true;
            while (_eventQueue.AnyNonAlloc()) 
                Publish(_eventQueue.Dequeue());
            _isPublishing = false;
        }

        private void Publish(object payload)
        {
            var eventType = GetTypeName(payload);
            //Log.Info($"Message - Published {eventType}");

            if (_eventActions.ContainsKey(eventType))
                foreach (var action in _eventActions[eventType].ToList())
                    ((Action<object>)action)(payload);
        }

        private string GetTypeName(object payload) => GetTypeName(payload.GetType());

        private string GetTypeName(Type t)
        {
            if (TypeNamesCache.TryGetValue(t, out var name)) 
                return name;
            
            var n = t.Name;
            TypeNamesCache[t] = t.Name;
            return n;
        }
    }
}
