using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyCongTacVien
{
    public static class Common
    {
        public static DateTime? ConvertStringToDateTime(object value)
        {
            if (value == null || value == DBNull.Value)
                return null;
            if (DateTime.TryParse(value.ToString(), out DateTime result))
                return result;
            return null;
        }
    }
}
