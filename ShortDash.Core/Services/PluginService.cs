using ShortDash.Core.Plugins;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace ShortDash.Core.Services
{
    public class PluginService
    {
        private readonly List<Type> pluginActions = new List<Type>();
        private readonly string pluginBasePath;

        public PluginService()
        {
            pluginBasePath = Path.Combine(Path.GetFullPath(Path.GetDirectoryName(typeof(PluginService).Assembly.Location)), "plugins");
            LoadPlugins();
        }

        public IEnumerable<Type> Actions => pluginActions;

        private static Assembly LoadPlugin(string pluginPath)
        {
            var loadContext = new PluginLoadContext(pluginPath);
            return loadContext.LoadFromAssemblyName(new AssemblyName(Path.GetFileNameWithoutExtension(pluginPath)));
        }

        private void FindActions(Assembly plugin)
        {
            foreach (var type in plugin.GetTypes())
            {
                if (!typeof(IShortDashAction).IsAssignableFrom(type))
                {
                    continue;
                }
                if (type.IsAbstract)
                {
                    continue;
                }
                pluginActions.Add(type);
            }
        }

        private void LoadPlugins()
        {
            if (!Directory.Exists(pluginBasePath))
            {
                return;
            }
            var options = new EnumerationOptions
            {
                IgnoreInaccessible = true,
                RecurseSubdirectories = false,
                MatchCasing = MatchCasing.CaseInsensitive
            };
            var pluginDirectories = new List<string>(Directory.EnumerateDirectories(pluginBasePath, "*", options));
            foreach (var pluginDirectory in pluginDirectories)
            {
                var pluginPaths = Directory.GetFiles(pluginDirectory, "ShortDash.Plugins.*.dll", options);
                foreach (var pluginPath in pluginPaths)
                {
                    var plugin = LoadPlugin(pluginPath);
                    FindActions(plugin);
                }
            }
        }
    }
}
