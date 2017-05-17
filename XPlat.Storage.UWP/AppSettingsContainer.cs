﻿namespace XPlat.Storage
{
    using System.Collections.Generic;

    /// <summary>
    /// Defines application settings.
    /// </summary>
    public sealed class AppSettingsContainer : IAppSettingsContainer
    {
        private readonly object obj = new object();

        /// <inheritdoc />
        public IDictionary<string, object> Values => ApplicationData.Current.LocalSettings.Values;

        /// <summary>
        /// Checks whether the settings container contains the specified key.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <returns>
        /// Returns true if exists; else false.
        /// </returns>
        public bool ContainsKey(string key)
        {
            bool containsKey;

            lock (this.obj)
            {
                containsKey = this.Values.ContainsKey(key);
            }

            return containsKey;
        }

        /// <summary>
        /// Gets the value from the settings container for the specified key.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <typeparam name="T">
        /// The type for the stored value.
        /// </typeparam>
        /// <returns>
        /// Returns the stored value.
        /// </returns>
        public T Get<T>(string key)
        {
            var value = default(T);

            if (!this.ContainsKey(key)) return value;

            lock (this.obj)
            {
                var setting = this.Values[key];
                if (setting is T)
                {
                    value = (T)setting;
                }
            }

            return value;
        }

        /// <summary>
        /// Adds or updates a key in the settings container with the specified value.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        public void AddOrUpdate(string key, object value)
        {
            lock (this.obj)
            {
                this.Values[key] = value;
            }
        }

        /// <summary>
        /// Removes the specified key's value from the settings container.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        public void Remove(string key)
        {
            if (!this.ContainsKey(key)) return;

            lock (this.obj)
            {
                this.Values.Remove(key);
            }
        }
    }
}