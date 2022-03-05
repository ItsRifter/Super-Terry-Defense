using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox;
using Sandbox.UI;

public partial class TDHUD : Sandbox.HudEntity<RootPanel>
{
	public TDHUD()
	{
		RootPanel.AddChild<ChatBox>();
		RootPanel.AddChild<NameTags>();
		RootPanel.AddChild<Scoreboard<ScoreboardEntry>>();

		RootPanel.AddChild<Status>();
		RootPanel.AddChild<Timer>();
	}
}
