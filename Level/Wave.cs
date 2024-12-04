public class Wave
{
    private int waveNumber;         // Número de la oleada
    private TimeSpan duration;      // Duración de la oleada
    private DateTime startTime;     // Momento en que inició la oleada
    private TimeSpan pausedDuration; // Tiempo acumulado en pausa
    private DateTime? pauseStartTime; // Momento en que comenzó la pausa, si aplica

    public Wave(int waveNumber, TimeSpan duration)
    {
        this.waveNumber = waveNumber;
        this.duration = duration;
        ResetStartTime();
    }

    public int WaveNumber => waveNumber;

    public void ResetStartTime()
    {
        startTime = DateTime.Now;
        pausedDuration = TimeSpan.Zero;
        pauseStartTime = null;
    }

    public int GetTimeLeft()
    {
        TimeSpan elapsed = DateTime.Now - startTime - pausedDuration;
        int timeLeft = (int)(duration.TotalSeconds - elapsed.TotalSeconds);
        return Math.Max(0, timeLeft);
    }

    public void Pause()
    {
        if (pauseStartTime == null) pauseStartTime = DateTime.Now;
    }

    public void Resume()
    {
        if (pauseStartTime != null)
        {
            pausedDuration += DateTime.Now - pauseStartTime.Value;
            pauseStartTime = null;
        }
    }
}
