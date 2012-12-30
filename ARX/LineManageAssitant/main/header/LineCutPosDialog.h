#pragma once

#include <resource.h>
#include <dbsymtb.h>
#include <dbapserv.h>
#include <adslib.h>
#include <adui.h>
#include <acui.h>
#include "afxwin.h"

// LineCutPosDialog dialog

class LineCutPosDialog : public CAcUiDialog
{
	DECLARE_DYNAMIC(LineCutPosDialog)

public:
	LineCutPosDialog(CWnd* pParent = NULL);   // standard constructor
	virtual ~LineCutPosDialog();

	virtual BOOL OnInitDialog();

// Dialog Data
	enum { IDD = IDD_DIALOG_CUT_POS };

protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support

	DECLARE_MESSAGE_MAP()

public:
	CAcUiNumericEdit mOffset;

	afx_msg void OnBnClickedOk();
	afx_msg void OnBnClickedX();
	afx_msg void OnBnClickedY();
	afx_msg void OnBnClickedZ();

private:

	CString m_strOffset;
	int m_Direction;

public:
	CButton mDirectionX;
	CButton m_DirectionY;
	CButton m_DirectionZ;
};
