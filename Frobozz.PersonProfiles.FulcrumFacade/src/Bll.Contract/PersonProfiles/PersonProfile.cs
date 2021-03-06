﻿using System;
using Xlent.Lever.Libraries2.Standard.Assert;
using Xlent.Lever.Libraries2.Standard.Storage.Model;

namespace Frobozz.PersonProfiles.FulcrumFacade.Contract.PersonProfiles
{
    /// <summary>
    /// A physical address.
    /// </summary>
    public partial class PersonProfile : StorableItem<string>, IPersonProfile
    {
        /// <inheritdoc />
        public string GivenName { get; set; }

        /// <inheritdoc />
        public string Surname { get; set; }
    }

    public partial class PersonProfile
    {
        #region IValidatable
        /// <inheritdoc />
        public override void Validate(string errorLocation, string propertyPath = "")
        {
            FulcrumValidate.IsNotNullOrWhiteSpace(GivenName, nameof(GivenName), errorLocation);
            FulcrumValidate.IsNotNullOrWhiteSpace(Surname, nameof(Surname), errorLocation);
        }
        #endregion
    }

    #region override object
    public partial class PersonProfile
    {
        
        /// <inheritdoc/>
        public override string ToString()
        {
            return $"{GivenName} {Surname}";
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            var person = obj as PersonProfile;
            if (person == null) return false;
            if (!Equals(person.Id, Id)) return false;
            if (!string.Equals(person.ETag, ETag, StringComparison.OrdinalIgnoreCase)) return false;
            if (!string.Equals(person.GivenName, GivenName, StringComparison.OrdinalIgnoreCase)) return false;
            // ReSharper disable once ConvertIfStatementToReturnStatement
            if (!string.Equals(person.Surname, Surname, StringComparison.OrdinalIgnoreCase)) return false;
            return true;
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            // ReSharper disable once NonReadonlyMemberInGetHashCode
            return Id.GetHashCode();
        }
    }
    #endregion
}
