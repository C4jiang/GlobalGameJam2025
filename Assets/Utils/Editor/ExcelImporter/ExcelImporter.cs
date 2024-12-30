using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Reflection;
using I2.Loc;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;
using Object = UnityEngine.Object;
using System.Text;
using vietlabs.fr2;
using System.Linq;

public class ExcelImporter : AssetPostprocessor
{
    class ExcelAssetInfo
    {
        public Type AssetType { get; set; }
        public ExcelAssetAttribute Attribute { get; set; }
        public string ExcelName
        {
            get
            {
                return string.IsNullOrEmpty(Attribute.ExcelName) ? AssetType.Name : Attribute.ExcelName;
            }
        }
    }

    private static string _charactersRangeFilePath = $"{Application.dataPath}/Data/CharacterRnage.txt";
    //private static HashSet<char> wroteCharacterRange;
    // static List<ExcelAssetInfo> cachedInfos = null; // Clear on compile.

    public static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {
        bool imported = false;
        //InitWriteCharacterRange();

        foreach (string path in importedAssets)
        {
            if (Path.GetExtension(path) == ".xls" || Path.GetExtension(path) == ".xlsx" || Path.GetExtension(path) == ".xlsm")
            {
                Debug.Log(path);
                if (path.Contains("Data/ExcelData/Localization"))
                {
                    Debug.Log("导入多语言本地化表格：" + path);
                    ImportLocalization(path);
                    continue;
                }

                // if (cachedInfos == null)  
                //     cachedInfos = FindExcelAssetInfos();

                // var excelName = Path.GetFileNameWithoutExtension(path);
                // if (excelName.StartsWith("~$")) continue;

                // ExcelAssetInfo info = cachedInfos.Find(i => i.ExcelName == excelName);

                // if (info == null) continue;

                // ImportExcel(path, info);
                imported = true;
            }
        }

        if (imported)
        {
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }

    //private static void FinishWriteCharacterRange()
    //{
    //    if(wroteCharacterRange != null)
    //    {
    //        var sw = new StreamWriter(_charactersRangeFilePath, false, Encoding.UTF8);
    //        StringBuilder sb = new StringBuilder();
    //        foreach (var ch in wroteCharacterRange)
    //        {
    //            sb.Append(ch);
    //        }
    //        var res = sb.ToString();
    //        sw.Write(res);
    //        sw.Close();
    //    }
    //    wroteCharacterRange = null;
    //}

    //private static void InitWriteCharacterRange()
    //{
    //    wroteCharacterRange = new HashSet<char>();
    //}

    static List<ExcelAssetInfo> FindExcelAssetInfos()
    {
        var list = new List<ExcelAssetInfo>();
        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            foreach (var type in assembly.GetTypes())
            {
                var attributes = type.GetCustomAttributes(typeof(ExcelAssetAttribute), false);
                if (attributes.Length == 0) continue;
                var attribute = (ExcelAssetAttribute)attributes[0];
                var info = new ExcelAssetInfo()
                {
                    AssetType = type,
                    Attribute = attribute
                };
                list.Add(info);
            }
        }
        return list;
    }

