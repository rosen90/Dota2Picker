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
        
        public void LoadHeroesByAttribute()
        {
            getHeroesByAgility();
            getHeroesByIntelligence();
            getHeroesByStrength();
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
        public List<Hero> GetWeakAgainst(int heroIndex)
        {
            List<Hero> heroes = new List<Hero>();

            string query = @"SELECT h.id, h.name FROM Heroes h, Weak w WHERE w.weak_id = h.id and w.id = " + heroIndex;

            using (var statement = new SQLite.SQLiteConnection(dbName))
            {
                heroes = statement.Query<Hero>(query);
            }

            return heroes;
        }

        //Get all heroes that picked hero is strong against
        public List<Hero> GetStrongAgainst(int heroIndex)
        {
            List<Hero> heroes = new List<Hero>();

            string query = @"SELECT h.id, h.name FROM Heroes h, Strong s WHERE s.strong_id = h.id AND s.id = " + heroIndex;

            using (var statement = new SQLite.SQLiteConnection(dbName))
            {
                heroes = statement.Query<Hero>(query);
            }

            return heroes;
        }

    }
}
