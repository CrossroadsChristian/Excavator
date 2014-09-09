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
using Rock.Data;
using Rock.Model;
using Rock.Security;
using Rock.Web.Cache;

namespace Excavator.F1
{
    /// <summary>
    /// Partial of F1Component that holds the Financial import methods
    /// </summary>
    partial class F1Component
    {
        /// <summary>
        /// Used to return the date range of an F1 Attribute, as long as the start date is not null.
        /// </summary>
        /// <param name="f1StartDate">Fellowship One Start_Date</param>
        /// <param name="f1EndDate"> Fellowship One End Date</param>
        /// <returns>Returns the ToShortDateString() 5/1/2013,5/2/2014. If End Date is null only the (string) start date is returned.</returns>

        private string DateRange(DateTime? f1StartDate, DateTime? f1EndDate)
        {
            if (f1StartDate != null)
            {
                DateTime startDate = (DateTime)f1StartDate;

                if (f1EndDate != null)
                {
                    DateTime endDate = (DateTime)f1EndDate;
                    string dateRange = startDate.ToShortDateString() + "," + endDate.ToShortDateString();
                    return dateRange;
                }
                else
                {
                    string justStartDate = startDate.ToShortDateString();
                    return justStartDate;
                }
            }
            else
            {
                return "";
            }
        }

        private string MultiSelectYearGUID(DateTime? f1StartDate) //using this to enter the Year Multi-Select GUID of a defined Year that is already in Rock
        {
            DateTime startDate = (DateTime)f1StartDate;

            var lookupContext = new RockContext();
            var dvService = new DefinedValueService(lookupContext);
            var dtService = new DefinedTypeService(lookupContext);

            var yearInList = new DefinedValue();
            int dtInList; //= new DefinedType();

            var yearMultiSelectDefinedType = dtService.Queryable()
                .Where(dt => dt.Name == "Year Multi-Selection").ToList(); //finds all rows in Defined Type with this name (only one present)
            dtInList = yearMultiSelectDefinedType.Where(dt => dt.Name == "Year Multi-Selection") //sets the above Defined Type ID to this variable.
                .Select(dt => dt.Id).FirstOrDefault();

            var existingDefinedYears = dvService.Queryable()
                .Where(dv => dv.DefinedTypeId == dtInList).ToList();  //finds all Definded Values with the Defined Type ID from the item above.

            string guid = string.Format("{0}", existingDefinedYears.Where(dt => dt.Value == string.Format("{0}", startDate.Year)).Select(dt => dt.Guid).FirstOrDefault()); //the value that will be returned.

            return guid;

            //if (f1StartDate != null)
            //{

            //    switch (startDate.Year)
            //    {
            //        case 2001:
            //            guid = "B9A40993-7758-49A3-BE6B-00E930FCF690";
            //            break;
            //        case 2002:
            //            guid = "56BF96EF-561E-424D-BA85-A93674569B47";
            //            break;
            //        case 2003:
            //            guid = "74EB6703-DEB4-4CEA-81E2-5EC7ED81BB18";
            //            break;
            //        case 2004:
            //            guid = "DD28ACBD-8B2C-49CC-81C9-B7FFE4D8E3C2";
            //            break;
            //        case 2005:
            //            guid = "F18A88B7-5228-4B7D-8079-4B118DF792C7";
            //            break;
            //        case 2006:
            //            guid = "719DF19D-B5AF-4125-B708-BDC22EB64E8F";
            //            break;
            //        case 2007:
            //            guid = "CE44EA17-020E-4B97-8975-4DE01830163D";
            //            break;
            //        case 2008:
            //            guid = "6810C1C9-85BD-42E9-9E04-85801A93096D";
            //            break;
            //        case 2009:
            //            guid = "2C8B55AF-B5E2-41F9-9E08-C2E6F4624550";
            //            break;
            //        case 2010:
            //            guid = "FB260D37-AEF4-4277-959C-5884E579E1AC";
            //            break;
            //        case 2011:
            //            guid = "6E84915B-CC11-4E66-954E-9B1D786B2E6F";
            //            break;
            //        case 2012:
            //            guid = "4ED12DFD-BA8F-4760-A045-E7AC898BEC50";
            //            break;
            //        case 2013:
            //            guid = "AFEC8401-3E49-4895-B320-6FF4918A5F4D";
            //            break;
            //        case 2014:
            //            guid = "F80B2BEA-5FA5-48C4-82FF-AC5E1A15C763";
            //            break;
            //        default:
            //            guid = "none";
            //            break;
            //    }
            //    return guid;
            //}
            //else
            //{
            //    return "none";
            //}
        }

        private string ConnectGroupSeasonsGUID(DateTime? f1StartDate) //using this to enter the Connect Group Seasons GUID of a defined Year that is already in Rock
        {

            DateTime startDate = (DateTime)f1StartDate;

            string guid = "none";

            if (startDate.Year == 2013)
            {
                switch (startDate.Month)
                {
                    case 08:
                    case 09:
                    case 10:
                    case 11:
                    case 12:
                        guid = "BA998BEC-4371-4279-8F70-DA00A2AE0F64";
                        break;
                    default:
                        break;
                }
            }
            else if (startDate.Year == 2014)
            {
                switch (startDate.Month)
                {
                    //Winter session, January - beginning of March. Since Spring CG starts in the mid of March, March will be used for Spring
                    case 1:
                    case 2:
                        guid = "7AAD4061-B9B1-4FCA-BE88-3EEABC9E467E";
                        break;
                    case 3:
                    case 4:
                    case 5:
                        guid = "580909EC-0EE4-4141-942A-7400C53509EF";
                        break;
                    default:
                        break;
                }
            }
            return guid;
        }

        private string CrossroadsSportsYears(DateTime? f1StartDate, string playCoachRefVol) //using this to enter the Sports GUID of a defined Year that is already in Rock
        {

            DateTime startDate = (DateTime)f1StartDate;

            var lookupContext = new RockContext();
            var dvService = new DefinedValueService(lookupContext);
            var dtService = new DefinedTypeService(lookupContext);

            var yearInList = new DefinedValue();
            int dtInList; //= new DefinedType();

            var yearMultiSelectDefinedType = dtService.Queryable()
                .Where(dt => dt.Name == "Crossroads Sports Years").ToList(); //finds all rows in Defined Type with this name (only one present)
            dtInList = yearMultiSelectDefinedType.Where(dt => dt.Name == "Crossroads Sports Years") //sets the above Defined Type ID to this variable.
                .Select(dt => dt.Id).FirstOrDefault();

            var existingDefinedYears = dvService.Queryable()
                .Where(dv => dv.DefinedTypeId == dtInList).ToList();  //finds all Definded Values with the Defined Type ID from the item above.

            string guid = string.Format("{0}", existingDefinedYears.Where(dt => dt.Value == string.Format("{0} ({1})", startDate.Year, playCoachRefVol)).Select(dt => dt.Guid).FirstOrDefault()); //the value that will be returned. Takes on two properties, the start date and the second word (Play) etc.

            return guid;

        }

        private string CrossroadsSportsCampYears(DateTime? f1StartDate, string playVol) //using this to enter the Connect Group Seasons GUID of a defined Year that is already in Rock
        {

            DateTime startDate = (DateTime)f1StartDate;

            var lookupContext = new RockContext();
            var dvService = new DefinedValueService(lookupContext);
            var dtService = new DefinedTypeService(lookupContext);

            var yearInList = new DefinedValue();
            int dtInList; //= new DefinedType();

            var yearMultiSelectDefinedType = dtService.Queryable()
                .Where(dt => dt.Name == "Crossroads Sports Camp Years").ToList(); //finds all rows in Defined Type with this name (only one present)
            dtInList = yearMultiSelectDefinedType.Where(dt => dt.Name == "Crossroads Sports Camp Years") //sets the above Defined Type ID to this variable.
                .Select(dt => dt.Id).FirstOrDefault();

            var existingDefinedYears = dvService.Queryable()
                .Where(dv => dv.DefinedTypeId == dtInList).ToList();  //finds all Definded Values with the Defined Type ID from the item above.

            string guid = string.Format("{0}", existingDefinedYears.Where(dt => dt.Value == string.Format("{0} ({1})", startDate.Year, playVol)).Select(dt => dt.Guid).FirstOrDefault()); //the value that will be returned. Takes on two properties, the start date and the second word (Play) etc.

            return guid;

        }

        //public bool CheckAttributeValues(int id)
        //{
        //    var listCurrentAttributeValues = new List<AttributeValue>();
        //    var listAttributeValues = attributeValueService.GetByAttributeIdAndEntityId(a, individualAttribute.Id).Select(av => new { PersonId = av.EntityId, IndividualId = av.Value }).ToList();

