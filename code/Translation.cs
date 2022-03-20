using System.Collections.Generic;

namespace Sandbox;

public class Translation
{
	private static Dictionary<string, string> CurrentLangStrings { get; set; } = new();
	private static List<string> AllLangs { get; set; } = new();
	
	public Translation()
	{
		FindAllTranslations();
		var preferedLang = LoadPreference();
		CurrentLangStrings = FileSystem.Mounted
			.ReadJson<Dictionary<string, string>>($"langs/{preferedLang}.json");
	}

	void FindAllTranslations()
	{
		var langs = FileSystem.Mounted.FindFile( "langs" );
		foreach ( var lang in langs )
		{
			var langFile = lang.Split( "." );
			if(langFile[1] == "json") continue;
			AllLangs.Add( lang.Split( '.')[0] );
		}
	}
	
	public static string Translate( string message = "MISSING TRANSLATION" )
	{
		var result = CurrentLangStrings[message];

		return result == null ? "Error" : result;
	}
	
	[ClientCmd("td_getlangs")]
	public static void GetLanguages()
	{
		Log.Info( "All available languages:" );
		foreach ( var lang in AllLangs ) {
			Log.Info( lang );
		}
	}

	[ClientCmd("td_setlang")]
	public static void SetLanguage(string lang)
	{
		var setLang = lang.ToLower();
		if ( AllLangs.Contains( setLang ) )
		{
			var filename = setLang.ToLower() + ".json";
			CurrentLangStrings = FileSystem.Mounted.ReadJson<Dictionary<string, string>>("langs/" + filename);
			SavePreference( setLang );
			return;
		}
		Log.Info( "Wrong language option" );
	}

	private static void SavePreference(string lang)
	{
		FileSystem.Data.WriteAllText( "lang.txt", lang );
	}
	
	private static string LoadPreference()
	{
		var lang = FileSystem.Data.ReadAllText( "lang.txt" );
		return AllLangs.Contains( lang ) ? lang : "en";
	}
}
