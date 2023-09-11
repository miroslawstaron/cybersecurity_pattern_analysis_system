private void txttransactionid_TextChanged(object sender, EventArgs e)
{
    
    sql = "SELECT p.SUPLIERCUSTOMERID as ID, `FIRSTNAME`, `LASTNAME` ,`ADDRESS` FROM  `tbltransaction` t, `tblperson`  p  WHERE t.`SUPLIERCUSTOMERID`=p.`SUPLIERCUSTOMERID` AND `TRANSACTIONNUMBER`='" + txttransactionid.Text + "'";
    config.singleResult(sql);
    if(config.dt.Rows.Count > 0)
    {
        custormerid = config.dt.Rows[0].Field<string>("ID");
        txtreturn_name.Text = config.dt.Rows[0].Field<string>("FIRSTNAME").ToString() + " " + config.dt.Rows[0].Field<string>("LASTNAME").ToString();
        txtreturn_address.Text = config.dt.Rows[0].Field<string>("ADDRESS").ToString();




        sql = "SELECT   i.`ITEMID`, `NAME`, `DESCRIPTION`, `PRICE`,`TRANSACTIONDATE`, o.`QTY`, `TOTALPRICE`,`STOCKOUTID` FROM  `tblitems` i , `tblstock_in_out` o WHERE i.`ITEMID`=o.`ITEMID` AND `TRANSACTIONNUMBER`='" + txttransactionid.Text + "'";
        config.Load_DTG(sql,dtgCus_itemlist);
        dtgCus_itemlist.Columns[7].Visible = false;
    }
    else
    {
        txtreturn_name.Clear();
        txtreturn_address.Clear();
        dtgCus_itemlist.Columns.Clear();
    }

    
}