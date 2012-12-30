// LineCutPosDialog.cpp : implementation file
//

#include "stdafx.h"
#include "LineCutPosDialog.h"
#include "afxdialogex.h"

#include "acedads.h"
#include "accmd.h"
#include <adscodes.h>

#include <adsdlg.h>

#include <dbapserv.h>

#include <dbregion.h>

#include <gepnt3d.h>

//symbol table
#include <dbsymtb.h>

//3D Object
#include <dbsol3d.h>

// LineCutPosDialog dialog

IMPLEMENT_DYNAMIC(LineCutPosDialog, CAcUiDialog)

LineCutPosDialog::LineCutPosDialog(CWnd* pParent /*=NULL*/)
	: CAcUiDialog(LineCutPosDialog::IDD, pParent),
	m_Direction(0)
{

}

LineCutPosDialog::~LineCutPosDialog()
{
}

BOOL LineCutPosDialog::OnInitDialog()
{
	//和页面交互数据
	CAcUiDialog::OnInitDialog();

	return TRUE;
}

void LineCutPosDialog::DoDataExchange(CDataExchange* pDX)
{
	CAcUiDialog::DoDataExchange(pDX);
	DDX_Control(pDX, IDC_OFFSET, mOffset);
	DDX_Control(pDX, IDC_X, mDirectionX);
	DDX_Control(pDX, IDC_Y, m_DirectionY);
	DDX_Control(pDX, IDC_Z, m_DirectionZ);
}

AcDbObjectId PostToModelSpace(AcDbEntity* pEnt)
{
	AcDbBlockTable *pBlockTable;
	acdbHostApplicationServices()->workingDatabase()
		->getBlockTable(pBlockTable, AcDb::kForRead);

	AcDbBlockTableRecord *pBlockTableRecord;
	pBlockTable->getAt(ACDB_MODEL_SPACE, pBlockTableRecord,
	AcDb::kForWrite);

	AcDbObjectId entId;
	pBlockTableRecord->appendAcDbEntity(entId, pEnt);

	pBlockTable->close();
	pBlockTableRecord->close();
	pEnt->close();

	return entId;
}

void moveToBottom(AcDbEntity* pEntry)
{
	AcGeVector3d vec(-8,10,0);

	AcGeMatrix3d moveMatrix;
	moveMatrix.setToTranslation(vec);

	pEntry->transformBy(moveMatrix);
}

AcDb3dSolid* drawCylinder()
{
	// 创建特定参数的圆柱体（实际上最后一个参数决定了实体是一个圆锥体还是圆柱） 
	AcDb3dSolid *pSolid = new AcDb3dSolid(); 
	pSolid->createFrustum(30, 10, 10, 10);

	// 将圆锥体添加到模型空间
	PostToModelSpace(pSolid);

	return pSolid;
}

void generateCutRegion(AcDb3dSolid* pSolid, int offset, int direction)
{
	//创建切面
	AcGePlane plane;

	if( direction == 1)
		plane.set(AcGePoint3d(offset,0,0),AcGeVector3d(1,0,0));
	else if( direction == 2)
		plane.set(AcGePoint3d(0,offset,0),AcGeVector3d(0,1,0));
	else if( direction == 3)
		plane.set(AcGePoint3d(0,0,offset),AcGeVector3d(0,0,1));

	//得到实体与切面相切的截面
	AcDbRegion *pSelectionRegion = NULL;
	pSolid->getSection(plane, pSelectionRegion);

	//将其移动到YZ平面
	//moveToBottom(pSelectionRegion);
	
	//将截面加入到模型空间
	PostToModelSpace(pSelectionRegion);
}

void LineCutPosDialog::OnBnClickedOk()
{
	static bool objCreated = false;
	static AcDb3dSolid* pSolid = NULL;

	if( !objCreated )
	{
		pSolid = drawCylinder();
		objCreated = true;
	}

	mOffset.Convert();
    mOffset.GetWindowText(m_strOffset);
	
	int offset = 0;
	
	if( m_strOffset.GetLength())
		offset = _wtoi(m_strOffset);

	generateCutRegion(pSolid,offset,m_Direction);

	CAcUiDialog::OnOK();
}

void LineCutPosDialog::OnBnClickedX()
{
	m_Direction = 1;
}

void LineCutPosDialog::OnBnClickedY()
{
	m_Direction = 2;
}

void LineCutPosDialog::OnBnClickedZ()
{
	m_Direction = 3;
}

BEGIN_MESSAGE_MAP(LineCutPosDialog, CAcUiDialog)
	ON_BN_CLICKED(IDOK, &LineCutPosDialog::OnBnClickedOk)
	ON_BN_CLICKED(IDC_X, &LineCutPosDialog::OnBnClickedX)
	ON_BN_CLICKED(IDC_Y, &LineCutPosDialog::OnBnClickedY)
	ON_BN_CLICKED(IDC_Z, &LineCutPosDialog::OnBnClickedZ)
END_MESSAGE_MAP()

// LineCutPosDialog message handlers
