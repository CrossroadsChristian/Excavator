// <copyright>
// Copyright 2013 by the Spark Development Network
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>
//

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Configuration;
using System.IO;
using System.Linq;
using LumenWorks.Framework.IO.Csv;
using Rock.Data;
using Rock.Model;
using Rock.Web.Cache;

namespace Excavator.CSV
{
    /// <summary>
    /// This example extends the base Excavator class to consume a CSV model.
    /// </summary>
    [Export( typeof( ExcavatorComponent ) )]
    partial class CSVComponent : ExcavatorComponent
    {
        #region Fields

        /// <summary>
        /// Gets the full name of the excavator type.
        /// </summary>
        /// <value>
        /// The name of the database being imported.
        /// </value>
        public override string FullName
        {
            get { return "CSV File"; }
        }

        /// <summary>
        /// Gets the supported file extension type(s).
        /// </summary>
        /// <value>
        /// The supported extension type.
        /// </value>
        public override string ExtensionType
        {
            get { return ".csv"; }
        }

        /// <summary>
        /// The local database
        /// </summary>
        public CsvReader Database;

        /// <summary>
        /// The person assigned to do the import
        /// </summary>
        private PersonAlias ImportPersonAlias;

        /// <summary>
        /// The person entity type identifier
        /// </summary>
        private int PersonEntityTypeId;

        /// <summary>
        /// All the people who've been imported
        /// </summary>
        private Dictionary<string, int?> ImportedPeople;

        /// <summary>
        /// The list of current campuses
        /// </summary>
        private List<Campus> CampusList;

        // Report progress when a multiple of this number has been imported
        private static int ReportingNumber = 50;

        #endregion

        #region Methods

        /// <summary>
        /// Loads the database for this instance.
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public override bool LoadSchema( string fileName )
        {
            var dbPreview = new CsvReader( new StreamReader( fileName ), true );
            //dbPreview.MissingFieldAction = MissingFieldAction.ReplaceByNull;
            TableNodes = new List<DatabaseNode>();

            var tableItem = new DatabaseNode();
            tableItem.Name = Path.GetFileNameWithoutExtension( fileName );
            int currentIndex = 0;

            var firstRow = dbPreview.ElementAtOrDefault( 0 );
            if ( firstRow != null )
            {
                foreach ( var columnName in dbPreview.GetFieldHeaders() )
                {
                    var childItem = new DatabaseNode();
                    childItem.Name = columnName;
                    childItem.NodeType = typeof( string );
                    childItem.Value = firstRow[currentIndex] ?? string.Empty;
                    childItem.Table.Add( tableItem );
                    tableItem.Columns.Add( childItem );
                    currentIndex++;
                }

                TableNodes.Add( tableItem );
            }

            // reset the reader so we don't skip the first row
            Database = new CsvReader( new StreamReader( fileName ), true );
            return TableNodes.Count() > 0 ? true : false;
        }

        /// <summary>
        /// Transforms the data from the dataset.
        /// </summary>
        public override int TransformData( string importUser = null )
        {
            ReportProgress( 0, "Starting import..." );
            CheckExistingImport( importUser );

            var columnList = TableNodes.Where( n => n.Checked != false ).ToList();

            MapFamilyData();

            return 0;
        }

