using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using MySql.Data.MySqlClient;

namespace osu2013server.MySql
{
    public class Database
    {
        private readonly string _connectionString;

        public Database(string location, string username, string password, string database)
        {
            _connectionString = string.Format(
                "server={0};userid={1};password={2};database={3}",
                location, username, password, database
            );
        }

        public NameValueCollection[] Query(string sql, MySqlParameter[] bindings)
        {
            using (MySqlConnection Connection = new MySqlConnection(_connectionString))
            {

                MySqlCommand Command = Connection.CreateCommand();
                Command.CommandText = sql;

                foreach (var p in bindings)
                {
                    Command.Parameters.AddWithValue(p.ParameterName, p.Value);
                }

                try
                {
                    Connection.Open();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }


                List<NameValueCollection> ret = new List<NameValueCollection>();
                try
                {
                    using (MySqlDataReader reader = Command.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            NameValueCollection current = new NameValueCollection();
                            for (int col = 0; col < reader.FieldCount; col++)
                            {
                                string type = reader.GetFieldType(col).ToString();
                                
                                switch (type)
                                {
                                    case "System.Int32":
                                        current.Add(reader.GetName(col).ToString(), reader.GetInt32(col).ToString());
                                        break;
                                    case "System.UInt64":
                                        current.Add(reader.GetName(col).ToString(), reader.GetUInt64(col).ToString());
                                        break;
                                    case "System.SByte":
                                        current.Add(reader.GetName(col).ToString(), reader.GetByte(col).ToString());
                                        break;
                                    case "System.String":
                                        current.Add(reader.GetName(col).ToString(), reader.GetString(col));
                                        break;
                                    case "System.Double":
                                        current.Add(reader.GetName(col).ToString(), reader.GetDouble(col).ToString());
                                        break;
                                    case "System.Single":
                                        current.Add(reader.GetName(col).ToString(), reader.GetFloat(col).ToString());
                                        break;
                                    case "System.Boolean":
                                        current.Add(reader.GetName(col).ToString(), reader.GetBoolean(col).ToString());
                                        break;
                                }
                            }
                            ret.Add(current);

                        }


                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    Connection.Close();
                }
                Connection.Close();
                return ret.ToArray();
            }
        }


        public NameValueCollection[] Query(string sql)
        {
            using (MySqlConnection Connection = new MySqlConnection(_connectionString))
            {

                MySqlCommand Command = Connection.CreateCommand();
                Command.CommandText = sql;

                try
                {
                    Connection.Open();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    Connection.Close();
                }


                List<NameValueCollection> ret = new List<NameValueCollection>();

                try
                {
                    using (MySqlDataReader reader = Command.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            NameValueCollection current = new NameValueCollection();
                            for (int col = 0; col < reader.FieldCount; col++)
                            {
                                switch (reader.GetFieldType(col).ToString())
                                {
                                    case "System.Int32":
                                        current.Add(reader.GetName(col).ToString(), reader.GetInt32(col).ToString());
                                        break;
                                    case "System.UInt64":
                                        current.Add(reader.GetName(col).ToString(), reader.GetUInt64(col).ToString());
                                        break;
                                    case "System.SByte":
                                        current.Add(reader.GetName(col).ToString(), reader.GetByte(col).ToString());
                                        break;
                                    case "System.String":
                                        current.Add(reader.GetName(col).ToString(), reader.GetString(col));
                                        break;
                                    case "System.Double":
                                        current.Add(reader.GetName(col).ToString(), reader.GetDouble(col).ToString());
                                        break;
                                    case "System.Boolean":
                                        current.Add(reader.GetName(col).ToString(), reader.GetBoolean(col).ToString());
                                        break;
                                }
                            }
                            ret.Add(current);

                        }


                    }
                }
                catch
                {

                    Connection.Close();

                }
                Connection.Close();
                return ret.ToArray();
            }
        }

        private static dynamic StringToObjectFromTypeName(string typeName, string str)
        {
            return typeName switch
            {
                "System.Int32"   => int.Parse(str),
                "System.Int64"   => long.Parse(str),
                "System.UInt32"  => uint.Parse(str),
                "System.UInt64"  => ulong.Parse(str),
                "System.Boolean" => sbyte.Parse(str) != 0,
                "System.Byte"    => byte.Parse(str),
                "System.SByte"   => sbyte.Parse(str),
                "System.String"  => str,
                "System.Double"  => double.Parse(str),
                "System.Single"  => float.Parse(str),

                _ => throw new NotImplementedException(),
            };
        }

        private static void SetMemberValueFromString(MemberInfo memberInfo, object obj, string str)
        {
            string typename;

            switch (memberInfo.MemberType)
            {
                case MemberTypes.Field:
                    var fieldInfo = ((FieldInfo)memberInfo);
                    typename = fieldInfo.FieldType.ToString();

                    fieldInfo.SetValue(obj, StringToObjectFromTypeName(typename, str));
                    break;

                case MemberTypes.Property:
                    var propInfo = ((PropertyInfo)memberInfo);
                    typename = propInfo.PropertyType.ToString();

                    propInfo.SetValue(obj, StringToObjectFromTypeName(typename, str));
                    break;

                default:
                    // Unreachable
                    throw new Exception();
            }
        }

        public NameValueCollection Get(string sql, MySqlParameter[] parameters)
        {
            var results = Query(sql, parameters);
            if (results.Length == 0)
                return null;

            return results[0];
        }


        public T Get<T>(string sql, MySqlParameter[] parameters) where T : class, new()
        {
            var results = Query(sql, parameters);
            if (results.Length == 0)
                return null;

            var objects = ResultsToObjects<T>(results);

            return objects[0];
        }

        public T[] Select<T>(string sql, MySqlParameter[] parameters) where T : class, new()
        {
            var results = Query(sql, parameters);
            var objects = ResultsToObjects<T>(results);

            return objects;
        }

        public T[] Select<T>(string sql) where T : class, new()
        {
            var results = Query(sql);
            if (results.Length == 0)
                return null;

            var objects = ResultsToObjects<T>(results);

            return objects;
        }

        private static T[] ResultsToObjects<T>(NameValueCollection[] results) where T : class, new()
        {
            var objects = new T[results.Length];

            var type = typeof(T);
            const BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Instance;

            var members = type.GetFields(bindingFlags).Cast<MemberInfo>()
                .Concat(type.GetProperties(bindingFlags)).ToArray();
            
            for (var i = 0; i < results.Length; i++)
            {
                var result = results[i];
                objects[i] = new T();

                foreach (var member in members)
                {
                    var column = result[member.Name];
                    if (column == null)
                        continue;

                    SetMemberValueFromString(member, objects[i], column);
                }
            }

            return objects;
        }

        public int Exec(string sql)
        {
            using (MySqlConnection Connection = new MySqlConnection(_connectionString))
            {

                try
                {
                    Connection.Open();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return -1;
                }

                MySqlCommand Command = Connection.CreateCommand();
                Command.CommandText = sql;

                Command.ExecuteNonQuery();
                Connection.Close();
                return 0;
            }
        }

        public int Exec(string sql, MySqlParameter[] bindings)
        {
            using (MySqlConnection Connection = new MySqlConnection(_connectionString))
            {

                MySqlCommand Command = Connection.CreateCommand();
                Command.CommandText = sql;

                try
                {
                    Connection.Open();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return -1;
                }

                foreach (var p in bindings)
                {
                    Command.Parameters.AddWithValue(p.ParameterName, p.Value);
                }
                Command.ExecuteNonQuery();
                Connection.Close();
                return 0;
            }
        }
    }
}