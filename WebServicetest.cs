using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;

/// <summary>
/// WebServicetest 的摘要描述
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// 若要允許使用 ASP.NET AJAX 從指令碼呼叫此 Web 服務，請取消註解下列一行。
// [System.Web.Script.Services.ScriptService]
public class WebServicetest : System.Web.Services.WebService
{
    System.DateTime currentTime = new System.DateTime();
    DBOperation dbOperation = new DBOperation();
    public WebServicetest()
    {
        //如果使用設計的元件，請取消註解下列一行
        //InitializeComponent(); 
    }

    [WebMethod(Description ="10.9調用成功")]
    public string HelloWorld()
    {
        string now = DateTime.Now.ToString("MM.dd");
        return now;
    }
    [WebMethod(Description = "獲取所有database的訊息---等到欄位調用成功就不遠，已經不再用的方法")]
    public List<string> selectAllCargoInfor()
    {
        return dbOperation.selectAllCargoInfor();
    }

    [WebMethod(Description = "增加一條database訊息---10.11調用成功")]
    public Boolean insertCargoInfo(int cno,string cname, string cnum)
    {
        return dbOperation.insertCargoInfo(cno , cname, cnum);
    }

    [WebMethod(Description = "刪除一條database訊息---10.11調用成功")]
    public bool deleteCargoInfo(string cno)
    {
        return dbOperation.deleteCargoInfo(cno);
    }
    [WebMethod(Description = "獲取所有column的訊息---10.19改成搜尋資料庫的依據，調用成功")]
    public List<string> selectcolumn(string table_name)
    {
        return dbOperation.selectcolumn(table_name);
    }
    [WebMethod(Description = "獲取所有table的訊息---10.20調用成功")]
    public string selecttable()
    {
        return dbOperation.selecttable();
    }
    [WebMethod(Description = "測試這該死的C#中---10.20調用成功")]
    public string selecttest(string table_name)
    {
        return dbOperation.selecttest(table_name);
    }
    [WebMethod(Description = "測試搜尋最新資料---10.20成功")]
    public string selecttopdata(string table_name)//在WebService開啟新的服務，並需要傳入一個table_name參數，其他服務同上
    {
        return dbOperation.selecttopdata(table_name);//調用selecttopdata方法，並回傳其結果
    }

} 