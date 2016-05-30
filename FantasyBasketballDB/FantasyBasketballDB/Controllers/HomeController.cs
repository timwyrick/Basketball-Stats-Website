using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Diagnostics;
using Newtonsoft.Json.Linq;
using FantasyBasketballDB.Models;
using FantasyBasketballDB.DAL;

namespace FantasyBasketballDB.Controllers
{
    public class HomeController : Controller
    {
        private FantasyBasketballDBContext db = new FantasyBasketballDBContext();

        public IWebDriver CreateSeleniumDriver()
        {
            return new ChromeDriver(); 
        }

        public void testIfSeleniumWorks()
        {
            /*
            IWebDriver testDriver = CreateSeleniumDriver();
            testDriver.Navigate().GoToUrl(@"http://stats.nba.com/scores/?ls=iref:nba:gnav#!/11/04/1946");
            testDriver.Manage().Window.Maximize();
            System.Threading.Thread.Sleep(1000);
            testDriver.FindElement(By.ClassName("game-footer__bs")).Click();
            
            string tempString = testDriver.FindElement(By.ClassName("player-name")).Text;
            testDriver.FindElement(By.Id("tab-career")).Click();
            Debug.WriteLine(tempString);
            */

            using (var webClient = new System.Net.WebClient())
            {
                //Beginning with the Game ID of the first NBA game in 1946
                int gameID = 0024600001;

                

                Game ExistingGame = db.Games.Where(g => g.GameID == gameID).FirstOrDefault();

                //Check to see if game is already in database
                if(ExistingGame != null)
                {
                    return;
                }
                else
                {
                    //Download the JSON object through stats.nba.com's API
                    string url = "http://stats.nba.com/stats/boxscore/?GameID=00" + gameID + "&StartPeriod=0&EndPeriod=10&StartRange=0&EndRange=0&RangeType=0";
                    var json = webClient.DownloadString(url);

                    //Parse through JSON object 
                    JObject gameJSON = JObject.Parse(json);

                    //Step 1: Get game data
                    string getDate = (string)gameJSON["resultSets"][0]["rowSet"][0][0];
                    int getGameID = (int)gameJSON["resultSets"][0]["rowSet"][0][2];
                    int getSeason = (int)gameJSON["resultSets"][0]["rowSet"][0][8];

                    Game newGame = new Game();
                    newGame.Date = getDate;
                    newGame.GameID = gameID;
                    newGame.Season = getSeason;

                    //Step 2: Get team data for teams playing in the game
                    int getHomeTeamID = (int)gameJSON["resultSets"][0]["rowSet"][0][6];
                    AddTeamData(gameJSON, getHomeTeamID, true, getSeason, newGame);
                    

                    int getAwayTeamID = (int)gameJSON["resultSets"][0]["rowSet"][0][7];
                    AddTeamData(gameJSON, getAwayTeamID, false, getSeason, newGame);
                    

                    //Add game to database
                    db.Games.Add(newGame);
                    db.SaveChanges();

                    //Step 3: Retrieve player stats and add new players to database

                    //Retrieve the number of players 
                    JArray players = (JArray)gameJSON["resultSets"][4]["rowSet"];
                    int numPlayers = players.Count;

                    System.Diagnostics.Debug.WriteLine(numPlayers);
                    //Iterate through JSONArray of players
                    for (int i = 0; i < numPlayers; i++)
                    {
                        AddPlayerData(gameJSON, i, getSeason);
                    }
                }
            }        
        }

