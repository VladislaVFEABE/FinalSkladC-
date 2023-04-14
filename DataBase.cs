using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace Final
{
    class DataBase
    {
        SqlConnection sql = new SqlConnection(@"Data Source=DESKTOP-081C48G;Initial Catalog=Cursovaya;Integrated Security=True");

        public void openConnection()
        {
            if (sql.State == System.Data.ConnectionState.Closed)
            {
                sql.Open();
            }
        }

        public void closeConnection()
        {
            if (sql.State == System.Data.ConnectionState.Open)
            {
                sql.Close();
            }
        }

        public SqlConnection GetConnection()
        {
            return sql;
        }
    }
}
