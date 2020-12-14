// **************************************************
// Custom code for MainController
// Created: 2020/12/2 14:20:02
// **************************************************
using System;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Windows.Forms;
using Ice.BO;
using Ice.UI;
using Ice.Lib;
using Ice.Adapters;
using Ice.Lib.Customization;
using Ice.Lib.ExtendedProps;
using Ice.Lib.Framework;
using Ice.Lib.Searches;
using Ice.UI.FormFunctions;
using Erp.Adapters;

public class Script
{
	// ** Wizard Insert Location - Do Not Remove 'Begin/End Wizard Added Module Level Variables' Comments! **
	// Begin Wizard Added Module Level Variables **

	private UD01Adapter _ud01Adapter;
	private EpiDataView _edvV_QuoteUpdateInfoCGVer1_1View3;
	private DataTable UD01_Column;
	private EpiDataView _edvUD01;
	private string _Key1UD01;
	private string _Key2UD01;
	private string _Key3UD01;
	private string _Key4UD01;
	private string _Key5UD01;
	private DataView V_QuoteUpdateInfoCGVer1_1View3_DataView;
	// End Wizard Added Module Level Variables **

	// Add Custom Module Level Variables Here **

	public void InitializeCustomCode()
	{
		// ** Wizard Insert Location - Do not delete 'Begin/End Wizard Added Variable Initialization' lines **
		// Begin Wizard Added Variable Initialization

		InitializeUD01Adapter();
		this._Key1UD01 = string.Empty;
		this._Key2UD01 = string.Empty;
		this._Key3UD01 = string.Empty;
		this._Key4UD01 = string.Empty;
		this._Key5UD01 = string.Empty;
		this.baseToolbarsManager.ToolClick += new Infragistics.Win.UltraWinToolbars.ToolClickEventHandler(this.baseToolbarsManager_ToolClickForUD01);
		this.MainController.BeforeToolClick += new Ice.Lib.Framework.BeforeToolClickEventHandler(this.MainController_BeforeToolClickForUD01);
		this.MainController.AfterToolClick += new Ice.Lib.Framework.AfterToolClickEventHandler(this.MainController_AfterToolClickForUD01);
		this.V_QuoteUpdateInfoCGVer1_1View3_Row.EpiRowChanged += new EpiRowChanged(this.V_QuoteUpdateInfoCGVer1_1View3_AfterRowChangeForUD01);
		this.V_QuoteUpdateInfoCGVer1_1View3_DataView = this.V_QuoteUpdateInfoCGVer1_1View3_Row.dataView;
		this.V_QuoteUpdateInfoCGVer1_1View3_DataView.ListChanged += new ListChangedEventHandler(this.V_QuoteUpdateInfoCGVer1_1View3_DataView_ListChangedForUD01);
		this.V_QuoteUpdateInfoCGVer1_1View3_Row.BeforeResetDataView += new Ice.Lib.Framework.EpiDataView.BeforeResetDataViewDelegate(this.V_QuoteUpdateInfoCGVer1_1View3_BeforeResetDataViewForUD01);
		this.V_QuoteUpdateInfoCGVer1_1View3_Row.AfterResetDataView += new Ice.Lib.Framework.EpiDataView.AfterResetDataViewDelegate(this.V_QuoteUpdateInfoCGVer1_1View3_AfterResetDataViewForUD01);
		// End Wizard Added Variable Initialization

		// Begin Wizard Added Custom Method Calls

		this.BtnCheck2.Click += new System.EventHandler(this.BtnCheck2_Click);
		this.BtnCheck1.Click += new System.EventHandler(this.BtnCheck1_Click);
		this.BtnCheck3.Click += new System.EventHandler(this.BtnCheck3_Click);
		// End Wizard Added Custom Method Calls
	}

