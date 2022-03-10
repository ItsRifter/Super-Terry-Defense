using System;
using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;
public partial class Timer : Panel
{
	public Label timerLbl;

	public Timer()
	{
		StyleSheet.Load( "UI/Timer.scss" );

		timerLbl = Add.Label( "" );
	}

	public override void Tick()
	{
		base.Tick();

		if( Local.Pawn is TDPlayer player )
		{
			if (TDGame.Current.CurGameStatus == TDGame.GameStatus.Idle)
            {
				if(!Local.Client.IsListenServerHost)
					timerLbl.SetText( "Waiting for host to set up game" );
				else
				{
					timerLbl.SetText( "Use commands: startcomp or startcoop" );
				}
			} 
			else if ( TDGame.Current.CurGameStatus == TDGame.GameStatus.Starting )
			{
				if( TDGame.Current.GameType == TDGame.GamemodeType.Cooperative )
				{
					timerLbl.SetText( "Host has selected 'Cooperative', starting in " + Math.Round(TDGame.Current.WaveTimer - Time.Now) );
				}
				else if( TDGame.Current.GameType == TDGame.GamemodeType.Competitive )
				{
					timerLbl.SetText( "Host has selected 'Competitive', starting in " + Math.Round(TDGame.Current.WaveTimer - Time.Now ) );
				}
			}

			//if ( TDGame.Current.CurGameStatus == TDGame.GameStatus.Idle )
			//timerLbl.SetText( Translation.Translate( "Timer_Game") + MathF.Round( TDGame.Current.WaveTimer - Time.Now ) );

			SetClass( "idle", TDGame.Current.CurGameStatus == TDGame.GameStatus.Idle || TDGame.Current.CurGameStatus == TDGame.GameStatus.Starting );

			if ( TDGame.Current.CurGameStatus == TDGame.GameStatus.Active )
				if ( TDGame.Current.CurWaveStatus == TDGame.WaveStatus.Waiting )
					timerLbl.SetText( Translation.Translate( "Timer_Wave" ) + MathF.Round( TDGame.Current.WaveTimer - Time.Now ) );
				else
					timerLbl.SetText( Translation.Translate( "Current_Wave" ) + TDGame.Current.CurWave + "/" + TDGame.Current.MaxWave );
			else if ( TDGame.Current.CurGameStatus == TDGame.GameStatus.Post )
			{
				timerLbl.SetText( Translation.Translate( "Game_Finished" ) ); 
			}
		}
	}

}
