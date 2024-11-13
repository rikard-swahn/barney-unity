using System;
using System.Collections.Generic;
using System.Linq;

namespace net.gubbi.goofy.events {
    public class EventManager {
		
        private static EventManager _instance;

        public static EventManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new EventManager();
                }

                return _instance;
            }
        }

        public delegate void EventDelegate<T>(T e) where T : GameEvents.GameEvent;

        private Dictionary<Type, Delegate> _delegates = new Dictionary<Type, Delegate>();

        public void addListener<T>(EventDelegate<T> listener) where T : GameEvents.GameEvent
        {
            Delegate d;
            if (_delegates.TryGetValue(typeof(T), out d))
            {
                _delegates[typeof(T)] = Delegate.Combine(d, listener);
            }
            else
            {
                _delegates[typeof(T)] = listener;
            }
        }

        public void addListenerFirst<T>(EventDelegate<T> listener) where T : GameEvents.GameEvent
        {
            Delegate d;
            if (_delegates.TryGetValue(typeof(T), out d))
            {
                _delegates[typeof(T)] = Delegate.Combine(listener, d);
            }
            else
            {
                _delegates[typeof(T)] = listener;
            }
        }

        public void removeListener<T>(EventDelegate<T> listener) where T : GameEvents.GameEvent
        {
            Delegate d;
            if (_delegates.TryGetValue(typeof(T), out d))
            {
                Delegate currentDel = Delegate.Remove(d, listener);

                if (currentDel == null)
                {
                    _delegates.Remove(typeof(T));
                }
                else
                {
                    _delegates[typeof(T)] = currentDel;
                }
            }
        }

        public void removeAllListeners() {
            _delegates.Clear ();
        }

        public void removeListeners<T>() {
            _delegates = _delegates.Where(d => !typeof(T).IsAssignableFrom(d.Key))
                .ToDictionary(d => d.Key, d => d.Value);
        }

        public void raise<T>(T e) where T : GameEvents.GameEvent {
            if (e == null)
            {
                throw new ArgumentNullException("Raised event is null");
            }

            Delegate d;
            if (_delegates.TryGetValue(typeof(T), out d))
            {
                EventDelegate<T> callback = d as EventDelegate<T>;
                if (callback != null)
                {
                    callback(e);
                }
            }
        }
    }
}