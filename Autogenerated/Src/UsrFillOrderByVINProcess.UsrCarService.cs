namespace Terrasoft.Core.Process
{

	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Drawing;
	using System.Globalization;
	using System.Text;
	using Terrasoft.Common;
	using Terrasoft.Configuration;
	using Terrasoft.Core;
	using Terrasoft.Core.Configuration;
	using Terrasoft.Core.DB;
	using Terrasoft.Core.Entities;
	using Terrasoft.Core.Process;
	using Terrasoft.Core.Process.Configuration;
	using Terrasoft.UsrCarService;

	#region Class: UsrFillOrderByVINProcessMethodsWrapper

	/// <exclude/>
	public class UsrFillOrderByVINProcessMethodsWrapper : ProcessModel
	{

		public UsrFillOrderByVINProcessMethodsWrapper(Process process)
			: base(process) {
			AddScriptTaskMethod("ScriptTaskVINExecute", ScriptTaskVINExecute);
		}

		#region Methods: Private

		private bool ScriptTaskVINExecute(ProcessExecutingContext context) {
			
			var orderId = Get<Guid>("ProcessSchemaParameterOrderID");
			
			var service = new UsrCarVINService(UserConnection);
			
			var response = service.FillOrderByVIN(orderId);
			
			var message = response.Message;
			MsgChannelUtilities.PostMessage(UserConnection, "VinProcessResult", message);
			
			return true;
		}

		#endregion

	}

	#endregion

}

