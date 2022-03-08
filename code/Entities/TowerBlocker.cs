using Sandbox;

[Library( "supertd_tower_blocker" )]
[Hammer.SupportsSolid]
[Hammer.EntityTool( "Tower Blocker", "Super TD", "Prevents spawning of towers" )]
[Hammer.RenderFields]
public class TowerBlocker : BrushEntity
{
	public override void Spawn()
	{
		EnableDrawing = true;
		UsePhysicsCollision = true;
	}

}
