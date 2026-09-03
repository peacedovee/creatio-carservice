define("UsrOrder1Page", ["ProcessModuleUtilities", "RightUtilities"], function(ProcessModuleUtilities, RightUtilities) {
	return {
		entitySchemaName: "UsrOrder",
		attributes: {
			"UsrOwnerId":{
				lookupListConfig: {
		            filters: [
					    function() {
					        var filter = Ext.create("Terrasoft.FilterGroup");
					        filter.logicalOperation = Terrasoft.LogicalOperatorType.OR;
					
					        filter.add("EmployeeRU",
					            Terrasoft.createColumnFilterWithParameter(
					                Terrasoft.ComparisonType.EQUAL,
					                "Type.Name",
					                "Сотрудник"
					            )
					        );
					
					        filter.add("EmployeeEN",
					            Terrasoft.createColumnFilterWithParameter(
					                Terrasoft.ComparisonType.EQUAL,
					                "Type.Name",
					                "Employee"
					            )
					        );
					
					        return filter;
					    }
					]
		        }
			},
			"CanFillByVin": {
			    dataValueType: Terrasoft.DataValueType.BOOLEAN,
			    type: Terrasoft.ViewModelColumnType.VIRTUAL_COLUMN,
			    value: false
			}
		},
		modules: /**SCHEMA_MODULES*/{}/**SCHEMA_MODULES*/,
		details: /**SCHEMA_DETAILS*/{
			"Files": {
				"schemaName": "FileDetailV2",
				"entitySchemaName": "UsrOrderFile",
				"filter": {
					"masterColumn": "Id",
					"detailColumn": "UsrOrder"
				}
			},
			"UsrRepairWorkDetail": {
				"schemaName": "UsrSchema2ad96e78Detail",
				"entitySchemaName": "UsrOrderWork",
				"filter": {
					"detailColumn": "UsrOrderId",
					"masterColumn": "Id"
				}
			}
		}/**SCHEMA_DETAILS*/,
		businessRules: /**SCHEMA_BUSINESS_RULES*/{}/**SCHEMA_BUSINESS_RULES*/,
		methods: {
			init: function() {
			    this.callParent(arguments);
			    Terrasoft.ServerChannel.on(Terrasoft.EventName.ON_MESSAGE, this.onServerMessage, this);
				this.checkCanFillByVin();
			},
			checkCanFillByVin: function() {
			    RightUtilities.checkCanExecuteOperation({
				    operation: "CanFillOrderByVIN"
				}, function(result) {
				    this.set("CanFillByVin", result);
				}, this);
			},
			onServerMessage: function(scope, message) {
			    if (!message || message.Header.Sender !== "VinProcessResult") {
			        return;
			    }
			
			    var code = message.Body;
				var localized = this.get("Resources.Strings." + code);
			
			    if (localized) {
			
			        // Успех
			        if (code === "VinUpdated") {
			            Terrasoft.showInformation(localized);
			            this.reloadEntity();
			        } else {
			            Terrasoft.showErrorMessage(localized);
			        }
			
			    } else {
			        Terrasoft.showErrorMessage(code);
			    }
			},
			onFillByVinClick: function() {
			    var orderId = this.get("Id");
			
			    // Если запись не сохранена
			    if (!orderId) {
			        this.save({
			            callback: function(response) {
			
			                if (!response.success) {
			                    var errors = response.validationInfo && response.validationInfo.validationInfo;
			                    if (errors && errors.length > 0) {
			                        Terrasoft.showErrorMessage(errors[0].message);
			                    } else {
			                        Terrasoft.showErrorMessage(response.message || this.get("Resources.Strings.SaveError"));
			                    }
			                    return;
			                }
			
			                var newId = this.get("Id");
			
			                ProcessModuleUtilities.executeProcess({
			                    sysProcessName: "UsrFillOrderByVINProcess",
			                    parameters: {
			                        ProcessSchemaParameterOrderID: newId
			                    },
			                    scope: this
			                });
			            },
			            scope: this
			        });
			
			        return;
			    }
			
			    // Если запись уже сохранена
			    ProcessModuleUtilities.executeProcess({
			        sysProcessName: "UsrFillOrderByVINProcess",
			        parameters: {
			            ProcessSchemaParameterOrderID: orderId
			        },
			        scope: this
			    });
			}
		},
		dataModels: /**SCHEMA_DATA_MODELS*/{}/**SCHEMA_DATA_MODELS*/,
		diff: /**SCHEMA_DIFF*/[
			{
				"operation": "insert",
				"name": "UsrNumbera85e44d8-bc0e-4edc-95c2-d663796f03d3",
				"values": {
					"layout": {
						"colSpan": 24,
						"rowSpan": 1,
						"column": 0,
						"row": 0,
						"layoutName": "ProfileContainer"
					},
					"bindTo": "UsrNumber",
					"enabled": false
				},
				"parentName": "ProfileContainer",
				"propertyName": "items",
				"index": 0
			},
			{
				"operation": "insert",
				"name": "UsrName0992eb94-e401-4de9-b7cb-ee616af2650a",
				"values": {
					"layout": {
						"colSpan": 24,
						"rowSpan": 1,
						"column": 0,
						"row": 1,
						"layoutName": "ProfileContainer"
					},
					"bindTo": "UsrName"
				},
				"parentName": "ProfileContainer",
				"propertyName": "items",
				"index": 1
			},
			{
				"operation": "insert",
				"name": "UsrAcceptanceDate6929d3b5-73ba-4ecc-9f91-0078701ad250",
				"values": {
					"layout": {
						"colSpan": 24,
						"rowSpan": 1,
						"column": 0,
						"row": 2,
						"layoutName": "ProfileContainer"
					},
					"bindTo": "UsrAcceptanceDate"
				},
				"parentName": "ProfileContainer",
				"propertyName": "items",
				"index": 2
			},
			{
				"operation": "insert",
				"name": "UsrOwnerIdaa00517c-c162-472e-90c0-6046e3f1b3ab",
				"values": {
					"layout": {
						"colSpan": 24,
						"rowSpan": 1,
						"column": 0,
						"row": 3,
						"layoutName": "ProfileContainer"
					},
					"bindTo": "UsrOwnerId"
				},
				"parentName": "ProfileContainer",
				"propertyName": "items",
				"index": 3
			},
			{
				"operation": "insert",
				"name": "UsrStatusIdb3024147-a09e-4252-a2c1-89704ef8a6d0",
				"values": {
					"layout": {
						"colSpan": 24,
						"rowSpan": 1,
						"column": 0,
						"row": 4,
						"layoutName": "ProfileContainer"
					},
					"bindTo": "UsrStatusId"
				},
				"parentName": "ProfileContainer",
				"propertyName": "items",
				"index": 4
			},
			{
				"operation": "insert",
				"name": "UsrActive43b3374d-eee0-4cce-a79d-3ab7782dbb00",
				"values": {
					"layout": {
						"colSpan": 24,
						"rowSpan": 1,
						"column": 0,
						"row": 5,
						"layoutName": "ProfileContainer"
					},
					"bindTo": "UsrActive"
				},
				"parentName": "ProfileContainer",
				"propertyName": "items",
				"index": 5
			},
			{
				"operation": "insert",
				"name": "UsrClientIda03187ad-8f07-49c9-80e2-5abf35be06fa",
				"values": {
					"layout": {
						"colSpan": 12,
						"rowSpan": 1,
						"column": 0,
						"row": 0,
						"layoutName": "Header"
					},
					"bindTo": "UsrClientId"
				},
				"parentName": "Header",
				"propertyName": "items",
				"index": 0
			},
			{
				"operation": "insert",
				"name": "UsrPlateNumber52ab44e2-b8c3-4fa7-8c23-4e1da171566b",
				"values": {
					"layout": {
						"colSpan": 12,
						"rowSpan": 1,
						"column": 0,
						"row": 1,
						"layoutName": "Header"
					},
					"bindTo": "UsrPlateNumber"
				},
				"parentName": "Header",
				"propertyName": "items",
				"index": 1
			},
			{
				"operation": "insert",
				"name": "UsrVIN1267a299-bdae-4002-963c-b177b2bc7016",
				"values": {
					"layout": {
						"colSpan": 12,
						"rowSpan": 1,
						"column": 0,
						"row": 2,
						"layoutName": "Header"
					},
					"bindTo": "UsrVIN"
				},
				"parentName": "Header",
				"propertyName": "items",
				"index": 2
			},
			{
				"operation": "insert",
				"name": "UsrBrand147b1b7e-11bd-477d-ae8e-dc783a9017ad",
				"values": {
					"layout": {
						"colSpan": 12,
						"rowSpan": 1,
						"column": 12,
						"row": 0,
						"layoutName": "Header"
					},
					"bindTo": "UsrBrand",
					"enabled": true
				},
				"parentName": "Header",
				"propertyName": "items",
				"index": 3
			},
			{
				"operation": "insert",
				"name": "UsrModela114348c-6a6e-425d-8b51-c1fe06f8b13b",
				"values": {
					"layout": {
						"colSpan": 12,
						"rowSpan": 1,
						"column": 12,
						"row": 1,
						"layoutName": "Header"
					},
					"bindTo": "UsrModel",
					"enabled": true
				},
				"parentName": "Header",
				"propertyName": "items",
				"index": 4
			},
			{
				"operation": "insert",
				"name": "UsrYear7b5cb4f2-954b-4379-be20-ee443d378474",
				"values": {
					"layout": {
						"colSpan": 12,
						"rowSpan": 1,
						"column": 12,
						"row": 2,
						"layoutName": "Header"
					},
					"bindTo": "UsrYear"
				},
				"parentName": "Header",
				"propertyName": "items",
				"index": 5
			},
			{
				"operation": "insert",
				"name": "FillByVinButton",
				"values": {
					"itemType": 5,
					"caption": {
						"bindTo": "Resources.Strings.FillByVinButtonCaption"
					},
					"click": {
						"bindTo": "onFillByVinClick"
					},
					"style": "blue",
					"layout": {
						"colSpan": 12,
						"rowSpan": 1,
						"column": 12,
						"row": 3,
						"layoutName": "Header"
					},
					"visible": { "bindTo": "CanFillByVin" }
				},
				"parentName": "Header",
				"propertyName": "items",
				"index": 6
			},
			{
				"operation": "insert",
				"name": "UsrCommentb417904c-7aad-4074-a8ab-f345fac0f556",
				"values": {
					"layout": {
						"colSpan": 12,
						"rowSpan": 1,
						"column": 0,
						"row": 3,
						"layoutName": "Header"
					},
					"bindTo": "UsrComment",
					"enabled": true,
					"contentType": 0
				},
				"parentName": "Header",
				"propertyName": "items",
				"index": 7
			},
			{
				"operation": "insert",
				"name": "TabRepairWorks",
				"values": {
					"caption": {
						"bindTo": "Resources.Strings.TabRepairWorksTabCaption"
					},
					"items": [],
					"order": 0
				},
				"parentName": "Tabs",
				"propertyName": "tabs",
				"index": 1
			},
			{
				"operation": "insert",
				"name": "UsrRepairWorkDetail",
				"values": {
					"itemType": 2,
					"markerValue": "added-detail"
				},
				"parentName": "TabRepairWorks",
				"propertyName": "items",
				"index": 0
			},
			{
				"operation": "remove",
				"name": "ESNTab"
			},
			{
				"operation": "remove",
				"name": "ESNFeedContainer"
			},
			{
				"operation": "remove",
				"name": "ESNFeed"
			}
		]/**SCHEMA_DIFF*/
	};
});
