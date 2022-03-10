using Sandbox;
using System.Collections.Generic;

[Library( "supertd_difficulty_setter" )]
[Hammer.SupportsSolid]
[Hammer.EntityTool( "Difficulty Setter", "Super TD", "Sets the difficulty" )]
[Hammer.VisGroup( Hammer.VisGroup.Logic )]
public class DifficultySetter : Entity
{
	public enum DiffEnum
	{
		Easy,
		Medium,
		Hard,
		Impossible
	}

	[Property( "Difficulty" ), Description( "What difficulty should be set to" )]
	public DiffEnum Difficulty { get; set; } = DiffEnum.Easy;

}
