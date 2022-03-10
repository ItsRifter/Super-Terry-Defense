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

		HostPnl = Add.Panel("menu");
		Label hostTitle = HostPnl.Add.Label("Host Setup", "titleText");
		Label hostModes = hostTitle.Add.Label( "Select Gamemode", "selectText" );

		CompPnl = HostPnl.Add.Panel( "compPnl" );

		Panel compLbl = CompPnl.Add.Label( "Competitive", "compText" );

		CompPnl.AddEventListener( "onclick", () =>
		{
			ConsoleSystem.Run( "Test" );
			Event.Run( "td_setgame_comp" );
		} );

		CoopPnl = HostPnl.Add.Panel( "coopPnl" );

		Panel coopLbl = CoopPnl.Add.Label( "Cooperative", "coopText" );

		CoopPnl.AddEventListener( "onclick", () =>
		{
			Event.Run( "td_setgame_coop" );
		} );
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