        /// <summary>
        /// Checks the database for existing import data.
        /// </summary>
        private void CheckExistingImport( string importUser )
        {
            var rockContext = new RockContext();
            var personService = new PersonService( rockContext );
            var attributeService = new AttributeService( rockContext );
            var attributeValueService = new AttributeValueService( rockContext );
            var importPerson = personService.GetByFullName( importUser, includeDeceased: false, allowFirstNameOnly: true ).FirstOrDefault();
            if ( importPerson == null )
            {
                importPerson = personService.Queryable().FirstOrDefault();
            }

            ImportPersonAlias = new PersonAliasService().Get( importPerson.Id );

            PersonEntityTypeId = EntityTypeCache.Read( "Rock.Model.Person" ).Id;
            var textFieldTypeId = FieldTypeCache.Read( new Guid( Rock.SystemGuid.FieldType.TEXT ) ).Id;

            var personAttributes = attributeService.GetByEntityTypeId( PersonEntityTypeId ).ToList();
            var importIdentifier = personAttributes.FirstOrDefault( a => a.Key == "ExcavatorImportId" );
            if ( importIdentifier == null )
            {
                importIdentifier = new Rock.Model.Attribute();
                importIdentifier.Key = "ExcavatorImportId";
                importIdentifier.Name = "Excavator Import Id";
                importIdentifier.FieldTypeId = textFieldTypeId;
                importIdentifier.EntityTypeId = PersonEntityTypeId;
                importIdentifier.EntityTypeQualifierValue = string.Empty;
                importIdentifier.EntityTypeQualifierColumn = string.Empty;
                importIdentifier.Description = "The identifier for this person imported using Excavator";
                importIdentifier.DefaultValue = string.Empty;
                importIdentifier.IsMultiValue = false;
                importIdentifier.IsRequired = false;
                importIdentifier.Order = 0;

                attributeService.Add( importIdentifier, ImportPersonAlias );
                attributeService.Save( importIdentifier, ImportPersonAlias );
                personAttributes.Add( importIdentifier );
            }

            ReportProgress( 0, "Checking for existing people..." );
            ImportedPeople = attributeValueService.GetByAttributeId( importIdentifier.Id )
                .ToDictionary( av => av.Value, av => av.EntityId );

            CampusList = new CampusService( rockContext ).Queryable().ToList();
        }

