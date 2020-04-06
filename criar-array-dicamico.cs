public static string[,] CriaArray(string lista)
{    
    string[] a = lista.Split(';');
    string[,] array = new string[a.Length, 2];

    int arrayLin = 0; 
    foreach (string b in a)
    {
        string[] campoValor = b.Split(':');
        array[arrayLin, 0] = campoValor[0].ToString();
        array[arrayLin, 1] = campoValor[1].ToString();
        arrayLin++;
    }
    return array;
}
