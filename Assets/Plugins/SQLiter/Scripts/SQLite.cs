//----------------------------------------------
// SQLiter
// Copyright � 2014 OuijaPaw Games LLC
// Modified version by Rodrigo Jurado Pajares
//----------------------------------------------

using UnityEngine;
using System.Data;
using Mono.Data.SqliteClient;
using System.IO;

namespace Database.SQLiter
{
	/// <summary>
	/// The idea is that here is a bunch of the basics on using SQLite
	/// Nothing is some advanced course on doing joins and unions and trying to make your infinitely normalized schema work
	/// SQLite is simple.  Very simple.  
	/// Pros:
	/// - Very simple to use
	/// - Very small memory footprint
	/// 
	/// Cons:
	/// - It is a flat file database.  You can change the settings to make it run completely in memory, which will make it even
	/// faster; however, you cannot have separate threads interact with it -ever-, so if you plan on using SQLite for any sort
	/// of multiplayer game and want different Unity instances to interact/read data... they absolutely cannot.
	/// - Doesn't offer as many bells and whistles as other DB systems
	/// - It is awfully slow.  I mean dreadfully slow.  I know "slow" is a relative term, but unless the DB is all in memory, every
	/// time you do a write/delete/update/replace, it has to write to a physical file - since SQLite is just a file based DB.
	/// If you ever do a write and then need to read it shortly after, like .5 to 1 second after... there's a chance it hasn't been
	/// updated yet... and this is local.  So, just make sure you use a coroutine or whatever to make sure data is written before
	/// using it.
	/// 
	/// SQLite is nice for small games, high scores, simple saved, etc.  It is not very secure and not very fast, but it's cheap,
	/// simple, and useful at times.
	/// 
	/// Here are some starting tools and information.  Go explore.
	/// </summary>
	public abstract class SQLite : MonoBehaviour
	{
		//public static SQLite Instance = null;
		public bool DebugMode = false;

		// Location of database - this will be set during Awake as to stop Unity 5.4 error regarding initialization before scene is set
		// file should show up in the Unity inspector after a few seconds of running it the first time
		private static string DBLocation = "";

		/// <summary>
		/// Table name and DB actual file name -- this is the name of the actual file on the filesystem
		/// </summary>
		protected abstract string DBName { get; }

        /// <summary>
        /// DB objects
        /// </summary>
        protected IDbConnection _connection = null;
		protected IDbCommand _command = null;
		protected IDataReader _reader = null;

		/// <summary>
		/// Awake will initialize the connection.  
		/// RunAsyncInit is just for show.  You can do the normal SQLiteInit to ensure that it is
		/// initialized during the Awake() phase and everything is ready during the Start() phase
		/// </summary>
		void Awake()
		{
#if UNITY_EDITOR
            if (DebugMode)
				Debug.Log("--- Awake ---");

            // here is where we set the file location
            // ------------ IMPORTANT ---------
            // - during builds, this is located in the project root - same level as Assets/Library/obj/ProjectSettings
            // - during runtime (Windows at least), this is located in the SAME directory as the executable
            // you can play around with the path if you like, but build-vs-run locations need to be taken into account
            string databaseName = DBName + ".db";
            string filepath = Application.persistentDataPath + "/" + databaseName;
            Debug.Log("filepath first: " + filepath);

            if (!File.Exists(filepath))

            {
                Debug.Log("File doesnt exists");
                WWW loadDB = new WWW("jar:file://" + Application.dataPath + "!/assets/" + databaseName);  // this is the path to your StreamingAssets in android

                while (!loadDB.isDone) { }  // CAREFUL here, for safety reasons you shouldn't let this while loop unattended, place a timer and error check

                // then save to Application.persistentDataPath

                File.WriteAllBytes(filepath, loadDB.bytes);

            }
#endif
#if UNITY_EDITOR
            DBLocation = "URI=file:Assets/Data/DB/" + DBName + ".db";
#else
            //DBLocation = "URI=file:" + DBName + ".db";
            //DBLocation = "URI=file:" + filepath;
#endif
#if UNITY_EDITOR
            Debug.Log(DBLocation);
            //Instance = this;
            //SQLiteInit();
#endif
        }

		void Start()
		{
			if (DebugMode) Debug.Log("--- Start ---");
		}

#if UNITY_EDITOR
        /// <summary>
        ///     Clean up SQLite Connections, anything else
        /// </summary>
        private void OnDestroy()
        {
            SQLiteClose();
        }
#endif

        /// <summary>
        /// Basic initialization of SQLite
        /// </summary>
        protected void SQLiteInit()
		{
#if UNITY_EDITOR
            Debug.Log("SQLiter - Opening SQLite Connection");
			_connection = new SqliteConnection(DBLocation);
			_command = _connection.CreateCommand();
			_connection.Open();

			// WAL = write ahead logging, very huge speed increase
			_command.CommandText = "PRAGMA journal_mode = WAL;";
			_command.ExecuteNonQuery();

			// journal mode = look it up on google, I don't remember
			_command.CommandText = "PRAGMA journal_mode";
			_reader = _command.ExecuteReader();
			if (DebugMode && _reader.Read())
				Debug.Log("SQLiter - WAL value is: " + _reader.GetString(0));
			_reader.Close();

			// more speed increases
			_command.CommandText = "PRAGMA synchronous = OFF";
			_command.ExecuteNonQuery();

			// and some more
			_command.CommandText = "PRAGMA synchronous";
			_reader = _command.ExecuteReader();
			if (DebugMode && _reader.Read())
				Debug.Log("SQLiter - synchronous value is: " + _reader.GetInt32(0));
			_reader.Close();

			// close connection
			_connection.Close();
#endif
        }

        /// <summary>
        ///     Method to run query command.
        /// </summary>
        /// <param name="command">The query to run.</param>
        protected void RunQuery(string command)
        {
            _connection.Open();
            NewCommand();
            this._command.CommandText = command;
            _reader = this._command.ExecuteReader();
            _connection.Close();
        }

        /// <summary>
        ///     Function to ease the clearing of the command so that we make the variable reusable.
        /// </summary>
        private void NewCommand()
        {
            _command = _connection.CreateCommand();
        }

        /// <summary>
        /// Clean up everything for SQLite
        /// </summary>
        private void SQLiteClose()
		{
			if (_reader != null && !_reader.IsClosed)
				_reader.Close();
			_reader = null;

            if (_command != null)
                NewCommand();
			_command = null;

			if (_connection != null && _connection.State != ConnectionState.Closed)
				_connection.Close();
			_connection = null;
		}
	}
}
