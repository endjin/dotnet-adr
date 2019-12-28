// <copyright file="SettingsManager{T}.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Endjin.Adr.Cli.Configuration
{
    using System;
    using System.IO;
    using Endjin.Adr.Cli.Contracts;
    using Newtonsoft.Json;

    public class SettingsManager<T> : ISettingsManager<T>
        where T : class
    {
        private readonly IAppEnvironment appEnvironment;

        public SettingsManager(IAppEnvironment appEnvironment)
        {
            this.appEnvironment = appEnvironment;
        }

        public T LoadSettings(string fileName)
        {
            string filePath = this.GetLocalFilePath(fileName);
            return File.Exists(filePath) ? JsonConvert.DeserializeObject<T>(File.ReadAllText(filePath)) : null;
        }

        public void SaveSettings(T settings, string fileName)
        {
            string filePath = this.GetLocalFilePath(fileName);
            string json = JsonConvert.SerializeObject(settings);

            File.WriteAllText(filePath, json);
        }

        private string GetLocalFilePath(string fileName)
        {
            string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

            return Path.Combine(this.appEnvironment.ConfigurationPath, fileName);
        }
    }
}