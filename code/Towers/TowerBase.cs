using System;
using Sandbox;

public partial class TowerBase : AnimEntity
{
	public virtual string TowerName => "Base Tower";
	public virtual string TowerDesc => "A base tower to derive towers from";
	public virtual string TowerModel => "models/citizen_props/crate01.vmdl";
	[Net] public int AttackDamage { get; set; }
	[Net] public float AttackCooldown { get; set; }
	[Net] public int AttackRange { get; set; }
	public virtual string AttackSound => "";

	public virtual int Cost => 1;
	public virtual int MaxTier => 2;
	public bool CanSeeCloaked { get; set; }
	[Net] public int CurTier { get; private set; }

	[Net] public TowerBase previewModel { get; private set; }
	private TimeSince lastAttack;
	private float rotationFloat;
	private TDNPCBase target;
	public enum AttackMethod
	{
		Bullet,
		Explosive,
		Frost,
		Electric
	}

	public virtual AttackMethod MethodOfAttack => AttackMethod.Bullet;

	private TowerWorldPanel towerPanel;

	public bool RadarEnhanced;

	[Net] public virtual int[] UpgradeCosts => new[] { 2, 3, 4, 5, 6 };
	public virtual string[] UpgradeDesc => new[] { "[Input Info Here]" };

	[ConVar.Replicated]
	public static bool td_tower_drawoverlay { get; set; }

	public override void Spawn()
	{
		SetModel( TowerModel );

		EnableHitboxes = true;

		RadarEnhanced = false;
		CanSeeCloaked = false;
		CurTier = 0;
		rotationFloat = 0;

		Tags.Add("Tower");
		SetupPhysicsFromModel( PhysicsMotionType.Invalid );
	}

	public virtual void AttackNPC(TDNPCBase npc)
	{
		DamageInfo dmgInfo = new DamageInfo();
		dmgInfo.Damage = AttackDamage;

		npc.TakeDamage( dmgInfo );

		lastAttack = 0;
	}

	[ClientRpc]
	protected virtual void ShootEffects()
	{
		Host.AssertClient();

		PlaySound( AttackSound );

		if ( MethodOfAttack == AttackMethod.Bullet )
		{
			Particles.Create( "particles/bullet_muzzleflash.vpcf", this, "barrel1" );
			Particles.Create( "particles/bullet_muzzleflash.vpcf", this, "barrel2" );
		}

		if(MethodOfAttack == AttackMethod.Explosive)
			Particles.Create( "particles/explosive_muzzleflash.vpcf", this, "muzzle" );
	}

	[Event.Tick.Server]
	public virtual void SimulateTower()
	{

		if( target.IsValid() && target.Position.Length >= AttackRange)
		{
			if ( target.NPCType == TDNPCBase.SpecialType.Cloaked && !CanSeeCloaked )
				return;

			if ( AttackDamage <= 0 )
				return;

			if ( MethodOfAttack == AttackMethod.Bullet || MethodOfAttack == AttackMethod.Explosive )
				SetAnimLookAt( "target_look", target.Position );

			if ( lastAttack > AttackCooldown )
			{
				ShootEffects();
				AttackNPC( target );
			}
		} else
		{
			target = null;
		}

		var ents = FindInSphere( Position, AttackRange );

		foreach ( var ent in ents )
		{
			if( ent is TDNPCBase npc )
			{
				if ( npc.NPCType == TDNPCBase.SpecialType.Cloaked && !CanSeeCloaked )
					return;

				if ( AttackDamage <= 0 )
					return;

				if(MethodOfAttack == AttackMethod.Bullet || MethodOfAttack == AttackMethod.Explosive)
					SetAnimLookAt( "target_look", npc.Position);
				
				if ( lastAttack > AttackCooldown )
				{
					ShootEffects();
					AttackNPC( npc );
				}

				target = npc;
			}
		}

		if ( rotationFloat >= 360 )
			rotationFloat = 0;

		if ( Owner != null || Owner is not TDPlayer player )
			return;

		DestroyClientPanel();
		Delete();
	}

