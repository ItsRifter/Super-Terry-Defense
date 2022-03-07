using System;
using System.Linq;
using System.Collections.Generic;
using Sandbox;

public partial class TDNPCBase : AnimEntity
{
	//Basics
	public virtual string NPCName => "Default NPC";
	public virtual string BaseModel => "models/citizen/citizen.vmdl";
	public virtual int BaseHealth => 1;
	public virtual float BaseSpeed => 1;
	public virtual int minCash => 1;
	public virtual int maxCash => 2;
	public virtual float NPCScale => 1;
	public virtual float CastleDamage => 1;

	public enum SpecialType
	{
		Standard,
		Armoured,
		Cloaked
	}

	public virtual SpecialType NPCType => SpecialType.Standard;

	private int cashReward = 0;

	[ConVar.Replicated]
	public static bool td_npc_drawoverlay { get; set; }
	public NPCDebugDraw Draw => NPCDebugDraw.Once;

	Vector3 InputVelocity;

	Vector3 LookDir;

	private Entity targetCastle;

	NPCNavigation Path = new NPCNavigation();
	public NPCSteering Steer;

	public override void Spawn()
	{ 
		SetModel( BaseModel );

		Scale = NPCScale;
		Health = BaseHealth;

		EnableHitboxes = true;

		cashReward = Rand.Int( minCash, maxCash );

		Tags.Add( "NPC" );

		SetBodyGroup( 1, 0 );

		Steer = new NPCSteering();
	}

	public void Despawn()
	{
		DamageInfo dmgInfo = new DamageInfo();
		dmgInfo.Damage = CastleDamage;

		targetCastle.TakeDamage( dmgInfo );

		if ( targetCastle.Health <= 0 )
			targetCastle = null;

		Event.Run( "td_npckilled", this );
		EnableDrawing = false;
		Delete();
	}

	[Event.Tick.Server]
	public void Tick()
	{
		InputVelocity = 0;

		if ( Steer != null )
		{
			Steer.Tick( Position );

			if ( !Steer.Output.Finished )
			{
				InputVelocity = Steer.Output.Direction.Normal;
				Velocity = Velocity.AddClamped( InputVelocity * Time.Delta * 500, BaseSpeed );
			}

			if ( TDGame.Current.CurGameStatus == TDGame.GameStatus.Active )
			{
				if ( targetCastle == null )
				{
					foreach ( var ent in All )
						if ( ent is Castle )
							targetCastle = ent;
				}

				Steer.Target = targetCastle.Position;
			}

			if( td_npc_drawoverlay )
				DebugOverlay.Sphere( Position + Vector3.Down * 8, 42, Color.Red );

			foreach(var ent in FindInSphere(Position + Vector3.Down * 8, 42))
			{
				if ( ent is Castle )
					Despawn();
			}
		}

		Move( Time.Delta );

		var walkVelocity = Velocity.WithZ( 0 );
		if ( walkVelocity.Length > 0.5f )
		{
			var turnSpeed = walkVelocity.Length.LerpInverse( 0, 100, true );
			var targetRotation = Rotation.LookAt( walkVelocity.Normal, Vector3.Up );
			Rotation = Rotation.Lerp( Rotation, targetRotation, turnSpeed * Time.Delta * 20.0f );
		}

		var animHelper = new CitizenAnimationHelper( this );

		LookDir = Vector3.Lerp( LookDir, InputVelocity.WithZ( 0 ) * 1000, Time.Delta * 100.0f );
		animHelper.WithLookAt( EyePosition + LookDir );
		animHelper.WithVelocity( Velocity );
		animHelper.WithWishVelocity( InputVelocity );

		
	}

	protected virtual void Move( float timeDelta )
	{
		var bbox = BBox.FromHeightAndRadius( 64, 4 );

		MoveHelper move = new( Position, Velocity );
		move.MaxStandableAngle = 50;
		move.Trace = move.Trace.Ignore( this ).Size( bbox );

		if ( !Velocity.IsNearlyZero( 0.001f ) )
		{
			move.TryUnstuck();
			move.TryMoveWithStep( timeDelta, 30 );
		}

		var tr = move.TraceDirection( Vector3.Down * 10.0f );

		if ( move.IsFloor( tr ) )
		{
			GroundEntity = tr.Entity;

			if ( !tr.StartedSolid )
			{
				move.Position = tr.EndPosition;
			}

			if ( InputVelocity.Length > 0 )
			{
				var movement = move.Velocity.Dot( InputVelocity.Normal );
				move.Velocity = move.Velocity - movement * InputVelocity.Normal;
				move.ApplyFriction( tr.Surface.Friction * 10.0f, timeDelta );
				move.Velocity += movement * InputVelocity.Normal;

				NPCDebugDraw.Once.Line( tr.StartPosition, tr.EndPosition );

			}
			else
			{
				move.ApplyFriction( tr.Surface.Friction * 10.0f, timeDelta );
			}

			
		}
		else
		{
			GroundEntity = null;
			move.Velocity += Vector3.Down * 900 * timeDelta;
			NPCDebugDraw.Once.WithColor( Color.Red ).Circle( Position, Vector3.Up, 10.0f );
		}

		Position = move.Position;
		Velocity = move.Velocity;
	}

	public override void Simulate( Client cl )
	{
		base.Simulate( cl );

		Rotation = Input.Rotation;
		EyeRotation = Rotation;

		var maxSpeed = 500;

		Velocity += Input.Rotation * new Vector3( Input.Forward, Input.Left, Input.Up ) * maxSpeed * 5 * Time.Delta;
		if ( Velocity.Length > maxSpeed ) Velocity = Velocity.Normal * maxSpeed;

		Velocity = Velocity.Approach( 0, Time.Delta * maxSpeed * 3 );

		Position += Velocity * Time.Delta;

		EyePosition = Position;
	}

	public override void TakeDamage( DamageInfo info )
	{
		Health -= info.Damage;

		if ( Health <= 0 )
		{
			OnKilled();
		}
	}

	public override void OnKilled()
	{
		foreach ( var client in Client.All)
		{
			if(client.Pawn is TDPlayer player)
				player.AddMoney( cashReward );
		}

		Event.Run( "td_npckilled", this );
		base.OnKilled();
	}

	public override void FrameSimulate( Client cl )
	{
		base.FrameSimulate( cl );

		Rotation = Input.Rotation;
		EyeRotation = Rotation;
		Position += Velocity * Time.Delta;
	}
}