        /// <summary>
        /// Checks to see if a team is completely new. 
        /// If so, a new Team object is created and information is filled.
        /// If not, the team owning the particular teamID is returned.
        /// </summary>
        /// <param name="gameJSON">The JSONObject that contains the team's data</param>
        /// <param name="teamID">An ID unique to each team in the NBA.</param>
        /// <returns></returns>
        public void AddTeamData(JObject gameJSON, int teamID, bool isHome, int year, Game currGame)
        {
            Team team = db.Teams.Where(t => t.TeamID == teamID).FirstOrDefault();

            //If team doesn't exist, create a new team and add it to database
            if (team == null && isHome == true)
            {
                Team newHomeTeam = new Team();
                newHomeTeam.Players = new List<Player>();
                newHomeTeam.Seasons = new List<TeamSeason>();
                newHomeTeam.TeamID = teamID;
                newHomeTeam.YearEstablished = year;
                newHomeTeam.YearClosed = 9999;
                newHomeTeam.Name = (string)gameJSON["resultSets"][5]["rowSet"][0][2];
                newHomeTeam.NickName = (string)gameJSON["resultSets"][5]["rowSet"][0][3];
                newHomeTeam.City = (string)gameJSON["resultSets"][5]["rowSet"][0][4];
                newHomeTeam.Wins = 0;
                newHomeTeam.Losses = 0;
                db.Teams.Add(newHomeTeam);
                db.SaveChanges();

                getTeamGameData(gameJSON, newHomeTeam, isHome, year, currGame);
            }
            //Away team is new
            else if(team == null && isHome == false)
            {
                Team newAwayTeam = new Team();
                newAwayTeam.Players = new List<Player>();
                newAwayTeam.Seasons = new List<TeamSeason>();
                newAwayTeam.TeamID = teamID;
                newAwayTeam.YearEstablished = year;
                newAwayTeam.YearClosed = 9999;
                newAwayTeam.Name = (string)gameJSON["resultSets"][5]["rowSet"][1][2];
                newAwayTeam.NickName = (string)gameJSON["resultSets"][5]["rowSet"][1][3];
                newAwayTeam.City = (string)gameJSON["resultSets"][5]["rowSet"][1][4];
                newAwayTeam.Wins = 0;
                newAwayTeam.Losses = 0;
                db.Teams.Add(newAwayTeam);
                db.SaveChanges();

                getTeamGameData(gameJSON, newAwayTeam, isHome, year, currGame);
            }
            //Team already exists
            else
            {
                getTeamGameData(gameJSON, team, isHome, year, currGame);
                return;
            }
        }

