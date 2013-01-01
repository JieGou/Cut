// AddLineDialog.cpp : implementation file
//

#include "stdafx.h"
#include "AddLineDialog.h"
#include "afxdialogex.h"

#include "GlobalDataConfig.h"
#include "LineTypeConfigPropertySheet.h"
#include <LMAException.h>
#include <LMAUtils.h>

using namespace com::guch::assistent::data;
using namespace com::guch::assistent::exception;

// AddLineConfigDialog dialog
namespace com
{

namespace guch
{

namespace assistent
{

namespace config
{

IMPLEMENT_DYNAMIC(AddLineConfigDialog, CDialog)

AddLineConfigDialog::AddLineConfigDialog(CWnd* pParent /*=NULL*/)
: CDialog(AddLineConfigDialog::IDD, pParent)
{
}

AddLineConfigDialog::AddLineConfigDialog(UINT dlgID,CWnd* pParent /*=NULL*/)
: CDialog(dlgID, pParent)
{
}

BOOL AddLineConfigDialog::OnInitDialog()
{
	//和页面交互数据
	CDialog::OnInitDialog();

	this->InitConfigData();

	return TRUE;
}

void AddLineConfigDialog::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	DDX_Control(pDX, IDC_EDIT_NAME, m_LineName);
	DDX_Control(pDX, IDC_COMBO_KIND, m_LineKind);

	DDX_Control(pDX, IDC_COMBO_SHAPE, m_LineShape);
	DDX_Control(pDX, IDC_COMBO_UNIT, m_LineUnit);

	DDX_Control(pDX, IDC_STATIC_RADIUS, m_StaticRadius);
	DDX_Control(pDX, IDC_EDIT_RADIUS, m_LineRadius);

	DDX_Control(pDX, IDC_STATIC_LENGTH, m_StaticLength);
	DDX_Control(pDX, IDC_EDIT_SIZE_LENGTH, m_LineLength);

	DDX_Control(pDX, IDC_STATIC_WIDTH, m_StaticWidth);
	DDX_Control(pDX, IDC_EDIT_SIZE_WIDTH, m_LineWidth);

	DDX_Control(pDX, IDC_EDIT_SAFESIZE,m_LineSafeSize);
	DDX_Control(pDX, IDC_EDIT_WALLSIZE,m_LineWallSize);

