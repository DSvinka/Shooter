namespace Code.Services
{
    public struct MessagePayload<T>
    {
        public object source;
        public T message;
    }
}