        /// <summary>
        /// Creates a new TeamGame object for each team, and a new TeamSeason object
        /// if it is the first game of the season for that team.
        /// </summary>
        /// <param name="gameJSON">JSON used to get game data.</param>
        /// <param name="team">Team being examined.</param>
        /// <param name="isHome">Whether the team is playing at home or not.</param>
        /// <param name="year">The year the current NBA season takes place.</param>
        /// <param name="currGame">The current game being analyzed.</param>
        public void getTeamGameData(JObject gameJSON, Team team, bool isHome, int year, Game currGame)
        {
            //Check to see if this is the first game of the NBA season
            Season season = db.Seasons.Where(e => e.Year == year).FirstOrDefault();
            if(season == null)
            {
                Season newSeason = new Season();
                newSeason.TeamSeasons = new List<TeamSeason>();
                db.Seasons.Add(newSeason);
                db.SaveChanges();
            }
            //This is not the first game of this NBA season
            else
            {
                //Look up current season
                Season thisSeason = db.Seasons.Where(e => e.Year == year).FirstOrDefault();

                //The team is the home team for this game
                if (isHome)
                {
                    //Check to see if this is the first game of the season for this team
                    TeamSeason homeTeamSeason = team.Seasons.Where(s => s.Year == year).FirstOrDefault();

                    //This is the home team's first game of the season
                    if (homeTeamSeason == null)
                    {
                        TeamSeason newTeamSeason = new TeamSeason();
                        newTeamSeason.Games = new List<TeamGame>();
                        newTeamSeason.Players = new List<Player>();
                        newTeamSeason.Wins = 0;
                        newTeamSeason.Losses = 0;
                        newTeamSeason.Year = year;
                        newTeamSeason.Team = team;
                        team.Seasons.Add(newTeamSeason);
                        thisSeason.TeamSeasons.Add(newTeamSeason);

                        TeamGame newTeamGame = new TeamGame();
                        newTeamGame.Players = new List<PlayerGame>();
                        newTeamGame.Game = currGame;
                        newTeamGame.TeamSeason = newTeamSeason;
                        newTeamSeason.Games.Add(newTeamGame);
                        currGame.HomeTeamGame = newTeamGame;
                        newTeamGame.FirstQuarterPoints = (int)gameJSON["resultSets"][1]["rowSet"][0][7];
                        newTeamGame.SecondQuarterPoints = (int)gameJSON["resultSets"][1]["rowSet"][0][8];
                        newTeamGame.ThirdQuarterPoints = (int)gameJSON["resultSets"][1]["rowSet"][0][9];
                        newTeamGame.FourthQuarterPoints = (int)gameJSON["resultSets"][1]["rowSet"][0][10];
                        newTeamGame.Overtime1Points = (int)gameJSON["resultSets"][1]["rowSet"][0][11];
                        newTeamGame.Overtime2Points = (int)gameJSON["resultSets"][1]["rowSet"][0][12];
                        newTeamGame.Overtime3Points = (int)gameJSON["resultSets"][1]["rowSet"][0][13];
                        newTeamGame.Overtime4Points = (int)gameJSON["resultSets"][1]["rowSet"][0][14];
                        newTeamGame.Overtime5Points = (int)gameJSON["resultSets"][1]["rowSet"][0][15];
                        newTeamGame.Overtime6Points = (int)gameJSON["resultSets"][1]["rowSet"][0][16];

                        db.TeamSeasons.Add(newTeamSeason);
                        db.TeamGames.Add(newTeamGame);
                        db.SaveChanges();
                    }
                    //This is not the home team's first game of the season
                    else
                    {
                        TeamGame newTeamGame = new TeamGame();
                        newTeamGame.Players = new List<PlayerGame>();
                        newTeamGame.Game = currGame;
                        newTeamGame.TeamSeason = homeTeamSeason;
                        homeTeamSeason.Games.Add(newTeamGame);
                        currGame.HomeTeamGame = newTeamGame;
                        newTeamGame.FirstQuarterPoints = (int)gameJSON["resultSets"][1]["rowSet"][0][7];
                        newTeamGame.SecondQuarterPoints = (int)gameJSON["resultSets"][1]["rowSet"][0][8];
                        newTeamGame.ThirdQuarterPoints = (int)gameJSON["resultSets"][1]["rowSet"][0][9];
                        newTeamGame.FourthQuarterPoints = (int)gameJSON["resultSets"][1]["rowSet"][0][10];
                        newTeamGame.Overtime1Points = (int)gameJSON["resultSets"][1]["rowSet"][0][11];
                        newTeamGame.Overtime2Points = (int)gameJSON["resultSets"][1]["rowSet"][0][12];
                        newTeamGame.Overtime3Points = (int)gameJSON["resultSets"][1]["rowSet"][0][13];
                        newTeamGame.Overtime4Points = (int)gameJSON["resultSets"][1]["rowSet"][0][14];
                        newTeamGame.Overtime5Points = (int)gameJSON["resultSets"][1]["rowSet"][0][15];
                        newTeamGame.Overtime6Points = (int)gameJSON["resultSets"][1]["rowSet"][0][16];

                        db.TeamGames.Add(newTeamGame);
                        db.SaveChanges();
                    }
                }
                //The team is playing away
                else
                {
                    //Check to see if this is the first game of the season for this team
                    TeamSeason awayTeamSeason = team.Seasons.Where(s => s.Year == year).FirstOrDefault();

                    if (awayTeamSeason == null)
                    {
                        TeamSeason newTeamSeason = new TeamSeason();
                        newTeamSeason.Games = new List<TeamGame>();
                        newTeamSeason.Players = new List<Player>();
                        newTeamSeason.Wins = 0;
                        newTeamSeason.Losses = 0;
                        newTeamSeason.Year = year;
                        newTeamSeason.Team = team;
                        team.Seasons.Add(newTeamSeason);
                        thisSeason.TeamSeasons.Add(newTeamSeason);

                        TeamGame newTeamGame = new TeamGame();
                        newTeamGame.Players = new List<PlayerGame>();
                        newTeamGame.Game = currGame;
                        newTeamGame.TeamSeason = newTeamSeason;
                        newTeamSeason.Games.Add(newTeamGame);
                        currGame.AwayTeamGame = newTeamGame;
                        newTeamGame.FirstQuarterPoints = (int)gameJSON["resultSets"][1]["rowSet"][1][7];
                        newTeamGame.SecondQuarterPoints = (int)gameJSON["resultSets"][1]["rowSet"][1][8];
                        newTeamGame.ThirdQuarterPoints = (int)gameJSON["resultSets"][1]["rowSet"][1][9];
                        newTeamGame.FourthQuarterPoints = (int)gameJSON["resultSets"][1]["rowSet"][1][10];
                        newTeamGame.Overtime1Points = (int)gameJSON["resultSets"][1]["rowSet"][1][11];
                        newTeamGame.Overtime2Points = (int)gameJSON["resultSets"][1]["rowSet"][1][12];
                        newTeamGame.Overtime3Points = (int)gameJSON["resultSets"][1]["rowSet"][1][13];
                        newTeamGame.Overtime4Points = (int)gameJSON["resultSets"][1]["rowSet"][1][14];
                        newTeamGame.Overtime5Points = (int)gameJSON["resultSets"][1]["rowSet"][1][15];
                        newTeamGame.Overtime6Points = (int)gameJSON["resultSets"][1]["rowSet"][1][16];

                        db.TeamSeasons.Add(newTeamSeason);
                        db.TeamGames.Add(newTeamGame);
                        db.SaveChanges();
                    }
                    //This is not the away team's first game of the season
                    else
                    {
                        TeamGame newTeamGame = new TeamGame();
                        newTeamGame.Players = new List<PlayerGame>();
                        newTeamGame.Game = currGame;
                        newTeamGame.TeamSeason = awayTeamSeason;
                        awayTeamSeason.Games.Add(newTeamGame);
                        currGame.AwayTeamGame = newTeamGame;
                        newTeamGame.FirstQuarterPoints = (int)gameJSON["resultSets"][1]["rowSet"][1][7];
                        newTeamGame.SecondQuarterPoints = (int)gameJSON["resultSets"][1]["rowSet"][1][8];
                        newTeamGame.ThirdQuarterPoints = (int)gameJSON["resultSets"][1]["rowSet"][1][9];
                        newTeamGame.FourthQuarterPoints = (int)gameJSON["resultSets"][1]["rowSet"][1][10];
                        newTeamGame.Overtime1Points = (int)gameJSON["resultSets"][1]["rowSet"][1][11];
                        newTeamGame.Overtime2Points = (int)gameJSON["resultSets"][1]["rowSet"][1][12];
                        newTeamGame.Overtime3Points = (int)gameJSON["resultSets"][1]["rowSet"][1][13];
                        newTeamGame.Overtime4Points = (int)gameJSON["resultSets"][1]["rowSet"][1][14];
                        newTeamGame.Overtime5Points = (int)gameJSON["resultSets"][1]["rowSet"][1][15];
                        newTeamGame.Overtime6Points = (int)gameJSON["resultSets"][1]["rowSet"][1][16];

                        db.TeamGames.Add(newTeamGame);
                        db.SaveChanges();
                    }
                }
            }
        }

