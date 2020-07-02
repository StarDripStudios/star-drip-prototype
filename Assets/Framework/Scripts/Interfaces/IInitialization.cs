namespace Framework
{
    public interface IInitializable
    {
        bool IsInitialized { get; }
        bool IsInitializing { get; }
        void Initialize();
    }
}
