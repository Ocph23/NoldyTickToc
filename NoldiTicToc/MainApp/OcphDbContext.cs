using Ocph.DAL.Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainApp
{
    public class OcphDbContext : Ocph.DAL.Provider.SQLite.SQLiteDbConnection
    {
        public OcphDbContext()
        {
            if (File.Exists("dbgame.db"))
            {
                this.ConnectionString = "Data Source=dbgame.db";
            }
            else
            {
                this.ConnectionString = "Data Source=dbgame.db;Version=3;New=True;Compress=True;";
                this.Open();

                IDbCommand cmd = CreateCommand();

                cmd.CommandText = @"CREATE TABLE IF NOT EXISTS Score (Id INTEGER PRIMARY KEY AUTOINCREMENT, Player1 VARCHAR(100),Player2 VARCHAR(100),GameType VARCHAR(100),Player1Win Integer,Player2Win Integer,Solutions Varchar(200), Time Varchar(200))";

                IDataReader reader = cmd.ExecuteReader();

                reader.Close();
            }
        }

        public IRepository<Score> Scores { get { return new Repository<Score>(this); } }

        internal void IsExist<T>()
        {
            var name = typeof(T).Name;
        }

    }

}
