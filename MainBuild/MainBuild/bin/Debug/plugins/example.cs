using System;
using PluginInterface;

namespace pluginBuild
{
    public class example : Plugin
    {
        public string Name { get { return "Example"; } }
        public string Description { get { return "An example plugin"; } }
        public string Author { get { return "Commodity"; } }
        public string[] Triggers { get { return new string[] { "eg", "example" }; } }
        public void Run()
        {
            Console.WriteLine(new Random().Next(0, 100).ToString());
        }
    }
}
