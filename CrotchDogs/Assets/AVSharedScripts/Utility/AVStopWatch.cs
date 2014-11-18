using UnityEngine;
public class AVStopWatchProfiler
{
	private System.Diagnostics.Stopwatch m_StopWatch = new System.Diagnostics.Stopwatch ();
	private string m_Name;

	public AVStopWatchProfiler (string name)
	{
		m_Name = name;
		m_StopWatch.Start ();
	}

	public void Stop ()
	{
		m_StopWatch.Stop ();
		Debug.Log ("Profiler Stopping [" + m_Name + "]: " + (float)m_StopWatch.ElapsedTicks/10000.0f + " ms");
	}
}