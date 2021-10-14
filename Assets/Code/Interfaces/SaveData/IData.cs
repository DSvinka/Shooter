namespace Code.Interfaces.SaveData
{
    internal interface IData<T>
    {
        void Save(T data, string path);
        T Load(string path);
    }
}