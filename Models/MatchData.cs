﻿using Newtonsoft.Json;

namespace API.Models
{
    public class MatchData
    {
        public Metadata metadata { get; set; }
        public Info info { get; set; }
        public class Atakhan
        {
            public bool first { get; set; }
            public int kills { get; set; }
        }

        public class Ban
        {
            public int championId { get; set; }
            public int pickTurn { get; set; }
        }

        public class Baron
        {
            public bool first { get; set; }
            public int kills { get; set; }
        }

        public class Challenges
        {
            [JsonProperty("12AssistStreakCount")]
            public int _12AssistStreakCount { get; set; }
            public double HealFromMapSources { get; set; }
            public int InfernalScalePickup { get; set; }
            public int SWARM_DefeatAatrox { get; set; }
            public int SWARM_DefeatBriar { get; set; }
            public int SWARM_DefeatMiniBosses { get; set; }
            public int SWARM_EvolveWeapon { get; set; }
            public int SWARM_Have3Passives { get; set; }
            public int SWARM_KillEnemy { get; set; }
            public int SWARM_PickupGold { get; set; }
            public int SWARM_ReachLevel50 { get; set; }
            public int SWARM_Survive15Min { get; set; }
            public int SWARM_WinWith5EvolvedWeapons { get; set; }
            public int abilityUses { get; set; }
            public int acesBefore15Minutes { get; set; }
            public int alliedJungleMonsterKills { get; set; }
            public int baronTakedowns { get; set; }
            public int blastConeOppositeOpponentCount { get; set; }
            public double bountyGold { get; set; }
            public int buffsStolen { get; set; }
            public int completeSupportQuestInTime { get; set; }
            public int controlWardsPlaced { get; set; }
            public double damagePerMinute { get; set; }
            public double damageTakenOnTeamPercentage { get; set; }
            public int dancedWithRiftHerald { get; set; }
            public int deathsByEnemyChamps { get; set; }
            public int dodgeSkillShotsSmallWindow { get; set; }
            public int doubleAces { get; set; }
            public int dragonTakedowns { get; set; }
            public int earlyLaningPhaseGoldExpAdvantage { get; set; }
            public double effectiveHealAndShielding { get; set; }
            public int elderDragonKillsWithOpposingSoul { get; set; }
            public int elderDragonMultikills { get; set; }
            public int enemyChampionImmobilizations { get; set; }
            public int enemyJungleMonsterKills { get; set; }
            public int epicMonsterKillsNearEnemyJungler { get; set; }
            public int epicMonsterKillsWithin30SecondsOfSpawn { get; set; }
            public int epicMonsterSteals { get; set; }
            public int epicMonsterStolenWithoutSmite { get; set; }
            public int firstTurretKilled { get; set; }
            public int fistBumpParticipation { get; set; }
            public int flawlessAces { get; set; }
            public int fullTeamTakedown { get; set; }
            public double gameLength { get; set; }
            public int getTakedownsInAllLanesEarlyJungleAsLaner { get; set; }
            public double goldPerMinute { get; set; }
            public int hadOpenNexus { get; set; }
            public int immobilizeAndKillWithAlly { get; set; }
            public int initialBuffCount { get; set; }
            public int initialCrabCount { get; set; }
            public double jungleCsBefore10Minutes { get; set; }
            public int junglerTakedownsNearDamagedEpicMonster { get; set; }
            public int kTurretsDestroyedBeforePlatesFall { get; set; }
            public double kda { get; set; }
            public int killAfterHiddenWithAlly { get; set; }
            public double killParticipation { get; set; }
            public int killedChampTookFullTeamDamageSurvived { get; set; }
            public int killingSprees { get; set; }
            public int killsNearEnemyTurret { get; set; }
            public int killsOnOtherLanesEarlyJungleAsLaner { get; set; }
            public int killsOnRecentlyHealedByAramPack { get; set; }
            public int killsUnderOwnTurret { get; set; }
            public int killsWithHelpFromEpicMonster { get; set; }
            public int knockEnemyIntoTeamAndKill { get; set; }
            public int landSkillShotsEarlyGame { get; set; }
            public int laneMinionsFirst10Minutes { get; set; }
            public int laningPhaseGoldExpAdvantage { get; set; }
            public int legendaryCount { get; set; }
            public List<int> legendaryItemUsed { get; set; }
            public int lostAnInhibitor { get; set; }
            public double maxCsAdvantageOnLaneOpponent { get; set; }
            public int maxKillDeficit { get; set; }
            public int maxLevelLeadLaneOpponent { get; set; }
            public int mejaisFullStackInTime { get; set; }
            public double moreEnemyJungleThanOpponent { get; set; }
            public int multiKillOneSpell { get; set; }
            public int multiTurretRiftHeraldCount { get; set; }
            public int multikills { get; set; }
            public int multikillsAfterAggressiveFlash { get; set; }
            public int outerTurretExecutesBefore10Minutes { get; set; }
            public int outnumberedKills { get; set; }
            public int outnumberedNexusKill { get; set; }
            public int perfectDragonSoulsTaken { get; set; }
            public int perfectGame { get; set; }
            public int pickKillWithAlly { get; set; }
            public int playedChampSelectPosition { get; set; }
            public int poroExplosions { get; set; }
            public int quickCleanse { get; set; }
            public int quickFirstTurret { get; set; }
            public int quickSoloKills { get; set; }
            public int riftHeraldTakedowns { get; set; }
            public int saveAllyFromDeath { get; set; }
            public int scuttleCrabKills { get; set; }
            public int skillshotsDodged { get; set; }
            public int skillshotsHit { get; set; }
            public int snowballsHit { get; set; }
            public int soloBaronKills { get; set; }
            public int soloKills { get; set; }
            public int soloTurretsLategame { get; set; }
            public int stealthWardsPlaced { get; set; }
            public int survivedSingleDigitHpCount { get; set; }
            public int survivedThreeImmobilizesInFight { get; set; }
            public int takedownOnFirstTurret { get; set; }
            public int takedowns { get; set; }
            public int takedownsAfterGainingLevelAdvantage { get; set; }
            public int takedownsBeforeJungleMinionSpawn { get; set; }
            public int takedownsFirstXMinutes { get; set; }
            public int takedownsInAlcove { get; set; }
            public int takedownsInEnemyFountain { get; set; }
            public int teamBaronKills { get; set; }
            public double teamDamagePercentage { get; set; }
            public int teamElderDragonKills { get; set; }
            public int teamRiftHeraldKills { get; set; }
            public int tookLargeDamageSurvived { get; set; }
            public int turretPlatesTaken { get; set; }
            public int turretTakedowns { get; set; }
            public int turretsTakenWithRiftHerald { get; set; }
            public int twentyMinionsIn3SecondsCount { get; set; }
            public int twoWardsOneSweeperCount { get; set; }
            public int unseenRecalls { get; set; }
            public double visionScoreAdvantageLaneOpponent { get; set; }
            public double visionScorePerMinute { get; set; }
            public int voidMonsterKill { get; set; }
            public int wardTakedowns { get; set; }
            public int wardTakedownsBefore20M { get; set; }
            public int wardsGuarded { get; set; }
            public double? earliestDragonTakedown { get; set; }
            public int? junglerKillsEarlyJungle { get; set; }
            public int? killsOnLanersEarlyJungleAsJungler { get; set; }
            public double? controlWardTimeCoverageInRiverOrEnemyHalf { get; set; }
            public int? highestWardKills { get; set; }
            public double? firstTurretKilledTime { get; set; }
            public double? shortestTimeToAceFromFirstTakedown { get; set; }
            public int? highestChampionDamage { get; set; }
            public int? highestCrowdControlScore { get; set; }
            public int? teleportTakedowns { get; set; }
        }

