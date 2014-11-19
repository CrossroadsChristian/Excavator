﻿// <copyright>
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
using System.Linq;
using OrcaMDF.Core.MetaData;
using Rock;
using Rock.Data;
using Rock.Model;
using Rock.Web.Cache;

namespace Excavator.F1
{
    /// <summary>
    /// Partial of F1Component that holds the Email/Phone # import methods
    /// </summary>
    partial class F1Component
    {
        /// <summary>
        /// Maps the communication data.
        /// </summary>
        /// <param name="tableData">The table data.</param>
        /// <returns></returns>
        private void MapCommunication( IQueryable<Row> tableData )
        {
            var lookupContext = new RockContext();
            var personService = new PersonService( lookupContext );
            var attributeService = new AttributeService( lookupContext );

            var numberTypeValues = DefinedTypeCache.Read( new Guid( Rock.SystemGuid.DefinedType.PERSON_PHONE_TYPE ), lookupContext ).DefinedValues;

            // Look up additional Person attributes (existing)
            var personAttributes = attributeService.GetByEntityTypeId( PersonEntityTypeId ).ToList();

            // Remove previously defined Excavator social attributes & categories if they exist
            var oldFacebookAttribute = personAttributes.Where( a => a.Key == "FacebookUsername" ).FirstOrDefault();
            if ( oldFacebookAttribute != null )
            {
                Rock.Web.Cache.AttributeCache.Flush( oldFacebookAttribute.Id );
                attributeService.Delete( oldFacebookAttribute );
                lookupContext.SaveChanges( true );
            }

            var oldTwitterAttribute = personAttributes.Where( a => a.Key == "TwitterUsername" ).FirstOrDefault();
            if ( oldTwitterAttribute != null )
            {
                Rock.Web.Cache.AttributeCache.Flush( oldTwitterAttribute.Id );
                attributeService.Delete( oldTwitterAttribute );
                lookupContext.SaveChanges( true );
            }

            int attributeEntityTypeId = EntityTypeCache.Read( "Rock.Model.Attribute" ).Id;
            var socialMediaCategory = new CategoryService( lookupContext ).GetByEntityTypeId( attributeEntityTypeId )
                .Where( c => c.Name == "Social Media" &&
                    c.EntityTypeQualifierValue == PersonEntityTypeId.ToString() &&
                    c.IconCssClass == "fa fa-twitter" )
                .FirstOrDefault();
            if ( socialMediaCategory != null )
            {
                lookupContext.Categories.Remove( socialMediaCategory );
                lookupContext.SaveChanges( true );
            }

            // Cached Rock attributes: Facebook, Twitter, Instagram
            var twitterAttribute = AttributeCache.Read( personAttributes.FirstOrDefault( a => a.Key == "Twitter" ) );
            var facebookAttribute = AttributeCache.Read( personAttributes.FirstOrDefault( a => a.Key == "Facebook" ) );
            var instagramAttribute = AttributeCache.Read( personAttributes.FirstOrDefault( a => a.Key == "Instagram" ) );
            var secondaryEmailAttribute = AttributeCache.Read( SecondaryEmailAttributeId );

            var existingNumbers = new PhoneNumberService( lookupContext ).Queryable().ToList();

            var newNumberList = new List<PhoneNumber>();
            var updatedPersonList = new List<Person>();

            int completed = 0;
            int totalRows = tableData.Count();
            int percentage = ( totalRows - 1 ) / 100 + 1;
            ReportProgress( 0, string.Format( "Verifying communication import ({0:N0} found, {1:N0} already exist).", totalRows, existingNumbers.Count() ) );

            foreach ( var row in tableData )
            {
                string value = row["Communication_Value"] as string;
                int? individualId = row["Individual_ID"] as int?;
                int? householdId = row["Household_ID"] as int?;
                var personList = new List<int?>();

                if ( individualId != null )
                {
                    int? personId = GetPersonAliasId( individualId, householdId );
                    if ( personId != null )
                    {
                        personList.Add( personId );
                    }
                }
                else
                {
                    List<int?> personIds = GetFamilyByHouseholdId( householdId );
                    if ( personIds.Any() )
                    {
                        personList.AddRange( personIds );
                    }
                }

                if ( personList.Any() && !string.IsNullOrWhiteSpace( value ) )
                {
                    DateTime? lastUpdated = row["LastUpdatedDate"] as DateTime?;
                    string communicationComment = row["Communication_Comment"] as string;
                    string type = row["Communication_Type"] as string;
                    bool isListed = (bool)row["Listed"];
                    value = value.RemoveWhitespace();

                    // Communication value is a number
                    if ( type.Contains( "Phone" ) || type.Contains( "Mobile" ) )
                    {
                        var extension = string.Empty;
                        var countryCode = Rock.Model.PhoneNumber.DefaultCountryCode();
                        var normalizedNumber = string.Empty;
                        var countryIndex = value.IndexOf( '+' );
                        int extensionIndex = value.LastIndexOf( 'x' ) > 0 ? value.LastIndexOf( 'x' ) : value.Length;
                        if ( countryIndex >= 0 )
                        {
                            countryCode = value.Substring( countryIndex, countryIndex + 3 ).AsNumeric();
                            normalizedNumber = value.Substring( countryIndex + 3, extensionIndex - 3 ).AsNumeric();
                            extension = value.Substring( extensionIndex );
                        }
                        else if ( extensionIndex > 0 )
                        {
                            normalizedNumber = value.Substring( 0, extensionIndex ).AsNumeric();
                            extension = value.Substring( extensionIndex ).AsNumeric();
                        }
                        else
                        {
                            normalizedNumber = value.AsNumeric();
                        }

                        if ( !string.IsNullOrWhiteSpace( normalizedNumber ) )
                        {
                            foreach ( var familyPersonId in personList )
                            {
                                bool numberExists = existingNumbers.Any( n => n.PersonId == familyPersonId && n.Number.Equals( value ) );
                                if ( !numberExists )
                                {
                                    var newNumber = new PhoneNumber();
                                    newNumber.CreatedByPersonAliasId = ImportPersonAlias.Id;
                                    newNumber.ModifiedDateTime = lastUpdated;
                                    newNumber.PersonId = (int)familyPersonId;
                                    newNumber.IsMessagingEnabled = false;
                                    newNumber.CountryCode = countryCode;
                                    newNumber.IsUnlisted = !isListed;
                                    newNumber.Extension = extension.Left( 20 );
                                    newNumber.Number = normalizedNumber.Left( 20 );
                                    newNumber.Description = communicationComment;

                                    newNumber.NumberTypeValueId = numberTypeValues.Where( v => type.StartsWith( v.Value ) )
                                        .Select( v => (int?)v.Id ).FirstOrDefault();

                                    newNumberList.Add( newNumber );
                                    existingNumbers.Add( newNumber );
                                }
                            }

                            completed++;
                        }
                    }
                    else
                    {
                        var person = personService.Queryable( includeDeceased: true ).FirstOrDefault( p => p.Id == personList.FirstOrDefault() );
                        person.Attributes = new Dictionary<string, AttributeCache>();
                        person.AttributeValues = new Dictionary<string, AttributeValue>();

                        if ( value.IsValidEmail() )
                        {
                            string secondaryEmail = string.Empty;
                            if ( string.IsNullOrWhiteSpace( person.Email ) )
                            {
                                secondaryEmail = person.Email;
                                person.Email = value.Left( 75 );
                                person.IsEmailActive = isListed;
                                person.ModifiedDateTime = lastUpdated;
                                person.EmailNote = communicationComment;
                                lookupContext.SaveChanges( true );
                            }
                            else if ( !person.Email.Equals( value ) )
                            {
                                secondaryEmail = value;
                            }

                            var existingSecondaryEmail = new AttributeValueService( lookupContext ).Queryable().Where( av => av.AttributeId == SecondaryEmailAttributeId && av.EntityId == person.Id ).FirstOrDefault();

                            if ( !string.IsNullOrWhiteSpace( secondaryEmail ) && existingSecondaryEmail == null )
                            {
                                person.Attributes.Add( secondaryEmailAttribute.Key, secondaryEmailAttribute );
                                person.AttributeValues.Add( secondaryEmailAttribute.Key, new AttributeValue()
                                {
                                    AttributeId = secondaryEmailAttribute.Id,
                                    Value = secondaryEmail
                                } );
                            }
                        }
                        else if ( type.Contains( "Twitter" ) )
                        {
                            person.Attributes.Add( twitterAttribute.Key, twitterAttribute );
                            person.AttributeValues.Add( twitterAttribute.Key, new AttributeValue()
                            {
                                AttributeId = twitterAttribute.Id,
                                Value = value
                            } );
                        }
                        else if ( type.Contains( "Facebook" ) )
                        {
                            var existingFacebook = new AttributeValueService( lookupContext ).Queryable().Where( av => av.AttributeId == facebookAttribute.Id && av.EntityId == person.Id ).FirstOrDefault();
                            if ( existingFacebook == null )
                            {
                                person.Attributes.Add( facebookAttribute.Key, facebookAttribute );
                                person.AttributeValues.Add( facebookAttribute.Key, new AttributeValue()
                                {
                                    AttributeId = facebookAttribute.Id,
                                    Value = value
                                } );
                            }
                        }
                        else if ( type.Contains( "Instagram" ) )
                        {
                            person.Attributes.Add( instagramAttribute.Key, instagramAttribute );
                            person.AttributeValues.Add( instagramAttribute.Key, new AttributeValue()
                            {
                                AttributeId = instagramAttribute.Id,
                                Value = value
                            } );
                        }

                        updatedPersonList.Add( person );
                        completed++;
                    }

                    if ( completed % percentage < 1 )
                    {
                        int percentComplete = completed / percentage;
                        ReportProgress( percentComplete, string.Format( "{0:N0} records imported ({1}% complete).", completed, percentComplete ) );
                    }
                    else if ( completed % ReportingNumber < 1 )
                    {
                        SaveCommunication( newNumberList, updatedPersonList );

                        // reset so context doesn't bloat
                        lookupContext = new RockContext();
                        personService = new PersonService( lookupContext );
                        updatedPersonList.Clear();
                        newNumberList.Clear();
                        ReportPartialProgress();
                    }
                }
            }

            if ( newNumberList.Any() || updatedPersonList.Any() )
            {
                SaveCommunication( newNumberList, updatedPersonList );
            }

            ReportProgress( 100, string.Format( "Finished communication import: {0:N0} records imported.", completed ) );
        }

        /// <summary>
        /// Saves the communication.
        /// </summary>
        /// <param name="newNumberList">The new number list.</param>
        /// <param name="updatedPersonList">The updated person list.</param>
        private static void SaveCommunication( List<PhoneNumber> newNumberList, List<Person> updatedPersonList )
        {
            var rockContext = new RockContext();
            rockContext.WrapTransaction( () =>
            {
                rockContext.Configuration.AutoDetectChangesEnabled = false;
                rockContext.PhoneNumbers.AddRange( newNumberList );
                rockContext.SaveChanges( DisableAudit );

                var newAttributeValues = new List<AttributeValue>();
                foreach ( var updatedPerson in updatedPersonList.Where( p => p.Attributes.Any() ) )
                {
                    foreach ( var attributeCache in updatedPerson.Attributes.Select( a => a.Value ) )
                    {
                        var newValue = updatedPerson.AttributeValues[attributeCache.Key];
                        if ( newValue != null )
                        {
                            newValue.EntityId = updatedPerson.Id;
                            newAttributeValues.Add( newValue );
                        }
                    }
                }

                rockContext.AttributeValues.AddRange( newAttributeValues );
                rockContext.SaveChanges( DisableAudit );
            } );
        }
    }
}
