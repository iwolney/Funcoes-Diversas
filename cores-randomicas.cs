public static string CoresRandomicas(int parametro)
{
    string cor = string.Empty;
    try
    {
        Random randColors = new Random(parametro);
        int r = int.Parse(randColors.Next(255).ToString());
        int g = int.Parse(randColors.Next(255).ToString());
        int b = int.Parse(randColors.Next(255).ToString());
        int a = int.Parse(randColors.Next(255).ToString());
        cor = string.Format("#{0}", System.Drawing.Color.FromArgb(a, r, g, b).Name.ToUpper().Substring(0, 6));
    }
    catch(Exception ex)
    {
        throw ex;
    }
    return cor;
}
