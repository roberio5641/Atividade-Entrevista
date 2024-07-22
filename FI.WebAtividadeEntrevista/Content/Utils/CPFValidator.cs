// File: Utils/CpfValidator.cs
using System.Linq;
using System.Text.RegularExpressions;

public static class CpfValidator
{
    public static bool IsValid(string cpf)
    {
        if (string.IsNullOrWhiteSpace(cpf)) return false;

        // Check if CPF is in the correct format
        if (!Regex.IsMatch(cpf, @"^\d{3}\.\d{3}\.\d{3}\-\d{2}$")) return false;

        // Remove the formatting
        cpf = cpf.Replace(".", "").Replace("-", "");

        if (cpf.Length != 11) return false;

        if (!cpf.All(char.IsDigit)) return false;

        var cpfArray = cpf.Select(c => int.Parse(c.ToString())).ToArray();
        var firstDigit = CalculateDigit(cpfArray, 9);
        var secondDigit = CalculateDigit(cpfArray, 10);

        return cpfArray[9] == firstDigit && cpfArray[10] == secondDigit;
    }

    private static int CalculateDigit(int[] cpf, int length)
    {
        var sum = 0;
        for (var i = 0; i < length; i++)
        {
            sum += cpf[i] * (length + 1 - i);
        }
        var remainder = sum % 11;
        return remainder < 2 ? 0 : 11 - remainder;
    }
}
