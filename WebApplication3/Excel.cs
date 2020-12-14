using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.HSSF.UserModel;
using NPOI.SS.Util;
using System.IO;
using System.Data;

namespace WebApplication3
{
    public class ExcelHelper : IDisposable
    {
        private string fileName = null; //文件名
        private IWorkbook workbook = null;
        private FileStream fs = null;
        private bool disposed;

        private string _ExportName = "";
        /// <summary>
        /// 默认导出文件名
        /// </summary>
        public string ExportName { get { return _ExportName; } set { _ExportName = value; } }

        public ExcelHelper()
        {
            disposed = false;
        }

        public DataTable ExcelToDataTable(string fileName, string sheetName, DataTable TbColType)
        {
            ISheet sheet = null;
            DataTable data = new DataTable();
            int startRow = 0, tempErrCol = 0;

            string[] listNum = TbColType.Rows[0]["NumericColList"].ToString().Split(',');
            string[] listDate = TbColType.Rows[0]["DateColList"].ToString().Split(',');
            string[] listBit = TbColType.Rows[0]["BitColList"].ToString().Split(',');
            int ColumnRowCount = Convert.ToInt32(TbColType.Rows[0]["ColRows"]);

            try
            {
                fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                if (fileName.IndexOf(".xlsx") > 0) // 2007版本
                    workbook = new XSSFWorkbook(fs);
                else if (fileName.IndexOf(".xls") > 0) // 2003版本
                    workbook = new HSSFWorkbook(fs);

                if (sheetName != null)
                {
                    sheet = workbook.GetSheet(sheetName);
                    if (sheet == null) //如果没有找到指定的sheetName对应的sheet，则尝试获取当前的sheet
                    {
                        sheet = workbook.GetSheetAt(workbook.ActiveSheetIndex);
                    }
                }
                else
                {
                    sheet = workbook.GetSheetAt(workbook.ActiveSheetIndex);
                }
                if (sheet != null)
                {
                    IRow firstRow = sheet.GetRow(0);
                    int cellCount = firstRow.LastCellNum; //一行最后一个cell的编号 即总的列数

                    IRow TmpColumnRow = null;
                    ICell cell = null;
                    string cellValue = string.Empty;
                    //写表头 i是行  j是列
                    for (int i = 0; i < ColumnRowCount; i++)
                    {
                        TmpColumnRow = sheet.GetRow(i); //获取当前标题行

                        for (int j = 0; j < cellCount; j++)
                        {
                            cell = TmpColumnRow.GetCell(j); //获取当前单元格

                            if (i == 0) //第一行直接加
                            {
                                if (cell != null)
                                {
                                    //获取单元格的值
                                    cellValue = cell.StringCellValue;
                                    if (cellValue == null || string.IsNullOrWhiteSpace(cellValue))
                                    {
                                        cellValue = j.ToString();
                                    }

                                    //第一次手动加，确保后缀是加上去的
                                    if ((data.Columns.Contains(cellValue)))
                                    {
                                        int k = 1;
                                        cellValue = cellValue + k.ToString();
                                        k++;

                                        while (data.Columns.Contains(cellValue))
                                        {
                                            cellValue = cellValue.Substring(0, cellValue.Length - 1) + k.ToString();
                                            k++;
                                        }
                                    }

                                    //没名字后，把该列加上
                                    if(listNum.Contains((j+1).ToString()))
                                        data.Columns.Add(cellValue, typeof(System.Decimal));
                                    else if (listDate.Contains((j + 1).ToString()))
                                        data.Columns.Add(cellValue, typeof(System.DateTime));
                                    else if (listBit.Contains((j + 1).ToString()))
                                        data.Columns.Add(cellValue, typeof(System.Boolean));
                                    else
                                        data.Columns.Add(cellValue, typeof(System.String));
                                }
                            }
                            else //第二行开始，下面格子为空，不替换  下面格子有值 ，替换上面的
                            {
                                if (cell != null)
                                {
                                    //获取单元格的值
                                    cellValue = cell.StringCellValue;
                                    if (cellValue == null || string.IsNullOrWhiteSpace(cellValue) || cellValue == "")
                                    {
                                        continue;
                                    }

                                    if ((data.Columns.Contains(cellValue)) && (data.Columns[j].ColumnName != cellValue)) //同列名称相同不循环
                                    {
                                        int k = 1;
                                        cellValue = cellValue + k.ToString();
                                        k++;

                                        while (data.Columns.Contains(cellValue))
                                        {
                                            cellValue = cellValue.Substring(0, cellValue.Length - 1) + k.ToString();
                                            k++;
                                        }
                                    }

                                    data.Columns[j].ColumnName = cellValue;
                                }
                            }
                        }
                    }

                    startRow = ColumnRowCount;
                    //最后一列的标号
                    int rowCount = sheet.LastRowNum;
                    for (int i = startRow; i <= rowCount; ++i)
                    {
                        IRow row = sheet.GetRow(i);
                        if (row == null) continue; //没有数据的行默认是null　　

                        //第一和二个单元格同时为空时，默认为当前行没有数据
                        if ((row.GetCell(0) == null || string.IsNullOrWhiteSpace(ValidToString(row.GetCell(0))))
                            && (row.GetCell(1) == null || string.IsNullOrWhiteSpace(ValidToString(row.GetCell(1)))))
                            continue;

                        DataRow dataRow = data.NewRow();
                        for (int j = row.FirstCellNum; j < cellCount; ++j)
                        {
                            if (row.GetCell(j) == null) continue;
                            tempErrCol = j + 1;

                            if (listNum.Contains((j + 1).ToString()))
                            {
                                row.GetCell(j).SetCellType(CellType.Numeric);
                                dataRow[j] = row.GetCell(j).NumericCellValue;
                            }
                            else if (listDate.Contains((j + 1).ToString()))
                            {
                                row.GetCell(j).SetCellType(CellType.Numeric);
                                dataRow[j] = DateTime.FromOADate(row.GetCell(j).NumericCellValue);
                            }
                            else if (listBit.Contains((j + 1).ToString()))
                            {
                                row.GetCell(j).SetCellType(CellType.Boolean);
                                dataRow[j] = row.GetCell(j).BooleanCellValue;
                            }
                            else
                            {
                                row.GetCell(j).SetCellType(CellType.String);
                                dataRow[j] = row.GetCell(j).StringCellValue;
                            }
                        }
                        data.Rows.Add(dataRow);
                    }
                }

                return data;
            }
            catch (Exception ex)
            {
                throw new Exception("导入失败!" + tempErrCol .ToString() + "列转换出错。" + NewLine + ex.Message);
            }
        }

