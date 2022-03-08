using System;
using System.Collections.Generic;
using Sandbox;

public partial class German
{
	public List<(string, string)> german;
	public German()
	{
		german = new List<(string, string)>();

		german.Add( ("Stat_Coins", "Münzen: ") );

		german.Add( ("Tower_Level", "Level: ") );

		german.Add( ("Current_Wave", "Welle: ") );
		german.Add( ("Timer_Game", "Spiel startet in ") );
		german.Add( ("Timer_Wave", "Welle startet in ") ); 
		german.Add( ("Game_Finished", "Game Over!") );

		//Info on the hotbar
		german.Add( ("Tower_Pistol_Info", "Pistolenturm\nKostet 10 Münzen") );
		german.Add( ("Tower_SMG_Info", "SMG-Turm\nKostet 25 Münzen") );
		german.Add( ("Tower_Explosive_Info", "Kanonenturm\nKosten 40 Münzen") );
		german.Add( ("Tower_Electric_Info", "Elektrischer Turm\nKosten 65 Münzen") );
		german.Add( ("Tower_Radar_Info", "Radarturm\nKostet 50 Münzen") );
		german.Add( ("Tower_Sniper_Info", "Scharfschützenturm\nKosten 75 Münzen") );
		german.Add( ("Tower_Frost_Info", "Frostturm\nKosten 45 Münzen") );

		//Info when hovering on a tower
		//Names
		german.Add( ("Pistol Tower", "Pistolenturm") );
		german.Add( ("SMG Tower", "SMG-Turm") );
		german.Add( ("Explosive Tower", "Kanonenturm") );
		german.Add( ("Electric Tower", "Elektrischer Turm") );
		german.Add( ("Radar Tower", "Radarturm") );
		german.Add( ("Sniper Tower", "Scharfschützenturm") );
		german.Add( ("Frost Tower", "Frostturm") );

		//Descriptions
		german.Add( ("Pistol Tower Desc", "Ein einfacher Schützenturm") );
		german.Add( ("SMG Tower Desc", "Ein automatischer Schützenturm") );
		german.Add( ("Explosive Tower Desc", "Ein Turm, welcher Kanonen feuert") );
		german.Add( ("Electric Tower Desc", "Ein elektrifizierender Turm") );
		german.Add( ("Radar Tower Desc", "Ein Radarturm, welcher Türme aufwertet und getarnte Feinde aufdeckt") );
		german.Add( ("Sniper Tower Desc", "Ein Turm mit großer Reichweite") );
		german.Add( ("Frost Tower Desc", "Ein Turm, welcher Gegner verlangsamt") );

		//Misc
		german.Add( ("Info_OnUpgrade", "Bei Aufrüstung") );
		german.Add( ("Info_DMG", "Schaden: ") );
		german.Add( ("Info_Cooldown", " | Abkühlzeit: ") );
		german.Add( ("Info_Range", " | Reichweite: ") );

		german.Add( ("Info_Upgrade", "Verbessert den Zielturm") );
		german.Add( ("Info_Sell", "Verkauft Zielturm") );

		//NPCs
		german.Add( ("Peasant", "Bauer\nHP: ") );
		german.Add( ("Hidden", "Verborgener\nHP: ") );
		german.Add( ("Zombie", "Zombie\nHP: ") );
		german.Add( ("Rebel", "Rebell\nHP: ") );
		german.Add( ("Rioter", "Randalierer\nHP: ") );
		german.Add( ("Voidling", "Voidling\nHP: ") );
		german.Add( ("Brute", "Wüstling\nHP: ") );

		german.Add( ("Zombie Boss", "Zombie-Boss\n HP: ") );
		german.Add( ("Void King", "Void-König\n HP: ") );
	}

	public List<(string, string)> GetGerman()
	{
		return german;
	}
}
