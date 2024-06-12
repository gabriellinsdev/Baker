using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace Baker_API.Repository
{
    public class Helper
    {

        private readonly IConfiguration config;
        const int DefaultCommandTimeout = 300; // segundos
        public int? CommandTimeout { get; set; }

        public Helper()
        {
            var builder = new ConfigurationBuilder()
               .SetBasePath(AppContext.BaseDirectory)
               .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            config = builder.Build();
        }

        public List<T> ExecuteList<T>(string dbName, string procedureName, object parameters = null)
        {
            DataTable dt = ExecuteTable(dbName, procedureName, parameters);

            return ConvertDataTableList<T>(dt);
        }

        public List<T> ExecuteListView<T>(string dbName, string viewScript)
        {
            DataTable dt = ExecuteTableView(dbName, viewScript);

            return ConvertDataTableList<T>(dt);
        }

        public DataTable ExecuteTableView(string dbName, string viewScript)
        {
            DataTable dt = new DataTable();

            using (var conn = new SqlConnection(GetConnectionString(dbName)))
            {
                using (SqlCommand cmd = new SqlCommand(viewScript, conn))
                {
                    using (var da = new SqlDataAdapter(cmd))
                    {
                        cmd.CommandTimeout = this.CommandTimeout.GetValueOrDefault(DefaultCommandTimeout);
                        cmd.CommandType = CommandType.Text;

                        conn.Open();
                        da.Fill(dt);
                    }
                }
            }

            return dt;
        }

        public DataTable ExecuteTable(string dbName, string procedureName, object parameters = null)
        {

            DataTable dt = new DataTable();

            using (var conn = new SqlConnection(GetConnectionString(dbName)))
            {
                using (SqlCommand cmd = new SqlCommand(procedureName, conn))
                {
                    using (var da = new SqlDataAdapter(cmd))
                    {
                        cmd.CommandTimeout = this.CommandTimeout.GetValueOrDefault(DefaultCommandTimeout);
                        cmd.CommandType = CommandType.StoredProcedure;
                        SetParameters(parameters, cmd);

                        conn.Open();
                        da.Fill(dt);
                    }
                }
            }

            return dt;
        }

        public DataSet ExecuteDataSet(string dbName, string procedureName, object parameters = null)
        {

            DataSet ds = new DataSet();

            using (var conn = new SqlConnection(GetConnectionString(dbName)))
            {
                using (SqlCommand cmd = new SqlCommand(procedureName, conn))
                {
                    using (var da = new SqlDataAdapter(cmd))
                    {
                        cmd.CommandTimeout = this.CommandTimeout.GetValueOrDefault(DefaultCommandTimeout);
                        cmd.CommandType = CommandType.StoredProcedure;
                        SetParameters(parameters, cmd);

                        conn.Open();
                        da.Fill(ds);
                    }
                }
            }

            return ds;
        }

        public int ExecuteNonQuery(string dbName, string procedureName, object parameters = null)
        {
            int id;
            using (var conn = new SqlConnection(GetConnectionString(dbName)))
            {
                using (SqlCommand cmd = new SqlCommand(procedureName, conn))
                {
                    cmd.CommandTimeout = this.CommandTimeout.GetValueOrDefault(DefaultCommandTimeout);
                    cmd.CommandType = CommandType.StoredProcedure;
                    SetParameters(parameters, cmd);

                    var returnParameter = cmd.Parameters.Add("@ret", SqlDbType.Int);
                    returnParameter.Direction = ParameterDirection.ReturnValue;

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    id = Convert.ToInt32(returnParameter.Value);
                }
            }
            return id;
        }

        public string ExecuteScalar(string dbName, string procedureName, object parameters = null)
        {
            object coluna;
            using (var conn = new SqlConnection(GetConnectionString(dbName)))
            {
                using (SqlCommand cmd = new SqlCommand(procedureName, conn))
                {
                    cmd.CommandTimeout = this.CommandTimeout.GetValueOrDefault(DefaultCommandTimeout);
                    cmd.CommandType = CommandType.StoredProcedure;
                    SetParameters(parameters, cmd);

                    conn.Open();
                    coluna = cmd.ExecuteScalar();
                }
            }

            return coluna == null ? "" : coluna.ToString();
        }

        public List<T> ConvertDataTableList<T>(DataTable dt)
        {

            List<T> data = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                T item = GetItem<T>(row);
                data.Add(item);
            }
            return data;
        }

        public T ConvertDataTable<T>(DataTable dt)
        {
            T data = Activator.CreateInstance<T>();
            foreach (DataRow row in dt.Rows)
            {
                T item = GetItem<T>(row);
                data = item;
            }
            return data;
        }

        public string GetConnectionString(string dbName)
        {
            string cnnStr = config.GetConnectionString(dbName.ToString());
            string cnnStrFormat = "";
            string cnnStrUid = "";
            string cnnStrPwd = "";

            if (cnnStr.Contains("uid=") && cnnStr.Contains("pwd="))
            {
                var cnnStrSplit = cnnStr.Split(";");

                for (int i = 0; i < cnnStrSplit.Length; i++)
                {
                    if (cnnStrSplit[i].Contains("uid="))
                    {
                        cnnStrUid = cnnStrSplit[i].Substring(4);
                    }

                    if (cnnStrSplit[i].Contains("pwd="))
                    {
                        cnnStrPwd = cnnStrSplit[i].Substring(4);
                    }
                }

                //var cnnStrDecryptorUid = new SCriptografia().Descriptografar(cnnStrUid);
                //var cnnStrDecryptorPwd = new SCriptografia().Descriptografar(cnnStrPwd);

                //cnnStrFormat = cnnStr.Replace($"uid={cnnStrUid}", $"uid={cnnStrDecryptorUid}");
                //cnnStrFormat = cnnStrFormat.Replace($"pwd={cnnStrPwd}", $"pwd={cnnStrDecryptorPwd}");

            }

            if (String.IsNullOrEmpty(cnnStr))
            {
                throw new KeyNotFoundException(String.Format("String de Conexão não configurada para a base: \"{0}\"", dbName));
            }

            return String.IsNullOrEmpty(cnnStrFormat) ? cnnStr : cnnStrFormat;
        }

        private T GetItem<T>(DataRow dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();

            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    if (pro.Name.ToUpper() == column.ColumnName.ToUpper())
                    {
                        if (dr[column.ColumnName] != DBNull.Value)
                        {
                            var targetType = IsNullableType(pro.PropertyType) ? Nullable.GetUnderlyingType(pro.PropertyType) : pro.PropertyType;
                            pro.SetValue(obj, Convert.ChangeType(dr[column.ColumnName], targetType), null);
                            break;
                        }
                    }
                    else
                    {
                        continue;
                    }
                }
            }
            return obj;
        }

        private bool IsNullableType(Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition().Equals(typeof(Nullable<>));
        }

        private static void SetParameters(object parameters, SqlCommand cmd)
        {
            if (parameters != null)
            {
                foreach (var p in parameters.GetType().GetProperties())
                {
                    cmd.Parameters.AddWithValue(p.Name, p.GetValue(parameters, null));
                }
            }
        }

    }
}

