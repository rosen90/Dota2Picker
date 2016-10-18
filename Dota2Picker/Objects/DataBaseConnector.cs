using Dota2Picker.Models;
using SQLitePCL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Storage;

namespace Dota2Picker.Objects
{
    class DataBaseConnector
    {

        public const string dbName = "HeroData.db";
        private static DataBaseConnector db = new DataBaseConnector();
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

        public void LoadAllHeroes()
        {
             getAllHeroes();
        }
        
        public async void LoadHeroesByAttribute()
        {
            await Task.Run( () => getHeroesByAgility());
            await Task.Run( () => getHeroesByIntelligence());
            await Task.Run( () => getHeroesByStrength());
        }
        
        
        //Get All Heroes From DataBase
        public void getAllHeroes()
        {
            //< get records > 
            string query = @"SELECT hero.id, hero.name, hero.attack_id, hero.attribute_id, hero.role_id, attack.type AS type_name, attribute.name AS attribute_name FROM 
                            Heroes hero, AttackType attack, Attribute attribute Where hero.attack_id = attack.id AND
                            hero.attribute_id = attribute.id";

            using (var statement = new SQLite.SQLiteConnection(dbName))
            {
                 AllHeroesList = statement.Query<Hero>(query);
            }
        }

        public void getHeroesByStrength()
        {
            //< get records > 
            string query = @"SELECT hero.id, hero.name, hero.attack_id, hero.attribute_id, hero.role_id, attack.type AS type_name, attribute.name AS attribute_name FROM 
                            Heroes hero, AttackType attack, Attribute attribute Where hero.attack_id = attack.id AND
                            hero.attribute_id = attribute.id AND hero.attribute_id = 1";

            using (var statement = new SQLite.SQLiteConnection(dbName))
            {
                HeroesByStrengthList = statement.Query<Hero>(query);
            }
        }

        public void getHeroesByAgility()
        {
            //< get records > 
            string query = @"SELECT hero.id, hero.name, hero.attack_id, hero.attribute_id, hero.role_id, attack.type AS type_name, attribute.name AS attribute_name FROM 
                            Heroes hero, AttackType attack, Attribute attribute Where hero.attack_id = attack.id AND
                            hero.attribute_id = attribute.id AND hero.attribute_id = 2";

            using (var statement = new SQLite.SQLiteConnection(dbName))
            {
                HeroesByAgilityList = statement.Query<Hero>(query);
            }
        }

        public void getHeroesByIntelligence()
        {
            //< get records > 
            string query = @"SELECT hero.id, hero.name, hero.attack_id, hero.attribute_id, hero.role_id, attack.type AS type_name, attribute.name AS attribute_name FROM 
                            Heroes hero, AttackType attack, Attribute attribute Where hero.attack_id = attack.id AND
                            hero.attribute_id = attribute.id AND hero.attribute_id = 3";

            using (var statement = new SQLite.SQLiteConnection(dbName))
            {
                HeroesByIntelligenceList = statement.Query<Hero>(query);
            }
        }

        //Get all heroes that picked hero is weak against
        public List<IsWeakAgainst> GetWeakAgainst(int heroIndex)
        {
            List<IsWeakAgainst> heroes = new List<IsWeakAgainst>();

            string query = @"SELECT w.weak_id, h.name, cp.type  FROM Heroes h, Weak w, CounterType cp WHERE w.weak_id = h.id and w.counter_type = cp.id and w.id = " + heroIndex;

            using (var statement = new SQLite.SQLiteConnection(dbName))
            {
                heroes = statement.Query<IsWeakAgainst>(query);
            }

            return heroes;
        }

        //Get all heroes that picked hero is strong against
        public List<IsStrongAgainst> GetStrongAgainst(int heroIndex)
        {
            List<IsStrongAgainst> heroes = new List<IsStrongAgainst>();

            string query = @"SELECT s.strong_id, h.name, cp.type  FROM Heroes h, Strong s, CounterType cp WHERE s.strong_id = h.id and s.counter_type = cp.id and s.id = " + heroIndex;

            using (var statement = new SQLite.SQLiteConnection(dbName))
            {
                heroes = statement.Query<IsStrongAgainst>(query);
            }

            return heroes;
        }

        public List<Role> GetHeroRoles(int heroIndex)
        {
            List<Role> roles = new List<Role>();

            string query = @"SELECT r.name FROM Roles r, HeroRoles hr WHERE hr.id_role = r.id AND hr.id_hero = " + heroIndex;

            using (var statement = new SQLite.SQLiteConnection(dbName))
            {
                roles = statement.Query<Role>(query);
            }

            return roles;
        }
    }
}
