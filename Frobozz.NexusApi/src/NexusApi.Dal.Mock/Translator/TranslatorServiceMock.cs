﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Nexus.Link.Libraries.Core.Assert;
using Nexus.Link.Libraries.Core.Translation;

namespace Frobozz.NexusApi.Dal.Mock.Translator
{
    public class TranslatorServiceMock : ITranslatorService
    {
        private readonly string _clientPrefix;
        private readonly bool _fromServer;

        public TranslatorServiceMock(string clientName, bool fromServer)
        {
            _clientPrefix = $"{clientName}-";
            _fromServer = fromServer;
        }

        /// <inheritdoc />
        public Task<IDictionary<string, string>> TranslateAsync(IEnumerable<string> conceptValues, string targetClientName)
        {
            var translations = new Dictionary<string, string>();
            foreach (var conceptValuePath in conceptValues)
            {
                var conceptValue = ToConceptValue(conceptValuePath);
                if (conceptValue == null)
                {
                    // Not a concept value path, no translation possible.
                    translations[conceptValuePath] = conceptValuePath;
                    continue;
                }

                if ((_fromServer && conceptValue.ClientName == "client")
                    || (!_fromServer && conceptValue.ClientName == "server"))
                {
                    // Not a concept value path, no translation possible.
                    translations[conceptValuePath] = conceptValue.Value;
                    continue;
                }


                var value = conceptValue.Value;
                if (conceptValue.ConceptName == "person.address.type.code")
                {
                    value = _fromServer ? ToAddressTypeClient(value) : ToAddressTypeServer(value);
                }
                else
                {
                    if (_fromServer)
                    {
                        if (value.StartsWith(_clientPrefix))
                            InternalContract.Fail(
                                $"The value of {conceptValue} must NOT begin with \"{_clientPrefix}\" from the server.");
                        value = $"{_clientPrefix}{value}";
                    }
                    else
                    {
                        if (!value.StartsWith(_clientPrefix))
                            InternalContract.Fail(
                                $"The value of {conceptValue} must begin with \"{_clientPrefix}\" from the client.");
                        value = value.Replace(_clientPrefix, "");
                    }
                }

                translations[conceptValuePath] = value;
            }

            return Task.FromResult((IDictionary<string, string>) translations);
        }

        private static IConceptValue ToConceptValue(string conceptValuePath)
        {
            return ConceptValue.TryParse(conceptValuePath, out var conceptValue) ? conceptValue : null;
        }

        private static string ToAddressTypeServer(string source)
        {
            switch (source)
            {
                case "Public": return "1";
                case "Invoice": return "2";
                case "Delivery": return "3";
                case "Postal": return "4";
                default:
                    FulcrumAssert.Fail($"Unknown address type ({source}). Must be one of Public, Invoice, Deliver, Postal.");
                    return "0";
            }
        }

        private static string ToAddressTypeClient(string source)
        {
            InternalContract.Require(int.TryParse(source, out var sourceAsInt),
                $"Expected value ({source}) to be an integer.");
            {

            }
            switch (sourceAsInt)
            {
                case 1: return "Public";
                case 2: return "Invoice";
                case 3:
                    return "Delivery";
                case 4: return "Postal";
                default:
                    FulcrumAssert.Fail($"Unknown address type ({source}).");
                    return "None";
            }
        }
    }
}
