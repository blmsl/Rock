
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Rock.CodeGeneration project
//     Changes to this file will be lost when the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//
using System.ComponentModel;
using System.ComponentModel.Composition;

namespace Rock.DataFilters.FinancialBatch
{
    /// <summary>
    /// Other FinancialBatch Data View Filter
    /// </summary>
    [Description( "Filter Financial Batches using other data view" )]
    [Export( typeof( DataFilterComponent ) )]
    [ExportMetadata( "ComponentName", "Other FinancialBatch Data View Filter" )]
    public partial class FinancialBatchDataViewFilter : OtherDataViewFilter<Rock.Model.FinancialBatch>
    {

        /// <summary>
        /// Gets the name of the filtered entity type.
        /// </summary>
        /// <value>
        /// The name of the filtered entity type.
        /// </value>
        public override string FilteredEntityTypeName
        {
            get { return "Rock.Model.FinancialBatch"; }
        }

    }
}
