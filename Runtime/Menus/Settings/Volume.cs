namespace MenuSystem.Settings
{
    public enum Volume
    {
        MASTER,
        MUSIC,
        ENVIRONMENT,
        SFX
    }
    public static class VolumeExtensions
    {
        public static string ToName(this Volume type)
        {
            return type switch
            {
                Volume.MASTER => "MasterVolume",
                Volume.MUSIC => "MusicVolume",
                Volume.ENVIRONMENT => "EnvironmentVolume",
                Volume.SFX => "SFXVolume",
                _ => throw new System.ArgumentOutOfRangeException()
            };
        }
    }
}