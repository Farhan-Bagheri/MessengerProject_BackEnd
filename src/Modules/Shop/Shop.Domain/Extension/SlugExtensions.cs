using System.Text.RegularExpressions;

namespace Shop.Domain.Extension;

public static class SlugExtensions
{
    public static string ToSlug(this string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Title cannot be empty.", nameof(value));

        // Trim
        value = value.Trim();

        // تبدیل فاصله و نیم‌فاصله به -
        value = Regex.Replace(value, @"[\s\u200C]+", "-");

        // حذف کاراکترهای غیرمجاز
        value = Regex.Replace(value, @"[^\p{L}\p{N}\-]", "");

        // حذف - های پشت سر هم
        value = Regex.Replace(value, @"-+", "-");

        // حذف - ابتدا و انتها
        value = value.Trim('-');

        // UniqueCode برای یکتا بودن و QR
        return value.ToLowerInvariant();
    }
}