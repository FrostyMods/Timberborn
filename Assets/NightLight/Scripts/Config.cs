using System;
using FrostyMods.NightLight.Lighting;
using TimberApi.ConfigSystem;

namespace FrostMods.NightLight.Config
{
    public class LightingConfig : IConfig
    {
        public string ConfigFileName => "LightingConfig";

        public DayStage Sunrise { get; set; }
        public DayStage Day { get; set; }
        public DayStage Sunset { get; set; }
        public DayStage Night { get; set; }

        public LightingConfig()
        {
            Sunrise = DayStage.Sunrise;
            Day = DayStage.Day;
            Sunset = DayStage.Sunset;
            Night = DayStage.Sunset;
        }

        public DayStage GetMappedDayStage(DayStage dayStage)
        {
            return dayStage switch
            {
                DayStage.Sunrise => Sunrise,
                DayStage.Day => Day,
                DayStage.Sunset => Sunrise,
                DayStage.Night => Night,
                _ => throw new ArgumentOutOfRangeException("dayStage", dayStage, null),
            };
        }
    }
}