	DDX_Control(pDX, IDC_EDIT_DESC,m_LineDesc);
}

BEGIN_MESSAGE_MAP(AddLineConfigDialog, CDialog)
	ON_BN_CLICKED(IDOK, &AddLineConfigDialog::OnBnClickedOk)
	ON_CBN_SELCHANGE(IDC_COMBO_SHAPE, &AddLineConfigDialog::OnCbnShapeChange)
END_MESSAGE_MAP()

BOOL AddLineConfigDialog::InitConfigData()
{
	//类型
	m_LineKind.AddString(GlobalData::LINE_CATEGORY_SHANGSHUI.c_str());
	m_LineKind.AddString(GlobalData::LINE_CATEGORY_XIASHUI.c_str());
	m_LineKind.AddString(GlobalData::LINE_CATEGORY_NUANQI.c_str());
	m_LineKind.AddString(GlobalData::LINE_CATEGORY_DIANLAN.c_str());
	m_LineKind.AddString(GlobalData::LINE_CATEGORY_YUSUI.c_str());
	m_LineKind.AddString(GlobalData::LINE_CATEGORY_TONGXIN.c_str());
	m_LineKind.SetCurSel(0);

	//断面形状
	m_LineShape.InsertString(0,GlobalData::LINE_SHAPE_CIRCLE.c_str());
	m_LineShape.InsertString(1,GlobalData::LINE_SHAPE_SQUARE.c_str());
	m_LineShape.SetCurSel(0);

	//单位
	m_LineUnit.AddString(GlobalData::LINE_UNIT_MM.c_str());
	m_LineUnit.AddString(GlobalData::LINE_UNIT_CM.c_str());
	m_LineUnit.AddString(GlobalData::LINE_UNIT_M.c_str());
	m_LineUnit.SetCurSel(0);

	//名称
	m_LineName.SetWindowTextW(L"");

	//大小
	m_LineRadius.SetWindowTextW(L"0");
	m_LineLength.SetWindowTextW(L"0");
	m_LineWidth.SetWindowTextW(L"0");

	//壁厚
	m_LineWallSize.SetWindowTextW(L"0");

	//安全范围
	m_LineSafeSize.SetWindowTextW(L"0");

	//描述
	m_LineDesc.SetWindowText(L"");

	if( m_OperType == OPER_UPDATE )
	{
		//填充数据
		FillUpdateData();
	}

	//动态控制控件
	ShowControlDynamic();

	return TRUE;
}

void AddLineConfigDialog::FillUpdateData()
{
	//更改控件的标题
	this->SetWindowTextW(L"管线更新");

	CWnd* pOKBtn = this->GetDlgItem(IDOK);
	if( pOKBtn )
	{
		//pOKBtn->SetWindowTextW(L"更新");
	}

	//Get the parent windows
	LineConfigDialog* lineConfig 
		= dynamic_cast<LineConfigDialog*>(this->m_pParentWnd);

	if( !lineConfig )
		return;

	LineCategoryItemData configData;
	
	//得到选择的数据
	lineConfig->GetSelectData(configData);

	//填充数据
	m_lineIndex = configData.mIndex;
	m_lineID = configData.mID;

	m_LineName.SetWindowTextW(configData.mName.c_str());
	m_LineKind.SelectString(0,configData.mKind.c_str());

	m_LineShape.SelectString(0,configData.mShape.c_str());
	m_LineUnit.SelectString(0,configData.mUnit.c_str());

	m_LineRadius.SetWindowTextW(configData.mRadius.c_str());
	m_LineLength.SetWindowTextW(configData.mLength.c_str());
	m_LineWidth.SetWindowTextW(configData.mWidth.c_str());

	m_LineWallSize.SetWindowTextW(configData.mWallSize.c_str());
	m_LineSafeSize.SetWindowTextW(configData.mSafeSize.c_str());

	m_LineDesc.SetWindowTextW(configData.mComment.c_str());
			
	UpdateData(FALSE);
}

AddLineConfigDialog::~AddLineConfigDialog()
{
}

void AddLineConfigDialog::OnBnClickedOk()
{
	//得到用户输入的数据
	UpdateData(TRUE);

	CString lineKind,lineName,lineShape,lineRadius,lineLength,lineWidth,lineUnit,lineSafeSize,lineWallSize,lineDesc;

	m_LineName.GetWindowTextW(lineName);
	m_LineKind.GetWindowTextW(lineKind);

	m_LineShape.GetWindowTextW(lineShape);
	m_LineUnit.GetWindowTextW(lineUnit);

	m_LineRadius.GetWindowTextW(lineRadius);
	m_LineLength.GetWindowTextW(lineLength);
	m_LineWidth.GetWindowTextW(lineWidth);

	m_LineWallSize.GetWindowTextW(lineWallSize);
	m_LineSafeSize.GetWindowTextW(lineSafeSize);

	m_LineDesc.GetWindowTextW(lineDesc);

	//准备配置数据结构体
	LineCategoryItemData categoryData;

	categoryData.mIndex = m_lineIndex;
	categoryData.mID = m_lineID;

	categoryData.mCategory = GlobalData::KIND_LINE;

	categoryData.mName = lineName;
	categoryData.mKind = lineKind;

	categoryData.mUnit = lineUnit;
	categoryData.mShape = lineShape;

	categoryData.mRadius = lineRadius;
	categoryData.mLength = lineLength;
	categoryData.mWidth = lineWidth;

	categoryData.mWallSize = lineWallSize;
	categoryData.mSafeSize = lineSafeSize;
	categoryData.mComment = lineDesc;

	try
	{
		//调用父窗口进行操作
		LineConfigDialog* lineConfig 
				= dynamic_cast<LineConfigDialog*>(this->m_pParentWnd);

		if( lineConfig == NULL )
		{
			acutPrintf(L"!!不能找到配置主窗口.\n");
		}

		//增加或更新管线配置
		if( m_OperType == OPER_ADD )
		{
			acutPrintf(L"增加管线类型.\n");

			lineConfig->CheckValid(categoryData,true);
			lineConfig->InsertLine(categoryData,false);
		}
		else if( m_OperType == OPER_UPDATE )
		{
			acutPrintf(L"更新管线类型.\n");

			lineConfig->CheckValid(categoryData,false);
			lineConfig->UpdateLine(categoryData);
		}

		CDialog::OnOK();
	}
	catch(ErrorException& e)
	{
		MessageBoxW(e.errMsg.c_str(), GlobalData::ERROR_DIALOG_CAPTION.c_str(), MB_OKCANCEL);
	}
}

afx_msg void AddLineConfigDialog::OnCbnShapeChange()
{
	ShowControlDynamic();
}

void AddLineConfigDialog::ShowControlDynamic()
{
	if( m_LineShape.GetCurSel() == 0 )
	{
		m_StaticRadius.ShowWindow( SW_SHOW );
		m_LineRadius.ShowWindow( SW_SHOW );

		m_StaticLength.ShowWindow( SW_HIDE ) ;
		m_LineWidth.ShowWindow( SW_HIDE ) ;

		m_StaticWidth.ShowWindow( SW_HIDE ) ;
		m_LineLength.ShowWindow( SW_HIDE ) ;
	}
	else if( m_LineShape.GetCurSel() == 1 )
	{
		m_StaticRadius.ShowWindow( SW_HIDE );
		m_LineRadius.ShowWindow( SW_HIDE );

		m_StaticLength.ShowWindow( SW_SHOW ) ;
		m_LineWidth.ShowWindow( SW_SHOW ) ;

		m_StaticWidth.ShowWindow( SW_SHOW ) ;
		m_LineLength.ShowWindow( SW_SHOW ) ;
	}
}

/////////////////////////////////////////////////////////////////////////////////////////////////

// AddLineDialog dialog

IMPLEMENT_DYNAMIC(AddLineDialog, CDialog)

AddLineDialog::AddLineDialog(CWnd* pParent /*=NULL*/)
	: CDialog(AddLineDialog::IDD, pParent)
{
}

BOOL AddLineDialog::OnInitDialog()
{
	//和页面交互数据
	CDialog::OnInitDialog();

	this->InitLineList();

	this->InitLineDetailHeader();

	this->InitLineDetailData();

	return TRUE;
}

BOOL AddLineDialog::InitLineList()
{
	/*
	acutPrintf(L"初始化管线实例数据.\n");

	HTREEITEM hKindItem,hCatogreyItem,kLineItem;
	
	//在根结点上添加"管线"
	hKindItem = m_LinesTree.InsertItem(L"管线",TVI_ROOT);

	//在“管线”下面插入分类数据
	hCatogreyItem = m_LinesTree.InsertItem(L"水管",hKindItem);

	//插入具体管线
	kLineItem = m_LinesTree.InsertItem(L"水管#1",hCatogreyItem);
	kLineItem = m_LinesTree.InsertItem(L"水管#2",hCatogreyItem,kLineItem);

	//插入其他种类
	hCatogreyItem = m_LinesTree.InsertItem(L"暖气",hKindItem,hCatogreyItem);//在Parent1上添加一个子结点，排在Child1_1后面
	hCatogreyItem = m_LinesTree.InsertItem(L"电线",hKindItem,hCatogreyItem);

	hKindItem = m_LinesTree.InsertItem(L"阻隔体",TVI_ROOT,hKindItem);   

	//在“阻隔体”下面插入分类数据
	hCatogreyItem = m_LinesTree.InsertItem(L"巷道",hKindItem);

	//插入具体管线
	kLineItem = m_LinesTree.InsertItem(L"巷道#1",hCatogreyItem);
	kLineItem = m_LinesTree.InsertItem(L"巷道#2",hCatogreyItem,kLineItem);
	*/

	return TRUE;
}

BOOL AddLineDialog::InitLineDetailHeader()
{
	acutPrintf(L"初始化管线数据表头.\n");

	/*
	int index = 0;

	LVCOLUMN lvColumn;

	lvColumn.mask = LVCF_FMT | LVCF_TEXT | LVCF_WIDTH;
	lvColumn.fmt = LVCFMT_CENTER;
	lvColumn.cx = 40;
	lvColumn.pszText = L"编号";
	this->m_LineDetailList.InsertColumn(index++, &lvColumn);

	lvColumn.mask = LVCF_FMT | LVCF_TEXT | LVCF_WIDTH;
	lvColumn.fmt = LVCFMT_LEFT;
	lvColumn.cx = 60;
	lvColumn.pszText = L"X坐标";
	m_LineDetailList.InsertColumn(index++, &lvColumn);

	lvColumn.mask = LVCF_FMT | LVCF_TEXT | LVCF_WIDTH;
	lvColumn.fmt = LVCFMT_LEFT;
	lvColumn.cx = 60;
	lvColumn.pszText = L"Y坐标";
	m_LineDetailList.InsertColumn(index++, &lvColumn);

	lvColumn.mask = LVCF_FMT | LVCF_TEXT | LVCF_WIDTH;
	lvColumn.fmt = LVCFMT_LEFT;
	lvColumn.cx = 60;
	lvColumn.pszText = L"高程";
	m_LineDetailList.InsertColumn(index++, &lvColumn);

	m_LineDetailList.SetExtendedStyle(LVS_EX_FULLROWSELECT | LVS_EX_GRIDLINES );
	*/

	return TRUE;
}

BOOL AddLineDialog::InitLineDetailData()
{
	acutPrintf(L"初始化管线坐标信息.\n");

	/*
	const int MAX_ITEM = 10;

	for( int i = 0; i < MAX_ITEM; i++)
	{
		LVITEM lvItem;
		int nItem;
	
		lvItem.mask = LVIF_TEXT;
		lvItem.iItem = i;
		lvItem.iSubItem = 0;
		
		CString strNumber;
		strNumber.Format(L"%d",i);

		lvItem.pszText = strNumber.GetBuffer();
		nItem = this->m_LineDetailList.InsertItem(&lvItem);

		m_LineDetailList.SetItemText(nItem, 1, L"30.5");
		m_LineDetailList.SetItemText(nItem, 2, L"29.6");
		m_LineDetailList.SetItemText(nItem, 3, L"17.3");
	}

	UpdateData(FALSE);
	*/

	return TRUE;
}

AddLineDialog::~AddLineDialog()
{
}

void AddLineDialog::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	//DDX_Control(pDX, IDC_TREE_LINES, m_LinesTree);
	//DDX_Control(pDX, IDC_LIST_LINE_DETAIL, m_LineDetailList);
}


BEGIN_MESSAGE_MAP(AddLineDialog, CDialog)
END_MESSAGE_MAP()

// AddLineDialog message handlers

} // end of config

} // end of assistant

} // end of guch

} // end of com
