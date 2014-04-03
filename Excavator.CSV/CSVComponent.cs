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
using System.Linq;
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

        // Disable compiler warning: value never assigned
#pragma warning disable 0649

        /// <summary>
        /// The person assigned to do the import
        /// </summary>
        private PersonAlias ImportPersonAlias;

#pragma warning restore

        #endregion

        #region Methods

        /// <summary>
        /// Transforms the data from the dataset.
        /// </summary>
        public override int TransformData( string importUser = null )
        {
            // Report progress to the main thread so it can update the UI
            ReportProgress( 0, "Starting import..." );

            // Connects to the source file (already loaded in memory by the UI)

            // Hold a count of how many records have been imported
            int completed = 0;

            // Pick a method to save data to Rock: #1 (simple) or #2 (fast)

            // Option #1. Standard way to put data in Rock
            //foreach ( var dataRow in tableData )
            //{
            // Create a Rock model and assign data to it
            Person person = new Person();

            RockTransactionScope.WrapTransaction( () =>
            {
                // Instantiate the object model service
                var personService = new PersonService();

                // If it's a new model, add it to the database first
                personService.Add( person, ImportPersonAlias );

                // Save the data to the database
                personService.Save( person, ImportPersonAlias );
            } );

            completed++;
            //}

            // end option #1

            // Report the final imported count
            ReportProgress( 100, string.Format( "Completed import: {0:N0} records imported.", completed ) );
            return completed;
        }

        #endregion
    }
}
