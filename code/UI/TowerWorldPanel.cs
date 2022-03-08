using Sandbox.UI;
using Sandbox.UI.Construct;

public partial class TowerWorldPanel : WorldPanel
{
	public Label OwnerName;
	public Label Level;
	public TowerWorldPanel()
	{
		StyleSheet.Load( "UI/TowerWorldPanel.scss" );
		OwnerName = Add.Label( "" );
		Level = Add.Label( "Level: ", "levelText" );
	}

	public override void Tick()
	{
		base.Tick();
	}
}
