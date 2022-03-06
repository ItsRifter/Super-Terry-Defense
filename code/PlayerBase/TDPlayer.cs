using Sandbox;
using System;
using System.Linq;

partial class TDPlayer : Player
{
	public Clothing.Container Clothing = new();

	public TowerBase curTower;

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
			curTower.UpdatePreview( tr );

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
				}
			}
		}

		

		if ( Input.Pressed( InputButton.Slot0 ) && curTower != null )
		{
			curTower.DestoryPreview();
			curTower = null;
		}

		if (Input.Pressed(InputButton.Slot1) && curTower != null && curTower.GetType() != Library.Get<TowerBase>( "PistolTower" ) )
		{
			curTower.DestoryPreview();

			curTower = Library.Create<TowerBase>( "PistolTower" );
			Log.Info( "Assigned Pistol tower" );

			curTower.CreatePreviews( tr );
		} 
		else if(Input.Pressed(InputButton.Slot1) && curTower == null)
		{
			curTower = Library.Create<TowerBase>( "PistolTower" );
			Log.Info( "Assigned Pistol tower" );

			curTower.CreatePreviews(tr);
		}

		if ( Input.Pressed( InputButton.Slot2 ) && curTower != null && curTower.GetType() != Library.Get<TowerBase>( "SMGTower" ) )
		{
			curTower.DestoryPreview();

			curTower = Library.Create<TowerBase>( "SMGTower" );
			Log.Info( "Assigned SMG tower" );

			curTower.CreatePreviews( tr );
		}
		else if ( Input.Pressed( InputButton.Slot2 ) && curTower == null )
		{
			curTower = Library.Create<TowerBase>( "SMGTower" );
			Log.Info( "Assigned SMG tower" );

			curTower.CreatePreviews( tr );
		}

		if ( Input.Pressed( InputButton.Slot3 ) && curTower != null && curTower.GetType() != Library.Get<TowerBase>( "ExplosiveTower" ) )
		{
			curTower.DestoryPreview();

			curTower = Library.Create<TowerBase>( "ExplosiveTower" );
			Log.Info( "Assigned Explosive tower" );

			curTower.CreatePreviews( tr );
		}
		else if ( Input.Pressed( InputButton.Slot3 ) && curTower == null )
		{
			curTower = Library.Create<TowerBase>( "ExplosiveTower" );
			Log.Info( "Assigned Explosive tower" );

			curTower.CreatePreviews( tr );
		}

		if(Input.Pressed(InputButton.Slot9))
		{
			if ( curTower != null )
			{
				curTower.DestoryPreview();
				curTower.Delete();
			}
		}

	}
}
