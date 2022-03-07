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
		base.Spawn();

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

		base.Respawn();
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

		var tr = Trace.Ray( EyePosition, EyePosition + EyeRotation.Forward * 150 )
				.Ignore( this )
				.Ignore( curTower )
				.Size( 2 )
				.Run();

		if ( curTower != null )
			curTower.UpdatePreview( tr, this );

		if (inSellMode)
		{
			if ( tr.Entity is TowerBase tower && tr.Entity is not Castle )
			{
				if ( Input.Pressed( InputButton.Attack1 ) && tower.Owner == this )
					tower.SellTower( this );
			}

		} else if (inUpgradeMode)
		{
			if ( tr.Entity is TowerBase tower && tr.Entity is not Castle )
			{
				if ( Input.Pressed( InputButton.Attack1 ) )
				{
					Log.Info( tower.CanUpgrade( this ) );

					if ( tower.CanUpgrade( this ) )
						tower.UpgradeTower( this );
				}

			}
		}

		var nearbyTowers = FindInSphere( tr.EndPosition, 28 );
		bool badSpot = false;

		foreach ( var nearby in nearbyTowers )
		{
			if ( nearby is TowerBase || tr.Entity is TowerBlocker )
				badSpot = true;
		}

		if (Input.Pressed(InputButton.Attack1) && curTower != null )
		{
			if ( tr.Entity != null && tr.Entity.GetType().ToString().Contains( "WorldEntity" ) && !badSpot )
			{
				if(tr.Normal.z == 1 && curTower.CanAfford(this))
				{
					var newTower = Library.Create<TowerBase>( curTower.GetType().FullName );
					newTower.Position = tr.EndPosition;
					CurMoney -= curTower.Cost;
					newTower.Owner = this;

					newTower.CreateClientPanel(To.Single(this), newTower);

				}
			}
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

		if (Input.Pressed(InputButton.Slot1) && curTower != null && curTower.GetType() != Library.Get<TowerBase>( "PistolTower" ) )
		{
			curTower.DestoryPreview();

			curTower = Library.Create<TowerBase>( "PistolTower" );

			inUpgradeMode = false;
			inSellMode = false;
			curTower.CreatePreviews( tr );
		} 
		else if(Input.Pressed(InputButton.Slot1) && curTower == null)
		{
			curTower = Library.Create<TowerBase>( "PistolTower" );

			inUpgradeMode = false;
			inSellMode = false;
			curTower.CreatePreviews(tr);
		}

		if ( Input.Pressed( InputButton.Slot2 ) && curTower != null && curTower.GetType() != Library.Get<TowerBase>( "SMGTower" ) )
		{
			curTower.DestoryPreview();

			curTower = Library.Create<TowerBase>( "SMGTower" );

			inUpgradeMode = false;
			inSellMode = false;
			curTower.CreatePreviews( tr );
		}
		else if ( Input.Pressed( InputButton.Slot2 ) && curTower == null )
		{
			curTower = Library.Create<TowerBase>( "SMGTower" );

			inUpgradeMode = false;
			inSellMode = false;
			curTower.CreatePreviews( tr );
		}

		if ( Input.Pressed( InputButton.Slot3 ) && curTower != null && curTower.GetType() != Library.Get<TowerBase>( "ExplosiveTower" ) )
		{
			curTower.DestoryPreview();

			curTower = Library.Create<TowerBase>( "ExplosiveTower" );

			inUpgradeMode = false;
			inSellMode = false;
			curTower.CreatePreviews( tr );
		}
		else if ( Input.Pressed( InputButton.Slot3 ) && curTower == null )
		{
			curTower = Library.Create<TowerBase>( "ExplosiveTower" );

			inUpgradeMode = false;
			inSellMode = false;
			curTower.CreatePreviews( tr );
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

		if (Input.Pressed(InputButton.Slot9) && !inSellMode )
		{
			if ( curTower != null )
			{
				curTower.DestoryPreview();
				curTower = null;
			}


			inUpgradeMode = false;
			inSellMode = true;
		}
		if( curTower != null )
			curTower.Owner = this;
	}
}
