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
using System.Linq;
using OrcaMDF.Core.MetaData;
using Rock.Data;
using Rock.Model;
using Rock.Web.Cache;

namespace Excavator.F1
{
    partial class F1Component
    {
        /// <summary>
        /// Maps the activity ministry.
        /// </summary>
        /// <param name="tableData">The table data.</param>
        /// <returns></returns>
        private void MapActivityMinistry(IQueryable<Row> tableData)
        {
            var lookupContext = new RockContext();
            var rockContext = new RockContext();

            // Add an Attribute for the unique F1 Ministry Id
            int groupEntityTypeId = EntityTypeCache.Read("Rock.Model.Group").Id;
            var ministryAttributeId = new AttributeService(lookupContext).Queryable().Where(a => a.EntityTypeId == groupEntityTypeId
                && a.Key == "F1MinistryId").Select(a => a.Id).FirstOrDefault();
            var activityAttributeId = new AttributeService(lookupContext).Queryable().Where(a => a.EntityTypeId == groupEntityTypeId
                && a.Key == "F1ActivityId").Select(a => a.Id).FirstOrDefault();

            if (ministryAttributeId == 0)
            {
                var newMinistryAttribute = new Rock.Model.Attribute();
                newMinistryAttribute.Key = "F1MinistryId";
                newMinistryAttribute.Name = "F1 Ministry Id";
                newMinistryAttribute.FieldTypeId = IntegerFieldTypeId;
                newMinistryAttribute.EntityTypeId = groupEntityTypeId;
                newMinistryAttribute.EntityTypeQualifierValue = string.Empty;
                newMinistryAttribute.EntityTypeQualifierColumn = string.Empty;
                newMinistryAttribute.Description = "The FellowshipOne identifier for the ministry that was imported";
                newMinistryAttribute.DefaultValue = string.Empty;
                newMinistryAttribute.IsMultiValue = false;
                newMinistryAttribute.IsRequired = false;
                newMinistryAttribute.Order = 0;

                lookupContext.Attributes.Add(newMinistryAttribute);
                lookupContext.SaveChanges(DisableAudit);
                ministryAttributeId = newMinistryAttribute.Id;
            }
            if (activityAttributeId == 0)
            {
                var newActivityAttribute = new Rock.Model.Attribute();
                newActivityAttribute.Key = "F1ActivityId";
                newActivityAttribute.Name = "F1 Activity Id";
                newActivityAttribute.FieldTypeId = IntegerFieldTypeId;
                newActivityAttribute.EntityTypeId = groupEntityTypeId;
                newActivityAttribute.EntityTypeQualifierValue = string.Empty;
                newActivityAttribute.EntityTypeQualifierColumn = string.Empty;
                newActivityAttribute.Description = "The FellowshipOne identifier for the activity that was imported";
                newActivityAttribute.DefaultValue = string.Empty;
                newActivityAttribute.IsMultiValue = false;
                newActivityAttribute.IsRequired = false;
                newActivityAttribute.Order = 0;

                lookupContext.Attributes.Add(newActivityAttribute);
                lookupContext.SaveChanges(DisableAudit);
                activityAttributeId = newActivityAttribute.Id;
            }

            // Get previously imported Ministries
            List<AttributeValue> importedMinistriesAVList = new AttributeValueService(lookupContext).GetByAttributeId(ministryAttributeId).ToList();

            // Get previously imported Activities
            List<AttributeValue> importedActivitiesAVList = new AttributeValueService( lookupContext ).GetByAttributeId( activityAttributeId ).ToList();


            int importedMinistriesCount = 0;
            int importedActivitiesCount = 0;

            if ( importedMinistriesAVList.Any() ) { importedMinistriesCount = importedMinistriesAVList.Count(); }
            if ( importedActivitiesAVList.Any() ) { importedActivitiesCount = importedActivitiesAVList.Count(); }

            int completed = 0;
            int totalRows = tableData.Count();
            int percentage = (totalRows - 1) / 100 + 1;

            ReportProgress(0, string.Format("Verifying ministry import ({0:N0} found).", totalRows));
            ReportProgress(0, string.Format("Previously Imported Ministries ({0:N0} found).", importedMinistriesCount));
            ReportProgress(0, string.Format("Previously Imported Activities ({0:N0} found).", importedActivitiesCount));

            var newAreas = new List<GroupType>();
            var newCategories = new List<GroupType>();

            foreach (var row in tableData)
            {
                int? ministryId = row["Ministry_ID"] as int?;
                string ministryName = row["Ministry_Name"] as string;
                string ministryIdString = Convert.ToString( ministryId );

                //GroupType importedMinistriesGTList = new GroupTypeService( lookupContext ).Queryable().Where( g => g.Name == ministryName && g.ForeignId == ( Convert.ToString( ministryId ) + 'm' ) ).FirstOrDefault();
                int? importedMinistry = new AttributeValueService( lookupContext ).Queryable().Where( a => a.Value == ministryIdString && a.ForeignId == ministryName ).Select( a => a.Id ).FirstOrDefault();
                //AttributeValue importedMinistry = new AttributeValueService(lookupContext).Queryable().Where(a => a.Value == Convert.ToString(ministryId) && a.ForeignId == ministryName).FirstOrDefault();
                //AttributeValue importedMinistry = importedMinistriesAVList.Where(av => av.Value == Convert.ToString(ministryId)).FirstOrDefault();
               // if (ministryId != null && !importedMinistries.ContainsKey(ministryId)) //Checks AttributeValue table to see if it has already been imported.
                //if ( ministryId != null && importedMinistriesAVList.Find( x => x.Value == Convert.ToString( ministryId ) ) == null ) //Checks AttributeValue table to see if it has already been imported.
                if ( ministryId != null && importedMinistry == 0) //Checks AttributeValue table to see if it has already been imported.

                {
                    
                    bool? ministryIsActive = row["Ministry_Active"] as bool?;

                    if (ministryName != null)
                    {

                        var ministryCategory = new GroupType();

                        var ministryAttribute = new AttributeValue();
                        var ministryAttributeList = new List<AttributeValue>();

                        //Creates the GroupType data
                        ministryCategory.IsSystem = false;
                        ministryCategory.Name = ministryName.Trim();
                        ministryCategory.GroupTerm = "Group";
                        ministryCategory.GroupMemberTerm = "Member";
                        ministryCategory.AllowMultipleLocations = false;
                        ministryCategory.ShowInGroupList = false;
                        ministryCategory.ShowInNavigation = false;
                        ministryCategory.TakesAttendance = false;
                        ministryCategory.AttendanceRule = 0;
                        ministryCategory.AttendancePrintTo = 0;
                        ministryCategory.Order = 0;
                        ministryCategory.LocationSelectionMode = 0;
                        ministryCategory.Guid = Guid.NewGuid();
                        ministryCategory.ForeignId = ministryId.ToString(); //F1 Ministry ID - adding 'm' just incase there is an activity with the same ID
                        ministryCategory.GroupTypePurposeValueId = DefinedValueCache.Read(Rock.SystemGuid.DefinedValue.GROUPTYPE_PURPOSE_CHECKIN_TEMPLATE).Id; // ID = 142 in my db

                        //Creates the AttributeValue data for the Ministry
                        ministryAttribute.IsSystem = false;
                        ministryAttribute.AttributeId = ministryAttributeId;
                        ministryAttribute.Value = ministryId.ToString();    //So the Value is the F1MinistryID
                        ministryAttribute.Guid = Guid.NewGuid();
                        ministryAttribute.ForeignId = ministryName.Trim();

                        newCategories.Add(ministryCategory);
                        ministryAttributeList.Add(ministryAttribute);

                        //Saves it to the DB so that I can check for its ID in the table
                        if ( newCategories.Any() || ministryAttributeList.Any() )
                        {
                            //var rockContext = new RockContext();
                            if ( newCategories.Any() )
                            {
                                //var rockContext = new RockContext();
                                rockContext.WrapTransaction( () =>
                                {
                                    rockContext.Configuration.AutoDetectChangesEnabled = false;
                                    rockContext.GroupTypes.AddRange( newCategories );
                                    rockContext.SaveChanges( DisableAudit );
                                } );
                                newCategories.Clear();
                            }
                            if ( ministryAttributeList.Any() )
                            {
                                //var rockContext = new RockContext();
                                rockContext.WrapTransaction( () =>
                                {
                                    rockContext.Configuration.AutoDetectChangesEnabled = false;
                                    rockContext.AttributeValues.AddRange( ministryAttributeList );
                                    rockContext.SaveChanges( DisableAudit );
                                } );
                                ministryAttributeList.Clear();
                            }
                        }
                    }
                }




                //Checks AttributeValue table to see if it has already been imported.
                int? activityId = row["Activity_ID"] as int?;
                string activityName = row["Activity_Name"] as string;
                string activityIdstring = Convert.ToString( activityId );
                ReportProgress( 0, string.Format( "Ministry ID {0}   Activity ID {1}.", ministryId, activityId ));

                //if (activityId != null && !importedActivities.ContainsKey(activityId))
                int? importedActivity = new AttributeValueService( lookupContext ).Queryable().Where( a => a.Value == activityIdstring && a.ForeignId == activityName ).Select(a => a.Id).FirstOrDefault();
                //AttributeValue importedActivity = new AttributeValueService( lookupContext ).Queryable().Where( a => a.Value == Convert.ToString( activityId ) && a.ForeignId == activityName ).FirstOrDefault();
                //AttributeValue importedActivity = importedActivitiesAVList.Where( av => av.Value == Convert.ToString( activityId ) ).FirstOrDefault();
                //if ( activityId != null && importedActivitiesAVList.Find(x => x.Value == Convert.ToString(activityId)) == null )
                if ( activityId != null && importedActivity == 0 )
                {

                    
                    bool? activityIsActive = row["Activity_Active"] as bool?;

                    //Looking up the Ministry GroupType ID so it can be used as the ParentGroupTypeId for the Activity GroupType/Area
                    var gtService = new GroupTypeService(lookupContext);
                    int parentGroupTypeId;

                        string ministryID = ministryId.ToString();
                        parentGroupTypeId = gtService.Queryable().Where(gt => gt.ForeignId == ministryID).FirstOrDefault().Id; 

                    var parentGroupType = new GroupTypeService(rockContext).Get(parentGroupTypeId);

                    var activityArea = new GroupType();
                    var activityAV = new AttributeValue();
                    var activityAVList = new List<AttributeValue>();
                    

                    // create new GroupType for activity (will set this as child to Ministry/Category)
                    activityArea.IsSystem = false;
                    activityArea.Name = activityName.Trim();
                    activityArea.GroupTerm = "Group";
                    activityArea.GroupMemberTerm = "Member";
                    activityArea.AllowMultipleLocations = true;
                    activityArea.ShowInGroupList = false;
                    activityArea.ShowInNavigation = false;
                    activityArea.TakesAttendance = true;
                    activityArea.AttendanceRule = 0;
                    activityArea.AttendancePrintTo = 0;
                    activityArea.Order = 0;
                    activityArea.LocationSelectionMode = 0;
                    activityArea.Guid = Guid.NewGuid();
                    activityArea.ForeignId = activityId.ToString();  //F1 Activity ID

                    //Sets GroupTypeAssociation for the Categories and Areas
                        activityArea.ParentGroupTypes = new List<GroupType>();
                        activityArea.ParentGroupTypes.Add(parentGroupType);


                    //Create Activity AttributeValue Data
                    activityAV.IsSystem = false;
                    activityAV.Guid = Guid.NewGuid();
                    activityAV.AttributeId = activityAttributeId;
                    activityAV.Value = activityId.ToString();
                    activityAV.ForeignId = activityName;

                    activityAVList.Add(activityAV);
                    newAreas.Add(activityArea);
                    completed++;

                    if ( newAreas.Any() || activityAVList.Any() )
                    {
                        //var rockContext = new RockContext();
                        if ( newAreas.Any() )
                        {
                            rockContext.WrapTransaction( () =>
                            {
                                rockContext.Configuration.AutoDetectChangesEnabled = false;
                                rockContext.GroupTypes.AddRange( newAreas );
                                rockContext.SaveChanges( DisableAudit );
                            } );
                            newAreas.Clear();
                        }
                        if ( activityAVList.Any() )
                        {
                            //var rockContext = new RockContext();
                            rockContext.WrapTransaction( () =>
                            {
                                rockContext.Configuration.AutoDetectChangesEnabled = false;
                                rockContext.AttributeValues.AddRange( activityAVList );
                                rockContext.SaveChanges( DisableAudit );
                            } );
                            activityAVList.Clear();
                        }
                    }
                }
                if (completed % percentage < 1)
                {
                    int percentComplete = completed / percentage;
                    ReportProgress(percentComplete, string.Format("{0:N0} ministries imported ({1}% complete). Categories: {2:N0} Areas: {3:N0}", completed, percentComplete, newCategories.Count, newAreas.Count));
                }
                else if (completed % ReportingNumber < 1)
                {
                    //var rockContext = new RockContext();
                    //rockContext.WrapTransaction(() =>
                    //{
                    //    rockContext.Configuration.AutoDetectChangesEnabled = false;
                    //    rockContext.GroupTypes.AddRange(newCategories);
                    //    rockContext.GroupTypes.AddRange(newAreas);
                    //    rockContext.SaveChanges(DisableAudit);
                    //});

                    ReportPartialProgress();
                }
            }
            //if (newAreas.Any())
            //{
            //    //var rockContext = new RockContext();
            //    rockContext.WrapTransaction(() =>
            //    {
            //        rockContext.Configuration.AutoDetectChangesEnabled = false;
            //        rockContext.GroupTypes.AddRange(newAreas);
            //        rockContext.SaveChanges(DisableAudit);
            //    });
            //}
            //ReportProgress(100, string.Format("Finished ministry import: {0:N0} ministries imported. Categories: {1}  Areas: {2}", completed,importedMinistriesAVList.Count(), importedActivitiesAVList.Count()));
            ReportProgress( 0, string.Format( "Categories: {0}  Areas: {1}", importedMinistriesAVList.Count(), importedActivitiesAVList.Count() ) );

        }

