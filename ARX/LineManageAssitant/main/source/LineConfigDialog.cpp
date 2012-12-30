// LineConfigDialog.cpp : implementation file
//

#include "stdafx.h"
#include "afxdialogex.h"

#include "resource.h"

#include <LineConfigDialog.h>

#include <LineConfigDataManager.h>
#include <GlobalDataConfig.h>

using namespace com::guch::assistent::data;

namespace com
{

namespace guch
{

namespace assistent
{

namespace config
{

// LineConfigDialog dialog

IMPLEMENT_DYNAMIC(LineConfigDialog, CPropertyPage)

LineConfigDialog::LineConfigDialog(CWnd* pParent /*=NULL*/)
: CPropertyPage(LineConfigDialog::IDD)
{
	
}

LineConfigDialog::~LineConfigDialog()
{
}

void LineConfigDialog::DoDataExchange(CDataExchange* pDX)
{
	CPropertyPage::DoDataExchange(pDX);
	DDX_Control(pDX, IDC_LIST_LINE_CONFIG, m_lineConfig);
}

BOOL LineConfigDialog::OnInitDialog()
{
	//和页面交互数据
	CPropertyPage::OnInitDialog();

	this->InitLineHeader();

	this->InitLineData();

	return TRUE; // return TRUE unless you set the focus to a control
}

BOOL LineConfigDialog::OnNotify(WPARAM wParam, LPARAM lParam, LRESULT* pResult)
{
	//acutPrintf(L"收到Notify数据WPARAM[%ld] LPARAM[%ld].\n",wParam,lParam);

	return CPropertyPage::OnNotify(wParam,lParam,pResult);
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
	//m_lineConfig.InsertColumn(index++, &lvColumn);

	lvColumn.mask = LVCF_FMT | LVCF_TEXT | LVCF_WIDTH;
	lvColumn.fmt = LVCFMT_LEFT;
	lvColumn.cx = 100;
	lvColumn.pszText = L"名称";
	m_lineConfig.InsertColumn(index++, &lvColumn);

	lvColumn.mask = LVCF_FMT | LVCF_TEXT | LVCF_WIDTH;
	lvColumn.fmt = LVCFMT_CENTER;
	lvColumn.cx = 80;
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
	lvColumn.cx = 60;
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

	int columIndex = 0;
	
	lvItem.mask = LVIF_TEXT;
	lvItem.iItem = index;
	lvItem.iSubItem = columIndex++;
	lvItem.pszText = const_cast<wchar_t*>(rName.c_str());
	nItem = m_lineConfig.InsertItem(&lvItem);

	m_lineConfig.SetItemData(index,index);

	//m_lineConfig.SetItemText(nItem, columIndex++, rName.c_str());

	m_lineConfig.SetItemText(nItem, columIndex++, rCategory.c_str());
	m_lineConfig.SetItemText(nItem, columIndex++, rShape.c_str());

	m_lineConfig.SetItemText(nItem, columIndex++, rSize.c_str());
	m_lineConfig.SetItemText(nItem, columIndex++, rEffectSize.c_str());
	m_lineConfig.SetItemText(nItem, columIndex++, rUnit.c_str());

	m_lineConfig.SetItemText(nItem, columIndex++, rComment.c_str());

	UpdateData(FALSE);

	return TRUE;
}

/**
 *
 **/
BOOL LineConfigDialog::UpdateLine(const int index,
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
	int nItem = index;

	int columIndex = 0;
	m_lineConfig.SetItemText(nItem,columIndex++, rName.c_str());

	m_lineConfig.SetItemText(nItem, columIndex++, rCategory.c_str());
	m_lineConfig.SetItemText(nItem, columIndex++, rShape.c_str());

	m_lineConfig.SetItemText(nItem, columIndex++, rSize.c_str());
	m_lineConfig.SetItemText(nItem, columIndex++, rEffectSize.c_str());
	m_lineConfig.SetItemText(nItem, columIndex++, rUnit.c_str());

	m_lineConfig.SetItemText(nItem, columIndex++, rComment.c_str());

	UpdateData(FALSE);

	return TRUE;
}

/**
 *
 **/
int LineConfigDialog::FindLine( const wstring& rName )
{
	int index = m_lineConfig.GetSelectedColumn();
	return index;
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

BEGIN_MESSAGE_MAP(LineConfigDialog, CPropertyPage)
END_MESSAGE_MAP()

void LineConfigDialog::OnBnClickedButtonAdd()
{
	// TODO: Add your control notification handler code here
	AddLineConfigDialog dlg(this);
	
	//设置操作类型
	dlg.SetOperType(AddLineConfigDialog::OPER_ADD);

	INT_PTR nReturnValue = dlg.DoModal();
}

void LineConfigDialog::OnBnClickedButtonMod()
{
	// TODO: Add your control notification handler code here
	AddLineConfigDialog dlg(this);
	
	//设置操作类型
	dlg.SetOperType(AddLineConfigDialog::OPER_UPDATE);

	dlg.DoModal();
}

BOOL LineConfigDialog::GetSelectData( LineConfigData& configData )
{
	//选择的行
	int item = m_lineConfig.GetSelectionMark();
	if( item == -1 )
		return FALSE;

	//得到要填充的数据
	configData.mIndex = item;

	//得到行的ID
	DWORD_PTR itemData = m_lineConfig.GetItemData(item);
	configData.mID = (UINT)itemData;

	//得到每一列的数据
	int columIndex = 0;

	LVITEM lvItem;
	lvItem.mask = LVIF_TEXT;
	lvItem.iItem = item;
	lvItem.iSubItem = columIndex++;

	TCHAR szBuffer[255];
	DWORD cchBuf(GlobalData::ITEM_TEXT_MAX_LENGTH);

	lvItem.pszText = szBuffer;
	lvItem.cchTextMax = cchBuf;

	if( m_lineConfig.GetItem(&lvItem) )
		configData.mName = lvItem.pszText;

	configData.mKind = GlobalData::KIND_LINE;

	lvItem.iSubItem = columIndex++;
	if( m_lineConfig.GetItem(&lvItem) )
		configData.mCategory = lvItem.pszText;

	lvItem.iSubItem = columIndex++;
	if( m_lineConfig.GetItem(&lvItem) )
		configData.mShape = lvItem.pszText;

	lvItem.iSubItem = columIndex++;
	if( m_lineConfig.GetItem(&lvItem) )
		configData.mSize = lvItem.pszText;

	lvItem.iSubItem = columIndex++;
	if( m_lineConfig.GetItem(&lvItem) )
		configData.mSafeSize = lvItem.pszText;

	lvItem.iSubItem = columIndex++;
	if( m_lineConfig.GetItem(&lvItem) )
		configData.mUnit = lvItem.pszText;

	lvItem.iSubItem = columIndex++;
	if( m_lineConfig.GetItem(&lvItem) )
		configData.mDesc = lvItem.pszText;

	return TRUE;
}

void LineConfigDialog::OnBnClickedButtonDel()
{
	// TODO: Add your control notification handler code here

	//选择的行
	int item = m_lineConfig.GetSelectionMark();
	if( item == -1 )
		return;

	LVITEM lvItem;
	lvItem.mask = LVIF_TEXT;
	lvItem.iItem = item;
	lvItem.iSubItem = 0;

	TCHAR szBuffer[255];
	DWORD cchBuf(GlobalData::ITEM_TEXT_MAX_LENGTH);

	lvItem.pszText = szBuffer;
	lvItem.cchTextMax = cchBuf;

	if( m_lineConfig.GetItem(&lvItem) )
	{
		// Initializes the variables to pass to the MessageBox::Show method.
		CString message;
		message.Format(L"确实要删除管线[%s]吗?",lvItem.pszText);

		LPCTSTR caption = L"删除管线";

		// Displays the MessageBox.
		int result = MessageBoxW(message, caption, MB_OKCANCEL);
		if ( result == IDOK )
		{
			// Closes the parent form. 
			m_lineConfig.DeleteItem(item);
		}
	}
}

} // end of config

} // end of assistant

} // end of guch

} // end of com

// LineConfigDialog message handlers
