using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Database.DataStructures;
using Database.SQLiter;

public class DataService : SQLite
{
    private static DataService instance;

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

    protected override string DBName
    {
        get
        {
            return "UBUSetasDB";
        }
    }

    private const string SQL_TABLE_NAME_1 = "TablaComestible";
    private const string SQL_TABLE_NAME_2 = "TablaDescripciones";
    private const string SQL_TABLE_NAME_3 = "TablaGeneros";

    private const string COL_NAME = "Nombre";
    private const string COL_DESCRIPTION_SPANISH = "DescripcionEs";
    private const string COL_EDIBLE_SPANISH = "ComestibleEs";
    private const string COL_DESCRIPTION_ENGLISH = "DescripcionEn";
    private const string COL_EDIBLE_ENGLISH = "ComestibleEn";
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