        /// <summary>
        /// 将excel中的数据导入到DataTable中
        /// </summary>
        /// <param name="sheetName">excel工作薄sheet的名称[可空]</param>
        /// <param name="ColumnRowCount">表头行的行数</param>
        /// <returns>返回的DataTable</returns>
        public DataTable ExcelToDataTable(string fileName, string sheetName, int ColumnRowCount)
        {
            ISheet sheet = null;
            DataTable data = new DataTable();
            int startRow = 0;

            try
            {
                fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                if (fileName.IndexOf(".xlsx") > 0) // 2007版本
                    workbook = new XSSFWorkbook(fs);
                else if (fileName.IndexOf(".xls") > 0) // 2003版本
                    workbook = new HSSFWorkbook(fs);

                if (sheetName != null)
                {
                    sheet = workbook.GetSheet(sheetName);
                    if (sheet == null) //如果没有找到指定的sheetName对应的sheet，则尝试获取当前的sheet
                    {
                        sheet = workbook.GetSheetAt(workbook.ActiveSheetIndex);
                    }
                }
                else
                {
                    sheet = workbook.GetSheetAt(workbook.ActiveSheetIndex);
                }
                if (sheet != null)
                {
                    IRow firstRow = sheet.GetRow(0);
                    int cellCount = firstRow.LastCellNum; //一行最后一个cell的编号 即总的列数

                    IRow TmpColumnRow = null;
                    ICell cell = null;
                    string cellValue = string.Empty;
                    //写表头 i是行  j是列
                    for (int i = 0; i < ColumnRowCount; i++)
                    {
                        TmpColumnRow = sheet.GetRow(i); //获取当前标题行

                        for (int j = 0; j < cellCount; j++)
                        {
                            cell = TmpColumnRow.GetCell(j); //获取当前单元格

                            if (i == 0) //第一行直接加
                            {
                                if (cell != null)
                                {
                                    //获取单元格的值
                                    cellValue = cell.StringCellValue;
                                    if (cellValue == null || string.IsNullOrWhiteSpace(cellValue))
                                    {
                                        cellValue = j.ToString();
                                    }

                                    //第一次手动加，确保后缀是加上去的
                                    if ((data.Columns.Contains(cellValue)))
                                    {
                                        int k = 1;
                                        cellValue = cellValue + k.ToString();
                                        k++;

                                        while (data.Columns.Contains(cellValue))
                                        {
                                            cellValue = cellValue.Substring(0, cellValue.Length - 1) + k.ToString();
                                            k++;
                                        }
                                    }

                                    //没名字后，把该列加上
                                    data.Columns.Add(cellValue);
                                }
                            }
                            else //第二行开始，下面格子为空，不替换  下面格子有值 ，替换上面的
                            {
                                if (cell != null)
                                {
                                    //获取单元格的值
                                    cellValue = cell.StringCellValue;
                                    if (cellValue == null || string.IsNullOrWhiteSpace(cellValue) || cellValue == "")
                                    {
                                        continue;
                                    }

                                    if ((data.Columns.Contains(cellValue)) && (data.Columns[j].ColumnName != cellValue)) //同列名称相同不循环
                                    {
                                        int k = 1;
                                        cellValue = cellValue + k.ToString();
                                        k++;

                                        while (data.Columns.Contains(cellValue))
                                        {
                                            cellValue = cellValue.Substring(0, cellValue.Length - 1) + k.ToString();
                                            k++;
                                        }
                                    }

                                    data.Columns[j].ColumnName = cellValue;
                                }
                            }
                        }
                    }

                    startRow = ColumnRowCount;
                    //最后一列的标号
                    int rowCount = sheet.LastRowNum;
                    for (int i = startRow; i <= rowCount; ++i)
                    {
                        IRow row = sheet.GetRow(i);
                        if (row == null) continue; //没有数据的行默认是null　　

                        //第一和二个单元格同时为空时，默认为当前行没有数据
                        if ((row.GetCell(0) == null || string.IsNullOrWhiteSpace(ValidToString(row.GetCell(0))))
                            && (row.GetCell(1) == null || string.IsNullOrWhiteSpace(ValidToString(row.GetCell(1)))))
                            continue;

                        DataRow dataRow = data.NewRow();
                        for (int j = row.FirstCellNum; j < cellCount; ++j)
                        {
                            if (row.GetCell(j) == null) continue;

                            if (row.GetCell(j).CellType == CellType.Formula)
                            {
                                dataRow[j] = row.GetCell(j).NumericCellValue;
                            }
                            else if (row.GetCell(j).CellType == CellType.Numeric)
                            {
                                if (DateUtil.IsCellDateFormatted(row.GetCell(j)) == true)
                                    dataRow[j] = row.GetCell(j).DateCellValue;
                                else
                                    dataRow[j] = row.GetCell(j).NumericCellValue;
                            }
                            else
                            {
                                dataRow[j] = row.GetCell(j);
                            }
                        }
                        data.Rows.Add(dataRow);
                    }
                }

                return data;
            }
            catch (Exception ex)
            {
                throw new Exception("导入失败!" + NewLine + ex.Message);
            }
        }

