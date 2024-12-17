public class Wave
{
    private int waveNumber;         
    private TimeSpan duration;      
    private DateTime startTime;     
    private TimeSpan pausedDuration; 
    private DateTime? pauseStartTime; 
    private Spawner spawner;
    public List<Diamond> diamonds;
    public List<Enemy> enemies;
    public List<Heart> hearts;

    public Wave(int waveNumber, TimeSpan duration, Omar omar)
    {
        this.waveNumber = waveNumber;
        this.duration = duration;
        ResetStartTime();
        diamonds = new List<Diamond>();
        enemies = new List<Enemy>();
        hearts = new List<Heart>();
        spawner = new Spawner(omar, diamonds, enemies, hearts);
    }

     public Wave(int waveNumber, Omar omar)
        : this(waveNumber, CalculateWaveDuration(waveNumber), omar) 
    {
    }
    public int WaveNumber => waveNumber;
    public Spawner Spawner => spawner;

    private static TimeSpan CalculateWaveDuration(int waveNumber)
    {
        int durationInSeconds = 30 + (waveNumber - 1) * 5;
        return TimeSpan.FromSeconds(durationInSeconds);
    }
    public void ResetStartTime()
    {
        startTime = DateTime.Now;
        pausedDuration = TimeSpan.Zero;
        pauseStartTime = null;
    }

    public int GetTimeLeft()
    {
          TimeSpan elapsed = DateTime.Now - startTime - pausedDuration;
        double timeLeft = duration.TotalSeconds - elapsed.TotalSeconds;
        return (int)Math.Round(timeLeft);
    }

    public void Pause()
    {
       if (pauseStartTime == null)
    {
        pauseStartTime = DateTime.Now;
        spawner.PauseTimers(); 
    }
    }

    public void Resume()
    {
        if (pauseStartTime != null)
        {
            pausedDuration += DateTime.Now - pauseStartTime.Value;
            pauseStartTime = null;
            spawner.ResumeTimers();
        }
    }
}
