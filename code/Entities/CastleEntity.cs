using Sandbox;

[Library( "info_supertd_castle" )]
[Hammer.EditorModel( "models/towers/castle.vmdl" )]
[Hammer.EntityTool( "Castle Base", "Super TD", "Defines a point where the castle will spawn" )]

public class CastleEntity : Entity
{
	[Property( "BlueSpawn" )]
	public bool Is_Blue_Castle { get; set; } = true;

	public override void Spawn()
	{
		base.Spawn();


		var Castle = new Castle();
		Castle.Position = Position;
		Castle.Rotation = Rotation;

		if ( Is_Blue_Castle )
			Castle.Name = "blue_castle";
		else
			Castle.Name = "red_castle";
	}
}