        /// <summary>
        /// Adds new game data to a player, and checks for 3 special conditions:
        /// 1. It is the player's first game ever in the NBA
        /// 2. It is the player's first game in the particular season
        /// 3. It is the player's first game with a new team in the same season
        /// </summary>
        /// <param name="testJSON">The JSON from NBA's API from which player data is extracted.</param>
        /// <param name="index">The index of the "PlayerStats" JSONArray in the JSON Object where the player's stats are.</param>
        public void AddPlayerData(JObject testJSON, int index, int year)
        {
            int playerID = (int)testJSON["resultSets"][4]["rowSet"][0 + index][4];
            Player currentPlayer = db.Players.Where(p => p.PlayerID == playerID).FirstOrDefault();

            int currentTeamID = (int)testJSON["resultSets"][4]["rowSet"][0 + index][1];
            Team currentTeam = db.Teams.Where(c => c.TeamID == currentTeamID).FirstOrDefault();

            //Player is completely new, and this is their first game
            if (currentPlayer == null)
            {
                CreateNewPlayer(testJSON, index, playerID);
            }
            //This is the player's first game in this season
            else if(currentPlayer.Seasons.Where(s => s.Year == year) == null)
            {
                CreatePlayerNewSeason(currentPlayer, testJSON, index);
            }
            //This is the player's first game with a new team in the same season ***NOT RIGOROUS ENOUGH: A -> B -> A
            else if(!currentPlayer.Teams.Contains(currentTeam))
            {
                CreatePlayerNewSeason(currentPlayer, testJSON, index);
            }
            //This is another game in the same season for the same team for the player
            else
            {
                CreateNewPlayerGame(currentPlayer, testJSON, index, year);
            }
        }

