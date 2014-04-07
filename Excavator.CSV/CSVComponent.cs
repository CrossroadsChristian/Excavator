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
        public CachedCsvReader Database;

        /// <summary>
        /// The person assigned to do the import
        /// </summary>
        private PersonAlias ImportPersonAlias;

        #endregion

        #region Methods

        /// <summary>
        /// Loads the database for this instance.
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public override bool LoadSchema( string fileName )
        {
            Database = new CachedCsvReader( new StringReader( fileName ), true );

            return true;
        }

        /// <summary>
        /// Transforms the data from the dataset.
        /// </summary>
        public override int TransformData( string importUser = null )
        {
            ReportProgress( 0, "Starting import..." );

            int completed = 0;
            var personService = new PersonService();
            var importPerson = personService.GetByFullName( importUser, includeDeceased: false, allowFirstNameOnly: true ).FirstOrDefault();
            if ( importPerson == null )
            {
                importPerson = personService.Queryable().FirstOrDefault();
            }

            ImportPersonAlias = new PersonAliasService().Get( importPerson.Id );

            Person person = new Person();
            person.CreatedByPersonAliasId = ImportPersonAlias.Id;

            RockTransactionScope.WrapTransaction( () =>
            {
                personService = new PersonService();
                personService.Add( person, ImportPersonAlias );
                personService.Save( person, ImportPersonAlias );
            } );

            completed++;

            ReportProgress( 100, string.Format( "Completed import: {0:N0} records imported.", completed ) );
            return completed;
        }

        #endregion
    }
}
