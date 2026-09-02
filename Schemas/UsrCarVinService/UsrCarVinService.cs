namespace Terrasoft.UsrCarService
{
    using System;
    using System.IO;
    using System.Net;
    using System.ServiceModel;
    using System.ServiceModel.Web;
    using System.ServiceModel.Activation;
    using Newtonsoft.Json;
    using Terrasoft.Core;
    using Terrasoft.Core.Entities;
    using Terrasoft.Web.Common;
    using Terrasoft.Common;
	using global::Common.Logging;

    [ServiceContract]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    public class UsrCarVINService: BaseService
    {
		public UsrCarVINService() { }

	    public UsrCarVINService(UserConnection userConnection) {
	      _UserConnection = userConnection;
	    }

		private UserConnection CurrentUserConnection {
	      get { return _UserConnection ?? UserConnection; }
	    }
		
		private string L(string code) {
		    return new LocalizableString(
		        CurrentUserConnection.Workspace.ResourceStorage,
		        "UsrCarVINService",
		        code
		    ).ToString();
		}

		public UserConnection _UserConnection { get; set; }

		private static readonly ILog log = LogManager.GetLogger("Terrasoft.Configuration.UsrCarVINService");

        public class VinResponse
        {
            public bool Success { get; set; }
            public string Make { get; set; }
            public string Model { get; set; }
            public string Year { get; set; }
            public string Message { get; set; }
			public string _json { get; set; }
        }

        [OperationContract]
        [WebInvoke(Method = "POST",
            BodyStyle = WebMessageBodyStyle.Wrapped,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
			public VinResponse FillOrderByVIN(Guid orderId)
			{
			    var result = new VinResponse();
				
			    try
			    {
			        log.Info($"FillOrderByVIN started. OrderId = {orderId}");
			
			        var uc = CurrentUserConnection;
			
			        // 1. Читаем заказ-наряд
			        var esq = new EntitySchemaQuery(uc.EntitySchemaManager, "UsrOrder");
					esq.PrimaryQueryColumn.IsAlwaysSelect = true;
					esq.AddColumn("Id");
			        esq.AddColumn("UsrVIN");
			        esq.AddColumn("UsrBrand");
			        esq.AddColumn("UsrModel");
			        esq.AddColumn("UsrYear");
			
			        esq.Filters.Add(esq.CreateFilterWithParameters(
			            FilterComparisonType.Equal, "Id", orderId));
			
					var orders = esq.GetEntityCollection(uc);
					if (orders.Count == 0) {
					    log.Warn("Order not found in DB for Id = " + orderId);
					    result.Success = false;
					    result.Message = "OrderNotSaved";
					    return result;
					}
					
					var order = orders[0];
					
			        var vin = order.GetTypedColumnValue<string>("UsrVIN");
			
			        // 2. Проверка VIN
			        if (string.IsNullOrWhiteSpace(vin))
			        {
			            result.Success = false;
			            result.Message = "VinEmpty";
			            log.Warn($"VIN is empty for OrderId = {orderId}");
			            return result;
			        }
			
			        // 3. Внешний API
			        var url = $"https://vpic.nhtsa.dot.gov/api/vehicles/decodevin/{vin}?format=json";
			        string json;
			
			        try
			        {
			            var request = WebRequest.Create(url);
			            using (var response = request.GetResponse())
			            using (var stream = response.GetResponseStream())
			            using (var reader = new StreamReader(stream))
			            {
			                json = reader.ReadToEnd();
			            }
			        }
			        catch (Exception ex)
			        {
			            result.Success = false;
			            result.Message = "ExternalServiceError";
			            log.Error($"External VIN API error for VIN={vin}: {ex}");
			            return result;
			        }
			
			        // 4. Парсим JSON
					result._json = json;
			        string brand = ExtractValue(json, "Make");
			        string model = ExtractValue(json, "Model");
			        string year = ExtractValue(json, "Model Year");
			
			        if (brand == null && model == null && year == null)
			        {
			            result.Success = false;
			            result.Message = "VinNotRecognized";
			            log.Warn($"VIN not recognized: {vin}");
			            return result;
			        }
			
			        // 5. Сохраняем данные в заказ-наряд
					if (!string.IsNullOrEmpty(brand))
	                    order.SetColumnValue("UsrBrand", brand);
                
	                if (!string.IsNullOrEmpty(model))
	                    order.SetColumnValue("UsrModel", model);
			
			        if (int.TryParse(year, out var y))
			            order.SetColumnValue("UsrYear", y);
			
			        order.Save();
			
			        log.Info($"FillOrderByVIN completed successfully for OrderId = {orderId}");
			
			        result.Success = true;
			        result.Make = brand;
			        result.Model = model;
			        result.Year = year;
			        result.Message = "VinUpdated";
			    }
			    catch (Exception ex)
			    {
			        result.Success = false;
			        result.Message = "ServiceError";
			        log.Error($"FillOrderByVIN error for OrderId = {orderId}: {ex}");
			    }
			
			    return result;
			}

			private string ExtractValue(string json, string variableName)
			{
			    try
			    {
			        // Ищем позицию Variable:"X"
			        string variableMarker = $"\"Variable\":\"{variableName}\"";
			        int varPos = json.IndexOf(variableMarker, StringComparison.OrdinalIgnoreCase);
			        if (varPos == -1)
			            return null;
			
			        // Ищем начало объекта { перед Variable
			        int objStart = json.LastIndexOf("{", varPos);
			        if (objStart == -1)
			            return null;
			
			        // Ищем конец объекта } после Variable
			        int objEnd = json.IndexOf("}", varPos);
			        if (objEnd == -1)
			            return null;
			
			        // Ограничиваем поиск Value только этим объектом
			        string objJson = json.Substring(objStart, objEnd - objStart + 1);
			
			        // Ищем Value внутри объекта
			        string valueMarker = "\"Value\":";
			        int valuePos = objJson.IndexOf(valueMarker, StringComparison.OrdinalIgnoreCase);
			        if (valuePos == -1)
			            return null;
			
			        // Проверяем null
			        if (objJson.IndexOf("null", valuePos) == valuePos + valueMarker.Length)
			            return null;
			
			        // Ищем кавычки
			        int startQuote = objJson.IndexOf("\"", valuePos + valueMarker.Length);
			        if (startQuote == -1)
			            return null;
			
			        int endQuote = objJson.IndexOf("\"", startQuote + 1);
			        if (endQuote == -1)
			            return null;
			
			        string result = objJson.Substring(startQuote + 1, endQuote - startQuote - 1);
			
			        return string.IsNullOrEmpty(result) ? null : result;
			    }
			    catch (Exception ex)
			    {
			        log.Error($"Error extracting '{variableName}': {ex}");
			        return null;
			    }
			}

		[OperationContract]
		[WebInvoke(
		    Method = "POST",
		    BodyStyle = WebMessageBodyStyle.Wrapped,
		    RequestFormat = WebMessageFormat.Json,
		    ResponseFormat = WebMessageFormat.Json)]
		public decimal GetOrderHoursByNumber(string orderNumber)
		{
		    try
		    {
		        var uc = CurrentUserConnection;
		
		        // Ищем заказ-наряд по номеру
		        var esqOrder = new EntitySchemaQuery(uc.EntitySchemaManager, "UsrOrder");
		        esqOrder.PrimaryQueryColumn.IsAlwaysSelect = true;
		        esqOrder.AddColumn("Id");
		        esqOrder.Filters.Add(esqOrder.CreateFilterWithParameters(
		            FilterComparisonType.Equal, "UsrNumber", orderNumber));
		
		        var orders = esqOrder.GetEntityCollection(uc);
		        if (orders.Count == 0)
		        {
		            return -1; // заказ не найден
		        }
		
		        var orderId = orders[0].GetTypedColumnValue<Guid>("Id");
		
		        // Суммируем нормо-часы по работам
		        var esqWork = new EntitySchemaQuery(uc.EntitySchemaManager, "UsrOrderWork");
		        esqWork.AddColumn("UsrHours");
		        esqWork.Filters.Add(esqWork.CreateFilterWithParameters(
		            FilterComparisonType.Equal, "UsrOrderId", orderId));
		
		        var works = esqWork.GetEntityCollection(uc);
		
		        decimal total = 0m;
		        foreach (var work in works)
		        {
		            total += work.GetTypedColumnValue<decimal>("UsrHours");
		        }
		
		        return Convert.ToDecimal(total);
		    }
		    catch
		    {
		        return -1;
		    }
		}
    }
}