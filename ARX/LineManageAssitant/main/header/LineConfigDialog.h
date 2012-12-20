#pragma once
#include "afxcmn.h"

#include <resource.h>

#include <dbsymtb.h>
#include <dbapserv.h>
#include <adslib.h>
#include <adui.h>
#include <acui.h>

// LineConfigDialog dialog

class LineConfigDialog : public CAcUiDialog
{
	DECLARE_DYNAMIC(LineConfigDialog)

public:
	LineConfigDialog(CWnd* pParent = NULL);   // standard constructor
	virtual ~LineConfigDialog();

// Dialog Data
	enum { IDD = IDD_DIALOG_LINE_CONFIG };

protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	virtual BOOL OnInitDialog();

	BOOL InitLineHeader();
	BOOL InitLineData();

	afx_msg void OnBnClickedButtonAdd();

	DECLARE_MESSAGE_MAP()
public:
	
	//CListCtrl m_lineConfig;
	CAcUiListCtrl m_lineConfig;
};
