
using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;

public class TowerIcon : Panel
{
	public Image IconImage;
	public Panel IconPnl;
	public Label IconKey;
	public TowerIcon( int i, Panel parent )
	{
		Parent = parent;
		IconPnl = Add.Panel();
		IconImage = IconPnl.Add.Image( $"ui/slot-{i}.png", "slot-icon" );
		
		if(i == 10)
		{
			IconKey = IconPnl.Add.Label( "0", "slot-number" );
		} 
		else
		{
			IconKey = IconPnl.Add.Label($"{i}", "slot-number");
		}
	}

	public void Clear()
	{
		SetClass( "active", false );
	}
}
