using System;
using System.Data;
using System.Data.SqlClient;

namespace CommandTest
{
    class Program
    {
        static void Main(string[] args)
        {
            // 构造连接字符串
            SqlConnectionStringBuilder connStr = new SqlConnectionStringBuilder();
            connStr.DataSource = @"192.168.0.89";
            connStr.InitialCatalog = "master";
            connStr.UserID = "sa";
            connStr.Password = "1q2w#E$R";

            using (SqlConnection conn = new SqlConnection(connStr.ConnectionString))
            {
                conn.Open();
            }
        }

        static void CreateCmd(SqlConnection conn)
        {
            string sqlString = @"";
            SqlCommand cmd = new SqlCommand(sqlString, conn);
        }

        static void CreateCmdOther(SqlConnection conn)
        {
            string sqlString = @"";
            SqlCommand cmdOther = new SqlCommand();
            cmdOther.Connection = conn;
            cmdOther.CommandText = sqlString;
        }

        static void ExecuteNonQuery(SqlConnection conn)
        {
            string sqlString = @"INSERT INTO 
                                    TB_Customer
                                VALUES
                                (
                                    '姓名'
                                    ,'地址'
                                    ,'联系方式'
                                )";
            SqlCommand cmdOther = new SqlCommand();
            cmdOther.Connection = conn;
            cmdOther.CommandText = sqlString;
            cmdOther.CommandType = CommandType.Text;

            int result = cmdOther.ExecuteNonQuery();
        }

        static void ExecuteReader(SqlConnection conn)
        {
            string sqlString = @"SELECT
                                    ID
                                    ,Name
                                    ,Adress
                                    ,Tel
                                FROM
                                    TB_Customer";
            SqlCommand cmdOther = new SqlCommand();
            cmdOther.Connection = conn;
            cmdOther.CommandText = sqlString;
            cmdOther.CommandType = CommandType.Text;

            SqlDataReader reader = cmdOther.ExecuteReader();
            while (reader.Read())
            {
                // 遍历Reader
                for (int i = 0; i < reader.FieldCount; ++i)
                {
                    Console.WriteLine("{0}:{1}", reader.GetName(i), reader.GetValue(i));
                }
            }
        }

        static void ExecuteScalar(SqlConnection conn)
        {
            string sqlString = @"SELECT 
                                    Name
                                FROM
                                    TB_Customer
                                WHERE
                                    ID=1";
            SqlCommand cmdOther = new SqlCommand();
            cmdOther.Connection = conn;
            cmdOther.CommandText = sqlString;
            cmdOther.CommandType = CommandType.Text;

            object result = cmdOther.ExecuteScalar();
            if (result != null)
            {
                string name = result.ToString();
            }
        }

        static void ExecuteDataSet(SqlConnection conn)
        {
            string sqlString = @"SELECT
                                    ID
                                    ,Name
                                    ,Adress
                                    ,Tel
                                FROM
                                    TB_Customer";
            SqlCommand cmdOther = new SqlCommand();
            cmdOther.Connection = conn;
            cmdOther.CommandText = sqlString;
            cmdOther.CommandType = CommandType.Text;

            SqlDataAdapter sda = new SqlDataAdapter(cmdOther);
            DataSet ds = new DataSet();
            sda.Fill(ds);

            DataTable tbResult = ds.Tables[0];
        }

        static void ExecuteWithParameter(SqlConnection conn)
        {
            string sqlString = @"SELECT 
                                    Name
                                FROM
                                    TB_Customer
                                WHERE
                                    ID={0}ID";
            sqlString = string.Format(sqlString, "@");
            SqlCommand cmdOther = new SqlCommand();
            cmdOther.Connection = conn;
            cmdOther.CommandText = sqlString;
            cmdOther.CommandType = CommandType.Text;

            // 参数化查询
            SqlParameter para1 = new SqlParameter("@ID", SqlDbType.Int);
            para1.Value = 1;
            cmdOther.Parameters.Add(para1);
            object result = cmdOther.ExecuteScalar();
            if (result != null)
            {
                string name = result.ToString();
            }
        }
    }
}
