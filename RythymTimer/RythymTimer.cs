using Godot;
using System;
using System.Collections.Generic; 

public partial class TimerRhythmThing : Timer
{
	const int MAXBPM = 1200,MINBPM = 20;
	[Export]int subdivisions,BPM;
	int currentBeat;
	[Export]Vector2 timeSignature;
	float timePerBeat,timerPerSubdivision;
	List<float> referenceTimes = new List<float>();
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		currentBeat = 0;
		Timeout += OnBeatPlayed;
		timePerBeat = 1f / (BPM / 60);
		timerPerSubdivision = timePerBeat / subdivisions;
		WaitTime = timePerBeat;
		GD.Print(WaitTime);
		Start();
		for (int i = 0; i < timeSignature.X + 1; i++)
		{
			for (int j = 0; j < subdivisions; j++)
			{
				referenceTimes.Add((i * timePerBeat) + (j * timerPerSubdivision));
			}
		}
		referenceTimes.ForEach((x) => GD.Print(x));
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("Spacebar"))
		{
			double currentBarTimer = (timePerBeat * (currentBeat - 1)) + (WaitTime - TimeLeft);
			GD.Print(currentBarTimer);
			for (int i = 0; i < referenceTimes.Count; i++)
			{
				if (currentBarTimer <= referenceTimes[i])
				{
					double difference1 = Math.Abs(currentBarTimer - referenceTimes[i]);
					double difference2 = Math.Abs(currentBarTimer - referenceTimes[i - 1]);
					if (difference1 < difference2)
					{
						//check distance from distance 1
						GetRating(difference1);
					}else
					{
						//check difference from distance 2
						GetRating(difference2);
					}
					break;
				}
			}
		}
	}
	private void GetRating(double difference)
	{
		switch (difference)
		{
			case < 0.05f:
				GD.Print("Perfect");
				break;
			case < 0.1f:
				GD.Print("Great");
				break;
			case < 0.25f:
				GD.Print("Okay");
				break;
			case < 0.40f:
				GD.Print("Bad");
				break;
			default:
				GD.Print("Missed");
				break;
		}
	}
	public void OnBeatPlayed()
	{
		currentBeat++;
		if(currentBeat == timeSignature.X + 1)
		{
			currentBeat = 1;
		}
		GD.Print("BeatComplete: " + currentBeat);
	}
}