        /// <summary>
        /// Maps the RLC data to rooms, locations & classes -- Mapping RLC Names as groups under check-in
        /// </summary>
        /// <param name="tableData">The table data.</param>
        /// <returns></returns>
        private void MapRLC(IQueryable<Row> tableData)
        {
            var lookupContext = new RockContext();

            // Add an Attribute for the unique F1 Ministry Id
            int groupEntityTypeId = EntityTypeCache.Read("Rock.Model.Group").Id;
            var rlcAttributeId = new AttributeService(lookupContext).Queryable().Where(a => a.EntityTypeId == groupEntityTypeId
                && a.Key == "F1RLCId").Select(a => a.Id).FirstOrDefault();

            if (rlcAttributeId == 0)
            {
                var newRLCAttribute = new Rock.Model.Attribute();
                newRLCAttribute.Key = "F1RLCId";
                newRLCAttribute.Name = "F1 RLC Id";
                newRLCAttribute.FieldTypeId = IntegerFieldTypeId;
                newRLCAttribute.EntityTypeId = groupEntityTypeId;
                newRLCAttribute.EntityTypeQualifierValue = string.Empty;
                newRLCAttribute.EntityTypeQualifierColumn = string.Empty;
                newRLCAttribute.Description = "The FellowshipOne identifier for the RLC Group that was imported";
                newRLCAttribute.DefaultValue = string.Empty;
                newRLCAttribute.IsMultiValue = false;
                newRLCAttribute.IsRequired = false;
                newRLCAttribute.Order = 0;

                lookupContext.Attributes.Add(newRLCAttribute);
                lookupContext.SaveChanges(DisableAudit);
                rlcAttributeId = newRLCAttribute.Id;
            }

            // Get previously imported Ministries
            //var importedRLCs = new AttributeValueService(lookupContext).GetByAttributeId(rlcAttributeId)
            //    .Select(av => new { RLCId = av.Value.AsType<int?>(), RLCName = av.ForeignId })
            //    .ToDictionary(t => t.RLCId, t => t.RLCName);

            List<AttributeValue> importedRLCAVList = new AttributeValueService( lookupContext ).GetByAttributeId( rlcAttributeId ).ToList();

            var newRLCGroupList = new List<Group>();

            var rlcAttributeValueList = new List<AttributeValue>();

            //Need a list of GroupTypes                              
            var gtService = new GroupTypeService(lookupContext);
            var existingGroupTypes = new List<GroupType>();
            existingGroupTypes = gtService.Queryable().ToList();

            int completed = 0;
            int totalRows = tableData.Count();
            int percentage = (totalRows - 1) / 100 + 1;
            ReportProgress(0, string.Format("Verifying RLC Group import ({0:N0} found).", totalRows));

            foreach (var row in tableData)
            {
                int? rlcId = row["RLC_ID"] as int?;
                string rlcName = row["RLC_Name"] as string;
                string rlcStringId = Convert.ToString(rlcId);

                int? importedRLCs = new AttributeValueService( lookupContext ).Queryable().Where( a => a.Value == rlcStringId && a.ForeignId == rlcName ).Select( a => a.Id ).FirstOrDefault();

                if (rlcId != null && importedRLCs == 0)
                {

                    if (rlcName != null)
                    {
                        bool? rlcIsActive = row["Is_Active"] as bool?;
                        string roomName = row["Room_Name"] as string;
                        string maxCapacity = row["Max_Capacity"] as string;
                        string roomDescription = row["Room_Name"] as string;
                        string roomCode = row["Room_Code"] as string;
                        DateTime? startAgeDate = row["Start_Age_Date"] as DateTime?;
                        DateTime? endAgeDate = row["End_Age_Date"] as DateTime?;
                        int? activityId = row["Activity_ID"] as int?;

                        //Searches for Parent ActivityId
                        string activityStringId = activityId.ToString();
                        GroupType parentActivityArea = existingGroupTypes.Where(gt => gt.ForeignId == activityStringId).FirstOrDefault();

                        if ( String.IsNullOrWhiteSpace( rlcName ) )
                        {
                            ReportProgress( 0, string.Format( "." ) );
                            rlcName = "Excavator Test";
                        }
                        var rlcGroup = new Group();
                        bool rlcIsActiveBool = (bool)rlcIsActive;

                        //Sets the Group values for RLC
                        rlcGroup.IsSystem = false;
                        rlcGroup.Name = rlcName.Trim();
                        rlcGroup.Order = 0;
                        //rlcGroup.Guid = new Guid();
                        rlcGroup.GroupTypeId = parentActivityArea.Id;
                        rlcGroup.IsActive = rlcIsActiveBool;
                        rlcGroup.Description = roomDescription;
                        rlcGroup.ForeignId = rlcStringId;

                        var rlcAttributeValue = new AttributeValue();

                        //Sets the Attribute Values for RLC
                        rlcAttributeValue.IsSystem = false;
                        rlcAttributeValue.AttributeId = rlcAttributeId;
                        rlcAttributeValue.Value = rlcStringId;
                        rlcAttributeValue.ForeignId = rlcName.Trim();

                        rlcAttributeValueList.Add(rlcAttributeValue);
                        newRLCGroupList.Add(rlcGroup);
                        completed++;

                        // Adds rlcGroup to newRLCGroup list
                        ReportProgress(0, string.Format("Parent Activity/Area: {1}  Group Added: {0}  IsActive: {2}.", rlcGroup.Name, parentActivityArea.Name, rlcGroup.IsActive));

                        if (completed % percentage < 1)
                        {
                            int percentComplete = completed / percentage;
                            ReportProgress(percentComplete, string.Format("{0:N0} RLC/Groups imported ({1}% complete). ", completed, percentComplete));
                        }
                        else if (completed % ReportingNumber < 1)
                        {
                            var rockContext = new RockContext();
                            rockContext.WrapTransaction(() =>
                            {
                                rockContext.Configuration.AutoDetectChangesEnabled = false;
                                rockContext.AttributeValues.AddRange(rlcAttributeValueList);
                                rockContext.Groups.AddRange(newRLCGroupList);
                                rockContext.SaveChanges(DisableAudit);
                                newRLCGroupList.Clear();
                                rlcAttributeValueList.Clear();
                            });


                            ReportPartialProgress();
                        }
                    }
                }
            }

            if (newRLCGroupList.Any())
            {
                var rockContext = new RockContext();
                rockContext.WrapTransaction(() =>
                {
                    rockContext.Configuration.AutoDetectChangesEnabled = false;
                    rockContext.AttributeValues.AddRange(rlcAttributeValueList);
                    rockContext.Groups.AddRange(newRLCGroupList);
                    rockContext.SaveChanges(DisableAudit);
                    newRLCGroupList.Clear();
                    rlcAttributeValueList.Clear();
                });
            }
            ReportProgress(100, string.Format("Finished ministry and activity import: {0:N0} imported.", completed));
        }

