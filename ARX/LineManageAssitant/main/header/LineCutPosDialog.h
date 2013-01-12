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
	void GetTransformData();

	DECLARE_MESSAGE_MAP()

public:
	CAcUiNumericEdit mOffset;

	afx_msg void OnBnClickedOk();
	afx_msg void OnBnClickedX();
	afx_msg void OnBnClickedY();
	afx_msg void OnBnClickedZ();

	static AcDb3dSolid *m3dSolid;

private:

	CString m_strOffset;

	CString m_A00;
	CString m_A10;
	CString m_A20;

	CString m_A01;
	CString m_A11;
	CString m_A21;

	CString m_A02;
	CString m_A12;
	CString m_A22;

	CString m_T0;
	CString m_T1;
	CString m_T2;

	int m_Direction;

public:
	CButton mDirectionX;
	CButton m_DirectionY;
	CButton m_DirectionZ;

	CAcUiNumericEdit edit_m_A00;
	CAcUiNumericEdit edit_m_A10;
	CAcUiNumericEdit edit_m_A20;

	CAcUiNumericEdit edit_m_A01;
	CAcUiNumericEdit edit_m_A11;
	CAcUiNumericEdit edit_m_A21;

	CAcUiNumericEdit edit_m_A02;
	CAcUiNumericEdit edit_m_A12;
	CAcUiNumericEdit edit_m_A22;

	CAcUiNumericEdit edit_m_T0;
	CAcUiNumericEdit edit_m_T1;
	CAcUiNumericEdit edit_m_T2;
};
