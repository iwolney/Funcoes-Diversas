public static string CarregaTexto(string Caminho)
{
    System.IO.StreamReader sr;
    sr = System.IO.File.OpenText(Caminho);
    string retorno = "";
    retorno = sr.ReadToEnd();
    sr.Close();
    sr = null;
    return retorno;
}
