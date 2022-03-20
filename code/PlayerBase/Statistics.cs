using System;
using Sandbox;

public partial class TDPlayer
{
	[Net]
	public int CurMoney { get; private set; }

	public void InitStats()
	{
		CurMoney = 20 + (20 * TDGame.Current.GetDifficulty());
	}

	[Event("td_evnt_givemoney")]
	public void AddMoney(int addMoney, TDPlayer target = null)
	{
		if ( target != null && target != this )
			target.CurMoney += addMoney;
		else
			CurMoney += addMoney;
	}

	public void TakeMoney(int subMoney)
	{
		CurMoney -= subMoney;
	}

	[AdminCmd("td_givemoney")]
	public static void AdminMoney(int amount, string target = null)
	{
		if ( target == null )
		{
			var player = ConsoleSystem.Caller.Pawn as TDPlayer;
			Event.Run( "td_evnt_givemoney", amount, player );
			Log.Info( "Gave self " + amount + " coins");
		}
		else
		{
			foreach ( var client in Client.All )
			{
				if ( client.Name.ToLower() == target.ToLower() )
				{
					var player = client.Pawn as TDPlayer;

					if ( player != null )
					{
						Event.Run( "td_evnt_givemoney", amount, player );
						Log.Info( "Gave " + client.Name + " " + amount + " coins" );
					}
				}
			}
		}
	}
}
