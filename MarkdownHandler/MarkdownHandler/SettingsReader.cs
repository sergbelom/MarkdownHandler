using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace MarkdownHandler
{
    class SettingsReader
    {
        /// <summary>
        /// Read tags from AppSettings
        /// </summary>
        /// <param name="nameTags"></param>
        /// <returns>Tags collection</returns>
        public static ICollection<String> ReadAppSettings(string nameTags)
        {
            nameTags = nameTags.Replace("_", "");
            try
            {
                ICollection<String> result = new List<String>();
                foreach (var nameTag in (StringCollection)AppSettings.Default[nameTags])
                    result.Add(nameTag);
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
    }
}
