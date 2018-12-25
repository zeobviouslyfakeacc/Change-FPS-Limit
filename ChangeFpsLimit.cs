using System;
using System.IO;
using System.Reflection;
using Harmony;
using UnityEngine;
using Newtonsoft.Json;

namespace ChangeFpsLimit {
	internal static class ChangeFpsLimit {

		private static readonly string MODS_FOLDER_PATH = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
		private static readonly string SETTINGS_PATH = Path.Combine(MODS_FOLDER_PATH, "Change-FPS-Limit.json");

		private static FpsSettings settings;

		public static void OnLoad() {
			settings = LoadOrCreateSettings();
			settings.AddToModSettings("Change FPS Limit");

			Version version = Assembly.GetExecutingAssembly().GetName().Version;
			Debug.Log("[Change-FPS-Limit] Version " + version + " loaded!");
		}

		[HarmonyPatch(typeof(GameManager), "Awake", new Type[0])]
		private static class SetFpsTargetOnLoad {
			private static void Postfix() {
				settings.Apply();
			}
		}

		private static FpsSettings LoadOrCreateSettings() {
			if (!File.Exists(SETTINGS_PATH)) {
				Debug.Log("[Change-FPS-Limit] Settings file did not exist, using default settings.");
				return new FpsSettings();
			}

			try {
				string json = File.ReadAllText(SETTINGS_PATH, System.Text.Encoding.UTF8);
				return JsonConvert.DeserializeObject<FpsSettings>(json);
			} catch (Exception ex) {
				Debug.LogError("[Change-FPS-Limit] Error while trying to read config file:");
				Debug.LogException(ex);

				// Re-throw to make error show up in main menu
				throw new IOException("Error while trying to read config file", ex);
			}
		}

		internal static void Save() {
			try {
				string json = JsonConvert.SerializeObject(settings, Formatting.Indented);
				File.WriteAllText(SETTINGS_PATH, json, System.Text.Encoding.UTF8);
				Debug.Log("[Change-FPS-Limit] Config file saved to " + SETTINGS_PATH);
			} catch (Exception ex) {
				Debug.LogError("[Change-FPS-Limit] Error while trying to write config file:");
				Debug.LogException(ex);
			}
		}
	}
}
