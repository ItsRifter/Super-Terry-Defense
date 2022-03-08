using System;
using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;

public partial class TowerInfoPanel : Panel
{
	public Panel TowerPanel;
	public Label TowerName;
	public Label TowerDesc;
	public Label TowerLevel;
	public Label TowerInfo;
	public Label TowerUpgradeInfo;

	public TowerInfoPanel()
	{
		StyleSheet.Load( "UI/TowerInfoPanel.scss" );
		TowerPanel = Add.Panel( "panelTower" );

		TowerName = TowerPanel.Add.Label( "?", "title" );
		TowerDesc = TowerPanel.Add.Label( "?", "desc" );
		TowerLevel = TowerPanel.Add.Label( "?", "level" );
		TowerInfo = TowerPanel.Add.Label( "?", "info" );
		TowerUpgradeInfo = TowerPanel.Add.Label( "Upon Upgrade: ", "upgradeInfo" );

	}

	public override void Tick()
	{
		base.Tick();

		if(Local.Pawn is TDPlayer player)
		{
			var tr = Trace.Ray( player.EyePosition, player.EyePosition + player.EyeRotation.Forward * 150 )
				.Ignore( player )
				.WithTag( "Tower" )
				.Size( 2 )
				.Run();

			SetClass( "isHovering", tr.Entity is TowerBase && tr.Entity.Owner == player );

			if ( tr.Entity is TowerBase tower && tr.Entity is not Castle)
			{
				TowerName.SetText( Translation.Translate( tower.TowerName) );
				TowerDesc.SetText( Translation.Translate( tower.TowerName + " Desc") );
				TowerLevel.SetText( Translation.Translate( "Tower_Level" ) + tower.CurTier );
				TowerInfo.SetText( Translation.Translate( "Info_DMG" ) + tower.AttackDamage +
					Translation.Translate( "Info_Cooldown" ) + Math.Round(tower.AttackCooldown, 2) 
					+ Translation.Translate( "Info_Range" ) + tower.AttackRange );
				TowerUpgradeInfo.SetText( "Upon Upgrade: " + tower.UpgradeDesc[tower.CurTier] + "\nUpgrade Cost: " + tower.UpgradeCosts[tower.CurTier] );
			} 
			else if ( tr.Entity is Castle castle )
			{
				TowerName.SetText( castle.TowerName );
				TowerDesc.SetText( castle.TowerDesc );
				TowerLevel.SetText( "Health: " + castle.Health );
			}
		}
	}
}
