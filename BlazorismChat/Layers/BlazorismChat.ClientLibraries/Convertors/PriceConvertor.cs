namespace BlazorismChat.ClientLibraries.Convertors;

public static class PriceConvertor
{
    /// <summary>
    /// Get Price in int and convert it to a formated text with suffix تومان
    /// </summary>
    /// <param name="price">Number you want to convert</param>
    /// <param name="isRial">Is your price in rial currency</param>
    /// <returns>123,456,000 تومان</returns>
    public static string ToTomanText(int price, bool isRial = false)
    {
        if (isRial)
            price /= 10;

        return $"{price:##,000} تومان";
    }

    /// <summary>
    /// Get Price in int and convert it to a formated text with suffix ریال
    /// </summary>
    /// <param name="price">Number you want to convert</param>
    /// <param name="isRial">Is your price in rial currency</param>
    /// <returns>1,234,567,000 ریال</returns>
    public static string ToRialText(int price, bool isRial = false)
    {
        if (!isRial)
            price *= 10;

        return $"{price:##,000} ریال";
    }
}