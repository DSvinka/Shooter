using System;

namespace Code.Services
{
    internal sealed class MessageBrokerService<T>
    {
        public event Action<object, T> OnPublish = delegate(object source, T publish) {  };

        public void Publish(object source, T message)
        {
            if (message == null || source == null)
                return;
            
            OnPublish.Invoke(source, message);
        }
    }
}