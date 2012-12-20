#pragma once

#include <resource.h>
#include <dbsymtb.h>
#include <dbapserv.h>
#include <adslib.h>
#include <adui.h>
#include <acui.h>

class LMALineConfigManagerDialog :public CAcUiDialog
{
public:
	LMALineConfigManagerDialog(CWnd* parent = NULL);

	~LMALineConfigManagerDialog(void);

	 enum { IDD = IDD_DIALOG_LINE_CONFIG };

private:

	void init();

	void initLineHeader();

	void initLineData();

protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
//	afx_msg void OnBnClickedButtonAdd();

	DECLARE_MESSAGE_MAP()

private:

	CListCtrl m_LineList;

public:
	
};

