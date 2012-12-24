// LineConfigDialog.cpp : implementation file
//

#include "stdafx.h"
#include "LineConfigDialog.h"
#include "afxdialogex.h"

#include "resource.h"

#include "AddLineDialog.h"

#include <LineConfigDataManager.h>

namespace com
{

namespace guch
{

namespace assistent
{

namespace config
{

// LineConfigDialog dialog

IMPLEMENT_DYNAMIC(LineConfigDialog, CAcUiDialog)

LineConfigDialog::LineConfigDialog(CWnd* pParent /*=NULL*/)
	: CAcUiDialog(LineConfigDialog::IDD, pParent)
{
	
}

LineConfigDialog::~LineConfigDialog()
{
}

void LineConfigDialog::DoDataExchange(CDataExchange* pDX)
{
	CAcUiDialog::DoDataExchange(pDX);
	DDX_Control(pDX, IDC_LIST_LINE_CONFIG, m_lineConfig);
}

BOOL LineConfigDialog::OnInitDialog()
{
	//和页面交互数据
	CAcUiDialog::OnInitDialog();

	this->InitLineHeader();

	this->InitLineData();

	return TRUE; // return TRUE unless you set the focus to a control
}

BOOL LineConfigDialog::InitLineHeader()
{
	acutPrintf(L"初始化管线配置数据.\n");
	int index = 0;

	LVCOLUMN lvColumn;

	lvColumn.mask = LVCF_FMT | LVCF_TEXT | LVCF_WIDTH;
	lvColumn.fmt = LVCFMT_CENTER;
	lvColumn.cx = 40;
	lvColumn.pszText = L"序号";
	m_lineConfig.InsertColumn(index++, &lvColumn);

	lvColumn.mask = LVCF_FMT | LVCF_TEXT | LVCF_WIDTH;
	lvColumn.fmt = LVCFMT_LEFT;
	lvColumn.cx = 40;
	lvColumn.pszText = L"名称";
	m_lineConfig.InsertColumn(index++, &lvColumn);

	lvColumn.mask = LVCF_FMT | LVCF_TEXT | LVCF_WIDTH;
	lvColumn.fmt = LVCFMT_CENTER;
	lvColumn.cx = 40;
	lvColumn.pszText = L"类型";
	m_lineConfig.InsertColumn(index++, &lvColumn);

	lvColumn.mask = LVCF_FMT | LVCF_TEXT | LVCF_WIDTH;
	lvColumn.fmt = LVCFMT_LEFT;
	lvColumn.cx = 60;
	lvColumn.pszText = L"断面形状";
	m_lineConfig.InsertColumn(index++, &lvColumn);

	lvColumn.mask = LVCF_FMT | LVCF_TEXT | LVCF_WIDTH;
	lvColumn.fmt = LVCFMT_LEFT;
	lvColumn.cx = 60;
	lvColumn.pszText = L"断面大小";
	m_lineConfig.InsertColumn(index++, &lvColumn);

	lvColumn.mask = LVCF_FMT | LVCF_TEXT | LVCF_WIDTH;
	lvColumn.fmt = LVCFMT_LEFT;
	lvColumn.cx = 80;
	lvColumn.pszText = L"有效范围";
	m_lineConfig.InsertColumn(index++, &lvColumn);

	lvColumn.mask = LVCF_FMT | LVCF_TEXT | LVCF_WIDTH;
	lvColumn.fmt = LVCFMT_LEFT;
	lvColumn.cx = 40;
	lvColumn.pszText = L"单位";
	m_lineConfig.InsertColumn(index++, &lvColumn);

	lvColumn.mask = LVCF_FMT | LVCF_TEXT | LVCF_WIDTH;
	lvColumn.fmt = LVCFMT_LEFT;
	lvColumn.cx = 80;
	lvColumn.pszText = L"描述";
	m_lineConfig.InsertColumn(index++, &lvColumn);

	m_lineConfig.SetExtendedStyle(LVS_EX_FULLROWSELECT | LVS_EX_GRIDLINES );

	return TRUE;
}

/**
 *
 **/
BOOL LineConfigDialog::InsertLine(const int index,
	const wstring& rID,
	const wstring& rName,
	const wstring& rKind,
	const wstring& rCategory,
	const wstring& rShape,
	const wstring& rSize,
	const wstring& rEffectSize,
	const wstring& rUnit,
	const wstring& rComment)
{
	LVITEM lvItem;
	int nItem;
	
	lvItem.mask = LVIF_TEXT;
	lvItem.iItem = index;
	lvItem.iSubItem = 0;
	lvItem.pszText = const_cast<wchar_t*>(rID.c_str());
	nItem = m_lineConfig.InsertItem(&lvItem);

	m_lineConfig.SetItemText(nItem, 1, rName.c_str());

	m_lineConfig.SetItemText(nItem, 2, rCategory.c_str());
	m_lineConfig.SetItemText(nItem, 3, rShape.c_str());

	m_lineConfig.SetItemText(nItem, 4, rSize.c_str());
	m_lineConfig.SetItemText(nItem, 6, rEffectSize.c_str());
	m_lineConfig.SetItemText(nItem, 5, rUnit.c_str());

	m_lineConfig.SetItemText(nItem, 7, rComment.c_str());

	return TRUE;
}

BOOL LineConfigDialog::InitLineData()
{
	const LineCategoryVecotr lineCategoryData
		 = LineConfigDataManager::Instance()->GetData();

	typedef vector<LineCategoryItemData*>::const_iterator DataIterator;
	
	int index = 0;
	for( DataIterator iter = lineCategoryData->begin(); 
			iter != lineCategoryData->end(); 
			iter++,index++)
	{
		LineCategoryItemData* data = *iter;

		if( data )
		{
			InsertLine(index,
					data->mID,
					data->mName,
					data->mKind,
					data->mCategory,
					data->mShape,
					data->mSize,
					data->mEffectSize,
					data->mUnit,
					data->mComment);
		}
	}

	UpdateData(FALSE);

	return TRUE;
}

BEGIN_MESSAGE_MAP(LineConfigDialog, CAcUiDialog)
	ON_BN_CLICKED(IDC_BUTTON_ADD, OnBnClickedButtonAdd)
END_MESSAGE_MAP()

void LineConfigDialog::OnBnClickedButtonAdd()
{
	// TODO: Add your control notification handler code here
	AddLineDialog dlg(this);
	INT_PTR nReturnValue = dlg.DoModal();
}


} // end of config

} // end of assistant

} // end of guch

} // end of com

// LineConfigDialog message handlers
