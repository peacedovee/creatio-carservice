using System;
using Terrasoft.Core;
using Terrasoft.Core.Entities;
using Terrasoft.Core.Entities.Events;
using Terrasoft.Common;
using Terrasoft.Core.Configuration;

namespace Terrasoft.Configuration
{
    [EntityEventListener(SchemaName = "UsrOrderWork")]
    public class UsrOrderWorkEventListener : BaseEntityEventListener
    {
        public override void OnSaving(object sender, EntityBeforeEventArgs e)
        {
            var entity = (Entity)sender;
            var userConnection = entity.UserConnection;

            var orderId = entity.GetTypedColumnValue<Guid>("UsrOrderId");
            var currentHours = entity.GetTypedColumnValue<decimal>("UsrHours");

            if (orderId == Guid.Empty)
                return;

            var esq = new EntitySchemaQuery(userConnection.EntitySchemaManager, "UsrOrderWork");
            var sumColumn = esq.AddColumn("UsrHours");
            sumColumn.SummaryType = AggregationType.Sum;

            var filterGroup = new EntitySchemaQueryFilterCollection(esq, LogicalOperationStrict.And);
            filterGroup.Add(esq.CreateFilterWithParameters(FilterComparisonType.Equal, "UsrOrderId", orderId));

            var currentId = entity.GetTypedColumnValue<Guid>("Id");
            if (currentId != Guid.Empty)
                filterGroup.Add(esq.CreateFilterWithParameters(FilterComparisonType.NotEqual, "Id", currentId));

            esq.Filters.Add(filterGroup);

            var result = esq.GetEntityCollection(userConnection);
            decimal sum = 0;
            if (result.Count > 0)
                sum = result[0].GetTypedColumnValue<decimal>("UsrHours");

            var total = sum + currentHours;

            int maxHours = Terrasoft.Core.Configuration.SysSettings.GetValue<int>(userConnection, "UsrMaxOrderHours", 0);

            if (total > Convert.ToDecimal(maxHours))
            {
                e.IsCanceled = true;
				throw new Exception($"В заказ-наряде допускается не более {maxHours} нормо-часов.");
                //throw new Exception(string.Format(
                    //new LocalizableString("UsrOrderWorkEventListener", "HoursValidationMessage").ToString(),
                    //maxHours));
            }
        }
    }
}
