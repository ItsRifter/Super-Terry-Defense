using System;
using System.Collections.Generic;
using System.Linq;
using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;
public class NPCTagBase : Panel
{
	public Label NameLabel;

	TDNPCBase npc;

	public NPCTagBase( TDNPCBase npc )
	{
		this.npc = npc;

		NameLabel = Add.Label( $"{npc.NPCName + "\nHP: " + npc.Health}" );
	}

	public override void Tick()
	{
		base.Tick();

		NameLabel.SetText( $"{npc.NPCName + "\nHP: " + npc.Health}" );
	}
}

public class NPCTag : Panel
{
	Dictionary<TDNPCBase, NPCTagBase> ActiveTags = new Dictionary<TDNPCBase, NPCTagBase>();

	public float MaxDrawDistance = 400;
	public int MaxTagsToShow = 1;

	public NPCTag()
	{
		StyleSheet.Load( "UI/NPCTag.scss" );
	}

	public override void Tick()
	{
		base.Tick();

		var deleteList = new List<TDNPCBase>();
		deleteList.AddRange( ActiveTags.Keys );

		int count = 0;
		foreach ( var npc in Entity.All.OfType<TDNPCBase>().OrderBy( x => Vector3.DistanceBetween( x.Position, CurrentView.Position ) ) )
		{
			if ( UpdateNameTag( npc ) )
			{
				deleteList.Remove( npc );
				count++;
			}

			if ( count >= MaxTagsToShow )
				break;
		}

		foreach ( var npc in deleteList )
		{
			ActiveTags[npc].Delete();
			ActiveTags.Remove( npc );
		}

	}

	public virtual NPCTagBase CreateNameTag( TDNPCBase npc )
	{
		if ( npc == null )
			return null;

		var tag = new NPCTagBase( npc );
		tag.Parent = this;
		return tag;
	}

	public bool UpdateNameTag( TDNPCBase npc )
	{
		if ( npc.LifeState != LifeState.Alive )
			return false;

		var head = new Transform( npc.Position );

		var labelPos = head.Position + head.Rotation.Up * 5;

		float dist = labelPos.Distance( CurrentView.Position );
		if ( dist > MaxDrawDistance )
			return false;

		var lookDir = (labelPos - CurrentView.Position).Normal;
		if ( CurrentView.Rotation.Forward.Dot( lookDir ) < 0.5 )
			return false;

		MaxDrawDistance = 150;

		var alpha = dist.LerpInverse( MaxDrawDistance, MaxDrawDistance * 0.1f, true );

		// If I understood this I'd make it proper function
		var objectSize = 0.05f / dist / (2.0f * MathF.Tan( (CurrentView.FieldOfView / 2.0f).DegreeToRadian() )) * 1500.0f;

		objectSize = objectSize.Clamp( 0.05f, 1.0f );

		if ( !ActiveTags.TryGetValue( npc, out var tag ) )
		{
			tag = CreateNameTag( npc );
			if ( tag != null )
			{
				ActiveTags[npc] = tag;
			}
		}

		if ( tag == null )
			return false;

		var screenPos = labelPos.ToScreen();

		tag.Style.Left = Length.Fraction( screenPos.x );
		tag.Style.Top = Length.Fraction( screenPos.y );
		tag.Style.Opacity = alpha;

		var transform = new PanelTransform();
		transform.AddTranslateY( Length.Fraction( -1.0f ) );
		transform.AddScale( objectSize );
		transform.AddTranslateX( Length.Fraction( -0.5f ) );

		tag.Style.Transform = transform;
		tag.Style.Dirty();

		return true;
	}
}