        /// <summary>
        /// Maps the family data.
        /// </summary>
        private void MapFamilyData()
        {
            var groupTypeRoleService = new GroupTypeRoleService();
            var attributeService = new AttributeService();
            var dvService = new DefinedValueService();
            var familyList = new List<Group>();
            int currentFamilyId = 0;
            Group familyGroup = new Group();

            // Marital statuses: Married, Single, Separated, etc
            List<DefinedValue> maritalStatusTypes = dvService.Queryable()
                .Where( dv => dv.DefinedType.Guid == new Guid( Rock.SystemGuid.DefinedType.PERSON_MARITAL_STATUS ) ).ToList();

            // Connection statuses: Member, Visitor, Attendee, etc
            List<DefinedValue> connectionStatusTypes = dvService.Queryable()
                .Where( dv => dv.DefinedType.Guid == new Guid( Rock.SystemGuid.DefinedType.PERSON_CONNECTION_STATUS ) ).ToList();

            // Record status reasons: No Activity, Moved, Deceased, etc
            List<DefinedValue> recordStatusReasons = dvService.Queryable()
                .Where( dv => dv.DefinedType.Guid == new Guid( Rock.SystemGuid.DefinedType.PERSON_RECORD_STATUS_REASON ) ).ToList();

            // Record statuses: Active, Inactive, Pending
            int? recordStatusActiveId = dvService.Get( new Guid( Rock.SystemGuid.DefinedValue.PERSON_RECORD_STATUS_ACTIVE ) ).Id;
            int? recordStatusInactiveId = dvService.Get( new Guid( Rock.SystemGuid.DefinedValue.PERSON_RECORD_STATUS_INACTIVE ) ).Id;
            int? recordStatusPendingId = dvService.Get( new Guid( Rock.SystemGuid.DefinedValue.PERSON_RECORD_STATUS_PENDING ) ).Id;

            // Record type: Person
            int? personRecordTypeId = dvService.Get( new Guid( Rock.SystemGuid.DefinedValue.PERSON_RECORD_TYPE_PERSON ) ).Id;

            // Suffix type: Dr., Jr., II, etc
            List<DefinedValue> suffixTypes = dvService.Queryable()
                .Where( dv => dv.DefinedType.Guid == new Guid( Rock.SystemGuid.DefinedType.PERSON_SUFFIX ) ).ToList();

            // Title type: Mr., Mrs. Dr., etc
            List<DefinedValue> titleTypes = dvService.Queryable()
                .Where( dv => dv.DefinedType.Guid == new Guid( Rock.SystemGuid.DefinedType.PERSON_TITLE ) ).ToList();

            // Note type: Comment
            int noteCommentTypeId = new NoteTypeService().Get( new Guid( "7E53487C-D650-4D85-97E2-350EB8332763" ) ).Id;

            // Group roles: Adult, Child, others
            int adultRoleId = groupTypeRoleService.Get( new Guid( Rock.SystemGuid.GroupRole.GROUPROLE_FAMILY_MEMBER_ADULT ) ).Id;
            int childRoleId = groupTypeRoleService.Get( new Guid( Rock.SystemGuid.GroupRole.GROUPROLE_FAMILY_MEMBER_CHILD ) ).Id;

            // Group type: Family
            int familyGroupTypeId = GroupTypeCache.GetFamilyGroupType().Id;

            // Look up additional Person attributes (existing)
            var personAttributes = attributeService.GetByEntityTypeId( PersonEntityTypeId ).ToList();

            // Cached attributes: ImportId, PreviousChurch, Position, Employer, School
            var importIdAttribute = AttributeCache.Read( personAttributes.FirstOrDefault( a => a.Key == "ExcavatorImportId" ) );
            var membershipDateAttribute = AttributeCache.Read( personAttributes.FirstOrDefault( a => a.Key == "MembershipDate" ) );
            var baptismDateAttribute = AttributeCache.Read( personAttributes.FirstOrDefault( a => a.Key == "BaptismDate" ) );
            var firstVisitAttribute = AttributeCache.Read( personAttributes.FirstOrDefault( a => a.Key == "FirstVisit" ) );
            var previousChurchAttribute = AttributeCache.Read( personAttributes.FirstOrDefault( a => a.Key == "PreviousChurch" ) );
            var employerAttribute = AttributeCache.Read( personAttributes.FirstOrDefault( a => a.Key == "Employer" ) );
            var positionAttribute = AttributeCache.Read( personAttributes.FirstOrDefault( a => a.Key == "Position" ) );
            var schoolAttribute = AttributeCache.Read( personAttributes.FirstOrDefault( a => a.Key == "School" ) );

            int completed = 0;
            ReportProgress( 0, string.Format( "Adding family data ({1:N0} people already exist).", ImportedPeople.Count() ) );

            do
            {
                var row = Database.First();
                if ( row != null )
                {
                    int groupRoleId = adultRoleId;
                    int rowPersonId = row[PersonId].AsType<int>();
                    int rowFamilyId = row[FamilyId].AsType<int>();
                    var rowFamilyName = row[FamilyName];
                    if ( rowFamilyId > 1 && rowFamilyId != currentFamilyId )
                    {
                        familyList.Add( familyGroup );
                        familyGroup = new Group();
                        currentFamilyId = rowFamilyId;
                    }

                    Person person = new Person();
                    person.RecordTypeValueId = personRecordTypeId;
                    person.CreatedByPersonAliasId = ImportPersonAlias.Id;
                    person.FirstName = row[FirstName];
                    person.NickName = row[NickName];
                    person.LastName = row[LastName];
                    person.Email = row[Email];

                    var activeEmail = row[IsEmailActive] as string;
                    if ( activeEmail != null )
                    {
                        person.IsEmailActive = bool.Parse( activeEmail );
                    }

                    var birthDate = row[DateOfBirth] as string;
                    if ( birthDate != null )
                    {
                        person.BirthDate = DateTime.Parse( birthDate );
                    }

                    var gender = row[Gender] as string;
                    if ( gender != null )
                    {
                        switch ( gender.Trim().ToLower() )
                        {
                            case "m":
                            case "male":
                                person.Gender = Rock.Model.Gender.Male;
                                break;

                            case "f":
                            case "female":
                                person.Gender = Rock.Model.Gender.Female;
                                break;

                            default:
                                person.Gender = Rock.Model.Gender.Unknown;
                                break;
                        }
                    }

                    var prefix = row[Prefix] as string;
                    if ( prefix != null )
                    {
                        prefix = prefix.RemoveSpecialCharacters().Trim();
                        person.TitleValueId = titleTypes.Where( s => prefix == s.Name.RemoveSpecialCharacters() )
                            .Select( s => (int?)s.Id ).FirstOrDefault();
                    }

                    var suffix = row[Suffix] as string;
                    if ( suffix != null )
                    {
                        suffix = suffix.RemoveSpecialCharacters().Trim();
                        person.SuffixValueId = suffixTypes.Where( s => suffix == s.Name.RemoveSpecialCharacters() )
                            .Select( s => (int?)s.Id ).FirstOrDefault();
                    }

                    var maritalStatus = row[MaritalStatus] as string;
                    if ( maritalStatus != null )
                    {
                        person.MaritalStatusValueId = maritalStatusTypes.Where( dv => dv.Name == maritalStatus )
                            .Select( dv => (int?)dv.Id ).FirstOrDefault();
                    }
                    else
                    {
                        person.MaritalStatusValueId = maritalStatusTypes.Where( dv => dv.Name == "Unknown" )
                            .Select( dv => (int?)dv.Id ).FirstOrDefault();
                    }

                    var familyRole = row[FamilyRole] as string;
                    if ( familyRole != null )
                    {
                        if ( familyRole == "Child" || person.Age < 18 )
                        {
                            groupRoleId = childRoleId;
                        }
                        else if ( familyRole == "Visitor" )
                        {
                            // assign person as a known relationship of this family/group
                        }
                    }

                    var connectionStatus = row[ConnectionStatus] as string;
                    if ( connectionStatus == "Member" )
                    {
                        person.ConnectionStatusValueId = connectionStatusTypes.FirstOrDefault( dv => dv.Guid == new Guid( Rock.SystemGuid.DefinedValue.PERSON_CONNECTION_STATUS_MEMBER ) ).Id;
                    }
                    else if ( connectionStatus == "Visitor" )
                    {
                        person.ConnectionStatusValueId = connectionStatusTypes.FirstOrDefault( dv => dv.Guid == new Guid( Rock.SystemGuid.DefinedValue.PERSON_CONNECTION_STATUS_VISITOR ) ).Id;
                    }
                    else if ( connectionStatus == "Deceased" )
                    {
                        person.IsDeceased = true;
                        person.RecordStatusReasonValueId = recordStatusReasons.Where( dv => dv.Name == "Deceased" )
                            .Select( dv => dv.Id ).FirstOrDefault();
                    }
                    else
                    {
                        // look for user-defined connection type or default to Attendee
                        var customConnectionType = connectionStatusTypes.Where( dv => dv.Name == connectionStatus )
                            .Select( dv => (int?)dv.Id ).FirstOrDefault();

                        int attendeeId = connectionStatusTypes.FirstOrDefault( dv => dv.Guid == new Guid( "39F491C5-D6AC-4A9B-8AC0-C431CB17D588" ) ).Id;
                        person.ConnectionStatusValueId = customConnectionType ?? attendeeId;
                    }

                    var recordStatus = row[RecordStatus] as string;
                    switch ( recordStatus.Trim() )
                    {
                        case "Active":
                            person.RecordStatusValueId = recordStatusActiveId;
                            break;

                        case "Inactive":
                            person.RecordStatusValueId = recordStatusInactiveId;
                            break;

                        default:
                            person.RecordStatusValueId = recordStatusPendingId;
                            break;
                    }

                    var campus = row[Campus] as string;
                    if ( campus != null )
                    {
                        familyGroup.CampusId = CampusList.Where( c => c.Name.StartsWith( campus ) )
                            .Select( c => (int?)c.Id ).FirstOrDefault();
                    }

                    // Map Person attributes
                    person.Attributes = new Dictionary<string, AttributeCache>();
                    person.AttributeValues = new Dictionary<string, List<AttributeValue>>();

                    var personIdValue = row[PersonId] as string;
                    if ( personIdValue != null )
                    {
                        person.Attributes.Add( importIdAttribute.Key, importIdAttribute );
                        person.AttributeValues.Add( importIdAttribute.Key, new List<AttributeValue>() );
                        person.AttributeValues[importIdAttribute.Key].Add( new AttributeValue()
                        {
                            AttributeId = importIdAttribute.Id,
                            Value = personIdValue,
                            Order = 0
                        } );
                    }

                    DateTime membershipDateValue;
                    if ( DateTime.TryParse( row[MembershipDate], out membershipDateValue ) )
                    {
                        person.Attributes.Add( membershipDateAttribute.Key, membershipDateAttribute );
                        person.AttributeValues.Add( membershipDateAttribute.Key, new List<AttributeValue>() );
                        person.AttributeValues[membershipDateAttribute.Key].Add( new AttributeValue()
                        {
                            AttributeId = membershipDateAttribute.Id,
                            Value = membershipDateValue.ToString(),
                            Order = 0
                        } );
                    }

                    DateTime baptismDateValue;
                    if ( DateTime.TryParse( row[BaptismDate], out baptismDateValue ) )
                    {
                        person.Attributes.Add( baptismDateAttribute.Key, baptismDateAttribute );
                        person.AttributeValues.Add( baptismDateAttribute.Key, new List<AttributeValue>() );
                        person.AttributeValues[baptismDateAttribute.Key].Add( new AttributeValue()
                        {
                            AttributeId = baptismDateAttribute.Id,
                            Value = baptismDateValue.ToString(),
                            Order = 0
                        } );
                    }

                    DateTime firstVisitValue;
                    if ( DateTime.TryParse( row[FirstVisit], out firstVisitValue ) )
                    {
                        person.Attributes.Add( firstVisitAttribute.Key, firstVisitAttribute );
                        person.AttributeValues.Add( firstVisitAttribute.Key, new List<AttributeValue>() );
                        person.AttributeValues[firstVisitAttribute.Key].Add( new AttributeValue()
                        {
                            AttributeId = firstVisitAttribute.Id,
                            Value = firstVisitValue.ToString(),
                            Order = 0
                        } );
                    }

                    var previousChurchValue = row[PreviousChurch] as string;
                    if ( previousChurchValue != null )
                    {
                        person.Attributes.Add( previousChurchAttribute.Key, previousChurchAttribute );
                        person.AttributeValues.Add( previousChurchAttribute.Key, new List<AttributeValue>() );
                        person.AttributeValues[previousChurchAttribute.Key].Add( new AttributeValue()
                        {
                            AttributeId = previousChurchAttribute.Id,
                            Value = previousChurchValue,
                            Order = 0
                        } );
                    }

                    var employerValue = row[Employer] as string;
                    if ( employerValue != null )
                    {
                        person.Attributes.Add( employerAttribute.Key, employerAttribute );
                        person.AttributeValues.Add( employerAttribute.Key, new List<AttributeValue>() );
                        person.AttributeValues[employerAttribute.Key].Add( new AttributeValue()
                        {
                            AttributeId = employerAttribute.Id,
                            Value = employerValue,
                            Order = 0
                        } );
                    }

                    var positionValue = row[Position] as string;
                    if ( positionValue != null )
                    {
                        person.Attributes.Add( positionAttribute.Key, positionAttribute );
                        person.AttributeValues.Add( positionAttribute.Key, new List<AttributeValue>() );
                        person.AttributeValues[positionAttribute.Key].Add( new AttributeValue()
                        {
                            AttributeId = positionAttribute.Id,
                            Value = positionValue,
                            Order = 0
                        } );
                    }

                    var schoolValue = row[School] as string;
                    if ( positionValue != null )
                    {
                        person.Attributes.Add( schoolAttribute.Key, schoolAttribute );
                        person.AttributeValues.Add( schoolAttribute.Key, new List<AttributeValue>() );
                        person.AttributeValues[schoolAttribute.Key].Add( new AttributeValue()
                        {
                            AttributeId = schoolAttribute.Id,
                            Value = schoolValue,
                            Order = 0
                        } );
                    }

                    var groupMember = new GroupMember();
                    groupMember.Person = person;
                    groupMember.GroupRoleId = groupRoleId;
                    groupMember.GroupMemberStatus = GroupMemberStatus.Active;
                    familyGroup.Members.Add( groupMember );

                    completed++;
                    if ( completed % ReportingNumber < 1 )
                    {
                        RockTransactionScope.WrapTransaction( () =>
                        {
                            var rockContext = new RockContext();
                            rockContext.Groups.AddRange( familyList );
                            rockContext.SaveChanges();

                            foreach ( var newFamilyGroup in familyList )
                            {
                                foreach ( var newFamilyMember in newFamilyGroup.Members )
                                {
                                    var newPerson = newFamilyMember.Person;
                                    foreach ( var attributeCache in newPerson.Attributes.Select( a => a.Value ) )
                                    {
                                        var newValue = newPerson.AttributeValues[attributeCache.Key].FirstOrDefault();
                                        if ( newValue != null )
                                        {
                                            newValue.EntityId = newPerson.Id;
                                            rockContext.AttributeValues.Add( newValue );
                                        }
                                    }

                                    if ( !newPerson.Aliases.Any( a => a.AliasPersonId == newPerson.Id ) )
                                    {
                                        newPerson.Aliases.Add( new PersonAlias
                                        {
                                            AliasPersonId = newPerson.Id,
                                            AliasPersonGuid = newPerson.Guid
                                        } );
                                    }

                                    if ( newFamilyMember.GroupRoleId != childRoleId )
                                    {
                                        newPerson.GivingGroupId = newFamilyGroup.Id;
                                    }
                                }
                            }

                            rockContext.SaveChanges();
                        } );

                        familyList.Clear();
                        ReportPartialProgress();
                    }
                }
            } while ( Database.ReadNextRecord() );

            ReportProgress( 100, string.Format( "Completed import: {0:N0} records imported.", completed ) );
        }

