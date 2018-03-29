﻿using System;
using Frobozz.GdprConsent.NexusFacade.WebApi.DalModel;
using Xlent.Lever.Libraries2.Core.Storage.Model;

namespace Frobozz.GdprConsent.NexusFacade.WebApi.Dal
{
    public class Storage : IStorage
    {
        public ICrud<PersonTable, Guid> Person { get; }
        public IManyToOneRelation<AddressTable, PersonTable, Guid> Address { get; }

        public Storage(
            ICrud<PersonTable, Guid> personStorage,
            IManyToOneRelation<AddressTable, PersonTable, Guid> addressStorage
            )
        {
            Person = personStorage;
            Address = addressStorage;
        }
    }
}