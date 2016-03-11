using System;
using PluginInterface;
using System.IO;

namespace pluginBuild
{
    public class another_one : Plugin
    {
        public string Name { get { return "Another"; } }
        public string Description { get { return "Seriously, another plugin!"; } }
        public string Author { get { return "Commodity"; } }
        public string[] Triggers { get { return new string[] { "another", "idk" }; } }
        public void Run()
        {
            Console.WriteLine(randomString());
        }
        string randomString()
        {
            return Path.GetRandomFileName().ToLower().Replace(".", "");
        }
    }
}