        /// <summary>
        /// Creates a new Player, PlayerSeason, and PlayerGame and takes data
        /// from the JSON of a game using NBA's API. This method is called
        /// for a player's first NBA game.
        /// </summary>
        /// <param name="testJSON">The JSON object that is being used to find the player's data.</param>
        /// <param name="index">The index of the "PlayerStats" JSONArray where the particular player's stats are.</param>
        /// <param name="playerID"></param>
        public void CreateNewPlayer(JObject testJSON, int index, int playerID)
        {
            Player newPlayer = new Player();
            newPlayer.Teams = new List<Team>();
            newPlayer.Seasons = new List<PlayerSeason>();
            newPlayer.PlayerID = playerID;
            newPlayer.Name = (string)testJSON["resultSets"][4]["rowSet"][0 + index][5];
            newPlayer.DraftYear = 9999;
            newPlayer.DraftPosition = 9999;
            newPlayer.College = "foo";

            //Look up team the player plays for, using nba team ID
            int currTeamID = (int)testJSON["resultSets"][4]["rowSet"][0 + index][1];
            Team currTeam = db.Teams.Where(c => c.TeamID == currTeamID).FirstOrDefault();
            newPlayer.Teams.Add(currTeam);
            currTeam.Players.Add(newPlayer);

            //Make a new PlayerSeason for the new player
            PlayerSeason newPlayerSeason = new PlayerSeason();
            newPlayerSeason.Games = new List<PlayerGame>();
            newPlayerSeason.Year = (int)testJSON["resultSets"][0]["rowSet"][0][8];
            newPlayerSeason.Player = newPlayer;
            newPlayerSeason.TeamPlayedFor = currTeam;
            newPlayer.Seasons.Add(newPlayerSeason);

            //Create a new PlayerGame for the current game
            PlayerGame newPlayerGame = new PlayerGame();
            newPlayerGame.PlayerSeason = newPlayerSeason;
            newPlayerSeason.Games.Add(newPlayerGame);

            //Get stats from respective player's JSONArray
            newPlayerGame.Date = (string)testJSON["resultSets"][0]["rowSet"][0][0];
            newPlayerGame.Player = newPlayer;
            newPlayerGame.GameID = (int)testJSON["resultSets"][0]["rowSet"][0][2];
            newPlayerGame.Minutes = (int?)testJSON["resultSets"][4]["rowSet"][0 + index][8];
            newPlayerGame.FieldGoals = (int?)testJSON["resultSets"][4]["rowSet"][0 + index][9];
            newPlayerGame.FieldGoalAttempts = (int?)testJSON["resultSets"][4]["rowSet"][0 + index][10];
            newPlayerGame.ThreePoints = (int?)testJSON["resultSets"][4]["rowSet"][0 + index][12];
            newPlayerGame.ThreePointAttempts = (int?)testJSON["resultSets"][4]["rowSet"][0 + index][13];
            newPlayerGame.FreeThrows = (int?)testJSON["resultSets"][4]["rowSet"][0 + index][15];
            newPlayerGame.FreeThrowAttempts = (int?)testJSON["resultSets"][4]["rowSet"][0 + index][16];
            newPlayerGame.OffensiveRebounds = (int?)testJSON["resultSets"][4]["rowSet"][0 + index][18];
            newPlayerGame.DefensiveRebounds = (int?)testJSON["resultSets"][4]["rowSet"][0 + index][19];
            newPlayerGame.Rebounds = (int?)testJSON["resultSets"][4]["rowSet"][0 + index][20];
            newPlayerGame.Assists = (int?)testJSON["resultSets"][4]["rowSet"][0 + index][21];
            newPlayerGame.Steals = (int?)testJSON["resultSets"][4]["rowSet"][0 + index][22];
            newPlayerGame.Blocks = (int?)testJSON["resultSets"][4]["rowSet"][0 + index][23];
            newPlayerGame.Fouls = (int?)testJSON["resultSets"][4]["rowSet"][0 + index][24];
            newPlayerGame.Turnovers = (int?)testJSON["resultSets"][4]["rowSet"][0 + index][25];
            newPlayerGame.Points = (int?)testJSON["resultSets"][4]["rowSet"][0 + index][26];
            newPlayerGame.PlusMinus = (int?)testJSON["resultSets"][4]["rowSet"][0 + index][27];

            db.Players.Add(newPlayer);
            db.PlayerSeasons.Add(newPlayerSeason);
            db.PlayerGames.Add(newPlayerGame);
            db.SaveChanges();

            CopyPlayerGameData(newPlayer, newPlayerSeason, newPlayerGame);
        }