    static UnityEngine.Object LoadOrCreateAsset(string assetPath, Type assetType)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(assetPath));

        var asset = AssetDatabase.LoadAssetAtPath(assetPath, assetType);

        if (asset == null)
        {
            asset = ScriptableObject.CreateInstance(assetType.Name);
            AssetDatabase.CreateAsset((ScriptableObject)asset, assetPath);
            asset.hideFlags = HideFlags.NotEditable;
        }

        return asset;
    }

    static IWorkbook LoadBook(string excelPath)
    {
        using (FileStream stream = File.Open(excelPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
        {
            Debug.Log("ExcelImporter LoadBook: " + excelPath);
            if (Path.GetExtension(excelPath) == ".xls") return new HSSFWorkbook(stream);
            else return new XSSFWorkbook(stream);
        }
    }

    static List<string> GetFieldNamesFromSheetHeader(ISheet sheet)
    {
        IRow headerRow = sheet.GetRow(0);

        var fieldNames = new List<string>();
        for (int i = 0; i < headerRow.LastCellNum; i++)
        {
            var cell = headerRow.GetCell(i);
            if (cell == null || cell.CellType == CellType.Blank) break;
            fieldNames.Add(cell.StringCellValue);
        }
        return fieldNames;
    }
    
    static List<string> GetStringsFromSheetAtRow(ISheet sheet, int rowIndex)
    {
        IRow headerRow = sheet.GetRow(rowIndex);

        var fieldNames = new List<string>();
        for (int i = 0; i < headerRow.LastCellNum; i++)
        {
            var cell = headerRow.GetCell(i);
            if (cell == null || cell.CellType == CellType.Blank) break;
            fieldNames.Add(cell.StringCellValue);
        }
        return fieldNames;
    }

    static object CellToFieldObject(ICell cell, FieldInfo fieldInfo, bool isFormulaEvalute = false)
    {
        var type = isFormulaEvalute ? cell.CachedFormulaResultType : cell.CellType;

        switch (type)
        {
            case CellType.String:
                if (fieldInfo.FieldType.IsEnum) return Enum.Parse(fieldInfo.FieldType, cell.StringCellValue);
                else if (fieldInfo.FieldType.IsSubclassOf(typeof(ScriptableObject)))
                {
                    string path = cell.StringCellValue;
                    if (!path.Contains("Assets/"))
                    {
                        path = "Assets/Prefabs/" + path +".asset";
                    }
                    var loadObject = AssetDatabase.LoadAssetAtPath(path, fieldInfo.FieldType);
                    Debug.Log("Excel Trying to add object: " + loadObject);
                    return loadObject;
                }
                else if (fieldInfo.FieldType == typeof(Sprite))
                {
                    string path = cell.StringCellValue;
                    if (!path.Contains("Assets/"))
                    {
                        path = "Assets/Prefabs/" + path +".png";
                    }
                    var loadObject = AssetDatabase.LoadAssetAtPath(path, fieldInfo.FieldType);
                    Debug.Log("Excel Trying to add object: " + loadObject);
                    return loadObject;
                }
                else if (fieldInfo.FieldType==typeof(GameObject))
                {
                    string path = cell.StringCellValue;
                    if (!path.Contains("Assets/"))
                    {
                        path = "Assets/Prefabs/" + path +".prefab";
                    }
                    var loadObject = AssetDatabase.LoadAssetAtPath(path, fieldInfo.FieldType);
                    Debug.Log("Excel Trying to add object: " + loadObject);
                    return loadObject;
                }
                else return cell.StringCellValue;
            case CellType.Boolean:
                return cell.BooleanCellValue;
            case CellType.Numeric:
                return Convert.ChangeType(cell.NumericCellValue, fieldInfo.FieldType);
            case CellType.Formula:
                if (isFormulaEvalute) return null;
                return CellToFieldObject(cell, fieldInfo, true);
            default:
                if (fieldInfo.FieldType.IsValueType)
                {
                    return Activator.CreateInstance(fieldInfo.FieldType);
                }
                return null;
        }
    }

    static object CreateEntityFromRow(IRow row, List<string> columnNames, Type entityType, string sheetName)
    {
        var entity = Activator.CreateInstance(entityType);

        for (int i = 0; i < columnNames.Count; i++)
        {
            FieldInfo entityField = entityType.GetField(
                columnNames[i],
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic
            );
            if (entityField == null) continue;
            if (!entityField.IsPublic && entityField.GetCustomAttributes(typeof(SerializeField), false).Length == 0) continue;

            ICell cell = row.GetCell(i);
            if (cell == null) continue;

            try
            {
                object fieldValue = CellToFieldObject(cell, entityField);
                entityField.SetValue(entity, fieldValue);
            }
            catch
            {
                throw new Exception(string.Format("Invalid excel cell type at row {0}, column {1}, {2} sheet.", row.RowNum, cell.ColumnIndex, sheetName));
            }
        }
        return entity;
    }

    static object GetEntityListFromSheet(ISheet sheet, Type entityType)
    {
        List<string> excelColumnNames = GetFieldNamesFromSheetHeader(sheet);

        Type listType = typeof(List<>).MakeGenericType(entityType);
        MethodInfo listAddMethod = listType.GetMethod("Add", new Type[] { entityType });
        object list = Activator.CreateInstance(listType);

        // row of index 0 is header
        for (int i = 1; i <= sheet.LastRowNum; i++)
        {
            IRow row = sheet.GetRow(i);
            if (row == null) break;

            ICell entryCell = row.GetCell(0);
            if (entryCell == null || entryCell.CellType == CellType.Blank) break;

            // skip comment row
            if (entryCell.CellType == CellType.String && entryCell.StringCellValue.StartsWith("#")) continue;

            var entity = CreateEntityFromRow(row, excelColumnNames, entityType, sheet.SheetName);
            listAddMethod.Invoke(list, new object[] { entity });
        }
        return list;
    }

    static void ImportExcel(string excelPath, ExcelAssetInfo info)
    {
        string assetPath = "";
        string assetName = info.AssetType.Name + ".asset";

        if (string.IsNullOrEmpty(info.Attribute.AssetPath))
        {
            string basePath = Path.GetDirectoryName(excelPath);
            assetPath = Path.Combine(basePath, assetName);
        }
        else
        {
            var path = Path.Combine("Assets", info.Attribute.AssetPath);
            assetPath = Path.Combine(path, assetName);
        }
        UnityEngine.Object asset = LoadOrCreateAsset(assetPath, info.AssetType);

        IWorkbook book = LoadBook(excelPath);

        var assetFields = info.AssetType.GetFields();
        int sheetCount = 0;

        foreach (var assetField in assetFields)
        {
            ISheet sheet = book.GetSheet(assetField.Name);
            if (sheet == null) continue;

            Type fieldType = assetField.FieldType;
            if (!fieldType.IsGenericType || (fieldType.GetGenericTypeDefinition() != typeof(List<>))) continue;

            Type[] types = fieldType.GetGenericArguments();
            Type entityType = types[0];

            object entities = GetEntityListFromSheet(sheet, entityType);
            assetField.SetValue(asset, entities);
            sheetCount++;
        }

        if (info.Attribute.LogOnImport)
        {
            Debug.Log(string.Format("Imported {0} sheets form {1}.", sheetCount, excelPath));
        }

        EditorUtility.SetDirty(asset);
        AssetDatabase.SaveAssets();
    }
    
    public static List<string[]> ReadExcelFile(string excelPath)
    {
        Debug.Log("Try read excel: " + excelPath);
        List<string[]> stringTable = new List<string[]>();
        IWorkbook book = LoadBook(excelPath);
        for (int s = 0; s < book.NumberOfSheets; s++)
        {
            ISheet sheet = book.GetSheetAt(s);
            if (sheet == null) continue;
            List<string> excelColumnNames = GetFieldNamesFromSheetHeader(sheet);
            // stringTable.Add(excelColumnNames.ToArray());
            Debug.Log("excelColumnNames: " + excelColumnNames.Count);
            for (int i = s==0? 0 : 1; i <= sheet.LastRowNum; i++)
            {
                Debug.Log("导入工作簿中的工作表：" + excelPath+ sheet.SheetName +" Row: " + i);
                IRow row = sheet.GetRow(i);
                if(row != null)
                {
                    string[] rowString = new string[excelColumnNames.Count];
                    for (int j = 0; j < excelColumnNames.Count; j++)
                    {
                        if (excelColumnNames[j].StartsWith("#")) continue;
                        var cell = row.GetCell(j);
                        if(cell?.StringCellValue != null)
                            rowString[j] = cell.StringCellValue;
                        else
                            rowString[j] = "";
                        // Debug.Log($"rowString[{i}][{j}]: {rowString[j]}");
                    }
                    stringTable.Add(rowString);
                }
            }
        }

        return stringTable;
    }

    public static void ExportFullDialogToExcel(List<string> fullTextInGame) {
        var filePath = Application.dataPath + "/Data/ExcelData/Localization_Dialog1.xlsx";
        IWorkbook book = LoadBook(filePath);
        for (int s = 0; s < book.NumberOfSheets; s++)
        {
            ISheet sheet = book.GetSheetAt(s);
            if (sheet == null) continue;
            List<string> excelColumnNames = GetFieldNamesFromSheetHeader(sheet);
            Debug.Log("excelColumnNames: " + excelColumnNames.Count + ", rownum:" + sheet.LastRowNum);
            var lastRow = sheet.LastRowNum;
            for (int i = s==0? 0 : 1; i < lastRow; i++)
            {
                IRow row = sheet.GetRow(i);
                if(row != null)
                {
                    var cell = row.GetCell(0);
                    if (cell?.StringCellValue == null) {
                        continue;
                    }
                    var str = cell.StringCellValue;
                    var index = fullTextInGame.FindIndex(t => t == str);
                    if(index != -1){
                        fullTextInGame.RemoveAt(index);
                    }
                }
            }
            Debug.Log("游戏内新文本数量:" + fullTextInGame.Count);
            for (var i = lastRow; i < lastRow + fullTextInGame.Count; i++) {
                IRow row = sheet.CreateRow(i);
                if (row != null) {
                    var cell = row.CreateCell(0);
                    Debug.Log("游戏内新文本:" + fullTextInGame[i - lastRow]);
                    cell.SetCellValue(fullTextInGame[i - lastRow]);
                }
            }
            break;
        }

        FileStream sw = File.Create(filePath);
        book.Write(sw);
        sw.Close();
        AssetDatabase.ImportAsset(filePath);
        AssetDatabase.Refresh();
    }

    public static void ImportLocalization(string excelPath)
    {
        string fileName =Path.GetFileName(excelPath).Replace(Path.GetExtension(excelPath),"");
        string loadFilePath = $"Assets/Data/{fileName}.prefab";
        Debug.Log("load "+ loadFilePath);
        GameObject sourcePrefab = AssetDatabase.LoadAssetAtPath<GameObject>(loadFilePath);
        var source = sourcePrefab.GetComponent<LanguageSource>();
        List<string[]> stringTable = ReadExcelFile(excelPath);
        
        
        //WriteCharacterRange(stringTable);
        //Debug.Log("列出所有名开始运行...");
        //var outputStr = "";
        //foreach (var kvp in wroteCharacterRange) {
        //    outputStr += kvp;
        //}
        //var cardInfos = FR2_Ref.FindUsedBy(new[] { "c7af09bde8091a54a95049510a7dbd9d" });
        //foreach (var kvp in cardInfos) {
        //    if (kvp.Value.depth == 1) {
        //        var path = kvp.Value.asset.assetPath;
        //        var text = AssetDatabase.LoadAssetAtPath<FullGameText>(path);
        //        text.text += outputStr;
        //        var tempstr = text.text;
        //        tempstr = new string(tempstr.Distinct().ToArray());
        //        text.text = tempstr;
        //        EditorUtility.SetDirty(text);
        //    }
        //}
        //Debug.Log("列出所有名运行结束");


        source.SourceData = new LanguageSourceData();
        string sError = source.SourceData.Import_CSV( string.Empty, stringTable, eSpreadsheetUpdateMode.Replace);
        if (!string.IsNullOrEmpty(sError))
            Debug.Log(sError);
        EditorUtility.SetDirty(sourcePrefab);
        AssetDatabase.SaveAssets();
    }
    //private static void WriteCharacterRange(List<string[]> stringTable)
    //{
    //    if(wroteCharacterRange != null)
    //    {
    //        foreach(var strArray in stringTable)
    //        {
    //            foreach(var str in strArray)
    //            {
    //                foreach(var ch in str)
    //                {
    //                    if (!wroteCharacterRange.Contains(ch))
    //                    {
    //                        wroteCharacterRange.Add(ch);
    //                    }
    //                }
    //            }
    //        }
    //    }
    //}
}
