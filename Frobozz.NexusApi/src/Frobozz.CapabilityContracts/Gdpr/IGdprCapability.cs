﻿using Xlent.Lever.Libraries2.Core.Storage.Model;

namespace Frobozz.CapabilityContracts.Gdpr
{
    public interface IGdprCapability
    {
        ICrud<Person, string> Person { get; }

        ICrud<Consent, string> Consent { get; }

        IManyToOneRelation<Consent, string> PersonConsent { get; }
    }
}
