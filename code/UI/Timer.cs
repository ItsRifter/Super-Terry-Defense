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

		timerLbl = Add.Label( "Starting game in" );
	}

	public override void Tick()
	{
		base.Tick();

		if ( TDGame.Current.CurGameStatus == TDGame.GameStatus.Idle )
			timerLbl.SetText( "Starting game in " + MathF.Round( TDGame.Current.WaveTimer - Time.Now ) );

		SetClass( "idle", TDGame.Current.CurGameStatus == TDGame.GameStatus.Idle );

		if ( TDGame.Current.CurGameStatus == TDGame.GameStatus.Active )
			if ( TDGame.Current.CurWaveStatus == TDGame.WaveStatus.Waiting )
				timerLbl.SetText( "Wave starting in " + MathF.Round( TDGame.Current.WaveTimer - Time.Now ) );
			else
				timerLbl.SetText( "Wave: " + TDGame.Current.CurWave + "/" + TDGame.Current.MaxWave );
		else if ( TDGame.Current.CurGameStatus == TDGame.GameStatus.Post )
		{
			timerLbl.SetText( "GAME OVER" ); 
		}
	}

}
