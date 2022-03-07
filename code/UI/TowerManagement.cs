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

		if ( input.Pressed( InputButton.Slot1 ) ) SetActiveSlot( 1 );
		if ( input.Pressed( InputButton.Slot2 ) ) SetActiveSlot( 2 );
		if ( input.Pressed( InputButton.Slot3 ) ) SetActiveSlot( 3 );
		if ( input.Pressed( InputButton.Slot4 ) ) SetActiveSlot( 4 );
		if ( input.Pressed( InputButton.Slot5 ) ) SetActiveSlot( 5 );
		if ( input.Pressed( InputButton.Slot6 ) ) SetActiveSlot( 6 );
		if ( input.Pressed( InputButton.Slot7 ) ) SetActiveSlot( 7 );
		if ( input.Pressed( InputButton.Slot8 ) ) SetActiveSlot( 8 );
		if ( input.Pressed( InputButton.Slot9 ) ) SetActiveSlot( 9 );
		if ( input.Pressed( InputButton.Slot0 ) ) SetActiveSlot( 10 );
	}

	private void SetActiveSlot(int i)
	{
		iconIndex = i;

		TowerPanel.SetClass( "hide", iconIndex == 10 );

		switch ( iconIndex )
		{
			case 1:
				TowerInfo.SetText( "Pistol Tower\nCosts 10 Coins" );
				break;
			case 2:
				TowerInfo.SetText( "SMG Tower\nCosts 25 Coins" );
				break;
			case 3:
				TowerInfo.SetText( "Cannon Tower\nCosts 40 Coins" );
				break;
			case 4:
				TowerInfo.SetText( "Electric Tower\nCosts 65 Coins" );
				break;
			case 5:
				TowerInfo.SetText( "Radar Tower\nCosts 50 Coins" );
				break;
			case 6:
				TowerInfo.SetText( "Sniper Tower\nCosts 75 Coins" );
				break;
			case 7:
				TowerInfo.SetText( "Get rifter to add a new unique tower" );
				break;
			case 8:
				TowerInfo.SetText( "Upgrades target tower" );
				break;
			case 9:
				TowerInfo.SetText( "Sells target tower" );
				break;
		}	
	}
}
