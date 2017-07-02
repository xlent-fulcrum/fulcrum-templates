﻿using System;
using System.Threading.Tasks;
using DM = Frobozz.PersonProfiles.Dal.MemoryStorage.PersonProfile;
using SM = Frobozz.PersonProfiles.FulcrumFacade.Contract.PersonProfiles;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Xlent.Lever.Libraries2.Standard.Storage.Model;

namespace Frobozz.PersonProfiles.Bll.Tests
{
    [TestClass]
    public class PersonProfilesTest
    {
        private static readonly string Namespace = typeof(PersonProfilesTest).Namespace;
        private Mock<DM.IPersonProfilePersistance> _personProfilesClientMock;
        private IPersonProfilesFunctionality _personProfilesFunctionality;
        private SM.PersonProfile _bllPerson;
        private IStorableItem<Guid> _dalPerson;

        [TestInitialize]
        public void Initialize()
        {
            _personProfilesClientMock = new Mock<DM.IPersonProfilePersistance>();
            _personProfilesFunctionality = new PersonProfilesFunctionality(_personProfilesClientMock.Object);
            var id = Guid.NewGuid();
            var eTag = Guid.NewGuid().ToString();
            _dalPerson = new DM.StorablePersonProfile
            {
                Id = id,
                ETag = eTag,
                GivenName = "Joe",
                Surname = "Smith"
            };
            _bllPerson = new SM.PersonProfile
            {
                Id = id.ToString(),
                ETag = eTag,
                GivenName = "Joe",
                Surname = "Smith"
            };
            _personProfilesClientMock.Setup(mock => mock.CreateAsync(It.IsAny<IStorableItem<Guid>>())).ReturnsAsync(_dalPerson);
            _personProfilesClientMock.Setup(mock => mock.ReadAsync(It.IsAny<Guid>())).ReturnsAsync(_dalPerson);
            _personProfilesClientMock.Setup(mock => mock.UpdateAsync(It.IsAny<IStorableItem<Guid>>())).ReturnsAsync(_dalPerson);
            _personProfilesClientMock.Setup(mock => mock.DeleteAsync(It.IsAny<Guid>())).Returns(Task.FromResult(0));
        }

        [TestMethod]
        public async Task Create()
        {
            var person = await _personProfilesFunctionality.CreateAsync(_bllPerson);
            Assert.IsNotNull(person);
            Assert.AreEqual(_bllPerson, person);

        }

        [TestMethod]
        public async Task Read()
        {
            var person = await _personProfilesFunctionality.ReadAsync(_bllPerson.Id);
            Assert.IsNotNull(person);
            Assert.AreEqual(_bllPerson, person);

        }

        [TestMethod]
        public async Task Update()
        {
            var person = await _personProfilesFunctionality.UpdateAsync(_bllPerson);
            Assert.IsNotNull(person);
            Assert.AreEqual(_bllPerson, person);

        }

        [TestMethod]
        public async Task Delete()
        {
            await _personProfilesFunctionality.DeleteAsync(_bllPerson.Id);

        }
    }
}
