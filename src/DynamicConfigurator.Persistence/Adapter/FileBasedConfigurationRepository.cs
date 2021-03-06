﻿using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DynamicConfigurator.Persistence.Adapter
{
    public class FileBasedConfigurationRepository : IConfigurationRepository
    {
        public const string ConfigFolder = "configs";
        private static string _configDirectory;

        public FileBasedConfigurationRepository() : this(Path.Combine(Directory.GetCurrentDirectory(), ConfigFolder))
        {
        }

        public FileBasedConfigurationRepository(string configDirectory)
        {
            _configDirectory = configDirectory;
            //throw exception if not valid directory
            new DirectoryInfo(configDirectory).Create();
        }

        public void Create(string key, string value)
        {
            var filePath = BasePath(key);
            File.WriteAllText(filePath, value);
        }

        public string Read(string key)
        {
            var filePath = BasePath(key);
            return File.Exists(filePath) ? File.ReadAllText(filePath) : null;
        }

        public List<string> GetKeys()
        {
            return Directory.GetFiles(_configDirectory).ToList();
        }

        public bool Delete(string key)
        {
            var filePath = BasePath(key);
            File.Delete(filePath);
            return !File.Exists(filePath);
        }

        private static string BasePath(string relativePath)
        {
            return Path.Combine(_configDirectory, relativePath);
        }
    }
}
