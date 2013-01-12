
// (C) Copyright 2002-2005 by Autodesk, Inc. 
//
// Permission to use, copy, modify, and distribute this software in
// object code form for any purpose and without fee is hereby granted, 
// provided that the above copyright notice appears in all copies and 
// that both that copyright notice and the limited warranty and
// restricted rights notice below appear in all supporting 
// documentation.
//
// AUTODESK PROVIDES THIS PROGRAM "AS IS" AND WITH ALL FAULTS. 
// AUTODESK SPECIFICALLY DISCLAIMS ANY IMPLIED WARRANTY OF
// MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE.  AUTODESK, INC. 
// DOES NOT WARRANT THAT THE OPERATION OF THE PROGRAM WILL BE
// UNINTERRUPTED OR ERROR FREE.
//
// Use, duplication, or disclosure by the U.S. Government is subject to 
// restrictions set forth in FAR 52.227-19 (Commercial Computer
// Software - Restricted Rights) and DFAR 252.227-7013(c)(1)(ii)
// (Rights in Technical Data and Computer Software), as applicable.
//

//-----------------------------------------------------------------------------
//----- CDialogSteel.h : Declaration of the CDialogSteel
//-----------------------------------------------------------------------------
#pragma once

//-----------------------------------------------------------------------------
#include "acui.h"
#include "afxwin.h"

	//-------------------------------------------------------------------------------------------
	// 
	//  功能： H型钢用户输入对话框
	//
	//  作者：Qin H.X.
	//
	// 日期：200709
	//
	//  历史：
	//          
	//
	//----------------------------------------------------------------------------------------------
class CDialogSteel : public CAcUiDialog {
	DECLARE_DYNAMIC (CDialogSteel)

public:
	CDialogSteel (CWnd *pParent =NULL, HINSTANCE hInstance =NULL) ;

	enum { IDD = IDD_DIALOG_STEEL} ;

protected:
	virtual void DoDataExchange (CDataExchange *pDX) ;
	afx_msg LRESULT OnAcadKeepFocus (WPARAM, LPARAM) ;

	DECLARE_MESSAGE_MAP()
public:
	// 选点按钮
	CAcUiPickButton m_btnPickPonit;
protected:
	// -----------------------------------------------------------------------------
	virtual BOOL OnInitDialog(void);
public:
	// X坐标
	CEdit m_ctrlPtX;
public:
	// Y坐标
	CEdit m_ctrlPtY;
public:
	afx_msg void OnBnClickedButtonPick();
public:

public:
	double m_dPtX;
public:
	double m_dPtY;
public:
	double m_dPtZ;
public:
	// 截面高
	double m_dH;
public:
	// 翼缘宽
	double m_dW;
public:
	// 翼缘厚
	double m_dA;
public:
	// 腹板宽
	double m_dB;
public:
	// 长度
	double m_dL;
public:
	afx_msg void OnBnClickedOk();
} ;