        #endregion

        #region Field Reference Constants

        private const int FamilyId = 1;
        private const int FamilyName = 2;
        private const int PersonId = 3;
        private const int RecordType = 4;
        private const int RecordStatus = 5;
        private const int Prefix = 6;
        private const int FirstName = 7;
        private const int NickName = 8;
        private const int LastName = 9;
        private const int Suffix = 10;
        private const int DateOfBirth = 11;
        private const int Gender = 12;
        private const int Email = 13;
        private const int IsEmailActive = 14;
        private const int ConnectionStatus = 15;
        private const int MaritalStatus = 16;
        private const int FamilyRole = 17;
        private const int HomePhone = 18;
        private const int MobilePhone = 19;
        private const int WorkPhone = 20;
        private const int Campus = 21;
        private const int AddressType = 22;
        private const int Street1 = 23;
        private const int Street2 = 24;
        private const int City = 25;
        private const int State = 26;
        private const int Zip = 27;
        private const int Country = 28;
        private const int Latitude = 29;
        private const int Longitude = 30;
        private const int MembershipDate = 31;
        private const int BaptismDate = 32;
        private const int FirstVisit = 33;
        private const int PreviousChurch = 34;
        private const int Employer = 35;
        private const int Position = 36;
        private const int School = 37;
        // Custom attributes added at 38, 39, ...

        #endregion
    }
}
