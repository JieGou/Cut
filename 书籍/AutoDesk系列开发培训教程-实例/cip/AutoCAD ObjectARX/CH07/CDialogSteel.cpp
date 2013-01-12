
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
//----- CDialogSteel.cpp : Implementation of CDialogSteel
//-----------------------------------------------------------------------------
#include "StdAfx.h"
#include "resource.h"
#include "CDialogSteel.h"
#include "CSolidHelper.h"

//-----------------------------------------------------------------------------
IMPLEMENT_DYNAMIC (CDialogSteel, CAcUiDialog)

BEGIN_MESSAGE_MAP(CDialogSteel, CAcUiDialog)
	ON_MESSAGE(WM_ACAD_KEEPFOCUS, OnAcadKeepFocus)
	ON_BN_CLICKED(IDC_BUTTON_Pick, &CDialogSteel::OnBnClickedButtonPick)
	ON_BN_CLICKED(IDOK, &CDialogSteel::OnBnClickedOk)
END_MESSAGE_MAP()

//-----------------------------------------------------------------------------
CDialogSteel::CDialogSteel (CWnd *pParent /*=NULL*/, HINSTANCE hInstance /*=NULL*/) : CAcUiDialog (CDialogSteel::IDD, pParent, hInstance) 
, m_dH(80)
, m_dW(50)
, m_dA(10)
, m_dB(8)
, m_dL(200)
{
m_dPtX = 0.0;
m_dPtY =  0.0;
m_dPtZ =  0.0;
}

//-----------------------------------------------------------------------------
void CDialogSteel::DoDataExchange (CDataExchange *pDX) {
	CAcUiDialog::DoDataExchange (pDX) ;
	DDX_Control(pDX, IDC_BUTTON_Pick, m_btnPickPonit);
	DDX_Control(pDX, IDC_EDIT_PTX, m_ctrlPtX);
	DDX_Control(pDX, IDC_EDIT_PTY, m_ctrlPtY);
	DDX_Text(pDX, IDC_EDIT_PTX, m_dPtX);
	DDX_Text(pDX, IDC_EDIT_PTY, m_dPtY);
	DDX_Text(pDX, IDC_EDIT_PTZ, m_dPtZ);
	DDX_Text(pDX, IDC_EDIT_HEIGHT, m_dH);
	DDX_Text(pDX, IDC_EDIT_Width, m_dW);
	DDX_Text(pDX, IDC_EDIT_SIDEWIDTH, m_dA);
	DDX_Text(pDX, IDC_EDIT_MIDDLEWIDTH, m_dB);
	DDX_Text(pDX, IDC_EDIT_LEN, m_dL);

}

//-----------------------------------------------------------------------------
//----- Needed for modeless dialogs to keep focus.
//----- Return FALSE to not keep the focus, return TRUE to keep the focus
LRESULT CDialogSteel::OnAcadKeepFocus (WPARAM, LPARAM) {
	return (TRUE) ;
}

// -----------------------------------------------------------------------------
BOOL CDialogSteel::OnInitDialog(void)
{
	BOOL retCode =CAcUiDialog::OnInitDialog () ;
	//m_btnPickPonit.AutoLoad ();
	return (retCode) ;
}
   //-------------------------------------------------------------------------------------------
	// 
	//  功能： 获取绘制位置
	//
	//  作者：Qin H.X.
	//
	// 日期：200709
	//
	//  历史：
	//          
	//
	//----------------------------------------------------------------------------------------------
void CDialogSteel::OnBnClickedButtonPick()
{
	//隐藏对话框，等待用户输入
	BeginEditorCommand();		
    ads_point pt; 
    if (acedGetPoint(NULL, _T("\n请拾取H型钢的绘制位置: "), pt) == RTNORM) 
	{ 
		//重新调出对话框
        CompleteEditorCommand();	
        m_dPtX= pt[X];
		m_dPtY= pt[Y];
		m_dPtZ= pt[Z];

		UpdateData(FALSE);
    } else 
	{
		//不显示对话框并且关闭
        CancelEditorCommand();	
    }

}
    //-------------------------------------------------------------------------------------------
	// 
	//  功能： 获取数据，拉伸实体
	//
	//  作者：Qin H.X.
	//
	// 日期：200709
	//
	//  历史：
	//          
	//
	//----------------------------------------------------------------------------------------------
void CDialogSteel::OnBnClickedOk()
{
	UpdateData(TRUE);
	acutPrintf(_T("创建H型钢...\n"));
	// 基于用户输入的数据创建多义线
	AcDbPolyline * pLine;
	AcGePoint2d pt0(0.0,0.0); 
	AcGePoint2d pt1(m_dW,0.0); 
	AcGePoint2d pt2(m_dW,m_dA); 
	AcGePoint2d pt3(0.5*(m_dW+m_dB),m_dA); 
	AcGePoint2d pt4(0.5*(m_dW+m_dB),m_dH- m_dA); 
	AcGePoint2d pt5(m_dW,m_dH-m_dA); 
	AcGePoint2d pt6(m_dW,m_dH); 
	AcGePoint2d pt7(0.0,m_dH); 
	AcGePoint2d pt8(0.0,m_dH- m_dA); 
	AcGePoint2d pt9(0.5*(m_dW-m_dB),m_dH- m_dA); 
	AcGePoint2d pt10(0.5*(m_dW-m_dB),m_dA); 
	AcGePoint2d pt11(0.0,m_dA); 

	pLine = new AcDbPolyline;
	pLine->setDatabaseDefaults();
	pLine->reset (Adesk::kFalse, 0);
	pLine->addVertexAt (0, pt0);
	pLine->addVertexAt (1, pt1);
	pLine->addVertexAt (2, pt2);
	pLine->addVertexAt (3, pt3);
	pLine->addVertexAt (4, pt4);
	pLine->addVertexAt (5, pt5);
	pLine->addVertexAt (6, pt6);
	pLine->addVertexAt (7, pt7);
	pLine->addVertexAt (8, pt8);
	pLine->addVertexAt (9, pt9);
	pLine->addVertexAt (10, pt10);
	pLine->addVertexAt (11, pt11);
	pLine->setClosed (Adesk::kTrue);
	// 对创建的多义线进行平移变换
	//移动到用户选择的位置点
	AcGeMatrix3d mat;
	mat.setToTranslation (AcGeVector3d(m_dPtX,m_dPtY,m_dPtZ));
	pLine->transformBy (mat);
	//拉伸为三维实体
	AcDbObjectId idSolid;
	CSolidHelper::extrudePoly(pLine,m_dL,idSolid);

	delete pLine;
	OnOK();
}
