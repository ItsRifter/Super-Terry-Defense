using System;
using System.Collections.Generic;
using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;

public partial class TowerManagement : Panel
{
	readonly List<TowerIcon> slots = new();
	private int iconIndex;

	public Panel TowerPanel;
	public Label TowerInfo;

	public TowerManagement()
	{
		iconIndex = 10;

		for ( int i = 0; i < 10; i++ )
		{
			var icon = new TowerIcon( i + 1, this );
			slots.Add( icon );
		}

		TowerPanel = Add.Panel("panel");
		TowerInfo = TowerPanel.Add.Label( "???", "pnlInfo" );

		TowerPanel.SetClass( "hide", true );
	}

	public override void Tick()
	{
		base.Tick();

		var player = Local.Pawn as TDPlayer;
		if ( player == null ) return;

		for ( int i = 0; i < slots.Count; i++ )
		{
			UpdateIcon( i, slots[i].IconImage, slots[i].IconKey);
		}
	}

	private void UpdateIcon( int curIndex, Image towerIcon, Label towerKey)
	{
		if ( TDGame.Current.CurGameStatus != TDGame.GameStatus.Active )
		{
			towerIcon.SetClass( "hide", TDGame.Current.CurGameStatus != TDGame.GameStatus.Active );
			towerKey.SetClass( "hide", TDGame.Current.CurGameStatus != TDGame.GameStatus.Active );
			return;
		} 
			else
		{
			towerIcon.SetClass( "hide", false );
			towerKey.SetClass( "hide", false );
		}

		towerIcon.SetClass( "active", iconIndex == (curIndex + 1) );
		towerKey.SetClass( "active", iconIndex == (curIndex + 1) );

	}

	[Event( "buildinput" )]
	public void ProcessClientInput( InputBuilder input )
	{
		if ( TDGame.Current.CurGameStatus != TDGame.GameStatus.Active )
			return;

		var player = Local.Pawn as TDPlayer;
		if ( player == null )
			return;

		if ( input.Pressed( InputButton.Slot1 ) ) SetActiveSlot( player, 1 );
		if ( input.Pressed( InputButton.Slot2 ) ) SetActiveSlot( player, 2 );
		if ( input.Pressed( InputButton.Slot3 ) ) SetActiveSlot( player, 3 );
		if ( input.Pressed( InputButton.Slot4 ) ) SetActiveSlot( player, 4 );
		if ( input.Pressed( InputButton.Slot5 ) ) SetActiveSlot( player, 5 );
		if ( input.Pressed( InputButton.Slot6 ) ) SetActiveSlot( player, 6 );
		if ( input.Pressed( InputButton.Slot7 ) ) SetActiveSlot( player, 7 );
		if ( input.Pressed( InputButton.Slot8 ) ) SetActiveSlot( player, 8 );
		if ( input.Pressed( InputButton.Slot9 ) ) SetActiveSlot( player, 9 );
		if ( input.Pressed( InputButton.Slot0 ) ) SetActiveSlot( player, 10 );
	}

	private void SetActiveSlot(TDPlayer player, int i)
	{
		iconIndex = i;

		TowerPanel.SetClass( "hide", iconIndex == 10 );

		switch ( iconIndex )
		{
			case 1:
				TowerInfo.SetText( player.Translate(ConsoleSystem.GetValue("td_currentlanguage"), "Tower_Pistol_Info" ) );
				break;
			case 2:
				TowerInfo.SetText( player.Translate( ConsoleSystem.GetValue( "td_currentlanguage" ), "Tower_SMG_Info" ) );
				break;
			case 3:
				TowerInfo.SetText( player.Translate( ConsoleSystem.GetValue( "td_currentlanguage" ), "Tower_Explosive_Info" ) );
				break;
			case 4:
				TowerInfo.SetText( player.Translate( ConsoleSystem.GetValue( "td_currentlanguage" ), "Tower_Electric_Info" ) );
				break;
			case 5:
				TowerInfo.SetText( player.Translate( ConsoleSystem.GetValue( "td_currentlanguage" ), "Tower_Radar_Info" ) );
				break;
			case 6:
				TowerInfo.SetText( player.Translate( ConsoleSystem.GetValue( "td_currentlanguage" ), "Tower_Sniper_Info" ) );
				break;
			case 7:
				TowerInfo.SetText( player.Translate( ConsoleSystem.GetValue( "td_currentlanguage" ), "Tower_Frost_Info" ) );
				break;
			case 8:
				TowerInfo.SetText( player.Translate( ConsoleSystem.GetValue( "td_currentlanguage" ), "Info_Upgrade" ) );
				break;
			case 9:
				TowerInfo.SetText( player.Translate( ConsoleSystem.GetValue( "td_currentlanguage" ), "Info_Sell" ) );
				break;
		}	
	}
}
