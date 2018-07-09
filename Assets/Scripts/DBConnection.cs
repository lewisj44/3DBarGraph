using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using Mono.Data.SqliteClient;


public class DBConnection : MonoBehaviour 
{
    private string dbPath;
    private string xLabel;
    private string yLabel;


    private void Awake()
    {
        dbPath = "URI=file:" + Application.dataPath + "/testData2.db";
        Debug.Log(dbPath);
        CreateSchema();
    }


    public void CreateSchema()
    {
        using (SqliteConnection conn = new SqliteConnection(dbPath))
        {
            string queryString = "CREATE TABLE IF NOT EXISTS 'sales_data'( `year` INTEGER NOT NULL, `quarter` INTEGER NOT NULL, `sales` INTEGER NOT NULL);";
            SqliteCommand cmd = new SqliteCommand(queryString, conn);
            cmd.Connection.Open();
            cmd.ExecuteNonQuery();
        }
    }


    public void InsertSale(int year, int quarter, int sales)
    {
        using (SqliteConnection conn = new SqliteConnection(dbPath))
        {
            
            string queryString = "INSERT INTO sales_data (year, quarter, sales) VALUES ('" + year + "', '" + quarter + "', '" + sales + "')";
            SqliteCommand cmd = new SqliteCommand(queryString, conn);
            cmd.Connection.Open();
            cmd.ExecuteNonQuery();
        }
    }

    public int GetSale(int year, int quarter)
    {
        using (SqliteConnection conn = new SqliteConnection(dbPath))
        {
            string queryString = "SELECT sales as saleData FROM sales_data WHERE year = @year AND quarter = @quarter";
            SqliteCommand cmd = new SqliteCommand(queryString, conn);
            cmd.Parameters.Add(new SqliteParameter("@year", year));
            cmd.Parameters.Add(new SqliteParameter("@quarter", quarter));
            cmd.Connection.Open();
            var reader = cmd.ExecuteReader();
            var result = -1;
            if(reader.Read()) result = System.Convert.ToInt32(reader["saleData"]);
            return result;
        }
    }

    public void UpdateSale(int year, int quarter, int sale)
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