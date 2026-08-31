namespace Terrasoft.Configuration
{

	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Globalization;
	using Terrasoft.Common;
	using Terrasoft.Core;
	using Terrasoft.Core.Configuration;

	#region Class: UsrOrderWorkEventListenerSchema

	/// <exclude/>
	public class UsrOrderWorkEventListenerSchema : Terrasoft.Core.SourceCodeSchema
	{

		#region Constructors: Public

		public UsrOrderWorkEventListenerSchema(SourceCodeSchemaManager sourceCodeSchemaManager)
			: base(sourceCodeSchemaManager) {
		}

		public UsrOrderWorkEventListenerSchema(UsrOrderWorkEventListenerSchema source)
			: base( source) {
		}

		#endregion

		#region Methods: Protected

		protected override void InitializeProperties() {
			base.InitializeProperties();
			UId = new Guid("aa2daf34-89f8-43f8-942f-4dac9b9a800a");
			Name = "UsrOrderWorkEventListener";
			ParentSchemaUId = new Guid("50e3acc0-26fc-4237-a095-849a1d534bd3");
			CreatedInPackageId = new Guid("b966a680-bc92-4fad-b3c4-17f9a852b9eb");
			ZipBody = new byte[] { 31,139,8,0,0,0,0,0,4,0,173,85,93,111,218,48,20,125,71,226,63,120,60,37,26,138,250,188,174,72,148,209,174,82,63,54,209,174,15,85,31,220,228,146,122,75,236,244,218,102,101,213,254,251,174,237,0,9,132,118,154,22,33,65,194,61,231,158,115,124,237,88,45,100,206,102,75,109,160,60,236,247,172,191,189,6,68,174,213,220,36,19,133,176,231,113,50,149,70,24,1,250,173,255,147,233,2,164,233,46,43,75,37,247,225,39,74,206,69,110,145,27,225,139,250,61,201,75,208,21,79,161,85,218,168,234,247,94,250,61,70,215,157,111,190,244,157,207,5,153,147,128,209,44,125,132,146,95,18,9,59,98,131,27,141,87,152,1,222,42,252,49,136,239,3,174,178,15,133,72,89,90,112,173,89,179,162,197,196,62,176,99,174,161,163,71,96,169,69,52,8,213,130,4,139,12,216,66,137,140,93,201,25,95,144,227,72,61,124,135,212,48,13,146,218,12,89,32,60,134,57,217,247,180,99,204,53,131,120,67,215,96,118,215,130,35,3,15,34,71,81,128,199,129,237,112,183,210,106,64,138,75,82,75,202,138,16,1,154,220,180,158,251,164,183,161,202,229,112,150,109,48,167,96,174,151,21,100,19,85,216,82,126,227,133,133,143,167,86,100,163,104,29,236,89,54,136,59,84,164,22,145,72,62,43,139,250,117,190,12,82,81,242,34,80,250,122,79,216,166,20,115,22,173,213,29,49,167,33,153,150,21,229,208,174,115,23,130,177,216,109,16,244,19,137,145,240,179,94,132,48,43,95,45,224,50,106,231,150,52,11,46,184,228,185,91,186,173,113,234,240,173,109,25,204,57,211,250,41,25,103,181,217,45,123,77,220,26,147,204,108,89,114,92,186,140,8,63,206,115,132,220,143,188,123,226,254,237,116,53,23,133,1,60,69,101,171,125,238,78,124,9,117,41,130,189,136,180,13,217,185,202,69,202,139,171,10,194,198,154,25,20,169,73,198,50,219,150,216,104,225,44,57,120,50,65,224,6,2,243,173,48,143,95,56,210,166,163,27,29,173,218,149,21,71,161,107,249,211,39,203,139,70,134,52,57,195,213,200,197,187,75,222,152,162,191,28,201,142,81,116,115,179,33,121,247,198,228,252,15,151,151,202,172,140,122,131,235,238,29,22,29,127,32,209,190,95,163,127,119,30,8,218,22,166,30,45,74,34,172,115,99,89,219,67,188,157,70,189,213,220,192,17,199,65,71,86,161,1,29,183,86,26,54,98,7,29,33,5,112,40,188,59,184,255,247,61,237,12,25,101,72,207,145,39,125,223,58,52,118,143,0,82,84,242,231,213,137,242,218,107,36,161,55,221,12,140,161,195,87,59,125,65,20,17,140,182,2,10,211,120,193,159,253,64,6,161,67,114,221,121,254,4,173,35,70,112,58,231,77,114,173,62,5,155,209,74,86,188,21,215,203,110,122,144,156,233,9,151,41,20,224,134,218,160,133,195,221,42,243,136,234,103,216,201,207,41,84,126,105,53,109,78,153,39,39,10,75,110,162,93,140,187,28,226,92,209,150,22,191,248,67,1,51,15,137,6,123,95,113,100,118,224,149,83,66,34,243,217,93,128,214,116,216,13,98,242,87,227,227,97,119,183,141,237,45,11,191,55,183,245,79,250,162,207,31,52,167,38,102,135,8,0,0 };
		}

		protected override void InitializeLocalizableStrings() {
			base.InitializeLocalizableStrings();
			SetLocalizableStringsDefInheritance();
			LocalizableStrings.Add(CreateHoursValidationMessageLocalizableString());
		}

		protected virtual SchemaLocalizableString CreateHoursValidationMessageLocalizableString() {
			SchemaLocalizableString localizableString = new SchemaLocalizableString() {
				UId = new Guid("0681610a-530f-3c9e-f622-f117436ea774"),
				Name = "HoursValidationMessage",
				CreatedInPackageId = new Guid("b966a680-bc92-4fad-b3c4-17f9a852b9eb"),
				CreatedInSchemaUId = new Guid("aa2daf34-89f8-43f8-942f-4dac9b9a800a"),
				ModifiedInSchemaUId = new Guid("aa2daf34-89f8-43f8-942f-4dac9b9a800a")
			};
			return localizableString;
		}

		#endregion

		#region Methods: Public

		public override void GetParentRealUIds(Collection<Guid> realUIds) {
			base.GetParentRealUIds(realUIds);
			realUIds.Add(new Guid("aa2daf34-89f8-43f8-942f-4dac9b9a800a"));
		}

		#endregion

	}

	#endregion

}

