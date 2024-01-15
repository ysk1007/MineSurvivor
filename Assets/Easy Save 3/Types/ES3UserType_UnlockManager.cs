using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("Instance", "DataExist", "OriCha_info", "UserCharacterData", "OriArti_info", "UserArtifactData")]
	public class ES3UserType_UnlockManager : ES3ComponentType
	{
		public static ES3Type Instance = null;

		public ES3UserType_UnlockManager() : base(typeof(UnlockManager)){ Instance = this; priority = 1;}


		protected override void WriteComponent(object obj, ES3Writer writer)
		{
			var instance = (UnlockManager)obj;
			
			writer.WritePropertyByRef("Instance", UnlockManager.Instance);
			writer.WriteProperty("DataExist", instance.DataExist, ES3Type_bool.Instance);
			writer.WriteProperty("OriCha_info", instance.OriCha_info, ES3Internal.ES3TypeMgr.GetOrCreateES3Type(typeof(System.Collections.Generic.List<UserCharacterData>)));
			writer.WriteProperty("UserCharacterData", instance.UserCharacterData, ES3Internal.ES3TypeMgr.GetOrCreateES3Type(typeof(System.Collections.Generic.List<UserCharacterData>)));
			writer.WriteProperty("OriArti_info", instance.OriArti_info, ES3Internal.ES3TypeMgr.GetOrCreateES3Type(typeof(System.Collections.Generic.List<UserArtifactData>)));
			writer.WriteProperty("UserArtifactData", instance.UserArtifactData, ES3Internal.ES3TypeMgr.GetOrCreateES3Type(typeof(System.Collections.Generic.List<UserArtifactData>)));
		}

		protected override void ReadComponent<T>(ES3Reader reader, object obj)
		{
			var instance = (UnlockManager)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					case "Instance":
						UnlockManager.Instance = reader.Read<UnlockManager>();
						break;
					case "DataExist":
						instance.DataExist = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "OriCha_info":
						instance.OriCha_info = reader.Read<System.Collections.Generic.List<UserCharacterData>>();
						break;
					case "UserCharacterData":
						instance.UserCharacterData = reader.Read<System.Collections.Generic.List<UserCharacterData>>();
						break;
					case "OriArti_info":
						instance.OriArti_info = reader.Read<System.Collections.Generic.List<UserArtifactData>>();
						break;
					case "UserArtifactData":
						instance.UserArtifactData = reader.Read<System.Collections.Generic.List<UserArtifactData>>();
						break;
					default:
						reader.Skip();
						break;
				}
			}
		}
	}


	public class ES3UserType_UnlockManagerArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3UserType_UnlockManagerArray() : base(typeof(UnlockManager[]), ES3UserType_UnlockManager.Instance)
		{
			Instance = this;
		}
	}
}