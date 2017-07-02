﻿using System;
using System.Threading.Tasks;
using Frobozz.PersonProfiles.Dal.MemoryStorage.PersonProfile;
using Frobozz.PersonProfiles.FulcrumFacade.Contract.PersonProfiles;
using Xlent.Lever.Libraries2.Standard.Assert;
using Xlent.Lever.Libraries2.Standard.Storage.Model;
using PersonProfile = Frobozz.PersonProfiles.FulcrumFacade.Contract.PersonProfiles.PersonProfile;

namespace Frobozz.PersonProfiles.Bll
{
    public class PersonProfilesFunctionality : IPersonProfilesFunctionality
    {
        private static readonly string Namespace = typeof(PersonProfilesFunctionality).Namespace;
        private IPersonProfilePersistance _storage;

        public PersonProfilesFunctionality(IPersonProfilePersistance storage)
        {
            _storage = storage;
        }

        public async Task<PersonProfile> CreateAsync(PersonProfile item)
        {
            var dalPerson = await _storage.CreateAsync(ToDal(item));
            return ToService(dalPerson);
        }

        public async Task<PersonProfile> ReadAsync(string id)
        {
            var dalPerson = await _storage.ReadAsync(ToGuid(id));
            return ToService(dalPerson);
        }

        public async Task<PersonProfile> UpdateAsync(PersonProfile item)
        {
            var dalPerson = await _storage.UpdateAsync(ToDal(item));
            return ToService(dalPerson);
        }

        public async Task DeleteAsync(string id)
        {
            await _storage.DeleteAsync(ToGuid(id));
        }

        private static PersonProfile ToService(IStorableItem<Guid> source)
        {
            if (source == null) return null;
            var s = source as StorablePersonProfile;
            InternalContract.Require(s != null, $"Expected parameter {nameof(source)} to be of type {typeof(StorablePersonProfile).Name}");
            var target = new PersonProfile
            {
                Id = s.Id.ToString(),
                ETag = s.ETag,
                GivenName = s.GivenName,
                Surname = s.Surname
            };
            return target;
        }

        private static IStorableItem<Guid> ToDal(PersonProfile source)
        {
            if (source == null) return null;
            var target = new StorablePersonProfile
            {
                Id = ToGuid(source.Id),
                ETag = source.ETag,
                GivenName = source.GivenName,
                Surname = source.Surname
            };
            return target;
        }

        private static Guid ToGuid(string id)
        {
            Guid guid;
            InternalContract.Require(Guid.TryParse(id, out guid), $"Expected a Guid in {nameof(id)} but the value was ({id}).");
            return guid;
        }
    }
}