        /// <summary>
        /// Used when it is a player's first game in a particular season, but not their first game ever.
        /// Update's a Player's stats by creating a new PlayerSeason and PlayerGame.
        /// </summary>
        /// <param name="testJSON">The JSON containing the player data taken using NBA's API.</param>
        /// <param name="index">The index of the "PlayerStats" JSONArray where the particular player's stats are.</param>
        public void CreatePlayerNewSeason(Player currPlayer, JObject testJSON, int index)
        {
            //Look up team the player plays for, using nba team ID
            int currTeamID = (int)testJSON["resultSets"][4]["rowSet"][0 + index][1];
            Team currTeam = db.Teams.Where(c => c.TeamID == currTeamID).FirstOrDefault();

            //Checks to see if player is playing for a  new team this season
            if(!currPlayer.Teams.Contains(currTeam))
            {
                //Add new team to player's Team's list, and vice versa
                currPlayer.Teams.Add(currTeam);
                currTeam.Players.Add(currPlayer);
            }

            //Make a new PlayerSeason for the new player
            PlayerSeason newPlayerSeason = new PlayerSeason();
            newPlayerSeason.Games = new List<PlayerGame>();
            newPlayerSeason.Year = (int)testJSON["resultSets"][0]["rowSet"][0][8];
            newPlayerSeason.Player = currPlayer;
            newPlayerSeason.TeamPlayedFor = currTeam;
            currPlayer.Seasons.Add(newPlayerSeason);

            //Create a new PlayerGame for the current game
            PlayerGame newPlayerGame = new PlayerGame();
            newPlayerGame.PlayerSeason = newPlayerSeason;
            newPlayerSeason.Games.Add(newPlayerGame);

            // Get stats from respective player's JSONArray
            newPlayerGame.Date = (string)testJSON["resultSets"][0]["rowSet"][0][0];
            newPlayerGame.Player = currPlayer;
            newPlayerGame.GameID = (int)testJSON["resultSets"][0]["rowSet"][0][2];
            newPlayerGame.Minutes = (int?)testJSON["resultSets"][4]["rowSet"][0 + index][8];
            newPlayerGame.FieldGoals = (int?)testJSON["resultSets"][4]["rowSet"][0 + index][9];
            newPlayerGame.FieldGoalAttempts = (int?)testJSON["resultSets"][4]["rowSet"][0 + index][10];
            newPlayerGame.ThreePoints = (int?)testJSON["resultSets"][4]["rowSet"][0 + index][12];
            newPlayerGame.ThreePointAttempts = (int?)testJSON["resultSets"][4]["rowSet"][0 + index][13];
            newPlayerGame.FreeThrows = (int?)testJSON["resultSets"][4]["rowSet"][0 + index][15];
            newPlayerGame.FreeThrowAttempts = (int?)testJSON["resultSets"][4]["rowSet"][0 + index][16];
            newPlayerGame.OffensiveRebounds = (int?)testJSON["resultSets"][4]["rowSet"][0 + index][18];
            newPlayerGame.DefensiveRebounds = (int?)testJSON["resultSets"][4]["rowSet"][0 + index][19];
            newPlayerGame.Rebounds = (int?)testJSON["resultSets"][4]["rowSet"][0 + index][20];
            newPlayerGame.Assists = (int?)testJSON["resultSets"][4]["rowSet"][0 + index][21];
            newPlayerGame.Steals = (int?)testJSON["resultSets"][4]["rowSet"][0 + index][22];
            newPlayerGame.Blocks = (int?)testJSON["resultSets"][4]["rowSet"][0 + index][23];
            newPlayerGame.Fouls = (int?)testJSON["resultSets"][4]["rowSet"][0 + index][24];
            newPlayerGame.Turnovers = (int?)testJSON["resultSets"][4]["rowSet"][0 + index][25];
            newPlayerGame.Points = (int?)testJSON["resultSets"][4]["rowSet"][0 + index][26];
            newPlayerGame.PlusMinus = (int?)testJSON["resultSets"][4]["rowSet"][0 + index][27];



            db.PlayerSeasons.Add(newPlayerSeason);
            db.PlayerGames.Add(newPlayerGame);
            db.SaveChanges();

            CopyPlayerGameData(currPlayer, newPlayerSeason, newPlayerGame);

        }