	[Event.Tick.Client]
	public virtual void SimulateTowerClient()
	{
		if ( rotationFloat >= 360 )
			rotationFloat = 0;

		if ( Owner != null && Owner is TDPlayer player )
		{
			if ( towerPanel == null )
				return;

			string towerTranslate = Translation.Translate( TowerName );
			string lvlTranslate = Translation.Translate( "Tower_Level" );

			towerPanel.Transform = Transform;
			towerPanel.Position = Position - (Vector3.Up * 5 - CollisionBounds.Center);
			towerPanel.Rotation = Rotation.FromYaw( rotationFloat += 1 );
			towerPanel.OwnerName.SetText( Owner.Client.Name + "'s " + towerTranslate );
			towerPanel.Level.SetText( lvlTranslate + CurTier );
		}
	}

	public void CreatePreviews(TraceResult tr, TDPlayer player)
	{
		Delete();
		previewModel = Library.Create<TowerBase>( "TowerBase" );
		previewModel.SetModel( TowerModel );

		previewModel.Tags.Add( "Tower" );

		previewModel.EnableAllCollisions = false;
		previewModel.Owner = player;
		previewModel.Position = tr.EndPosition;
	}

	public void UpdatePreview( TraceResult tr, TDPlayer placer )
	{
		if ( previewModel == null )
			return;

		previewModel.Position = tr.EndPosition;

		bool isCollidingTower = false;

		foreach ( var nearbyTower in FindInSphere( previewModel.Position, 28 ) )
		{
			if ( nearbyTower is TowerBase || tr.Entity is TowerBlocker )
			{
				isCollidingTower = true;
			}
		}

		if ( tr.Normal.z == 1 && !isCollidingTower )
		{
			previewModel.RenderColor = Color.Green;
		}
		else
		{
			previewModel.RenderColor = Color.Red;
		}

		previewModel.RenderColor = previewModel.RenderColor.WithAlpha( 0.5f );
	}

	public void DestoryPreview()
	{
		if ( previewModel == null )
			return;

		previewModel.Delete();
		previewModel = null;
	}

	[ClientRpc]
	public void CreateClientPanel(TowerBase tower)
	{
		towerPanel = new TowerWorldPanel();
		var towerTranslate = Translation.Translate( TowerName );
		towerPanel.OwnerName.SetText( tower.Owner.Client.Name + "'s " + towerTranslate );
		towerPanel.Rotation = tower.Rotation;
	}

	[ClientRpc]
	public void UpdateClientPanel( TowerBase tower, float rot)
	{
		if ( tower == null || towerPanel == null || tower.Owner == null )
			return;

		string towerTranslate = Translation.Translate( TowerName );
		string lvlTranslate = Translation.Translate( "Tower_Level" );

		towerPanel.Transform = tower.Transform;
		towerPanel.Position = tower.Position - (Vector3.Up * 5 - tower.CollisionBounds.Center);
		towerPanel.Rotation = Rotation.FromYaw( rot );
		towerPanel.OwnerName.SetText( tower.Owner.Client.Name + "'s " + towerTranslate );
		towerPanel.Level.SetText( lvlTranslate + tower.CurTier );
	}

	[ClientRpc]
	public void DestroyClientPanel()
	{
		if(towerPanel != null)
			towerPanel.Delete();
	}

	public TowerBase GetPreview()
	{
		return previewModel;
	}

	public bool CanAfford( TDPlayer buyer )
	{
		if ( buyer.CurMoney >= Cost )
			return true;

		return false;
	}

	public bool CanUpgrade(TDPlayer upgrader)
	{
		if ( CurTier < MaxTier && upgrader.CurMoney >= UpgradeCosts[CurTier] )
			return true;
		
		return false;
	}

	public virtual void UpgradeTower(TDPlayer upgrader)
	{
		upgrader.TakeMoney( UpgradeCosts[CurTier] );
		OnUpgrade();
	}

	public virtual void OnUpgrade()
	{
		CurTier++;
	}

	public virtual void SellTower(TDPlayer seller)
	{
		seller.AddMoney( (int)MathF.Round( Cost / 2 * (CurTier + 1)) );
		DestroyClientPanel( To.Everyone );
		Delete();
	}
}
