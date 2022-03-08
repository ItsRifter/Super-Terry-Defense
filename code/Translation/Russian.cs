using System;
using System.Collections.Generic;
using Sandbox;

public partial class Russian
{
	public List<(string, string)> russian;
	public Russian()
	{
		russian = new List<(string, string)>();

		russian.Add( ("Stat_Coins", "Монет: ") );

		russian.Add( ("Tower_Level", "Уровень: ") );

		russian.Add( ("Current_Wave", "Волна: ") );
		russian.Add( ("Timer_Game", "Начинаем игру через ") );
		russian.Add( ("Timer_Wave", "След. Волна через ") );
		russian.Add( ("Game_Finished", "Игра Окончена!") );

		//Info on the hotbar
		russian.Add( ("Tower_Pistol_Info", "Пистолетная Башня\nСтоит 10 Монет") );
		russian.Add( ("Tower_SMG_Info", "SMG Tower\nСтоит 25 Монет") );
		russian.Add( ("Tower_Explosive_Info", "Cannon Tower\nСтоит 40 Монет") );
		russian.Add( ("Tower_Electric_Info", "Electric Tower\nСтоит 65 Монет") );
		russian.Add( ("Tower_Radar_Info", "Radar Tower\nСтоит 50 Монет") );
		russian.Add( ("Tower_Sniper_Info", "Sniper Tower\nСтоит 75 Монет") );
		russian.Add( ("Tower_Frost_Info", "Frost Tower\nСтоит 45 Монет") );

		//Info when hovering on a tower
		//Names
		russian.Add( ("Pistol Tower", "Пистолетная Башня") );
		russian.Add( ("SMG Tower", "Автоматическая Башня") );
		russian.Add( ("Explosive Tower", "Разрывная Башня") );
		russian.Add( ("Electric Tower", "Башня-Тесла") );
		russian.Add( ("Radar Tower", "Башня-Радар") );
		russian.Add( ("Sniper Tower", "Дальнобойная Башня") );
		russian.Add( ("Frost Tower", "Ледяная Башня") );

		//Descriptions
		russian.Add( ("Pistol Tower Desc", "Довольно простенькая...") );
		russian.Add( ("SMG Tower Desc", "Автоматическая.") );
		russian.Add( ("Explosive Tower Desc", "Снаряды взрываются при попадании") );
		russian.Add( ("Electric Tower Desc", "Башня-Заппер") );
		russian.Add( ("Radar Tower Desc", "Раскрывает врагов в радиусе всем башням") );
		russian.Add( ("Sniper Tower Desc", "Стреляет дальше всех") );
		russian.Add( ("Frost Tower Desc", "Замедляет врагов") );

		//Misc
		russian.Add( ("Info_OnUpgrade", "При улучшении") );
		russian.Add( ("Info_DMG", "Урон: ") );
		russian.Add( ("Info_Cooldown", " | Перезарядка: ") );
		russian.Add( ("Info_Range", " | Дальность: ") );

		russian.Add( ("Info_Upgrade", "Улучшает выбранную башню") );
		russian.Add( ("Info_Sell", "Продаёт выбранную башню") );

		//NPCs
		russian.Add( ("Peasant", "Крестьянин\nХП: ") );
		russian.Add( ("Hidden", "Скрытный\nХП: ") );
		russian.Add( ("Zombie", "Зомби\nХП: ") );
		russian.Add( ("Rebel", "Повстанец\nХП: ") );
		russian.Add( ("Rioter", "Мятежник\nХП: ") );
		russian.Add( ("Voidling", "Войдлинг\nХП: ") );
		russian.Add( ("Brute", "Зверь\nХП: ") );

		russian.Add( ("ZombieBoss", "Босс Зомби\n ХП: ") );
		russian.Add( ("VoidKing", "Void King\n ХП: ") );
	}

	public List<(string, string)> GetRussian()
	{
		return russian;
	}
}
