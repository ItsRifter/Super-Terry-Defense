using Sandbox;
using System;
using System.Linq;

partial class TDPlayer : Player
{

	public Clothing.Container Clothing = new();

	public TowerBase curTower;

	private bool inUpgradeMode;
	private bool inSellMode;
	public TDPlayer()
	{
		
	}

	public TDPlayer( Client cl ) : this()
	{
		Clothing.LoadFromClient( cl );
	}

	public override void Spawn()
	{
		base.Respawn();

		SetModel( "models/citizen/citizen.vmdl" );

		Controller = new WalkController();
		Animator = new StandardPlayerAnimator();
		CameraMode = new FirstPersonCamera();

		EnableDrawing = true;
		EnableHideInFirstPerson = true;
		EnableShadowInFirstPerson = true;
		EnableAllCollisions = false;

		Clothing.DressEntity( this );

		inUpgradeMode = false;
		inSellMode = false;
	}

	public override void Simulate( Client cl )
	{
		SimulateActiveChild( cl, ActiveChild );

		if (TDGame.Current.CurGameStatus == TDGame.GameStatus.Active)
			DoTowerInputs();

		TickPlayerUse();

		EyeRotation = Rotation;

		base.Simulate( cl );
	}

	public override void FrameSimulate( Client cl )
	{
		EyeRotation = Rotation;

		base.FrameSimulate( cl );
	}

	public void DoTowerInputs()
	{
		if ( IsClient )
			return;

		SelectingTowers();

		if ( !inSellMode && !inUpgradeMode )
			return;

		var targetTr = Trace.Ray( EyePosition, EyePosition + EyeRotation.Forward * 150 )
			.Ignore( this )
			.Size( 2 )
			.Run();

		if (inSellMode)
		{
			if ( targetTr.Entity is TowerBase tower && targetTr.Entity is not Castle )
			{
				if ( Input.Pressed( InputButton.Attack1 ) && tower.Owner == this )
					tower.SellTower( this );
			}

		} else if (inUpgradeMode)
		{
			if ( targetTr.Entity is TowerBase tower && targetTr.Entity is not Castle )
			{
				if ( Input.Pressed( InputButton.Attack1 ) )
				{
					if ( tower.CanUpgrade( this ) )
						tower.UpgradeTower( this );
				}

			}
		}
	}

