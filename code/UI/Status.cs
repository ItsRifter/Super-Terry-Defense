using System;
using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;

public partial class Status : Panel
{
	public Label curCashLbl;

	public Status()
	{
		StyleSheet.Load( "UI/Status.scss" );

		curCashLbl = Add.Label("");
	}

	public override void Tick()
	{
		base.Tick();

		if ( TDGame.Current.CurGameStatus == TDGame.GameStatus.Idle )
			return;

		if(Local.Pawn is TDPlayer player)
		{
			curCashLbl.SetText( player.Translate(ConsoleSystem.GetValue( "td_currentlanguage" ), "Stat_Coins" ) + player.CurMoney );
		}
	}
}
