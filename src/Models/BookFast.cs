using Dapper.FastCrud;
using System;
using System.Data;
using System.Linq;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data.SqlClient;

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace dapperOdata.Models{

    public class Repo{

        private readonly IConfiguration _config;

        public Repo(IConfiguration config)
        {
            _config = config;
        }

        public IDbConnection Conn
        {
            get
            {
                return new SqlConnection(_config.GetConnectionString("db"));
            }
        }

        public T Get<T>(T param)
        {
            using (IDbConnection conn = Conn)
            {
                return conn.Get(param);
            }
        }

        public void Insert<T>(T param){
                using (IDbConnection conn = Conn)
                {
                    conn.Insert(param);
                }
        }

        public IEnumerable<T> Find<T>(){
                using (IDbConnection conn = Conn)
                {
                    return conn.Find<T>();
                }
        }


    }

    [Table ("Book")]
    public class BookFast
    {
    	[Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id {get; set;}
        [Required]
        public string Title {get; set;}
        public string ISBN {get; set;}
    }
}