	private void SelectingTowers()
	{
		var tr = Trace.Ray( EyePosition, EyePosition + EyeRotation.Forward * 150 )
				.Ignore( this )
				.Ignore( curTower )
				.Size( 2 )
				.Run();

		if ( curTower != null )
			curTower.UpdatePreview( tr, this );

		var nearbyTowers = FindInSphere( tr.EndPosition, 28 );
		bool badSpot = false;

		foreach ( var nearby in nearbyTowers )
		{
			if ( nearby is TowerBase || tr.Entity is TowerBlocker )
				badSpot = true;
		}

		if ( Input.Pressed( InputButton.Attack1 ) && curTower != null )
		{
			if ( tr.Entity != null && tr.Entity.GetType().ToString().Contains( "WorldEntity" ) && !badSpot )
			{
				if ( tr.Normal.z == 1 && curTower.CanAfford( this ) )
				{
					var newTower = Library.Create<TowerBase>( curTower.GetType().FullName );
					newTower.Position = tr.EndPosition;
					CurMoney -= curTower.Cost;
					newTower.Owner = this;

					newTower.CreateClientPanel( To.Everyone, newTower );

				}
			}
		}

		//Pistol
		if ( Input.Pressed( InputButton.Slot1 ) && curTower != null && curTower.GetType() != Library.Get<TowerBase>( "PistolTower" ) )
		{
			curTower.DestoryPreview();

			curTower = Library.Create<TowerBase>( "PistolTower" );

			inUpgradeMode = false;
			inSellMode = false;
			curTower.CreatePreviews( tr, this );
		}
		else if ( Input.Pressed( InputButton.Slot1 ) && curTower == null )
		{
			curTower = Library.Create<TowerBase>( "PistolTower" );

			inUpgradeMode = false;
			inSellMode = false;
			curTower.CreatePreviews( tr, this );
		}

		//SMG
		if ( Input.Pressed( InputButton.Slot2 ) && curTower != null && curTower.GetType() != Library.Get<TowerBase>( "SMGTower" ) )
		{
			curTower.DestoryPreview();

			curTower = Library.Create<TowerBase>( "SMGTower" );

			inUpgradeMode = false;
			inSellMode = false;
			curTower.CreatePreviews( tr, this );
		}
		else if ( Input.Pressed( InputButton.Slot2 ) && curTower == null )
		{
			curTower = Library.Create<TowerBase>( "SMGTower" );

			inUpgradeMode = false;
			inSellMode = false;
			curTower.CreatePreviews( tr, this );
		}

		//Explosive
		if ( Input.Pressed( InputButton.Slot3 ) && curTower != null && curTower.GetType() != Library.Get<TowerBase>( "ExplosiveTower" ) )
		{
			curTower.DestoryPreview();

			curTower = Library.Create<TowerBase>( "ExplosiveTower" );

			inUpgradeMode = false;
			inSellMode = false;
			curTower.CreatePreviews( tr, this );
		}
		else if ( Input.Pressed( InputButton.Slot3 ) && curTower == null )
		{
			curTower = Library.Create<TowerBase>( "ExplosiveTower" );

			inUpgradeMode = false;
			inSellMode = false;
			curTower.CreatePreviews( tr, this );
		}

		//Electric
		if ( Input.Pressed( InputButton.Slot4 ) && curTower != null && curTower.GetType() != Library.Get<TowerBase>( "ElectricTower" ) )
		{
			curTower.DestoryPreview();

			curTower = Library.Create<TowerBase>( "ElectricTower" );

			inUpgradeMode = false;
			inSellMode = false;
			curTower.CreatePreviews( tr, this );
		}
		else if ( Input.Pressed( InputButton.Slot4 ) && curTower == null )
		{
			curTower = Library.Create<TowerBase>( "ElectricTower" );

			inUpgradeMode = false;
			inSellMode = false;
			curTower.CreatePreviews( tr, this );
		}

		//Radar
		if ( Input.Pressed( InputButton.Slot5 ) && curTower != null && curTower.GetType() != Library.Get<TowerBase>( "RadarTower" ) )
		{
			curTower.DestoryPreview();

			curTower = Library.Create<TowerBase>( "RadarTower" );

			inUpgradeMode = false;
			inSellMode = false;
			curTower.CreatePreviews( tr, this );
		}
		else if ( Input.Pressed( InputButton.Slot5 ) && curTower == null )
		{
			curTower = Library.Create<TowerBase>( "RadarTower" );

			inUpgradeMode = false;
			inSellMode = false;
			curTower.CreatePreviews( tr, this );
		}

		//Sniper
		if ( Input.Pressed( InputButton.Slot6 ) && curTower != null && curTower.GetType() != Library.Get<TowerBase>( "SniperTower" ) )
		{
			curTower.DestoryPreview();

			curTower = Library.Create<TowerBase>( "SniperTower" );

			inUpgradeMode = false;
			inSellMode = false;
			curTower.CreatePreviews( tr, this );
		}
		else if ( Input.Pressed( InputButton.Slot6 ) && curTower == null )
		{
			curTower = Library.Create<TowerBase>( "SniperTower" );

			inUpgradeMode = false;
			inSellMode = false;
			curTower.CreatePreviews( tr, this );
		}

		//Frost
		if ( Input.Pressed( InputButton.Slot7 ) && curTower != null && curTower.GetType() != Library.Get<TowerBase>( "FrostTower" ) )
		{
			curTower.DestoryPreview();

			curTower = Library.Create<TowerBase>( "FrostTower" );

			inUpgradeMode = false;
			inSellMode = false;
			curTower.CreatePreviews( tr, this );
		}
		else if ( Input.Pressed( InputButton.Slot7 ) && curTower == null )
		{
			curTower = Library.Create<TowerBase>( "FrostTower" );

			inUpgradeMode = false;
			inSellMode = false;
			curTower.CreatePreviews( tr, this );
		}

		if ( Input.Pressed( InputButton.Slot8 ) && !inUpgradeMode )
		{
			if ( curTower != null )
			{
				curTower.DestoryPreview();
				curTower = null;
			}

			inSellMode = false;
			inUpgradeMode = true;
		}

		if ( Input.Pressed( InputButton.Slot9 ) && !inSellMode )
		{
			if ( curTower != null )
			{
				curTower.DestoryPreview();
				curTower = null;
			}

			inUpgradeMode = false;
			inSellMode = true;
		}

		if ( Input.Pressed( InputButton.Slot0 ) )
		{
			if ( curTower != null )
			{
				curTower.DestoryPreview();
				curTower = null;
			}

			inSellMode = false;
			inUpgradeMode = false;
		}

		if ( curTower != null )
			curTower.Owner = this;
	}
}
