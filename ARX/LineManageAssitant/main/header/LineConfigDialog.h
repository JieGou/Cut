#pragma once
#include "afxcmn.h"

#include <resource.h>

#include <dbsymtb.h>
#include <dbapserv.h>
#include <adslib.h>
#include <adui.h>
#include <acui.h>

#include <AddLineDialog.h>
#include <LMAException.h>

// LineConfigDialog dialog

using namespace std;

namespace com
{

namespace guch
{

namespace assistent
{

namespace config
{

class LineConfigDialog : public CPropertyPage
{
	DECLARE_DYNAMIC(LineConfigDialog)

public:
	LineConfigDialog( CWnd* pParentWnd = NULL );

	virtual ~LineConfigDialog();

// Dialog Data
	enum { IDD = IDD_DIALOG_LINE_CONFIG };

	afx_msg void OnBnClickedButtonAdd();
	afx_msg void OnBnClickedButtonMod();
	afx_msg void OnBnClickedButtonDel();
	afx_msg void OnBnClickedButtonOK();

	BOOL InsertLine(LineCategoryItemData& itemData, BOOL initialize = false);
	BOOL UpdateLine(const LineCategoryItemData& itemData);

	BOOL GetSelectData(LineCategoryItemData& configData );
	void CheckValid( const LineCategoryItemData& item, BOOL bNew );

protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	virtual BOOL OnInitDialog();

	BOOL InitLineHeader();
	BOOL InitLineData();

	virtual BOOL OnNotify(WPARAM wParam, LPARAM lParam, LRESULT* pResult);

	DECLARE_MESSAGE_MAP()

	BOOL LineConfigDialog::GetItemData( int item, LineCategoryItemData& configData);

private:
	
	void UpdateUILineData(const LineCategoryItemData& itemData);

public:

	CAcUiListCtrl m_lineConfig;
};

} // end of config

} // end of assistant

} // end of guch

} // end of com