using System;
using System.Linq;

namespace Daniel.Utils
{
    public static class StringUtils
    {
        public static string UnmaskCNPJ(string cnpj)
        {
            return cnpj.Replace(".", string.Empty).Replace("-", string.Empty).Replace("/", string.Empty);
        }
        public static string UnmaskPhone(string phone)
        {
            return phone.Replace("(", string.Empty).Replace(")", string.Empty).Replace("-", string.Empty).Replace(" ", string.Empty);
        }
        public static string UnmaskCEP(string cep)
        {
            return cep.Replace("-", string.Empty);
        }
        public static bool IsValidCpf(string cpf)
        {
            if (string.IsNullOrWhiteSpace(cpf))
            {
                return false;
            }
            string unmaskedCPF = cpf.Replace(".", "").Replace("/", "").Replace("-", "");

            if (unmaskedCPF == null)
            {

                return false;
            }

            var invalidCPF = new[]
         {
                "00000000000",
                "11111111111",
                "22222222222",
                "33333333333",
                "44444444444",
                "55555555555",
                "66666666666",
                "77777777777",
                "88888888888",
                "99999999999",
            };

            if (invalidCPF.Contains(unmaskedCPF))
            {

                return false;
            }

            int[] mult1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] mult2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

            string tempCpf, digit;
            int sum, remain;

            cpf = unmaskedCPF;

            if (cpf.Length != 11)
                return false;


            tempCpf = cpf.Substring(0, 9);
            sum = 0;
            for (int i = 0; i < 9; i++)
                sum += int.Parse(tempCpf[i].ToString()) * mult1[i];
            remain = sum % 11;
            if (remain < 2)
                remain = 0;
            else
                remain = 11 - remain;
            digit = remain.ToString();
            tempCpf = tempCpf + digit;
            sum = 0;
            for (int i = 0; i < 10; i++)
                sum += int.Parse(tempCpf[i].ToString()) * mult2[i];
            remain = sum % 11;
            if (remain < 2)
                remain = 0;
            else
                remain = 11 - remain;
            digit = digit + remain.ToString();

            return cpf.EndsWith(digit);
        }

        public static bool IsValidCnpj(string cnpj)
        {
            string unformattedCnpj = cnpj.Replace(".", "").Replace("/", "").Replace("-", "");
            if (unformattedCnpj == null) return false;
            int[] mult1 = new int[12] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] mult2 = new int[13] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int sum, remain;
            string digito, tempCnpj;

            cnpj = unformattedCnpj;
            if (cnpj.Length != 14 || cnpj == "00000000000000")
                return false;
            tempCnpj = cnpj.Substring(0, 12);
            sum = 0;
            for (int i = 0; i < 12; i++)
                sum += int.Parse(tempCnpj[i].ToString()) * mult1[i];
            remain = (sum % 11);
            if (remain < 2)
                remain = 0;
            else
                remain = 11 - remain;
            digito = remain.ToString();
            tempCnpj = tempCnpj + digito;
            sum = 0;
            for (int i = 0; i < 13; i++)
                sum += int.Parse(tempCnpj[i].ToString()) * mult2[i];
            remain = (sum % 11);
            if (remain < 2)
                remain = 0;
            else
                remain = 11 - remain;
            digito = digito + remain.ToString();

            return cnpj.EndsWith(digito);
        }



        public static DateTime HorarioBrasilia()
        {
            DateTime dateTime = DateTime.UtcNow;
            TimeZoneInfo hrBrasilia = TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time");
            return TimeZoneInfo.ConvertTimeFromUtc(dateTime, hrBrasilia);
        }
    }
}
