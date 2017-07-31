using System;
using System.Data;
using System.Data.SqlClient;

namespace ConnectionTest
{
    class Program
    {
        static void Main(string[] args)
        {
            //// 构造连接字符串
            //SqlConnectionStringBuilder connStr = new SqlConnectionStringBuilder();
            //connStr.DataSource = @"192.168.0.89";
            //connStr.InitialCatalog = "master";
            //connStr.UserID = "sa";
            //connStr.Password = "1q2w#E$R";

            //BaseDemo(connStr);
            //TryCatchDemo(connStr.ConnectionString);
            //UsingDemo(connStr.ConnectionString);

            // 数据库连接池
            PoolException();
        }

        static void BaseDemo(SqlConnectionStringBuilder connStr)
        {
            // 创建连接对象
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = connStr.ConnectionString; // 设置连接字符串
            conn.Open(); // 打开连接

            // 数据库连接成功时输出连接信息
            if (conn.State == ConnectionState.Open)
            {
                Console.WriteLine("Database is linked.");
                Console.WriteLine("\nDataSource:{0}", conn.DataSource);
                Console.WriteLine("Database:{0}", conn.Database);
                Console.WriteLine("ConnectionTimeOut:{0}", conn.ConnectionTimeout);
            }
            conn.Close();//关闭连接
            conn.Dispose();//释放资源

            // 检查是否已关闭数据库连接
            if (conn.State == ConnectionState.Closed)
            {
                Console.WriteLine("\nDatabase is closed.");
            }
            Console.ReadKey();
        }

        static void TryCatchDemo(string connectionString)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            try
            {
                conn.Open();
                throw new Exception("这是一个被强制抛出错误！！");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Console.WriteLine("数据库连接出现异常，数据库连接需要关闭");
                conn.Close();
            }

            if (conn.State == ConnectionState.Closed)
            {
                Console.WriteLine("\nDatabase is closed.");
            }
            Console.ReadKey();
        }

        static void UsingDemo(string connectionString)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                Console.WriteLine("我只是用了Using，里面没有关闭任务东西");
            }

            Console.WriteLine("为了验证Using后是否关闭，再调一次其他方法");
            TryCatchDemo(connectionString);
        }

        static void ConnectionPoolDemo()
        {
            SqlConnectionStringBuilder connStr = new SqlConnectionStringBuilder();
            connStr.DataSource = @"192.168.0.89";
            connStr.InitialCatalog = "master";
            connStr.UserID = "sa";
            connStr.Password = "1q2w#E$R";
            connStr.Pooling = true; // 开启连接池
            connStr.MinPoolSize = 0; // 设置最小连接数为0
            connStr.MaxPoolSize = 50; // 设置最大连接数为50             
            connStr.ConnectTimeout = 10; // 设置超时时间为10秒

            using (SqlConnection conn = new SqlConnection(connStr.ConnectionString))
            {
                ;//todo
            }
        }

        static void PoolException()
        {
            SqlConnectionStringBuilder connStr = new SqlConnectionStringBuilder();
            connStr.DataSource = @"192.168.0.89";
            connStr.InitialCatalog = "master";
            connStr.UserID = "sa";
            connStr.Password = "1q2w#E$R";
            connStr.MaxPoolSize = 5;//设置最大连接池为5
            connStr.ConnectTimeout = 1;//设置超时时间为1秒

            SqlConnection conn = null;
            for (int i = 1; i <= 100; ++i)
            {
                conn = new SqlConnection(connStr.ConnectionString);
                try
                {
                    conn.Open();
                    Console.WriteLine("Connection{0} is linked", i);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("\n异常信息:\n{0}", ex.Message);
                    break;
                }
            }
            Console.ReadKey();
        }
    }
}
