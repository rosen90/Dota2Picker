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

        public SQLiteConnection dbConnection = new SQLiteConnection(dbName);

        public DataBaseConnector()
        {
            ConnectDataBase();
        }

        //Connect to DataBase
        private async void ConnectDataBase()
        {

            var dbFilePath = Path.Combine(ApplicationData.Current.LocalFolder.Path, dbName);

            StorageFile fileSource = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///" + dbName, UriKind.RelativeOrAbsolute));
            StorageFolder desFolder = ApplicationData.Current.LocalFolder;

            try
            {
                StorageFile fileCheck = await desFolder.GetFileAsync(dbName);
            }
            catch
            {
                await fileSource.CopyAsync(desFolder, dbName, NameCollisionOption.ReplaceExisting);
            }
        }

        public async Task CopyDatabase()
        {
            bool isDatabaseExisting = false;

            try
            {
                StorageFile storageFile = await ApplicationData.Current.LocalFolder.GetFileAsync("HeroData.db");
                isDatabaseExisting = true;
            }
            catch
            {
                isDatabaseExisting = false;
            }

            if (!isDatabaseExisting)
            {
                StorageFile databaseFile = await Package.Current.InstalledLocation.GetFileAsync("HeroData.db");
                await databaseFile.CopyAsync(ApplicationData.Current.LocalFolder);
            }
        }

        //Get All Heroes From DataBase
        public List<Hero> getAllHeroes()
        {
            List<Hero> heroes = new List<Hero>();

            //< get records > 
            string query = @"SELECT hero.id, hero.name, attack.type, attribute.name as attribute FROM 
                            Heroes hero, AttackType attack, Attribute attribute Where hero.attack_id = attack.id AND
                            hero.attribute_id = attribute.id";

            ISQLiteStatement dbState = dbConnection.Prepare(query);

            while (dbState.Step() == SQLiteResult.ROW)
            {
                string heroId = dbState["id"].ToString();
                string heroName = dbState["name"] as string;
                string heroAttack = dbState["type"] as string;
                string heroAttribute = dbState["attribute"] as string;

                Hero currentHero = new Hero();

                currentHero.hero_id = Int32.Parse(heroId);
                currentHero.hero_name = heroName;
                currentHero.hero_attack = heroAttack;
                currentHero.hero_attribute = heroAttribute;

                heroes.Add(currentHero);
            }

            return heroes;
        }

        //Get all heroes that picked hero is weak against
        public List<Hero> GetWeakAgainst(int heroIndex)
        {
            List<Hero> heroes = new List<Hero>();

            string query = @"SELECT h.id, h.name FROM Heroes h, Weak w WHERE w.weak_id = h.id and w.id = " + heroIndex;

            ISQLiteStatement dbState = dbConnection.Prepare(query);

            while (dbState.Step() == SQLiteResult.ROW)
            {
                string heroId = dbState["id"].ToString();
                string heroName = dbState["name"] as string;

                Hero currentHero = new Hero();

                currentHero.hero_id = Int32.Parse(heroId);
                currentHero.hero_name = heroName;

                heroes.Add(currentHero);
            }


            return heroes;
        }


        //Get all heroes that picked hero is strong against
        public List<Hero> GetStrongAgainst(int heroIndex)
        {
            List<Hero> heroes = new List<Hero>();

            string query = @"SELECT h.id, h.name FROM Heroes h, Strong s WHERE s.strong_id = h.id AND s.id = " + heroIndex;

            ISQLiteStatement dbState = dbConnection.Prepare(query);

            while (dbState.Step() == SQLiteResult.ROW)
            {
                string heroId = dbState["id"].ToString();
                string heroName = dbState["name"] as string;

                Hero currentHero = new Hero();

                currentHero.hero_id = Int32.Parse(heroId);
                currentHero.hero_name = heroName;

                heroes.Add(currentHero);
            }


            return heroes;
        }
    }
}
