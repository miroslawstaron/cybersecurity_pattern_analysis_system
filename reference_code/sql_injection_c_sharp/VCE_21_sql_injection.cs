public ActionResult GetPerson(int id)
{
    string sql = @"select * from people where personid=" + id.ToString();

    SqlDataReader dr = DBUtil.GetDataReader(sql);
    ViewBag.dr = dr;

    return View();
}