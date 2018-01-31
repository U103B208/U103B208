using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Web.Script.Serialization;
using System.Runtime.Serialization;

/// <summary>
/// DBOperation 的摘要描述
/// </summary>
public class DBOperation : IDisposable
{

    public static SqlConnection sqlCon;  //用於連接資料庫  

    //將下面的引號之間的內容換成上面記錄下的屬性中的連接字符串  
    private String ConServerStr = @"Data Source=localhost;Initial Catalog=fish;Integrated Security=True";//連接資料庫的位置字串
    private string fish = "";

    public DBOperation()
    {
        if (sqlCon == null)//如果沒有連接資料庫
        {
            sqlCon = new SqlConnection();//開啟一個新的連接
            sqlCon.ConnectionString = ConServerStr;
            sqlCon.Open();//開啟連接
        }
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }
    public void Dispose()//用來釋放資源的方法
    {
        if (sqlCon != null)
        {
            sqlCon.Close();
            sqlCon = null;//如果已經連接就釋放資源
        }
    }
    //獲取所有資料表內的資料，現在已經沒再用了，改成新的
    public List<string> selectAllCargoInfor()
    {
        List<string> list = new List<string>();
        List<string> selectAll = new List<string>();
        
        JavaScriptSerializer js = new JavaScriptSerializer();
        try
        {
            string sql = "select * from fishtest";
            
            SqlCommand cmd = new SqlCommand(sql, sqlCon);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                //將结果集信息添加到返回向量中  
                //這邊再做一個for迴圈跑陣列SIZE，list.Add(reader[i].Tostring());
                //test a = new test();
                //a.cno = reader["cno"].ToString();
                //a.cname = reader["cname"].ToString();
                //a.cnum = reader["cnum"].ToString();
                //list.Add(reader["cno"].ToString());
                //list.Add(reader["cname"].ToString());
                //list.Add(reader["cnum"].ToString());
                //list.Add(a);
            }
            reader.Close();
            cmd.Dispose();
        }
        catch (Exception)
        {
        }
        return list;
    }
    //新增一條數據的方法，回傳Boolean的true或false
    public Boolean insertCargoInfo(int cno,string cname, string cnum)
    {
        try//跟JAVA一樣連接資料庫需要使用try catch包住
        {
            string sql = "insert into fishtest (cno,cname,cnum) values("+ cno +",'" + cname + "','" + cnum + "')";//對數據庫操作的字串
            SqlCommand cmd = new SqlCommand(sql, sqlCon);//連接資料庫
            cmd.ExecuteNonQuery();//執行操作
            cmd.Dispose();//釋放資源
            return true;//回傳一個true
        }
        catch (Exception)
        {
            return false;//有錯誤就回傳false
        }
    }
    //刪除一條數據的方法，回傳Boolean的true或false
    public bool deleteCargoInfo(string cno)//因為寫法跟新增一樣所以就不註解了
    {
        try
        {
            string sql = "delete from fishtest where cno=" + cno;
            SqlCommand cmd = new SqlCommand(sql, sqlCon);
            cmd.ExecuteNonQuery();
            cmd.Dispose();

            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
    //獲取所有欄位名稱，自己寫的，後來多半被用來作為搜尋依據
    public List<string> selectcolumn(string table_name)//需傳入一個table_name的字串參數
    {
        List<string> test = new List<string>();//建一個string的List準備來接收資料庫的資料
        try
        {
            string sql = "select column_name from INFORMATION_SCHEMA.COLUMNS where table_name='"+table_name+"'";//對資料庫進行搜索欄位的操作
            SqlCommand cmd = new SqlCommand(sql, sqlCon);
            SqlDataReader reader = cmd.ExecuteReader();//與JAVA不同的是，C#是用SqlDataReader來讀取資料，用的是SqlCommand的ExecuteReader()

            while (reader.Read())//與JAVA不同，這用的是.Read()
            {
                test.Add(reader.GetString(0));//將取得的字串放入List中
            }
            JavaScriptSerializer js = new JavaScriptSerializer();//創建一個JavaScriptSerializer用來改變編碼方式。但是因為這裡必須回傳一個List所以還不會用到這個方法。
            fish = js.Serialize(test);
            reader.Close();//關閉
            cmd.Dispose();//關閉
            return test;//回傳List
        }
        catch (Exception e)//因為回傳是一個List，所以只好將error轉成string 存進List後回傳。
        {
            List<string> error = new List<string>();
            error.Add(e.ToString());
            return error;
        }
    }
    //獲取所有table名稱
    public string selecttable()//這裡回傳一個string 類型，為什麼是回傳一個string類型，下面會解釋
    {
        List<string> test = new List<string>();//宣告一個List用來存資料庫取得的資料
        try
        {
            string sql = "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES ORDER BY TABLE_NAME";//搜索所有table
            SqlCommand cmd = new SqlCommand(sql, sqlCon);
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                test.Add(reader.GetString(0));//將搜索到的table存到List中
            }
            JavaScriptSerializer js = new JavaScriptSerializer();//創建JavaScriptSerializer，這裡終於要使用了。

            reader.Close();
            cmd.Dispose();

            //由於JavaScriptSerializer改變編碼方式會變成一個string所以再回傳的時候必須回傳一個string
            return js.Serialize(test);//JavaScriptSerializer會把List展開變成一個string 字串的資料。
        }
        catch (Exception)
        {
            return "false";
        }
    }

    //測試擷取的字串，取得所有資料，成功版本
    public string selecttest(string table_name)//這裡要傳入一個table_name的參數
    {
        List<string> list = new List<string>();//建立一個List來存取資料庫搜索到的資料
        List<string> selectAll = selectcolumn(table_name);//上面寫的selectcolumn方法被我也用在這裡，因為selectcolumn回傳是一個List所以也剛剛好可以用一個List來接收

        JavaScriptSerializer js = new JavaScriptSerializer();//因為這裡也打算用JavaScriptSerializer來排序資料，所以導致我回傳也是一個string 
        try
        {
            string sql = "select * from "+table_name;//利用傳入的table_name做為一個搜尋條件

            SqlCommand cmd = new SqlCommand(sql, sqlCon);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                //這邊做一個for迴圈跑陣列SIZE，list.Add(reader[i].Tostring());//10.18改成list.Add(reader[selectAll[i]].ToString());
                //準備返回一個字串，Android 端解析此字串，目前用土法煉鋼的方式將字串中的資料提取出來。
                //test a = new test();
                //a.cno = reader["cno"].ToString();
                //a.cname = reader["cname"].ToString();
                //a.cnum = reader["cnum"].ToString();
                for (int i = 0; i < selectAll.Count; i++)//在while 中再做一層迴圈跑所有取得到的欄位，感謝網路上的寫法讓我有了這種方法
                {
                    list.Add(reader[selectAll[i]].ToString());//用第一個建立的List存，用reader讀取selectcolumn 傳回的List的每一個欄位
                }
                
            }
            reader.Close();
            cmd.Dispose();
        }
        catch (Exception)
        {
        }
        return js.Serialize(list);//將資料轉換回傳
    }
    //測試取得最新一筆資料，成功版本
    public string selecttopdata(string table_name)//跟搜尋所有資料幾乎一樣寫法
    {
        List<string> list = new List<string>();
        List<string> selectAll = selectcolumn(table_name);

        JavaScriptSerializer js = new JavaScriptSerializer();
        try
        {
            string sql = @"select top 1 * from """+table_name+@""" order by " + selectAll[0] + " desc";//因為這個寫法是我想出來的，所以是照用app上寫的搜尋欄位寫法一樣

            SqlCommand cmd = new SqlCommand(sql, sqlCon);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())//其實這裡不需要寫這層迴圈的，只是寫法太相似我就懶得改了，因為這裡搜尋的是top1所以本來就只會有一筆資料
            {
                for (int i = 0; i < selectAll.Count; i++)
                {
                    list.Add(reader[selectAll[i]].ToString());
                }
            }
            reader.Close();
            cmd.Dispose();
        }
        catch (Exception e)
        {
        }
        return js.Serialize(list);
    }
}