        //    list.Find(p => p.IdItem == id).FieldToModify = newValueForTheFIeld;
        //}
        /// <summary>
        /// Maps the attribute data.
        /// </summary>
        /// <param name="tableData">The table data.</param> //Make sure this tableData is linked to attributes. Prob configed in F1Component.cs
        /// <returns></returns>
        private void MapAttributes(IQueryable<Row> tableData)
        {
            var lookupContext = new RockContext();
            // var importedAttributes = new AttributeValueService(lookupContext).Queryable().ToList();
            var newAttributes = new List<AttributeValue>();
            // var extraDateAttribute = new List<AttributeValue>();
            // var extraCommentAttribute = new List<AttributeValue>();

            int completed = 0;
            int totalRows = tableData.Count();
            int percentage = (totalRows - 1) / 100 + 1;
            ReportProgress(0, string.Format("Verifying attribute import ({0:N0} found).", totalRows));

            foreach (var row in tableData)
            {
                string f1AttributeGroupName = row["Attribute_Group_Name"] as string;
                string f1AttributeName = row["Attribute_Name"] as string;
                int? individualId = row["Individual_Id"] as int?;
                DateTime? f1StartDate = row["Start_Date"] as DateTime?;
                DateTime? f1EndDate = row["End_Date"] as DateTime?;
                string f1Comment = row["Comment"] as string;
                int? f1StaffIndividualId = row["Staff_Individual_ID"] as int?;
                int? staffId = GetPersonId(f1StaffIndividualId);
                int? personId = GetPersonId(individualId);


                //if (personId != null)
                //{
                //int? routingNumber = row["Routing_Number"] as int?;
                //string accountNumber = row["Account"] as string;
                // if (f1AttributeGroupName != null && !string.IsNullOrWhiteSpace(f1AttributeName))
                if (!string.IsNullOrWhiteSpace(f1AttributeGroupName))
                {
                    //accountNumber = accountNumber.Replace(" ", string.Empty);
                    //string encodedNumber = FinancialPersonBankAccount.EncodeAccountNumber(routingNumber.ToString(), accountNumber);
                    //if (!importedAttributes.Any(a => a.PersonId == (int)personId && a.AccountNumberSecured == encodedNumber))
                    //if (!importedAttributes.Any(a => a.EntityId == (int)personId && a.AttributeId == 
                    //{
                    var attributeValues = new AttributeValue();
                    var attributeExtraDate = new AttributeValue();
                    var attributeExtraComment = new AttributeValue();

                    //Checking Attribute Group Name in F1 and then Attribute Name and will configure the attributes from there.
                    //var rockContext = new RockContext();
                    // var attributeValueService = new AttributeValueService(rockContext);
                    //       //var listAttributeValues = attributeValueService.GetByAttributeIdAndEntityId(a, individualAttribute.Id).Select(av => new { PersonId = av.EntityId, IndividualId = av.Value }).ToList();
                    // var listImportedAttributes = attributeValueService.GetByEntityId(personId).Select(av => new { PersonId = av.EntityId, AttributeId = av.AttributeId, AttributeValue = av.Value }).ToList();
                    //GetByAttributeId(individualAttribute.Id).Select(av => new { PersonId = av.EntityId, IndividualId = av.Value }).ToList();


                    //For searching inside a list
                    //int count = 0 , index = -1;
                    //foreach (Class1 s in list)
                    //{
                    //    if (s.Number == textBox6.Text)
                    //        index = count; // I found a match and I want to edit the item at this index
                    //    count++;
                    //}

                    //For editing a property in an object that's inside a List
                    // list[someIndex].SomeProperty = someValue;
                    //list.Find(p => p.IdItem == id).FieldToModify = newValueForTheFIeld;

                    //if (newAttributes.Find(a => a.AttributeId == 1160 && a.EntityId == personId) != null)
                    //{
                    //    int test = 1;
                    //}
                    ReportProgress(0, string.Format("Adding {0} :: {1}.", f1AttributeGroupName, f1AttributeName));
                    if (f1AttributeGroupName == "2013 Preteen Camp")
                    {
                        //So it adds and extra attribute for the date, which is 2013 in this case.
                        attributeExtraDate.AttributeId = 1160;
                        attributeExtraDate.EntityId = (int)personId;
                        attributeExtraDate.Value = "AFEC8401-3E49-4895-B320-6FF4918A5F4D";

                        if (f1AttributeName == "Camper Cabinmate Request" && !string.IsNullOrWhiteSpace(f1Comment))
                        {
                            //Will not be able to convert since Rock FieldType is Person search whereas F1 is string/text
                        }
                        else if (f1AttributeName == "Camper Grade" && !string.IsNullOrWhiteSpace(f1Comment))
                        {
                            //This will be in the person bio already. Ask if they still want me to create an attribute for this.
                        }
                        else if (f1AttributeName == "Camper Leader Request" && !string.IsNullOrWhiteSpace(f1Comment))
                        {
                            //Will not be able to convert since Rock FieldType is Person search whereas F1 is string/text

                        }
                        else if (f1AttributeName == "Camper Medical Info" && !string.IsNullOrWhiteSpace(f1Comment))
                        {
                            if (f1Comment != null)
                            {
                                attributeValues.AttributeId = 996;
                                attributeValues.EntityId = (int)personId;
                                attributeValues.Value = f1Comment;
                            }

                        }
                        else if (f1AttributeName == "Camper Needs" && !string.IsNullOrWhiteSpace(f1Comment))
                        {
                            if (f1Comment != null)
                            {
                                attributeValues.AttributeId = 997;
                                attributeValues.EntityId = (int)personId;
                                attributeValues.Value = f1Comment;
                            }

                        }
                        else if (f1AttributeName == "Camper Payment" && !string.IsNullOrWhiteSpace(f1Comment))
                        {
                            if (f1Comment != null)
                            {
                                attributeValues.AttributeId = 998;
                                attributeValues.EntityId = (int)personId;
                                attributeValues.Value = f1Comment;
                            }

                        }
                        else if (f1AttributeName == "Camper Shirt Size" && !string.IsNullOrWhiteSpace(f1Comment))
                        {
                            //Uses the Guid from the Defined Value table for each size value.
                            attributeValues.AttributeId = 1219;
                            attributeValues.EntityId = (int)personId;

                            switch (f1Comment.ToUpper())
                            {
                                case "AS":
                                    attributeValues.Value = "A7393CAA-8C40-4A66-B085-D2F4EC41066C";
                                    break;
                                case "AM":
                                    attributeValues.Value = "B3703AE3-5FF4-47D2-BEBE-74B348940055";
                                    break;
                                case "AL":
                                    attributeValues.Value = "D005414A-1196-4495-BFE1-79539F1BC366";
                                    break;
                                case "AXL":
                                    attributeValues.Value = "386A1190-1BE1-452B-8197-D12A384525B7";
                                    break;
                                case "AXXL":
                                    attributeValues.Value = "A9210FCD-A8B3-4BBE-93EC-3BA429C3E031";
                                    break;
                                case "YS":
                                    attributeValues.Value = "29AF8D41-1FFF-4553-87B4-A08689923281";
                                    break;
                                case "YM":
                                    attributeValues.Value = "DCF08875-DF43-4F2C-B7CC-740A97C3D7BE";
                                    break;
                                case "YL":
                                    attributeValues.Value = "C5B04DF2-B121-4360-94A2-216A4BADCEB6";
                                    break;
                                case "CS":
                                    attributeValues.Value = "2F1CD55A-7E26-4A75-871D-3C769439CBB7";
                                    break;
                                case "CM":
                                    attributeValues.Value = "16C56A9A-41F1-4E68-8CE8-824F29B98145";
                                    break;
                                case "CL":
                                    attributeValues.Value = "9701D0E5-9F3E-4F0B-8822-B12D111A00C6";
                                    break;
                                default:
                                    break;
                            }
                        }
                        else if (f1AttributeName == "Counselor Shirt Size" && !string.IsNullOrWhiteSpace(f1Comment))
                        {
                            //Uses the Guid from the Defined Value table for each size value.
                            attributeValues.AttributeId = 1219;
                            attributeValues.EntityId = (int)personId;

                            switch (f1Comment.ToUpper())
                            {
                                case "AS":
                                    attributeValues.Value = "A7393CAA-8C40-4A66-B085-D2F4EC41066C";
                                    break;
                                case "AM":
                                    attributeValues.Value = "B3703AE3-5FF4-47D2-BEBE-74B348940055";
                                    break;
                                case "AL":
                                    attributeValues.Value = "D005414A-1196-4495-BFE1-79539F1BC366";
                                    break;
                                case "AXL":
                                    attributeValues.Value = "386A1190-1BE1-452B-8197-D12A384525B7";
                                    break;
                                case "AXXL":
                                    attributeValues.Value = "A9210FCD-A8B3-4BBE-93EC-3BA429C3E031";
                                    break;
                                case "YS":
                                    attributeValues.Value = "29AF8D41-1FFF-4553-87B4-A08689923281";
                                    break;
                                case "YM":
                                    attributeValues.Value = "DCF08875-DF43-4F2C-B7CC-740A97C3D7BE";
                                    break;
                                case "YL":
                                    attributeValues.Value = "C5B04DF2-B121-4360-94A2-216A4BADCEB6";
                                    break;
                                case "CS":
                                    attributeValues.Value = "2F1CD55A-7E26-4A75-871D-3C769439CBB7";
                                    break;
                                case "CM":
                                    attributeValues.Value = "16C56A9A-41F1-4E68-8CE8-824F29B98145";
                                    break;
                                case "CL":
                                    attributeValues.Value = "9701D0E5-9F3E-4F0B-8822-B12D111A00C6";
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                    //  Can't do much with this Attribute until we get a new copy of the database.

                    if (f1AttributeGroupName == "2014 Spring Cleaning")
                    {
                        if (f1Comment != null)
                        {
                            attributeValues.AttributeId = 964;
                            attributeValues.EntityId = (int)personId;
                            attributeValues.Value = f1Comment;
                        }
                        if (f1Comment != null)
                        {
                            attributeValues.AttributeId = 979;
                            attributeValues.EntityId = (int)personId;
                            attributeValues.Value = "F80B2BEA-5FA5-48C4-82FF-AC5E1A15C763";
                        }
                    }
                    else if (f1AttributeGroupName == "2009 Family Camp Out")
                    {
                        attributeValues.AttributeId = 1084;
                        attributeValues.EntityId = (int)personId;
                        attributeValues.Value = "2C8B55AF-B5E2-41F9-9E08-C2E6F4624550";
                    }
                    else if (f1AttributeGroupName == "Adult Ministries")
                    {
                        if (f1AttributeName == "2009 Family Camp Out")
                        {
                            attributeValues.AttributeId = 1084;
                            attributeValues.EntityId = (int)personId;
                            attributeValues.Value = "2C8B55AF-B5E2-41F9-9E08-C2E6F4624550";
                        }
                    }
                    //Uses Years Multi-Select Attribute and a Comment Attribute
                    //When items are converted go back through AT recipients because some may have been here for multiple instead of 1 (since excavator may overwrite previous value).
                    else if (f1AttributeGroupName == "Angel Tree")
                    {
                        if (f1AttributeName == "Recipient 2006")
                        {
                            //Checks if current person already has this Rock Attribute and will append the current F1 attribute value to the Rock one.
                            if (newAttributes.Find(a => (a.AttributeId == 971 && a.EntityId == personId) || (a.AttributeId == 972 && a.EntityId == personId)) != null)
                            {
                                if (newAttributes.Find(a => a.AttributeId == 971 && a.EntityId == personId) != null)  //multi-select year value
                                {
                                    if (newAttributes.Find(a => a.Value.StartsWith("719DF19D-B5AF-4125-B708-BDC22EB64E8F")) == null) //Making sure it isn't adding a duplicate year.
                                    {
                                        ReportProgress(0, string.Format("Adding Multi-Select Year 2006"));
                                        newAttributes.Find(a => a.AttributeId == 971 && a.EntityId == personId).Value += ",719DF19D-B5AF-4125-B708-BDC22EB64E8F";
                                    }
                                }
                                if (newAttributes.Find(a => a.AttributeId == 972 && a.EntityId == personId) != null)  //f1comment value
                                {
                                    if (f1Comment != null)
                                    {
                                        if (newAttributes.Find(a => a.Value.StartsWith(f1Comment)) == null) //Making sure it isn't adding a comment that is already added.
                                        {
                                            newAttributes.Find(a => a.AttributeId == 972 && a.EntityId == personId).Value += ", " + f1Comment;
                                        }
                                    }
                                }
                            }
                            //if current person does not have these attributes
                            else
                            {
                                //setting Angel Tree (Years Multi-Select)
                                attributeExtraDate.AttributeId = 971;
                                attributeExtraDate.EntityId = (int)personId;
                                attributeExtraDate.Value = "719DF19D-B5AF-4125-B708-BDC22EB64E8F";
                                if (f1Comment != null)
                                {
                                    //Setting Angel Tree Comments/Text
                                    attributeValues.AttributeId = 972;
                                    attributeValues.EntityId = (int)personId;
                                    attributeValues.Value = f1Comment;
                                }
                            }
                        }
                        else if (f1AttributeName == "Recipient 2007")
                        {
                            //Checks if current person already has this Rock Attribute and will append the current F1 attribute value to the Rock one.
                            if (newAttributes.Find(a => (a.AttributeId == 971 && a.EntityId == personId) || (a.AttributeId == 972 && a.EntityId == personId)) != null)
                            {
                                if (newAttributes.Find(a => a.AttributeId == 971 && a.EntityId == personId) != null)  //multi-select year value
                                {
                                    if (newAttributes.Find(a => a.Value.StartsWith("CE44EA17-020E-4B97-8975-4DE01830163D")) == null) //Making sure it isn't adding a duplicate year Guid.
                                    {
                                        ReportProgress(0, string.Format("Adding Multi-Select Year 2007"));
                                        newAttributes.Find(a => a.AttributeId == 971 && a.EntityId == personId).Value += ",CE44EA17-020E-4B97-8975-4DE01830163D";
                                    }
                                }
                                if (newAttributes.Find(a => a.AttributeId == 972 && a.EntityId == personId) != null)  //f1comment value
                                {
                                    if (f1Comment != null)
                                    {
                                        if (newAttributes.Find(a => a.Value.StartsWith(f1Comment)) == null) //making sure it isn't adding a comment that is already entered
                                        {
                                            newAttributes.Find(a => a.AttributeId == 972 && a.EntityId == personId).Value += ", " + f1Comment;
                                        }
                                    }
                                }
                            }
                            //if current person does not have these attributes
                            else
                            {
                                //setting Angel Tree (Years Multi-Select)
                                attributeExtraDate.AttributeId = 971;
                                attributeExtraDate.EntityId = (int)personId;
                                attributeExtraDate.Value = "CE44EA17-020E-4B97-8975-4DE01830163D";

                                if (f1Comment != null)
                                {
                                    //Setting Angel Tree Comments/Text
                                    attributeValues.AttributeId = 972;
                                    attributeValues.EntityId = (int)personId;
                                    attributeValues.Value = f1Comment;
                                }
                            }
                        }
                        else if (f1AttributeName == "Recipient 2008")
                        {
                            //Checks if current person already has this Rock Attribute and will append the current F1 attribute value to the Rock one.
                            if (newAttributes.Find(a => (a.AttributeId == 971 && a.EntityId == personId) || (a.AttributeId == 972 && a.EntityId == personId)) != null)
                            {
                                if (newAttributes.Find(a => a.AttributeId == 971 && a.EntityId == personId) != null)  //multi-select year value
                                {
                                    if (newAttributes.Find(a => a.Value.StartsWith("6810C1C9-85BD-42E9-9E04-85801A93096D")) == null) //Making sure it isn't adding a duplicate year Guid.
                                    {
                                        ReportProgress(0, string.Format("Adding Multi-Select Year 2008"));
                                        newAttributes.Find(a => a.AttributeId == 971 && a.EntityId == personId).Value += ",6810C1C9-85BD-42E9-9E04-85801A93096D";
                                    }
                                }
                                if (newAttributes.Find(a => a.AttributeId == 972 && a.EntityId == personId) != null)  //f1comment value
                                {
                                    if (f1Comment != null)
                                    {
                                        if (newAttributes.Find(a => a.Value.StartsWith(f1Comment)) == null) //making sure it isn't adding a comment that is already entered
                                        {
                                            newAttributes.Find(a => a.AttributeId == 972 && a.EntityId == personId).Value += ", " + f1Comment;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                //if current person does not have these attributes
                                //setting Angel Tree (Years Multi-Select)
                                attributeExtraDate.AttributeId = 971;
                                attributeExtraDate.EntityId = (int)personId;
                                attributeExtraDate.Value = "6810C1C9-85BD-42E9-9E04-85801A93096D";

                                if (f1Comment != null)
                                {
                                    //Setting Angel Tree Comments/Text
                                    attributeValues.AttributeId = 972;
                                    attributeValues.EntityId = (int)personId;
                                    attributeValues.Value = f1Comment;
                                }
                            }
                        }
                        else if (f1AttributeName == "Recipient 2009")
                        {
                            //Checks if current person already has this Rock Attribute and will append the current F1 attribute value to the Rock one.
                            if (newAttributes.Find(a => (a.AttributeId == 971 && a.EntityId == personId) || (a.AttributeId == 972 && a.EntityId == personId)) != null)
                            {
                                if (newAttributes.Find(a => a.AttributeId == 971 && a.EntityId == personId) != null)  //multi-select year value
                                {
                                    if (newAttributes.Find(a => a.Value.StartsWith("2C8B55AF-B5E2-41F9-9E08-C2E6F4624550")) == null) //Making sure it isn't adding a duplicate year Guid.
                                    {
                                        ReportProgress(0, string.Format("Adding Multi-Select Year 2009"));
                                        newAttributes.Find(a => a.AttributeId == 971 && a.EntityId == personId).Value += ",2C8B55AF-B5E2-41F9-9E08-C2E6F4624550";
                                    }
                                }
                                if (newAttributes.Find(a => a.AttributeId == 972 && a.EntityId == personId) != null)  //f1comment value
                                {
                                    if (f1Comment != null)
                                    {
                                        if (newAttributes.Find(a => a.Value.StartsWith(f1Comment)) == null) //making sure it isn't adding a comment that is already entered
                                        {
                                            newAttributes.Find(a => a.AttributeId == 972 && a.EntityId == personId).Value += ", " + f1Comment;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                //setting Angel Tree (Years Multi-Select)
                                attributeExtraDate.AttributeId = 971;
                                attributeExtraDate.EntityId = (int)personId;
                                attributeExtraDate.Value = "2C8B55AF-B5E2-41F9-9E08-C2E6F4624550";

                                if (f1Comment != null)
                                {
                                    //Setting Angel Tree Comments/Text
                                    attributeValues.AttributeId = 972;
                                    attributeValues.EntityId = (int)personId;
                                    attributeValues.Value = f1Comment;
                                }
                            }
                        }
                        else if (f1AttributeName == "Recipient 2010")
                        {
                            //Checks if current person already has this Rock Attribute and will append the current F1 attribute value to the Rock one.
                            if (newAttributes.Find(a => (a.AttributeId == 971 && a.EntityId == personId) || (a.AttributeId == 972 && a.EntityId == personId)) != null)
                            {
                                if (newAttributes.Find(a => a.AttributeId == 971 && a.EntityId == personId) != null)  //multi-select year value
                                {
                                    if (newAttributes.Find(a => a.Value.StartsWith("FB260D37-AEF4-4277-959C-5884E579E1AC")) == null) //Making sure it isn't adding a duplicate year Guid.
                                    {
                                        ReportProgress(0, string.Format("Adding Multi-Select Year 2010"));
                                        newAttributes.Find(a => a.AttributeId == 971 && a.EntityId == personId).Value += ",FB260D37-AEF4-4277-959C-5884E579E1AC";
                                    }
                                }
                                if (newAttributes.Find(a => a.AttributeId == 972 && a.EntityId == personId) != null)  //f1comment value
                                {
                                    if (f1Comment != null)
                                    {
                                        if (newAttributes.Find(a => a.Value.StartsWith(f1Comment)) == null) //making sure it isn't adding a comment that is already entered
                                        {
                                            newAttributes.Find(a => a.AttributeId == 972 && a.EntityId == personId).Value += ", " + f1Comment;
                                        }
                                    }
                                }
                            }

                            else
                            {
                                //setting Angel Tree (Years Multi-Select)
                                attributeExtraDate.AttributeId = 971;
                                attributeExtraDate.EntityId = (int)personId;
                                attributeExtraDate.Value = "FB260D37-AEF4-4277-959C-5884E579E1AC";

                                if (f1Comment != null)
                                {
                                    //Setting Angel Tree Comments/Text
                                    attributeValues.AttributeId = 972;
                                    attributeValues.EntityId = (int)personId;
                                    attributeValues.Value = f1Comment;
                                }
                            }
                        }
                        else if (f1AttributeName == "Recipient 2011")
                        {
                            //Checks if current person already has this Rock Attribute and will append the current F1 attribute value to the Rock one.
                            if (newAttributes.Find(a => (a.AttributeId == 971 && a.EntityId == personId) || (a.AttributeId == 972 && a.EntityId == personId)) != null)
                            {
                                if (newAttributes.Find(a => a.AttributeId == 971 && a.EntityId == personId) != null)  //multi-select year value
                                {
                                    if (newAttributes.Find(a => a.Value.StartsWith("6E84915B-CC11-4E66-954E-9B1D786B2E6F")) == null) //Making sure it isn't adding a duplicate year Guid.
                                    {
                                        ReportProgress(0, string.Format("Adding Multi-Select Year 2011"));
                                        newAttributes.Find(a => a.AttributeId == 971 && a.EntityId == personId).Value += ",6E84915B-CC11-4E66-954E-9B1D786B2E6F";
                                    }
                                }
                                if (newAttributes.Find(a => a.AttributeId == 972 && a.EntityId == personId) != null)  //f1comment value
                                {
                                    if (f1Comment != null)
                                    {
                                        if (newAttributes.Find(a => a.Value.StartsWith(f1Comment)) == null) //making sure it isn't adding a comment that is already entered
                                        {
                                            newAttributes.Find(a => a.AttributeId == 972 && a.EntityId == personId).Value += ", " + f1Comment;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                //setting Angel Tree (Years Multi-Select)
                                attributeExtraDate.AttributeId = 971;
                                attributeExtraDate.EntityId = (int)personId;
                                attributeExtraDate.Value = "6E84915B-CC11-4E66-954E-9B1D786B2E6F";

                                if (f1Comment != null)
                                {
                                    //Setting Angel Tree Comments/Text
                                    attributeValues.AttributeId = 972;
                                    attributeValues.EntityId = (int)personId;
                                    attributeValues.Value = f1Comment;
                                }
                            }
                        }
                        else if (f1AttributeName == "Recipient 2012")
                        {
                            //Checks if current person already has this Rock Attribute and will append the current F1 attribute value to the Rock one.
                            if (newAttributes.Find(a => (a.AttributeId == 971 && a.EntityId == personId) || (a.AttributeId == 972 && a.EntityId == personId)) != null)
                            {
                                if (newAttributes.Find(a => a.AttributeId == 971 && a.EntityId == personId) != null)  //multi-select year value
                                {
                                    if (newAttributes.Find(a => a.Value.StartsWith("4ED12DFD-BA8F-4760-A045-E7AC898BEC50")) == null) //Making sure it isn't adding a duplicate year Guid.
                                    {
                                        ReportProgress(0, string.Format("Adding Multi-Select Year 2012"));
                                        newAttributes.Find(a => a.AttributeId == 971 && a.EntityId == personId).Value += ",4ED12DFD-BA8F-4760-A045-E7AC898BEC50";
                                    }
                                }
                                if (newAttributes.Find(a => a.AttributeId == 972 && a.EntityId == personId) != null)  //f1comment value
                                {
                                    if (f1Comment != null)
                                    {
                                        if (newAttributes.Find(a => a.Value.StartsWith(f1Comment)) == null) //making sure it isn't adding a comment that is already entered
                                        {
                                            newAttributes.Find(a => a.AttributeId == 972 && a.EntityId == personId).Value += ", " + f1Comment;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                //setting Angel Tree (Years Multi-Select)
                                attributeExtraDate.AttributeId = 971;
                                attributeExtraDate.EntityId = (int)personId;
                                attributeExtraDate.Value = "4ED12DFD-BA8F-4760-A045-E7AC898BEC50";

                                if (f1Comment != null)
                                {
                                    //Setting Angel Tree Comments/Text
                                    attributeValues.AttributeId = 972;
                                    attributeValues.EntityId = (int)personId;
                                    attributeValues.Value = f1Comment;
                                }
                            }
                        }
                        else if (f1AttributeName == "Recipient 2013")
                        {
                            //Checks if current person already has this Rock Attribute and will append the current F1 attribute value to the Rock one.
                            if (newAttributes.Find(a => (a.AttributeId == 971 && a.EntityId == personId) || (a.AttributeId == 972 && a.EntityId == personId)) != null)
                            {
                                if (newAttributes.Find(a => a.AttributeId == 971 && a.EntityId == personId) != null)  //multi-select year value
                                {
                                    if (newAttributes.Find(a => a.Value.StartsWith("AFEC8401-3E49-4895-B320-6FF4918A5F4D")) == null) //Making sure it isn't adding a duplicate year Guid.
                                    {
                                        ReportProgress(0, string.Format("Adding Multi-Select Year 2013"));
                                        newAttributes.Find(a => a.AttributeId == 971 && a.EntityId == personId).Value += ",AFEC8401-3E49-4895-B320-6FF4918A5F4D";
                                    }
                                }
                                if (newAttributes.Find(a => a.AttributeId == 972 && a.EntityId == personId) != null)  //f1comment value
                                {
                                    if (f1Comment != null)
                                    {
                                        if (newAttributes.Find(a => a.Value.StartsWith(f1Comment)) == null) //making sure it isn't adding a comment that is already entered
                                        {
                                            newAttributes.Find(a => a.AttributeId == 972 && a.EntityId == personId).Value += ", " + f1Comment;
                                        }
                                    }
                                }
                            }

                            else
                            {
                                //setting Angel Tree (Years Multi-Select)
                                attributeExtraDate.AttributeId = 971;
                                attributeExtraDate.EntityId = (int)personId;
                                attributeExtraDate.Value = "AFEC8401-3E49-4895-B320-6FF4918A5F4D";

                                if (f1Comment != null)
                                {
                                    //Setting Angel Tree Comments/Text
                                    attributeValues.AttributeId = 972;
                                    attributeValues.EntityId = (int)personId;
                                    attributeValues.Value = f1Comment;
                                }
                            }
                        }
                    }

                    else if (f1AttributeGroupName == "Children's")
                    {
                        if (f1AttributeName == "Art's & Crafts")
                        {
                            if (f1Comment != null)
                            {
                                attributeValues.AttributeId = 973;
                                attributeValues.EntityId = (int)personId;
                                attributeValues.Value = f1Comment;
                            }
                            if (f1StartDate != null)
                            {

                                if (f1EndDate != null)
                                {
                                    attributeExtraDate.AttributeId = 974;
                                    attributeExtraDate.EntityId = (int)personId;
                                    attributeExtraDate.Value = DateRange(f1StartDate, f1EndDate);
                                }
                                else
                                {
                                    attributeExtraDate.AttributeId = 974;
                                    attributeExtraDate.EntityId = (int)personId;
                                    attributeExtraDate.Value = DateRange(f1StartDate, f1EndDate) + ",";
                                }
                            }
                        }

                        //else if (f1AttributeName == "Awana Store")
                        //{

                        //    if (f1StartDate != null)
                        //    {

                        //        if (f1EndDate != null)
                        //        {

                        //            attributeValues.AttributeId = 978;
                        //            attributeValues.EntityId = (int)personId;
                        //            attributeValues.Value = DateRange(f1StartDate, f1EndDate);
                        //        }
                        //        else
                        //        {
                        //            attributeValues.AttributeId = 978;
                        //            attributeValues.EntityId = (int)personId;
                        //            attributeValues.Value = DateRange(f1StartDate, f1EndDate) + ",";
                        //        }
                        //    }
                        //}
                        //else if (f1AttributeName == "Awana Leader")
                        //{
                        //    if (f1StartDate != null)
                        //    {

                        //        if (f1EndDate != null)
                        //        {

                        //            attributeValues.AttributeId = 975;
                        //            attributeValues.EntityId = (int)personId;
                        //            attributeValues.Value = DateRange(f1StartDate, f1EndDate);
                        //        }
                        //        else
                        //        {
                        //            attributeValues.AttributeId = 975;
                        //            attributeValues.EntityId = (int)personId;
                        //            attributeValues.Value = DateRange(f1StartDate, f1EndDate) + ",";
                        //        }
                        //    }
                        //}
                        //else if (f1AttributeName == "Awana Listener")
                        //{
                        //    if (f1StartDate != null)
                        //    {

                        //        if (f1EndDate != null)
                        //        {

                        //            attributeValues.AttributeId = 976;
                        //            attributeValues.EntityId = (int)personId;
                        //            attributeValues.Value = DateRange(f1StartDate, f1EndDate);
                        //        }
                        //        else
                        //        {
                        //            attributeValues.AttributeId = 976;
                        //            attributeValues.EntityId = (int)personId;
                        //            attributeValues.Value = DateRange(f1StartDate, f1EndDate) + ",";
                        //        }
                        //    }
                        //}
                        //else if (f1AttributeName == "Awana Secretary")
                        //{
                        //    if (f1StartDate != null)
                        //    {

                        //        if (f1EndDate != null)
                        //        {

                        //            attributeValues.AttributeId = 977;
                        //            attributeValues.EntityId = (int)personId;
                        //            attributeValues.Value = DateRange(f1StartDate, f1EndDate);
                        //        }
                        //        else
                        //        {
                        //            attributeValues.AttributeId = 977;
                        //            attributeValues.EntityId = (int)personId;
                        //            attributeValues.Value = DateRange(f1StartDate, f1EndDate) + ",";
                        //        }
                        //    }
                        //}

                        else if (f1AttributeName == "Carpentry")
                        {
                            if (f1StartDate != null)
                            {

                                if (f1EndDate != null)
                                {

                                    attributeValues.AttributeId = 999;
                                    attributeValues.EntityId = (int)personId;
                                    attributeValues.Value = DateRange(f1StartDate, f1EndDate);
                                }
                                else
                                {
                                    attributeValues.AttributeId = 999;
                                    attributeValues.EntityId = (int)personId;
                                    attributeValues.Value = DateRange(f1StartDate, f1EndDate) + ",";
                                }
                            }
                        }

                        else if (f1AttributeName == "Childcare (Special Events)")
                        {
                            if (f1StartDate != null)
                            {

                                if (f1EndDate != null)
                                {

                                    attributeValues.AttributeId = 1005;
                                    attributeValues.EntityId = (int)personId;
                                    attributeValues.Value = DateRange(f1StartDate, f1EndDate);
                                }
                                else
                                {
                                    attributeValues.AttributeId = 1005;
                                    attributeValues.EntityId = (int)personId;
                                    attributeValues.Value = DateRange(f1StartDate, f1EndDate) + ",";
                                }
                            }
                        }

                        else if (f1AttributeName == "Children's Summer Camp Counselor")
                        {
                            if (f1Comment != null)
                            {
                                attributeValues.AttributeId = 1009;
                                attributeValues.EntityId = (int)personId;
                                attributeValues.Value = f1Comment;
                            }
                            if (f1StartDate != null)
                            {

                                if (f1EndDate != null)
                                {

                                    attributeExtraDate.AttributeId = 1011;
                                    attributeExtraDate.EntityId = (int)personId;
                                    attributeExtraDate.Value = DateRange(f1StartDate, f1EndDate);
                                }
                                else
                                {
                                    attributeExtraDate.AttributeId = 1011;
                                    attributeExtraDate.EntityId = (int)personId;
                                    attributeExtraDate.Value = DateRange(f1StartDate, f1EndDate) + ",";
                                }
                            }
                        }

                        else if (f1AttributeName == "Choregraphy")
                        {
                            if (f1StartDate != null)
                            {
                                if (f1EndDate != null)
                                {
                                    attributeValues.AttributeId = 1012;
                                    attributeValues.EntityId = (int)personId;
                                    attributeValues.Value = DateRange(f1StartDate, f1EndDate);
                                }
                                else
                                {
                                    attributeValues.AttributeId = 1012;
                                    attributeValues.EntityId = (int)personId;
                                    attributeValues.Value = DateRange(f1StartDate, f1EndDate) + ",";
                                }
                            }
                        }

                        else if (f1AttributeName == "Costume Characters")
                        {
                            if (f1StartDate != null)
                            {
                                if (f1EndDate != null)
                                {
                                    attributeValues.AttributeId = 1037;
                                    attributeValues.EntityId = (int)personId;
                                    attributeValues.Value = DateRange(f1StartDate, f1EndDate);
                                }
                                else
                                {
                                    attributeValues.AttributeId = 1037;
                                    attributeValues.EntityId = (int)personId;
                                    attributeValues.Value = DateRange(f1StartDate, f1EndDate) + ",";
                                }
                            }
                        }

                        else if (f1AttributeName == "CPR Certified")
                        {
                            //Checks if current person already has this Rock Attribute and will append the current F1 attribute value to the Rock one.
                            if (newAttributes.Find(a => (a.AttributeId == 1040 && a.EntityId == personId)) != null)
                            {
                                ReportProgress(0, string.Format("Adding CPR from Children's"));
                                newAttributes.Find(a => a.AttributeId == 1040 && a.EntityId == personId).Value += "; " + DateRange(f1StartDate, f1EndDate) + " - " + f1Comment;
                                //newAttributes.Find(a => a.AttributeId == 1039 && a.EntityId == personId).Value += DateRange(f1StartDate, f1EndDate) + ",";
                            }
                            else
                            {
                                if (f1Comment != null)
                                {
                                    attributeValues.AttributeId = 1040;
                                    attributeValues.EntityId = (int)personId;
                                    attributeValues.Value = f1Comment;
                                }
                                if (f1StartDate != null)
                                {
                                    if (f1EndDate != null)
                                    {
                                        attributeExtraDate.AttributeId = 1039;
                                        attributeExtraDate.EntityId = (int)personId;
                                        attributeExtraDate.Value = DateRange(f1StartDate, f1EndDate);
                                    }
                                    else
                                    {
                                        attributeExtraDate.AttributeId = 1039;
                                        attributeExtraDate.EntityId = (int)personId;
                                        attributeExtraDate.Value = DateRange(f1StartDate, f1EndDate) + ",";
                                    }
                                }
                            }
                        }

                        else if (f1AttributeName == "DC4K (Divorce Care for Kids) Teacher")
                        {
                            if (f1StartDate != null)
                            {

                                if (f1EndDate != null)
                                {

                                    attributeValues.AttributeId = 1057;
                                    attributeValues.EntityId = (int)personId;
                                    attributeValues.Value = DateRange(f1StartDate, f1EndDate);
                                }
                                else
                                {
                                    attributeValues.AttributeId = 1057;
                                    attributeValues.EntityId = (int)personId;
                                    attributeValues.Value = DateRange(f1StartDate, f1EndDate) + ",";
                                }
                            }
                        }

                        else if (f1AttributeName == "Decorating - Special Events")
                        {
                            if (f1StartDate != null)
                            {

                                if (f1EndDate != null)
                                {

                                    attributeValues.AttributeId = 1062;
                                    attributeValues.EntityId = (int)personId;
                                    attributeValues.Value = DateRange(f1StartDate, f1EndDate);
                                }
                                else
                                {
                                    attributeValues.AttributeId = 1062;
                                    attributeValues.EntityId = (int)personId;
                                    attributeValues.Value = DateRange(f1StartDate, f1EndDate) + ",";
                                }
                            }
                        }

                        else if (f1AttributeName == "Elementary (1st - 3rd grade)")
                        {
                            if (f1Comment != null)
                            {
                                attributeValues.AttributeId = 1072;
                                attributeValues.EntityId = (int)personId;
                                attributeValues.Value = f1Comment;
                            }
                            if (f1StartDate != null)
                            {
                                if (f1EndDate != null)
                                {
                                    attributeExtraDate.AttributeId = 1073;
                                    attributeExtraDate.EntityId = (int)personId;
                                    attributeExtraDate.Value = DateRange(f1StartDate, f1EndDate);
                                }
                                else
                                {
                                    attributeExtraDate.AttributeId = 1073;
                                    attributeExtraDate.EntityId = (int)personId;
                                    attributeExtraDate.Value = DateRange(f1StartDate, f1EndDate) + ",";
                                }
                            }
                        }

                        else if (f1AttributeName == "Elementary (4th - 6th grade)")
                        {
                            if (f1Comment != null)
                            {
                                attributeValues.AttributeId = 1074;
                                attributeValues.EntityId = (int)personId;
                                attributeValues.Value = f1Comment;
                            }
                            if (f1StartDate != null)
                            {
                                if (f1EndDate != null)
                                {
                                    attributeExtraDate.AttributeId = 1075;
                                    attributeExtraDate.EntityId = (int)personId;
                                    attributeExtraDate.Value = DateRange(f1StartDate, f1EndDate);
                                }
                                else
                                {
                                    attributeExtraDate.AttributeId = 1075;
                                    attributeExtraDate.EntityId = (int)personId;
                                    attributeExtraDate.Value = DateRange(f1StartDate, f1EndDate) + ",";
                                }
                            }
                        }

                        else if (f1AttributeName == "Games")
                        {
                            if (f1StartDate != null)
                            {
                                if (f1EndDate != null)
                                {
                                    attributeValues.AttributeId = 1096;
                                    attributeValues.EntityId = (int)personId;
                                    attributeValues.Value = DateRange(f1StartDate, f1EndDate);
                                }
                                else
                                {
                                    attributeValues.AttributeId = 1096;
                                    attributeValues.EntityId = (int)personId;
                                    attributeValues.Value = DateRange(f1StartDate, f1EndDate) + ",";
                                }
                            }
                        }

                        else if (f1AttributeName == "Hallelujah Carnival")
                        {
                            if (f1StartDate != null)
                            {

                                if (f1EndDate != null)
                                {

                                    attributeValues.AttributeId = 1098;
                                    attributeValues.EntityId = (int)personId;
                                    attributeValues.Value = DateRange(f1StartDate, f1EndDate);
                                }
                                else
                                {
                                    attributeValues.AttributeId = 1098;
                                    attributeValues.EntityId = (int)personId;
                                    attributeValues.Value = DateRange(f1StartDate, f1EndDate) + ",";
                                }
                            }
                        }

                        //else if (f1AttributeName == "Hospitality")
                        //{
                        //    if (f1StartDate != null)
                        //    {

                        //        if (f1EndDate != null)
                        //        {
                        //            attributeValues.AttributeId = 1102;
                        //            attributeValues.EntityId = (int)personId;
                        //            attributeValues.Value = DateRange(f1StartDate, f1EndDate);
                        //        }
                        //        else
                        //        {
                        //            attributeValues.AttributeId = 1102;
                        //            attributeValues.EntityId = (int)personId;
                        //            attributeValues.Value = DateRange(f1StartDate, f1EndDate) + ",";
                        //        }
                        //    }
                        //}

                        else if (f1AttributeName == "Kid's Connection")
                        {
                            if (f1Comment != null)
                            {
                                attributeValues.AttributeId = 1110;
                                attributeValues.EntityId = (int)personId;
                                attributeValues.Value = f1Comment;
                            }
                            if (f1StartDate != null)
                            {
                                if (f1EndDate != null)
                                {
                                    attributeExtraDate.AttributeId = 1111;
                                    attributeExtraDate.EntityId = (int)personId;
                                    attributeExtraDate.Value = DateRange(f1StartDate, f1EndDate);
                                }
                                else
                                {
                                    attributeExtraDate.AttributeId = 1111;
                                    attributeExtraDate.EntityId = (int)personId;
                                    attributeExtraDate.Value = DateRange(f1StartDate, f1EndDate) + ",";
                                }
                            }
                        }

                        else if (f1AttributeName == "KAT's - Children's Choir")
                        {

                            if (f1Comment != null)
                            {
                                attributeValues.AttributeId = 1108;
                                attributeValues.EntityId = (int)personId;
                                attributeValues.Value = f1Comment;
                            }
                            if (f1StartDate != null)
                            {
                                if (f1EndDate != null)
                                {
                                    attributeExtraDate.AttributeId = 1109;
                                    attributeExtraDate.EntityId = (int)personId;
                                    attributeExtraDate.Value = DateRange(f1StartDate, f1EndDate);
                                }
                                else
                                {
                                    attributeExtraDate.AttributeId = 1109;
                                    attributeExtraDate.EntityId = (int)personId;
                                    attributeExtraDate.Value = DateRange(f1StartDate, f1EndDate) + ",";
                                }
                            }
                        }

                        else if (f1AttributeName == "Kidstuf - Drama")
                        {

                            if (f1Comment != null)
                            {
                                attributeValues.AttributeId = 1112;
                                attributeValues.EntityId = (int)personId;
                                attributeValues.Value = f1Comment;
                            }
                            if (f1StartDate != null)
                            {
                                if (f1EndDate != null)
                                {
                                    attributeExtraDate.AttributeId = 1113;
                                    attributeExtraDate.EntityId = (int)personId;
                                    attributeExtraDate.Value = DateRange(f1StartDate, f1EndDate);
                                }
                                else
                                {
                                    attributeExtraDate.AttributeId = 1113;
                                    attributeExtraDate.EntityId = (int)personId;
                                    attributeExtraDate.Value = DateRange(f1StartDate, f1EndDate) + ",";
                                }
                            }
                        }

                        else if (f1AttributeName == "Kindergarten")
                        {
                            if (f1Comment != null)
                            {
                                attributeValues.AttributeId = 1114;
                                attributeValues.EntityId = (int)personId;
                                attributeValues.Value = f1Comment;
                            }
                            if (f1StartDate != null)
                            {
                                if (f1EndDate != null)
                                {

                                    attributeExtraDate.AttributeId = 1115;
                                    attributeExtraDate.EntityId = (int)personId;
                                    attributeExtraDate.Value = DateRange(f1StartDate, f1EndDate);
                                }
                                else
                                {
                                    attributeExtraDate.AttributeId = 1115;
                                    attributeExtraDate.EntityId = (int)personId;
                                    attributeExtraDate.Value = DateRange(f1StartDate, f1EndDate) + ",";
                                }
                            }
                        }

                        else if (f1AttributeName == "Media")
                        {
                            if (f1StartDate != null)
                            {

                                if (f1EndDate != null)
                                {

                                    attributeValues.AttributeId = 1124;
                                    attributeValues.EntityId = (int)personId;
                                    attributeValues.Value = DateRange(f1StartDate, f1EndDate);
                                }
                                else
                                {
                                    attributeValues.AttributeId = 1124;
                                    attributeValues.EntityId = (int)personId;
                                    attributeValues.Value = DateRange(f1StartDate, f1EndDate) + ",";
                                }
                            }
                        }

                        else if (f1AttributeName == "Music")
                        {
                            if (f1StartDate != null)
                            {

                                if (f1EndDate != null)
                                {

                                    attributeValues.AttributeId = 1137;
                                    attributeValues.EntityId = (int)personId;
                                    attributeValues.Value = DateRange(f1StartDate, f1EndDate);
                                }
                                else
                                {
                                    attributeValues.AttributeId = 1137;
                                    attributeValues.EntityId = (int)personId;
                                    attributeValues.Value = DateRange(f1StartDate, f1EndDate) + ",";
                                }
                            }
                        }

                        else if (f1AttributeName == "Newborns - Toddlers")
                        {
                            if (f1Comment != null)
                            {
                                attributeValues.AttributeId = 1139;
                                attributeValues.EntityId = (int)personId;
                                attributeValues.Value = f1Comment;
                            }
                            if (f1StartDate != null)
                            {

                                if (f1EndDate != null)
                                {

                                    attributeExtraDate.AttributeId = 1140;
                                    attributeExtraDate.EntityId = (int)personId;
                                    attributeExtraDate.Value = DateRange(f1StartDate, f1EndDate);
                                }
                                else
                                {
                                    attributeExtraDate.AttributeId = 1140;
                                    attributeExtraDate.EntityId = (int)personId;
                                    attributeExtraDate.Value = DateRange(f1StartDate, f1EndDate) + ",";
                                }
                            }
                        }

                        else if (f1AttributeName == "Office Volunteer")
                        {
                            if (f1StartDate != null)
                            {

                                if (f1EndDate != null)
                                {

                                    attributeValues.AttributeId = 1141;
                                    attributeValues.EntityId = (int)personId;
                                    attributeValues.Value = DateRange(f1StartDate, f1EndDate);
                                }
                                else
                                {
                                    attributeValues.AttributeId = 1141;
                                    attributeValues.EntityId = (int)personId;
                                    attributeValues.Value = DateRange(f1StartDate, f1EndDate) + ",";
                                }
                            }
                        }

                        else if (f1AttributeName == "Organizing Supplies")
                        {
                            if (f1StartDate != null)
                            {

                                if (f1EndDate != null)
                                {

                                    attributeValues.AttributeId = 1144;
                                    attributeValues.EntityId = (int)personId;
                                    attributeValues.Value = DateRange(f1StartDate, f1EndDate);
                                }
                                else
                                {
                                    attributeValues.AttributeId = 1144;
                                    attributeValues.EntityId = (int)personId;
                                    attributeValues.Value = DateRange(f1StartDate, f1EndDate) + ",";
                                }
                            }
                        }

                        else if (f1AttributeName == "Praise Band")
                        {
                            if (f1StartDate != null)
                            {

                                if (f1EndDate != null)
                                {

                                    attributeValues.AttributeId = 1148;
                                    attributeValues.EntityId = (int)personId;
                                    attributeValues.Value = DateRange(f1StartDate, f1EndDate);
                                }
                                else
                                {
                                    attributeValues.AttributeId = 1148;
                                    attributeValues.EntityId = (int)personId;
                                    attributeValues.Value = DateRange(f1StartDate, f1EndDate) + ",";
                                }
                            }
                        }

                        else if (f1AttributeName == "Preschool (2yrs - 5 yrs)")
                        {
                            if (f1Comment != null)
                            {
                                attributeValues.AttributeId = 1155;
                                attributeValues.EntityId = (int)personId;
                                attributeValues.Value = f1Comment;
                            }
                            if (f1StartDate != null)
                            {

                                if (f1EndDate != null)
                                {

                                    attributeExtraDate.AttributeId = 1156;
                                    attributeExtraDate.EntityId = (int)personId;
                                    attributeExtraDate.Value = DateRange(f1StartDate, f1EndDate);
                                }
                                else
                                {
                                    attributeExtraDate.AttributeId = 1156;
                                    attributeExtraDate.EntityId = (int)personId;
                                    attributeExtraDate.Value = DateRange(f1StartDate, f1EndDate) + ",";
                                }
                            }
                        }
                        else if (f1AttributeName == "Previous Church Work")
                        {
                            if (f1Comment != null)
                            {
                                attributeValues.AttributeId = 1161;
                                attributeValues.EntityId = (int)personId;
                                attributeValues.Value = f1Comment;
                            }
                        }

                        else if (f1AttributeName == "Previous Non Church Work")
                        {
                            if (f1Comment != null)
                            {
                                attributeValues.AttributeId = 1162;
                                attributeValues.EntityId = (int)personId;
                                attributeValues.Value = f1Comment;
                            }
                        }

                        else if (f1AttributeName == "Puppet Team")
                        {
                            if (f1StartDate != null)
                            {

                                if (f1EndDate != null)
                                {
                                    attributeValues.AttributeId = 1163;
                                    attributeValues.EntityId = (int)personId;
                                    attributeValues.Value = DateRange(f1StartDate, f1EndDate);
                                }
                                else
                                {
                                    attributeValues.AttributeId = 1163;
                                    attributeValues.EntityId = (int)personId;
                                    attributeValues.Value = DateRange(f1StartDate, f1EndDate) + ",";
                                }
                            }
                        }

                        else if (f1AttributeName == "Security")
                        {
                            if (f1StartDate != null)
                            {

                                if (f1EndDate != null)
                                {
                                    attributeValues.AttributeId = 1176;
                                    attributeValues.EntityId = (int)personId;
                                    attributeValues.Value = DateRange(f1StartDate, f1EndDate);
                                }
                                else
                                {
                                    attributeValues.AttributeId = 1176;
                                    attributeValues.EntityId = (int)personId;
                                    attributeValues.Value = DateRange(f1StartDate, f1EndDate) + ",";
                                }
                            }
                        }

                        else if (f1AttributeName == "Set-up")
                        {
                            if (f1StartDate != null)
                            {

                                if (f1EndDate != null)
                                {

                                    attributeValues.AttributeId = 1184;
                                    attributeValues.EntityId = (int)personId;
                                    attributeValues.Value = DateRange(f1StartDate, f1EndDate);
                                }
                                else
                                {
                                    attributeValues.AttributeId = 1184;
                                    attributeValues.EntityId = (int)personId;
                                    attributeValues.Value = DateRange(f1StartDate, f1EndDate) + ",";
                                }
                            }
                        }

                        else if (f1AttributeName == "Sewing")
                        {
                            if (f1StartDate != null)
                            {

                                if (f1EndDate != null)
                                {

                                    attributeValues.AttributeId = 1185;
                                    attributeValues.EntityId = (int)personId;
                                    attributeValues.Value = DateRange(f1StartDate, f1EndDate);
                                }
                                else
                                {
                                    attributeValues.AttributeId = 1185;
                                    attributeValues.EntityId = (int)personId;
                                    attributeValues.Value = DateRange(f1StartDate, f1EndDate) + ",";
                                }
                            }
                        }

                        else if (f1AttributeName == "Special Children's Events")
                        {
                            if (f1Comment != null)
                            {
                                attributeValues.AttributeId = 1198;
                                attributeValues.EntityId = (int)personId;
                                attributeValues.Value = f1Comment;
                            }
                            if (f1StartDate != null)
                            {

                                if (f1EndDate != null)
                                {

                                    attributeExtraDate.AttributeId = 1199;
                                    attributeExtraDate.EntityId = (int)personId;
                                    attributeExtraDate.Value = DateRange(f1StartDate, f1EndDate);
                                }
                                else
                                {
                                    attributeExtraDate.AttributeId = 1199;
                                    attributeExtraDate.EntityId = (int)personId;
                                    attributeExtraDate.Value = DateRange(f1StartDate, f1EndDate) + ",";
                                }
                            }
                        }

                        else if (f1AttributeName == "Special Needs Children")
                        {
                            if (f1Comment != null)
                            {
                                attributeValues.AttributeId = 1202;
                                attributeValues.EntityId = (int)personId;
                                attributeValues.Value = f1Comment;
                            }
                            if (f1StartDate != null)
                            {

                                if (f1EndDate != null)
                                {

                                    attributeExtraDate.AttributeId = 1203;
                                    attributeExtraDate.EntityId = (int)personId;
                                    attributeExtraDate.Value = DateRange(f1StartDate, f1EndDate);
                                }
                                else
                                {
                                    attributeExtraDate.AttributeId = 1203;
                                    attributeExtraDate.EntityId = (int)personId;
                                    attributeExtraDate.Value = DateRange(f1StartDate, f1EndDate) + ",";
                                }
                            }
                        }

                        //else if (f1AttributeName == "Sports Coach")
                        //{
                        //    if (f1StartDate != null && f1Comment != null)
                        //    {

                        //        if (f1EndDate != null)
                        //        {

                        //            attributeValues.AttributeId = 1206;
                        //            attributeValues.EntityId = (int)personId;
                        //            attributeValues.Value = f1Comment + ". F1 Dates: " + DateRange(f1StartDate, f1EndDate);
                        //        }
                        //        else
                        //        {
                        //            attributeValues.AttributeId = 1206;
                        //            attributeValues.EntityId = (int)personId;
                        //            attributeValues.Value = f1Comment + ". F1 Dates: " + DateRange(f1StartDate, f1EndDate) + ",";
                        //        }
                        //    }
                        //}

                        else if (f1AttributeName == "Storytelling")
                        {
                            if (f1StartDate != null)
                            {

                                if (f1EndDate != null)
                                {

                                    attributeValues.AttributeId = 1215;
                                    attributeValues.EntityId = (int)personId;
                                    attributeValues.Value = DateRange(f1StartDate, f1EndDate);
                                }
                                else
                                {
                                    attributeValues.AttributeId = 1215;
                                    attributeValues.EntityId = (int)personId;
                                    attributeValues.Value = DateRange(f1StartDate, f1EndDate) + ",";
                                }
                            }
                        }

                        else if (f1AttributeName == "VBS")
                        {
                            if (f1StartDate != null && f1Comment != null)
                            {

                                if (f1EndDate != null)
                                {

                                    attributeValues.AttributeId = 1243;
                                    attributeValues.EntityId = (int)personId;
                                    attributeValues.Value = "F1 Dates: " + DateRange(f1StartDate, f1EndDate) + " " + f1Comment;
                                }
                                else
                                {
                                    attributeValues.AttributeId = 1243;
                                    attributeValues.EntityId = (int)personId;
                                    attributeValues.Value = "F1 Date: " + DateRange(f1StartDate, f1EndDate) + " " + f1Comment;
                                }
                            }
                        }

                        else if (f1AttributeName == "Volunteer Orientation")
                        {
                            if (f1StartDate != null)
                            {

                                if (f1EndDate != null)
                                {

                                    attributeValues.AttributeId = 1244;
                                    attributeValues.EntityId = (int)personId;
                                    attributeValues.Value = DateRange(f1StartDate, f1EndDate);
                                }
                                else
                                {
                                    attributeValues.AttributeId = 1244;
                                    attributeValues.EntityId = (int)personId;
                                    attributeValues.Value = DateRange(f1StartDate, f1EndDate) + ",";
                                }
                            }
                        }

                        else if (f1AttributeName == "Wherever the greatest need is")
                        {
                            if (f1Comment != null)
                            {
                                attributeValues.AttributeId = 1248;
                                attributeValues.EntityId = (int)personId;
                                attributeValues.Value = f1Comment;
                            }
                            if (f1StartDate != null)
                            {

                                if (f1EndDate != null)
                                {

                                    attributeExtraDate.AttributeId = 1249;
                                    attributeExtraDate.EntityId = (int)personId;
                                    attributeExtraDate.Value = DateRange(f1StartDate, f1EndDate);
                                }
                                else
                                {
                                    attributeExtraDate.AttributeId = 1249;
                                    attributeExtraDate.EntityId = (int)personId;
                                    attributeExtraDate.Value = DateRange(f1StartDate, f1EndDate) + ",";
                                }
                            }
                        }

                        else if (f1AttributeName == "Youth Covenant")
                        {

                            if (f1Comment != null)
                            {
                                attributeValues.AttributeId = 1260;
                                attributeValues.EntityId = (int)personId;
                                attributeValues.Value = f1Comment;
                            }
                            if (f1StartDate != null)
                            {
                                if (f1EndDate != null)
                                {

                                    attributeValues.AttributeId = 1260;
                                    attributeValues.EntityId = (int)personId;
                                    attributeValues.Value = DateRange(f1StartDate, f1EndDate);
                                }
                                else
                                {
                                    attributeValues.AttributeId = 1260;
                                    attributeValues.EntityId = (int)personId;
                                    attributeValues.Value = DateRange(f1StartDate, f1EndDate) + ",";
                                }
                            }
                        }
                    }

                    if (f1AttributeGroupName == "CIY (Youth)")
                    {
                        attributeValues.AttributeId = 1013;
                        attributeValues.EntityId = (int)personId;
                        attributeValues.Value = "56BF96EF-561E-424D-BA85-A93674569B47";
                    }

                    if (f1AttributeGroupName == "Commitments")
                    {
                        if (f1AttributeName == "2014 Summer of No Excuses")
                        {
                            attributeValues.AttributeId = 965;
                            attributeValues.EntityId = (int)personId;
                            attributeValues.Value = "F80B2BEA-5FA5-48C4-82FF-AC5E1A15C763";
                        }
                        if (f1AttributeName == "2014 Summer of No Excuses - Spanish")
                        {
                            attributeValues.AttributeId = 966;
                            attributeValues.EntityId = (int)personId;
                            attributeValues.Value = "F80B2BEA-5FA5-48C4-82FF-AC5E1A15C763";
                        }
                    }

                    //Just for leaders and hosts
                    if (f1AttributeGroupName == "Connect Group Leaders")
                    {
                        if (f1AttributeName == "Host")
                        {

                            //Checks if current person already has this Rock Attribute and will append the current F1 attribute value to the Rock one.
                            if (newAttributes.Find(a => (a.AttributeId == 1022 && a.EntityId == personId) || (a.AttributeId == 1023 && a.EntityId == personId)) != null)
                            {
                                if (newAttributes.Find(a => a.AttributeId == 1022 && a.EntityId == personId) != null)  //multi-select year value
                                {

                                    DateTime startDate = (DateTime)f1StartDate;
                                    if (startDate.Year == 2013)
                                    {
                                        switch (startDate.Month)
                                        {
                                            case 08:
                                            case 09:
                                            case 10:
                                            case 11:
                                            case 12:
                                                if (newAttributes.Find(a => a.Value.StartsWith("BA998BEC-4371-4279-8F70-DA00A2AE0F64,")) == null)
                                                {
                                                    newAttributes.Find(a => a.AttributeId == 1022 && a.EntityId == personId).Value += "BA998BEC-4371-4279-8F70-DA00A2AE0F64" + ",";
                                                }
                                                break;
                                            default:
                                                break;
                                        }
                                    }
                                    else if (startDate.Year == 2014)
                                    {
                                        switch (startDate.Month)
                                        {
                                            //Winter session, January - beginning of March. Since Spring CG starts in the mid of March, March will be used for Spring
                                            case 1:
                                            case 2:
                                                if (newAttributes.Find(a => a.Value.StartsWith("BA998BEC-4371-4279-8F70-DA00A2AE0F64,7AAD4061-B9B1-4FCA-BE88-3EEABC9E467E,")) == null)
                                                {
                                                    newAttributes.Find(a => a.AttributeId == 1022 && a.EntityId == personId).Value += "7AAD4061-B9B1-4FCA-BE88-3EEABC9E467E" + ",";
                                                }
                                                else if (newAttributes.Find(a => a.Value.StartsWith("7AAD4061-B9B1-4FCA-BE88-3EEABC9E467E,")) == null)
                                                {
                                                    newAttributes.Find(a => a.AttributeId == 1022 && a.EntityId == personId).Value += "7AAD4061-B9B1-4FCA-BE88-3EEABC9E467E" + ",";
                                                }
                                                break;
                                            case 3:
                                            case 4:
                                            case 5:
                                                if (newAttributes.Find(a => a.Value.StartsWith("BA998BEC-4371-4279-8F70-DA00A2AE0F64,7AAD4061-B9B1-4FCA-BE88-3EEABC9E467E,580909EC-0EE4-4141-942A-7400C53509EF,")) == null)
                                                {
                                                    newAttributes.Find(a => a.AttributeId == 1022 && a.EntityId == personId).Value += "580909EC-0EE4-4141-942A-7400C53509EF" + ",";
                                                }
                                                else if (newAttributes.Find(a => a.Value.StartsWith("BA998BEC-4371-4279-8F70-DA00A2AE0F64,580909EC-0EE4-4141-942A-7400C53509EF,")) == null)
                                                {
                                                    newAttributes.Find(a => a.AttributeId == 1022 && a.EntityId == personId).Value += "580909EC-0EE4-4141-942A-7400C53509EF" + ",";
                                                }
                                                else if (newAttributes.Find(a => a.Value.StartsWith("7AAD4061-B9B1-4FCA-BE88-3EEABC9E467E,580909EC-0EE4-4141-942A-7400C53509EF,")) == null)
                                                {
                                                    newAttributes.Find(a => a.AttributeId == 1022 && a.EntityId == personId).Value += "580909EC-0EE4-4141-942A-7400C53509EF" + ",";
                                                }
                                                else if (newAttributes.Find(a => a.Value.StartsWith("580909EC-0EE4-4141-942A-7400C53509EF,")) == null)
                                                {
                                                    newAttributes.Find(a => a.AttributeId == 1022 && a.EntityId == personId).Value += "580909EC-0EE4-4141-942A-7400C53509EF" + ",";
                                                }
                                                break;
                                            default:
                                                break;
                                        }
                                    }
                                }
                                if (newAttributes.Find(a => a.AttributeId == 1023 && a.EntityId == personId) != null)  //f1comment value
                                {
                                    if (f1Comment != null)
                                    {
                                        if (newAttributes.Find(a => a.Value.StartsWith(f1Comment)) == null)
                                        {
                                            newAttributes.Find(a => a.AttributeId == 1023 && a.EntityId == personId).Value += ", " + f1Comment;
                                        }
                                    }
                                }
                            }


                            //if current person does not have these attributes
                            else
                            {
                                //host comments
                                attributeValues.AttributeId = 1023;
                                attributeValues.EntityId = (int)personId;
                                attributeValues.Value = f1Comment;

                                //host seasons
                                attributeExtraDate.AttributeId = 1022;
                                attributeExtraDate.EntityId = (int)personId;

                                DateTime startDate = (DateTime)f1StartDate;
                                if (startDate.Year == 2013)
                                {
                                    switch (startDate.Month)
                                    {
                                        case 08:
                                        case 09:
                                        case 10:
                                        case 11:
                                        case 12:
                                            attributeExtraDate.Value = "BA998BEC-4371-4279-8F70-DA00A2AE0F64" + ",";
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                else if (startDate.Year == 2014)
                                {
                                    switch (startDate.Month)
                                    {
                                        //Winter session, January - beginning of March. Since Spring CG starts in the mid of March, March will be used for Spring
                                        case 1:
                                        case 2:
                                            attributeExtraDate.Value = "7AAD4061-B9B1-4FCA-BE88-3EEABC9E467E" + ",";
                                            break;
                                        case 3:
                                        case 4:
                                        case 5:
                                            attributeExtraDate.Value = "580909EC-0EE4-4141-942A-7400C53509EF" + ",";
                                            break;
                                        default:
                                            break;
                                    }
                                }
                            }
                        }

                        if (f1AttributeName == "Leader")
                        {
                            //Checks if current person already has this Rock Attribute and will append the current F1 attribute value to the Rock one.
                            if (newAttributes.Find(a => (a.AttributeId == 1028 && a.EntityId == personId) || (a.AttributeId == 1029 && a.EntityId == personId)) != null)
                            {
                                if (newAttributes.Find(a => a.AttributeId == 1028 && a.EntityId == personId) != null)  //multi-select year value
                                {

                                    DateTime startDate = (DateTime)f1StartDate;
                                    if (startDate.Year == 2013)
                                    {
                                        switch (startDate.Month)
                                        {
                                            case 08:
                                            case 09:
                                            case 10:
                                            case 11:
                                            case 12:
                                                if (newAttributes.Find(a => a.Value.StartsWith("BA998BEC-4371-4279-8F70-DA00A2AE0F64,")) == null)
                                                {
                                                    newAttributes.Find(a => a.AttributeId == 1028 && a.EntityId == personId).Value += "BA998BEC-4371-4279-8F70-DA00A2AE0F64" + ",";
                                                }
                                                break;
                                            default:
                                                break;
                                        }
                                    }
                                    else if (startDate.Year == 2014)
                                    {
                                        switch (startDate.Month)
                                        {
                                            //Winter session, January - beginning of March. Since Spring CG starts in the mid of March, March will be used for Spring
                                            case 1:
                                            case 2:
                                                if (newAttributes.Find(a => a.Value.StartsWith("BA998BEC-4371-4279-8F70-DA00A2AE0F64,7AAD4061-B9B1-4FCA-BE88-3EEABC9E467E,")) == null)
                                                {
                                                    newAttributes.Find(a => a.AttributeId == 1028 && a.EntityId == personId).Value += "7AAD4061-B9B1-4FCA-BE88-3EEABC9E467E" + ",";
                                                }
                                                else if (newAttributes.Find(a => a.Value.StartsWith("7AAD4061-B9B1-4FCA-BE88-3EEABC9E467E,")) == null)
                                                {
                                                    newAttributes.Find(a => a.AttributeId == 1028 && a.EntityId == personId).Value += "7AAD4061-B9B1-4FCA-BE88-3EEABC9E467E" + ",";
                                                }
                                                break;
                                            case 3:
                                            case 4:
                                            case 5:
                                                if (newAttributes.Find(a => a.Value.StartsWith("BA998BEC-4371-4279-8F70-DA00A2AE0F64,7AAD4061-B9B1-4FCA-BE88-3EEABC9E467E,580909EC-0EE4-4141-942A-7400C53509EF,")) == null)
                                                {
                                                    newAttributes.Find(a => a.AttributeId == 1028 && a.EntityId == personId).Value += "580909EC-0EE4-4141-942A-7400C53509EF" + ",";
                                                }
                                                else if (newAttributes.Find(a => a.Value.StartsWith("BA998BEC-4371-4279-8F70-DA00A2AE0F64,580909EC-0EE4-4141-942A-7400C53509EF,")) == null)
                                                {
                                                    newAttributes.Find(a => a.AttributeId == 1028 && a.EntityId == personId).Value += "580909EC-0EE4-4141-942A-7400C53509EF" + ",";
                                                }
                                                else if (newAttributes.Find(a => a.Value.StartsWith("7AAD4061-B9B1-4FCA-BE88-3EEABC9E467E,580909EC-0EE4-4141-942A-7400C53509EF,")) == null)
                                                {
                                                    newAttributes.Find(a => a.AttributeId == 1028 && a.EntityId == personId).Value += "580909EC-0EE4-4141-942A-7400C53509EF" + ",";
                                                }
                                                else if (newAttributes.Find(a => a.Value.StartsWith("580909EC-0EE4-4141-942A-7400C53509EF,")) == null)
                                                {
                                                    newAttributes.Find(a => a.AttributeId == 1028 && a.EntityId == personId).Value += "580909EC-0EE4-4141-942A-7400C53509EF" + ",";
                                                }
                                                break;
                                            case 8:
                                            case 9:
                                            case 10:
                                            case 11:
                                                newAttributes.Find(a => a.AttributeId == 1028 && a.EntityId == personId).Value += "DD6E1BFC-7458-4804-805D-16D5144AACE8" + ",";
                                                break;
                                            default:
                                                break;
                                        }
                                    }
                                }
                                if (newAttributes.Find(a => a.AttributeId == 1029 && a.EntityId == personId) != null)  //f1comment value
                                {
                                    if (f1Comment != null)
                                    {
                                        if (newAttributes.Find(a => a.Value.StartsWith(f1Comment)) == null)
                                        {
                                            newAttributes.Find(a => a.AttributeId == 1029 && a.EntityId == personId).Value += ", " + f1Comment;
                                        }
                                    }
                                }
                            }


                            //if current person does not have these attributes
                            else
                            {
                                //host comments
                                attributeValues.AttributeId = 1029;
                                attributeValues.EntityId = (int)personId;
                                attributeValues.Value = f1Comment;

                                //host seasons
                                attributeExtraDate.AttributeId = 1028;
                                attributeExtraDate.EntityId = (int)personId;

                                DateTime startDate = (DateTime)f1StartDate;
                                if (startDate.Year == 2013)
                                {
                                    switch (startDate.Month)
                                    {
                                        case 08:
                                        case 09:
                                        case 10:
                                        case 11:
                                        case 12:
                                            attributeExtraDate.Value = "BA998BEC-4371-4279-8F70-DA00A2AE0F64" + ",";
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                else if (startDate.Year == 2014)
                                {
                                    switch (startDate.Month)
                                    {
                                        //Winter session, January - beginning of March. Since Spring CG starts in the mid of March, March will be used for Spring
                                        case 1:
                                        case 2:
                                            attributeExtraDate.Value = "7AAD4061-B9B1-4FCA-BE88-3EEABC9E467E" + ",";
                                            break;
                                        case 3:
                                        case 4:
                                        case 5:
                                            attributeExtraDate.Value = "580909EC-0EE4-4141-942A-7400C53509EF" + ",";
                                            break;
                                        case 8:
                                        case 9:
                                        case 10:
                                        case 11:
                                            attributeExtraDate.Value = "DD6E1BFC-7458-4804-805D-16D5144AACE8" + ",";
                                            break;
                                        default:
                                            break;
                                    }
                                }
                            }
                        }
                    }
                    else if (f1AttributeGroupName == "Connect@Crossroads")
                    {
                        if (f1AttributeName == "Connect@Crossroads MIA")
                        {
                            attributeValues.AttributeId = 1133;
                            attributeValues.EntityId = (int)personId;
                            attributeValues.Value = DateRange(f1StartDate, f1EndDate) + ",";
                        }
                    }
                    //else if (f1AttributeGroupName == "Connection Central")
                    //{
                    //    if (f1AttributeName == "Connection Central")
                    //    {
                    //        if (f1Comment != null)
                    //        {
                    //            attributeValues.AttributeId = 1030;
                    //            attributeValues.EntityId = (int)personId;
                    //            attributeValues.Value = f1Comment;
                    //        }
                    //        else if (f1StartDate != null)
                    //        {
                    //            attributeValues.AttributeId = 1031;
                    //            attributeValues.EntityId = (int)personId;
                    //            attributeValues.Value = DateRange(f1StartDate, f1EndDate) + ",";
                    //        }
                    //    }
                    //}
                    //else if (f1AttributeGroupName == "Count Me In")
                    //{
                    //    if (f1AttributeName == "Count Me In")
                    //    {
                    //        attributeValues.AttributeId = 1038;
                    //        attributeValues.EntityId = (int)personId;
                    //        attributeValues.Value = DateRange(f1StartDate, f1EndDate) + ",";
                    //    }
                    //}
                    else if (f1AttributeGroupName == "Decision Counselor")
                    {
                        if (f1AttributeName == "Decision Counselor")
                        {
                            if (f1Comment != null)
                            {
                                attributeValues.AttributeId = 1059;
                                attributeValues.EntityId = (int)personId;
                                attributeValues.Value = f1Comment;
                            }
                            //else if (f1StartDate != null)
                            //{
                            //    attributeValues.AttributeId = 1060;
                            //    attributeValues.EntityId = (int)personId;
                            //    attributeValues.Value = DateRange(f1StartDate, f1EndDate);
                            //}
                        }
                    }
                    else if (f1AttributeGroupName == "Discovering Crossroads")
                    {
                        if (f1AttributeName == "Completed Embrace")
                        {
                            //Checks if current person already has this Rock Attribute and will append the current F1 attribute value to the Rock one.
                            if (newAttributes.Find(a => (a.AttributeId == 1019 && a.EntityId == personId)) != null)
                            {
                                ReportProgress(0, string.Format("Adding Multi-Select Embrace"));
                                newAttributes.Find(a => a.AttributeId == 1019 && a.EntityId == personId).Value += "Embrace,";
                                //if (f1Comment != null)
                                //{
                                //    attributeExtraDate.AttributeId = 1076;
                                //    attributeExtraDate.EntityId = (int)personId;
                                //    attributeExtraDate.Value = f1Comment;
                                //}
                                //else
                                //{
                                //    attributeExtraDate.AttributeId = 1076;
                                //    attributeExtraDate.EntityId = (int)personId;
                                //    attributeExtraDate.Value = "5/1/2100";
                                //}
                            }
                            //if current person does not have these attributes
                            else
                            {
                                attributeValues.AttributeId = 1019;
                                attributeValues.EntityId = (int)personId;
                                attributeValues.Value = "Embrace,";

                                //if (f1Comment != null)
                                //{
                                //    attributeExtraDate.AttributeId = 1076;
                                //    attributeExtraDate.EntityId = (int)personId;
                                //    attributeExtraDate.Value = f1Comment;
                                //}
                                //else
                                //{
                                //    attributeExtraDate.AttributeId = 1076;
                                //    attributeExtraDate.EntityId = (int)personId;
                                //    attributeExtraDate.Value = "5/1/2100";
                                //}
                            }
                        }
                        else if (f1AttributeName == "Completed Engage")
                        {
                            //Checks if current person already has this Rock Attribute and will append the current F1 attribute value to the Rock one.
                            if (newAttributes.Find(a => (a.AttributeId == 1019 && a.EntityId == personId)) != null)
                            {
                                ReportProgress(0, string.Format("Adding Multi-Select Engage"));
                                newAttributes.Find(a => a.AttributeId == 1019 && a.EntityId == personId).Value += "Engage,";
                                //if (f1Comment != null)
                                //{
                                //    attributeExtraDate.AttributeId = 1077;
                                //    attributeExtraDate.EntityId = (int)personId;
                                //    attributeExtraDate.Value = f1Comment;
                                //}
                                //else
                                //{
                                //    attributeExtraDate.AttributeId = 1077;
                                //    attributeExtraDate.EntityId = (int)personId;
                                //    attributeExtraDate.Value = "5/1/2100";
                                //}
                            }
                            //if current person does not have these attributes
                            else
                            {
                                attributeValues.AttributeId = 1019;
                                attributeValues.EntityId = (int)personId;
                                attributeValues.Value = "Engage,";

                                //if (f1Comment != null)
                                //{
                                //    attributeExtraDate.AttributeId = 1077;
                                //    attributeExtraDate.EntityId = (int)personId;
                                //    attributeExtraDate.Value = f1Comment;
                                //}
                                //else
                                //{
                                //    attributeExtraDate.AttributeId = 1077;
                                //    attributeExtraDate.EntityId = (int)personId;
                                //    attributeExtraDate.Value = "5/1/2100";
                                //}
                            }
                        }
                        else if (f1AttributeName == "Completed Enlist")
                        {
                            //Checks if current person already has this Rock Attribute and will append the current F1 attribute value to the Rock one.
                            if (newAttributes.Find(a => (a.AttributeId == 1019 && a.EntityId == personId)) != null)
                            {
                                ReportProgress(0, string.Format("Adding Multi-Select Enlist"));
                                newAttributes.Find(a => a.AttributeId == 1019 && a.EntityId == personId).Value += "Enlist,";
                                //if (f1Comment != null)
                                //{
                                //    attributeExtraDate.AttributeId = 1078;
                                //    attributeExtraDate.EntityId = (int)personId;
                                //    attributeExtraDate.Value = f1Comment;
                                //}
                                //else
                                //{
                                //    attributeExtraDate.AttributeId = 1078;
                                //    attributeExtraDate.EntityId = (int)personId;
                                //    attributeExtraDate.Value = "5/1/2100";
                                //}
                            }
                            //if current person does not have these attributes
                            else
                            {
                                attributeValues.AttributeId = 1019;
                                attributeValues.EntityId = (int)personId;
                                attributeValues.Value = "Enlist,";

                                //if (f1Comment != null)
                                //{
                                //    attributeExtraDate.AttributeId = 1078;
                                //    attributeExtraDate.EntityId = (int)personId;
                                //    attributeExtraDate.Value = f1Comment;
                                //}
                                //else
                                //{
                                //    attributeExtraDate.AttributeId = 1078;
                                //    attributeExtraDate.EntityId = (int)personId;
                                //    attributeExtraDate.Value = "5/1/2100";
                                //}
                            }
                        }
                        else if (f1AttributeName == "Completed Explore")
                        {
                            //Checks if current person already has this Rock Attribute and will append the current F1 attribute value to the Rock one.
                            if (newAttributes.Find(a => (a.AttributeId == 1019 && a.EntityId == personId)) != null)
                            {
                                ReportProgress(0, string.Format("Adding Multi-Select Explore"));
                                newAttributes.Find(a => a.AttributeId == 1019 && a.EntityId == personId).Value += "Explore,";
                                //if (f1Comment != null)
                                //{
                                //    attributeExtraDate.AttributeId = 1079;
                                //    attributeExtraDate.EntityId = (int)personId;
                                //    attributeExtraDate.Value = f1Comment;
                                //}
                                //else
                                //{
                                //    attributeExtraDate.AttributeId = 1079;
                                //    attributeExtraDate.EntityId = (int)personId;
                                //    attributeExtraDate.Value = "5/1/2100";
                                //}
                            }
                            //if current person does not have these attributes
                            else
                            {
                                attributeValues.AttributeId = 1019;
                                attributeValues.EntityId = (int)personId;
                                attributeValues.Value = "Explore,";

                                //if (f1Comment != null)
                                //{
                                //    attributeExtraDate.AttributeId = 1079;
                                //    attributeExtraDate.EntityId = (int)personId;
                                //    attributeExtraDate.Value = f1Comment;
                                //}
                                //else
                                //{
                                //    attributeExtraDate.AttributeId = 1079;
                                //    attributeExtraDate.EntityId = (int)personId;
                                //    attributeExtraDate.Value = "5/1/2100";
                                //}
                            }
                        }
                        //else if (f1AttributeName == "Dropout")
                        //{
                        //    attributeValues.AttributeId = 1067;
                        //    attributeValues.EntityId = (int)personId;
                        //    attributeValues.Value = "11/3/2010";

                        //}
                        //else if (f1AttributeName == "Explore Crossroads MIA")
                        //{

                        //    if (f1Comment != null)
                        //    {
                        //        attributeValues.AttributeId = 1133;
                        //        attributeValues.EntityId = (int)personId;
                        //        attributeValues.Value = f1Comment;
                        //    }
                        //    else
                        //    {
                        //        attributeValues.AttributeId = 1133;
                        //        attributeValues.EntityId = (int)personId;
                        //        attributeValues.Value = "5/1/2014";
                        //    }
                        //}
                        else if (f1AttributeName == "Need Embrace")
                        {
                            //Checks if current person already has this Rock Attribute and will append the current F1 attribute value to the Rock one.
                            if (newAttributes.Find(a => (a.AttributeId == 1138 && a.EntityId == personId)) != null)
                            {
                                ReportProgress(0, string.Format("Adding Multi-Select Need Embrace"));
                                newAttributes.Find(a => a.AttributeId == 1138 && a.EntityId == personId).Value += "Embrace,";
                            }
                            //if current person does not have these attributes
                            else
                            {
                                attributeValues.AttributeId = 1138;
                                attributeValues.EntityId = (int)personId;
                                attributeValues.Value = "Embrace,";
                            }
                        }

                        else if (f1AttributeName == "Need Engage")
                        {
                            //Checks if current person already has this Rock Attribute and will append the current F1 attribute value to the Rock one.
                            if (newAttributes.Find(a => (a.AttributeId == 1138 && a.EntityId == personId)) != null)
                            {
                                ReportProgress(0, string.Format("Adding Multi-Select Need Engage"));
                                newAttributes.Find(a => a.AttributeId == 1138 && a.EntityId == personId).Value += "Engage,";
                            }
                            //if current person does not have these attributes
                            else
                            {
                                attributeValues.AttributeId = 1138;
                                attributeValues.EntityId = (int)personId;
                                attributeValues.Value = "Engage,";
                            }
                        }
                        else if (f1AttributeName == "Need Enlist")
                        {
                            //Checks if current person already has this Rock Attribute and will append the current F1 attribute value to the Rock one.
                            if (newAttributes.Find(a => (a.AttributeId == 1138 && a.EntityId == personId)) != null)
                            {
                                ReportProgress(0, string.Format("Adding Multi-Select Need Enlist"));
                                newAttributes.Find(a => a.AttributeId == 1138 && a.EntityId == personId).Value += "Enlist,";
                            }
                            //if current person does not have these attributes
                            else
                            {
                                attributeValues.AttributeId = 1138;
                                attributeValues.EntityId = (int)personId;
                                attributeValues.Value = "Enlist,";
                            }
                        }
                    }
                    else if (f1AttributeGroupName == "Drop")
                    {
                        if (f1Comment != null)
                        {
                            attributeValues.AttributeId = 1066;
                            attributeValues.EntityId = (int)personId;
                            attributeValues.Value = f1Comment;
                        }
                        if (f1StartDate != null)
                        {
                            attributeExtraDate.AttributeId = 1065;
                            attributeExtraDate.EntityId = (int)personId;
                            attributeExtraDate.Value = DateRange(f1StartDate, f1EndDate) + ",";
                        }
                    }
                    else if (f1AttributeGroupName == "Experiences")
                    {
                        //if (f1AttributeName == "Background Checks")
                        //{
                        //    if (f1Comment != null)
                        //    {
                        //        attributeValues.AttributeId = 979;
                        //        attributeValues.EntityId = (int)personId;
                        //        attributeValues.Value = f1Comment;
                        //    }
                        //    else if (f1StartDate != null)
                        //    {
                        //        attributeExtraDate.AttributeId = 980;
                        //        attributeExtraDate.EntityId = (int)personId;
                        //        attributeExtraDate.Value = DateRange(f1StartDate, f1EndDate) + ",";
                        //    }
                        //}
                        if (f1AttributeName == "Baptism")
                        {
                            //if (f1Comment != null)
                            //{
                            //    //Checks if current person already has this Rock Attribute and will append the current F1 attribute value to the Rock one.
                            //    if (newAttributes.Find(a => (a.AttributeId == 985 && a.EntityId == personId)) != null)
                            //    {
                            //        ReportProgress(0, string.Format("Adding Baptism (experience)"));
                            //        newAttributes.Find(a => a.AttributeId == 985 && a.EntityId == personId).Value += "; " + DateRange(f1StartDate, f1EndDate) + " - " + f1Comment;
                            //    }
                            //    else
                            //    {
                            //        attributeValues.AttributeId = 985;
                            //        attributeValues.EntityId = (int)personId;
                            //        attributeValues.Value = f1Comment;
                            //    }
                            //}
                            if (f1StartDate != null)
                            {
                                //Checks if current person already has this Rock Attribute and will append the current F1 attribute value to the Rock one.
                                if (newAttributes.Find(a => (a.AttributeId == 174 && a.EntityId == personId)) != null)
                                {
                                    ReportProgress(0, string.Format("Adding Baptism (experience) Date"));
                                    newAttributes.Find(a => a.AttributeId == 174 && a.EntityId == personId).Value += "; " + f1Comment;
                                }
                                //else if (newAttributes.Find(a => (a.AttributeId == 714 && a.EntityId == personId)) != null)
                                //{
                                //    ReportProgress(0, string.Format("Adding Baptism (experience) Date"));
                                //    //newAttributes.Find(a => a.AttributeId == 714 && a.EntityId == personId).Value += "; " + f1Comment;
                                //}
                                else
                                {
                                    //Baptism Date
                                    attributeExtraDate.AttributeId = 174;
                                    attributeExtraDate.EntityId = (int)personId;
                                    attributeExtraDate.Value = DateRange(f1StartDate, f1EndDate) + ",";

                                    //Baptized Here
                                    attributeExtraComment.AttributeId = 714;
                                    attributeExtraComment.EntityId = (int)personId;
                                    attributeExtraComment.Value = "True";
                                }
                            }
                        }
                        //else if (f1AttributeName == "Completed ABCs Class")
                        //{
                        //    if (f1StartDate != null)
                        //    {
                        //        attributeValues.AttributeId = 1018;
                        //        attributeValues.EntityId = (int)personId;
                        //        attributeValues.Value = DateRange(f1StartDate, f1EndDate);
                        //    }
                        //}
                        else if (f1AttributeName == "Completed Connect@Crossroads")
                        {
                            if (f1StartDate != null)
                            {
                                attributeValues.AttributeId = 1020;
                                attributeValues.EntityId = (int)personId;
                                attributeValues.Value = DateRange(f1StartDate, f1EndDate) + ",";
                            }
                        }
                        else if (f1AttributeName == "CPR Certified")
                        {
                            //Checks if current person already has this Rock Attribute and will append the current F1 attribute value to the Rock one.
                            if (newAttributes.Find(a => (a.AttributeId == 1040 && a.EntityId == personId)) != null)
                            {
                                ReportProgress(0, string.Format("Adding CPR from Experiences"));
                                newAttributes.Find(a => a.AttributeId == 1040 && a.EntityId == personId).Value += "; " + DateRange(f1StartDate, f1EndDate) + " - " + f1Comment;
                                //newAttributes.Find(a => a.AttributeId == 1039 && a.EntityId == personId).Value += DateRange(f1StartDate, f1EndDate) + ",";
                            }
                            else
                            {
                                if (f1Comment != null)
                                {
                                    attributeValues.AttributeId = 1040;
                                    attributeValues.EntityId = (int)personId;
                                    attributeValues.Value = f1Comment;
                                }
                                if (f1StartDate != null)
                                {
                                    attributeExtraDate.AttributeId = 1039;
                                    attributeExtraDate.EntityId = (int)personId;
                                    attributeExtraDate.Value = DateRange(f1StartDate, f1EndDate) + ",";
                                }
                            }
                        }
                        else if (f1AttributeName == "DC Grad")
                        {
                            //if (f1Comment != null)
                            //{
                            //    attributeValues.AttributeId = 1055;
                            //    attributeValues.EntityId = (int)personId;
                            //    attributeValues.Value = f1Comment;
                            //}
                            if (f1StartDate != null)
                            {
                                attributeExtraDate.AttributeId = 1056;
                                attributeExtraDate.EntityId = (int)personId;
                                attributeExtraDate.Value = DateRange(f1StartDate, f1EndDate) + ",";
                            }
                        }
                        else if (f1AttributeName == "Deacon")
                        {
                            if (f1StartDate != null)
                            {
                                if (f1EndDate != null)
                                {
                                    attributeValues.AttributeId = 1058;
                                    attributeValues.EntityId = (int)personId;
                                    attributeValues.Value = DateRange(f1StartDate, f1EndDate);
                                }
                                else
                                {
                                    attributeExtraDate.AttributeId = 1058;
                                    attributeExtraDate.EntityId = (int)personId;
                                    attributeExtraDate.Value = DateRange(f1StartDate, f1EndDate) + ",";
                                }
                            }
                        }
                        else if (f1AttributeName == "Dedication")
                        {
                            if (f1Comment != null)
                            {
                                attributeValues.AttributeId = 1063;
                                attributeValues.EntityId = (int)personId;
                                attributeValues.Value = f1Comment;
                            }
                            else if (f1StartDate != null)
                            {
                                attributeExtraDate.AttributeId = 1064;
                                attributeExtraDate.EntityId = (int)personId;
                                attributeExtraDate.Value = DateRange(f1StartDate, f1EndDate) + ",";
                            }
                        }
                        else if (f1AttributeName == "Elder")
                        {
                            if (f1StartDate != null && f1EndDate != null)
                            {
                                attributeValues.AttributeId = 1070;
                                attributeValues.EntityId = (int)personId;
                                attributeValues.Value = DateRange(f1StartDate, f1EndDate);
                            }
                        }
                        //else if (f1AttributeName == "Leadership 2005-2006")
                        //{
                        //    if (f1Comment != null)
                        //    {
                        //        attributeValues.AttributeId = 1116;
                        //        attributeValues.EntityId = (int)personId;
                        //        attributeValues.Value = f1Comment;
                        //    }
                        //}
                        else if (f1AttributeName == "Marriage") // Rock Anniversaries. I think this is just Single Date Format with Date span shown in person profile.
                        {
                            if (f1Comment != null && f1Comment == "2009")
                            {
                                attributeValues.AttributeId = 1121;
                                attributeValues.EntityId = (int)personId;
                                attributeValues.Value = "1/1/2009";
                            }
                            else if (f1StartDate != null)
                            {
                                attributeValues.AttributeId = 1121;
                                attributeValues.EntityId = (int)personId;
                                attributeValues.Value = DateRange(f1StartDate, f1EndDate);

                            }
                        }
                        else if (f1AttributeName == "Prayer")
                        {
                            if (f1Comment != null)
                            {
                                //Checks if current person already has this Rock Attribute and will append the current F1 attribute value to the Rock one.
                                if (newAttributes.Find(a => (a.AttributeId == 1151 && a.EntityId == personId)) != null)
                                {
                                    ReportProgress(0, string.Format("Adding Prayer from Experiences"));
                                    newAttributes.Find(a => a.AttributeId == 1151 && a.EntityId == personId).Value += "; " + DateRange(f1StartDate, f1EndDate) + " - " + f1Comment;
                                }
                                else
                                {
                                    attributeValues.AttributeId = 1151;
                                    attributeValues.EntityId = (int)personId;
                                    attributeValues.Value = DateRange(f1StartDate, f1EndDate) + " - " + f1Comment;
                                }
                            }
                            else if (f1StartDate != null)
                            {
                                if (f1EndDate != null)
                                {
                                    attributeExtraDate.AttributeId = 1152;
                                    attributeExtraDate.EntityId = (int)personId;
                                    attributeExtraDate.Value = DateRange(f1StartDate, f1EndDate);
                                }
                                else
                                {
                                    attributeExtraDate.AttributeId = 1152;
                                    attributeExtraDate.EntityId = (int)personId;
                                    attributeExtraDate.Value = DateRange(f1StartDate, f1EndDate) + ",";
                                }
                            }
                        }
                        else if (f1AttributeName == "Rededication")
                        {
                            //if (f1Comment != null)
                            //{
                            //    //Checks if current person already has this Rock Attribute and will append the current F1 attribute value to the Rock one.
                            //    if (newAttributes.Find(a => (a.AttributeId == 1168 && a.EntityId == personId)) != null)
                            //    {
                            //        ReportProgress(0, string.Format("Adding Rededication from Experiences"));
                            //        newAttributes.Find(a => a.AttributeId == 1168 && a.EntityId == personId).Value += "; " + DateRange(f1StartDate, f1EndDate) + " - " + f1Comment;
                            //    }
                            //    else
                            //    {
                            //        attributeValues.AttributeId = 1168;
                            //        attributeValues.EntityId = (int)personId;
                            //        attributeValues.Value = DateRange(f1StartDate, f1EndDate) + " - " + f1Comment;
                            //    }
                            //}
                            if (f1StartDate != null)
                            {
                                if (f1EndDate != null)
                                {
                                    attributeExtraDate.AttributeId = 1169;
                                    attributeExtraDate.EntityId = (int)personId;
                                    attributeExtraDate.Value = DateRange(f1StartDate, f1EndDate);
                                }
                                else
                                {
                                    attributeExtraDate.AttributeId = 1169;
                                    attributeExtraDate.EntityId = (int)personId;
                                    attributeExtraDate.Value = DateRange(f1StartDate, f1EndDate) + ",";
                                }
                            }
                        }

                    }
                    else if (f1AttributeGroupName == "Family Camp Out")
                    {
                        attributeValues.AttributeId = 1084;
                        attributeValues.EntityId = (int)personId;
                        attributeValues.Value = "2C8B55AF-B5E2-41F9-9E08-C2E6F4624550";
                    }
                    //  Will have to manually add these because if they do mulitple tournaments, Excavator will overwrite the value instead of appending a new one.
                    if (f1AttributeGroupName == "Golf Tournament")
                    {
                        if (f1AttributeName == "2007 Spring Classic")
                        {
                            //Checks if current person already has this Rock Attribute and will append the current F1 attribute value to the Rock one.
                            if (newAttributes.Find(a => (a.AttributeId == 1097 && a.EntityId == personId)) != null)
                            {
                                ReportProgress(0, string.Format("Adding 2007 Spring Classic"));
                                newAttributes.Find(a => a.AttributeId == 1097 && a.EntityId == personId).Value += "2007 Spring Classic,";
                            }
                            else
                            {
                                attributeValues.AttributeId = 1097;
                                attributeValues.EntityId = (int)personId;
                                attributeValues.Value = "2007 Spring Classic,";
                            }

                        }
                        if (f1AttributeName == "2008 Spring Classic")
                        {
                            // Checks if current person already has this Rock Attribute and will append the current F1 attribute value to the Rock one.
                            if (newAttributes.Find(a => (a.AttributeId == 1097 && a.EntityId == personId)) != null)
                            {
                                ReportProgress(0, string.Format("Adding 2008 Spring Classic"));
                                newAttributes.Find(a => a.AttributeId == 1097 && a.EntityId == personId).Value += "2008 Spring Classic,";
                            }
                            else
                            {
                                attributeValues.AttributeId = 1097;
                                attributeValues.EntityId = (int)personId;
                                attributeValues.Value = "2008 Spring Classic,";
                            }
                        }
                        if (f1AttributeName == "2009 Fall Classic")
                        {
                            // Checks if current person already has this Rock Attribute and will append the current F1 attribute value to the Rock one.
                            if (newAttributes.Find(a => (a.AttributeId == 1097 && a.EntityId == personId)) != null)
                            {
                                ReportProgress(0, string.Format("Adding 2009 Spring Classic"));
                                newAttributes.Find(a => a.AttributeId == 1097 && a.EntityId == personId).Value += "2009 Fall Classic,";
                            }
                            else
                            {
                                attributeValues.AttributeId = 1097;
                                attributeValues.EntityId = (int)personId;
                                attributeValues.Value = "2009 Fall Classic,";
                            }
                        }
                        if (f1AttributeName == "2010 Spring Classic")
                        {
                            //Checks if current person already has this Rock Attribute and will append the current F1 attribute value to the Rock one.
                            if (newAttributes.Find(a => (a.AttributeId == 1097 && a.EntityId == personId)) != null)
                            {
                                ReportProgress(0, string.Format("Adding 2010 Spring Classic"));
                                newAttributes.Find(a => a.AttributeId == 1097 && a.EntityId == personId).Value += "2010 Spring Classic,";
                            }
                            else
                            {
                                attributeValues.AttributeId = 1097;
                                attributeValues.EntityId = (int)personId;
                                attributeValues.Value = "2010 Spring Classic,";
                            }
                        }
                        if (f1AttributeName == "2010 Fall Classic")
                        {
                            //Checks if current person already has this Rock Attribute and will append the current F1 attribute value to the Rock one.
                            if (newAttributes.Find(a => (a.AttributeId == 1097 && a.EntityId == personId)) != null)
                            {
                                ReportProgress(0, string.Format("Adding 2010 Fall Classic"));
                                newAttributes.Find(a => a.AttributeId == 1097 && a.EntityId == personId).Value += "2010 Fall Classic,";
                            }
                            else
                            {
                                attributeValues.AttributeId = 1097;
                                attributeValues.EntityId = (int)personId;
                                attributeValues.Value = "2010 Fall Classic,";
                            }
                        }
                        if (f1AttributeName == "2011 Fall Classic")
                        {
                            //Checks if current person already has this Rock Attribute and will append the current F1 attribute value to the Rock one.
                            if (newAttributes.Find(a => (a.AttributeId == 1097 && a.EntityId == personId)) != null)
                            {
                                ReportProgress(0, string.Format("Adding 2011 Fall Classic"));
                                newAttributes.Find(a => a.AttributeId == 1097 && a.EntityId == personId).Value += "2011 Fall Classic,";
                            }
                            else
                            {
                                attributeValues.AttributeId = 1097;
                                attributeValues.EntityId = (int)personId;
                                attributeValues.Value = "2011 Fall Classic,";
                            }
                        }
                        if (f1AttributeName == "2012 Spring Classic")
                        {
                            //Checks if current person already has this Rock Attribute and will append the current F1 attribute value to the Rock one.
                            if (newAttributes.Find(a => (a.AttributeId == 1097 && a.EntityId == personId)) != null)
                            {
                                ReportProgress(0, string.Format("Adding 2012 Spring Classic"));
                                newAttributes.Find(a => a.AttributeId == 1097 && a.EntityId == personId).Value += "2012 Spring Classic,";
                            }
                            else
                            {
                                attributeValues.AttributeId = 1097;
                                attributeValues.EntityId = (int)personId;
                                attributeValues.Value = "2012 Spring Classic,";
                            }
                        }
                        if (f1AttributeName == "2013 Spring Classic")
                        {
                            //Checks if current person already has this Rock Attribute and will append the current F1 attribute value to the Rock one.
                            if (newAttributes.Find(a => (a.AttributeId == 1097 && a.EntityId == personId)) != null)
                            {
                                ReportProgress(0, string.Format("Adding 2013 Spring Classic"));
                                newAttributes.Find(a => a.AttributeId == 1097 && a.EntityId == personId).Value += "2013 Spring Classic,";
                            }
                            else
                            {
                                attributeValues.AttributeId = 1097;
                                attributeValues.EntityId = (int)personId;
                                attributeValues.Value = "2013 Spring Classic,";
                            }
                        }
                        if (f1AttributeName == "2014 Spring Classic")
                        {
                            //Checks if current person already has this Rock Attribute and will append the current F1 attribute value to the Rock one.
                            if (newAttributes.Find(a => (a.AttributeId == 1097 && a.EntityId == personId)) != null)
                            {
                                ReportProgress(0, string.Format("Adding 2014 Spring Classic"));
                                newAttributes.Find(a => a.AttributeId == 1097 && a.EntityId == personId).Value += "2014 Spring Classic,";
                            }
                            else
                            {
                                attributeValues.AttributeId = 1097;
                                attributeValues.EntityId = (int)personId;
                                attributeValues.Value = "2014 Spring Classic,";
                            }
                        }
                    }

                    else if (f1AttributeGroupName == "Leadership")
                    {
                        if (f1AttributeName == "Bookstore")
                        {
                            if (f1StartDate != null)
                            {
                                if (f1EndDate != null)
                                {
                                    attributeValues.AttributeId = 990;
                                    attributeValues.EntityId = (int)personId;
                                    attributeValues.Value = DateRange(f1StartDate, f1EndDate);
                                }
                                else
                                {
                                    attributeValues.AttributeId = 990;
                                    attributeValues.EntityId = (int)personId;
                                    attributeValues.Value = DateRange(f1StartDate, f1EndDate) + ",";
                                }
                            }
                            else
                            {
                                attributeValues.AttributeId = 990;
                                attributeValues.EntityId = (int)personId;
                                attributeValues.Value = "1/1/2100";
                            }
                        }
                        else if (f1AttributeName == "Children")
                        {
                            if (f1StartDate != null)
                            {
                                if (f1EndDate != null)
                                {
                                    attributeValues.AttributeId = 1006;
                                    attributeValues.EntityId = (int)personId;
                                    attributeValues.Value = DateRange(f1StartDate, f1EndDate);
                                }
                                else
                                {
                                    attributeValues.AttributeId = 1006;
                                    attributeValues.EntityId = (int)personId;
                                    attributeValues.Value = DateRange(f1StartDate, f1EndDate) + ",";
                                }
                            }
                            else
                            {
                                attributeValues.AttributeId = 1006;
                                attributeValues.EntityId = (int)personId;
                                attributeValues.Value = "1/1/2100";
                            }
                        }
                        else if (f1AttributeName == "College")
                        {
                            if (f1StartDate != null)
                            {
                                if (f1EndDate != null)
                                {
                                    attributeValues.AttributeId = 1015;
                                    attributeValues.EntityId = (int)personId;
                                    attributeValues.Value = DateRange(f1StartDate, f1EndDate);
                                }
                                else
                                {
                                    attributeValues.AttributeId = 1015;
                                    attributeValues.EntityId = (int)personId;
                                    attributeValues.Value = DateRange(f1StartDate, f1EndDate) + ",";
                                }
                            }
                            else
                            {
                                attributeValues.AttributeId = 1015;
                                attributeValues.EntityId = (int)personId;
                                attributeValues.Value = "1/1/2100";
                            }

                        }
                        else if (f1AttributeName == "Connect Group Leader")
                        {
                            if (f1Comment != null)
                            {
                                //Checks if current person already has this Rock Attribute and will append the current F1 attribute value to the Rock one.
                                if (newAttributes.Find(a => (a.AttributeId == 1029 && a.EntityId == personId)) != null)
                                {
                                    ReportProgress(0, string.Format("Adding Connect Group Leader Comment"));
                                    newAttributes.Find(a => a.AttributeId == 1029 && a.EntityId == personId).Value += "; " + f1Comment;
                                }
                                else
                                {
                                    attributeValues.AttributeId = 1029;
                                    attributeValues.EntityId = (int)personId;
                                    attributeValues.Value = f1Comment;
                                }
                            }
                            if (f1StartDate != null)
                            {
                                DateTime startDate = (DateTime)f1StartDate;

                                if (startDate.Year == 2013)
                                {
                                    switch (startDate.Month)
                                    {
                                        //Months for Summer CG
                                        case 5:
                                        case 6:
                                        case 7:
                                            if (newAttributes.Find(a => (a.AttributeId == 1028 && a.EntityId == personId)) != null)
                                            {
                                                ReportProgress(0, string.Format("Adding Connect Group Leader 2013 Summer"));
                                                newAttributes.Find(a => a.AttributeId == 1028 && a.EntityId == personId).Value += "DD20341B-411D-4A04-BFC3-9C20485DBEA3,";
                                            }
                                            else
                                            {
                                                attributeExtraDate.AttributeId = 1028;
                                                attributeExtraDate.EntityId = (int)personId;
                                                attributeValues.Value = "DD20341B-411D-4A04-BFC3-9C20485DBEA3" + ",";
                                            }
                                            break;
                                        //Months for Fall CG
                                        case 08:
                                        case 09:
                                        case 10:
                                        case 11:
                                        case 12:

                                            if (newAttributes.Find(a => (a.AttributeId == 1028 && a.EntityId == personId)) != null)
                                            {
                                                ReportProgress(0, string.Format("Adding Connect Group Leader 2013 Fall"));
                                                newAttributes.Find(a => a.AttributeId == 1028 && a.EntityId == personId).Value += "BA998BEC-4371-4279-8F70-DA00A2AE0F64,";
                                            }
                                            else
                                            {
                                                attributeExtraDate.AttributeId = 1028;
                                                attributeExtraDate.EntityId = (int)personId;
                                                attributeExtraDate.Value = "BA998BEC-4371-4279-8F70-DA00A2AE0F64" + ",";
                                            }
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                else if (startDate.Year == 2014)
                                {
                                    switch (startDate.Month)
                                    {
                                        //Months for Winter CG
                                        case 1:
                                        case 2:
                                            if (newAttributes.Find(a => (a.AttributeId == 1028 && a.EntityId == personId)) != null)
                                            {
                                                ReportProgress(0, string.Format("Adding Connect Group Leader 2014 Winter"));
                                                newAttributes.Find(a => a.AttributeId == 1028 && a.EntityId == personId).Value += "7AAD4061-B9B1-4FCA-BE88-3EEABC9E467E,";
                                            }
                                            else
                                            {
                                                attributeExtraDate.AttributeId = 1028;
                                                attributeExtraDate.EntityId = (int)personId;
                                                attributeExtraDate.Value = "7AAD4061-B9B1-4FCA-BE88-3EEABC9E467E" + ",";
                                            }
                                            break;
                                        //Months for Spring CG
                                        case 3:
                                        case 4:
                                        case 5:
                                            if (newAttributes.Find(a => (a.AttributeId == 1028 && a.EntityId == personId)) != null)
                                            {
                                                ReportProgress(0, string.Format("Adding Connect Group Leader 2014 Spring"));
                                                newAttributes.Find(a => a.AttributeId == 1028 && a.EntityId == personId).Value += "580909EC-0EE4-4141-942A-7400C53509EF,";
                                            }
                                            else
                                            {
                                                attributeExtraDate.AttributeId = 1028;
                                                attributeExtraDate.EntityId = (int)personId;
                                                attributeExtraDate.Value = "580909EC-0EE4-4141-942A-7400C53509EF" + ",";
                                            }
                                            break;
                                        case 8:
                                        case 9:
                                        case 10:
                                        case 11:
                                            if (newAttributes.Find(a => (a.AttributeId == 1028 && a.EntityId == personId)) != null)
                                            {
                                                ReportProgress(0, string.Format("Adding Connect Group Leader 2014 Fall"));
                                                newAttributes.Find(a => a.AttributeId == 1028 && a.EntityId == personId).Value += "DD6E1BFC-7458-4804-805D-16D5144AACE8,";
                                            }
                                            else
                                            {
                                                attributeExtraDate.AttributeId = 1028;
                                                attributeExtraDate.EntityId = (int)personId;
                                                attributeExtraDate.Value = "DD6E1BFC-7458-4804-805D-16D5144AACE8" + ",";
                                            }
                                            break;
                                        default:
                                            break;
                                    }
                                }
                            }
                        }
                        else if (f1AttributeName == "Deacons")
                        {
                            if (f1StartDate != null)
                            {
                                if (f1EndDate != null)
                                {
                                    attributeValues.AttributeId = 1058;
                                    attributeValues.EntityId = (int)personId;
                                    attributeValues.Value = DateRange(f1StartDate, f1EndDate);
                                }
                                else
                                {
                                    attributeValues.AttributeId = 1058;
                                    attributeValues.EntityId = (int)personId;
                                    attributeValues.Value = DateRange(f1StartDate, f1EndDate) + ",";
                                }
                            }
                            else
                            {
                                attributeValues.AttributeId = 1058;
                                attributeValues.EntityId = (int)personId;
                                attributeValues.Value = "1/1/2100";
                            }
                        }
                        else if (f1AttributeName == "Decision Counselors")
                        {
                            if (f1StartDate != null)
                            {
                                if (f1EndDate != null)
                                {
                                    attributeValues.AttributeId = 1061;
                                    attributeValues.EntityId = (int)personId;
                                    attributeValues.Value = DateRange(f1StartDate, f1EndDate);
                                }
                                else
                                {
                                    attributeValues.AttributeId = 1061;
                                    attributeValues.EntityId = (int)personId;
                                    attributeValues.Value = DateRange(f1StartDate, f1EndDate) + ",";
                                }
                            }
                            else
                            {
                                attributeValues.AttributeId = 1061;
                                attributeValues.EntityId = (int)personId;
                                attributeValues.Value = "1/1/2100";
                            }
                        }
                        else if (f1AttributeName == "Elders")
                        {
                            if (f1StartDate != null)
                            {
                                if (f1EndDate != null)
                                {
                                    attributeValues.AttributeId = 1070;
                                    attributeValues.EntityId = (int)personId;
                                    attributeValues.Value = DateRange(f1StartDate, f1EndDate);
                                }
                                else
                                {
                                    attributeValues.AttributeId = 1070;
                                    attributeValues.EntityId = (int)personId;
                                    attributeValues.Value = DateRange(f1StartDate, f1EndDate) + ",";
                                }
                            }
                            else
                            {
                                attributeValues.AttributeId = 1070;
                                attributeValues.EntityId = (int)personId;
                                attributeValues.Value = "1/1/2100";
                            }
                        }
                        else if (f1AttributeName == "Food Services")
                        {
                            if (f1StartDate != null)
                            {
                                if (f1EndDate != null)
                                {
                                    attributeValues.AttributeId = 1089;
                                    attributeValues.EntityId = (int)personId;
                                    attributeValues.Value = DateRange(f1StartDate, f1EndDate);
                                }
                                else
                                {
                                    attributeValues.AttributeId = 1089;
                                    attributeValues.EntityId = (int)personId;
                                    attributeValues.Value = DateRange(f1StartDate, f1EndDate) + ",";
                                }
                            }
                            else
                            {
                                attributeValues.AttributeId = 1089;
                                attributeValues.EntityId = (int)personId;
                                attributeValues.Value = "1/1/2100";
                            }
                        }
                        else if (f1AttributeName == "Helping Hands")
                        {
                            if (f1StartDate != null)
                            {
                                if (f1EndDate != null)
                                {
                                    attributeValues.AttributeId = 1100;
                                    attributeValues.EntityId = (int)personId;
                                    attributeValues.Value = DateRange(f1StartDate, f1EndDate);
                                }
                                else
                                {
                                    attributeValues.AttributeId = 1100;
                                    attributeValues.EntityId = (int)personId;
                                    attributeValues.Value = DateRange(f1StartDate, f1EndDate) + ",";
                                }
                            }
                            else
                            {
                                attributeValues.AttributeId = 1100;
                                attributeValues.EntityId = (int)personId;
                                attributeValues.Value = "1/1/2100";
                            }
                        }
                        else if (f1AttributeName == "Media")
                        {
                            if (f1StartDate != null)
                            {
                                if (f1EndDate != null)
                                {
                                    attributeValues.AttributeId = 1123;
                                    attributeValues.EntityId = (int)personId;
                                    attributeValues.Value = DateRange(f1StartDate, f1EndDate);
                                }
                                else
                                {
                                    attributeValues.AttributeId = 1123;
                                    attributeValues.EntityId = (int)personId;
                                    attributeValues.Value = DateRange(f1StartDate, f1EndDate) + ",";
                                }
                            }
                            else
                            {
                                attributeValues.AttributeId = 1123;
                                attributeValues.EntityId = (int)personId;
                                attributeValues.Value = "1/1/2100";
                            }
                        }
                        else if (f1AttributeName == "Men")
                        {
                            if (f1StartDate != null)
                            {
                                if (f1EndDate != null)
                                {
                                    attributeValues.AttributeId = 1130;
                                    attributeValues.EntityId = (int)personId;
                                    attributeValues.Value = DateRange(f1StartDate, f1EndDate);
                                }
                                else
                                {
                                    attributeValues.AttributeId = 1130;
                                    attributeValues.EntityId = (int)personId;
                                    attributeValues.Value = DateRange(f1StartDate, f1EndDate) + ",";
                                }
                            }
                            else
                            {
                                attributeValues.AttributeId = 1130;
                                attributeValues.EntityId = (int)personId;
                                attributeValues.Value = "1/1/2100";
                            }
                        }
                        else if (f1AttributeName == "Mother's Day Out")
                        {
                            if (f1StartDate != null)
                            {
                                if (f1EndDate != null)
                                {
                                    attributeValues.AttributeId = 1136;
                                    attributeValues.EntityId = (int)personId;
                                    attributeValues.Value = DateRange(f1StartDate, f1EndDate);
                                }
                                else
                                {
                                    attributeValues.AttributeId = 1136;
                                    attributeValues.EntityId = (int)personId;
                                    attributeValues.Value = DateRange(f1StartDate, f1EndDate) + ",";
                                }
                            }
                            else
                            {
                                attributeValues.AttributeId = 1136;
                                attributeValues.EntityId = (int)personId;
                                attributeValues.Value = "1/1/2100";
                            }
                        }
                        else if (f1AttributeName == "Parking Lot")
                        {
                            if (f1StartDate != null)
                            {
                                if (f1EndDate != null)
                                {
                                    attributeValues.AttributeId = 1145;
                                    attributeValues.EntityId = (int)personId;
                                    attributeValues.Value = DateRange(f1StartDate, f1EndDate);
                                }
                                else
                                {
                                    attributeValues.AttributeId = 1145;
                                    attributeValues.EntityId = (int)personId;
                                    attributeValues.Value = DateRange(f1StartDate, f1EndDate) + ",";
                                }
                            }
                            else
                            {
                                attributeValues.AttributeId = 1145;
                                attributeValues.EntityId = (int)personId;
                                attributeValues.Value = "1/1/2100";
                            }
                        }
                        else if (f1AttributeName == "Preteen")
                        {
                            if (f1StartDate != null)
                            {
                                if (f1EndDate != null)
                                {
                                    attributeValues.AttributeId = 1157;
                                    attributeValues.EntityId = (int)personId;
                                    attributeValues.Value = DateRange(f1StartDate, f1EndDate);
                                }
                                else
                                {
                                    attributeValues.AttributeId = 1157;
                                    attributeValues.EntityId = (int)personId;
                                    attributeValues.Value = DateRange(f1StartDate, f1EndDate) + ",";
                                }
                            }
                            else
                            {
                                attributeValues.AttributeId = 1157;
                                attributeValues.EntityId = (int)personId;
                                attributeValues.Value = "1/1/2100";
                            }
                        }
                        else if (f1AttributeName == "Security")
                        {
                            if (f1StartDate != null)
                            {
                                if (f1EndDate != null)
                                {
                                    attributeValues.AttributeId = 1177;
                                    attributeValues.EntityId = (int)personId;
                                    attributeValues.Value = DateRange(f1StartDate, f1EndDate);
                                }
                                else
                                {
                                    attributeValues.AttributeId = 1177;
                                    attributeValues.EntityId = (int)personId;
                                    attributeValues.Value = DateRange(f1StartDate, f1EndDate) + ",";
                                }
                            }
                            else
                            {
                                attributeValues.AttributeId = 1177;
                                attributeValues.EntityId = (int)personId;
                                attributeValues.Value = "1/1/2100";
                            }
                        }
                        else if (f1AttributeName == "Seniors")
                        {
                            if (f1StartDate != null)
                            {
                                if (f1EndDate != null)
                                {
                                    attributeValues.AttributeId = 1183;
                                    attributeValues.EntityId = (int)personId;
                                    attributeValues.Value = DateRange(f1StartDate, f1EndDate);
                                }
                                else
                                {
                                    attributeValues.AttributeId = 1183;
                                    attributeValues.EntityId = (int)personId;
                                    attributeValues.Value = DateRange(f1StartDate, f1EndDate) + ",";
                                }
                            }
                            else
                            {
                                attributeValues.AttributeId = 1183;
                                attributeValues.EntityId = (int)personId;
                                attributeValues.Value = "1/1/2100";
                            }
                        }
                        else if (f1AttributeName == "Singles")
                        {
                            if (f1StartDate != null)
                            {
                                if (f1EndDate != null)
                                {
                                    attributeValues.AttributeId = 1190;
                                    attributeValues.EntityId = (int)personId;
                                    attributeValues.Value = DateRange(f1StartDate, f1EndDate);
                                }
                                else
                                {
                                    attributeValues.AttributeId = 1190;
                                    attributeValues.EntityId = (int)personId;
                                    attributeValues.Value = DateRange(f1StartDate, f1EndDate) + ",";
                                }
                            }
                            else
                            {
                                attributeValues.AttributeId = 1190;
                                attributeValues.EntityId = (int)personId;
                                attributeValues.Value = "1/1/2100";
                            }
                        }
                        else if (f1AttributeName == "Spanish")
                        {
                            if (f1StartDate != null)
                            {
                                if (f1EndDate != null)
                                {
                                    attributeValues.AttributeId = 1197;
                                    attributeValues.EntityId = (int)personId;
                                    attributeValues.Value = DateRange(f1StartDate, f1EndDate);
                                }
                                else
                                {
                                    attributeValues.AttributeId = 1197;
                                    attributeValues.EntityId = (int)personId;
                                    attributeValues.Value = DateRange(f1StartDate, f1EndDate) + ",";
                                }
                            }
                            else
                            {
                                attributeValues.AttributeId = 1197;
                                attributeValues.EntityId = (int)personId;
                                attributeValues.Value = "1/1/2100";
                            }
                        }
                        else if (f1AttributeName == "Sports")
                        {
                            if (f1StartDate != null)
                            {
                                if (f1EndDate != null)
                                {
                                    attributeValues.AttributeId = 1205;
                                    attributeValues.EntityId = (int)personId;
                                    attributeValues.Value = DateRange(f1StartDate, f1EndDate);
                                }
                                else
                                {
                                    attributeValues.AttributeId = 1205;
                                    attributeValues.EntityId = (int)personId;
                                    attributeValues.Value = DateRange(f1StartDate, f1EndDate) + ",";
                                }
                            }
                            else
                            {
                                attributeValues.AttributeId = 1205;
                                attributeValues.EntityId = (int)personId;
                                attributeValues.Value = "1/1/2100";
                            }
                        }
                        else if (f1AttributeName == "Staff")
                        {
                            if (f1Comment != null)
                            {
                                attributeExtraComment.AttributeId = 1212;
                                attributeExtraComment.EntityId = (int)personId;
                                attributeExtraComment.Value = f1Comment;
                            }
                            if (f1StartDate != null)
                            {
                                if (f1EndDate != null)
                                {
                                    attributeValues.AttributeId = 1211;
                                    attributeValues.EntityId = (int)personId;
                                    attributeValues.Value = DateRange(f1StartDate, f1EndDate);
                                }
                                else
                                {
                                    attributeValues.AttributeId = 1211;
                                    attributeValues.EntityId = (int)personId;
                                    attributeValues.Value = DateRange(f1StartDate, f1EndDate) + ",";
                                }
                            }
                            else
                            {
                                attributeValues.AttributeId = 1211;
                                attributeValues.EntityId = (int)personId;
                                attributeValues.Value = "1/1/2100";
                            }
                        }
                        else if (f1AttributeName == "Sunday Study Teacher")
                        {
                            if (f1StartDate != null)
                            {
                                if (f1EndDate != null)
                                {
                                    attributeValues.AttributeId = 1216;
                                    attributeValues.EntityId = (int)personId;
                                    attributeValues.Value = DateRange(f1StartDate, f1EndDate);
                                }
                                else
                                {
                                    attributeValues.AttributeId = 1216;
                                    attributeValues.EntityId = (int)personId;
                                    attributeValues.Value = DateRange(f1StartDate, f1EndDate) + ",";
                                }
                            }
                            else
                            {
                                attributeValues.AttributeId = 1216;
                                attributeValues.EntityId = (int)personId;
                                attributeValues.Value = "1/1/2100";
                            }
                        }
                        else if (f1AttributeName == "UpFront")
                        {
                            if (f1Comment != null)
                            {
                                attributeExtraComment.AttributeId = 1226;
                                attributeExtraComment.EntityId = (int)personId;
                                attributeExtraComment.Value = f1Comment;
                            }
                            else if (f1StartDate != null)
                            {
                                if (f1EndDate != null)
                                {
                                    attributeValues.AttributeId = 1221;
                                    attributeValues.EntityId = (int)personId;
                                    attributeValues.Value = DateRange(f1StartDate, f1EndDate);
                                }
                                else
                                {
                                    attributeValues.AttributeId = 1221;
                                    attributeValues.EntityId = (int)personId;
                                    attributeValues.Value = DateRange(f1StartDate, f1EndDate) + ",";
                                }
                            }
                            else
                            {
                                attributeValues.AttributeId = 1221;
                                attributeValues.EntityId = (int)personId;
                                attributeValues.Value = "1/1/2100";
                            }
                        }
                        else if (f1AttributeName == "Women")
                        {
                            if (f1StartDate != null)
                            {
                                if (f1EndDate != null)
                                {
                                    attributeValues.AttributeId = 1250;
                                    attributeValues.EntityId = (int)personId;
                                    attributeValues.Value = DateRange(f1StartDate, f1EndDate);
                                }
                                else
                                {
                                    attributeValues.AttributeId = 1250;
                                    attributeValues.EntityId = (int)personId;
                                    attributeValues.Value = DateRange(f1StartDate, f1EndDate) + ",";
                                }
                            }
                            else
                            {
                                attributeValues.AttributeId = 1250;
                                attributeValues.EntityId = (int)personId;
                                attributeValues.Value = "1/1/2100";
                            }
                        }
                        else if (f1AttributeName == "Worship")
                        {
                            if (f1Comment != null)
                            {
                                attributeExtraComment.AttributeId = 1256;
                                attributeExtraComment.EntityId = (int)personId;
                                attributeExtraComment.Value = f1Comment;
                            }
                            else if (f1StartDate != null)
                            {
                                if (f1EndDate != null)
                                {
                                    attributeValues.AttributeId = 1253;
                                    attributeValues.EntityId = (int)personId;
                                    attributeValues.Value = DateRange(f1StartDate, f1EndDate);
                                }
                                else
                                {
                                    attributeValues.AttributeId = 1253;
                                    attributeValues.EntityId = (int)personId;
                                    attributeValues.Value = DateRange(f1StartDate, f1EndDate) + ",";
                                }
                            }
                            else
                            {
                                attributeValues.AttributeId = 1253;
                                attributeValues.EntityId = (int)personId;
                                attributeValues.Value = "1/1/2100";
                            }
                        }
                        else if (f1AttributeName == "Youth")
                        {
                            if (f1StartDate != null)
                            {
                                if (f1EndDate != null)
                                {
                                    attributeValues.AttributeId = 1257;
                                    attributeValues.EntityId = (int)personId;
                                    attributeValues.Value = DateRange(f1StartDate, f1EndDate);
                                }
                                else
                                {
                                    attributeValues.AttributeId = 1257;
                                    attributeValues.EntityId = (int)personId;
                                    attributeValues.Value = DateRange(f1StartDate, f1EndDate) + ",";
                                }
                            }
                            else
                            {
                                attributeValues.AttributeId = 1257;
                                attributeValues.EntityId = (int)personId;
                                attributeValues.Value = "1/1/2100";
                            }
                        }
                    }
                    else if (f1AttributeGroupName == "Meals Ministry")
                    {
                        if (f1StartDate != null)
                        {
                            if (f1EndDate != null)
                            {
                                attributeValues.AttributeId = 1122;
                                attributeValues.EntityId = (int)personId;
                                attributeValues.Value = DateRange(f1StartDate, f1EndDate);
                            }
                            else
                            {
                                attributeValues.AttributeId = 1122;
                                attributeValues.EntityId = (int)personId;
                                attributeValues.Value = DateRange(f1StartDate, f1EndDate) + ",";
                            }
                        }
                        else
                        {
                            attributeValues.AttributeId = 1122;
                            attributeValues.EntityId = (int)personId;
                            attributeValues.Value = "1/1/2100";
                        }
                    }
                    else if (f1AttributeGroupName == "Media")
                    {
                        if (f1AttributeName == "Camera Training")
                        {
                            if (f1StartDate != null)
                            {
                                attributeValues.AttributeId = 993;
                                attributeValues.EntityId = (int)personId;
                                attributeValues.Value = DateRange(f1StartDate, f1EndDate) + ",";
                            }
                            else
                            {
                                attributeValues.AttributeId = 993;
                                attributeValues.EntityId = (int)personId;
                                attributeValues.Value = "1/1/2100";
                            }
                        }
                    }
                    //skipping this group (only one attribute for one person)
                    /*
                    if (f1AttributeGroupName == "Men's Zone Group")
                    {

                    }
                     * */
                    else if (f1AttributeGroupName == "MIA")
                    {
                        if (f1Comment != null)
                        {
                            //Checks if current person already has this Rock Attribute and will append the current F1 attribute value to the Rock one.
                            if (newAttributes.Find(a => (a.AttributeId == 1133 && a.EntityId == personId)) != null)
                            {
                                ReportProgress(0, string.Format("Adding MIA"));
                                newAttributes.Find(a => a.AttributeId == 1133 && a.EntityId == personId).Value += f1Comment;
                                if (staffId != null)
                                {
                                    newAttributes.Find(a => a.AttributeId == 1133 && a.EntityId == personId).Value += " - " + staffId;
                                }
                            }
                            else
                            {
                                attributeValues.AttributeId = 1133;
                                attributeValues.EntityId = (int)personId;
                                attributeValues.Value = f1Comment;
                                if (staffId != null)
                                {
                                    attributeValues.Value += " - " + staffId;
                                }
                            }
                        }
                    }
                    else if (f1AttributeGroupName == "Military Service")
                    {
                        if (f1Comment != null)
                        {
                            attributeExtraComment.AttributeId = 1134;
                            attributeExtraComment.EntityId = (int)personId;

                            string air = "AIR FORCE", airForce = "USAF", army = "ARMY", coast = "COAST GUARD", coastGaurd = "USCG", marine = "MARINE", marineCorps = "USMC", marines = "MARINES", nationalGuard = "NATIONAL GUARD", navy = "NAVY", reserve = "RESERVE";
                            string f1MilitaryComments = f1Comment.ToUpper();

                            if (f1MilitaryComments.Contains(air) || f1MilitaryComments.Contains(airForce))
                            {
                                attributeExtraComment.Value = "78E31C9C-AD2D-44D4-AFE0-E52878B5786F";
                            }
                            else if (f1MilitaryComments.Contains(army))
                            {
                                attributeExtraComment.Value = "F91B03A5-72D2-4915-A90C-23B54D1847BE";
                            }
                            else if (f1MilitaryComments.Contains(coast) || f1MilitaryComments.Contains(coastGaurd))
                            {
                                attributeExtraComment.Value = "85E9E19E-E726-4DFE-B6E3-625C04BFA24A";
                            }
                            else if (f1MilitaryComments.Contains(marine) || f1MilitaryComments.Contains(marineCorps) || f1MilitaryComments.Contains(marines))
                            {
                                attributeExtraComment.Value = "AFD8EB78-68B4-4912-BF54-031D57FA6775";
                            }
                            else if (f1MilitaryComments.Contains(nationalGuard))
                            {
                                attributeExtraComment.Value = "2D1B2D52-72B9-4CB1-8A0F-F48BE206153E";
                            }
                            else if (f1MilitaryComments.Contains(navy))
                            {
                                attributeExtraComment.Value = "BC529FA2-4DD8-4230-B0AA-863A5DC6BD68";
                            }
                            else if (f1MilitaryComments.Contains(reserve))
                            {
                                attributeExtraComment.Value = "B37F8BBD-E040-48D6-90B9-A63C44649F71";
                            }
                            else
                            {
                                attributeExtraComment.Value = "805CB53A-843A-4B79-BA1B-3B451DD41CDE";
                            }
                        }
                        //if F1 Comment is null it sets the military branch to not specified. This way we still have some kind of record of them being in service --- since a lot of the attributes don't have dates or comments.
                        else if (string.IsNullOrEmpty(f1Comment) || string.IsNullOrWhiteSpace(f1Comment))
                        {
                            attributeExtraComment.AttributeId = 1134;
                            attributeExtraComment.EntityId = (int)personId;
                            attributeExtraComment.Value = "805CB53A-843A-4B79-BA1B-3B451DD41CDE";
                        }
                        else if (f1StartDate != null)
                        {
                            if (f1EndDate != null)
                            {
                                attributeValues.AttributeId = 1135;
                                attributeValues.EntityId = (int)personId;
                                attributeValues.Value = DateRange(f1StartDate, f1EndDate);
                            }
                            else
                            {
                                attributeValues.AttributeId = 1135;
                                attributeValues.EntityId = (int)personId;
                                attributeValues.Value = DateRange(f1StartDate, f1EndDate) + ",";
                            }
                        }
                    }

                    //MDO contains old data that isn't in use and isn't needed, will not be transfering this attribute.
                    /*
                     * if (f1AttributeGroupName == "Mother's Day Out")
                    {

                    }
                     * */
                    else if (f1AttributeGroupName == "New Volunteer")
                    {
                        //if (f1AttributeName == "Adult Ministry")
                        //{
                        //    if (f1Comment != null)
                        //    {
                        //        attributeValues.AttributeId = 967;
                        //        attributeValues.EntityId = (int)personId;
                        //        attributeValues.Value = f1Comment;
                        //    }
                        //    if (f1StartDate != null)
                        //    {
                        //        if (f1EndDate != null)
                        //        {
                        //            attributeExtraDate.AttributeId = 968;
                        //            attributeExtraDate.EntityId = (int)personId;
                        //            attributeExtraDate.Value = DateRange(f1StartDate, f1EndDate);
                        //        }
                        //        else
                        //        {
                        //            attributeExtraDate.AttributeId = 968;
                        //            attributeExtraDate.EntityId = (int)personId;
                        //            attributeExtraDate.Value = DateRange(f1StartDate, f1EndDate) + ",";
                        //        }
                        //    }
                        //}
                        if (f1AttributeName == "Childrens Ministry")
                        {
                            if (f1Comment != null)
                            {
                                attributeValues.AttributeId = 1007;
                                attributeValues.EntityId = (int)personId;
                                attributeValues.Value = f1Comment;
                            }
                            if (f1StartDate != null)
                            {
                                if (f1EndDate != null)
                                {
                                    attributeExtraDate.AttributeId = 1008;
                                    attributeExtraDate.EntityId = (int)personId;
                                    attributeExtraDate.Value = DateRange(f1StartDate, f1EndDate);
                                }
                                else
                                {
                                    attributeExtraDate.AttributeId = 1008;
                                    attributeExtraDate.EntityId = (int)personId;
                                    attributeExtraDate.Value = DateRange(f1StartDate, f1EndDate) + ",";
                                }
                            }
                        }
                        else if (f1AttributeName == "College Ministry")
                        {
                            if (f1Comment != null)
                            {
                                attributeValues.AttributeId = 1016;
                                attributeValues.EntityId = (int)personId;
                                attributeValues.Value = f1Comment;
                            }
                            if (f1StartDate != null)
                            {
                                if (f1EndDate != null)
                                {
                                    attributeExtraDate.AttributeId = 1017;
                                    attributeExtraDate.EntityId = (int)personId;
                                    attributeExtraDate.Value = DateRange(f1StartDate, f1EndDate);
                                }
                                else
                                {
                                    attributeExtraDate.AttributeId = 1017;
                                    attributeExtraDate.EntityId = (int)personId;
                                    attributeExtraDate.Value = DateRange(f1StartDate, f1EndDate) + ",";
                                }
                            }
                        }
                        else if (f1AttributeName == "First Serve")
                        {
                            if (f1Comment != null)
                            {
                                attributeValues.AttributeId = 1085;
                                attributeValues.EntityId = (int)personId;
                                attributeValues.Value = f1Comment;
                            }
                            if (f1StartDate != null)
                            {
                                if (f1EndDate != null)
                                {
                                    attributeExtraDate.AttributeId = 1086;
                                    attributeExtraDate.EntityId = (int)personId;
                                    attributeExtraDate.Value = DateRange(f1StartDate, f1EndDate);
                                }
                                else
                                {
                                    attributeExtraDate.AttributeId = 1086;
                                    attributeExtraDate.EntityId = (int)personId;
                                    attributeExtraDate.Value = DateRange(f1StartDate, f1EndDate) + ",";
                                }
                            }
                        }
                        else if (f1AttributeName == "Food Services Ministry")
                        {
                            if (f1Comment != null)
                            {
                                attributeValues.AttributeId = 1090;
                                attributeValues.EntityId = (int)personId;
                                attributeValues.Value = f1Comment;
                            }
                            if (f1StartDate != null)
                            {
                                if (f1EndDate != null)
                                {
                                    attributeExtraDate.AttributeId = 1091;
                                    attributeExtraDate.EntityId = (int)personId;
                                    attributeExtraDate.Value = DateRange(f1StartDate, f1EndDate);
                                }
                                else
                                {
                                    attributeExtraDate.AttributeId = 1091;
                                    attributeExtraDate.EntityId = (int)personId;
                                    attributeExtraDate.Value = DateRange(f1StartDate, f1EndDate) + ",";
                                }
                            }
                        }
                        else if (f1AttributeName == "Helping Hands")
                        {
                            if (f1Comment != null)
                            {
                                attributeValues.AttributeId = 1099;
                                attributeValues.EntityId = (int)personId;
                                attributeValues.Value = f1Comment;
                            }
                            if (f1StartDate != null)
                            {
                                if (f1EndDate != null)
                                {
                                    attributeExtraDate.AttributeId = 1101;
                                    attributeExtraDate.EntityId = (int)personId;
                                    attributeExtraDate.Value = DateRange(f1StartDate, f1EndDate);
                                }
                                else
                                {
                                    attributeExtraDate.AttributeId = 1101;
                                    attributeExtraDate.EntityId = (int)personId;
                                    attributeExtraDate.Value = DateRange(f1StartDate, f1EndDate) + ",";
                                }
                            }
                        }
                        else if (f1AttributeName == "Media Ministry")
                        {
                            if (f1Comment != null)
                            {
                                attributeValues.AttributeId = 1128;
                                attributeValues.EntityId = (int)personId;
                                attributeValues.Value = f1Comment;
                            }
                            if (f1StartDate != null)
                            {
                                if (f1EndDate != null)
                                {
                                    attributeExtraDate.AttributeId = 1129;
                                    attributeExtraDate.EntityId = (int)personId;
                                    attributeExtraDate.Value = DateRange(f1StartDate, f1EndDate);
                                }
                                else
                                {
                                    attributeExtraDate.AttributeId = 1129;
                                    attributeExtraDate.EntityId = (int)personId;
                                    attributeExtraDate.Value = DateRange(f1StartDate, f1EndDate) + ",";
                                }
                            }
                        }
                        else if (f1AttributeName == "Men's Ministry")
                        {
                            if (f1Comment != null)
                            {
                                attributeValues.AttributeId = 1131;
                                attributeValues.EntityId = (int)personId;
                                attributeValues.Value = f1Comment;
                            }
                            if (f1StartDate != null)
                            {
                                if (f1EndDate != null)
                                {
                                    attributeExtraDate.AttributeId = 1132;
                                    attributeExtraDate.EntityId = (int)personId;
                                    attributeExtraDate.Value = DateRange(f1StartDate, f1EndDate);
                                }
                                else
                                {
                                    attributeExtraDate.AttributeId = 1132;
                                    attributeExtraDate.EntityId = (int)personId;
                                    attributeExtraDate.Value = DateRange(f1StartDate, f1EndDate) + ",";
                                }
                            }
                        }
                        else if (f1AttributeName == "Music Ministry")
                        {
                            if (f1Comment != null)
                            {
                                attributeValues.AttributeId = 1254;
                                attributeValues.EntityId = (int)personId;
                                attributeValues.Value = f1Comment;
                            }
                            if (f1StartDate != null)
                            {
                                if (f1EndDate != null)
                                {
                                    attributeExtraDate.AttributeId = 1255;
                                    attributeExtraDate.EntityId = (int)personId;
                                    attributeExtraDate.Value = DateRange(f1StartDate, f1EndDate);
                                }
                                else
                                {
                                    attributeExtraDate.AttributeId = 1255;
                                    attributeExtraDate.EntityId = (int)personId;
                                    attributeExtraDate.Value = DateRange(f1StartDate, f1EndDate) + ",";
                                }
                            }
                        }
                        else if (f1AttributeName == "Office Volunteer")
                        {
                            if (f1Comment != null)
                            {
                                attributeValues.AttributeId = 1143;
                                attributeValues.EntityId = (int)personId;
                                attributeValues.Value = f1Comment;
                            }
                            if (f1StartDate != null)
                            {
                                if (f1EndDate != null)
                                {
                                    attributeExtraDate.AttributeId = 1142;
                                    attributeExtraDate.EntityId = (int)personId;
                                    attributeExtraDate.Value = DateRange(f1StartDate, f1EndDate);
                                }
                                else
                                {
                                    attributeExtraDate.AttributeId = 1142;
                                    attributeExtraDate.EntityId = (int)personId;
                                    attributeExtraDate.Value = DateRange(f1StartDate, f1EndDate) + ",";
                                }
                            }
                        }
                        //else if (f1AttributeName == "Parking Lot Ministry")
                        //{
                        //    if (f1Comment != null)
                        //    {
                        //        attributeValues.AttributeId = 1146;
                        //        attributeValues.EntityId = (int)personId;
                        //        attributeValues.Value = f1Comment;
                        //    }
                        //    if (f1StartDate != null)
                        //    {
                        //        if (f1EndDate != null)
                        //        {
                        //            attributeExtraDate.AttributeId = 1147;
                        //            attributeExtraDate.EntityId = (int)personId;
                        //            attributeExtraDate.Value = DateRange(f1StartDate, f1EndDate);
                        //        }
                        //        else
                        //        {
                        //            attributeExtraDate.AttributeId = 1147;
                        //            attributeExtraDate.EntityId = (int)personId;
                        //            attributeExtraDate.Value = DateRange(f1StartDate, f1EndDate) + ",";
                        //        }
                        //    }
                        //}
                        else if (f1AttributeName == "Preteen Ministry")
                        {
                            if (f1Comment != null)
                            {
                                attributeValues.AttributeId = 1158;
                                attributeValues.EntityId = (int)personId;
                                attributeValues.Value = f1Comment;
                            }
                            if (f1StartDate != null)
                            {
                                if (f1EndDate != null)
                                {
                                    attributeExtraDate.AttributeId = 1159;
                                    attributeExtraDate.EntityId = (int)personId;
                                    attributeExtraDate.Value = DateRange(f1StartDate, f1EndDate);
                                }
                                else
                                {
                                    attributeExtraDate.AttributeId = 1159;
                                    attributeExtraDate.EntityId = (int)personId;
                                    attributeExtraDate.Value = DateRange(f1StartDate, f1EndDate) + ",";
                                }
                            }
                        }
                        //else if (f1AttributeName == "Security Ministry")
                        //{
                        //    if (f1Comment != null)
                        //    {
                        //        attributeValues.AttributeId = 1180;
                        //        attributeValues.EntityId = (int)personId;
                        //        attributeValues.Value = f1Comment;
                        //    }
                        //    if (f1StartDate != null)
                        //    {
                        //        if (f1EndDate != null)
                        //        {
                        //            attributeExtraDate.AttributeId = 1181;
                        //            attributeExtraDate.EntityId = (int)personId;
                        //            attributeExtraDate.Value = DateRange(f1StartDate, f1EndDate);
                        //        }
                        //        else
                        //        {
                        //            attributeExtraDate.AttributeId = 1181;
                        //            attributeExtraDate.EntityId = (int)personId;
                        //            attributeExtraDate.Value = DateRange(f1StartDate, f1EndDate) + ",";
                        //        }
                        //    }
                        //}
                        else if (f1AttributeName == "Sports Ministry" || f1AttributeName == "Basketball" || f1AttributeName == "Cheerleading" || f1AttributeName == "Football" || f1AttributeName == "Soccer")
                        {
                            if (f1Comment != null)
                            {
                                attributeValues.AttributeId = 1207;
                                attributeValues.EntityId = (int)personId;
                                attributeValues.Value = f1Comment;
                            }
                            if (f1StartDate != null)
                            {
                                if (f1EndDate != null)
                                {
                                    attributeExtraDate.AttributeId = 1208;
                                    attributeExtraDate.EntityId = (int)personId;
                                    attributeExtraDate.Value = DateRange(f1StartDate, f1EndDate);
                                }
                                else
                                {
                                    attributeExtraDate.AttributeId = 1208;
                                    attributeExtraDate.EntityId = (int)personId;
                                    attributeExtraDate.Value = DateRange(f1StartDate, f1EndDate) + ",";
                                }
                            }
                        }
                        else if (f1AttributeName == "UpFront Ministry")
                        {
                            if (f1Comment != null)
                            {
                                attributeValues.AttributeId = 1224;
                                attributeValues.EntityId = (int)personId;
                                attributeValues.Value = f1Comment;
                            }
                            if (f1StartDate != null)
                            {
                                if (f1EndDate != null)
                                {
                                    attributeExtraDate.AttributeId = 1225;
                                    attributeExtraDate.EntityId = (int)personId;
                                    attributeExtraDate.Value = DateRange(f1StartDate, f1EndDate);
                                }
                                else
                                {
                                    attributeExtraDate.AttributeId = 1225;
                                    attributeExtraDate.EntityId = (int)personId;
                                    attributeExtraDate.Value = DateRange(f1StartDate, f1EndDate) + ",";
                                }
                            }
                        }
                        else if (f1AttributeName == "Women's Ministry")
                        {
                            if (f1Comment != null)
                            {
                                attributeValues.AttributeId = 1251;
                                attributeValues.EntityId = (int)personId;
                                attributeValues.Value = f1Comment;
                            }
                            if (f1StartDate != null)
                            {
                                if (f1EndDate != null)
                                {
                                    attributeExtraDate.AttributeId = 1252;
                                    attributeExtraDate.EntityId = (int)personId;
                                    attributeExtraDate.Value = DateRange(f1StartDate, f1EndDate);
                                }
                                else
                                {
                                    attributeExtraDate.AttributeId = 1252;
                                    attributeExtraDate.EntityId = (int)personId;
                                    attributeExtraDate.Value = DateRange(f1StartDate, f1EndDate) + ",";
                                }
                            }
                        }
                        else if (f1AttributeName == "Worship Ministry")
                        {
                            if (f1Comment != null)
                            {
                                attributeValues.AttributeId = 1254;
                                attributeValues.EntityId = (int)personId;
                                attributeValues.Value = f1Comment;
                            }
                            if (f1StartDate != null)
                            {
                                if (f1EndDate != null)
                                {
                                    attributeExtraDate.AttributeId = 1255;
                                    attributeExtraDate.EntityId = (int)personId;
                                    attributeExtraDate.Value = DateRange(f1StartDate, f1EndDate);
                                }
                                else
                                {
                                    attributeExtraDate.AttributeId = 1255;
                                    attributeExtraDate.EntityId = (int)personId;
                                    attributeExtraDate.Value = DateRange(f1StartDate, f1EndDate) + ",";
                                }
                            }
                        }
                        else if (f1AttributeName == "Youth Ministry")
                        {

                            if (f1Comment != null)
                            {
                                attributeValues.AttributeId = 1264;
                                attributeValues.EntityId = (int)personId;
                                attributeValues.Value = f1Comment;
                            }
                            if (f1StartDate != null)
                            {
                                if (f1EndDate != null)
                                {
                                    attributeExtraDate.AttributeId = 1265;
                                    attributeExtraDate.EntityId = (int)personId;
                                    attributeExtraDate.Value = DateRange(f1StartDate, f1EndDate);
                                }
                                else
                                {
                                    attributeExtraDate.AttributeId = 1265;
                                    attributeExtraDate.EntityId = (int)personId;
                                    attributeExtraDate.Value = DateRange(f1StartDate, f1EndDate) + ",";
                                }
                            }
                        }
                    }

                    //No individual attributes exist for this group
                    /*
                    if (f1AttributeGroupName == "Potential Carnival Volunteers")
                    {

                    }
                     Only one person has this attribute and the date is from 2005
                    if (f1AttributeGroupName == "Seniors")
                    {

                    }
                     * */
                    else if (f1AttributeGroupName == "Significant Loss")
                    {
                        attributeValues.AttributeId = 1186;
                        attributeValues.EntityId = (int)personId;
                        attributeValues.Value = DateRange(f1StartDate, f1EndDate) + " - " + f1Comment;
                    }
                    else if (f1AttributeGroupName == "Single Parent/Widow/Military")
                    {
                        attributeValues.AttributeId = 1189;
                        attributeValues.EntityId = (int)personId;
                        attributeValues.Value = DateRange(f1StartDate, f1EndDate) + "," + " - " + f1Comment;
                    }
                    /* We do not currently use these attributes for anything so don't convert them
                    if (f1AttributeGroupName == "Sports")
                    {

                    }
                     */
                    else if (f1AttributeGroupName == "Summer Sports Camps")
                    {
                        if (f1AttributeName == "Basketball")
                        {
                            if (f1Comment != null)
                            {
                                attributeValues.AttributeId = 989;
                                attributeValues.EntityId = (int)personId;
                                attributeValues.Value = f1Comment;
                            }
                            else if (f1StartDate != null)
                            {
                                attributeExtraDate.AttributeId = 988;
                                attributeExtraDate.EntityId = (int)personId;
                                attributeExtraDate.Value = CrossroadsSportsCampYears(f1StartDate, "Play") + ",";
                            }
                        }
                        else if (f1AttributeName == "Cheerleading")
                        {
                            if (f1Comment != null)
                            {
                                attributeValues.AttributeId = 1004;
                                attributeValues.EntityId = (int)personId;
                                attributeValues.Value = f1Comment;
                            }
                            else if (f1StartDate != null)
                            {
                                attributeExtraDate.AttributeId = 1003;
                                attributeExtraDate.EntityId = (int)personId;
                                attributeExtraDate.Value = CrossroadsSportsCampYears(f1StartDate, "Play") + ",";
                            }
                        }
                        else if (f1AttributeName == "Flag Football")
                        {
                            if (f1Comment != null)
                            {
                                attributeValues.AttributeId = 1087;
                                attributeValues.EntityId = (int)personId;
                                attributeValues.Value = f1Comment;
                            }
                            else if (f1StartDate != null)
                            {
                                attributeExtraDate.AttributeId = 1088;
                                attributeExtraDate.EntityId = (int)personId;
                                attributeExtraDate.Value = CrossroadsSportsCampYears(f1StartDate, "Play") + ",";
                            }
                        }
                        else if (f1AttributeName == "Soccer")
                        {
                            if (f1Comment != null)
                            {
                                attributeValues.AttributeId = 1191;
                                attributeValues.EntityId = (int)personId;
                                attributeValues.Value = f1Comment;
                            }
                            else if (f1StartDate != null)
                            {
                                attributeExtraDate.AttributeId = 1192;
                                attributeExtraDate.EntityId = (int)personId;
                                attributeExtraDate.Value = CrossroadsSportsCampYears(f1StartDate, "Play") + ",";
                            }
                        }
                    }

                    else if (f1AttributeGroupName == "Sports (Adults)")
                    {
                        if (f1AttributeName == "Basketball (Play)" || f1AttributeName == "Basketball (Volunteer)")
                        {
                            string action = "Play";
                            if (f1AttributeName == "Basketball (Play)") { action = "Play"; }
                            if (f1AttributeName == "Basketball (Volunteer)") { action = "Vol"; }
                            if (f1Comment != null)
                            {
                                attributeValues.AttributeId = 967;
                                attributeValues.EntityId = (int)personId;
                                attributeValues.Value = f1Comment;
                            }
                            else if (f1StartDate != null)
                            {
                                attributeExtraDate.AttributeId = 1076;
                                attributeExtraDate.EntityId = (int)personId;
                                attributeExtraDate.Value = CrossroadsSportsYears(f1StartDate, action) + ",";
                            }
                        }
                        else if (f1AttributeName == "Flag Football (Play)" || f1AttributeName == "Flag Football (Volunteer)")
                        {
                            string action = "Play";
                            if (f1AttributeName == "Flag Football (Play)") { action = "Play"; }
                            if (f1AttributeName == "Flag Football (Volunteer)") { action = "Vol"; }
                            if (f1Comment != null)
                            {
                                attributeValues.AttributeId = 975;
                                attributeValues.EntityId = (int)personId;
                                attributeValues.Value = f1Comment;
                            }
                            else if (f1StartDate != null)
                            {
                                attributeExtraDate.AttributeId = 968;
                                attributeExtraDate.EntityId = (int)personId;
                                attributeExtraDate.Value = CrossroadsSportsYears(f1StartDate, action) + ",";
                            }
                        }
                        else if (f1AttributeName == "Tennis (Volunteer)")
                        {
                            if (f1Comment != null)
                            {
                                attributeValues.AttributeId = 977;
                                attributeValues.EntityId = (int)personId;
                                attributeValues.Value = f1Comment;
                            }
                            else if (f1StartDate != null)
                            {
                                attributeExtraDate.AttributeId = 976;
                                attributeExtraDate.EntityId = (int)personId;
                                attributeExtraDate.Value = CrossroadsSportsYears(f1StartDate, "Vol") + ",";
                            }
                        }
                        else if (f1AttributeName == "Volleyball (Play)" || f1AttributeName == "Volleyball (Volunteer)")
                        {
                            string action = "Play";
                            if (f1AttributeName == "Volleyball (Play)") { action = "Play"; }
                            if (f1AttributeName == "Volleyball (Volunteer)") { action = "Vol"; }
                            if (f1Comment != null)
                            {
                                attributeValues.AttributeId = 985;
                                attributeValues.EntityId = (int)personId;
                                attributeValues.Value = f1Comment;
                            }
                            else if (f1StartDate != null)
                            {
                                attributeExtraDate.AttributeId = 978;
                                attributeExtraDate.EntityId = (int)personId;
                                attributeExtraDate.Value = CrossroadsSportsYears(f1StartDate, action) + ",";
                            }
                        }
                    }
                    else if (f1AttributeGroupName == "Sports (Kids)")
                    {
                        if (f1AttributeName == "Basketball")
                        {
                            if (f1Comment != null)
                            {
                                attributeValues.AttributeId = 1041;
                                attributeValues.EntityId = (int)personId;
                                attributeValues.Value = f1Comment;
                            }
                            else if (f1StartDate != null)
                            {
                                attributeExtraDate.AttributeId = 1042;
                                attributeExtraDate.EntityId = (int)personId;
                                attributeExtraDate.Value = CrossroadsSportsYears(f1StartDate, "Play") + ",";
                            }
                        }
                        else if (f1AttributeName == "Cheerleading")
                        {
                            if (f1Comment != null)
                            {
                                attributeValues.AttributeId = 1043;
                                attributeValues.EntityId = (int)personId;
                                attributeValues.Value = f1Comment;
                            }
                            else if (f1StartDate != null)
                            {
                                attributeExtraDate.AttributeId = 1044;
                                attributeExtraDate.EntityId = (int)personId;
                                attributeExtraDate.Value = CrossroadsSportsYears(f1StartDate, "Play") + ",";
                            }
                        }
                        else if (f1AttributeName == "Flag Football")
                        {
                            if (f1Comment != null)
                            {
                                attributeValues.AttributeId = 1047;
                                attributeValues.EntityId = (int)personId;
                                attributeValues.Value = f1Comment;
                            }
                            else if (f1StartDate != null)
                            {
                                attributeExtraDate.AttributeId = 1048;
                                attributeExtraDate.EntityId = (int)personId;
                                attributeExtraDate.Value = CrossroadsSportsYears(f1StartDate, "Play") + ",";
                            }
                        }
                        else if (f1AttributeName == "Soccer")
                        {
                            if (f1Comment != null)
                            {
                                attributeValues.AttributeId = 1051;
                                attributeValues.EntityId = (int)personId;
                                attributeValues.Value = f1Comment;
                            }
                            else if (f1StartDate != null)
                            {
                                attributeExtraDate.AttributeId = 1052;
                                attributeExtraDate.EntityId = (int)personId;
                                attributeExtraDate.Value = CrossroadsSportsYears(f1StartDate, "Play") + ",";
                            }
                        }
                    }
                    else if (f1AttributeGroupName == "Tithing Commitment")
                    {
                        //if (f1AttributeName == "Continue Giving Above Tithe")
                        //{
                        //    DateTime startDate = (DateTime)f1StartDate;

                        //    if (f1StartDate != null)
                        //    {
                        //        switch (startDate.Year)
                        //        {
                        //            case 2001:
                        //                //Checks if current person already has this Rock Attribute and will append the current F1 attribute value to the Rock one.
                        //                if (newAttributes.Find(a => (a.AttributeId == 1035 && a.EntityId == personId)) != null)
                        //                {
                        //                    ReportProgress(0, string.Format("Adding Continue Giving Above Tithe 2001"));
                        //                    newAttributes.Find(a => a.AttributeId == 1035 && a.EntityId == personId).Value += "B9A40993-7758-49A3-BE6B-00E930FCF690,";
                        //                }
                        //                else
                        //                {
                        //                    attributeValues.AttributeId = 1035;
                        //                    attributeValues.EntityId = (int)personId;
                        //                    attributeValues.Value = "B9A40993-7758-49A3-BE6B-00E930FCF690,";
                        //                }
                        //                break;
                        //            case 2002:
                        //                //Checks if current person already has this Rock Attribute and will append the current F1 attribute value to the Rock one.
                        //                if (newAttributes.Find(a => (a.AttributeId == 1035 && a.EntityId == personId)) != null)
                        //                {
                        //                    ReportProgress(0, string.Format("Adding Continue Giving Above Tithe 2002"));
                        //                    newAttributes.Find(a => a.AttributeId == 1035 && a.EntityId == personId).Value += "56BF96EF-561E-424D-BA85-A93674569B47,";
                        //                }
                        //                else
                        //                {
                        //                    attributeValues.AttributeId = 1035;
                        //                    attributeValues.EntityId = (int)personId;
                        //                    attributeValues.Value = "56BF96EF-561E-424D-BA85-A93674569B47,";
                        //                }
                        //                break;
                        //            case 2003:
                        //                //Checks if current person already has this Rock Attribute and will append the current F1 attribute value to the Rock one.
                        //                if (newAttributes.Find(a => (a.AttributeId == 1035 && a.EntityId == personId)) != null)
                        //                {
                        //                    ReportProgress(0, string.Format("Adding Continue Giving Above Tithe 2003"));
                        //                    newAttributes.Find(a => a.AttributeId == 1035 && a.EntityId == personId).Value += "74EB6703-DEB4-4CEA-81E2-5EC7ED81BB18,";
                        //                }
                        //                else
                        //                {
                        //                    attributeValues.AttributeId = 1035;
                        //                    attributeValues.EntityId = (int)personId;
                        //                    attributeValues.Value = "74EB6703-DEB4-4CEA-81E2-5EC7ED81BB18,";
                        //                }
                        //                break;
                        //            case 2004:
                        //                //Checks if current person already has this Rock Attribute and will append the current F1 attribute value to the Rock one.
                        //                if (newAttributes.Find(a => (a.AttributeId == 1035 && a.EntityId == personId)) != null)
                        //                {
                        //                    ReportProgress(0, string.Format("Adding Continue Giving Above Tithe 2004"));
                        //                    newAttributes.Find(a => a.AttributeId == 1035 && a.EntityId == personId).Value += "DD28ACBD-8B2C-49CC-81C9-B7FFE4D8E3C2,";
                        //                }
                        //                else
                        //                {
                        //                    attributeValues.AttributeId = 1035;
                        //                    attributeValues.EntityId = (int)personId;
                        //                    attributeValues.Value = "DD28ACBD-8B2C-49CC-81C9-B7FFE4D8E3C2,";
                        //                }
                        //                break;
                        //            case 2005:
                        //                //Checks if current person already has this Rock Attribute and will append the current F1 attribute value to the Rock one.
                        //                if (newAttributes.Find(a => (a.AttributeId == 1035 && a.EntityId == personId)) != null)
                        //                {
                        //                    ReportProgress(0, string.Format("Adding Continue Giving Above Tithe 2005"));
                        //                    newAttributes.Find(a => a.AttributeId == 1035 && a.EntityId == personId).Value += "F18A88B7-5228-4B7D-8079-4B118DF792C7,";
                        //                }
                        //                else
                        //                {
                        //                    attributeValues.AttributeId = 1035;
                        //                    attributeValues.EntityId = (int)personId;
                        //                    attributeValues.Value = "F18A88B7-5228-4B7D-8079-4B118DF792C7,";
                        //                }
                        //                break;
                        //            case 2006:
                        //                //Checks if current person already has this Rock Attribute and will append the current F1 attribute value to the Rock one.
                        //                if (newAttributes.Find(a => (a.AttributeId == 1035 && a.EntityId == personId)) != null)
                        //                {
                        //                    ReportProgress(0, string.Format("Adding Continue Giving Above Tithe 2006"));
                        //                    newAttributes.Find(a => a.AttributeId == 1035 && a.EntityId == personId).Value += "719DF19D-B5AF-4125-B708-BDC22EB64E8F,";
                        //                }
                        //                else
                        //                {
                        //                    attributeValues.AttributeId = 1035;
                        //                    attributeValues.EntityId = (int)personId;
                        //                    attributeValues.Value = "719DF19D-B5AF-4125-B708-BDC22EB64E8F,";
                        //                }
                        //                break;
                        //            case 2007:
                        //                //Checks if current person already has this Rock Attribute and will append the current F1 attribute value to the Rock one.
                        //                if (newAttributes.Find(a => (a.AttributeId == 1035 && a.EntityId == personId)) != null)
                        //                {
                        //                    ReportProgress(0, string.Format("Adding Continue Giving Above Tithe 2007"));
                        //                    newAttributes.Find(a => a.AttributeId == 1035 && a.EntityId == personId).Value += "CE44EA17-020E-4B97-8975-4DE01830163D,";
                        //                }
                        //                else
                        //                {
                        //                    attributeValues.AttributeId = 1035;
                        //                    attributeValues.EntityId = (int)personId;
                        //                    attributeValues.Value = "CE44EA17-020E-4B97-8975-4DE01830163D,";
                        //                }
                        //                break;
                        //            case 2008:
                        //                //Checks if current person already has this Rock Attribute and will append the current F1 attribute value to the Rock one.
                        //                if (newAttributes.Find(a => (a.AttributeId == 1035 && a.EntityId == personId)) != null)
                        //                {
                        //                    ReportProgress(0, string.Format("Adding Continue Giving Above Tithe 2008"));
                        //                    newAttributes.Find(a => a.AttributeId == 1035 && a.EntityId == personId).Value += "6810C1C9-85BD-42E9-9E04-85801A93096D,";
                        //                }
                        //                else
                        //                {
                        //                    attributeValues.AttributeId = 1035;
                        //                    attributeValues.EntityId = (int)personId;
                        //                    attributeValues.Value = "6810C1C9-85BD-42E9-9E04-85801A93096D,";
                        //                }
                        //                break;
                        //            case 2009:
                        //                //Checks if current person already has this Rock Attribute and will append the current F1 attribute value to the Rock one.
                        //                if (newAttributes.Find(a => (a.AttributeId == 1035 && a.EntityId == personId)) != null)
                        //                {
                        //                    ReportProgress(0, string.Format("Adding Continue Giving Above Tithe 2009"));
                        //                    newAttributes.Find(a => a.AttributeId == 1035 && a.EntityId == personId).Value += "2C8B55AF-B5E2-41F9-9E08-C2E6F4624550,";
                        //                }
                        //                else
                        //                {
                        //                    attributeValues.AttributeId = 1035;
                        //                    attributeValues.EntityId = (int)personId;
                        //                    attributeValues.Value = "2C8B55AF-B5E2-41F9-9E08-C2E6F4624550,";
                        //                }
                        //                break;
                        //            case 2010:
                        //                //Checks if current person already has this Rock Attribute and will append the current F1 attribute value to the Rock one.
                        //                if (newAttributes.Find(a => (a.AttributeId == 1035 && a.EntityId == personId)) != null)
                        //                {
                        //                    ReportProgress(0, string.Format("Adding Continue Giving Above Tithe 2010"));
                        //                    newAttributes.Find(a => a.AttributeId == 1035 && a.EntityId == personId).Value += "FB260D37-AEF4-4277-959C-5884E579E1AC,";
                        //                }
                        //                else
                        //                {
                        //                    attributeValues.AttributeId = 1035;
                        //                    attributeValues.EntityId = (int)personId;
                        //                    attributeValues.Value = "FB260D37-AEF4-4277-959C-5884E579E1AC,";
                        //                }
                        //                break;
                        //            case 2011:
                        //                //Checks if current person already has this Rock Attribute and will append the current F1 attribute value to the Rock one.
                        //                if (newAttributes.Find(a => (a.AttributeId == 1035 && a.EntityId == personId)) != null)
                        //                {
                        //                    ReportProgress(0, string.Format("Adding Continue Giving Above Tithe 2011"));
                        //                    newAttributes.Find(a => a.AttributeId == 1035 && a.EntityId == personId).Value += "6E84915B-CC11-4E66-954E-9B1D786B2E6F,";
                        //                }
                        //                else
                        //                {
                        //                    attributeValues.AttributeId = 1035;
                        //                    attributeValues.EntityId = (int)personId;
                        //                    attributeValues.Value = "6E84915B-CC11-4E66-954E-9B1D786B2E6F,";
                        //                }
                        //                break;
                        //            case 2012:
                        //                //Checks if current person already has this Rock Attribute and will append the current F1 attribute value to the Rock one.
                        //                if (newAttributes.Find(a => (a.AttributeId == 1035 && a.EntityId == personId)) != null)
                        //                {
                        //                    ReportProgress(0, string.Format("Adding Continue Giving Above Tithe 2012"));
                        //                    newAttributes.Find(a => a.AttributeId == 1035 && a.EntityId == personId).Value += "4ED12DFD-BA8F-4760-A045-E7AC898BEC50,";
                        //                }
                        //                else
                        //                {
                        //                    attributeValues.AttributeId = 1035;
                        //                    attributeValues.EntityId = (int)personId;
                        //                    attributeValues.Value = "4ED12DFD-BA8F-4760-A045-E7AC898BEC50,";
                        //                }
                        //                break;
                        //            case 2013:
                        //                //Checks if current person already has this Rock Attribute and will append the current F1 attribute value to the Rock one.
                        //                if (newAttributes.Find(a => (a.AttributeId == 1035 && a.EntityId == personId)) != null)
                        //                {
                        //                    ReportProgress(0, string.Format("Adding Continue Giving Above Tithe 2013"));
                        //                    newAttributes.Find(a => a.AttributeId == 1035 && a.EntityId == personId).Value += "AFEC8401-3E49-4895-B320-6FF4918A5F4D,";
                        //                }
                        //                else
                        //                {
                        //                    attributeValues.AttributeId = 1035;
                        //                    attributeValues.EntityId = (int)personId;
                        //                    attributeValues.Value = "AFEC8401-3E49-4895-B320-6FF4918A5F4D,";
                        //                }
                        //                break;
                        //            default:
                        //                break;
                        //        }

                        //    }

                        //    //if (f1StartDate != null)
                        //    //{
                        //    //    if (f1EndDate != null)
                        //    //    {
                        //    //        // Checks if current person already has this Rock Attribute and will append the current F1 attribute value to the Rock one.
                        //    //        if (newAttributes.Find(a => (a.AttributeId == 1097 && a.EntityId == personId)) != null)
                        //    //        {
                        //    //            ReportProgress(0, string.Format("Adding Continue Giving Above Tithe"));
                        //    //            newAttributes.Find(a => a.AttributeId == 1097 && a.EntityId == personId).Value += "2008 Spring Classic,";
                        //    //        }
                        //    //        else
                        //    //        {
                        //    //            attributeValues.AttributeId = 1035;
                        //    //            attributeValues.EntityId = (int)personId;
                        //    //            attributeValues.Value = DateRange(f1StartDate, f1EndDate);
                        //    //        }
                        //    //    }
                        //    //    else
                        //    //    {
                        //    //        attributeValues.AttributeId = 1035;
                        //    //        attributeValues.EntityId = (int)personId;
                        //    //        attributeValues.Value = DateRange(f1StartDate, f1EndDate) + ",";
                        //    //    }
                        //    //}
                        //}
                        //else if (f1AttributeName == "Continue Tithing")
                        //{

                        //    DateTime startDate = (DateTime)f1StartDate;

                        //    if (f1StartDate != null)
                        //    {
                        //        switch (startDate.Year)
                        //        {
                        //            case 2001:
                        //                //Checks if current person already has this Rock Attribute and will append the current F1 attribute value to the Rock one.
                        //                if (newAttributes.Find(a => (a.AttributeId == 1036 && a.EntityId == personId)) != null)
                        //                {
                        //                    ReportProgress(0, string.Format("Adding Continue Tithe 2001"));
                        //                    newAttributes.Find(a => a.AttributeId == 1036 && a.EntityId == personId).Value += "B9A40993-7758-49A3-BE6B-00E930FCF690,";
                        //                }
                        //                else
                        //                {
                        //                    attributeValues.AttributeId = 1036;
                        //                    attributeValues.EntityId = (int)personId;
                        //                    attributeValues.Value = "B9A40993-7758-49A3-BE6B-00E930FCF690,";
                        //                }
                        //                break;
                        //            case 2002:
                        //                //Checks if current person already has this Rock Attribute and will append the current F1 attribute value to the Rock one.
                        //                if (newAttributes.Find(a => (a.AttributeId == 1036 && a.EntityId == personId)) != null)
                        //                {
                        //                    ReportProgress(0, string.Format("Adding Continue Tithe 2002"));
                        //                    newAttributes.Find(a => a.AttributeId == 1036 && a.EntityId == personId).Value += "56BF96EF-561E-424D-BA85-A93674569B47,";
                        //                }
                        //                else
                        //                {
                        //                    attributeValues.AttributeId = 1036;
                        //                    attributeValues.EntityId = (int)personId;
                        //                    attributeValues.Value = "56BF96EF-561E-424D-BA85-A93674569B47,";
                        //                }
                        //                break;
                        //            case 2003:
                        //                //Checks if current person already has this Rock Attribute and will append the current F1 attribute value to the Rock one.
                        //                if (newAttributes.Find(a => (a.AttributeId == 1036 && a.EntityId == personId)) != null)
                        //                {
                        //                    ReportProgress(0, string.Format("Adding Continue Tithe 2003"));
                        //                    newAttributes.Find(a => a.AttributeId == 1036 && a.EntityId == personId).Value += "74EB6703-DEB4-4CEA-81E2-5EC7ED81BB18,";
                        //                }
                        //                else
                        //                {
                        //                    attributeValues.AttributeId = 1036;
                        //                    attributeValues.EntityId = (int)personId;
                        //                    attributeValues.Value = "74EB6703-DEB4-4CEA-81E2-5EC7ED81BB18,";
                        //                }
                        //                break;
                        //            case 2004:
                        //                //Checks if current person already has this Rock Attribute and will append the current F1 attribute value to the Rock one.
                        //                if (newAttributes.Find(a => (a.AttributeId == 1036 && a.EntityId == personId)) != null)
                        //                {
                        //                    ReportProgress(0, string.Format("Adding Continue Tithe 2004"));
                        //                    newAttributes.Find(a => a.AttributeId == 1036 && a.EntityId == personId).Value += "DD28ACBD-8B2C-49CC-81C9-B7FFE4D8E3C2,";
                        //                }
                        //                else
                        //                {
                        //                    attributeValues.AttributeId = 1036;
                        //                    attributeValues.EntityId = (int)personId;
                        //                    attributeValues.Value = "DD28ACBD-8B2C-49CC-81C9-B7FFE4D8E3C2,";
                        //                }
                        //                break;
                        //            case 2005:
                        //                //Checks if current person already has this Rock Attribute and will append the current F1 attribute value to the Rock one.
                        //                if (newAttributes.Find(a => (a.AttributeId == 1036 && a.EntityId == personId)) != null)
                        //                {
                        //                    ReportProgress(0, string.Format("Adding Continue Tithe 2005"));
                        //                    newAttributes.Find(a => a.AttributeId == 1036 && a.EntityId == personId).Value += "F18A88B7-5228-4B7D-8079-4B118DF792C7,";
                        //                }
                        //                else
                        //                {
                        //                    attributeValues.AttributeId = 1036;
                        //                    attributeValues.EntityId = (int)personId;
                        //                    attributeValues.Value = "F18A88B7-5228-4B7D-8079-4B118DF792C7,";
                        //                }
                        //                break;
                        //            case 2006:
                        //                //Checks if current person already has this Rock Attribute and will append the current F1 attribute value to the Rock one.
                        //                if (newAttributes.Find(a => (a.AttributeId == 1036 && a.EntityId == personId)) != null)
                        //                {
                        //                    ReportProgress(0, string.Format("Adding Continue Tithe 2006"));
                        //                    newAttributes.Find(a => a.AttributeId == 1036 && a.EntityId == personId).Value += "719DF19D-B5AF-4125-B708-BDC22EB64E8F,";
                        //                }
                        //                else
                        //                {
                        //                    attributeValues.AttributeId = 1036;
                        //                    attributeValues.EntityId = (int)personId;
                        //                    attributeValues.Value = "719DF19D-B5AF-4125-B708-BDC22EB64E8F,";
                        //                }
                        //                break;
                        //            case 2007:
                        //                //Checks if current person already has this Rock Attribute and will append the current F1 attribute value to the Rock one.
                        //                if (newAttributes.Find(a => (a.AttributeId == 1036 && a.EntityId == personId)) != null)
                        //                {
                        //                    ReportProgress(0, string.Format("Adding Continue Tithe 2007"));
                        //                    newAttributes.Find(a => a.AttributeId == 1036 && a.EntityId == personId).Value += "CE44EA17-020E-4B97-8975-4DE01830163D,";
                        //                }
                        //                else
                        //                {
                        //                    attributeValues.AttributeId = 1036;
                        //                    attributeValues.EntityId = (int)personId;
                        //                    attributeValues.Value = "CE44EA17-020E-4B97-8975-4DE01830163D,";
                        //                }
                        //                break;
                        //            case 2008:
                        //                //Checks if current person already has this Rock Attribute and will append the current F1 attribute value to the Rock one.
                        //                if (newAttributes.Find(a => (a.AttributeId == 1036 && a.EntityId == personId)) != null)
                        //                {
                        //                    ReportProgress(0, string.Format("Adding Continue Tithe 2008"));
                        //                    newAttributes.Find(a => a.AttributeId == 1036 && a.EntityId == personId).Value += "6810C1C9-85BD-42E9-9E04-85801A93096D,";
                        //                }
                        //                else
                        //                {
                        //                    attributeValues.AttributeId = 1036;
                        //                    attributeValues.EntityId = (int)personId;
                        //                    attributeValues.Value = "6810C1C9-85BD-42E9-9E04-85801A93096D,";
                        //                }
                        //                break;
                        //            case 2009:
                        //                //Checks if current person already has this Rock Attribute and will append the current F1 attribute value to the Rock one.
                        //                if (newAttributes.Find(a => (a.AttributeId == 1036 && a.EntityId == personId)) != null)
                        //                {
                        //                    ReportProgress(0, string.Format("Adding Continue Tithe 2009"));
                        //                    newAttributes.Find(a => a.AttributeId == 1036 && a.EntityId == personId).Value += "2C8B55AF-B5E2-41F9-9E08-C2E6F4624550,";
                        //                }
                        //                else
                        //                {
                        //                    attributeValues.AttributeId = 1036;
                        //                    attributeValues.EntityId = (int)personId;
                        //                    attributeValues.Value = "2C8B55AF-B5E2-41F9-9E08-C2E6F4624550,";
                        //                }
                        //                break;
                        //            case 2010:
                        //                //Checks if current person already has this Rock Attribute and will append the current F1 attribute value to the Rock one.
                        //                if (newAttributes.Find(a => (a.AttributeId == 1036 && a.EntityId == personId)) != null)
                        //                {
                        //                    ReportProgress(0, string.Format("Adding Continue Tithe 2010"));
                        //                    newAttributes.Find(a => a.AttributeId == 1036 && a.EntityId == personId).Value += "FB260D37-AEF4-4277-959C-5884E579E1AC,";
                        //                }
                        //                else
                        //                {
                        //                    attributeValues.AttributeId = 1036;
                        //                    attributeValues.EntityId = (int)personId;
                        //                    attributeValues.Value = "FB260D37-AEF4-4277-959C-5884E579E1AC,";
                        //                }
                        //                break;
                        //            case 2011:
                        //                //Checks if current person already has this Rock Attribute and will append the current F1 attribute value to the Rock one.
                        //                if (newAttributes.Find(a => (a.AttributeId == 1036 && a.EntityId == personId)) != null)
                        //                {
                        //                    ReportProgress(0, string.Format("Adding Continue Tithe 2011"));
                        //                    newAttributes.Find(a => a.AttributeId == 1036 && a.EntityId == personId).Value += "6E84915B-CC11-4E66-954E-9B1D786B2E6F,";
                        //                }
                        //                else
                        //                {
                        //                    attributeValues.AttributeId = 1036;
                        //                    attributeValues.EntityId = (int)personId;
                        //                    attributeValues.Value = "6E84915B-CC11-4E66-954E-9B1D786B2E6F,";
                        //                }
                        //                break;
                        //            case 2012:
                        //                //Checks if current person already has this Rock Attribute and will append the current F1 attribute value to the Rock one.
                        //                if (newAttributes.Find(a => (a.AttributeId == 1036 && a.EntityId == personId)) != null)
                        //                {
                        //                    ReportProgress(0, string.Format("Adding Continue Tithe 2012"));
                        //                    newAttributes.Find(a => a.AttributeId == 1036 && a.EntityId == personId).Value += "4ED12DFD-BA8F-4760-A045-E7AC898BEC50,";
                        //                }
                        //                else
                        //                {
                        //                    attributeValues.AttributeId = 1036;
                        //                    attributeValues.EntityId = (int)personId;
                        //                    attributeValues.Value = "4ED12DFD-BA8F-4760-A045-E7AC898BEC50,";
                        //                }
                        //                break;
                        //            case 2013:
                        //                //Checks if current person already has this Rock Attribute and will append the current F1 attribute value to the Rock one.
                        //                if (newAttributes.Find(a => (a.AttributeId == 1036 && a.EntityId == personId)) != null)
                        //                {
                        //                    ReportProgress(0, string.Format("Adding Continue Tithe 2013"));
                        //                    newAttributes.Find(a => a.AttributeId == 1036 && a.EntityId == personId).Value += "AFEC8401-3E49-4895-B320-6FF4918A5F4D,";
                        //                }
                        //                else
                        //                {
                        //                    attributeValues.AttributeId = 1036;
                        //                    attributeValues.EntityId = (int)personId;
                        //                    attributeValues.Value = "AFEC8401-3E49-4895-B320-6FF4918A5F4D,";
                        //                }
                        //                break;
                        //            default:
                        //                break;
                        //        }

                        //    }

                        //    //if (f1StartDate != null)
                        //    //{
                        //    //    if (f1EndDate != null)
                        //    //    {
                        //    //        attributeValues.AttributeId = 1036;
                        //    //        attributeValues.EntityId = (int)personId;
                        //    //        attributeValues.Value = DateRange(f1StartDate, f1EndDate);
                        //    //    }
                        //    //    else
                        //    //    {
                        //    //        attributeValues.AttributeId = 1036;
                        //    //        attributeValues.EntityId = (int)personId;
                        //    //        attributeValues.Value = DateRange(f1StartDate, f1EndDate) + ",";
                        //    //    }
                        //    //}
                        //}
                        //else if (f1AttributeName == "I Will")
                        //{
                        //    if (f1StartDate != null)
                        //    {
                        //        if (f1EndDate != null)
                        //        {
                        //            attributeValues.AttributeId = 1103;
                        //            attributeValues.EntityId = (int)personId;
                        //            attributeValues.Value = DateRange(f1StartDate, f1EndDate);
                        //        }
                        //        else
                        //        {
                        //            attributeValues.AttributeId = 1103;
                        //            attributeValues.EntityId = (int)personId;
                        //            attributeValues.Value = DateRange(f1StartDate, f1EndDate) + ",";
                        //        }
                        //    }
                        //}
                        if (f1AttributeName == "I'm Connecting (begin) 2013" || f1AttributeName == "I'm Connecting (begin) 2.2013")
                        {
                            if (f1StartDate != null)
                            {
                                if (f1EndDate != null)
                                {
                                    attributeValues.AttributeId = 1104;
                                    attributeValues.EntityId = (int)personId;
                                    attributeValues.Value = DateRange(f1StartDate, f1EndDate);
                                }
                                else
                                {
                                    attributeValues.AttributeId = 1104;
                                    attributeValues.EntityId = (int)personId;
                                    attributeValues.Value = DateRange(f1StartDate, f1EndDate) + ",";
                                }
                            }
                            else
                            {
                                if (f1AttributeName == "I'm Connecting (begin) 2013")
                                {
                                    attributeValues.AttributeId = 1104;
                                    attributeValues.EntityId = (int)personId;
                                    attributeValues.Value = "1/1/2013,12/30/2013";
                                }
                                else if (f1AttributeName == "I'm Connecting (begin) 2.2013")
                                {
                                    attributeValues.AttributeId = 1104;
                                    attributeValues.EntityId = (int)personId;
                                    attributeValues.Value = "2/1/2013,12/30/2013";
                                }
                            }
                        }
                        else if (f1AttributeName == "I'm Connecting (continue) 2013" || f1AttributeName == "I'm Connecting (continue) 2.2013")
                        {
                            if (f1StartDate != null)
                            {
                                if (f1EndDate != null)
                                {
                                    attributeValues.AttributeId = 1105;
                                    attributeValues.EntityId = (int)personId;
                                    attributeValues.Value = DateRange(f1StartDate, f1EndDate);
                                }
                                else
                                {
                                    attributeValues.AttributeId = 1105;
                                    attributeValues.EntityId = (int)personId;
                                    attributeValues.Value = DateRange(f1StartDate, f1EndDate) + ",";
                                }
                            }
                            else
                            {
                                if (f1AttributeName == "I'm Connecting (continue) 2013")
                                {
                                    attributeValues.AttributeId = 1105;
                                    attributeValues.EntityId = (int)personId;
                                    attributeValues.Value = "1/1/2013,12/30/2013";
                                }
                                else if (f1AttributeName == "I'm Connecting (continue) 2.2013")
                                {
                                    attributeValues.AttributeId = 1105;
                                    attributeValues.EntityId = (int)personId;
                                    attributeValues.Value = "2/1/2013,12/30/2013";
                                }
                            }
                        }
                        else if (f1AttributeName == "All In (begin) 2014" || f1AttributeName == "All In (begin) 2.2014")
                        {
                            if (f1StartDate != null)
                            {
                                if (f1EndDate != null)
                                {
                                    attributeValues.AttributeId = 969;
                                    attributeValues.EntityId = (int)personId;
                                    attributeValues.Value = DateRange(f1StartDate, f1EndDate);
                                }
                                else
                                {
                                    attributeValues.AttributeId = 969;
                                    attributeValues.EntityId = (int)personId;
                                    attributeValues.Value = DateRange(f1StartDate, f1EndDate) + ",";
                                }
                            }
                            else
                            {
                                if (f1AttributeName == "All In (begin) 2014")
                                {
                                    attributeValues.AttributeId = 969;
                                    attributeValues.EntityId = (int)personId;
                                    attributeValues.Value = "1/1/2013,12/30/2013";
                                }
                                else if (f1AttributeName == "All In (begin) 2.2014")
                                {
                                    attributeValues.AttributeId = 969;
                                    attributeValues.EntityId = (int)personId;
                                    attributeValues.Value = "2/1/2013,12/30/2013";
                                }
                            }
                        }
                        else if (f1AttributeName == "All In (continue) 2014" || f1AttributeName == "All In (continue) 2.2014")
                        {
                            if (f1StartDate != null)
                            {
                                if (f1EndDate != null)
                                {
                                    attributeValues.AttributeId = 970;
                                    attributeValues.EntityId = (int)personId;
                                    attributeValues.Value = DateRange(f1StartDate, f1EndDate);
                                }
                                else
                                {
                                    attributeValues.AttributeId = 970;
                                    attributeValues.EntityId = (int)personId;
                                    attributeValues.Value = DateRange(f1StartDate, f1EndDate) + ",";
                                }
                            }
                            else
                            {
                                if (f1AttributeName == "All In (continue) 2014")
                                {
                                    attributeValues.AttributeId = 970;
                                    attributeValues.EntityId = (int)personId;
                                    attributeValues.Value = "1/1/2013,12/30/2013";
                                }
                                else if (f1AttributeName == "All In (continue) 2.2014")
                                {
                                    attributeValues.AttributeId = 970;
                                    attributeValues.EntityId = (int)personId;
                                    attributeValues.Value = "2/1/2013,12/30/2013";
                                }
                            }
                        }
                        //else if (f1AttributeName == "Start Giving Above Tithe")
                        //{

                        //    DateTime startDate = (DateTime)f1StartDate;

                        //    if (f1StartDate != null)
                        //    {
                        //        switch (startDate.Year)
                        //        {
                        //            case 2001:
                        //                //Checks if current person already has this Rock Attribute and will append the current F1 attribute value to the Rock one.
                        //                if (newAttributes.Find(a => (a.AttributeId == 1213 && a.EntityId == personId)) != null)
                        //                {
                        //                    ReportProgress(0, string.Format("Adding Start Giving Above Tithe 2001"));
                        //                    newAttributes.Find(a => a.AttributeId == 1213 && a.EntityId == personId).Value += "B9A40993-7758-49A3-BE6B-00E930FCF690,";
                        //                }
                        //                else
                        //                {
                        //                    attributeValues.AttributeId = 1213;
                        //                    attributeValues.EntityId = (int)personId;
                        //                    attributeValues.Value = "B9A40993-7758-49A3-BE6B-00E930FCF690,";
                        //                }
                        //                break;
                        //            case 2002:
                        //                //Checks if current person already has this Rock Attribute and will append the current F1 attribute value to the Rock one.
                        //                if (newAttributes.Find(a => (a.AttributeId == 1213 && a.EntityId == personId)) != null)
                        //                {
                        //                    ReportProgress(0, string.Format("Adding Start Giving Above Tithe 2002"));
                        //                    newAttributes.Find(a => a.AttributeId == 1213 && a.EntityId == personId).Value += "56BF96EF-561E-424D-BA85-A93674569B47,";
                        //                }
                        //                else
                        //                {
                        //                    attributeValues.AttributeId = 1213;
                        //                    attributeValues.EntityId = (int)personId;
                        //                    attributeValues.Value = "56BF96EF-561E-424D-BA85-A93674569B47,";
                        //                }
                        //                break;
                        //            case 2003:
                        //                //Checks if current person already has this Rock Attribute and will append the current F1 attribute value to the Rock one.
                        //                if (newAttributes.Find(a => (a.AttributeId == 1213 && a.EntityId == personId)) != null)
                        //                {
                        //                    ReportProgress(0, string.Format("Adding Start Giving Above Tithe 2003"));
                        //                    newAttributes.Find(a => a.AttributeId == 1213 && a.EntityId == personId).Value += "74EB6703-DEB4-4CEA-81E2-5EC7ED81BB18,";
                        //                }
                        //                else
                        //                {
                        //                    attributeValues.AttributeId = 1213;
                        //                    attributeValues.EntityId = (int)personId;
                        //                    attributeValues.Value = "74EB6703-DEB4-4CEA-81E2-5EC7ED81BB18,";
                        //                }
                        //                break;
                        //            case 2004:
                        //                //Checks if current person already has this Rock Attribute and will append the current F1 attribute value to the Rock one.
                        //                if (newAttributes.Find(a => (a.AttributeId == 1213 && a.EntityId == personId)) != null)
                        //                {
                        //                    ReportProgress(0, string.Format("Adding Start Giving Above Tithe 2004"));
                        //                    newAttributes.Find(a => a.AttributeId == 1213 && a.EntityId == personId).Value += "DD28ACBD-8B2C-49CC-81C9-B7FFE4D8E3C2,";
                        //                }
                        //                else
                        //                {
                        //                    attributeValues.AttributeId = 1213;
                        //                    attributeValues.EntityId = (int)personId;
                        //                    attributeValues.Value = "DD28ACBD-8B2C-49CC-81C9-B7FFE4D8E3C2,";
                        //                }
                        //                break;
                        //            case 2005:
                        //                //Checks if current person already has this Rock Attribute and will append the current F1 attribute value to the Rock one.
                        //                if (newAttributes.Find(a => (a.AttributeId == 1213 && a.EntityId == personId)) != null)
                        //                {
                        //                    ReportProgress(0, string.Format("Adding Start Giving Above Tithe 2005"));
                        //                    newAttributes.Find(a => a.AttributeId == 1213 && a.EntityId == personId).Value += "F18A88B7-5228-4B7D-8079-4B118DF792C7,";
                        //                }
                        //                else
                        //                {
                        //                    attributeValues.AttributeId = 1213;
                        //                    attributeValues.EntityId = (int)personId;
                        //                    attributeValues.Value = "F18A88B7-5228-4B7D-8079-4B118DF792C7,";
                        //                }
                        //                break;
                        //            case 2006:
                        //                //Checks if current person already has this Rock Attribute and will append the current F1 attribute value to the Rock one.
                        //                if (newAttributes.Find(a => (a.AttributeId == 1213 && a.EntityId == personId)) != null)
                        //                {
                        //                    ReportProgress(0, string.Format("Adding Start Giving Above Tithe 2006"));
                        //                    newAttributes.Find(a => a.AttributeId == 1213 && a.EntityId == personId).Value += "719DF19D-B5AF-4125-B708-BDC22EB64E8F,";
                        //                }
                        //                else
                        //                {
                        //                    attributeValues.AttributeId = 1213;
                        //                    attributeValues.EntityId = (int)personId;
                        //                    attributeValues.Value = "719DF19D-B5AF-4125-B708-BDC22EB64E8F,";
                        //                }
                        //                break;
                        //            case 2007:
                        //                //Checks if current person already has this Rock Attribute and will append the current F1 attribute value to the Rock one.
                        //                if (newAttributes.Find(a => (a.AttributeId == 1213 && a.EntityId == personId)) != null)
                        //                {
                        //                    ReportProgress(0, string.Format("Adding Start Giving Above Tithe 2007"));
                        //                    newAttributes.Find(a => a.AttributeId == 1213 && a.EntityId == personId).Value += "CE44EA17-020E-4B97-8975-4DE01830163D,";
                        //                }
                        //                else
                        //                {
                        //                    attributeValues.AttributeId = 1213;
                        //                    attributeValues.EntityId = (int)personId;
                        //                    attributeValues.Value = "CE44EA17-020E-4B97-8975-4DE01830163D,";
                        //                }
                        //                break;
                        //            case 2008:
                        //                //Checks if current person already has this Rock Attribute and will append the current F1 attribute value to the Rock one.
                        //                if (newAttributes.Find(a => (a.AttributeId == 1213 && a.EntityId == personId)) != null)
                        //                {
                        //                    ReportProgress(0, string.Format("Adding Start Giving Above Tithe 2008"));
                        //                    newAttributes.Find(a => a.AttributeId == 1213 && a.EntityId == personId).Value += "6810C1C9-85BD-42E9-9E04-85801A93096D,";
                        //                }
                        //                else
                        //                {
                        //                    attributeValues.AttributeId = 1213;
                        //                    attributeValues.EntityId = (int)personId;
                        //                    attributeValues.Value = "6810C1C9-85BD-42E9-9E04-85801A93096D,";
                        //                }
                        //                break;
                        //            case 2009:
                        //                //Checks if current person already has this Rock Attribute and will append the current F1 attribute value to the Rock one.
                        //                if (newAttributes.Find(a => (a.AttributeId == 1213 && a.EntityId == personId)) != null)
                        //                {
                        //                    ReportProgress(0, string.Format("Adding Start Giving Above Tithe 2009"));
                        //                    newAttributes.Find(a => a.AttributeId == 1213 && a.EntityId == personId).Value += "2C8B55AF-B5E2-41F9-9E08-C2E6F4624550,";
                        //                }
                        //                else
                        //                {
                        //                    attributeValues.AttributeId = 1213;
                        //                    attributeValues.EntityId = (int)personId;
                        //                    attributeValues.Value = "2C8B55AF-B5E2-41F9-9E08-C2E6F4624550,";
                        //                }
                        //                break;
                        //            case 2010:
                        //                //Checks if current person already has this Rock Attribute and will append the current F1 attribute value to the Rock one.
                        //                if (newAttributes.Find(a => (a.AttributeId == 1213 && a.EntityId == personId)) != null)
                        //                {
                        //                    ReportProgress(0, string.Format("Adding Start Giving Above Tithe 2010"));
                        //                    newAttributes.Find(a => a.AttributeId == 1213 && a.EntityId == personId).Value += "FB260D37-AEF4-4277-959C-5884E579E1AC,";
                        //                }
                        //                else
                        //                {
                        //                    attributeValues.AttributeId = 1213;
                        //                    attributeValues.EntityId = (int)personId;
                        //                    attributeValues.Value = "FB260D37-AEF4-4277-959C-5884E579E1AC,";
                        //                }
                        //                break;
                        //            case 2011:
                        //                //Checks if current person already has this Rock Attribute and will append the current F1 attribute value to the Rock one.
                        //                if (newAttributes.Find(a => (a.AttributeId == 1213 && a.EntityId == personId)) != null)
                        //                {
                        //                    ReportProgress(0, string.Format("Adding Start Giving Above Tithe 2011"));
                        //                    newAttributes.Find(a => a.AttributeId == 1213 && a.EntityId == personId).Value += "6E84915B-CC11-4E66-954E-9B1D786B2E6F,";
                        //                }
                        //                else
                        //                {
                        //                    attributeValues.AttributeId = 1213;
                        //                    attributeValues.EntityId = (int)personId;
                        //                    attributeValues.Value = "6E84915B-CC11-4E66-954E-9B1D786B2E6F,";
                        //                }
                        //                break;
                        //            case 2012:
                        //                //Checks if current person already has this Rock Attribute and will append the current F1 attribute value to the Rock one.
                        //                if (newAttributes.Find(a => (a.AttributeId == 1213 && a.EntityId == personId)) != null)
                        //                {
                        //                    ReportProgress(0, string.Format("Adding Start Giving Above Tithe 2012"));
                        //                    newAttributes.Find(a => a.AttributeId == 1213 && a.EntityId == personId).Value += "4ED12DFD-BA8F-4760-A045-E7AC898BEC50,";
                        //                }
                        //                else
                        //                {
                        //                    attributeValues.AttributeId = 1213;
                        //                    attributeValues.EntityId = (int)personId;
                        //                    attributeValues.Value = "4ED12DFD-BA8F-4760-A045-E7AC898BEC50,";
                        //                }
                        //                break;
                        //            case 2013:
                        //                //Checks if current person already has this Rock Attribute and will append the current F1 attribute value to the Rock one.
                        //                if (newAttributes.Find(a => (a.AttributeId == 1213 && a.EntityId == personId)) != null)
                        //                {
                        //                    ReportProgress(0, string.Format("Adding Start Giving Above Tithe 2013"));
                        //                    newAttributes.Find(a => a.AttributeId == 1213 && a.EntityId == personId).Value += "AFEC8401-3E49-4895-B320-6FF4918A5F4D,";
                        //                }
                        //                else
                        //                {
                        //                    attributeValues.AttributeId = 1213;
                        //                    attributeValues.EntityId = (int)personId;
                        //                    attributeValues.Value = "AFEC8401-3E49-4895-B320-6FF4918A5F4D,";
                        //                }
                        //                break;
                        //            default:
                        //                break;
                        //        }
                        //    }

                        //    //if (f1StartDate != null)
                        //    //{
                        //    //    if (f1EndDate != null)
                        //    //    {
                        //    //        attributeValues.AttributeId = 1213;
                        //    //        attributeValues.EntityId = (int)personId;
                        //    //        attributeValues.Value = DateRange(f1StartDate, f1EndDate);
                        //    //    }
                        //    //    else
                        //    //    {
                        //    //        attributeValues.AttributeId = 1213;
                        //    //        attributeValues.EntityId = (int)personId;
                        //    //        attributeValues.Value = DateRange(f1StartDate, f1EndDate) + ",";
                        //    //    }
                        //    //}
                        //}
                        //else if (f1AttributeName == "Start Tithing")
                        //{

                        //    DateTime startDate = (DateTime)f1StartDate;

                        //    if (f1StartDate != null)
                        //    {
                        //        switch (startDate.Year)
                        //        {
                        //            case 2001:
                        //                //Checks if current person already has this Rock Attribute and will append the current F1 attribute value to the Rock one.
                        //                if (newAttributes.Find(a => (a.AttributeId == 1214 && a.EntityId == personId)) != null)
                        //                {
                        //                    ReportProgress(0, string.Format("Adding Start Giving Above Tithe 2001"));
                        //                    newAttributes.Find(a => a.AttributeId == 1214 && a.EntityId == personId).Value += "B9A40993-7758-49A3-BE6B-00E930FCF690,";
                        //                }
                        //                else
                        //                {
                        //                    attributeValues.AttributeId = 1214;
                        //                    attributeValues.EntityId = (int)personId;
                        //                    attributeValues.Value = "B9A40993-7758-49A3-BE6B-00E930FCF690,";
                        //                }
                        //                break;
                        //            case 2002:
                        //                //Checks if current person already has this Rock Attribute and will append the current F1 attribute value to the Rock one.
                        //                if (newAttributes.Find(a => (a.AttributeId == 1214 && a.EntityId == personId)) != null)
                        //                {
                        //                    ReportProgress(0, string.Format("Adding Start Giving Above Tithe 2002"));
                        //                    newAttributes.Find(a => a.AttributeId == 1214 && a.EntityId == personId).Value += "56BF96EF-561E-424D-BA85-A93674569B47,";
                        //                }
                        //                else
                        //                {
                        //                    attributeValues.AttributeId = 1214;
                        //                    attributeValues.EntityId = (int)personId;
                        //                    attributeValues.Value = "56BF96EF-561E-424D-BA85-A93674569B47,";
                        //                }
                        //                break;
                        //            case 2003:
                        //                //Checks if current person already has this Rock Attribute and will append the current F1 attribute value to the Rock one.
                        //                if (newAttributes.Find(a => (a.AttributeId == 1214 && a.EntityId == personId)) != null)
                        //                {
                        //                    ReportProgress(0, string.Format("Adding Start Giving Above Tithe 2003"));
                        //                    newAttributes.Find(a => a.AttributeId == 1214 && a.EntityId == personId).Value += "74EB6703-DEB4-4CEA-81E2-5EC7ED81BB18,";
                        //                }
                        //                else
                        //                {
                        //                    attributeValues.AttributeId = 1214;
                        //                    attributeValues.EntityId = (int)personId;
                        //                    attributeValues.Value = "74EB6703-DEB4-4CEA-81E2-5EC7ED81BB18,";
                        //                }
                        //                break;
                        //            case 2004:
                        //                //Checks if current person already has this Rock Attribute and will append the current F1 attribute value to the Rock one.
                        //                if (newAttributes.Find(a => (a.AttributeId == 1214 && a.EntityId == personId)) != null)
                        //                {
                        //                    ReportProgress(0, string.Format("Adding Start Giving Above Tithe 2004"));
                        //                    newAttributes.Find(a => a.AttributeId == 1214 && a.EntityId == personId).Value += "DD28ACBD-8B2C-49CC-81C9-B7FFE4D8E3C2,";
                        //                }
                        //                else
                        //                {
                        //                    attributeValues.AttributeId = 1214;
                        //                    attributeValues.EntityId = (int)personId;
                        //                    attributeValues.Value = "DD28ACBD-8B2C-49CC-81C9-B7FFE4D8E3C2,";
                        //                }
                        //                break;
                        //            case 2005:
                        //                //Checks if current person already has this Rock Attribute and will append the current F1 attribute value to the Rock one.
                        //                if (newAttributes.Find(a => (a.AttributeId == 1214 && a.EntityId == personId)) != null)
                        //                {
                        //                    ReportProgress(0, string.Format("Adding Start Giving Above Tithe 2005"));
                        //                    newAttributes.Find(a => a.AttributeId == 1214 && a.EntityId == personId).Value += "F18A88B7-5228-4B7D-8079-4B118DF792C7,";
                        //                }
                        //                else
                        //                {
                        //                    attributeValues.AttributeId = 1214;
                        //                    attributeValues.EntityId = (int)personId;
                        //                    attributeValues.Value = "F18A88B7-5228-4B7D-8079-4B118DF792C7,";
                        //                }
                        //                break;
                        //            case 2006:
                        //                //Checks if current person already has this Rock Attribute and will append the current F1 attribute value to the Rock one.
                        //                if (newAttributes.Find(a => (a.AttributeId == 1214 && a.EntityId == personId)) != null)
                        //                {
                        //                    ReportProgress(0, string.Format("Adding Start Giving Above Tithe 2006"));
                        //                    newAttributes.Find(a => a.AttributeId == 1214 && a.EntityId == personId).Value += "719DF19D-B5AF-4125-B708-BDC22EB64E8F,";
                        //                }
                        //                else
                        //                {
                        //                    attributeValues.AttributeId = 1214;
                        //                    attributeValues.EntityId = (int)personId;
                        //                    attributeValues.Value = "719DF19D-B5AF-4125-B708-BDC22EB64E8F,";
                        //                }
                        //                break;
                        //            case 2007:
                        //                //Checks if current person already has this Rock Attribute and will append the current F1 attribute value to the Rock one.
                        //                if (newAttributes.Find(a => (a.AttributeId == 1214 && a.EntityId == personId)) != null)
                        //                {
                        //                    ReportProgress(0, string.Format("Adding Start Giving Above Tithe 2007"));
                        //                    newAttributes.Find(a => a.AttributeId == 1214 && a.EntityId == personId).Value += "CE44EA17-020E-4B97-8975-4DE01830163D,";
                        //                }
                        //                else
                        //                {
                        //                    attributeValues.AttributeId = 1214;
                        //                    attributeValues.EntityId = (int)personId;
                        //                    attributeValues.Value = "CE44EA17-020E-4B97-8975-4DE01830163D,";
                        //                }
                        //                break;
                        //            case 2008:
                        //                //Checks if current person already has this Rock Attribute and will append the current F1 attribute value to the Rock one.
                        //                if (newAttributes.Find(a => (a.AttributeId == 1214 && a.EntityId == personId)) != null)
                        //                {
                        //                    ReportProgress(0, string.Format("Adding Start Giving Above Tithe 2008"));
                        //                    newAttributes.Find(a => a.AttributeId == 1214 && a.EntityId == personId).Value += "6810C1C9-85BD-42E9-9E04-85801A93096D,";
                        //                }
                        //                else
                        //                {
                        //                    attributeValues.AttributeId = 1214;
                        //                    attributeValues.EntityId = (int)personId;
                        //                    attributeValues.Value = "6810C1C9-85BD-42E9-9E04-85801A93096D,";
                        //                }
                        //                break;
                        //            case 2009:
                        //                //Checks if current person already has this Rock Attribute and will append the current F1 attribute value to the Rock one.
                        //                if (newAttributes.Find(a => (a.AttributeId == 1214 && a.EntityId == personId)) != null)
                        //                {
                        //                    ReportProgress(0, string.Format("Adding Start Giving Above Tithe 2009"));
                        //                    newAttributes.Find(a => a.AttributeId == 1214 && a.EntityId == personId).Value += "2C8B55AF-B5E2-41F9-9E08-C2E6F4624550,";
                        //                }
                        //                else
                        //                {
                        //                    attributeValues.AttributeId = 1214;
                        //                    attributeValues.EntityId = (int)personId;
                        //                    attributeValues.Value = "2C8B55AF-B5E2-41F9-9E08-C2E6F4624550,";
                        //                }
                        //                break;
                        //            case 2010:
                        //                //Checks if current person already has this Rock Attribute and will append the current F1 attribute value to the Rock one.
                        //                if (newAttributes.Find(a => (a.AttributeId == 1214 && a.EntityId == personId)) != null)
                        //                {
                        //                    ReportProgress(0, string.Format("Adding Start Giving Above Tithe 2010"));
                        //                    newAttributes.Find(a => a.AttributeId == 1214 && a.EntityId == personId).Value += "FB260D37-AEF4-4277-959C-5884E579E1AC,";
                        //                }
                        //                else
                        //                {
                        //                    attributeValues.AttributeId = 1214;
                        //                    attributeValues.EntityId = (int)personId;
                        //                    attributeValues.Value = "FB260D37-AEF4-4277-959C-5884E579E1AC,";
                        //                }
                        //                break;
                        //            case 2011:
                        //                //Checks if current person already has this Rock Attribute and will append the current F1 attribute value to the Rock one.
                        //                if (newAttributes.Find(a => (a.AttributeId == 1214 && a.EntityId == personId)) != null)
                        //                {
                        //                    ReportProgress(0, string.Format("Adding Start Giving Above Tithe 2011"));
                        //                    newAttributes.Find(a => a.AttributeId == 1214 && a.EntityId == personId).Value += "6E84915B-CC11-4E66-954E-9B1D786B2E6F,";
                        //                }
                        //                else
                        //                {
                        //                    attributeValues.AttributeId = 1214;
                        //                    attributeValues.EntityId = (int)personId;
                        //                    attributeValues.Value = "6E84915B-CC11-4E66-954E-9B1D786B2E6F,";
                        //                }
                        //                break;
                        //            case 2012:
                        //                //Checks if current person already has this Rock Attribute and will append the current F1 attribute value to the Rock one.
                        //                if (newAttributes.Find(a => (a.AttributeId == 1214 && a.EntityId == personId)) != null)
                        //                {
                        //                    ReportProgress(0, string.Format("Adding Start Giving Above Tithe 2012"));
                        //                    newAttributes.Find(a => a.AttributeId == 1214 && a.EntityId == personId).Value += "4ED12DFD-BA8F-4760-A045-E7AC898BEC50,";
                        //                }
                        //                else
                        //                {
                        //                    attributeValues.AttributeId = 1214;
                        //                    attributeValues.EntityId = (int)personId;
                        //                    attributeValues.Value = "4ED12DFD-BA8F-4760-A045-E7AC898BEC50,";
                        //                }
                        //                break;
                        //            case 2013:
                        //                //Checks if current person already has this Rock Attribute and will append the current F1 attribute value to the Rock one.
                        //                if (newAttributes.Find(a => (a.AttributeId == 1214 && a.EntityId == personId)) != null)
                        //                {
                        //                    ReportProgress(0, string.Format("Adding Start Giving Above Tithe 2013"));
                        //                    newAttributes.Find(a => a.AttributeId == 1214 && a.EntityId == personId).Value += "AFEC8401-3E49-4895-B320-6FF4918A5F4D,";
                        //                }
                        //                else
                        //                {
                        //                    attributeValues.AttributeId = 1214;
                        //                    attributeValues.EntityId = (int)personId;
                        //                    attributeValues.Value = "AFEC8401-3E49-4895-B320-6FF4918A5F4D,";
                        //                }
                        //                break;
                        //            default:
                        //                break;
                        //        }
                        //    }

                        //    //    if (f1StartDate != null)
                        //    //    {
                        //    //        if (f1EndDate != null)
                        //    //        {
                        //    //            attributeValues.AttributeId = 1214;
                        //    //            attributeValues.EntityId = (int)personId;
                        //    //            attributeValues.Value = DateRange(f1StartDate, f1EndDate);
                        //    //        }
                        //    //        else
                        //    //        {
                        //    //            attributeValues.AttributeId = 1214;
                        //    //            attributeValues.EntityId = (int)personId;
                        //    //            attributeValues.Value = DateRange(f1StartDate, f1EndDate) + ",";
                        //    //        }
                        //    //    }
                        //}
                    }
                    else if (f1AttributeGroupName == "Up Front")
                    {
                        if (f1AttributeName == "Current Volunteers")
                        {
                            if (f1Comment != null)
                            {

                                attributeValues.AttributeId = 1053;
                                attributeValues.EntityId = (int)personId;
                                attributeValues.Value = f1Comment;
                            }
                            if (f1StartDate != null)
                            {
                                if (f1EndDate != null)
                                {
                                    attributeExtraDate.AttributeId = 1054;
                                    attributeExtraDate.EntityId = (int)personId;
                                    attributeExtraDate.Value = DateRange(f1StartDate, f1EndDate);
                                }
                                else
                                {
                                    attributeExtraDate.AttributeId = 1054;
                                    attributeExtraDate.EntityId = (int)personId;
                                    attributeExtraDate.Value = DateRange(f1StartDate, f1EndDate) + ",";
                                }
                            }
                        }
                        else if (f1AttributeName == "Dropped Volunteers")
                        {
                            if (f1Comment != null)
                            {

                                attributeValues.AttributeId = 1068;
                                attributeValues.EntityId = (int)personId;
                                attributeValues.Value = f1Comment;
                            }
                            if (f1StartDate != null)
                            {
                                if (f1EndDate != null)
                                {
                                    attributeExtraDate.AttributeId = 1071;
                                    attributeExtraDate.EntityId = (int)personId;
                                    attributeExtraDate.Value = DateRange(f1StartDate, f1EndDate);
                                }
                                else
                                {
                                    attributeExtraDate.AttributeId = 1071;
                                    attributeExtraDate.EntityId = (int)personId;
                                    attributeExtraDate.Value = DateRange(f1StartDate, f1EndDate) + ",";
                                }
                            }
                        }
                        //else if (f1AttributeName == "Temp Out Volunteers")
                        //{
                        //    if (f1Comment != null)
                        //    {

                        //        attributeValues.AttributeId = 1217;
                        //        attributeValues.EntityId = (int)personId;
                        //        attributeValues.Value = f1Comment;
                        //    }
                        //    if (f1StartDate != null)
                        //    {
                        //        if (f1EndDate != null)
                        //        {
                        //            attributeExtraDate.AttributeId = 1218;
                        //            attributeExtraDate.EntityId = (int)personId;
                        //            attributeExtraDate.Value = DateRange(f1StartDate, f1EndDate);
                        //        }
                        //        else
                        //        {
                        //            attributeExtraDate.AttributeId = 1218;
                        //            attributeExtraDate.EntityId = (int)personId;
                        //            attributeExtraDate.Value = DateRange(f1StartDate, f1EndDate) + ",";
                        //        }
                        //    }
                        //}
                    }
                    //else if (f1AttributeGroupName == "UpFront - Connection Central")
                    //{
                    //    if (f1Comment != null)
                    //    {

                    //        attributeValues.AttributeId = 1222;
                    //        attributeValues.EntityId = (int)personId;
                    //        attributeValues.Value = f1Comment;
                    //    }
                    //    if (f1StartDate != null)
                    //    {
                    //        if (f1EndDate != null)
                    //        {
                    //            attributeExtraDate.AttributeId = 1223;
                    //            attributeExtraDate.EntityId = (int)personId;
                    //            attributeExtraDate.Value = DateRange(f1StartDate, f1EndDate);
                    //        }
                    //        else
                    //        {
                    //            attributeExtraDate.AttributeId = 1223;
                    //            attributeExtraDate.EntityId = (int)personId;
                    //            attributeExtraDate.Value = DateRange(f1StartDate, f1EndDate) + ",";
                    //        }
                    //    }
                    //}
                    else if (f1AttributeGroupName == "Upward") //changed multi-select type to Defined Value Type of Years Multi-Select
                    {
                        if (f1AttributeName == "Basketball")
                        {
                            if (f1Comment != null)
                            {
                                attributeValues.AttributeId = 1227;
                                attributeValues.EntityId = (int)personId;
                                attributeValues.Value = f1Comment;
                            }
                            if (f1StartDate != null)
                            {
                                attributeExtraDate.AttributeId = 1228;
                                attributeExtraDate.EntityId = (int)personId;
                                attributeExtraDate.Value = MultiSelectYearGUID(f1StartDate) + ",";
                            }
                        }
                        else if (f1AttributeName == "Cheerleading")
                        {
                            if (f1Comment != null)
                            {
                                attributeValues.AttributeId = 1229;
                                attributeValues.EntityId = (int)personId;
                                attributeValues.Value = f1Comment;
                            }
                            if (f1StartDate != null)
                            {
                                attributeExtraDate.AttributeId = 1230;
                                attributeExtraDate.EntityId = (int)personId;
                                attributeExtraDate.Value = MultiSelectYearGUID(f1StartDate) + ",";
                            }
                        }
                        else if (f1AttributeName == "Coaches")
                        {
                            if (f1Comment != null)
                            {
                                attributeValues.AttributeId = 1231;
                                attributeValues.EntityId = (int)personId;
                                attributeValues.Value = f1Comment;
                            }
                            if (f1StartDate != null)
                            {
                                attributeExtraDate.AttributeId = 1233;
                                attributeExtraDate.EntityId = (int)personId;
                                attributeExtraDate.Value = MultiSelectYearGUID(f1StartDate) + ","; ;
                            }
                        }
                        else if (f1AttributeName == "Flag Football")
                        {
                            if (f1Comment != null)
                            {
                                attributeValues.AttributeId = 1234;
                                attributeValues.EntityId = (int)personId;
                                attributeValues.Value = f1Comment;
                            }
                            if (f1StartDate != null)
                            {
                                attributeExtraDate.AttributeId = 1235;
                                attributeExtraDate.EntityId = (int)personId;
                                attributeExtraDate.Value = MultiSelectYearGUID(f1StartDate) + ",";
                            }
                        }
                        else if (f1AttributeName == "Referees")
                        {
                            if (f1Comment != null)
                            {
                                attributeValues.AttributeId = 1237;
                                attributeValues.EntityId = (int)personId;
                                attributeValues.Value = f1Comment;
                            }
                            else if (f1StartDate != null)
                            {
                                attributeExtraDate.AttributeId = 1238;
                                attributeExtraDate.EntityId = (int)personId;
                                attributeExtraDate.Value = MultiSelectYearGUID(f1StartDate) + ",";
                            }
                        }
                        else if (f1AttributeName == "Soccer")
                        {
                            if (f1Comment != null)
                            {

                                attributeValues.AttributeId = 1239;
                                attributeValues.EntityId = (int)personId;
                                attributeValues.Value = f1Comment;
                            }
                            else if (f1StartDate != null)
                            {
                                attributeExtraDate.AttributeId = 1240;
                                attributeExtraDate.EntityId = (int)personId;
                                attributeExtraDate.Value = MultiSelectYearGUID(f1StartDate) + ",";
                            }
                        }
                    }
                    else if (f1AttributeGroupName == "We Believe")
                    {
                        if (f1AttributeName == "We Believe Commitment")
                        {
                            if (f1StartDate != null)
                            {
                                if (f1EndDate != null)
                                {
                                    attributeValues.AttributeId = 1245;
                                    attributeValues.EntityId = (int)personId;
                                    attributeValues.Value = DateRange(f1StartDate, f1EndDate);
                                }
                                else
                                {
                                    attributeValues.AttributeId = 1245;
                                    attributeValues.EntityId = (int)personId;
                                    attributeValues.Value = DateRange(f1StartDate, f1EndDate) + ",";
                                }
                            }
                        }
                    }
                    else if (f1AttributeGroupName == "Youth")
                    {
                        //if (f1AttributeName == "Administrative Volunteer")
                        //{
                        //    if (f1Comment != null)
                        //    {

                        //        attributeValues.AttributeId = 965;
                        //        attributeValues.EntityId = (int)personId;
                        //        attributeValues.Value = f1Comment;
                        //    }
                        //    else if (f1StartDate != null)
                        //    {
                        //        if (f1EndDate != null)
                        //        {
                        //            attributeExtraDate.AttributeId = 966;
                        //            attributeExtraDate.EntityId = (int)personId;
                        //            attributeExtraDate.Value = DateRange(f1StartDate, f1EndDate);
                        //        }
                        //        else
                        //        {
                        //            attributeExtraDate.AttributeId = 966;
                        //            attributeExtraDate.EntityId = (int)personId;
                        //            attributeExtraDate.Value = DateRange(f1StartDate, f1EndDate) + ",";
                        //        }
                        //    }
                        //}
                        //else if (f1AttributeName == "Cafe Worker")
                        //{
                        //    if (f1Comment != null)
                        //    {

                        //        attributeValues.AttributeId = 991;
                        //        attributeValues.EntityId = (int)personId;
                        //        attributeValues.Value = f1Comment;
                        //    }
                        //    else if (f1StartDate != null)
                        //    {
                        //        if (f1EndDate != null)
                        //        {
                        //            attributeExtraDate.AttributeId = 992;
                        //            attributeExtraDate.EntityId = (int)personId;
                        //            attributeExtraDate.Value = DateRange(f1StartDate, f1EndDate);
                        //        }
                        //        else
                        //        {
                        //            attributeExtraDate.AttributeId = 992;
                        //            attributeExtraDate.EntityId = (int)personId;
                        //            attributeExtraDate.Value = DateRange(f1StartDate, f1EndDate) + ",";
                        //        }
                        //    }
                        //}
                        //else if (f1AttributeName == "Game Monitor")
                        //{
                        //    if (f1Comment != null)
                        //    {

                        //        attributeValues.AttributeId = 1094;
                        //        attributeValues.EntityId = (int)personId;
                        //        attributeValues.Value = f1Comment;
                        //    }
                        //    else if (f1StartDate != null)
                        //    {
                        //        if (f1EndDate != null)
                        //        {
                        //            attributeExtraDate.AttributeId = 1095;
                        //            attributeExtraDate.EntityId = (int)personId;
                        //            attributeExtraDate.Value = DateRange(f1StartDate, f1EndDate);
                        //        }
                        //        else
                        //        {
                        //            attributeExtraDate.AttributeId = 1095;
                        //            attributeExtraDate.EntityId = (int)personId;
                        //            attributeExtraDate.Value = DateRange(f1StartDate, f1EndDate) + ",";
                        //        }
                        //    }
                        //}
                        //else if (f1AttributeName == "Jr. High Sunday Morning")
                        //{
                        //    if (f1Comment != null)
                        //    {

                        //        attributeValues.AttributeId = 1106;
                        //        attributeValues.EntityId = (int)personId;
                        //        attributeValues.Value = f1Comment;
                        //    }
                        //    else if (f1StartDate != null)
                        //    {
                        //        if (f1EndDate != null)
                        //        {
                        //            attributeExtraDate.AttributeId = 1107;
                        //            attributeExtraDate.EntityId = (int)personId;
                        //            attributeExtraDate.Value = DateRange(f1StartDate, f1EndDate);
                        //        }
                        //        else
                        //        {
                        //            attributeExtraDate.AttributeId = 1107;
                        //            attributeExtraDate.EntityId = (int)personId;
                        //            attributeExtraDate.Value = DateRange(f1StartDate, f1EndDate) + ",";
                        //        }
                        //    }
                        //}
                        //else if (f1AttributeName == "Media")
                        //{
                        //    if (f1Comment != null)
                        //    {

                        //        attributeValues.AttributeId = 1127;
                        //        attributeValues.EntityId = (int)personId;
                        //        attributeValues.Value = f1Comment;
                        //    }
                        //    else if (f1StartDate != null)
                        //    {
                        //        if (f1EndDate != null)
                        //        {
                        //            attributeExtraDate.AttributeId = 1126;
                        //            attributeExtraDate.EntityId = (int)personId;
                        //            attributeExtraDate.Value = DateRange(f1StartDate, f1EndDate);
                        //        }
                        //        else
                        //        {
                        //            attributeExtraDate.AttributeId = 1126;
                        //            attributeExtraDate.EntityId = (int)personId;
                        //            attributeExtraDate.Value = DateRange(f1StartDate, f1EndDate) + ",";
                        //        }
                        //    }
                        //}
                        //else if (f1AttributeName == "Praise Team")
                        //{
                        //    if (f1Comment != null)
                        //    {

                        //        attributeValues.AttributeId = 1149;
                        //        attributeValues.EntityId = (int)personId;
                        //        attributeValues.Value = f1Comment;
                        //    }
                        //    else if (f1StartDate != null)
                        //    {
                        //        if (f1EndDate != null)
                        //        {
                        //            attributeExtraDate.AttributeId = 1150;
                        //            attributeExtraDate.EntityId = (int)personId;
                        //            attributeExtraDate.Value = DateRange(f1StartDate, f1EndDate);
                        //        }
                        //        else
                        //        {
                        //            attributeExtraDate.AttributeId = 1150;
                        //            attributeExtraDate.EntityId = (int)personId;
                        //            attributeExtraDate.Value = DateRange(f1StartDate, f1EndDate) + ",";
                        //        }
                        //    }
                        //}
                        //else if (f1AttributeName == "Purity Commitment")
                        //{
                        //    if (f1Comment != null)
                        //    {
                        //        //Checks if current person already has this Rock Attribute and will append the current F1 attribute value to the Rock one.
                        //        if (newAttributes.Find(a => (a.AttributeId == 1164 && a.EntityId == personId)) != null)
                        //        {
                        //            ReportProgress(0, string.Format("Adding Purity Commitment"));
                        //            newAttributes.Find(a => a.AttributeId == 1164 && a.EntityId == personId).Value += DateRange(f1StartDate, f1EndDate) + " - " + f1Comment;
                        //        }
                        //        else
                        //        {
                        //            attributeValues.AttributeId = 1164;
                        //            attributeValues.EntityId = (int)personId;
                        //            attributeValues.Value = f1Comment;
                        //        }
                        //    }
                        //    else if (f1StartDate != null)
                        //    {
                        //        //Checks if current person already has this Rock Attribute and will append the current F1 attribute value to the Rock one.
                        //        if (newAttributes.Find(a => (a.AttributeId == 1164 && a.EntityId == personId)) != null)
                        //        {
                        //            ReportProgress(0, string.Format("Adding Purity Commitment Date"));
                        //        }
                        //        else
                        //        {

                        //            if (f1EndDate != null)
                        //            {
                        //                attributeExtraDate.AttributeId = 1165;
                        //                attributeExtraDate.EntityId = (int)personId;
                        //                attributeExtraDate.Value = DateRange(f1StartDate, f1EndDate);
                        //            }
                        //            else
                        //            {
                        //                attributeExtraDate.AttributeId = 1165;
                        //                attributeExtraDate.EntityId = (int)personId;
                        //                attributeExtraDate.Value = DateRange(f1StartDate, f1EndDate) + ",";
                        //            }
                        //        }
                        //    }
                        //}
                        //else if (f1AttributeName == "Security")
                        //{
                        //    if (f1Comment != null)
                        //    {

                        //        attributeValues.AttributeId = 1178;
                        //        attributeValues.EntityId = (int)personId;
                        //        attributeValues.Value = f1Comment;
                        //    }
                        //    else if (f1StartDate != null)
                        //    {
                        //        if (f1EndDate != null)
                        //        {
                        //            attributeExtraDate.AttributeId = 1179;
                        //            attributeExtraDate.EntityId = (int)personId;
                        //            attributeExtraDate.Value = DateRange(f1StartDate, f1EndDate);
                        //        }
                        //        else
                        //        {
                        //            attributeExtraDate.AttributeId = 1179;
                        //            attributeExtraDate.EntityId = (int)personId;
                        //            attributeExtraDate.Value = DateRange(f1StartDate, f1EndDate) + ",";
                        //        }
                        //    }
                        //}
                        //else if (f1AttributeName == "Special Events")
                        //{
                        //    if (f1Comment != null)
                        //    {

                        //        attributeValues.AttributeId = 1200;
                        //        attributeValues.EntityId = (int)personId;
                        //        attributeValues.Value = f1Comment;
                        //    }
                        //    else if (f1StartDate != null)
                        //    {
                        //        if (f1EndDate != null)
                        //        {
                        //            attributeExtraDate.AttributeId = 1201;
                        //            attributeExtraDate.EntityId = (int)personId;
                        //            attributeExtraDate.Value = DateRange(f1StartDate, f1EndDate);
                        //        }
                        //        else
                        //        {
                        //            attributeExtraDate.AttributeId = 1201;
                        //            attributeExtraDate.EntityId = (int)personId;
                        //            attributeExtraDate.Value = DateRange(f1StartDate, f1EndDate) + ",";
                        //        }
                        //    }
                        //}
                        //else if (f1AttributeName == "Sr. High Sunday Morning Date")
                        //{
                        //    if (f1Comment != null)
                        //    {

                        //        attributeValues.AttributeId = 1209;
                        //        attributeValues.EntityId = (int)personId;
                        //        attributeValues.Value = f1Comment;
                        //    }
                        //    else if (f1StartDate != null)
                        //    {
                        //        if (f1EndDate != null)
                        //        {
                        //            attributeExtraDate.AttributeId = 1210;
                        //            attributeExtraDate.EntityId = (int)personId;
                        //            attributeExtraDate.Value = DateRange(f1StartDate, f1EndDate);
                        //        }
                        //        else
                        //        {
                        //            attributeExtraDate.AttributeId = 1210;
                        //            attributeExtraDate.EntityId = (int)personId;
                        //            attributeExtraDate.Value = DateRange(f1StartDate, f1EndDate) + ",";
                        //        }
                        //    }
                        //}
                        //else if (f1AttributeName == "Usher")
                        //{
                        //    if (f1Comment != null)
                        //    {

                        //        attributeValues.AttributeId = 1241;
                        //        attributeValues.EntityId = (int)personId;
                        //        attributeValues.Value = f1Comment;
                        //    }
                        //    else if (f1StartDate != null)
                        //    {
                        //        if (f1EndDate != null)
                        //        {
                        //            attributeExtraDate.AttributeId = 1242;
                        //            attributeExtraDate.EntityId = (int)personId;
                        //            attributeExtraDate.Value = DateRange(f1StartDate, f1EndDate);
                        //        }
                        //        else
                        //        {
                        //            attributeExtraDate.AttributeId = 1242;
                        //            attributeExtraDate.EntityId = (int)personId;
                        //            attributeExtraDate.Value = DateRange(f1StartDate, f1EndDate) + ",";
                        //        }
                        //    }
                        //}
                        //else if (f1AttributeName == "Wednesday Night Activities")
                        //{
                        //    if (f1Comment != null)
                        //    {

                        //        attributeValues.AttributeId = 1246;
                        //        attributeValues.EntityId = (int)personId;
                        //        attributeValues.Value = f1Comment;
                        //    }
                        //    else if (f1StartDate != null)
                        //    {
                        //        if (f1EndDate != null)
                        //        {
                        //            attributeExtraDate.AttributeId = 1247;
                        //            attributeExtraDate.EntityId = (int)personId;
                        //            attributeExtraDate.Value = DateRange(f1StartDate, f1EndDate);
                        //        }
                        //        else
                        //        {
                        //            attributeExtraDate.AttributeId = 1247;
                        //            attributeExtraDate.EntityId = (int)personId;
                        //            attributeExtraDate.Value = DateRange(f1StartDate, f1EndDate) + ",";
                        //        }
                        //    }
                        //}
                        if (f1AttributeName == "Youth Coach")
                        {
                            if (f1Comment != null)
                            {

                                attributeValues.AttributeId = 1258;
                                attributeValues.EntityId = (int)personId;
                                attributeValues.Value = f1Comment;
                            }
                            else if (f1StartDate != null)
                            {
                                if (f1EndDate != null)
                                {
                                    attributeExtraDate.AttributeId = 1259;
                                    attributeExtraDate.EntityId = (int)personId;
                                    attributeExtraDate.Value = DateRange(f1StartDate, f1EndDate);
                                }
                                else
                                {
                                    attributeExtraDate.AttributeId = 1259;
                                    attributeExtraDate.EntityId = (int)personId;
                                    attributeExtraDate.Value = DateRange(f1StartDate, f1EndDate) + ",";
                                }
                            }
                        }
                    }
                    else if (f1AttributeGroupName == "Youth Camp Decisions")
                    {
                        if (f1AttributeName == "Baptism")
                        {
                            //Checks if current person already has this Rock Attribute and will append the current F1 attribute value to the Rock one.
                            if (newAttributes.Find(a => (a.AttributeId == 986 && a.EntityId == personId)) != null)
                            {
                                ReportProgress(0, string.Format("Adding Baptism Decision"));
                                newAttributes.Find(a => a.AttributeId == 986 && a.EntityId == personId).Value += DateRange(f1StartDate, f1EndDate) + " - " + f1Comment;
                            }
                            else
                            {

                                if (f1Comment != null)
                                {
                                    attributeValues.AttributeId = 986;
                                    attributeValues.EntityId = (int)personId;
                                    attributeValues.Value = f1Comment;
                                }
                                else if (f1StartDate != null)
                                {
                                    attributeExtraDate.AttributeId = 987;
                                    attributeExtraDate.EntityId = (int)personId;
                                    attributeExtraDate.Value = DateRange(f1StartDate, f1EndDate) + ",";
                                }
                            }
                        }
                        else if (f1AttributeName == "Full Time Ministry")
                        {
                            if (f1Comment != null)
                            {
                                attributeValues.AttributeId = 1092;
                                attributeValues.EntityId = (int)personId;
                                attributeValues.Value = f1Comment;
                            }
                            else if (f1StartDate != null)
                            {
                                attributeExtraDate.AttributeId = 1093;
                                attributeExtraDate.EntityId = (int)personId;
                                attributeExtraDate.Value = DateRange(f1StartDate, f1EndDate) + ",";
                            }
                        }
                        else if (f1AttributeName == "Prayer")
                        {
                            //Checks if current person already has this Rock Attribute and will append the current F1 attribute value to the Rock one.
                            if (newAttributes.Find(a => (a.AttributeId == 1153 && a.EntityId == personId)) != null)
                            {
                                ReportProgress(0, string.Format("Adding Prayer Decision"));
                                newAttributes.Find(a => a.AttributeId == 1153 && a.EntityId == personId).Value += DateRange(f1StartDate, f1EndDate) + " - " + f1Comment;
                            }
                            else
                            {
                                if (f1Comment != null)
                                {
                                    attributeValues.AttributeId = 1153;
                                    attributeValues.EntityId = (int)personId;
                                    attributeValues.Value = f1Comment;
                                }
                                else if (f1StartDate != null)
                                {
                                    attributeExtraDate.AttributeId = 1154;
                                    attributeExtraDate.EntityId = (int)personId;
                                    attributeExtraDate.Value = DateRange(f1StartDate, f1EndDate) + ",";
                                }
                            }
                        }
                        else if (f1AttributeName == "Rededication")
                        {
                            //Checks if current person already has this Rock Attribute and will append the current F1 attribute value to the Rock one.
                            if (newAttributes.Find(a => (a.AttributeId == 1166 && a.EntityId == personId)) != null)
                            {
                                ReportProgress(0, string.Format("Adding Rededication Decision"));
                                newAttributes.Find(a => a.AttributeId == 1166 && a.EntityId == personId).Value += DateRange(f1StartDate, f1EndDate) + " - " + f1Comment;
                            }
                            else
                            {
                                if (f1Comment != null)
                                {
                                    attributeValues.AttributeId = 1166;
                                    attributeValues.EntityId = (int)personId;
                                    attributeValues.Value = f1Comment;
                                }
                                else if (f1StartDate != null)
                                {
                                    attributeExtraDate.AttributeId = 1167;
                                    attributeExtraDate.EntityId = (int)personId;
                                    attributeExtraDate.Value = DateRange(f1StartDate, f1EndDate) + ",";
                                }
                            }
                        }
                        else if (f1AttributeName == "Salvation")
                        {
                            if (f1Comment != null)
                            {
                                attributeValues.AttributeId = 1173;
                                attributeValues.EntityId = (int)personId;
                                attributeValues.Value = f1Comment;
                            }
                            else if (f1StartDate != null)
                            {
                                attributeExtraDate.AttributeId = 1174;
                                attributeExtraDate.EntityId = (int)personId;
                                attributeExtraDate.Value = DateRange(f1StartDate, f1EndDate) + ",";
                            }
                        }
                    }
                    else if (f1AttributeGroupName == "Youth Summer Camp")
                    {
                        //attributeValues.AttributeId = 1266;
                        //attributeValues.EntityId = (int)personId;

                        DateTime startDate = (DateTime)f1StartDate;

                        if (f1StartDate != null)
                        {
                            switch (startDate.Year)
                            {
                                case 2001:
                                    //Checks if current person already has this Rock Attribute and will append the current F1 attribute value to the Rock one.
                                    if (newAttributes.Find(a => (a.AttributeId == 1266 && a.EntityId == personId)) != null)
                                    {
                                        ReportProgress(0, string.Format("Adding Youth Summer Camp 2001"));
                                        newAttributes.Find(a => a.AttributeId == 1266 && a.EntityId == personId).Value += "B9A40993-7758-49A3-BE6B-00E930FCF690,";
                                    }
                                    else
                                    {
                                        attributeValues.AttributeId = 1266;
                                        attributeValues.EntityId = (int)personId;
                                        attributeValues.Value = "B9A40993-7758-49A3-BE6B-00E930FCF690,";
                                    }
                                    break;
                                case 2002:
                                    //Checks if current person already has this Rock Attribute and will append the current F1 attribute value to the Rock one.
                                    if (newAttributes.Find(a => (a.AttributeId == 1266 && a.EntityId == personId)) != null)
                                    {
                                        ReportProgress(0, string.Format("Adding Youth Summer Camp 2002"));
                                        newAttributes.Find(a => a.AttributeId == 1266 && a.EntityId == personId).Value += "56BF96EF-561E-424D-BA85-A93674569B47,";
                                    }
                                    else
                                    {
                                        attributeValues.AttributeId = 1266;
                                        attributeValues.EntityId = (int)personId;
                                        attributeValues.Value = "56BF96EF-561E-424D-BA85-A93674569B47,";
                                    }
                                    break;
                                case 2003:
                                    //Checks if current person already has this Rock Attribute and will append the current F1 attribute value to the Rock one.
                                    if (newAttributes.Find(a => (a.AttributeId == 1266 && a.EntityId == personId)) != null)
                                    {
                                        ReportProgress(0, string.Format("Adding Youth Summer Camp 2003"));
                                        newAttributes.Find(a => a.AttributeId == 1266 && a.EntityId == personId).Value += "74EB6703-DEB4-4CEA-81E2-5EC7ED81BB18,";
                                    }
                                    else
                                    {
                                        attributeValues.AttributeId = 1266;
                                        attributeValues.EntityId = (int)personId;
                                        attributeValues.Value = "74EB6703-DEB4-4CEA-81E2-5EC7ED81BB18,";
                                    }
                                    break;
                                case 2004:
                                    //Checks if current person already has this Rock Attribute and will append the current F1 attribute value to the Rock one.
                                    if (newAttributes.Find(a => (a.AttributeId == 1266 && a.EntityId == personId)) != null)
                                    {
                                        ReportProgress(0, string.Format("Adding Youth Summer Camp 2004"));
                                        newAttributes.Find(a => a.AttributeId == 1266 && a.EntityId == personId).Value += "DD28ACBD-8B2C-49CC-81C9-B7FFE4D8E3C2,";
                                    }
                                    else
                                    {
                                        attributeValues.AttributeId = 1266;
                                        attributeValues.EntityId = (int)personId;
                                        attributeValues.Value = "DD28ACBD-8B2C-49CC-81C9-B7FFE4D8E3C2,";
                                    }
                                    break;
                                case 2005:
                                    //Checks if current person already has this Rock Attribute and will append the current F1 attribute value to the Rock one.
                                    if (newAttributes.Find(a => (a.AttributeId == 1266 && a.EntityId == personId)) != null)
                                    {
                                        ReportProgress(0, string.Format("Adding Youth Summer Camp 2005"));
                                        newAttributes.Find(a => a.AttributeId == 1266 && a.EntityId == personId).Value += "F18A88B7-5228-4B7D-8079-4B118DF792C7,";
                                    }
                                    else
                                    {
                                        attributeValues.AttributeId = 1266;
                                        attributeValues.EntityId = (int)personId;
                                        attributeValues.Value = "F18A88B7-5228-4B7D-8079-4B118DF792C7,";
                                    }
                                    break;
                                case 2006:
                                    //Checks if current person already has this Rock Attribute and will append the current F1 attribute value to the Rock one.
                                    if (newAttributes.Find(a => (a.AttributeId == 1266 && a.EntityId == personId)) != null)
                                    {
                                        ReportProgress(0, string.Format("Adding Youth Summer Camp 2006"));
                                        newAttributes.Find(a => a.AttributeId == 1266 && a.EntityId == personId).Value += "719DF19D-B5AF-4125-B708-BDC22EB64E8F,";
                                    }
                                    else
                                    {
                                        attributeValues.AttributeId = 1266;
                                        attributeValues.EntityId = (int)personId;
                                        attributeValues.Value = "719DF19D-B5AF-4125-B708-BDC22EB64E8F,";
                                    }
                                    break;
                                case 2007:
                                    //Checks if current person already has this Rock Attribute and will append the current F1 attribute value to the Rock one.
                                    if (newAttributes.Find(a => (a.AttributeId == 1266 && a.EntityId == personId)) != null)
                                    {
                                        ReportProgress(0, string.Format("Adding Youth Summer Camp 2007"));
                                        newAttributes.Find(a => a.AttributeId == 1266 && a.EntityId == personId).Value += "CE44EA17-020E-4B97-8975-4DE01830163D,";
                                    }
                                    else
                                    {
                                        attributeValues.AttributeId = 1266;
                                        attributeValues.EntityId = (int)personId;
                                        attributeValues.Value = "CE44EA17-020E-4B97-8975-4DE01830163D,";
                                    }
                                    break;
                                case 2008:
                                    //Checks if current person already has this Rock Attribute and will append the current F1 attribute value to the Rock one.
                                    if (newAttributes.Find(a => (a.AttributeId == 1266 && a.EntityId == personId)) != null)
                                    {
                                        ReportProgress(0, string.Format("Adding Youth Summer Camp 2008"));
                                        newAttributes.Find(a => a.AttributeId == 1266 && a.EntityId == personId).Value += "6810C1C9-85BD-42E9-9E04-85801A93096D,";
                                    }
                                    else
                                    {
                                        attributeValues.AttributeId = 1266;
                                        attributeValues.EntityId = (int)personId;
                                        attributeValues.Value = "6810C1C9-85BD-42E9-9E04-85801A93096D,";
                                    }
                                    break;
                                case 2009:
                                    //Checks if current person already has this Rock Attribute and will append the current F1 attribute value to the Rock one.
                                    if (newAttributes.Find(a => (a.AttributeId == 1266 && a.EntityId == personId)) != null)
                                    {
                                        ReportProgress(0, string.Format("Adding Youth Summer Camp 2009"));
                                        newAttributes.Find(a => a.AttributeId == 1266 && a.EntityId == personId).Value += "2C8B55AF-B5E2-41F9-9E08-C2E6F4624550,";
                                    }
                                    else
                                    {
                                        attributeValues.AttributeId = 1266;
                                        attributeValues.EntityId = (int)personId;
                                        attributeValues.Value = "2C8B55AF-B5E2-41F9-9E08-C2E6F4624550,";
                                    }
                                    break;
                                case 2010:
                                    //Checks if current person already has this Rock Attribute and will append the current F1 attribute value to the Rock one.
                                    if (newAttributes.Find(a => (a.AttributeId == 1266 && a.EntityId == personId)) != null)
                                    {
                                        ReportProgress(0, string.Format("Adding Youth Summer Camp 2010"));
                                        newAttributes.Find(a => a.AttributeId == 1266 && a.EntityId == personId).Value += "FB260D37-AEF4-4277-959C-5884E579E1AC,";
                                    }
                                    else
                                    {
                                        attributeValues.AttributeId = 1266;
                                        attributeValues.EntityId = (int)personId;
                                        attributeValues.Value = "FB260D37-AEF4-4277-959C-5884E579E1AC,";
                                    }
                                    break;
                                case 2011:
                                    //Checks if current person already has this Rock Attribute and will append the current F1 attribute value to the Rock one.
                                    if (newAttributes.Find(a => (a.AttributeId == 1266 && a.EntityId == personId)) != null)
                                    {
                                        ReportProgress(0, string.Format("Adding Youth Summer Camp 2011"));
                                        newAttributes.Find(a => a.AttributeId == 1266 && a.EntityId == personId).Value += "6E84915B-CC11-4E66-954E-9B1D786B2E6F,";
                                    }
                                    else
                                    {
                                        attributeValues.AttributeId = 1266;
                                        attributeValues.EntityId = (int)personId;
                                        attributeValues.Value = "6E84915B-CC11-4E66-954E-9B1D786B2E6F,";
                                    }
                                    break;
                                case 2012:
                                    //Checks if current person already has this Rock Attribute and will append the current F1 attribute value to the Rock one.
                                    if (newAttributes.Find(a => (a.AttributeId == 1266 && a.EntityId == personId)) != null)
                                    {
                                        ReportProgress(0, string.Format("Adding Youth Summer Camp 2012"));
                                        newAttributes.Find(a => a.AttributeId == 1266 && a.EntityId == personId).Value += "4ED12DFD-BA8F-4760-A045-E7AC898BEC50,";
                                    }
                                    else
                                    {
                                        attributeValues.AttributeId = 1266;
                                        attributeValues.EntityId = (int)personId;
                                        attributeValues.Value = "4ED12DFD-BA8F-4760-A045-E7AC898BEC50,";
                                    }
                                    break;
                                case 2013:
                                    //Checks if current person already has this Rock Attribute and will append the current F1 attribute value to the Rock one.
                                    if (newAttributes.Find(a => (a.AttributeId == 1266 && a.EntityId == personId)) != null)
                                    {
                                        ReportProgress(0, string.Format("Adding Youth Summer Camp 2013"));
                                        newAttributes.Find(a => a.AttributeId == 1266 && a.EntityId == personId).Value += "AFEC8401-3E49-4895-B320-6FF4918A5F4D,";
                                    }
                                    else
                                    {
                                        attributeValues.AttributeId = 1266;
                                        attributeValues.EntityId = (int)personId;
                                        attributeValues.Value = "AFEC8401-3E49-4895-B320-6FF4918A5F4D,";
                                    }
                                    break;
                                default:
                                    break;
                            }
                        }
                        //switch (startDate.Year)
                        //{
                        //    case 2003:
                        //        attributeValues.Value = "B9A51D0B-689A-4916-AF5B-1AE388029ADB" + ",";
                        //        break;
                        //    case 2004:
                        //        attributeValues.Value = "DD28ACBD-8B2C-49CC-81C9-B7FFE4D8E3C2" + ",";
                        //        break;
                        //    case 2005:
                        //        attributeValues.Value = "F18A88B7-5228-4B7D-8079-4B118DF792C7" + ",";
                        //        break;
                        //    case 2006:
                        //        attributeValues.Value = "719DF19D-B5AF-4125-B708-BDC22EB64E8F" + ",";
                        //        break;
                        //    case 2007:
                        //        attributeValues.Value = "CE44EA17-020E-4B97-8975-4DE01830163D" + ",";
                        //        break;
                        //    case 2008:
                        //        attributeValues.Value = "6810C1C9-85BD-42E9-9E04-85801A93096D" + ",";
                        //        break;
                        //    case 2009:
                        //        attributeValues.Value = "2C8B55AF-B5E2-41F9-9E08-C2E6F4624550" + ",";
                        //        break;
                        //    case 2010:
                        //        attributeValues.Value = "FB260D37-AEF4-4277-959C-5884E579E1AC" + ",";
                        //        break;
                        //    case 2011:
                        //        attributeValues.Value = "6E84915B-CC11-4E66-954E-9B1D786B2E6F" + ",";
                        //        break;
                        //    case 2012:
                        //        attributeValues.Value = "4ED12DFD-BA8F-4760-A045-E7AC898BEC50" + ",";
                        //        break;
                        //    case 2013:
                        //        attributeValues.Value = "AFEC8401-3E49-4895-B320-6FF4918A5F4D" + ",";
                        //        break;
                        //    default:
                        //        break;
                        //}
                    }

                    //attributeValues.CreatedByPersonAliasId = ImportPersonAlias.Id;
                    //attributeValues.AccountNumberSecured = encodedNumber;
                    //attributeValues.(int)personId = (int)personId;

                    // Other Attributes (not used):
                    // Account_Type_Name


                    if (!attributeValues.AttributeId.Equals(0))
                    {
                        newAttributes.Add(attributeValues);
                    }
                    if (!attributeExtraDate.AttributeId.Equals(0))
                    {
                        newAttributes.Add(attributeExtraDate);
                    }
                    if (!attributeExtraComment.AttributeId.Equals(0))
                    {
                        newAttributes.Add(attributeExtraComment);
                    }

                    completed++;
                    if (completed == 1006)
                    {
                        int percentComplete = completed / percentage;
                        ReportProgress(percentComplete, string.Format("{0:N0} attributes imported ({1}% complete).", completed, percentComplete));

                        var rockContext = new RockContext();
                        rockContext.WrapTransaction(() =>
                        {
                            rockContext.Configuration.AutoDetectChangesEnabled = false;
                            rockContext.AttributeValues.AddRange(newAttributes);
                            rockContext.SaveChanges(DisableAudit);
                        });

                        newAttributes.Clear();
                    }
                    else if (completed % ReportingNumber < 1)
                    {
                        //RockTransactionScope.WrapTransaction(() =>
                        //{
                        //    var rockContext = new RockContext();
                        //    rockContext.Configuration.AutoDetectChangesEnabled = false;
                        //    //rockContext.FinancialPersonBankAccounts.AddRange(newAttributes);
                        //    rockContext.AttributeValues.AddRange(newAttributes);
                        //    rockContext.SaveChanges(DisableAudit);
                        //});

                        //newAttributes.Clear();
                        ReportPartialProgress();
                    }
                    //}
                }
                //}
            }
            if (newAttributes.Any())
            {
                var rockContext = new RockContext();
                rockContext.WrapTransaction(() =>
                {
                    rockContext.Configuration.AutoDetectChangesEnabled = false;
                    rockContext.AttributeValues.AddRange(newAttributes);
                    rockContext.SaveChanges(DisableAudit);
                });
            }

            ReportProgress(100, string.Format("Finished attribute import: {0:N0} attributes imported.", completed));
        }
    }
}