	public void DestroyCustomCode()
	{
		// ** Wizard Insert Location - Do not delete 'Begin/End Wizard Added Object Disposal' lines **
		// Begin Wizard Added Object Disposal

		this.BtnCheck2.Click -= new System.EventHandler(this.BtnCheck2_Click);
		this.BtnCheck1.Click -= new System.EventHandler(this.BtnCheck1_Click);
		this.BtnCheck3.Click -= new System.EventHandler(this.BtnCheck3_Click);
		if ((this._ud01Adapter != null))
		{
			this._ud01Adapter.Dispose();
			this._ud01Adapter = null;
		}
		this._edvUD01 = null;
		this._edvV_QuoteUpdateInfoCGVer1_1View3 = null;
		this.UD01_Column = null;
		this._Key1UD01 = null;
		this._Key2UD01 = null;
		this._Key3UD01 = null;
		this._Key4UD01 = null;
		this._Key5UD01 = null;
		this.baseToolbarsManager.ToolClick -= new Infragistics.Win.UltraWinToolbars.ToolClickEventHandler(this.baseToolbarsManager_ToolClickForUD01);
		this.MainController.BeforeToolClick -= new Ice.Lib.Framework.BeforeToolClickEventHandler(this.MainController_BeforeToolClickForUD01);
		this.MainController.AfterToolClick -= new Ice.Lib.Framework.AfterToolClickEventHandler(this.MainController_AfterToolClickForUD01);
		this.V_QuoteUpdateInfoCGVer1_1View3_Row.EpiRowChanged -= new EpiRowChanged(this.V_QuoteUpdateInfoCGVer1_1View3_AfterRowChangeForUD01);
		this.V_QuoteUpdateInfoCGVer1_1View3_DataView.ListChanged -= new ListChangedEventHandler(this.V_QuoteUpdateInfoCGVer1_1View3_DataView_ListChangedForUD01);
		this.V_QuoteUpdateInfoCGVer1_1View3_DataView = null;
		this.V_QuoteUpdateInfoCGVer1_1View3_Row.BeforeResetDataView -= new Ice.Lib.Framework.EpiDataView.BeforeResetDataViewDelegate(this.V_QuoteUpdateInfoCGVer1_1View3_BeforeResetDataViewForUD01);
		this.V_QuoteUpdateInfoCGVer1_1View3_Row.AfterResetDataView -= new Ice.Lib.Framework.EpiDataView.AfterResetDataViewDelegate(this.V_QuoteUpdateInfoCGVer1_1View3_AfterResetDataViewForUD01);
		// End Wizard Added Object Disposal

		// Begin Custom Code Disposal

		// End Custom Code Disposal
	}

	private void BtnCheck2_Click(object sender, System.EventArgs args)
	{
		CheckData(2);
	}

	private void BtnCheck1_Click(object sender, System.EventArgs args)
	{
		CheckData(1);
	}

	private void BtnCheck3_Click(object sender, System.EventArgs args)
	{
		CheckData(3);
	}

	private Boolean HaveRight(string UserId, string GroupName)
	{
		Boolean haveRight = false;

		try
		{
			// Get the Quoted data from QuoteHed
			UserFileAdapter uf = new UserFileAdapter(this.oTrans);
			uf.BOConnect();
			uf.GetByID(UserId);

			if (uf.UserFileData.Tables["UserFile"].Rows.Count > 0)
			{
				string[] GroupList = uf.UserFileData.Tables["UserFile"].Rows[0]["GroupList"].ToString().Split('~');
				if (GroupList.Contains(GroupName)) { haveRight = true; }
			}

			uf.Dispose();
		}
		catch (System.Exception ex)
		{
			ExceptionBox.Show(ex);
		}

		return haveRight;
	}

