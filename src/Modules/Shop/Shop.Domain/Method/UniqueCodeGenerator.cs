using System.Security.Cryptography;

namespace Shop.Domain.Method;

public static class UniqueCodeGenerator
{
    private const string Characters = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789";

    public static string Generate(int length = 8)
    {
        if (length <= 0)
            throw new ArgumentOutOfRangeException(nameof(length));

        Span<char> result = stackalloc char[length];

        for (var i = 0; i < length; i++)
        {
            result[i] = Characters[
                RandomNumberGenerator.GetInt32(Characters.Length)
            ];
        }

        return new string(result);
    }
}