        /// <summary>
        /// Used when a player plays a new game for their original team 
        /// </summary>
        /// <param name="currPlayer"></param>
        /// <param name="testJSON"></param>
        /// <param name="index"></param>
        /// <param name="year"></param>
        public void CreateNewPlayerGame(Player currPlayer, JObject testJSON, int index, int year)
        {
            PlayerSeason currPlayerSeason = currPlayer.Seasons.Where(p=> p.Year == year).FirstOrDefault();

            //Create a new PlayerGame for the current game
            PlayerGame newPlayerGame = new PlayerGame();
            newPlayerGame.PlayerSeason = currPlayerSeason;
            currPlayerSeason.Games.Add(newPlayerGame);

            // Get stats from respective player's JSONArray
            newPlayerGame.Date = (string)testJSON["resultSets"][0]["rowSet"][0][0];
            newPlayerGame.Player = currPlayer;
            newPlayerGame.GameID = (int)testJSON["resultSets"][0]["rowSet"][0][2];
            newPlayerGame.Minutes = (int?)testJSON["resultSets"][4]["rowSet"][0 + index][8];
            newPlayerGame.FieldGoals = (int?)testJSON["resultSets"][4]["rowSet"][0 + index][9];
            newPlayerGame.FieldGoalAttempts = (int?)testJSON["resultSets"][4]["rowSet"][0 + index][10];
            newPlayerGame.ThreePoints = (int?)testJSON["resultSets"][4]["rowSet"][0 + index][12];
            newPlayerGame.ThreePointAttempts = (int?)testJSON["resultSets"][4]["rowSet"][0 + index][13];
            newPlayerGame.FreeThrows = (int?)testJSON["resultSets"][4]["rowSet"][0 + index][15];
            newPlayerGame.FreeThrowAttempts = (int?)testJSON["resultSets"][4]["rowSet"][0 + index][16];
            newPlayerGame.OffensiveRebounds = (int?)testJSON["resultSets"][4]["rowSet"][0 + index][18];
            newPlayerGame.DefensiveRebounds = (int?)testJSON["resultSets"][4]["rowSet"][0 + index][19];
            newPlayerGame.Rebounds = (int?)testJSON["resultSets"][4]["rowSet"][0 + index][20];
            newPlayerGame.Assists = (int?)testJSON["resultSets"][4]["rowSet"][0 + index][21];
            newPlayerGame.Steals = (int?)testJSON["resultSets"][4]["rowSet"][0 + index][22];
            newPlayerGame.Blocks = (int?)testJSON["resultSets"][4]["rowSet"][0 + index][23];
            newPlayerGame.Fouls = (int?)testJSON["resultSets"][4]["rowSet"][0 + index][24];
            newPlayerGame.Turnovers = (int?)testJSON["resultSets"][4]["rowSet"][0 + index][25];
            newPlayerGame.Points = (int?)testJSON["resultSets"][4]["rowSet"][0 + index][26];
            newPlayerGame.PlusMinus = (int?)testJSON["resultSets"][4]["rowSet"][0 + index][27];


            CopyPlayerGameData(currPlayer, currPlayerSeason, newPlayerGame);
        }