	private void CheckData(int CheckLevel)
	{
		//Last, current, next CheckColumn   CheckRightGroup   check is True or false
		string LastLevel, CurLevel, CurUserLevel, NextLevel, GroupName, isCheck = "=1", isUnCheck = "=0";
		int TotalCheckRows = 0; //All Quoted Rows
		Boolean ck = false; //defalt check status to set

		if (CheckLevel == 1)
		{
			LastLevel = "1";
			CurLevel = "Checked1_c";
			CurUserLevel = "Checked1By_c";
			NextLevel = "Checked2_c";
			GroupName = "Quote";
		}
		else if (CheckLevel == 2)
		{
			LastLevel = "Checked1_c";
			CurLevel = "Checked2_c";
			CurUserLevel = "Checked2By_c";
			NextLevel = "Checked3_c";
			GroupName = "QA";
		}
		else
		{
			LastLevel = "Checked2_c";
			CurLevel = "Checked3_c";
			CurUserLevel = "Checked3By_c";
			NextLevel = "0";
			GroupName = "Admin";
		}

		try
		{
			EpiDataView edvUser = (EpiDataView)oTrans.EpiDataViews["CallContextClientData"];
			string UserId = edvUser.dataView[edvUser.Row]["CurrentUserId"].ToString();

			if (!HaveRight(UserId, GroupName))
			{
				ExceptionBox.Show(new System.Exception("no right to check!"));
				return;
			}

			//get the view data
			EpiDataView edv = (EpiDataView)(oTrans.EpiDataViews["V_QuoteUpdateInfoCGVer1_1View3"]);
			if (edv.dataView.Count > 0)
			{
				// Get the Quoted data from QuoteHed
				QuoteAdapter qa = new QuoteAdapter(this.oTrans);
				qa.BOConnect();

				string whereClause = " Quoted = 1 ";
				System.Collections.Hashtable whereClauses = new System.Collections.Hashtable(1);
				whereClauses.Add("QuoteHed", whereClause);

				SearchOptions searchOptions = SearchOptions.CreateRuntimeSearch(whereClauses, DataSetMode.RowsDataSet);
				qa.InvokeSearch(searchOptions);
				TotalCheckRows = qa.QuoteData.Tables["QuoteHed"].Rows.Count;

				//check or uncheck [checked rows = max rows ,then uncheck.   checked rows != max rows, then check]
				DataRow[] RowsCur = qa.QuoteData.QuoteHed.Select(CurLevel + isCheck);
				if (RowsCur.Length != TotalCheckRows)
				{
					ck = true;
				}

				//check cur row, last level must be checked.
				//uncheck cur row, the next level must be unchecked
				DataRow[] RowsPrevious = ck ? qa.QuoteData.QuoteHed.Select(LastLevel + isCheck) : qa.QuoteData.QuoteHed.Select(NextLevel + isUnCheck);
				if (RowsPrevious.Length == TotalCheckRows)
				{
					//check the data in SQL
					for (int i = 0; i < qa.QuoteData.Tables["QuoteHed"].Rows.Count; i++)
					{
						qa.QuoteData.Tables["QuoteHed"].Rows[i][CurLevel] = ck;
						qa.QuoteData.Tables["QuoteHed"].Rows[i][CurUserLevel] = UserId;
					}
					qa.Update();
					qa.Dispose();

					// Check the data on the interface
					for (int i = 0; i < edv.dataView.Count; i++)
					{
						edv.dataView[i]["QuoteHed_" + CurLevel] = ck;
					}

					edv.Notify(new EpiNotifyArgs(this.oTrans, edv.Row, edv.Column));
				}
				else
				{
					ExceptionBox.Show(new System.Exception(ck ? "check the last level please!" : "uncheck the next level please!"));
				}
			}
			else
			{
				ExceptionBox.Show(new System.Exception("no row can be checked or unchecked!"));
			}
		}
		catch (System.Exception ex)
		{
			ExceptionBox.Show(ex);
		}
	}

	private void CallUD01AdapterGetByIDMethod(string stringId)
	{
		try
		{
			// Declare and Initialize EpiDataView Variables
			// Declare and create an instance of the Adapter.
			UD01Adapter adapterUD01 = new UD01Adapter(this.oTrans);
			adapterUD01.BOConnect();

			// Declare and Initialize Variables
			// TODO: You may need to replace the default initialization with valid values as required for the BL method call.

			// Call Adapter method
			bool result = adapterUD01.GetByID(stringId);

			// Cleanup Adapter Reference
			adapterUD01.Dispose();

		}
		catch (System.Exception ex)
		{
			ExceptionBox.Show(ex);
		}
	}

	private void InitializeUD01Adapter()
	{
		// Create an instance of the Adapter.
		this._ud01Adapter = new UD01Adapter(this.oTrans);
		this._ud01Adapter.BOConnect();

		// Add Adapter Table to List of Views
		// This allows you to bind controls to the custom UD Table
		this._edvUD01 = new EpiDataView();
		this._edvUD01.dataView = new DataView(this._ud01Adapter.UD01Data.UD01);
		this._edvUD01.AddEnabled = true;
		this._edvUD01.AddText = "New UD01";
		if ((this.oTrans.EpiDataViews.ContainsKey("UD01View") == false))
		{
			this.oTrans.Add("UD01View", this._edvUD01);
		}

		// Initialize DataTable variable
		this.UD01_Column = this._ud01Adapter.UD01Data.UD01;

		// Initialize EpiDataView field.
		this._edvV_QuoteUpdateInfoCGVer1_1View3 = ((EpiDataView)(this.oTrans.EpiDataViews["V_QuoteUpdateInfoCGVer1_1View3"]));

		// Set the parent view / keys for UD child view
		string[] parentKeyFields = new string[1];
		string[] childKeyFields = new string[1];
		parentKeyFields[0] = "QuoteHed_Checked1_c";
		childKeyFields[0] = "Key1";
		this._edvUD01.SetParentView(this._edvV_QuoteUpdateInfoCGVer1_1View3, parentKeyFields, childKeyFields);

		if ((this.oTrans.PrimaryAdapter != null))
		{
			// this.oTrans.PrimaryAdapter.GetCurrentDataSet(Ice.Lib.Searches.DataSetMode.RowsDataSet).Tables.Add(this._edvUD01.dataView.Table.Clone())
		}

	}

