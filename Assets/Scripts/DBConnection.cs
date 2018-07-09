using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using System.IO;
using Mono.Data.SqliteClient;

public class DBConnection : MonoBehaviour 
{
    public string dbName;
    public int startYear;
    public int endYear;

    private string dbPath;
    private string xLabel;
    private string yLabel;


    private void Awake()
    {
        dbPath = "URI=file:" + Application.dataPath + "/" + dbName + ".db";
        string filePath = Application.dataPath + "/" + dbName + ".db";
        if(!File.Exists(filePath))
        {
            Debug.Log("DATA NOT FOUND. CREATING DATABASE");
            CreateSchema();
            for (int year = startYear; year <= endYear; year++)
            {
                for (int qtr = 0; qtr < 4; qtr++)
                {
                    InsertSale(year, qtr, 1.00);
                }
            }
        }
        Debug.Log(dbPath);
    }


    public void CreateSchema()
    {
        using (SqliteConnection conn = new SqliteConnection(dbPath))
        {
            string queryString = "CREATE TABLE IF NOT EXISTS 'sales_data'( `year` INTEGER NOT NULL, `quarter` INTEGER NOT NULL, `sales` NUMERIC(18,2) NOT NULL);";
            SqliteCommand cmd = new SqliteCommand(queryString, conn);
            cmd.Connection.Open();
            cmd.ExecuteNonQuery();
        }
    }


    public void InsertSale(int year, int quarter, double sale)
    {
        using (SqliteConnection conn = new SqliteConnection(dbPath))
        {
            
            string queryString = "INSERT INTO sales_data (year, quarter, sales) VALUES ('" + year + "', '" + quarter + "', '" + sale + "')";
            SqliteCommand cmd = new SqliteCommand(queryString, conn);
            cmd.Connection.Open();
            cmd.ExecuteNonQuery();
        }
    }

    public double GetSale(int year, int quarter)
    {
        using (SqliteConnection conn = new SqliteConnection(dbPath))
        {
            string queryString = "SELECT sales as saleData FROM sales_data WHERE year = @year AND quarter = @quarter";
            SqliteCommand cmd = new SqliteCommand(queryString, conn);
            cmd.Parameters.Add(new SqliteParameter("@year", year));
            cmd.Parameters.Add(new SqliteParameter("@quarter", quarter));
            cmd.Connection.Open();
            var reader = cmd.ExecuteReader();
            var result = -1.00;
            if(reader.Read()) result = System.Convert.ToDouble(reader["saleData"]);
            return result;
        }
    }

    public void UpdateSale(int year, int quarter, float sale)
    {
        using (SqliteConnection conn = new SqliteConnection(dbPath))
        {
            string queryString = "UPDATE sales_data SET sales = @sale WHERE year = @year AND quarter = @quarter";
            SqliteCommand cmd = new SqliteCommand(queryString, conn);
            cmd.Parameters.Add(new SqliteParameter("@year", year + this.GetStartYear()));
            cmd.Parameters.Add(new SqliteParameter("@quarter", quarter));
            cmd.Parameters.Add(new SqliteParameter("@sale", sale));
            cmd.Connection.Open();
            cmd.ExecuteNonQuery();
        }
    }

    public int GetNumYears()
    {
        using (SqliteConnection conn = new SqliteConnection(dbPath))
        {
            string queryString = "SELECT COUNT (DISTINCT year) as yearCount FROM sales_data";
            SqliteCommand cmd = new SqliteCommand(queryString, conn);
            cmd.Connection.Open();
            var reader = cmd.ExecuteReader();
            var result = -1;
            if (reader.Read()) result = System.Convert.ToInt32(reader["yearCount"]);
            return result;
        }
    }

    public int GetNumQuarters()
    {
        using (SqliteConnection conn = new SqliteConnection(dbPath))
        {
            string queryString = "SELECT COUNT (DISTINCT quarter) as qtrCount  FROM sales_data";
            SqliteCommand cmd = new SqliteCommand(queryString, conn);
            cmd.Connection.Open();
            var reader = cmd.ExecuteReader();
            var result = -1;
            if (reader.Read()) result = System.Convert.ToInt32(reader["qtrCount"]);
            return result;
        }
    }

    public int GetStartYear()
    {
        using (SqliteConnection conn = new SqliteConnection(dbPath))
        {
            string queryString = "SELECT MIN(year) as minYear  FROM sales_data";
            SqliteCommand cmd = new SqliteCommand(queryString, conn);
            cmd.Connection.Open();
            var reader = cmd.ExecuteReader();
            var result = -1;
            if (reader.Read()) result = System.Convert.ToInt32(reader["minYear"]);
            return result;
        }
    }
}