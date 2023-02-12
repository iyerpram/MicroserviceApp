using System.Data;

namespace MicroserviceApp.Logic.Domain.Data
{
    public class DataParameter
    {
        public string Key { get; set; }
        public object Value { get; set; }
        public SqlDbType DbType { get; set; }
    }
}
