using System;
using Sandbox;

public partial class TowerBase : AnimEntity
{
	public virtual string TowerName => "Base Tower";
	public virtual string TowerDesc => "A base tower to derive towers from";
	public virtual string TowerModel => "models/citizen_props/crate01.vmdl";
	public virtual int AttackDamage => 1;
	public virtual float AttackCooldown => 2;
	public virtual int AttackRange => 1;
	public virtual int Cost => 1;
	public virtual int Level => 0;
	public virtual int MaxTier => 2;

	private int CurTier = 0;

	private TowerBase previewModel;

	private TimeSince lastAttack;

	public virtual int[] UpgradeCosts => new[] { 2, 3, 4, 5 };

	[ConVar.Replicated]
	public static bool td_tower_drawoverlay { get; set; }

	public override void Spawn()
	{
		base.Spawn();

		SetModel( TowerModel );
		CollisionGroup = CollisionGroup.Debris;
	}

	[Event.Tick.Server]
	public virtual void SimulateTower()
	{
		var ents = FindInSphere( Position, AttackRange );

		foreach ( var ent in ents )
		{
			if(ent is TDNPCBase npc)
			{
				if( lastAttack > AttackCooldown )
				{
					DamageInfo dmgInfo = new DamageInfo();
					dmgInfo.Damage = AttackDamage;

					npc.TakeDamage( dmgInfo );

					lastAttack = 0;
				}
			}
		}
	}

	public void CreatePreviews(TraceResult tr)
	{
		Delete();
		previewModel = Library.Create<TowerBase>( "TowerBase" );
		previewModel.SetModel( TowerModel );

		previewModel.EnableAllCollisions = false;

		previewModel.Position = tr.EndPosition;
	}

	public void DestoryPreview()
	{
		previewModel.Delete();
		previewModel = null;
	}

	public void UpdatePreview(TraceResult tr)
	{
		if ( previewModel == null )
			return;

		previewModel.Position = tr.EndPosition;
		DebugOverlay.Sphere( previewModel.Position, AttackRange, Color.Blue );
		bool isCollidingTower = false;

		foreach ( var nearbyTower in FindInSphere( previewModel.Position, 28 ) )
		{
			if( nearbyTower is TowerBase || tr.Entity is TowerBlocker )
			{
				isCollidingTower = true;
			}
		}

		if (tr.Normal.z == 1 && !isCollidingTower )
		{
			previewModel.RenderColor = Color.Green;
		} else
		{
			previewModel.RenderColor = Color.Red;
		}

		previewModel.RenderColor = previewModel.RenderColor.WithAlpha( 0.5f );


	}

	public bool CanAfford( TDPlayer buyer )
	{
		if ( buyer.CurMoney >= Cost )
			return true;

		return false;
	}

	public bool CanUpgrade(TDPlayer upgrader)
	{
		if ( upgrader.CurMoney >= UpgradeCosts[CurTier] )
			return true;
		
		return false;
	}

	public virtual void UpgradeTower(TDPlayer upgrader)
	{
		upgrader.TakeMoney( UpgradeCosts[CurTier] );
		CurTier++;
	}

	public virtual void SellTower(TDPlayer seller)
	{
		seller.AddMoney( (int)MathF.Round( Cost / 2 ) );
	}
}
