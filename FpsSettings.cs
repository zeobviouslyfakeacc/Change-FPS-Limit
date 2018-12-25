using ModSettings;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using UnityEngine;

namespace ChangeFpsLimit {
	internal class FpsSettings : ModSettingsBase {

		private static readonly int[] enumValues = { -1, 15, 30, 50, 60, 100, 120, 144, 200, 240, 300 };

		[Name("FPS Limit")]
		[Choice("Unlimited", "15 FPS", "30 FPS", "50 FPS", "60 FPS", "100 FPS", "120 FPS (default)", "144 FPS", "200 FPS", "240 FPS", "300 FPS")]
		public FpsTarget fpsTarget = FpsTarget.FPS_120;

		protected override void OnConfirm() {
			Apply();
			ChangeFpsLimit.Save();
		}

		public void Apply() {
			Application.targetFrameRate = enumValues[(int) fpsTarget];
		}
	}

	[JsonConverter(typeof(StringEnumConverter))]
	internal enum FpsTarget {
		UNLIMITED,
		FPS_15,
		FPS_30,
		FPS_50,
		FPS_60,
		FPS_100,
		FPS_120,
		FPS_144,
		FPS_200,
		FPS_240,
		FPS_300
	}
}
