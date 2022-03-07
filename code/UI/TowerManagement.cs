using System;
using System.Collections.Generic;
using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;

public partial class TowerManagement : Panel
{
	readonly List<TowerIcon> slots = new();
	private int iconIndex;
	public TowerManagement()
	{
		iconIndex = 10;

		for ( int i = 0; i < 10; i++ )
		{
			var icon = new TowerIcon( i + 1, this );
			slots.Add( icon );
		}
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
		var player = Local.Pawn as TDPlayer;

		if ( player == null )
			return;

		iconIndex = i;
	}
}