	private void GetUD01Data(string key1, string key2, string key3, string key4, string key5)
	{
		if ((this._Key1UD01 != key1) || (this._Key2UD01 != key2) || (this._Key3UD01 != key3) || (this._Key4UD01 != key4) || (this._Key5UD01 != key5))
		{
			// Build where clause for search.
			string whereClause = "Key1 = \'" + key1 + "\' And Key2 = \'" + key2 + "\' And Key3 = \'" + key3 + "\' And Key4 = \'" + key4 + "\'";
			System.Collections.Hashtable whereClauses = new System.Collections.Hashtable(1);
			whereClauses.Add("UD01", whereClause);

			// Call the adapter search.
			SearchOptions searchOptions = SearchOptions.CreateRuntimeSearch(whereClauses, DataSetMode.RowsDataSet);
			this._ud01Adapter.InvokeSearch(searchOptions);

			if ((this._ud01Adapter.UD01Data.UD01.Rows.Count > 0))
			{
				this._edvUD01.Row = 0;
			}
			else
			{
				this._edvUD01.Row = -1;
			}

			// Notify that data was updated.
			this._edvUD01.Notify(new EpiNotifyArgs(this.oTrans, this._edvUD01.Row, this._edvUD01.Column));

			// Set key fields to their new values.
			this._Key1UD01 = key1;
			this._Key2UD01 = key2;
			this._Key3UD01 = key3;
			this._Key4UD01 = key4;
			this._Key5UD01 = key5;
		}
	}

	private void GetNewUD01Record()
	{
		DataRow parentViewRow = this._edvV_QuoteUpdateInfoCGVer1_1View3.CurrentDataRow;
		// Check for existence of Parent Row.
		if ((parentViewRow == null))
		{
			return;
		}
		if (this._ud01Adapter.GetaNewUD01())
		{
			string quotehed_checked1_c = parentViewRow["QuoteHed_Checked1_c"].ToString();

			// Get unique row count id for Key5
			int rowCount = this._ud01Adapter.UD01Data.UD01.Rows.Count;
			int lineNum = rowCount;
			bool goodIndex = false;
			while ((goodIndex == false))
			{
				// Check to see if index exists
				DataRow[] matchingRows = this._ud01Adapter.UD01Data.UD01.Select("Key5 = \'" + lineNum.ToString() + "\'");
				if ((matchingRows.Length > 0))
				{
					lineNum = (lineNum + 1);
				}
				else
				{
					goodIndex = true;
				}
			}

			// Set initial UD Key values
			DataRow editRow = this._ud01Adapter.UD01Data.UD01.Rows[(rowCount - 1)];
			editRow.BeginEdit();
			editRow["Key1"] = quotehed_checked1_c;
			editRow["Key2"] = string.Empty;
			editRow["Key3"] = string.Empty;
			editRow["Key4"] = string.Empty;
			editRow["Key5"] = lineNum.ToString();
			editRow.EndEdit();

			// Notify that data was updated.
			this._edvUD01.Notify(new EpiNotifyArgs(this.oTrans, (rowCount - 1), this._edvUD01.Column));
		}
	}

	private void SaveUD01Record()
	{
		// Save adapter data
		this._ud01Adapter.Update();
	}

	private void DeleteUD01Record()
	{
		// Check to see if deleted view is ancestor view
		bool isAncestorView = false;
		Ice.Lib.Framework.EpiDataView parView = this._edvUD01.ParentView;
		while ((parView != null))
		{
			if ((this.oTrans.LastView == parView))
			{
				isAncestorView = true;
				break;
			}
			else
			{
				parView = parView.ParentView;
			}
		}

		// If Ancestor View then delete all child rows
		if (isAncestorView)
		{
			DataRow[] drsDeleted = this._ud01Adapter.UD01Data.UD01.Select("Key1 = \'" + this._Key1UD01 + "\' AND Key2 = \'" + this._Key2UD01 + "\' AND Key3 = \'" + this._Key3UD01 + "\' AND Key4 = \'" + this._Key4UD01 + "\'");
			for (int i = 0; (i < drsDeleted.Length); i = (i + 1))
			{
				this._ud01Adapter.Delete(drsDeleted[i]);
			}
		}
		else
		{
			if ((this.oTrans.LastView == this._edvUD01))
			{
				if ((this._edvUD01.Row >= 0))
				{
					DataRow drDeleted = ((DataRow)(this._ud01Adapter.UD01Data.UD01.Rows[this._edvUD01.Row]));
					if ((drDeleted != null))
					{
						if (this._ud01Adapter.Delete(drDeleted))
						{
							if ((_edvUD01.Row > 0))
							{
								_edvUD01.Row = (_edvUD01.Row - 1);
							}

							// Notify that data was updated.
							this._edvUD01.Notify(new EpiNotifyArgs(this.oTrans, this._edvUD01.Row, this._edvUD01.Column));
						}
					}
				}
			}
		}
	}

