define("UsrSchema6cccf61fPage", [], function() {
	return {
		entitySchemaName: "UsrOrderWork",
		attributes: {},
		modules: /**SCHEMA_MODULES*/{}/**SCHEMA_MODULES*/,
		details: /**SCHEMA_DETAILS*/{}/**SCHEMA_DETAILS*/,
		businessRules: /**SCHEMA_BUSINESS_RULES*/{}/**SCHEMA_BUSINESS_RULES*/,
		methods: {
		
		    // Асинхронная валидация перед сохранением
		    asyncValidate: function(callback, scope) {
		        this.callParent([
		            function(parentResult) {
		                if (!parentResult.success) {
		                    callback.call(scope || this, parentResult);
		                    return;
		                }
		                this.validateTotalHours(function(result) {
		                    callback.call(scope || this, result);
		                }, this);
		            },
		            scope || this
		        ]);
		    },
		
		    // Проверка суммы нормо-часов по заказ-наряду
		    validateTotalHours: function(callback, scope) {
		        var orderId = this.get("UsrOrderId");
		        var currentHours = this.get("UsrHours") || 0;
		
		        if (!orderId) {
		            callback.call(scope || this, { success: true });
		            return;
		        }
		
		        var esq = Ext.create("Terrasoft.EntitySchemaQuery", {
		            rootSchemaName: "UsrOrderWork"
		        });
		
		        var sumColumn = esq.addColumn("UsrHours", "SumUsrHours");
		        sumColumn.summaryType = Terrasoft.AggregationType.SUM;
		
		        esq.filters.addItem(esq.createColumnFilterWithParameter(
		            Terrasoft.ComparisonType.EQUAL,
		            "UsrOrderId",
		            orderId
		        ));
		
		        var currentId = this.get("Id");
		        if (currentId) {
		            esq.filters.addItem(esq.createColumnFilterWithParameter(
		                Terrasoft.ComparisonType.NOT_EQUAL,
		                "Id",
		                currentId
		            ));
		        }
		
		        var self = this;
		
		        esq.getEntityCollection(function(result) {
		            if (!result.success) {
		                callback.call(scope || self, { success: true });
		                return;
		            }
		
		            var sum = 0;
		            var collection = result.collection;
		            if (collection.getCount() > 0) {
		                var row = collection.getByIndex(0);
		                sum = row.get("SumUsrHours") || 0;
		            }
		
		            var total = sum + currentHours;
		
		            // Чтение системной настройки с обработчиком ошибок
		            Terrasoft.SysSettings.querySysSettingsItem(
		                "UsrMaxOrderHours",
		                function(maxHours) {
		                    maxHours = maxHours || 0;
		
		                    if (total > maxHours) {
		                        var message = Ext.String.format(
		                            self.get("Resources.Strings.HoursValidationMessage"),
		                            maxHours
		                        );
		
		                        Terrasoft.showInformation(message);
		
		                        callback.call(scope || self, {
		                            success: false,
		                            message: message
		                        });
		                    } else {
		                        callback.call(scope || self, { success: true });
		                    }
		                },
		                self,
		                false, // отключаем кэш
		                function() { // ОБЯЗАТЕЛЬНО: обработка ошибки
		                    callback.call(scope || self, { success: true });
		                }
		            );
		        }, this);
		    }
		},
		dataModels: /**SCHEMA_DATA_MODELS*/{}/**SCHEMA_DATA_MODELS*/,
		diff: /**SCHEMA_DIFF*/[
			{
				"operation": "insert",
				"name": "UsrNumber87d12f3b-bfa3-46ea-ab48-1a9b82c94220",
				"values": {
					"layout": {
						"colSpan": 12,
						"rowSpan": 1,
						"column": 0,
						"row": 0,
						"layoutName": "Header"
					},
					"bindTo": "UsrNumber",
					"enabled": false
				},
				"parentName": "Header",
				"propertyName": "items",
				"index": 0
			},
			{
				"operation": "insert",
				"name": "UsrName8c0a971f-5b1c-4002-998b-69b92c81505e",
				"values": {
					"layout": {
						"colSpan": 12,
						"rowSpan": 1,
						"column": 12,
						"row": 0,
						"layoutName": "Header"
					},
					"bindTo": "UsrName"
				},
				"parentName": "Header",
				"propertyName": "items",
				"index": 1
			},
			{
				"operation": "insert",
				"name": "UsrWorkTypeId127e5b08-1718-404c-9640-fbda84e5e2b6",
				"values": {
					"layout": {
						"colSpan": 12,
						"rowSpan": 1,
						"column": 0,
						"row": 1,
						"layoutName": "Header"
					},
					"bindTo": "UsrWorkTypeId"
				},
				"parentName": "Header",
				"propertyName": "items",
				"index": 2
			},
			{
				"operation": "insert",
				"name": "UsrHours3cd7a87a-a7ff-4c77-b1b1-b8c0d2a2ad6d",
				"values": {
					"layout": {
						"colSpan": 12,
						"rowSpan": 1,
						"column": 12,
						"row": 1,
						"layoutName": "Header"
					},
					"bindTo": "UsrHours"
				},
				"parentName": "Header",
				"propertyName": "items",
				"index": 3
			}
		]/**SCHEMA_DIFF*/
	};
});
