using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using PluginInterface;
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using System.Reflection;

namespace MainBuild
{
    class Program
    {
        static List<Plugin> loadedPlugins = new List<Plugin>();
        static Dictionary<string, Plugin> allCommands = new Dictionary<string, Plugin>();
        static void Main(string[] args)
        {
            foreach (string singleFile in Directory.GetFiles("plugins", "*.cs"))
            {
                Console.Write(singleFile + ": ");
                PluginCompiler pluginCompiler = new PluginCompiler(singleFile);
                Console.Write(((pluginCompiler.compilePlugin()) ? "Compile success (" + pluginCompiler.finalPath + ")" : "Compile failed") + Environment.NewLine);
                foreach (Type singleType in Assembly.LoadFile(pluginCompiler.finalPath).GetTypes())
                {
                    if (singleType.GetInterfaces().Contains(typeof(Plugin)))
                    {
                        loadedPlugins.Add((Plugin)Activator.CreateInstance(singleType));
                    }
                }
            }
            foreach (Plugin loadedPlugin in loadedPlugins)
            {
                foreach (string singleTrigger in loadedPlugin.Triggers)
                {
                    allCommands.Add(singleTrigger, loadedPlugin);
                }
            }
            while (true)
            {
                string inputCommand = Console.ReadLine();
                if (inputCommand == "help")
                {
                    string availableCommands = "Available commands:";
                    foreach (Plugin loadedPlugin in loadedPlugins)
                    {
                        availableCommands += Environment.NewLine + "    " + string.Join(", ", loadedPlugin.Triggers) + " - " + loadedPlugin.Description + " (" + loadedPlugin.Name + " by " + loadedPlugin.Author + ")";
                    }
                    Console.WriteLine(availableCommands);
                }
                else
                {
                    if (!(allCommands.ContainsKey(inputCommand)))
                    {
                        Console.WriteLine("Invalid command");
                    }
                    else
                    {
                        allCommands[inputCommand].Run();
                    }
                }
            }
        }
    }
    class PluginCompiler
    {
        string fullPath;
        public string finalPath;
        public PluginCompiler(string sourcePath)
        {
            fullPath = Path.GetFullPath(sourcePath);
        }
        public bool compilePlugin(string outputAssembly = null, string outputPath = "plugins/compiled/")
        {
            string toOutput = outputPath + (((outputAssembly == null) ? Path.GetFileNameWithoutExtension(fullPath) : outputAssembly) + ".dll");
            if (File.Exists(toOutput))
            {
                finalPath = Path.GetFullPath(toOutput);
                return true;
            }
            CSharpCodeProvider codeProvider = new CSharpCodeProvider();
            ICodeCompiler codeCompiler = codeProvider.CreateCompiler();
            CompilerParameters compileParams = new CompilerParameters();
            compileParams.ReferencedAssemblies.Add(typeof(Plugin).Assembly.Location);
            compileParams.GenerateExecutable = false;
            compileParams.OutputAssembly = toOutput;
            CompilerResults compileResults = codeCompiler.CompileAssemblyFromSource(compileParams, File.ReadAllText(fullPath));
            if (compileResults.Errors.Count == 0)
            {
                finalPath = Path.GetFullPath(toOutput);
                return true;
            }
            return false;
        }
    }
}
