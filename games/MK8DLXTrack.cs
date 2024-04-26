namespace MKTimer {
    public enum MK8DLXTrack {
        // SNES
        SNESMarioCircuit3,
        SNESBowserCastle3,
        SNESDonutPlains3,
        SNESRainbowRoad,

        // N64
        N64KalimariDesert,
        N64ToadsTurnpike,
        N64ChocoMountain,
        N64RoyalRaceway,
        N64YoshiValley,
        N64RainbowRoad,

        // GBA
        GBARiversidePark,
        GBAMarioCircuit,
        GBABooLake,
        GBACheeseLand,
        GBASkyGarden,
        GBASunsetWilds,
        GBASnowLand,
        GBARibbonRoad,

        // GCN
        GCNBabyPark,
        GCNDryDryDesert,
        GCNDaisyCruiser,
        GCNWaluigiStadium,
        GCNSherbetLand,
        GCNYoshiCircuit,
        GCNDKMountain,

        // DS
        DSCheepCheepBeach,
        DSWaluigiPinball,
        DSShroomRidge,
        DSTickTockClock,
        DSMarioCircuit,
        DSWarioStadium,
        DSPeachGardens,

        // Wii
        WiiMooMooMeadows,
        WiiMushroomGorge,
        WiiCoconutMall,
        WiiDKSummit,
        WiiWariosGoldMine,
        WiiDaisyCircuit,
        WiiKoopaCape,
        WiiMapleTreeway,
        WiiGrumbleVolcano,
        WiiMoonviewHighway,
        WiiRainbowRoad,

        // 3DS
        _3DSToadCircuit,
        _3DSMusicPark,
        _3DSRockRockMountain,
        _3DSPiranhaPlantSlide,
        _3DSNeoBowserCity,
        _3DSDKJungle,
        _3DSRosalinasIceWorld,
        _3DSRainbowRoad,

        // Mario Kart Tour
        MarioKartStadium,
        WaterPark,
        SweetSweetCanyon,
        TwompRuins,
        MarioCircuit,
        ToadHarbor,
        TwistedMansion,
        ShyGuyFalls,
        SunshineAirport,
        DolphinShoals,
        Electrodrome,
        MountWario,
        CloudtopCruise,
        BoneDryDunes,
        BowsersCastle,
        RainbowRoad,
        ExcitebikeArena,
        DragonDriftway,
        MuteCity,
        IceIceOutpost,
        HyruleCircuit,
        WildWoods,
        AnimalCrossing,
        SuperBellSubway,
        BigBlue,

        // Tour (additional)
        TourNewYorkMinute,
        TourTokioBlur,
        TourParisPromenade,
        TourLondonLoop,
        TourVancouverVelocity,
        TourLosAngelesLaps,
        MerryMountain,
        TourBerlinByways,
        NinjaHideaway,
        TourSidneySprint,
        TourSingaporeSpeedway,
        TourAmsterdamDrift,
        TourBangkokRush,
        SkyHighSundae,
        PiranhaPlantCove,
        YoshisIsland,
        TourAthensDash,
        TourRomeAvanti,
        SqueakyCleanSprint,
        TourMadridDrive
    }
    public static class TrackName {
    public static string GetTrackName(this MK8DLXTrack track)
    {
        switch (track)
        {
            // SNES
            case MK8DLXTrack.SNESMarioCircuit3:
                return "SNES Mario Circuit 3";
            case MK8DLXTrack.SNESBowserCastle3:
                return "SNES Bowser Castle 3";
            case MK8DLXTrack.SNESDonutPlains3:
                return "SNES Donut Plains 3";
            case MK8DLXTrack.SNESRainbowRoad:
                return "SNES Rainbow Road";

            // N64
            case MK8DLXTrack.N64KalimariDesert:
                return "N64 Kalimari Desert";
            case MK8DLXTrack.N64ToadsTurnpike:
                return "N64 Toad's Turnpike";
            case MK8DLXTrack.N64ChocoMountain:
                return "N64 Choco Mountain";
            case MK8DLXTrack.N64RoyalRaceway:
                return "N64 Royal Raceway";
            case MK8DLXTrack.N64YoshiValley:
                return "N64 Yoshi Valley";
            case MK8DLXTrack.N64RainbowRoad:
                return "N64 Rainbow Road";

            // GBA
            case MK8DLXTrack.GBARiversidePark:
                return "GBA Riverside Park";
            case MK8DLXTrack.GBAMarioCircuit:
                return "GBA Mario Circuit";
            case MK8DLXTrack.GBABooLake:
                return "GBA Boo Lake";
            case MK8DLXTrack.GBACheeseLand:
                return "GBA Cheese Land";
            case MK8DLXTrack.GBASkyGarden:
                return "GBA Sky Garden";
            case MK8DLXTrack.GBASunsetWilds:
                return "GBA Sunset Wilds";
            case MK8DLXTrack.GBASnowLand:
                return "GBA Snow Land";
            case MK8DLXTrack.GBARibbonRoad:
                return "GBA Ribbon Road";

            // GCN
            case MK8DLXTrack.GCNBabyPark:
                return "GCN Baby Park";
            case MK8DLXTrack.GCNDryDryDesert:
                return "GCN Dry Dry Desert";
            case MK8DLXTrack.GCNDaisyCruiser:
                return "GCN Daisy Cruiser";
            case MK8DLXTrack.GCNWaluigiStadium:
                return "GCN Waluigi Stadium";
            case MK8DLXTrack.GCNSherbetLand:
                return "GCN Sherbet Land";
            case MK8DLXTrack.GCNYoshiCircuit:
                return "GCN Yoshi Circuit";
            case MK8DLXTrack.GCNDKMountain:
                return "GCN DK Mountain";

            // DS
            case MK8DLXTrack.DSCheepCheepBeach:
                return "DS Cheep Cheep Beach";
            case MK8DLXTrack.DSWaluigiPinball:
                return "DS Waluigi Pinball";
            case MK8DLXTrack.DSShroomRidge:
                return "DS Shroom Ridge";
            case MK8DLXTrack.DSTickTockClock:
                return "DS Tick-Tock Clock";
            case MK8DLXTrack.DSMarioCircuit:
                return "DS Mario Circuit";
            case MK8DLXTrack.DSWarioStadium:
                return "DS Wario Stadium";
            case MK8DLXTrack.DSPeachGardens:
                return "DS Peach Gardens";

            // Wii
            case MK8DLXTrack.WiiMooMooMeadows:
                return "Wii Moo Moo Meadows";
            case MK8DLXTrack.WiiMushroomGorge:
                return "Wii Mushroom Gorge";
            case MK8DLXTrack.WiiCoconutMall:
                return "Wii Coconut Mall";
            case MK8DLXTrack.WiiDKSummit:
                return "Wii DK Summit";
            case MK8DLXTrack.WiiWariosGoldMine:
                return "Wii Wario's Gold Mine";
            case MK8DLXTrack.WiiDaisyCircuit:
                return "Wii Daisy Circuit";
            case MK8DLXTrack.WiiKoopaCape:
                return "Wii Koopa Cape";
            case MK8DLXTrack.WiiMapleTreeway:
                return "Wii Maple Treeway";
            case MK8DLXTrack.WiiGrumbleVolcano:
                return "Wii Grumble Volcano";
            case MK8DLXTrack.WiiMoonviewHighway:
                return "Wii Moonview Highway";
            case MK8DLXTrack.WiiRainbowRoad:
                return "Wii Rainbow Road";

            // 3DS
            case MK8DLXTrack._3DSToadCircuit:
                return "3DS Toad Circuit";
            case MK8DLXTrack._3DSMusicPark:
                return "3DS Music Park";
            case MK8DLXTrack._3DSRockRockMountain:
                return "3DS Rock Rock Mountain";
            case MK8DLXTrack._3DSPiranhaPlantSlide:
                return "3DS Piranha Plant Slide";
            case MK8DLXTrack._3DSNeoBowserCity:
                return "3DS Neo Bowser City";
            case MK8DLXTrack._3DSDKJungle:
                return "3DS DK Jungle";
            case MK8DLXTrack._3DSRosalinasIceWorld:
                return "3DS Rosalina's Ice World";
            case MK8DLXTrack._3DSRainbowRoad:
                return "3DS Rainbow Road";

            // Mario Kart Tour
            case MK8DLXTrack.MarioKartStadium:
                return "Mario Kart Stadium";
            case MK8DLXTrack.WaterPark:
                return "Water Park";
            case MK8DLXTrack.SweetSweetCanyon:
                return "Sweet Sweet Canyon";
            case MK8DLXTrack.TwompRuins:
                return "Twomp Ruins";
            case MK8DLXTrack.MarioCircuit:
                return "Mario Circuit";
            case MK8DLXTrack.ToadHarbor:
                return "Toad Harbor";
            case MK8DLXTrack.TwistedMansion:
                return "Twisted Mansion";
            case MK8DLXTrack.ShyGuyFalls:
                return "Shy Guy Falls";
            case MK8DLXTrack.SunshineAirport:
                return "Sunshine Airport";
            case MK8DLXTrack.DolphinShoals:
                return "Dolphin Shoals";
            case MK8DLXTrack.Electrodrome:
                return "Electrodrome";
            case MK8DLXTrack.MountWario:
                return "Mount Wario";
            case MK8DLXTrack.CloudtopCruise:
                return "Cloudtop Cruise";
            case MK8DLXTrack.BoneDryDunes:
                return "Bone Dry Dunes";
            case MK8DLXTrack.BowsersCastle:
                return "Bowser's Castle";
            case MK8DLXTrack.RainbowRoad:
                return "Rainbow Road";
            case MK8DLXTrack.ExcitebikeArena:
                return "Excitebike Arena";
            case MK8DLXTrack.DragonDriftway:
                return "Dragon Driftway";
            case MK8DLXTrack.MuteCity:
                return "Mute City";
            case MK8DLXTrack.IceIceOutpost:
                return "Ice Ice Outpost";
            case MK8DLXTrack.HyruleCircuit:
                return "Hyrule Circuit";
            case MK8DLXTrack.WildWoods:
                return "Wild Woods";
            case MK8DLXTrack.AnimalCrossing:
                return "Animal Crossing";
            case MK8DLXTrack.SuperBellSubway:
                return "Super Bell Subway";
            case MK8DLXTrack.BigBlue:
                return "Big Blue";

            // Tour (additional)
            case MK8DLXTrack.TourNewYorkMinute:
                return "New York Minute";
            case MK8DLXTrack.TourTokioBlur:
                return "Tokio Blur";
            case MK8DLXTrack.TourParisPromenade:
                return "Paris Promenade";
            case MK8DLXTrack.TourLondonLoop:
                return "London Loop";
            case MK8DLXTrack.TourVancouverVelocity:
                return "Vancouver Velocity";
            case MK8DLXTrack.TourLosAngelesLaps:
                return "Los Angeles Laps";
            case MK8DLXTrack.MerryMountain:
                return "Merry Mountain";
            case MK8DLXTrack.TourBerlinByways:
                return "Berlin Byways";
            case MK8DLXTrack.NinjaHideaway:
                return "Ninja Hideaway";
            case MK8DLXTrack.TourSidneySprint:
                return "Sidney Sprint";
            case MK8DLXTrack.TourSingaporeSpeedway:
                return "Singapore Speedway";
            case MK8DLXTrack.TourAmsterdamDrift:
                return "Amsterdam Drift";
            case MK8DLXTrack.TourBangkokRush:
                return "Bangkok Rush";
            case MK8DLXTrack.SkyHighSundae:
                return "Sky High Sundae";
            case MK8DLXTrack.PiranhaPlantCove:
                return "Piranha Plant Cove";
            case MK8DLXTrack.YoshisIsland:
                return "Yoshi's Island";
            case MK8DLXTrack.TourAthensDash:
                return "Athens Dash";
            case MK8DLXTrack.TourRomeAvanti:
                return "Rome Avanti";
            case MK8DLXTrack.SqueakyCleanSprint:
                return "Squeaky Clean Sprint";
            case MK8DLXTrack.TourMadridDrive:
                return "Madrid Drive";

            default:
                return "";
        }
    }
    }
}