        public class Champion
        {
            public bool first { get; set; }
            public int kills { get; set; }
        }

        public class Dragon
        {
            public bool first { get; set; }
            public int kills { get; set; }
        }

        public class EPICMONSTERKILL
        {
            public int featState { get; set; }
        }

        public class Feats
        {
            public EPICMONSTERKILL EPIC_MONSTER_KILL { get; set; }
            public FIRSTBLOOD FIRST_BLOOD { get; set; }
            public FIRSTTURRET FIRST_TURRET { get; set; }
        }

        public class FIRSTBLOOD
        {
            public int featState { get; set; }
        }

        public class FIRSTTURRET
        {
            public int featState { get; set; }
        }

        public class Horde
        {
            public bool first { get; set; }
            public int kills { get; set; }
        }

        public class Info
        {
            public string endOfGameResult { get; set; }
            public long gameCreation { get; set; }
            public int gameDuration { get; set; }
            public long gameEndTimestamp { get; set; }
            public long gameId { get; set; }
            public string gameMode { get; set; }
            public string gameName { get; set; }
            public long gameStartTimestamp { get; set; }
            public string gameType { get; set; }
            public string gameVersion { get; set; }
            public int mapId { get; set; }
            public List<Participant> participants { get; set; }
            public string platformId { get; set; }
            public int queueId { get; set; }
            public List<Team> teams { get; set; }
            public string tournamentCode { get; set; }
        }