	private void UndoUD01Changes()
	{
		this._ud01Adapter.UD01Data.RejectChanges();

		// Notify that data was updated.
		this._edvUD01.Notify(new EpiNotifyArgs(this.oTrans, this._edvUD01.Row, this._edvUD01.Column));
	}

	private void ClearUD01Data()
	{
		this._Key1UD01 = string.Empty;
		this._Key2UD01 = string.Empty;
		this._Key3UD01 = string.Empty;
		this._Key4UD01 = string.Empty;
		this._Key5UD01 = string.Empty;

		this._ud01Adapter.UD01Data.Clear();

		// Notify that data was updated.
		this._edvUD01.Notify(new EpiNotifyArgs(this.oTrans, this._edvUD01.Row, this._edvUD01.Column));
	}

	private void baseToolbarsManager_ToolClickForUD01(object sender, Infragistics.Win.UltraWinToolbars.ToolClickEventArgs args)
	{
		// EpiMessageBox.Show(args.Tool.Key);
		switch (args.Tool.Key)
		{
			case "EpiAddNewNew UD01":
				GetNewUD01Record();
				break;

			case "ClearTool":
				ClearUD01Data();
				break;

			case "UndoTool":
				UndoUD01Changes();
				break;
		}
	}

	private void MainController_BeforeToolClickForUD01(object sender, Ice.Lib.Framework.BeforeToolClickEventArgs args)
	{
		// EpiMessageBox.Show(args.Tool.Key);
		switch (args.Tool.Key)
		{
			case "SaveTool":
				SaveUD01Record();
				break;
		}
	}

	private void MainController_AfterToolClickForUD01(object sender, Ice.Lib.Framework.AfterToolClickEventArgs args)
	{
		// EpiMessageBox.Show(args.Tool.Key);
		switch (args.Tool.Key)
		{
			case "DeleteTool":
				if ((args.Cancelled == false))
				{
					DeleteUD01Record();
				}
				break;
		}
	}

	private void V_QuoteUpdateInfoCGVer1_1View3_AfterRowChangeForUD01(EpiRowChangedArgs args)
	{
		// ** add AfterRowChange event handler
		string quotehed_checked1_c = args.CurrentView.dataView[args.CurrentRow]["QuoteHed_Checked1_c"].ToString();
		GetUD01Data(quotehed_checked1_c, string.Empty, string.Empty, string.Empty, string.Empty);
	}

	private void V_QuoteUpdateInfoCGVer1_1View3_DataView_ListChangedForUD01(object sender, ListChangedEventArgs args)
	{
		// ** add ListChanged event handler
		string quotehed_checked1_c = V_QuoteUpdateInfoCGVer1_1View3_DataView[0]["QuoteHed_Checked1_c"].ToString();
		GetUD01Data(quotehed_checked1_c, string.Empty, string.Empty, string.Empty, string.Empty);
	}

	private void V_QuoteUpdateInfoCGVer1_1View3_BeforeResetDataViewForUD01(object sender, EventArgs args)
	{
		// ** remove ListChanged event handler
		this.V_QuoteUpdateInfoCGVer1_1View3_DataView.ListChanged -= new ListChangedEventHandler(this.V_QuoteUpdateInfoCGVer1_1View3_DataView_ListChangedForUD01);
	}

	private void V_QuoteUpdateInfoCGVer1_1View3_AfterResetDataViewForUD01(object sender, EventArgs args)
	{
		// ** reassign DataView and add ListChanged event handler
		this.V_QuoteUpdateInfoCGVer1_1View3_DataView = this.V_QuoteUpdateInfoCGVer1_1View3_Row.dataView;
		this.V_QuoteUpdateInfoCGVer1_1View3_DataView.ListChanged += new ListChangedEventHandler(this.V_QuoteUpdateInfoCGVer1_1View3_DataView_ListChangedForUD01);
	}
}
