using Microsoft.Xna.Framework;

namespace TRexGame.Entities
{
    // a simple interface which can be confortably reused
    public interface IDayNightCycle
    {
        int NightCount { get; }
        bool IsNight { get; }

        Color ClearColor { get; }
    }
}
