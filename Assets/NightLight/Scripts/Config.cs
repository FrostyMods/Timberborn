using System;
using TimberApi.ConfigSystem;
using FrostyMods.NightLight;

namespace FrostMods.NightLight {
    public class NightLightConfig : IConfig {
        public string ConfigFileName => "LightingConfig";

        public DayStage Sunrise { get; set; }
        public DayStage Day { get; set; }
        public DayStage Sunset { get; set; }
        public DayStage Night { get; set; }

        public NightLightConfig() {
            Sunrise = DayStage.Sunrise;
            Day = DayStage.Day;
            Sunset = DayStage.Sunset;
            Night = DayStage.Sunset;
        }

        public DayStage GetMappedDayStage(DayStage dayStage) {
            return dayStage switch {
                DayStage.Sunrise => Sunrise,
                DayStage.Day => Day,
                DayStage.Sunset => Sunrise,
                DayStage.Night => Night,
                _ => throw new ArgumentOutOfRangeException("dayStage", dayStage, null),
            };
        }
    }
}