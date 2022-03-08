using System;
using System.Collections.Generic;
using Sandbox;

public partial class English
{
	public List<(string, string)> english;
	public English()
	{
		english = new List<(string, string)>();

		english.Add( ("Stat_Coins", "Coins: ") );

		english.Add( ("Tower_Level", "Level: ") );

		english.Add( ("Current_Wave", "Wave: ") );
		english.Add( ("Timer_Game", "Starting game in ") );
		english.Add( ("Timer_Wave", "Wave starting in ") );
		english.Add( ("Game_Finished", "Game Over!") );

		//Info on the hotbar
		english.Add( ("Tower_Pistol_Info", "Pistol Tower\nCosts 10 Coins"));
		english.Add( ("Tower_SMG_Info", "SMG Tower\nCosts 25 Coins") );
		english.Add( ("Tower_Explosive_Info", "Cannon Tower\nCosts 40 Coins") );
		english.Add( ("Tower_Electric_Info", "Electric Tower\nCosts 65 Coins") );
		english.Add( ("Tower_Radar_Info", "Radar Tower\nCosts 50 Coins") );
		english.Add( ("Tower_Sniper_Info", "Sniper Tower\nCosts 75 Coins") );
		english.Add( ("Tower_Frost_Info", "Frost Tower\nCosts 45 Coins") );

		//Info when hovering on a tower
		//Names
		english.Add( ("Pistol Tower", "Pistol Tower") );
		english.Add( ("SMG Tower", "SMG Tower") );
		english.Add( ("Explosive Tower", "Explosive Tower") );
		english.Add( ("Electric Tower", "Electric Tower") );
		english.Add( ("Radar Tower", "Radar Tower") );
		english.Add( ("Sniper Tower", "Sniper Tower") );
		english.Add( ("Frost Tower", "Frost Tower") );

		//Descriptions
		english.Add( ("Pistol Tower Desc", "A simple shooting tower") );
		english.Add( ("SMG Tower Desc", "An automatic shooting tower") );
		english.Add( ("Explosive Tower Desc", "A tower that fires cannons") );
		english.Add( ("Electric Tower Desc", "An electrifying tower") );
		english.Add( ("Radar Tower Desc", "A radar tower that enhances towers and reveal cloaked hostiles") );
		english.Add( ("Sniper Tower Desc", "A long ranged tower") );
		english.Add( ("Frost Tower Desc", "A tower that slows targets") );

		//Misc
		english.Add( ("Info_OnUpgrade", "On Upgrade") );
		english.Add( ("Info_DMG", "Damage: ") );
		english.Add( ("Info_Cooldown", " | Cooldown: ") );
		english.Add( ("Info_Range", " | Range: ") );

		english.Add( ("Info_Upgrade", "Upgrades target tower") );
		english.Add( ("Info_Sell", "Sells target tower") );

		//NPCs
		english.Add( ("Peasant", "Peasant\nHP: ") );
		english.Add( ("Hidden", "Hidden\nHP: ") );
		english.Add( ("Zombie", "Zombie\nHP: ") );
		english.Add( ("Rebel", "Rebel\nHP: ") );
		english.Add( ("Rioter", "Rioter\nHP: ") );
		english.Add( ("Voidling", "Voidling\nHP: ") );
		english.Add( ("Brute", "Brute\nHP: ") );

		english.Add( ("Zombie Boss", "Zombie Boss\n HP: ") );
		english.Add( ("Void King", "Void King\n HP: ") );
	}

	public List<(string, string)> GetEnglish()
	{
		return english;
	}
}
