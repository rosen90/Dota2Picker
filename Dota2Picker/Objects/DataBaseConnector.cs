using Dota2Picker.Models;
using SQLitePCL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Storage;
using SQLite.Net;
using SQLite.Net.Async;
using SQLite.Net.Platform.WinRT;

namespace Dota2Picker.Objects
{
    class DataBaseConnector
    {
        public const string dbName = "HeroData.db";
        /*          connection element for SQLite universal platform            */
        //private static string dbFilePath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "HeroData.db");
        Func<SQLiteConnectionWithLock> connectionFactory =
                new Func<SQLiteConnectionWithLock>(
                    () =>
                    new SQLiteConnectionWithLock(
                        new SQLitePlatformWinRT(),
        new SQLiteConnectionString(dbName, storeDateTimeAsTicks: false)));


        private static DataBaseConnector db = new DataBaseConnector();

        private SQLiteAsyncConnection GetDbConnectionAsync()
        {
            var asyncConnection = new SQLiteAsyncConnection(connectionFactory);

            return asyncConnection;
        }

        public static DataBaseConnector dbInstance
        {
            get {
                if ( db == null)
                {
                    db = new DataBaseConnector();
                }
                return db;
            }
        }
       public static List<Hero> AllHeroesList = new List<Hero>();
       public static List<Hero> HeroesByStrengthList = new List<Hero>();
       public static List<Hero> HeroesByAgilityList = new List<Hero>();
       public static List<Hero> HeroesByIntelligenceList = new List<Hero>();

        public DataBaseConnector()
        {
          
        }
        
        //Get All Heroes From DataBase
        public async Task<List<Hero>> getAllHeroes()
        {
            if (AllHeroesList.Count == 0) { 
             //< get records > 
            string query = @"SELECT hero.id, hero.name, hero.attack_id, hero.attribute_id, hero.role_id, attack.type AS type_name, attribute.name AS attribute_name FROM 
                            Heroes hero, AttackType attack, Attribute attribute Where hero.attack_id = attack.id AND
                            hero.attribute_id = attribute.id";

            var connection = this.GetDbConnectionAsync();
            
            AllHeroesList = await  connection.QueryAsync<Hero>(query);
            }
            return AllHeroesList;
        }

        public async Task<List<Hero>> getHeroesByStrength()
        {
            if ( HeroesByStrengthList.Count == 0)
            { 
                //< get records > 
                string query = @"SELECT hero.id, hero.name, hero.attack_id, hero.attribute_id, hero.role_id, attack.type AS type_name, attribute.name AS attribute_name FROM 
                                Heroes hero, AttackType attack, Attribute attribute Where hero.attack_id = attack.id AND
                                hero.attribute_id = attribute.id AND hero.attribute_id = 1";
                var connection = this.GetDbConnectionAsync();

                HeroesByStrengthList = await connection.QueryAsync<Hero>(query);
            }
            return HeroesByStrengthList;
        }

        public async Task<List<Hero>> getHeroesByAgility()
        {
            if ( HeroesByAgilityList.Count == 0)
            {
                //< get records > 
                string query = @"SELECT hero.id, hero.name, hero.attack_id, hero.attribute_id, hero.role_id, attack.type AS type_name, attribute.name AS attribute_name FROM 
                                Heroes hero, AttackType attack, Attribute attribute Where hero.attack_id = attack.id AND
                                hero.attribute_id = attribute.id AND hero.attribute_id = 2";
                var connection = this.GetDbConnectionAsync();
                HeroesByAgilityList = await connection.QueryAsync<Hero>(query);
            }
            return HeroesByAgilityList;
        }

        public async Task<List<Hero>> getHeroesByIntelligence()
        {
            if ( HeroesByIntelligenceList.Count == 0 )
            { 
                //< get records > 
                string query = @"SELECT hero.id, hero.name, hero.attack_id, hero.attribute_id, hero.role_id, attack.type AS type_name, attribute.name AS attribute_name FROM 
                                Heroes hero, AttackType attack, Attribute attribute Where hero.attack_id = attack.id AND
                                hero.attribute_id = attribute.id AND hero.attribute_id = 3";
                var connection = this.GetDbConnectionAsync();
                HeroesByIntelligenceList = await connection.QueryAsync<Hero>(query);
            }
            return HeroesByIntelligenceList;
        }

        //Get all heroes that picked hero is weak against
        public async Task<List<IsWeakAgainst>> GetWeakAgainst(int heroIndex)
        {
            List<IsWeakAgainst> heroes = new List<IsWeakAgainst>();

            string query = @"SELECT w.weak_id, h.name, cp.type  FROM Heroes h, Weak w, CounterType cp WHERE w.weak_id = h.id and w.counter_type = cp.id and w.id = " + heroIndex;

            var connection = this.GetDbConnectionAsync();
            heroes = await connection.QueryAsync<IsWeakAgainst>(query);

            return heroes;
        }

        //Get all heroes that picked hero is strong against
        public async Task<List<IsStrongAgainst>> GetStrongAgainst(int heroIndex)
        {
            List<IsStrongAgainst> heroes = new List<IsStrongAgainst>();

            string query = @"SELECT s.strong_id, h.name, cp.type  FROM Heroes h, Strong s, CounterType cp WHERE s.strong_id = h.id and s.counter_type = cp.id and s.id = " + heroIndex;

            var connection = this.GetDbConnectionAsync();
            heroes = await connection.QueryAsync<IsStrongAgainst>(query);

            return heroes;
        }

        public async Task<List<Role>> GetHeroRoles(int heroIndex)
        {
            List<Role> roles = new List<Role>();

            string query = @"SELECT r.name FROM Roles r, HeroRoles hr WHERE hr.id_role = r.id AND hr.id_hero = " + heroIndex;

            var connection = this.GetDbConnectionAsync();
            roles = await connection.QueryAsync<Role>(query);

            return roles;
        }
    }
}
