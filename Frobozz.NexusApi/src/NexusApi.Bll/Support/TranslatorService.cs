﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Nexus.Link.Libraries.Core.Translation;

namespace Frobozz.NexusApi.Bll.Support
{
    public class TranslatorService : ITranslatorService
    {
        /// <inheritdoc />
        public Task<IDictionary<string, string>> TranslateAsync(IEnumerable<string> conceptValues, string targetClientName)
        {
            var translations = new Dictionary<string, string>();
            foreach (var path in conceptValues)
            {
                if (!ConceptValue.TryParse(path, out var conceptValue))
                {
                    translations[path] = path;
                    continue;
                }
                var value = conceptValue.Value;
                if (conceptValue.Value.Contains("client-")) value = value.Replace("client-", "server-");
                else if (conceptValue.Value.Contains("server-")) value = value.Replace("server-", "client-");
                translations[path] = value;
            }

            return Task.FromResult((IDictionary<string, string>)translations);
        }
    }
}
