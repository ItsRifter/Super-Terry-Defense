using Sandbox;

[Library( "info_supertd_castle" )]
[Hammer.EditorModel( "models/towers/castle.vmdl" )]
[Hammer.EntityTool( "Castle Base", "Super TD", "Defines a point where the castle will spawn" )]

public class CastleEntity : Entity
{
	public enum CastleTeam
	{
		Unknown,
		Blue,
		Red
	}

	[Property( "CastleTeam" ), Description("Which team does this castle belong to")]
	public CastleTeam TeamCastle { get; set; } = CastleTeam.Unknown;

	public override void Spawn()
	{
		base.Spawn();

		var newCastle = new Castle();
		newCastle.Position = Position;
		newCastle.Rotation = Rotation;

		if ( TeamCastle == CastleTeam.Blue )
			newCastle.TeamCastle = Castle.CastleTeam.Blue;
		else if ( TeamCastle == CastleTeam.Red )
			newCastle.TeamCastle = Castle.CastleTeam.Red;
	}
}