        public DataTable ReadExcel()
        {
            if (fileName == null) return null;

            DataTable dtTable = new DataTable();
            List<string> rowList = new List<string>();
            ISheet sheet;
            using (var stream = new FileStream(fileName, FileMode.Open))
            {
                stream.Position = 0;
                XSSFWorkbook xssWorkbook = new XSSFWorkbook(stream);
                sheet = xssWorkbook.GetSheetAt(0);
                IRow headerRow = sheet.GetRow(0);
                int cellCount = headerRow.LastCellNum;
                for (int j = 0; j < cellCount; j++)
                {
                    ICell cell = headerRow.GetCell(j);
                    if (cell == null || string.IsNullOrWhiteSpace(cell.ToString())) continue;
                    {
                        dtTable.Columns.Add(cell.ToString());
                    }
                }
                for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
                {
                    IRow row = sheet.GetRow(i);
                    if (row == null) continue;
                    if (row.Cells.All(d => d.CellType == CellType.Blank)) continue;
                    for (int j = row.FirstCellNum; j < cellCount; j++)
                    {
                        if (row.GetCell(j) != null)
                        {
                            if (!string.IsNullOrEmpty(row.GetCell(j).ToString()) && !string.IsNullOrWhiteSpace(row.GetCell(j).ToString()))
                            {
                                rowList.Add(row.GetCell(j).ToString());
                            }
                        }
                    }
                    if (rowList.Count > 0)
                        dtTable.Rows.Add(rowList.ToArray());
                    rowList.Clear();
                }
            }
            return dtTable;
        }

        public string NewLine = "\r\n";

        public string ValidToString(object v)
        {
            if (v == null || v == DBNull.Value) return "";
            else return v.ToString().Trim();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    if (fs != null)
                        fs.Close();
                }

                fs = null;
                disposed = true;
            }
        }
    }
}