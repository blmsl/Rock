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
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;

using Rock.Attribute;
using Rock.Data;
using Rock.Model;
using Rock.Security;
using Rock.Web.Cache;

namespace Rock.Workflow.Action
{
    /// <summary>
    /// Sends a Background Check Request.
    /// </summary>
    [Description( "Sends a Background Check Request." )]
    [Export( typeof( ActionComponent ) )]
    [ExportMetadata( "ComponentName", "Create Connection Reques" )]

    [WorkflowAttribute( "Person Attribute", "The Person attribute that contains the person that connection request should be created for.", true, "", "", 0, null,
        new string[] { "Rock.Field.Types.PersonFieldType" } )]
    [WorkflowAttribute( "Connection Opportunity Attribute", "The attribute that contains the type of connection opportunity to create.", true, "", "", 1, null,
        new string[] { "Rock.Field.Types.ConnectionOpportunityFieldType" } )]
    [ConnectionStatusField( "Connection Status", "The status of the request to create. If not specified (or if status selected is for the wrong connextion type) the connection type's default status will be used.", false, "", "", 2 )]
    [WorkflowAttribute( "Campus Attribute", "An optional attribute that contains the campus to use for the request.", false, "", "", 2, null,
        new string[] { "Rock.Field.Types.CampusFieldType" } )]
    [WorkflowAttribute( "Connection Request Attribute", "An optional connection request attribute to store the request that is created.", true, "", "", 4, null,
        new string[] { "Rock.Field.Types.ConnectionRequestFieldType" } )]

    public class CreateConnectionRequest : ActionComponent
    {
        /// <summary>
        /// Executes the specified workflow.
        /// </summary>
        /// <param name="rockContext">The rock context.</param>
        /// <param name="action">The action.</param>
        /// <param name="entity">The entity.</param>
        /// <param name="errorMessages">The error messages.</param>
        /// <returns></returns>
        public override bool Execute( RockContext rockContext, WorkflowAction action, Object entity, out List<string> errorMessages )
        {
            errorMessages = new List<string>();

            PersonAlias personAlias = null;
            ConnectionOpportunity opportunity = null;
            ConnectionStatus status = null;
            int? campusId = null;

            // Get the person
            Guid personAliasGuid = action.GetWorklowAttributeValue(GetAttributeValue( action, "PersonAttribute" ).AsGuid()).AsGuid();
            personAlias = new PersonAliasService( rockContext ).Get( personAliasGuid );
            if ( personAlias == null )
            {
                errorMessages.Add( "Invalid Person Attribute or Value!" );
                return false;
            }

            // Get the opportunity
            Guid opportunityTypeGuid = action.GetWorklowAttributeValue( GetAttributeValue( action, "ConnectionOpportunityAttribute" ).AsGuid() ).AsGuid();
            opportunity = new ConnectionOpportunityService( rockContext ).Get( opportunityTypeGuid );
            if ( opportunity == null )
            {
                errorMessages.Add( "Invalid Connection Opportunity Attribute or Value!" );
                return false;
            }

            // Get connection status
            Guid? connectionStatusGuid = GetAttributeValue( action, "ConnectionStatus" ).AsGuidOrNull();
            if ( connectionStatusGuid.HasValue )
            {
                status = opportunity.ConnectionType.ConnectionStatuses
                    .Where( s => s.Guid.Equals( connectionStatusGuid.Value ))
                    .FirstOrDefault();
            }
            if ( status == null )
            {
                status = opportunity.ConnectionType.ConnectionStatuses
                    .Where( s => s.IsDefault )
                    .FirstOrDefault();
            }

            // Get Campus
            Guid? campusAttributeGuid = GetAttributeValue( action, "CampusAttribute" ).AsGuidOrNull();
            if ( campusAttributeGuid.HasValue )
            {
                Guid? campusGuid = action.GetWorklowAttributeValue( campusAttributeGuid.Value ).AsGuidOrNull();
                if ( campusGuid.HasValue )
                {
                    var campus = CampusCache.Read( campusGuid.Value );
                    if ( campus != null )
                    {
                        campusId = campus.Id;
                    }
                }
            }

            var connectionRequest = new ConnectionRequest();
            connectionRequest.PersonAliasId = personAlias.Id;
            connectionRequest.ConnectionOpportunityId = opportunity.Id;
            connectionRequest.ConnectionState = ConnectionState.Active;
            connectionRequest.ConnectionStatusId = status.Id;
            connectionRequest.CampusId = campusId;
            new ConnectionRequestService( rockContext ).Add( connectionRequest );
            rockContext.SaveChanges();

            return true;
        }
    }
}