        /// <summary>
        /// Updates a Player's and a PlayerSeason's stats with
        /// the stats of a PlayerGame.
        /// </summary>
        /// <param name="player">The Player whose stats are being updated.</param>
        /// <param name="season">The PlayerSeason whose stats are being updated.</param>
        /// <param name="game">The PlayerGame who's stats are being used to update.</param>
        public void CopyPlayerGameData(Player player, PlayerSeason season, PlayerGame game)
        {
            //Add new PlayerGame data to current PlayerSeason's totals
            season.Minutes = season.Minutes.GetValueOrDefault() + game.Minutes;
            season.FieldGoals = season.FieldGoals.GetValueOrDefault() + game.FieldGoals;
            season.FieldGoalAttempts = season.FieldGoalAttempts.GetValueOrDefault() + game.FieldGoalAttempts;
            season.ThreePoints = season.ThreePoints.GetValueOrDefault() + game.ThreePoints;
            season.ThreePointAttempts = season.ThreePointAttempts.GetValueOrDefault() + game.ThreePointAttempts;
            season.FreeThrows = season.FreeThrows.GetValueOrDefault() + game.FreeThrows;
            season.FreeThrowAttempts = season.FreeThrowAttempts.GetValueOrDefault() + game.FreeThrowAttempts;
            season.OffensiveRebounds = season.OffensiveRebounds.GetValueOrDefault() + game.OffensiveRebounds;
            season.DefensiveRebounds = season.DefensiveRebounds.GetValueOrDefault() + game.DefensiveRebounds;
            season.Rebounds = season.Rebounds.GetValueOrDefault() + game.Rebounds;
            season.Assists = season.Assists.GetValueOrDefault() + game.Assists;
            season.Steals = season.Steals.GetValueOrDefault() + game.Steals;
            season.Blocks = season.Blocks.GetValueOrDefault() + game.Blocks;
            season.Fouls  = season.Fouls.GetValueOrDefault() + game.Fouls;
            season.Turnovers = season.Turnovers.GetValueOrDefault() + game.Turnovers;
            season.Points = season.Points.GetValueOrDefault() + game.Points;
            season.PlusMinus = season.PlusMinus.GetValueOrDefault() + game.PlusMinus;

            //Add new PlayerGame data to Player's career totals
            player.Minutes = player.Minutes.GetValueOrDefault() + game.Minutes;
            player.FieldGoals = player.FieldGoals.GetValueOrDefault() + game.FieldGoals;
            player.FieldGoalAttempts = player.FieldGoalAttempts.GetValueOrDefault() + game.FieldGoalAttempts;
            player.ThreePoints = player.ThreePoints.GetValueOrDefault() + game.ThreePoints;
            player.ThreePointAttempts = player.ThreePointAttempts.GetValueOrDefault() + game.ThreePointAttempts;
            player.FreeThrows = player.FreeThrows.GetValueOrDefault() + game.FreeThrows;
            player.FreeThrowAttempts = player.FreeThrowAttempts.GetValueOrDefault() + game.FreeThrowAttempts;
            player.OffensiveRebounds = player.OffensiveRebounds.GetValueOrDefault() + game.OffensiveRebounds;
            player.DefensiveRebounds = player.DefensiveRebounds.GetValueOrDefault() + game.DefensiveRebounds;
            player.Rebounds = player.Rebounds.GetValueOrDefault() + game.Rebounds;
            player.Assists = player.Assists.GetValueOrDefault() + game.Assists;
            player.Steals = player.Steals.GetValueOrDefault() + game.Steals;
            player.Blocks = player.Blocks.GetValueOrDefault() + game.Blocks;
            player.Fouls = player.Fouls.GetValueOrDefault() + game.Fouls;
            player.Turnovers = player.Turnovers.GetValueOrDefault() + game.Turnovers;
            player.Points = player.Points.GetValueOrDefault() + game.Points;
            player.PlusMinus = player.PlusMinus.GetValueOrDefault() + game.PlusMinus;

            db.SaveChanges();
        }

        /// <summary>
        /// Copies the data from a TeamGame to a TeamSeason,
        /// to reduce computation when looking up team stats.
        /// </summary>
        /// <param name="season">The TeamSeason being updated.</param>
        /// <param name="game">The TeamGame whose data updates the TeamSeason.</param>
        public void CopyTeamGameData(TeamSeason season, TeamGame game)
        {

            season.Minutes = season.Minutes.GetValueOrDefault() + game.Minutes;
            season.FieldGoals = season.FieldGoals.GetValueOrDefault() + game.FieldGoals;
            season.FieldGoalAttempts = season.FieldGoalAttempts.GetValueOrDefault() + game.FieldGoalAttempts;
            season.ThreePoints = season.ThreePoints.GetValueOrDefault() + game.ThreePoints;
            season.ThreePointAttempts = season.ThreePointAttempts.GetValueOrDefault() + game.ThreePointAttempts;
            season.FreeThrows = season.FreeThrows.GetValueOrDefault() + game.FreeThrows;
            season.FreeThrowAttempts = season.FreeThrowAttempts.GetValueOrDefault() + game.FreeThrowAttempts;
            season.OffensiveRebounds = season.OffensiveRebounds.GetValueOrDefault() + game.OffensiveRebounds;
            season.DefensiveRebounds = season.DefensiveRebounds.GetValueOrDefault() + game.DefensiveRebounds;
            season.Rebounds = season.Rebounds.GetValueOrDefault() + game.Rebounds;
            season.Assists = season.Assists.GetValueOrDefault() + game.Assists;
            season.Steals = season.Steals.GetValueOrDefault() + game.Steals;
            season.Blocks = season.Blocks.GetValueOrDefault() + game.Blocks;
            season.Fouls = season.Fouls.GetValueOrDefault() + game.Fouls;
            season.Turnovers = season.Turnovers.GetValueOrDefault() + game.Turnovers;
            season.Points = season.Points.GetValueOrDefault() + game.Points;
            

            db.SaveChanges();
        }



        public ActionResult Index()
        {
            testIfSeleniumWorks();
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}