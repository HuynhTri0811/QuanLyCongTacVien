using System;

namespace ScratchTest
{
    public static class VietnameseNumberToWordsHelper
    {
        private static readonly string[] ChuSo = { "không", "một", "hai", "ba", "bốn", "năm", "sáu", "bảy", "tám", "chín" };
        private static readonly string[] DonVi = { "", "nghìn", "triệu", "tỷ", "triệu tỷ", "tỷ tỷ" };

        public static string ConvertToWords(long number)
        {
            if (number == 0)
                return "Không";

            string result = "";
            string numStr = number.ToString();
            
            // Chia chuỗi số thành các nhóm 3 chữ số từ phải sang trái
            int len = numStr.Length;
            int groupCount = (len + 2) / 3;
            
            // Đệm thêm số 0 vào đầu chuỗi để chia hết cho 3
            numStr = numStr.PadLeft(groupCount * 3, '0');

            bool isFirstGroup = true;
            for (int i = 0; i < groupCount; i++)
            {
                string group = numStr.Substring(i * 3, 3);
                int dvIdx = groupCount - 1 - i;

                if (group == "000")
                {
                    // Nếu là nhóm 000 và không phải nhóm cuối cùng (tỷ, triệu...), ta có thể bỏ qua
                    // Ngoại trừ trường hợp tỷ (dvIdx % 3 == 3) để giữ đơn vị lớn
                    if (dvIdx % 3 == 0 && dvIdx > 0)
                    {
                        // Giữ đơn vị tỷ nếu cần thiết
                    }
                    continue;
                }

                int tram = group[0] - '0';
                int chuc = group[1] - '0';
                int donvi = group[2] - '0';

                string groupText = "";

                // Đọc hàng trăm
                // Không đọc hàng trăm nếu là nhóm đầu tiên và hàng trăm bằng 0
                if (!isFirstGroup || tram > 0)
                {
                    groupText += ChuSo[tram] + " trăm ";
                }

                // Đọc hàng chục
                if (chuc == 0)
                {
                    if (donvi > 0 && (!isFirstGroup || tram > 0))
                    {
                        groupText += "lẻ ";
                    }
                }
                else if (chuc == 1)
                {
                    groupText += "mười ";
                }
                else
                {
                    groupText += ChuSo[chuc] + " mươi ";
                }

                // Đọc hàng đơn vị
                if (chuc > 0 && donvi == 5)
                {
                    groupText += "lăm ";
                }
                else if (chuc > 1 && donvi == 1)
                {
                    groupText += "mốt ";
                }
                else if (donvi > 0)
                {
                    groupText += ChuSo[donvi] + " ";
                }

                groupText += DonVi[dvIdx % 4] + " "; // Đơn vị hàng nghìn, triệu, tỷ...
                
                result += groupText;
                isFirstGroup = false;
            }

            result = result.Trim();
            if (string.IsNullOrEmpty(result))
                return "";

            // Viết hoa chữ cái đầu tiên và chuẩn hóa khoảng trắng
            result = char.ToUpper(result[0]) + result.Substring(1);
            result = System.Text.RegularExpressions.Regex.Replace(result, @"\s+", " ");
            
            return result + " đồng";
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            long[] testNumbers = { 7000000, 7500000, 10000000, 12500000, 1005000, 0, 500000, 123456789 };
            foreach (var num in testNumbers)
            {
                Console.WriteLine($"{num:N0} -> {VietnameseNumberToWordsHelper.ConvertToWords(num)}");
            }
        }
    }
}
