using System;
using Assets.SimpleLocalization.Scripts;
using UnityEngine;
using UnityEngine.UI;

	public class LanguageMgr : BaseMgr<LanguageMgr>
	{
		// public Text FormattedText;

		/// <summary>
		/// Called on app start.
		/// </summary>
		public void Awake()
		{
			LocalizationManager.Read();

			/*switch (Application.systemLanguage)
			{
				case SystemLanguage.Chinese:
					LocalizationManager.Language = "Chinese";
					break;
				default:
					LocalizationManager.Language = "English";
					break;
			}*/

			// This way you can localize and format strings from code.
			// FormattedText.text = LocalizationManager.Localize("Settings.Example.PlayTime", TimeSpan.FromHours(10.5f).TotalHours);

			// This way you can subscribe to LocalizationChanged event.
			// LocalizationManager.OnLocalizationChanged += () => FormattedText.text = LocalizationManager.Localize("Settings.Example.PlayTime", TimeSpan.FromHours(10.5f).TotalHours);
		}

		/// <summary>
		/// Change localization at runtime.
		/// <param name="localization">language name</param>>
		/// </summary>
		public void SetLocalization(string localization)
		{
			LocalizationManager.Language = localization;
		}

		/// <summary>
		/// Change localization at runtime between Chinese and English only.
		/// </summary>
		public void SetLocalization()
		{
			LocalizationManager.Language = (LocalizationManager.Language == "Chinese")
				? "English"
				: "Chinese";
		}

		/// <summary>
		/// Play Dialog From .csv file
		/// </summary>
		public void PlayDialog(int index)
		{
			// FormattedText.text = LocalizationManager.Localize("Setting.DialogTest" + index);
		}
		
	}
