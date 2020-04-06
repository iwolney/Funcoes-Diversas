public static DataSet ListToDataSet(IList ResList)
{
    DataSet RDS = new DataSet();
    DataTable TempDT = new DataTable();

    try
    {
        System.Reflection.PropertyInfo[] p = ResList[0].GetType().GetProperties();
        foreach (System.Reflection.PropertyInfo pi in p)
        {
            //TempDT.Columns.Add(pi.Name, System.Type.GetType(pi.PropertyType.ToString()));
            TempDT.Columns.Add(pi.Name, typeof(string));
        }
        for (int i = 0; i < ResList.Count; i++)
        {
            IList TempList = new ArrayList();
            foreach (System.Reflection.PropertyInfo pi in p)
            {
                object oo = pi.GetValue(ResList[i], null);
                TempList.Add(oo);
            }
            object[] itm = new object[p.Length];
            for (int j = 0; j < TempList.Count; j++)
            {
                itm.SetValue(TempList[j], j);
            }
            TempDT.LoadDataRow(itm, true);
        }
        RDS.Tables.Add(TempDT);
    }
    catch { }
    return RDS;
}