        public class Inhibitor
        {
            public bool first { get; set; }
            public int kills { get; set; }
        }

        public class Metadata
        {
            public string dataVersion { get; set; }
            public string matchId { get; set; }
            public List<string> participants { get; set; }
        }

        public class Missions
        {
            public string? playerScore0 { get; set; }
            public string? playerScore1 { get; set; }
            public string? playerScore2 { get; set; }
            public string? playerScore3 { get; set; }
            public string? playerScore4 { get; set; }
            public string? playerScore5 { get; set; }
            public string? playerScore6 { get; set; }
            public string? playerScore7 { get; set; }
            public string? playerScore8 { get; set; }
            public string? playerScore9 { get; set; }
            public string? playerScore10 { get; set; }
            public string? playerScore11 { get; set; }
        }

        public class Objectives
        {
            public Atakhan atakhan { get; set; }
            public Baron baron { get; set; }
            public Champion champion { get; set; }
            public Dragon dragon { get; set; }
            public Horde horde { get; set; }
            public Inhibitor inhibitor { get; set; }
            public RiftHerald riftHerald { get; set; }
            public Tower tower { get; set; }
        }

        public class Participant
        {
            public string? PlayerScore0 { get; set; }
            public string? PlayerScore1 { get; set; }
            public string? PlayerScore10 { get; set; }
            public string? PlayerScore11 { get; set; }
            public string? PlayerScore2 { get; set; }
            public string? PlayerScore3 { get; set; }
            public string? PlayerScore4 { get; set; }
            public string? PlayerScore5 { get; set; }
            public string? PlayerScore6 { get; set; }
            public string? PlayerScore7 { get; set; }
            public string? PlayerScore8 { get; set; }
            public string? PlayerScore9 { get; set; }
            public string? allInPings { get; set; }
            public string? assistMePings { get; set; }
            public string? assists { get; set; }
            public string? baronKills { get; set; }
            public string? basicPings { get; set; }
            public Challenges challenges { get; set; }
            public int champExperience { get; set; }
            public int champLevel { get; set; }
            public int championId { get; set; }
            public string championName { get; set; }
            public int championSkinId { get; set; }
            public int championTransform { get; set; }
            public int commandPings { get; set; }
            public int consumablesPurchased { get; set; }
            public int damageDealtToBuildings { get; set; }
            public int damageDealtToObjectives { get; set; }
            public int damageDealtToTurrets { get; set; }
            public int damageSelfMitigated { get; set; }
            public int dangerPings { get; set; }
            public int deaths { get; set; }
            public int detectorWardsPlaced { get; set; }
            public int doubleKills { get; set; }
            public int dragonKills { get; set; }
            public bool eligibleForProgression { get; set; }
            public int enemyMissingPings { get; set; }
            public int enemyVisionPings { get; set; }
            public bool firstBloodAssist { get; set; }
            public bool firstBloodKill { get; set; }
            public bool firstTowerAssist { get; set; }
            public bool firstTowerKill { get; set; }
            public bool gameEndedInEarlySurrender { get; set; }
            public bool gameEndedInSurrender { get; set; }
            public int getBackPings { get; set; }
            public int goldEarned { get; set; }
            public int goldSpent { get; set; }
            public int holdPings { get; set; }
            public string individualPosition { get; set; }
            public int inhibitorKills { get; set; }
            public int inhibitorTakedowns { get; set; }
            public int inhibitorsLost { get; set; }
            public int item0 { get; set; }
            public int item1 { get; set; }
            public int item2 { get; set; }
            public int item3 { get; set; }
            public int item4 { get; set; }
            public int item5 { get; set; }
            public int item6 { get; set; }
            public int itemsPurchased { get; set; }
            public int killingSprees { get; set; }
            public int kills { get; set; }
            public string lane { get; set; }
            public int largestCriticalStrike { get; set; }
            public int largestKillingSpree { get; set; }
            public int largestMultiKill { get; set; }
            public int longestTimeSpentLiving { get; set; }
            public int magicDamageDealt { get; set; }
            public int magicDamageDealtToChampions { get; set; }
            public int magicDamageTaken { get; set; }
            public Missions missions { get; set; }
            public int needVisionPings { get; set; }
            public int neutralMinionsKilled { get; set; }
            public int nexusKills { get; set; }
            public int nexusLost { get; set; }
            public int nexusTakedowns { get; set; }
            public int objectivesStolen { get; set; }
            public int objectivesStolenAssists { get; set; }
            public int onMyWayPings { get; set; }
            public int participantId { get; set; }
            public int pentaKills { get; set; }
            public Perks perks { get; set; }
            public int physicalDamageDealt { get; set; }
            public int physicalDamageDealtToChampions { get; set; }
            public int physicalDamageTaken { get; set; }
            public int placement { get; set; }
            public int playerAugment1 { get; set; }
            public int playerAugment2 { get; set; }
            public int playerAugment3 { get; set; }
            public int playerAugment4 { get; set; }
            public int playerAugment5 { get; set; }
            public int playerAugment6 { get; set; }
            public int playerSubteamId { get; set; }
            public int profileIcon { get; set; }
            public int pushPings { get; set; }
            public string puuid { get; set; }
            public int quadraKills { get; set; }
            public int retreatPings { get; set; }
            public string riotIdGameName { get; set; }
            public string riotIdTagline { get; set; }
            public string role { get; set; }
            public int sightWardsBoughtInGame { get; set; }
            public int spell1Casts { get; set; }
            public int spell2Casts { get; set; }
            public int spell3Casts { get; set; }
            public int spell4Casts { get; set; }
            public int subteamPlacement { get; set; }
            public int summoner1Casts { get; set; }
            public int summoner1Id { get; set; }
            public int summoner2Casts { get; set; }
            public int summoner2Id { get; set; }
            public string summonerId { get; set; }
            public int summonerLevel { get; set; }
            public string summonerName { get; set; }
            public bool teamEarlySurrendered { get; set; }
            public int teamId { get; set; }
            public string teamPosition { get; set; }
            public int timeCCingOthers { get; set; }
            public int timePlayed { get; set; }
            public int totalAllyJungleMinionsKilled { get; set; }
            public int totalDamageDealt { get; set; }
            public int totalDamageDealtToChampions { get; set; }
            public int totalDamageShieldedOnTeammates { get; set; }
            public int totalDamageTaken { get; set; }
            public int totalEnemyJungleMinionsKilled { get; set; }
            public int totalHeal { get; set; }
            public int totalHealsOnTeammates { get; set; }
            public int totalMinionsKilled { get; set; }
            public int totalTimeCCDealt { get; set; }
            public int totalTimeSpentDead { get; set; }
            public int totalUnitsHealed { get; set; }
            public int tripleKills { get; set; }
            public int trueDamageDealt { get; set; }
            public int trueDamageDealtToChampions { get; set; }
            public int trueDamageTaken { get; set; }
            public int turretKills { get; set; }
            public int turretTakedowns { get; set; }
            public int turretsLost { get; set; }
            public int unrealKills { get; set; }
            public int visionClearedPings { get; set; }
            public int visionScore { get; set; }
            public int visionWardsBoughtInGame { get; set; }
            public int wardsKilled { get; set; }
            public int wardsPlaced { get; set; }
            public bool win { get; set; }
        }

        public class Perks
        {
            public StatPerks statPerks { get; set; }
            public List<Style> styles { get; set; }
        }

        public class RiftHerald
        {
            public bool first { get; set; }
            public int kills { get; set; }
        }

        public class Selection
        {
            public int perk { get; set; }
            public int var1 { get; set; }
            public int var2 { get; set; }
            public int var3 { get; set; }
        }

        public class StatPerks
        {
            public int defense { get; set; }
            public int flex { get; set; }
            public int offense { get; set; }
        }

        public class Style
        {
            public string description { get; set; }
            public List<Selection> selections { get; set; }
            public int style { get; set; }
        }

        public class Team
        {
            public List<Ban> bans { get; set; }
            public Feats feats { get; set; }
            public Objectives objectives { get; set; }
            public int teamId { get; set; }
            public bool win { get; set; }
        }

        public class Tower
        {
            public bool first { get; set; }
            public int kills { get; set; }
        }


    }
}
