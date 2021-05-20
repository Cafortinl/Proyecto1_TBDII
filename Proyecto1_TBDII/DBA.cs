using System;
using System.Collections.Generic;
using System.Text;
using StackExchange.Redis;

namespace Proyecto1_TBDII
{
    class DBA{ 
        private IDatabase conn;
        public DBA() { }
        public DBA(string connString)
        {
            ConnectionMultiplexer muxer = ConnectionMultiplexer.Connect(connString);
            conn = muxer.GetDatabase();
        }

        public IDatabase getConn()
        {
            return conn;
        }

        public void setKey(string k, string v)
        {
            conn.StringSet(k,v);
        }

        public string getValue(string k)
        {
            return conn.StringGet(k);
        }

    }
}
