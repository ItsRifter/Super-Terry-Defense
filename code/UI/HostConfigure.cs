using System;
using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;

public partial class HostConfigure : Panel
{
	public Panel HostPnl;
	public Panel CoopPnl;
	public Panel CompPnl;

	public HostConfigure()
	{
		StyleSheet.Load( "UI/HostConfigure.scss" );
		
		HostPnl = Add.Panel( "menu" );

		CompPnl = HostPnl.Add.Panel( "compPnl" );

		CompPnl.Add.Label( "Competitive", "compText" );

		CompPnl.AddEventListener( "onclick", () =>
		{
			SetGameType( TDGame.GamemodeType.Competitive );
		} );

		//

		Label hostTitle = HostPnl.Add.Label("Host Setup", "titleText");
		hostTitle.Add.Label( "Select Gamemode", "selectText" );

		//

		CoopPnl = HostPnl.Add.Panel( "coopPnl" );

		CoopPnl.Add.Label( "Cooperative", "coopText" );

		CoopPnl.AddEventListener( "onclick", () =>
		{
			SetGameType( TDGame.GamemodeType.Cooperative );
		} );
	}

	public void SetGameType(TDGame.GamemodeType gameType)
	{
		ConsoleSystem.Run( "td_start_gametype", gameType );
	}

	public override void Tick()
	{
		base.Tick();
		
		if( Local.Client.IsListenServerHost )
		{
			SetClass( "active", TDGame.Current.GameType == TDGame.GamemodeType.Unspecified );
		}
			
	}
}

