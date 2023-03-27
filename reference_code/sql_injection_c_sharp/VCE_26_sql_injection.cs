private void btnreturn_save_Click(object sender, EventArgs e)
        {
            string tranid;

            config.singleResult("SELECT concat(STRT,END) FROM tblautonumber WHERE ID = 6");
            tranid = config.dt.Rows[0].Field<string>(0);

            if (txttransactionid.Text == "")
            {
                MessageBox.Show("There are empty fields left that must be fill up!", "Invalid", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (dtCus_addedlist.RowCount == 0)
            {
                MessageBox.Show("Cart is empty!", "Invalid", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            sql = "SELECT `ITEMID`, `QTY` FROM `tblstock_in_out` WHERE  `TRANSACTIONNUMBER` ='" + txttransactionid.Text + "'";
            config.singleResult(sql);

            foreach (DataRow row in config.dt.Rows)
            {
                for (int i = 0; i < dtCus_addedlist.Rows.Count; i++)
                {
                    if (dtCus_addedlist.Rows[i].Cells[0].Value.ToString() == row.Field<string>(0))
                    {
                        if (int.Parse(dtCus_addedlist.Rows[i].Cells[4].Value.ToString()) > row.Field<int>(1))
                        {
                            MessageBox.Show("The returned quantity of the item ( " + dtCus_addedlist.Rows[i].Cells[1].Value.ToString() + " ) is greater than the available quantity of it.", "Invalid", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return;
                        }
                    }
                    if (dtCus_addedlist.Rows[i].Cells[4].Value.ToString() == "")
                    {
                        MessageBox.Show("Set your purpose.", "Invalid", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                }
            }

            foreach (DataGridViewRow r in dtCus_addedlist.Rows)
            {
                sql = "INSERT INTO `tblstock_return` (  `STOCKRETURNNUMBER`, `ITEMID`, `RETURNDATE`, `QTY`, `TOTALPRICE`, `OWNER_CUS_ID`)" +
                        " VALUES ('" + tranid + "','" + r.Cells[0].Value + "','" + DateTime.Now.ToString("yyyy-MM-dd") + "','" + r.Cells[4].Value +
                        "','" + r.Cells[5].Value + "','" + custormerid + "')";
                config.Execute_Query(sql);

                //     '-----------------------------------------------update item
                sql = "UPDATE `tblitems`  SET `QTY`=`QTY` + '" + r.Cells[4].Value + "' WHERE ITEMID='" + r.Cells[0].Value + "'";
                config.Execute_Query(sql);

                sql = "UPDATE `tblstock_in_out` SET  `QTY`=`QTY`-'" + r.Cells[4].Value + "', `TOTALPRICE`=`TOTALPRICE`-'" + r.Cells[5].Value + "'  WHERE `STOCKOUTID` ='" + r.Cells[6].Value + "'";
                config.Execute_Query(sql);
            }


            sql = "INSERT INTO  `tbltransaction` (`TRANSACTIONNUMBER`,  `TRANSACTIONDATE`,  `TYPE`, `SUPLIERCUSTOMERID`)" +
                  " VALUES ('" + txttransactionid.Text + "','" + DateTime.Now.ToString("yyyy-MM-dd") + "','Returned','" + custormerid + "')";
            config.Execute_Query(sql);

            // '-----------------------------------------------update autonumber
            config.Execute_Query("UPDATE tblautonumber SET END= END + INCREMENT WHERE ID = 6");

            // '------------------------------------------------------------
            MessageBox.Show("Item(s) has been returned in the database.");
            // '------------------------------------------------------------clearing
            funct.clearTxt(GroupBox3);
            dtCus_addedlist.Rows.Clear();

            frmReturn_Load(sender, e);
        }