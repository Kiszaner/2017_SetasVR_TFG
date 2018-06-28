using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UBUSetasVR.Database.DataStructures;
using Database.SQLiter;

namespace UBUSetasVR.Database
{
    /// <summary>
    /// Class that defines a conection to a SQLite database and applies a Singleton pattern.
    /// </summary>
    public class DataService : SQLite
    {
        /// <summary>
        /// Instance of the class.
        /// </summary>
        private static DataService instance;

        /// <summary>
        /// Property to get the instance of the class.
        /// </summary>
        public static DataService Instance
        {
            get
            {
                if (instance != null) return instance;
                instance = (DataService)FindObjectOfType(typeof(DataService));

                if (instance == null)
                    Debug.LogError("An instance of " + typeof(DataService) +
                                   " is needed in the scene, but there is none.");

                return instance;
            }
        }

        /// <summary>
        /// Property with the name of the database.
        /// </summary>
        protected override string DBName
        {
            get
            {
                return "UBUSetasDB";
            }
        }

        /// <summary>
        /// Table in the database with the edible conditions of each mushroom.
        /// </summary>
        private const string SQL_TABLE_NAME_1 = "TablaComestible";

        /// <summary>
        /// Table in the database with the descriptions of each mushroom.
        /// </summary>
        private const string SQL_TABLE_NAME_2 = "TablaDescripciones";

        /// <summary>
        /// Table in the database with the genre of each mushroom.
        /// </summary>
        private const string SQL_TABLE_NAME_3 = "TablaGeneros";

        /// <summary>
        /// Column with the name of the muhsroom.
        /// </summary>
        private const string COL_NAME = "Nombre";

        /// <summary>
        /// Column with the description in spanish of the mushroom.
        /// </summary>
        private const string COL_DESCRIPTION_SPANISH = "DescripcionEs";

        /// <summary>
        /// Column with the edible conditions in spanish of the mushroom.
        /// </summary>
        private const string COL_EDIBLE_SPANISH = "ComestibleEs";

        /// <summary>
        /// Column with the descriptions in english of the mushroom.
        /// </summary>
        private const string COL_DESCRIPTION_ENGLISH = "DescripcionEn";

        /// <summary>
        /// Column with the edible conditions in english of the mushroom.
        /// </summary>
        private const string COL_EDIBLE_ENGLISH = "ComestibleEn";

        /// <summary>
        /// Column with the genre of the mushroom.
        /// </summary>
        private const string COL_SPECIES = "Especie";

        /// <summary>
        ///     Initialization method.
        /// </summary>
        public void Start()
        {
            LoomManager.Loom.QueueOnMainThread(() =>
            {
                SQLiteInit();
            });
        }

        /// <summary>
        /// Gets the mushroom data from the database by name.
        /// </summary>
        /// <param name="name">Name of the mushroom</param>
        /// <returns>The data structure of the mushroom</returns>
        public Mushroom GetMushroomByName(string name)
        {
            Debug.Log("SELECT c." + COL_NAME + ", d." + COL_DESCRIPTION_SPANISH + ", c." + COL_EDIBLE_SPANISH + ", g." + COL_SPECIES + " FROM " + SQL_TABLE_NAME_1 + " c join " + SQL_TABLE_NAME_2 + " d on c.Nombre = d.Nombre join " + SQL_TABLE_NAME_3 + " g on c.IdSeta = g.IdSeta where c." + COL_NAME + " = '" + name + "'");
            //select c.Nombre, d.DescripcionEs, c.ComestibleEs, g.Especie
            //from TablaComestible c
            //join TablaDescripciones d on c.IdSeta = d.IdSeta
            //join TablaGeneros g on c.IdSeta = g.IdSeta
            //where c.Nombre = 'aleuria aurantia'
            RunQuery("SELECT c." + COL_NAME + ", d." + COL_DESCRIPTION_SPANISH + ", c." + COL_EDIBLE_SPANISH + ", g." + COL_SPECIES + " FROM " + SQL_TABLE_NAME_1 + " c join " + SQL_TABLE_NAME_2 + " d on c.Nombre = d.Nombre join " + SQL_TABLE_NAME_3 + " g on c.IdSeta = g.IdSeta where c." + COL_NAME + " = '" + name + "'");
            if (_reader.Read())
            {
                Mushroom mush = new Mushroom(_reader.GetString(0), _reader.GetString(1), _reader.GetString(2), _reader.GetString(3));
                Debug.Log(mush.ToString());
                return mush;
            }
            Debug.LogWarning("Mushroom " + name + " not found on db!");
            _reader.Close();
            return null;
        }

        /// <summary>
        /// Gets all the mushrooms in the database.
        /// </summary>
        /// <returns></returns>
        public List<Mushroom> GetMushrooms()
        {
            List<Mushroom> mushrooms = new List<Mushroom>();
            Debug.Log("SELECT c." + COL_NAME + ", d." + COL_DESCRIPTION_SPANISH + ", c." + COL_EDIBLE_SPANISH + ", g." + COL_SPECIES + " FROM " + SQL_TABLE_NAME_1 + " c join " + SQL_TABLE_NAME_2 + " d on c.IdSeta = d.IdSeta join " + SQL_TABLE_NAME_3 + "g on c.IdSeta = g.IdSeta");
            //select c.Nombre, d.DescripcionEs, c.ComestibleEs, g.Especie
            //from TablaComestible c
            //join TablaDescripciones d on c.IdSeta = d.IdSeta
            //join TablaGeneros g on c.IdSeta = g.IdSeta
            RunQuery("SELECT c." + COL_NAME + ", d." + COL_DESCRIPTION_SPANISH + ", c." + COL_EDIBLE_SPANISH + ", g." + COL_SPECIES + " FROM " + SQL_TABLE_NAME_1 + " c join " + SQL_TABLE_NAME_2 + " d on c.IdSeta = d.IdSeta join " + SQL_TABLE_NAME_3 + "g on c.IdSeta = g.IdSeta");
            while (_reader.Read())
            {
                mushrooms.Add(new Mushroom(_reader.GetString(0), _reader.GetString(1), _reader.GetString(2), _reader.GetString(3)));
            }
            return mushrooms;
        }
    }
}