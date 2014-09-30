// <copyright> new
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
    /// <summary>
    /// Partial of F1Component that holds the People import methods
    /// </summary>
    partial class F1Component
    {
        /// <summary>
        /// Maps the Connect Groups.
        /// </summary>
        /// <param name="tableData">The table data.</param>
        /// <returns></returns>
        private void MapGroups( IQueryable<Row> tableData ) //Just mapping Connect Groups and not People Lists (Wonder if People lists could be Tags?)
        {
            var lookupContext = new RockContext();
            int completedMembers = 0;
            int completedGroups = 0;
            int completedLifeStages = 0;
            int totalRows = tableData.Count();
            int percentage = ( totalRows - 1 ) / 100 + 1;
            ReportProgress( 0, string.Format( "Verifying group import ({0:N0} found. Total may vary based on Group Type Name).", totalRows ) );

            var rockContext = new RockContext();

            foreach ( var row in tableData )
            {
                string groupTypeName = row["Group_Type_Name"] as string;
                if ( groupTypeName.Trim() == "Connect Groups" )
                {

                    var groupTypeIdSection = new GroupTypeService( lookupContext ).Queryable().Where( gt => gt.Name == "Event/Serving/Small Group Section" ).Select( a => a.Id ).FirstOrDefault();
                    var connectGroupsId = new GroupService( lookupContext ).Queryable().Where( g => g.Name == "Connect Groups" && g.GroupTypeId == groupTypeIdSection ).Select( a => a.Id ).FirstOrDefault();
                    var groupTypeIdSmallGroup = new GroupTypeService( lookupContext ).Queryable().Where( gt => gt.Name == "Small Group" ).Select( a => a.Id ).FirstOrDefault();

                    string groupName = row["Group_Name"] as string;
                    int? groupId = row["Group_ID"] as int?;
                    int? individualId = row["Individual_ID"] as int?;
                    int? personId = GetPersonId( individualId );
                    DateTime? createdDateTime = row["Created_Date"] as DateTime?;


                    //Check to see if Head of Connect Group Tree exists

                    //If it doesn't exist
                    if ( connectGroupsId == null || connectGroupsId == 0 )
                    {
                        //Create one.
                        var connectGroupTree = new Group();
                        connectGroupTree.IsSystem = false;
                        connectGroupTree.GroupTypeId = groupTypeIdSection;
                        connectGroupTree.CampusId = 1;
                        connectGroupTree.Name = "Connect Groups";
                        connectGroupTree.Description = "Crossroads Connect Group Ministry";
                        connectGroupTree.IsActive = true;
                        //connectGroupTree.Order = 0;
                        connectGroupTree.CreatedByPersonAliasId = 1;
                        connectGroupTree.CreatedDateTime = DateTime.Now;

                        //save group
                        rockContext.WrapTransaction( () =>
                        {
                            rockContext.Configuration.AutoDetectChangesEnabled = false;
                            rockContext.Groups.Add( connectGroupTree );
                            rockContext.SaveChanges( DisableAudit );
                        } );
                    }

                    //check to see if life stage exists
                    //getting the life stage name
                    string lifeStage = groupName;
                    int index = lifeStage.IndexOf( "-" );
                    if ( index > 0 )
                        lifeStage = lifeStage.Substring( 0, index ).Trim();

                    //checks to see if it exists
                    int existingLifeStage = new GroupService( lookupContext ).Queryable().Where( g => g.Name == lifeStage ).Select( a => a.Id ).FirstOrDefault();
                    if ( existingLifeStage == null || existingLifeStage == 0 )
                    {
                        //Create one.
                        var connectGroupsLifeStage = new Group();
                        connectGroupsLifeStage.IsSystem = false;
                        connectGroupsLifeStage.ParentGroupId = connectGroupsId;
                        connectGroupsLifeStage.GroupTypeId = groupTypeIdSection;
                        connectGroupsLifeStage.CampusId = 1;
                        connectGroupsLifeStage.Name = lifeStage;
                        connectGroupsLifeStage.Description = "";
                        connectGroupsLifeStage.IsActive = true;
                        //connectGroupsLifeStage.Order = 0;
                        connectGroupsLifeStage.CreatedByPersonAliasId = 1;
                        connectGroupsLifeStage.CreatedDateTime = DateTime.Now;

                        //save Life Stage

                        rockContext.WrapTransaction( () =>
                        {
                            rockContext.Configuration.AutoDetectChangesEnabled = false;
                            rockContext.Groups.Add( connectGroupsLifeStage );
                            rockContext.SaveChanges( DisableAudit );
                        } );
                        completedLifeStages++;
                    }

                    int existingConnectGroup = new GroupService( lookupContext ).Queryable().Where( g => g.Name == groupName ).Select( a => a.Id ).FirstOrDefault();

                    //check to see if Connect Group exists.
                    if ( existingConnectGroup == null || existingConnectGroup == 0 )
                    {
                        //Create one.
                        var connectGroups = new Group();
                        connectGroups.IsSystem = false;
                        connectGroups.GroupTypeId = groupTypeIdSmallGroup;
                        connectGroups.ParentGroupId = existingLifeStage;
                        connectGroups.CampusId = 1;
                        connectGroups.Name = groupName;
                        connectGroups.Description = "";
                        connectGroups.IsActive = true;
                        //connectGroups.Order = 0;
                        connectGroups.CreatedByPersonAliasId = 1;
                        connectGroups.CreatedDateTime = createdDateTime;

                        //Save Group
                        rockContext.WrapTransaction( () =>
                        {
                            rockContext.Configuration.AutoDetectChangesEnabled = false;
                            rockContext.Groups.Add( connectGroups );
                            rockContext.SaveChanges( DisableAudit );
                        } );
                        completedGroups++;
                    }

                    //makes sure Connect Group Exists
                    if ( existingConnectGroup != null || existingConnectGroup != 0 )
                    {
                        int memberGroupTypeRoleId = new GroupService( lookupContext ).Queryable().Where( g => g.Guid == new Guid( "F0806058-7E5D-4CA9-9C04-3BDF92739462" ) ).Select( a => a.Id ).FirstOrDefault();

                        //adds member
                        var connectGroupMember = new GroupMember();
                        connectGroupMember.IsSystem = false;
                        connectGroupMember.GroupId = existingConnectGroup;
                        connectGroupMember.PersonId = (int)personId;
                        connectGroupMember.GroupRoleId = memberGroupTypeRoleId; //will add them as a member

                        //Save Member
                        rockContext.WrapTransaction( () =>
                        {
                            rockContext.Configuration.AutoDetectChangesEnabled = false;
                            rockContext.GroupMembers.Add( connectGroupMember );
                            rockContext.SaveChanges( DisableAudit );
                        } );
                        completedMembers++;

                    }

                    if ( completedMembers % percentage < 1 )
                    {
                        int percentComplete = completedMembers / percentage;
                        ReportProgress( percentComplete, string.Format( "Life Stages Imported: {0}, Groups Imported: {1}, Members Imported: {2} ({3}% complete). ", completedLifeStages, completedGroups, completedMembers, percentComplete ) );
                    }
                    else if ( completedMembers % ReportingNumber < 1 )
                    {
                        ReportPartialProgress();
                    }

                }
            }

        }

    }
}
