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

        public DataBaseConnector()
        {
          
        }

        //Get All Heroes From DataBase
        public List<Hero> getAllHeroes()
        {
            List<Hero> heroes = new List<Hero>();

            //< get records > 
            string query = @"SELECT hero.id, hero.name, hero.attack_id, hero.attribute_id, hero.role_id, attack.type AS type_name, attribute.name AS attribute_name FROM 
                            Heroes hero, AttackType attack, Attribute attribute Where hero.attack_id = attack.id AND
                            hero.attribute_id = attribute.id";
            using (var statement = new SQLite.SQLiteConnection(dbName))
            {
                heroes = statement.Query<Hero>(query);
            }
            return heroes;
        }
        
    }
}
