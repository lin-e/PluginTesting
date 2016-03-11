namespace PluginInterface
{
    public interface Plugin
    {
        string Name { get; }
        string Author { get; }
        string Description { get; }
        string[] Triggers { get; }
        void Run();
    }
}