        /// <summary>
        /// Maps the family address.
        /// </summary>
        /// <param name="tableData">The table data.</param>
        /// <returns></returns>
        private void MapFamilyAddress(IQueryable<Row> tableData)
        {
            var lookupContext = new RockContext();
            var lookupService = new LocationService(lookupContext);

            List<DefinedValue> groupLocationTypeList = new DefinedValueService(lookupContext).GetByDefinedTypeGuid(new Guid(Rock.SystemGuid.DefinedType.GROUP_LOCATION_TYPE)).ToList();

            List<GroupMember> groupMembershipList = new GroupMemberService(lookupContext).Queryable().Where(gm => gm.Group.GroupType.Guid == new Guid(Rock.SystemGuid.GroupType.GROUPTYPE_FAMILY)).ToList();

            int homeGroupLocationTypeId = groupLocationTypeList.FirstOrDefault(dv => dv.Guid == new Guid(Rock.SystemGuid.DefinedValue.GROUP_LOCATION_TYPE_HOME)).Id;
            int workGroupLocationTypeId = groupLocationTypeList.FirstOrDefault(dv => dv.Guid == new Guid(Rock.SystemGuid.DefinedValue.GROUP_LOCATION_TYPE_WORK)).Id;
            int previousGroupLocationTypeId = groupLocationTypeList.FirstOrDefault(dv => dv.Guid == new Guid(Rock.SystemGuid.DefinedValue.GROUP_LOCATION_TYPE_PREVIOUS)).Id;

            var newGroupLocations = new List<GroupLocation>();

            int completed = 0;
            int totalRows = tableData.Count();
            int percentage = (totalRows - 1) / 100 + 1;
            ReportProgress(0, string.Format("Verifying address import ({0:N0} found).", totalRows));

            foreach (var row in tableData)
            {
                int? individualId = row["Individual_ID"] as int?;
                int? householdId = row["Household_ID"] as int?;
                int? associatedPersonId = GetPersonId(individualId, householdId);

                if (associatedPersonId != null)
                {
                    var familyGroup = groupMembershipList.Where(gm => gm.PersonId == (int)associatedPersonId)
                        .Select(gm => gm.Group).FirstOrDefault();

                    if ( familyGroup != null )
                    {
                        var groupLocation = new GroupLocation();

                        string street1 = row["Address_1"] as string;
                        string street2 = row["Address_2"] as string;
                        string city = row["City"] as string;
                        string state = row["State"] as string;
                        string country = row["country"] as string; // NOT A TYPO: F1 has property in lower-case
                        string zip = row["Postal_Code"] as string;

                        /* Use CheckAddress.Get instead of Rock.Model.LocationService.Get (more details below) */
                        //Location familyAddress = CheckAddress.Get( street1, street2, city, state, zip, DisableAudit );
                        Location familyAddress = lookupService.Get( street1, street2, city, state, zip, country );

                        if ( familyAddress != null )
                        {
                            familyAddress.CreatedByPersonAliasId = ImportPersonAlias.Id;
                            familyAddress.Name = familyGroup.Name;
                            familyAddress.IsActive = true;

                            groupLocation.GroupId = familyGroup.Id;
                            groupLocation.LocationId = familyAddress.Id;
                            groupLocation.IsMailingLocation = true;
                            groupLocation.IsMappedLocation = true;

                            string addressType = row["Address_Type"] as string;

                            if ( addressType.Equals( "Primary" ) )
                            {
                                groupLocation.GroupLocationTypeValueId = homeGroupLocationTypeId;
                            }
                            else if ( addressType.Equals( "Business" ) || addressType.Equals( "Org" ) )
                            {
                                groupLocation.GroupLocationTypeValueId = workGroupLocationTypeId;
                            }
                            else if ( addressType.Equals( "Previous" ) )
                            {
                                groupLocation.GroupLocationTypeValueId = previousGroupLocationTypeId;
                            }
                            else if ( !string.IsNullOrEmpty( addressType ) )
                            {
                                groupLocation.GroupLocationTypeValueId = groupLocationTypeList.Where( dv => dv.Value.Equals( addressType ) )
                                    .Select( dv => (int?)dv.Id ).FirstOrDefault();
                            }

                            newGroupLocations.Add( groupLocation );
                            completed++;

                            if ( completed % percentage < 1 )
                            {
                                int percentComplete = completed / percentage;
                                ReportProgress( percentComplete, string.Format( "{0:N0} addresses imported ({1}% complete).", completed, percentComplete ) );
                            }
                            else if ( completed % ReportingNumber < 1 )
                            {
                                var rockContext = new RockContext();
                                rockContext.WrapTransaction( () =>
                                {
                                    rockContext.Configuration.AutoDetectChangesEnabled = false;
                                    rockContext.GroupLocations.AddRange( newGroupLocations );
                                    rockContext.ChangeTracker.DetectChanges();
                                    rockContext.SaveChanges( DisableAudit );
                                } );

                                newGroupLocations.Clear();
                                lookupContext = new RockContext();
                                lookupService = new LocationService( lookupContext );
                                ReportPartialProgress();
                            }
                        }
                    }
                }
            }

            if (newGroupLocations.Any())
            {
                var rockContext = new RockContext();
                rockContext.WrapTransaction(() =>
                {
                    rockContext.Configuration.AutoDetectChangesEnabled = false;
                    rockContext.GroupLocations.AddRange(newGroupLocations);
                    rockContext.SaveChanges(DisableAudit);
                });
            }

            ReportProgress(100, string.Format("Finished address import: {0:N0} addresses imported.", completed));
        }
    }

