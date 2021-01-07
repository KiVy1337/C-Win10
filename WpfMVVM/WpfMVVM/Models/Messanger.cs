using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfMVVM.Models {
    public class Messenger {
        private static readonly object CreationLock = new object();
        private static readonly ConcurrentDictionary<MessengerKey, object> Dictionary = new ConcurrentDictionary<MessengerKey, object>();


        private static Messenger _instance;

        /// Gets the single instance of the Messenger.
        public static Messenger Default {
            get {
                if (_instance == null) {
                    lock (CreationLock) {
                        if (_instance == null) {
                            _instance = new Messenger();
                        }
                    }
                }

                return _instance;
            }
        }

        /// Initializes a new instance of the Messenger class.
        private Messenger() {
            
        }

        /// Registers a recipient for a type of message T. The action parameter will be executed
        /// when a corresponding message is sent.
        public void Register<T>(object recipient, Action<T> action) {
            Register(recipient, action, null);
        }

        /// Registers a recipient for a type of message T and a matching context. The action parameter will be executed
        /// when a corresponding message is sent.
        public void Register<T>(object recipient, Action<T> action, object context) {
            var key = new MessengerKey(recipient, context);
            Dictionary.TryAdd(key, action);
        }

        /// Unregisters a messenger recipient completely. After this method is executed, the recipient will
        /// no longer receive any messages.
        public void Unregister(object recipient) {
            Unregister(recipient, null);
        }

        /// Unregisters a messenger recipient with a matching context completely. After this method is executed, the recipient will
        /// no longer receive any messages.
        public void Unregister(object recipient, object context) {
            object action;
            var key = new MessengerKey(recipient, context);
            Dictionary.TryRemove(key, out action);
        }

        /// Sends a message to registered recipients. The message will reach all recipients that are
        public void Send<T>(T message) {
            Send(message, null);
        }

        /// Sends a message to registered recipients. The message will reach all recipients that are
        /// registered for this message type and matching context.
        public void Send<T>(T message, object context) {
            IEnumerable<KeyValuePair<MessengerKey, object>> result;

            if (context == null) {
                // Get all recipients where the context is null.
                result = from r in Dictionary where r.Key.Context == null select r;
            }
            else {
                // Get all recipients where the context is matching.
                result = from r in Dictionary where r.Key.Context != null && r.Key.Context.Equals(context) select r;
            }

            foreach (var action in result.Select(x => x.Value).OfType<Action<T>>()) {
                // Send the message to all recipients.
                action(message);
            }
        }

        protected class MessengerKey {
            public object Recipient { get; private set; }
            public object Context { get; private set; }

            /// Initializes a new instance of the MessengerKey class.
            public MessengerKey(object recipient, object context) {
                Recipient = recipient;
                Context = context;
            }

            /// Determines whether the specified MessengerKey is equal to the current MessengerKey.
            protected bool Equals(MessengerKey other) {
                return Equals(Recipient, other.Recipient) && Equals(Context, other.Context);
            }

            /// Determines whether the specified MessengerKey is equal to the current MessengerKey.
            public override bool Equals(object obj) {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != GetType()) return false;

                return Equals((MessengerKey)obj);
            }

            /// Serves as a hash function for a particular type. 
            public override int GetHashCode() {
                unchecked {
                    return ((Recipient != null ? Recipient.GetHashCode() : 0) * 397) ^ (Context != null ? Context.GetHashCode() : 0);
                }
            }
        }
    }
}