    internal struct CheckAddress
    {
        /// <summary>
        /// Modified version of Rock.Model.LocationService.Get()
        /// to get or set the specified location in the database
        /// (minus the call for address verification)
        /// </summary>
        /// <param name="street1">The street1.</param>
        /// <param name="street2">The street2.</param>
        /// <param name="city">The city.</param>
        /// <param name="state">The state.</param>
        /// <param name="zip">The zip.</param>
        /// <returns></returns>
        public static Location Get(string street1, string street2, string city, string state, string zip, bool DisableAudit = false)
        {
            // Create a new context/service so that save does not affect calling method's context
            var rockContext = new RockContext();
            var locationService = new LocationService(rockContext);

            // Make sure it's not an empty address
            if (string.IsNullOrWhiteSpace(street1) &&
                string.IsNullOrWhiteSpace(street2) &&
                string.IsNullOrWhiteSpace(city) &&
                string.IsNullOrWhiteSpace(state) &&
                string.IsNullOrWhiteSpace(zip))
            {
                return null;
            }

            // First check if a location exists with the entered values
            Location existingLocation = locationService.Queryable().FirstOrDefault(t =>
                (t.Street1 == street1 || (street1 == null && t.Street1 == null)) &&
                (t.Street2 == street2 || (street2 == null && t.Street2 == null)) &&
                (t.City == city || (city == null && t.City == null)) &&
                (t.State == state || (state == null && t.State == null)) &&
                (t.PostalCode == zip || (zip == null && t.PostalCode == null)));
            if (existingLocation != null)
            {
                return existingLocation;
            }

            // If existing location wasn't found with entered values, try standardizing the values, and
            // search for an existing value again
            var newLocation = new Location
            {
                Street1 = street1,
                Street2 = street2,
                City = city,
                State = state,
                PostalCode = zip
            };

            // uses MEF to look for verification providers (which Excavator doesn't have)
            // Verify( newLocation, false );

            existingLocation = locationService.Queryable().FirstOrDefault(t =>
                (t.Street1 == newLocation.Street1 || (newLocation.Street1 == null && t.Street1 == null)) &&
                (t.Street2 == newLocation.Street2 || (newLocation.Street2 == null && t.Street2 == null)) &&
                (t.City == newLocation.City || (newLocation.City == null && t.City == null)) &&
                (t.State == newLocation.State || (newLocation.State == null && t.State == null)) &&
                (t.PostalCode == newLocation.PostalCode || (newLocation.PostalCode == null && t.PostalCode == null)));

            if (existingLocation != null)
            {
                return existingLocation;
            }

            locationService.Add(newLocation);
            rockContext.SaveChanges(DisableAudit);

            // refetch it from the database to make sure we get a valid .Id
            return locationService.Get(newLocation.Guid);
        }
    }
}
